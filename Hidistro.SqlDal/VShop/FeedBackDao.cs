using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.VShop
{
	public class FeedBackDao
	{
		private Database database;

		public FeedBackDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool Save(FeedBackInfo info)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO vshop_FeedBackNotify (AppId, TimeStamp, OpenId, MsgType, FeedBackId, TransId, Reason, Solution, ExtInfo) VALUES (@AppId, @TimeStamp, @OpenId, @MsgType, @FeedBackId, @TransId, @Reason, @Solution, @ExtInfo)");
			this.database.AddInParameter(sqlStringCommand, "AppId", System.Data.DbType.String, info.AppId);
			this.database.AddInParameter(sqlStringCommand, "TimeStamp", System.Data.DbType.DateTime, info.TimeStamp);
			this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, info.OpenId);
			this.database.AddInParameter(sqlStringCommand, "MsgType", System.Data.DbType.String, info.MsgType);
			this.database.AddInParameter(sqlStringCommand, "FeedBackId", System.Data.DbType.String, info.FeedBackId);
			this.database.AddInParameter(sqlStringCommand, "TransId", System.Data.DbType.String, info.TransId);
			this.database.AddInParameter(sqlStringCommand, "Reason", System.Data.DbType.String, info.Reason);
			this.database.AddInParameter(sqlStringCommand, "Solution", System.Data.DbType.String, info.Solution);
			this.database.AddInParameter(sqlStringCommand, "ExtInfo", System.Data.DbType.String, info.ExtInfo);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public FeedBackInfo Get(int id)
		{
			FeedBackInfo result = new FeedBackInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_FeedBackNotify WHERE FeedBackNotifyID=@FeedBackNotifyID");
			this.database.AddInParameter(sqlStringCommand, "FeedBackNotifyID", System.Data.DbType.Int32, id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<FeedBackInfo>(dataReader);
			}
			return result;
		}

		public FeedBackInfo Get(string feedBackID)
		{
			FeedBackInfo result = new FeedBackInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_FeedBackNotify WHERE FeedBackId=@FeedBackId");
			this.database.AddInParameter(sqlStringCommand, "FeedBackId", System.Data.DbType.String, feedBackID);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<FeedBackInfo>(dataReader);
			}
			return result;
		}

		public bool UpdateMsgType(string feedBackId, string msgType)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_FeedBackNotify SET MsgType=@MsgType WHERE FeedBackId=@FeedBackId");
			this.database.AddInParameter(sqlStringCommand, "MsgType", System.Data.DbType.String, msgType);
			this.database.AddInParameter(sqlStringCommand, "FeedBackId", System.Data.DbType.String, feedBackId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool Delete(int id)
		{
			string query = string.Format("DELETE FROM vshop_FeedBackNotify WHERE FeedBackNotifyID = {0}", id);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DbQueryResult List(int pageIndex, int pageSize, string msgType)
		{
			string filter = " 1=1 ";
			if (!string.IsNullOrEmpty(msgType))
			{
				filter = string.Format(" MsgType = '{0}' ", msgType);
			}
			return DataHelper.PagingByRownumber(pageIndex, pageSize, "FeedBackNotifyID", SortAction.Desc, true, "vshop_FeedBackNotify p", "FeedBackNotifyID", filter, "*");
		}
	}
}
