using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Xml;

namespace Hishop.TransferManager
{
	public static class TransferHelper
	{
		public static Dictionary<string, string> GetExportAdapters(Target source, string exportToName)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			DataRow[] exporterList = TransferContainer.Instance().GetExporterList(source.Name, exportToName);
			if (exporterList == null || exporterList.Length == 0)
			{
				return dictionary;
			}
			string value = null;
			int num = 0;
			do
			{
				Version v = new Version(exporterList[num]["sourceVersion"].ToString());
				if (v <= source.Version)
				{
					value = exporterList[num]["sourceVersion"].ToString();
				}
				num++;
			}
			while (string.IsNullOrEmpty(value) && num < exporterList.Length);
			if (!string.IsNullOrEmpty(value))
			{
				DataRow[] array = exporterList;
				for (int i = 0; i < array.Length; i++)
				{
					DataRow dataRow = array[i];
					string text = dataRow["sourceVersion"].ToString();
					if (text.Equals(value))
					{
						dictionary.Add(dataRow["fullName"].ToString(), dataRow["exportToName"].ToString() + dataRow["exportToVersion"].ToString());
					}
				}
			}
			return dictionary;
		}

		public static Dictionary<string, string> GetImportAdapters(Target importTo, string sourceName)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			DataRow[] importerList = TransferContainer.Instance().GetImporterList(sourceName, importTo.Name);
			if (importerList == null || importerList.Length == 0)
			{
				return dictionary;
			}
			string value = null;
			int num = 0;
			do
			{
				Version v = new Version(importerList[num]["importToVersion"].ToString());
				if (v <= importTo.Version)
				{
					value = importerList[num]["importToVersion"].ToString();
				}
				num++;
			}
			while (string.IsNullOrEmpty(value) && num < importerList.Length);
			if (!string.IsNullOrEmpty(value))
			{
				DataRow[] array = importerList;
				for (int i = 0; i < array.Length; i++)
				{
					DataRow dataRow = array[i];
					string text = dataRow["importToVersion"].ToString();
					if (text.Equals(value))
					{
						dictionary.Add(dataRow["fullName"].ToString(), dataRow["sourceName"].ToString() + dataRow["sourceVersion"].ToString());
					}
				}
			}
			return dictionary;
		}

		public static ExportAdapter GetExporter(string fullName, params object[] exportParams)
		{
			if (string.IsNullOrEmpty(fullName))
			{
				return null;
			}
			Type exporter = TransferContainer.Instance().GetExporter(fullName);
			if (exporter == null)
			{
				return null;
			}
			if (exportParams != null && exportParams.Length > 0)
			{
				return Activator.CreateInstance(exporter, exportParams) as ExportAdapter;
			}
			return Activator.CreateInstance(exporter) as ExportAdapter;
		}

		public static ImportAdapter GetImporter(string fullName, params object[] exportParams)
		{
			if (string.IsNullOrEmpty(fullName))
			{
				return null;
			}
			Type importer = TransferContainer.Instance().GetImporter(fullName);
			if (importer == null)
			{
				return null;
			}
			if (exportParams != null && exportParams.Length > 0)
			{
				return Activator.CreateInstance(importer, exportParams) as ImportAdapter;
			}
			return Activator.CreateInstance(importer) as ImportAdapter;
		}

		public static byte[] ConvertToBytes(string imageUrl)
		{
			byte[] result = new byte[0];
			if (string.IsNullOrEmpty(imageUrl))
			{
				return result;
			}
			string path = HttpContext.Current.Request.MapPath("~" + imageUrl);
			if (!File.Exists(path))
			{
				return result;
			}
			try
			{
				result = File.ReadAllBytes(path);
			}
			catch
			{
			}
			return result;
		}

		public static void WriteImageElement(XmlWriter writer, string nodeName, bool includeImages, string imageUrl, DirectoryInfo destDir)
		{
			writer.WriteStartElement(nodeName);
			if (!string.IsNullOrEmpty(imageUrl))
			{
				if (includeImages)
				{
					string text = HttpContext.Current.Request.MapPath("~" + imageUrl);
					string fileName = Path.GetFileName(text);
					writer.WriteString(fileName);
					if (File.Exists(text))
					{
						File.Copy(text, Path.Combine(destDir.FullName, fileName), true);
					}
				}
				else
				{
					writer.WriteString(imageUrl);
				}
			}
			writer.WriteEndElement();
		}

		public static void WriteImageElement(XmlWriter writer, string nodeName, bool includeImages, string imageUrl)
		{
			writer.WriteStartElement(nodeName);
			if (includeImages)
			{
				byte[] array = TransferHelper.ConvertToBytes(imageUrl);
				writer.WriteBase64(array, 0, array.Length);
			}
			else
			{
				writer.WriteString(imageUrl);
			}
			writer.WriteEndElement();
		}

		public static void WriteCDataElement(XmlWriter writer, string nodeName, string text)
		{
			writer.WriteStartElement(nodeName);
			writer.WriteCData(text);
			writer.WriteEndElement();
		}
	}
}
