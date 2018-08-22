using Hidistro.Entities;
using Hidistro.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class NoticeUserDao
	{
		private Database database;

		public NoticeUserDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public void DelAllUser(int noticeId)
		{
			string query = "delete from Hishop_NoticeUser where NoticeId=" + noticeId;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public System.Data.DataSet GetTempSelectedUser(string adminName)
		{
			string query = "select UserID from Hishop_NoticeTempUser where LoginName=@LoginName order by userid ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "LoginName", System.Data.DbType.String, adminName);
			return this.database.ExecuteDataSet(sqlStringCommand);
		}

		public void AddUser(int userid, string adminname)
		{
			string query = string.Concat(new object[]
			{
				"delete from Hishop_NoticeTempUser where LoginName=@LoginName and UserID=",
				userid,
				";insert into Hishop_NoticeTempUser(UserID,LoginName)values(",
				userid,
				",@LoginName)"
			});
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "LoginName", System.Data.DbType.String, adminname);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void DelUser(int userid, string adminname)
		{
			string query = "delete from Hishop_NoticeTempUser where LoginName=@LoginName and UserID=" + userid;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "LoginName", System.Data.DbType.String, adminname);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public IList<NoticeUserInfo> GetNoticeUserInfo(int noticeId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select NoticeId,UserId from Hishop_NoticeUser ");
			stringBuilder.Append(" where NoticeId=@NoticeId order by UserId asc ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "NoticeId", System.Data.DbType.Int32, noticeId);
			IList<NoticeUserInfo> result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<NoticeUserInfo>(dataReader);
			}
			return result;
		}
	}
}
