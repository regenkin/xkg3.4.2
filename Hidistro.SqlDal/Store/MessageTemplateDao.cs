using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Store
{
	public class MessageTemplateDao
	{
		private Database database;

		public MessageTemplateDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public void UpdateSettings(IList<MessageTemplate> templates)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplates SET SendEmail = @SendEmail, SendSMS = @SendSMS, SendInnerMessage = @SendInnerMessage,SendWeixin = @SendWeixin WHERE LOWER(MessageType) = LOWER(@MessageType)");
			this.database.AddInParameter(sqlStringCommand, "SendEmail", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "SendSMS", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "SendInnerMessage", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand, "SendWeixin", System.Data.DbType.Boolean);
			foreach (MessageTemplate current in templates)
			{
				this.database.SetParameterValue(sqlStringCommand, "SendEmail", current.SendEmail);
				this.database.SetParameterValue(sqlStringCommand, "SendSMS", current.SendSMS);
				this.database.SetParameterValue(sqlStringCommand, "SendInnerMessage", current.SendInnerMessage);
				this.database.SetParameterValue(sqlStringCommand, "MessageType", current.MessageType);
				this.database.SetParameterValue(sqlStringCommand, "SendWeixin", current.SendWeixin);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}

		public void UpdateTemplate(MessageTemplate template)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplates SET EmailSubject = @EmailSubject, EmailBody = @EmailBody, InnerMessageSubject = @InnerMessageSubject, InnerMessageBody = @InnerMessageBody,WeixinTemplateId=@WeixinTemplateId, SMSBody = @SMSBody WHERE LOWER(MessageType) = LOWER(@MessageType)");
			this.database.AddInParameter(sqlStringCommand, "EmailSubject", System.Data.DbType.String, template.EmailSubject);
			this.database.AddInParameter(sqlStringCommand, "EmailBody", System.Data.DbType.String, template.EmailBody);
			this.database.AddInParameter(sqlStringCommand, "InnerMessageSubject", System.Data.DbType.String, template.InnerMessageSubject);
			this.database.AddInParameter(sqlStringCommand, "InnerMessageBody", System.Data.DbType.String, template.InnerMessageBody);
			this.database.AddInParameter(sqlStringCommand, "SMSBody", System.Data.DbType.String, template.SMSBody);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.String, template.MessageType);
			this.database.AddInParameter(sqlStringCommand, "WeixinTemplateId", System.Data.DbType.String, template.WeixinTemplateId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public MessageTemplate GetMessageTemplate(string messageType)
		{
			MessageTemplate result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MessageTemplates WHERE LOWER(MessageType) = LOWER(@MessageType)");
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.String, messageType);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					result = this.PopulateEmailTempletFromIDataReader(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}

		public MessageTemplate GetFuwuMessageTemplate(string messageType)
		{
			MessageTemplate result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AliFuWuMessageTemplates WHERE LOWER(MessageType) = LOWER(@MessageType)");
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.String, messageType);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					result = this.PopulateEmailTempletFromIDataReader(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}

		public MessageTemplate GetMessageTemplateByDetailType(string DetailType)
		{
			MessageTemplate result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("  select a.DetailType, a.DetailName, a.AllowToAdmin,a.AllowToDistributor,a.AllowToMember, a.IsSelectedByDistributor,a.IsSelectedByMember,\r\n                    b.* from Hishop_MessageTemplatesDetail a \r\n                left join Hishop_MessageTemplates b on a.MessageType= b.MessageType\r\n                where b.MessageType is not null and isnull(b.IsValid,0)=1\r\n                    and  LOWER(DetailType) = LOWER(@DetailType)");
			this.database.AddInParameter(sqlStringCommand, "DetailType", System.Data.DbType.String, DetailType);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					result = this.PopulateEmailTempletFromIDataReader(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}

		public MessageTemplate GetFuwuMessageTemplateByDetailType(string DetailType)
		{
			MessageTemplate result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("  select a.DetailType, a.DetailName, a.AllowToAdmin,a.AllowToDistributor,a.AllowToMember, a.IsSelectedByDistributor,a.IsSelectedByMember,\r\n                    b.* from Hishop_AliFuWuMessageTemplatesDetail a \r\n                left join Hishop_AliFuWuMessageTemplates b on a.MessageType= b.MessageType\r\n                where b.MessageType is not null and isnull(b.IsValid,0)=1\r\n                    and  LOWER(DetailType) = LOWER(@DetailType)");
			this.database.AddInParameter(sqlStringCommand, "DetailType", System.Data.DbType.String, DetailType);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					result = this.PopulateEmailTempletFromIDataReader(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}

		public IList<MessageTemplate> GetMessageTemplates()
		{
			IList<MessageTemplate> list = new List<MessageTemplate>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MessageTemplates");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(this.PopulateEmailTempletFromIDataReader(dataReader));
				}
				dataReader.Close();
			}
			return list;
		}

		public MessageTemplate PopulateEmailTempletFromIDataReader(System.Data.IDataReader reader)
		{
			MessageTemplate result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				MessageTemplate messageTemplate = new MessageTemplate((string)reader["TagDescription"], (string)reader["Name"])
				{
					MessageType = (string)reader["MessageType"],
					SendInnerMessage = (bool)reader["SendInnerMessage"],
					SendWeixin = (bool)reader["SendWeixin"],
					SendSMS = (bool)reader["SendSMS"],
					SendEmail = (bool)reader["SendEmail"],
					EmailSubject = (string)reader["EmailSubject"],
					EmailBody = (string)reader["EmailBody"],
					InnerMessageSubject = (string)reader["InnerMessageSubject"],
					InnerMessageBody = (string)reader["InnerMessageBody"],
					SMSBody = (string)reader["SMSBody"],
					WeixinTemplateId = (reader["WeixinTemplateId"] != DBNull.Value) ? ((string)reader["WeixinTemplateId"]) : "",
					IsSendWeixin_ToDistributor = Convert.ToString(reader["IsSelectedByDistributor"]) == "1",
					IsSendWeixin_ToMember = Convert.ToString(reader["IsSelectedBymember"]) == "1"
				};
				result = messageTemplate;
			}
			return result;
		}

		public List<string> GetAdminUserMsgList(string MsgFieldName)
		{
			List<string> list = new List<string>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_MessageAdminUserMsgList WHERE isnull(" + MsgFieldName + ",0) = 1 and  Type=0 ");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((string)dataReader["UserOpenId"]);
				}
				dataReader.Close();
			}
			return list;
		}

		public List<string> GetFuwuAdminUserMsgList(string MsgFieldName)
		{
			List<string> list = new List<string>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_MessageAdminUserMsgList WHERE isnull(" + MsgFieldName + ",0) = 1  and  Type=1");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((string)dataReader["UserOpenId"]);
				}
				dataReader.Close();
			}
			return list;
		}

		public string GetUserOpenIdByUserId(int UserId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select top 1 OpenId  from aspnet_Members WHERE UserId= " + UserId.ToString());
			return Convert.ToString(this.database.ExecuteScalar(sqlStringCommand));
		}
	}
}
