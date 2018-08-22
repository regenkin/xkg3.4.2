using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.Core
{
	public static class DataHelper
	{
		public static string GetSafeDateTimeFormat(DateTime date)
		{
			return date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.SortableDateTimePattern, CultureInfo.InvariantCulture);
		}

		public static string DateComparerString(int dateComparer)
		{
			string result;
			switch (dateComparer)
			{
			case -1:
				result = "<";
				break;
			case 0:
				result = "=";
				break;
			case 1:
				result = ">";
				break;
			default:
				result = "=";
				break;
			}
			return result;
		}

		public static string CleanSearchString(string searchString)
		{
			string result;
			if (string.IsNullOrEmpty(searchString))
			{
				result = null;
			}
			else
			{
				searchString = searchString.Replace("*", "%");
				searchString = Globals.StripHtmlXmlTags(searchString);
				searchString = Regex.Replace(searchString, "--|;|'|\"", " ", RegexOptions.Multiline | RegexOptions.Compiled);
				searchString = Regex.Replace(searchString, " {1,}", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
				result = searchString;
			}
			return result;
		}

		public static System.Data.DataTable ConverDataReaderToDataTable(System.Data.IDataReader reader)
		{
			System.Data.DataTable result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				System.Data.DataTable dataTable = new System.Data.DataTable();
				dataTable.Locale = CultureInfo.InvariantCulture;
				int fieldCount = reader.FieldCount;
				for (int i = 0; i < fieldCount; i++)
				{
					dataTable.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
				}
				dataTable.BeginLoadData();
				object[] values = new object[fieldCount];
				while (reader.Read())
				{
					reader.GetValues(values);
					dataTable.LoadDataRow(values, true);
				}
				dataTable.EndLoadData();
				result = dataTable;
			}
			return result;
		}

		public static DbQueryResult PagingByRownumber(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields)
		{
			return DataHelper.PagingByRownumber(pageIndex, pageSize, sortBy, sortOrder, isCount, table, pk, filter, selectFields, 0);
		}

		public static DbQueryResult PagingByRownumber(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields, int partitionSize)
		{
			DbQueryResult result;
			if (string.IsNullOrEmpty(table))
			{
				result = null;
			}
			else if (string.IsNullOrEmpty(sortBy) && string.IsNullOrEmpty(pk))
			{
				result = null;
			}
			else
			{
				if (string.IsNullOrEmpty(selectFields))
				{
					selectFields = "*";
				}
				string query = DataHelper.BuildRownumberQuery(sortBy, sortOrder, isCount, table, pk, filter, selectFields, partitionSize);
				int num = (pageIndex - 1) * pageSize + 1;
				int num2 = num + pageSize - 1;
				DbQueryResult dbQueryResult = new DbQueryResult();
				Database database = DatabaseFactory.CreateDatabase();
				System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
				database.AddInParameter(sqlStringCommand, "StartNumber", System.Data.DbType.Int32, num);
				database.AddInParameter(sqlStringCommand, "EndNumber", System.Data.DbType.Int32, num2);
				using (System.Data.IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
				{
					dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (isCount && partitionSize == 0 && dataReader.NextResult())
					{
						dataReader.Read();
						dbQueryResult.TotalRecords = dataReader.GetInt32(0);
					}
				}
				result = dbQueryResult;
			}
			return result;
		}

		public static string BuildRownumberQuery(string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields, int partitionSize)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = string.IsNullOrEmpty(filter) ? "" : ("WHERE " + filter);
			if (partitionSize > 0)
			{
				stringBuilder.AppendFormat("SELECT TOP {0} {1}, ROW_NUMBER() OVER (ORDER BY ", partitionSize.ToString(CultureInfo.InvariantCulture), selectFields);
			}
			else
			{
				stringBuilder.AppendFormat("SELECT {0} , ROW_NUMBER() OVER (ORDER BY ", selectFields);
			}
			stringBuilder.AppendFormat("{0} {1}", string.IsNullOrEmpty(sortBy) ? pk : sortBy, sortOrder.ToString());
			stringBuilder.AppendFormat(") AS RowNumber FROM {0} {1}", table, text);
			stringBuilder.Insert(0, "SELECT * FROM (").Append(") T WHERE T.RowNumber BETWEEN @StartNumber AND @EndNumber");
			string text2 = "";
			if (!string.IsNullOrEmpty(sortBy))
			{
				if (sortBy.IndexOf(",") > 0)
				{
					text2 = sortBy.Substring(0, sortBy.IndexOf(","));
				}
				else
				{
					text2 = sortBy;
				}
			}
			if (isCount && partitionSize == 0)
			{
				stringBuilder.AppendFormat(";SELECT COUNT(0) FROM {1} {2}", string.IsNullOrEmpty(text2) ? pk : text2, table, text);
			}
			return stringBuilder.ToString();
		}

		public static DbQueryResult PagingByTopsort(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields)
		{
			DbQueryResult result;
			if (string.IsNullOrEmpty(table))
			{
				result = null;
			}
			else if (string.IsNullOrEmpty(sortBy) && string.IsNullOrEmpty(pk))
			{
				result = null;
			}
			else
			{
				if (string.IsNullOrEmpty(selectFields))
				{
					selectFields = "*";
				}
				string query = DataHelper.BuildTopQuery(pageIndex, pageSize, sortBy, sortOrder, isCount, table, pk, filter, selectFields);
				DbQueryResult dbQueryResult = new DbQueryResult();
				Database database = DatabaseFactory.CreateDatabase();
				System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
				using (System.Data.IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
				{
					dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (isCount && dataReader.NextResult())
					{
						dataReader.Read();
						dbQueryResult.TotalRecords = dataReader.GetInt32(0);
					}
				}
				result = dbQueryResult;
			}
			return result;
		}

		private static string BuildTopQuery(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields)
		{
			string text = string.IsNullOrEmpty(sortBy) ? pk : sortBy;
			string text2 = string.IsNullOrEmpty(filter) ? "" : ("WHERE " + filter);
			string text3 = string.IsNullOrEmpty(filter) ? "" : ("AND " + filter);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} {1} FROM {2} ", pageSize.ToString(CultureInfo.InvariantCulture), selectFields, table);
			if (pageIndex == 1)
			{
				stringBuilder.AppendFormat("{0} ORDER BY {1} {2}", text2, text, sortOrder.ToString());
			}
			else
			{
				int num = (pageIndex - 1) * pageSize;
				if (sortOrder == SortAction.Asc)
				{
					stringBuilder.AppendFormat("WHERE {0} > (SELECT MAX({0}) FROM (SELECT TOP {1} {0} FROM {2} {3} ORDER BY {0} ASC) AS TMP) {4} ORDER BY {0} ASC", new object[]
					{
						text,
						num,
						table,
						text2,
						text3
					});
				}
				else
				{
					stringBuilder.AppendFormat("WHERE {0} < (SELECT MIN({0}) FROM (SELECT TOP {1} {0} FROM {2} {3} ORDER BY {0} DESC) AS TMP) {4} ORDER BY {0} DESC", new object[]
					{
						text,
						num,
						table,
						text2,
						text3
					});
				}
			}
			if (isCount)
			{
				stringBuilder.AppendFormat(";SELECT COUNT({0}) FROM {1} {2}", text, table, text2);
			}
			return stringBuilder.ToString();
		}

		public static DbQueryResult PagingByTopnotin(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string key, string filter, string selectFields)
		{
			DbQueryResult result;
			if (string.IsNullOrEmpty(table))
			{
				result = null;
			}
			else if (string.IsNullOrEmpty(key))
			{
				result = null;
			}
			else
			{
				if (string.IsNullOrEmpty(selectFields))
				{
					selectFields = "*";
				}
				string query = DataHelper.BuildNotinQuery(pageIndex, pageSize, sortBy, sortOrder, isCount, table, key, filter, selectFields);
				DbQueryResult dbQueryResult = new DbQueryResult();
				Database database = DatabaseFactory.CreateDatabase();
				System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
				using (System.Data.IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
				{
					dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (isCount && dataReader.NextResult())
					{
						dataReader.Read();
						dbQueryResult.TotalRecords = dataReader.GetInt32(0);
					}
				}
				result = dbQueryResult;
			}
			return result;
		}

		public static string BuildNotinQuery(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string key, string filter, string selectFields)
		{
			string text = string.IsNullOrEmpty(filter) ? "" : ("WHERE " + filter);
			string text2 = string.IsNullOrEmpty(filter) ? "" : ("AND " + filter);
			string text3 = string.IsNullOrEmpty(sortBy) ? "" : ("ORDER BY " + sortBy + " " + sortOrder.ToString());
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} {1} FROM {2} ", pageSize.ToString(CultureInfo.InvariantCulture), selectFields, table);
			if (pageIndex == 1)
			{
				stringBuilder.AppendFormat("{0} {1}", text, text3);
			}
			else
			{
				int num = (pageIndex - 1) * pageSize;
				stringBuilder.AppendFormat("WHERE {0} NOT IN (SELECT TOP {1} {0} FROM {2} {3} {4}) {5} {4}", new object[]
				{
					key,
					num,
					table,
					text,
					text3,
					text2
				});
			}
			if (isCount)
			{
				stringBuilder.AppendFormat(";SELECT COUNT({0}) FROM {1} {2}", key, table, text);
			}
			return stringBuilder.ToString();
		}

		public static bool SwapSequence(string table, string keyField, string sequenceField, int key, int replaceKey, int sequence, int replaceSequence)
		{
			string text = string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4}", new object[]
			{
				table,
				sequenceField,
				replaceSequence,
				keyField,
				key
			});
			text += string.Format(" UPDATE {0} SET {1} = {2} WHERE {3} = {4}", new object[]
			{
				table,
				sequenceField,
				sequence,
				keyField,
				replaceKey
			});
			Database database = DatabaseFactory.CreateDatabase();
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(text);
			return database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
