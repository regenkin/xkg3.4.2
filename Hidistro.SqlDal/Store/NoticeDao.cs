using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class NoticeDao
	{
		private Database database;

		public NoticeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool GetUserIsSel(int userid, string adminName)
		{
			string query = "select top 1 userid from Hishop_NoticeTempUser where userid=" + userid + " and LoginName=@LoginName";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "LoginName", System.Data.DbType.String, adminName);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0].Rows.Count > 0;
		}

		public System.Data.DataSet GetSelectedUser(string adminName)
		{
			string query = "select a.UserId,b.UserName,b.CellPhone,b.UserBindName from Hishop_NoticeTempUser a left join aspnet_Members b on a.userid=b.userid where a.LoginName=@LoginName and b.Status=1";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "LoginName", System.Data.DbType.String, adminName);
			return this.database.ExecuteDataSet(sqlStringCommand);
		}

		public int GetSelectedUser(int noticeid)
		{
			string query = "select count(0) from Hishop_NoticeUser where NoticeId=" + noticeid;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
		}

		public int SaveNotice(NoticeInfo info)
		{
			string query = string.Empty;
			if (info.SendType == 1)
			{
				if (!info.PubTime.HasValue)
				{
					info.PubTime = new DateTime?(DateTime.Now);
				}
				if (info.IsPub == 0)
				{
					info.IsPub = 1;
				}
			}
			if (info.Id > 0)
			{
				query = "Update Hishop_Notice set Title=@Title,Memo=@Memo,Author=@Author where Id=" + info.Id;
			}
			else if (info.PubTime.HasValue)
			{
				query = "INSERT INTO Hishop_Notice (Title,Memo,Author,IsPub,AddTime,PubTime,SendType,SendTo) VALUES (@Title,@Memo,@Author,@IsPub,@AddTime,@PubTime,@SendType,@SendTo);select @@identity;";
			}
			else
			{
				query = "INSERT INTO Hishop_Notice (Title,Memo,Author,IsPub,AddTime,SendType,SendTo) VALUES (@Title,@Memo,@Author,@IsPub,@AddTime,@SendType,@SendTo);select @@identity;";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			if (info.PubTime.HasValue)
			{
				this.database.AddInParameter(sqlStringCommand, "PubTime", System.Data.DbType.DateTime, info.PubTime);
			}
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, info.Title);
			this.database.AddInParameter(sqlStringCommand, "Memo", System.Data.DbType.String, info.Memo);
			this.database.AddInParameter(sqlStringCommand, "Author", System.Data.DbType.String, info.Author);
			this.database.AddInParameter(sqlStringCommand, "IsPub", System.Data.DbType.Int32, info.IsPub);
			this.database.AddInParameter(sqlStringCommand, "AddTime", System.Data.DbType.DateTime, info.AddTime);
			this.database.AddInParameter(sqlStringCommand, "SendType", System.Data.DbType.Int32, info.SendType);
			this.database.AddInParameter(sqlStringCommand, "SendTo", System.Data.DbType.Int32, info.SendTo);
			int num = Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
			if (info.Id > 0)
			{
				num = info.Id;
			}
			if (num > 0 && info.SendTo == 2)
			{
				if (info.Id > 0)
				{
					new NoticeUserDao().DelAllUser(info.Id);
				}
				foreach (NoticeUserInfo current in info.NoticeUserInfo)
				{
					query = string.Concat(new object[]
					{
						"insert into Hishop_NoticeUser(NoticeId,UserID)values(",
						num,
						",",
						current.UserId,
						")"
					});
					sqlStringCommand = this.database.GetSqlStringCommand(query);
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
			}
			return num;
		}

		public NoticeInfo GetNoticeInfo(int id)
		{
			NoticeInfo noticeInfo = new NoticeInfo();
			string query = "select id,Title,Memo,Author,IsPub,AddTime,PubTime,SendType,SendTo from Hishop_Notice where ID=" + id;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable dataTable = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
			NoticeInfo result;
			if (dataTable.Rows.Count > 0)
			{
				noticeInfo.Id = id;
				noticeInfo.Title = dataTable.Rows[0]["Title"].ToString();
				noticeInfo.Memo = dataTable.Rows[0]["Memo"].ToString();
				noticeInfo.Author = dataTable.Rows[0]["Author"].ToString();
				noticeInfo.IsPub = Globals.ToNum(dataTable.Rows[0]["IsPub"]);
				noticeInfo.AddTime = DateTime.Parse(dataTable.Rows[0]["AddTime"].ToString());
				noticeInfo.SendType = Globals.ToNum(dataTable.Rows[0]["SendType"]);
				noticeInfo.SendTo = Globals.ToNum(dataTable.Rows[0]["SendTo"]);
				if (dataTable.Rows[0]["PubTime"] != DBNull.Value)
				{
					noticeInfo.PubTime = new DateTime?(DateTime.Parse(dataTable.Rows[0]["PubTime"].ToString()));
				}
				if (noticeInfo.SendTo == 2)
				{
					noticeInfo.NoticeUserInfo = new NoticeUserDao().GetNoticeUserInfo(noticeInfo.Id);
				}
				result = noticeInfo;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public DbQueryResult GetNoticeRequest(NoticeQuery query)
		{
			string table = "Hishop_Notice";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SendType={0} ", query.SendType.ToString());
			if (!string.IsNullOrEmpty(query.Title))
			{
				stringBuilder.AppendFormat(" AND Title LIKE '%{0}%' ", DataHelper.CleanSearchString(query.Title));
			}
			if (!string.IsNullOrEmpty(query.Author))
			{
				stringBuilder.AppendFormat(" AND Author='{0}' ", DataHelper.CleanSearchString(query.Author));
			}
			if (query.StartTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND PubTime>='{0}' ", query.StartTime.Value.ToString("yyyy-MM-dd"));
			}
			if (query.EndTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND PubTime<'{0}' ", query.EndTime.Value.AddDays(1.0).ToString("yyyy-MM-dd"));
			}
			if (query.UserId.HasValue)
			{
				if (query.SendType == 1)
				{
					table = "vw_Hishop_Notice";
					stringBuilder.AppendFormat(" AND (UserID={0} or UserID=0) ", query.UserId);
				}
				if (query.IsNotShowRead.HasValue)
				{
					table = "vw_Hishop_Notice";
					stringBuilder.Append(" AND id not in(select NoticeId from Hishop_NoticeRead where UserID=" + query.UserId + ") ");
				}
			}
			if (!query.IsDistributor.HasValue || !query.IsDistributor.Value)
			{
				stringBuilder.Append(" AND SendTo<>1 ");
			}
			if (query.IsPub.HasValue)
			{
				stringBuilder.AppendFormat(" AND IsPub={0} ", query.IsPub.Value);
			}
			if (query.IsDel.HasValue && query.IsDel == 1)
			{
				table = "vw_Hishop_Notice";
				stringBuilder.AppendFormat(" AND id not in(select NoticeId from Hishop_NoticeRead where UserID=" + query.UserId + " and NoticeIsDel=1)  ", query.IsPub.Value);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "ID", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public int GetNoticeNotReadCount(NoticeQuery query)
		{
			string str = "Hishop_Notice";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SendType={0} ", query.SendType.ToString());
			stringBuilder.AppendFormat(" AND PubTime>='{0}' ", DateTime.Now.AddYears(-1));
			if (query.UserId.HasValue)
			{
				if (query.SendType == 1)
				{
					stringBuilder.AppendFormat(" AND (UserID={0} or UserID=0) ", query.UserId);
				}
				str = "vw_Hishop_Notice";
				stringBuilder.Append(" AND id not in(select NoticeId from Hishop_NoticeRead where UserID=" + query.UserId + ") ");
			}
			if (!query.IsDistributor.HasValue || !query.IsDistributor.Value)
			{
				stringBuilder.Append(" AND SendTo<>1 ");
			}
			stringBuilder.AppendFormat(" AND IsPub={0} ", 1);
			if (query.IsDel.HasValue && query.IsDel == 1)
			{
				str = "vw_Hishop_Notice";
				stringBuilder.AppendFormat(" AND id not in(select NoticeId from Hishop_NoticeRead where UserID={0} and NoticeIsDel=1)  ", query.UserId);
			}
			string query2 = "select count(0) from " + str + " where " + stringBuilder.ToString();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query2);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
		}

		public System.Data.DataTable GetNoticeNotReadDt(NoticeQuery query)
		{
			string str = "vw_Hishop_Notice";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("  PubTime>='{0}' ", DateTime.Now.AddYears(-1));
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND (UserID={0} or UserID=0) ", query.UserId);
				stringBuilder.Append(" AND id not in(select NoticeId from Hishop_NoticeRead where UserID=" + query.UserId + ") ");
			}
			if (!query.IsDistributor.HasValue || (query.IsDistributor.HasValue && !query.IsDistributor.Value))
			{
				stringBuilder.Append(" AND SendTo<>1 ");
			}
			stringBuilder.AppendFormat(" AND IsPub={0} ", 1);
			stringBuilder.Append("  order by PubTime desc ");
			string query2 = "select top 5 * from " + str + " where " + stringBuilder.ToString();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query2);
			System.Data.DataTable result = new System.Data.DataTable();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public void ViewNotice(int userid, int noticeid)
		{
			if (!this.IsView(userid, noticeid))
			{
				string query = string.Concat(new object[]
				{
					"insert into Hishop_NoticeRead(NoticeId,UserId)values(",
					noticeid,
					",",
					userid,
					")"
				});
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}

		public bool IsView(int userid, int noticeid)
		{
			string query = string.Concat(new object[]
			{
				"select top 1 noticeid from Hishop_NoticeRead where NoticeId=",
				noticeid,
				" and UserId=",
				userid
			});
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)) > 0;
		}

		public bool DelNotice(int noticeid)
		{
			string query = string.Concat(new object[]
			{
				"delete from Hishop_NoticeRead where NoticeId=",
				noticeid,
				";delete from Hishop_NoticeUser where NoticeId=",
				noticeid,
				";delete from Hishop_Notice where ID=",
				noticeid
			});
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)) > 0;
		}

		public bool NoticePub(int noticeid)
		{
			string query = "update Hishop_Notice set IsPub=1,PubTime=getdate() where ID=" + noticeid;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)) > 0;
		}
	}
}
