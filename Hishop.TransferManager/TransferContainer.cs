using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Caching;

namespace Hishop.TransferManager
{
	internal class TransferContainer
	{
		private const string CacheKey = "Hishop_TransferIndexes";

		private static readonly object LockHelper = new object();

		private static volatile TransferContainer _instance = null;

		private static volatile Cache TransferCache = HttpRuntime.Cache;

		private TransferContainer()
		{
			TransferContainer.TransferCache.Remove("Hishop_TransferIndexes");
		}

		internal static TransferContainer Instance()
		{
			if (TransferContainer._instance == null)
			{
				lock (TransferContainer.LockHelper)
				{
					if (TransferContainer._instance == null)
					{
						TransferContainer._instance = new TransferContainer();
					}
				}
			}
			TransferContainer.Init();
			return TransferContainer._instance;
		}

		internal Type GetExporter(string fullName)
		{
			return TransferContainer.GetPlugin(fullName, "Exporters");
		}

		internal Type GetImporter(string fullName)
		{
			return TransferContainer.GetPlugin(fullName, "Importers");
		}

		internal DataRow[] GetExporterList(string sourceName, string exportToName)
		{
			DataSet dataSet = TransferContainer.TransferCache.Get("Hishop_TransferIndexes") as DataSet;
			return dataSet.Tables["Exporters"].Select(string.Format("sourceName='{0}' and exportToName='{1}'", sourceName, exportToName), "sourceVersion desc");
		}

		internal DataRow[] GetImporterList(string sourceName, string importToName)
		{
			DataSet dataSet = TransferContainer.TransferCache.Get("Hishop_TransferIndexes") as DataSet;
			return dataSet.Tables["Importers"].Select(string.Format("sourceName='{0}' and importToName='{1}'", sourceName, importToName), "importToVersion desc");
		}

		private static void Init()
		{
			if (TransferContainer.TransferCache.Get("Hishop_TransferIndexes") != null)
			{
				return;
			}
			string text = HttpContext.Current.Request.MapPath("~/plugins/transfer");
			DataSet dataSet = new DataSet();
			DataTable dataTable = new DataTable("Exporters");
			DataTable dataTable2 = new DataTable("Importers");
			TransferContainer.InitTable(dataTable);
			TransferContainer.InitTable(dataTable2);
			dataTable.Columns.Add(new DataColumn("exportToName"));
			dataTable.Columns.Add(new DataColumn("exportToVersion"));
			dataTable2.Columns.Add(new DataColumn("importToName"));
			dataTable2.Columns.Add(new DataColumn("importToVersion"));
			dataSet.Tables.Add(dataTable);
			dataSet.Tables.Add(dataTable2);
			TransferContainer.BuildIndex(text, dataTable, dataTable2);
			TransferContainer.TransferCache.Insert("Hishop_TransferIndexes", dataSet, new CacheDependency(text));
		}

		private static void InitTable(DataTable table)
		{
			table.Columns.Add(new DataColumn("fullName")
			{
				Unique = true
			});
			table.Columns.Add(new DataColumn("filePath"));
			table.Columns.Add(new DataColumn("sourceName"));
			table.Columns.Add(new DataColumn("sourceVersion"));
			table.PrimaryKey = new DataColumn[]
			{
				table.Columns["fullName"]
			};
		}

		private static void BuildIndex(string pluginPath, DataTable dtExporters, DataTable dtImporters)
		{
			if (!Directory.Exists(pluginPath))
			{
				return;
			}
			string[] files = Directory.GetFiles(pluginPath, "*.dll", SearchOption.AllDirectories);
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string filename = array[i];
				Assembly assembly = Assembly.Load(TransferContainer.LoadPluginFile(filename));
				Type[] exportedTypes = assembly.GetExportedTypes();
				for (int j = 0; j < exportedTypes.Length; j++)
				{
					Type type = exportedTypes[j];
					if (type.BaseType != null)
					{
						if (type.BaseType.Name == "ExportAdapter")
						{
							TransferContainer.AddToExportIndex(type, filename, dtExporters);
						}
						else if (type.BaseType.Name == "ImportAdapter")
						{
							TransferContainer.AddToImportIndex(type, filename, dtImporters);
						}
					}
				}
			}
		}

		private static byte[] LoadPluginFile(string filename)
		{
			byte[] array;
			using (FileStream fileStream = new FileStream(filename, FileMode.Open))
			{
				array = new byte[(int)fileStream.Length];
				fileStream.Read(array, 0, array.Length);
			}
			return array;
		}

		private static void AddToExportIndex(Type t, string filename, DataTable dtExporters)
		{
			ExportAdapter exportAdapter = Activator.CreateInstance(t) as ExportAdapter;
			DataRow dataRow = dtExporters.NewRow();
			dataRow["fullName"] = t.FullName.ToLower();
			dataRow["filePath"] = filename;
			dataRow["sourceName"] = exportAdapter.Source.Name;
			dataRow["sourceVersion"] = exportAdapter.Source.Version.ToString();
			dataRow["exportToName"] = exportAdapter.ExportTo.Name;
			dataRow["exportToVersion"] = exportAdapter.ExportTo.Version.ToString();
			dtExporters.Rows.Add(dataRow);
		}

		private static void AddToImportIndex(Type t, string filename, DataTable dtImporters)
		{
			ImportAdapter importAdapter = Activator.CreateInstance(t) as ImportAdapter;
			DataRow dataRow = dtImporters.NewRow();
			dataRow["fullName"] = t.FullName.ToLower();
			dataRow["filePath"] = filename;
			dataRow["sourceName"] = importAdapter.Source.Name;
			dataRow["sourceVersion"] = importAdapter.Source.Version.ToString();
			dataRow["importToName"] = importAdapter.ImportTo.Name;
			dataRow["importToVersion"] = importAdapter.ImportTo.Version.ToString();
			dtImporters.Rows.Add(dataRow);
		}

		private static Type GetPlugin(string fullName, string tableName)
		{
			DataSet dataSet = TransferContainer.TransferCache.Get("Hishop_TransferIndexes") as DataSet;
			DataRow[] array = dataSet.Tables[tableName].Select("fullName='" + fullName.ToLower() + "'");
			if (array.Length == 0 || !File.Exists(array[0]["filePath"].ToString()))
			{
				return null;
			}
			Assembly assembly = Assembly.Load(TransferContainer.LoadPluginFile(array[0]["filePath"].ToString()));
			return assembly.GetType(fullName, false, true);
		}
	}
}
