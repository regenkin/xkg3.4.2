using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.VShop
{
	public class AlarmDao
	{
		private Database database;

		public AlarmDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool Save(AlarmInfo info)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO vshop_AlarmNotify (AppId, ErrorType, Description, AlarmContent, TimeStamp) VALUES (@AppId, @ErrorType, @Description, @AlarmContent, @TimeStamp)");
			this.database.AddInParameter(sqlStringCommand, "AppId", System.Data.DbType.String, info.AppId);
			this.database.AddInParameter(sqlStringCommand, "ErrorType", System.Data.DbType.Int32, info.ErrorType);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, info.Description);
			this.database.AddInParameter(sqlStringCommand, "AlarmContent", System.Data.DbType.String, info.AlarmContent);
			this.database.AddInParameter(sqlStringCommand, "TimeStamp", System.Data.DbType.DateTime, info.TimeStamp);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool Delete(int id)
		{
			string query = string.Format("DELETE FROM vshop_AlarmNotify WHERE AlarmNotifyId = {0}", id);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DbQueryResult List(int pageIndex, int pageSize)
		{
			return DataHelper.PagingByRownumber(pageIndex, pageSize, "AlarmNotifyId", SortAction.Desc, true, "vshop_AlarmNotify p", "AlarmNotifyId", " 1=1 ", "*");
		}
	}
}
