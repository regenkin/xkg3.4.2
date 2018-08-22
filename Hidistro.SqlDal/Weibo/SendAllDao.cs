using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using Hidistro.Entities.WeiXin;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Weibo
{
	public class SendAllDao
	{
		private Database database;

		public SendAllDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public int UpdateRencentOpenID(string openid)
		{
			string query = "delete from WeiXin_RecentOpenID where OpenID=@OpenID;insert into WeiXin_RecentOpenID(OpenID)values(@OpenID)";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OpenID", System.Data.DbType.String, openid);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int UpdateRencentAliOpenID(string openid)
		{
			string query = "delete from vshop_AlipayActiveOpendId where AliOpenID=@OpenID;insert into vshop_AlipayActiveOpendId(AliOpenID)values(@OpenID)";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OpenID", System.Data.DbType.String, openid);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public string ClearWeiXinMediaID()
		{
			string query = "update vshop_Article set mediaid=null where len(mediaid)>0;update vshop_ArticleItems set mediaid=null where len(mediaid)>0;delete from WeiXin_RecentOpenID";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand).ToString();
		}

		public System.Data.DataTable GetRencentOpenID(int topnum)
		{
			int num = topnum;
			if (num < 1)
			{
				num = 1;
			}
			string query = "select top " + num + " OpenID from WeiXin_RecentOpenID where DATEADD(day,2, PubTime)>getdate() order by PubTime desc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public System.Data.DataTable GetRencentAliOpenID()
		{
			string query = "select  AliOpenID from vshop_AlipayActiveOpendId where DATEADD(day,2, PubTime)>getdate() order by PubTime desc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public bool DelOldSendAllList()
		{
			string query = "update WeiXin_SendAll set SendState=2 where SendState=0 and DATEADD(HOUR,2, SendTime)<getdate()";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateMsgId(int id, string msgid, int sendstate, int sendcount, int totalcount, string returnjsondata)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (id > 0)
			{
				stringBuilder.Append("UPDATE WeiXin_SendAll SET msgid=@msgid,sendstate=@sendstate,sendcount=@sendcount,totalcount=@totalcount,returnjsondata=@returnjsondata WHERE ID=@ID");
			}
			else if (msgid.Length > 0)
			{
				stringBuilder.Append("UPDATE WeiXin_SendAll SET sendstate=@sendstate,sendcount=@sendcount,totalcount=@totalcount,returnjsondata=@returnjsondata WHERE msgid=@msgid and sendcount=0");
			}
			bool result;
			if (!string.IsNullOrEmpty(stringBuilder.ToString()))
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, id);
				this.database.AddInParameter(sqlStringCommand, "msgid", System.Data.DbType.String, msgid);
				this.database.AddInParameter(sqlStringCommand, "sendstate", System.Data.DbType.Int32, sendstate);
				this.database.AddInParameter(sqlStringCommand, "sendcount", System.Data.DbType.Int32, sendcount);
				this.database.AddInParameter(sqlStringCommand, "totalcount", System.Data.DbType.Int32, totalcount);
				this.database.AddInParameter(sqlStringCommand, "returnjsondata", System.Data.DbType.String, returnjsondata);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public bool UpdateAddSendCount(int id, int addcount, int SendState = -1)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (id > 0)
			{
				stringBuilder.Append("UPDATE WeiXin_SendAll SET sendcount=sendcount+@addcount ");
				if (SendState >= 0)
				{
					stringBuilder.Append(",sendstate=@sendstate ");
				}
				stringBuilder.Append(" WHERE ID=@ID");
			}
			bool result;
			if (!string.IsNullOrEmpty(stringBuilder.ToString()))
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, id);
				this.database.AddInParameter(sqlStringCommand, "sendstate", System.Data.DbType.Int32, SendState);
				this.database.AddInParameter(sqlStringCommand, "addcount", System.Data.DbType.Int32, addcount);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public DbQueryResult GetSendAllRequest(SendAllQuery query, int Platform = 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(query.Title))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" Title LIKE '%{0}%' ", DataHelper.CleanSearchString(query.Title));
			}
			if (Platform > -1)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" platform={0} ", Platform);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "WeiXin_SendAll ", "ID", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public SendAllInfo GetSendAllInfo(int sendID)
		{
			SendAllInfo sendAllInfo = null;
			if (sendID > 0)
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM WeiXin_SendAll WHERE ID = @ID");
				this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, sendID);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					sendAllInfo = new SendAllInfo();
					if (dataReader.Read())
					{
						sendAllInfo.Id = sendID;
						sendAllInfo.Title = dataReader["Title"].ToString();
						object obj = dataReader["MessageType"];
						if (obj != null && obj != DBNull.Value)
						{
							sendAllInfo.MessageType = (MessageType)obj;
						}
						obj = dataReader["ArticleID"];
						if (obj != null && obj != DBNull.Value)
						{
							sendAllInfo.ArticleID = (int)obj;
						}
						sendAllInfo.Content = dataReader["Content"].ToString();
						obj = dataReader["SendState"];
						if (obj != null && obj != DBNull.Value)
						{
							sendAllInfo.SendState = (int)obj;
						}
						obj = dataReader["SendTime"];
						if (obj != null && obj != DBNull.Value)
						{
							sendAllInfo.SendTime = (DateTime)obj;
						}
						obj = dataReader["SendCount"];
						if (obj != null && obj != DBNull.Value)
						{
							sendAllInfo.SendCount = (int)obj;
						}
						obj = dataReader["msgid"];
						if (obj != null && obj != DBNull.Value)
						{
							sendAllInfo.MsgID = obj.ToString();
						}
					}
				}
			}
			return sendAllInfo;
		}

		public string SaveSendAllInfo(SendAllInfo sendAllInfo, int platform = 0)
		{
			string result = string.Empty;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO WeiXin_SendAll (Title,MessageType,ArticleID,Content,SendState,SendTime,SendCount,platform) VALUES(@Title,@MessageType,@ArticleID,@Content,@SendState,@SendTime,@SendCount,@platform);select @@identity");
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, sendAllInfo.Title);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.Int32, (int)sendAllInfo.MessageType);
			this.database.AddInParameter(sqlStringCommand, "ArticleID", System.Data.DbType.Int32, sendAllInfo.ArticleID);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, sendAllInfo.Content);
			this.database.AddInParameter(sqlStringCommand, "SendState", System.Data.DbType.Int32, sendAllInfo.SendState);
			this.database.AddInParameter(sqlStringCommand, "SendTime", System.Data.DbType.DateTime, DateTime.Now.ToString());
			this.database.AddInParameter(sqlStringCommand, "SendCount", System.Data.DbType.Int32, sendAllInfo.SendCount);
			this.database.AddInParameter(sqlStringCommand, "platform", System.Data.DbType.Int32, platform);
			int num = Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
			if (num > 0)
			{
				result = num.ToString();
			}
			return result;
		}

		public int getAlypayUserNum()
		{
			string empty = string.Empty;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(UserId) as n FROM aspnet_Members where Len(AlipayUserId)>15");
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool DeleteOldQRCode(string AppID)
		{
			string empty = string.Empty;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Delete from vshop_ScanOpenID where 1=1 or AppID=@AppID");
			this.database.AddInParameter(sqlStringCommand, "AppID", System.Data.DbType.String, AppID);
			int num = Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
			return num > 0;
		}

		public bool SaveQRCodeScanInfo(string AppID, string SCannerUserOpenID, string SCannerUserNickName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" delete  from  vshop_ScanOpenID  where 1=1 or AppID= @AppID    ");
			this.database.AddInParameter(sqlStringCommand, "AppID", System.Data.DbType.String, AppID);
			this.database.ExecuteNonQuery(sqlStringCommand);
			string empty = string.Empty;
			System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("insert into vshop_ScanOpenID(AppID,SCannerUserOpenID,SCannerUserNickName,ScanDate) values ( @AppID, @SCannerUserOpenID, @SCannerUserNickName , getdate() ) ");
			this.database.AddInParameter(sqlStringCommand2, "AppID", System.Data.DbType.String, AppID);
			this.database.AddInParameter(sqlStringCommand2, "SCannerUserOpenID", System.Data.DbType.String, SCannerUserOpenID);
			this.database.AddInParameter(sqlStringCommand2, "SCannerUserNickName", System.Data.DbType.String, SCannerUserNickName);
			int num = Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand2));
			return num > 0;
		}

		public bool GetQRCodeScanInfo(string AppID, bool IsClearAfterRead, out string SCannerUserOpenID, out string SCannerUserNickName, out string UserHead)
		{
			bool flag = false;
			SCannerUserOpenID = "";
			SCannerUserNickName = "";
			UserHead = "";
			string empty = string.Empty;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" select a.SCannerUserOpenID, b.UserName as NickName, b.UserHead    from  vshop_ScanOpenID  a  left join aspnet_Members b  on a.SCannerUserOpenID= b.OpenId   where 1=1 or a.AppID= @AppID  order by AutoID desc ");
			this.database.AddInParameter(sqlStringCommand, "AppID", System.Data.DbType.String, AppID);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					flag = true;
					object obj = dataReader["SCannerUserOpenID"];
					if (obj != null && obj != DBNull.Value)
					{
						SCannerUserOpenID = (string)obj;
						flag = true;
					}
					obj = dataReader["NickName"];
					if (obj != null && obj != DBNull.Value)
					{
						SCannerUserNickName = (string)obj;
					}
					obj = dataReader["UserHead"];
					if (obj != null && obj != DBNull.Value)
					{
						UserHead = (string)obj;
					}
				}
			}
			if (flag && IsClearAfterRead)
			{
				System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(" delete  from  vshop_ScanOpenID  where 1=1 or  AppID= @AppID    ");
				this.database.AddInParameter(sqlStringCommand2, "AppID", System.Data.DbType.String, AppID);
				this.database.ExecuteNonQuery(sqlStringCommand2);
			}
			return flag;
		}
	}
}
