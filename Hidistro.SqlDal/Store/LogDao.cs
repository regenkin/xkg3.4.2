using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class LogDao
	{
		private Database database;

		public LogDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool DeleteLog(long logId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Logs WHERE LogId = @LogId");
			this.database.AddInParameter(sqlStringCommand, "LogId", System.Data.DbType.Int64, logId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool DeleteAllLogs()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("TRUNCATE TABLE Hishop_Logs");
			bool result;
			try
			{
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			catch (Exception var_1_24)
			{
				result = false;
			}
			return result;
		}

		public int DeleteLogs(string strIds)
		{
			int result;
			if (strIds.Length <= 0)
			{
				result = 0;
			}
			else
			{
				string query = string.Format("DELETE FROM Hishop_Logs WHERE LogId IN ({0})", strIds);
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			return result;
		}

		public DbQueryResult GetLogs(OperationLogQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pagination page = query.Page;
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat("AddedTime >= '{0}'", query.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"));
			}
			if (query.ToDate.HasValue)
			{
				if (!string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Append(" AND");
				}
				stringBuilder.AppendFormat(" AddedTime <= '{0}'", query.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"));
			}
			if (!string.IsNullOrEmpty(query.OperationUserName))
			{
				if (!string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Append(" AND");
				}
				stringBuilder.AppendFormat(" UserName = '{0}'", DataHelper.CleanSearchString(query.OperationUserName));
			}
			return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Logs", "LogId", stringBuilder.ToString(), "*");
		}

		public IList<string> GetOperationUserNames()
		{
			IList<string> list = new List<string>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DISTINCT UserName FROM aspnet_Managers");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(dataReader["UserName"].ToString());
				}
			}
			return list;
		}

		public void WriteOperationLogEntry(OperationLogEntry entry)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_Logs]([PageUrl],[AddedTime],[UserName],[IPAddress],[Privilege],[Description]) VALUES(@PageUrl,@AddedTime,@UserName,@IPAddress,@Privilege,@Description)");
			this.database.AddInParameter(sqlStringCommand, "PageUrl", System.Data.DbType.String, entry.PageUrl);
			this.database.AddInParameter(sqlStringCommand, "AddedTime", System.Data.DbType.DateTime, entry.AddedTime);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, entry.UserName);
			this.database.AddInParameter(sqlStringCommand, "IPAddress", System.Data.DbType.String, entry.IpAddress);
			this.database.AddInParameter(sqlStringCommand, "Privilege", System.Data.DbType.Int32, (int)entry.Privilege);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, entry.Description);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
