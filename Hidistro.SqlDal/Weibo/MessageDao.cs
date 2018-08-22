using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Weibo;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Weibo
{
	public class MessageDao
	{
		private Database database;

		public MessageDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public int SaveMessage(MessageInfo messageInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Weibo_Message ([Type],[Receiver_id],[Sender_id],[Created_at],[Text],[Vfid],[Tovfid],[Status],[Access_Token]) VALUES(@Type,@Receiver_id,@Sender_id,@Created_at,@Text,@Vfid,@Tovfid,@Status,@Access_Token);SELECT @@Identity");
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.String, messageInfo.Type);
			this.database.AddInParameter(sqlStringCommand, "Receiver_id", System.Data.DbType.String, messageInfo.Receiver_id);
			this.database.AddInParameter(sqlStringCommand, "Sender_id", System.Data.DbType.String, messageInfo.Sender_id);
			this.database.AddInParameter(sqlStringCommand, "Created_at", System.Data.DbType.DateTime, messageInfo.Created_at);
			this.database.AddInParameter(sqlStringCommand, "Text", System.Data.DbType.String, messageInfo.Text);
			this.database.AddInParameter(sqlStringCommand, "Vfid", System.Data.DbType.String, messageInfo.Vfid);
			this.database.AddInParameter(sqlStringCommand, "Tovfid", System.Data.DbType.String, messageInfo.Tovfid);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, messageInfo.Status);
			this.database.AddInParameter(sqlStringCommand, "Access_Token", System.Data.DbType.String, messageInfo.Access_Token);
			int result = 0;
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				result = Convert.ToInt32(obj.ToString());
			}
			return result;
		}

		public MessageInfo GetMessageInfo(int MessageId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Message WHERE MessageId = @MessageId");
			this.database.AddInParameter(sqlStringCommand, "MessageId", System.Data.DbType.Int32, MessageId);
			MessageInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MessageInfo>(dataReader);
			}
			return result;
		}

		public bool UpdateMessage(MessageInfo messageInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Weibo_Message set SenderMessage=@SenderMessage,SenderDate=@SenderDate,Display_Name=@Display_Name,Summary=@Summary,Image=@Image,Url=@Url,Status=@Status,ArticleId=@ArticleId where MessageId=@MessageId");
			this.database.AddInParameter(sqlStringCommand, "SenderMessage", System.Data.DbType.String, messageInfo.SenderMessage);
			this.database.AddInParameter(sqlStringCommand, "Display_Name", System.Data.DbType.String, messageInfo.DisplayName);
			this.database.AddInParameter(sqlStringCommand, "Summary", System.Data.DbType.String, messageInfo.Summary);
			this.database.AddInParameter(sqlStringCommand, "SenderDate", System.Data.DbType.DateTime, messageInfo.SenderDate);
			this.database.AddInParameter(sqlStringCommand, "Image", System.Data.DbType.String, messageInfo.Image);
			this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, messageInfo.Url);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, messageInfo.Status);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, messageInfo.ArticleId);
			this.database.AddInParameter(sqlStringCommand, "MessageId", System.Data.DbType.Int32, messageInfo.MessageId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public DbQueryResult GetMessages(MessageQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.Status > -1)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("Status = {0}", query.Status);
			}
			query.SortBy = "Created_at";
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" AND ");
			}
			stringBuilder.AppendFormat("Access_Token = '{0}'", SettingsManager.GetMasterSettings(false).Access_Token);
			query.SortOrder = SortAction.Desc;
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Weibo_Message", "MessageId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}
	}
}
