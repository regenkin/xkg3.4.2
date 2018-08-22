using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.VShop
{
	public class MessageTemplateHelperDao
	{
		private Database database;

		public MessageTemplateHelperDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public IList<MessageTemplate> GetMessageTemplates()
		{
			IList<MessageTemplate> list = new List<MessageTemplate>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MessageTemplates  where IsValid=1 ORDER BY OrderIndex");
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

		public IList<MessageTemplate> GetAliFuWuMessageTemplates()
		{
			IList<MessageTemplate> list = new List<MessageTemplate>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AliFuWuMessageTemplates  where IsValid=1 ORDER BY OrderIndex");
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
					WXOpenTM = (string)reader["WXOpenTM"]
				};
				result = messageTemplate;
			}
			return result;
		}

		public void UpdateSettings(IList<MessageTemplate> templates)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplates SET SendEmail = @SendEmail, SendSMS = @SendSMS, SendInnerMessage = @SendInnerMessage,SendWeixin = @SendWeixin , WeixinTemplateId=@WeixinTemplateId  WHERE LOWER(MessageType) = LOWER(@MessageType)");
			this.database.AddInParameter(sqlStringCommand, "SendEmail", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "SendSMS", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "SendInnerMessage", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand, "WeixinTemplateId", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand, "SendWeixin", System.Data.DbType.Boolean);
			foreach (MessageTemplate current in templates)
			{
				this.database.SetParameterValue(sqlStringCommand, "SendEmail", current.SendEmail);
				this.database.SetParameterValue(sqlStringCommand, "SendSMS", current.SendSMS);
				this.database.SetParameterValue(sqlStringCommand, "SendInnerMessage", current.SendInnerMessage);
				this.database.SetParameterValue(sqlStringCommand, "MessageType", current.MessageType);
				this.database.SetParameterValue(sqlStringCommand, "WeixinTemplateId", current.WeixinTemplateId);
				this.database.SetParameterValue(sqlStringCommand, "SendWeixin", current.SendWeixin);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}

		public void UpdateAliFuWuSettings(IList<MessageTemplate> templates)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_AliFuWuMessageTemplates SET SendEmail = @SendEmail, SendSMS = @SendSMS, SendInnerMessage = @SendInnerMessage,SendWeixin = @SendWeixin , WeixinTemplateId=@WeixinTemplateId  WHERE LOWER(MessageType) = LOWER(@MessageType)");
			this.database.AddInParameter(sqlStringCommand, "SendEmail", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "SendSMS", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "SendInnerMessage", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand, "WeixinTemplateId", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand, "SendWeixin", System.Data.DbType.Boolean);
			foreach (MessageTemplate current in templates)
			{
				this.database.SetParameterValue(sqlStringCommand, "SendEmail", current.SendEmail);
				this.database.SetParameterValue(sqlStringCommand, "SendSMS", current.SendSMS);
				this.database.SetParameterValue(sqlStringCommand, "SendInnerMessage", current.SendInnerMessage);
				this.database.SetParameterValue(sqlStringCommand, "MessageType", current.MessageType);
				this.database.SetParameterValue(sqlStringCommand, "WeixinTemplateId", current.WeixinTemplateId);
				this.database.SetParameterValue(sqlStringCommand, "SendWeixin", current.SendWeixin);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
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

		public System.Data.DataTable GetAdminUserMsgList(int userType = 0)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" select   *,\r\n                    case Msg1 when 1 then '新订单' else '' end as MsgDesc1,\r\n                    case Msg2 when 1 then '订单付款' else '' end as MsgDesc2,\r\n                    case Msg3 when 1 then '退款申请' else '' end as MsgDesc3,\r\n                    case Msg4 when 1 then '用户咨询' else '' end as MsgDesc4,\r\n                    case Msg5 when 1 then '提现申请' else '' end as MsgDesc5,\r\n                    case Msg6 when 1 then '分销商申请成功' else '' end as MsgDesc6\r\n                    from Hishop_MessageAdminUserMsgList where type=" + userType);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public System.Data.DataTable GetAdminUserMsgDetail(bool IsDistributor)
		{
			System.Data.Common.DbCommand sqlStringCommand;
			if (IsDistributor)
			{
				sqlStringCommand = this.database.GetSqlStringCommand("select *,  isnull(IsSelectedByDistributor,0)  as  IsSelected    from Hishop_MessageTemplatesDetail\r\n                    where AllowToDistributor=1\r\n                ");
			}
			else
			{
				sqlStringCommand = this.database.GetSqlStringCommand("select *,  isnull(IsSelectedByMember,0)  as  IsSelected    from Hishop_MessageTemplatesDetail\r\n                    where AllowToMember=1\r\n                ");
			}
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public bool SaveAdminUserMsgList(bool IsInsert, MsgList myList, string OldUserOpenIdIfUpdate, out string RetInfo)
		{
			RetInfo = "";
			System.Data.Common.DbCommand sqlStringCommand;
			int num;
			bool result;
			if (IsInsert)
			{
				sqlStringCommand = this.database.GetSqlStringCommand(" select count(*) as SumRec from Hishop_MessageAdminUserMsgList where UserOpenId='" + myList.UserOpenId + "' ");
				num = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand).ToString());
				if (num > 0)
				{
					RetInfo = "此OpenId已存在。";
					result = false;
					return result;
				}
				sqlStringCommand = this.database.GetSqlStringCommand(" insert into Hishop_MessageAdminUserMsgList(UserOpenId, RealName,RoleName, Msg1,Msg2,Msg3,Msg4,Msg5,Msg6,Type )\r\n                        values (@UserOpenId, @RealName,@RoleName, @Msg1,@Msg2,@Msg3,@Msg4,@Msg5,@Msg6,@Type)\r\n                ");
				this.database.AddInParameter(sqlStringCommand, "UserOpenId", System.Data.DbType.String, myList.UserOpenId);
			}
			else
			{
				sqlStringCommand = this.database.GetSqlStringCommand(" select count(*) as SumRec from Hishop_MessageAdminUserMsgList where UserOpenId='" + OldUserOpenIdIfUpdate + "' ");
				num = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand).ToString());
				if (num == 0)
				{
					RetInfo = "此OpenId不存在，无法更新。";
					result = false;
					return result;
				}
				sqlStringCommand = this.database.GetSqlStringCommand(" update  Hishop_MessageAdminUserMsgList set  RealName=@RealName,\r\n                        RoleName=@RoleName, Msg1=@Msg1,Msg2=@Msg2,Msg3=@Msg3,Msg4=@Msg4,Msg5=@Msg5,Msg6=@Msg6   \r\n                        where UserOpenId=@OldUserOpenId\r\n                ");
				this.database.AddInParameter(sqlStringCommand, "OldUserOpenId", System.Data.DbType.String, OldUserOpenIdIfUpdate);
			}
			this.database.AddInParameter(sqlStringCommand, "RealName", System.Data.DbType.String, myList.RealName);
			this.database.AddInParameter(sqlStringCommand, "RoleName", System.Data.DbType.String, myList.RoleName);
			this.database.AddInParameter(sqlStringCommand, "Msg1", System.Data.DbType.Int32, myList.Msg1);
			this.database.AddInParameter(sqlStringCommand, "Msg2", System.Data.DbType.Int32, myList.Msg2);
			this.database.AddInParameter(sqlStringCommand, "Msg3", System.Data.DbType.Int32, myList.Msg3);
			this.database.AddInParameter(sqlStringCommand, "Msg4", System.Data.DbType.Int32, myList.Msg4);
			this.database.AddInParameter(sqlStringCommand, "Msg5", System.Data.DbType.Int32, myList.Msg5);
			this.database.AddInParameter(sqlStringCommand, "Msg6", System.Data.DbType.Int32, myList.Msg6);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, myList.Type);
			num = this.database.ExecuteNonQuery(sqlStringCommand);
			if (num == 0)
			{
				RetInfo = "保存失败。";
			}
			RetInfo = "保存成功！";
			result = (num > 0);
			return result;
		}

		public bool DeleteAdminUserMsgList(MsgList myList, out string RetInfo)
		{
			RetInfo = "";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" delete from Hishop_MessageAdminUserMsgList  where UserOpenId='" + myList.UserOpenId + "' ");
			int num = this.database.ExecuteNonQuery(sqlStringCommand);
			bool result;
			if (num == 0)
			{
				RetInfo = "此OpenId不存在，无法删除。";
				result = false;
			}
			else
			{
				RetInfo = "删除成功！";
				result = true;
			}
			return result;
		}

		public void UpdateWeiXinMsgDetail(bool IsDistributor, IList<MsgDetail> templates)
		{
			if (IsDistributor)
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplatesDetail SET IsSelectedByDistributor = @IsSelectedByDistributor   WHERE LOWER(DetailType) = LOWER(@DetailType)");
				this.database.AddInParameter(sqlStringCommand, "IsSelectedByDistributor", System.Data.DbType.Int32);
				this.database.AddInParameter(sqlStringCommand, "DetailType", System.Data.DbType.String);
				foreach (MsgDetail current in templates)
				{
					this.database.SetParameterValue(sqlStringCommand, "IsSelectedByDistributor", current.IsSelectedByDistributor);
					this.database.SetParameterValue(sqlStringCommand, "DetailType", current.DetailType);
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplatesDetail SET IsSelectedByMember = @IsSelectedByMember  WHERE LOWER(DetailType) = LOWER(@DetailType)");
				this.database.AddInParameter(sqlStringCommand, "IsSelectedByMember", System.Data.DbType.Int32);
				this.database.AddInParameter(sqlStringCommand, "DetailType", System.Data.DbType.String);
				foreach (MsgDetail current in templates)
				{
					this.database.SetParameterValue(sqlStringCommand, "IsSelectedByMember", current.IsSelectedByMember);
					this.database.SetParameterValue(sqlStringCommand, "DetailType", current.DetailType);
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
			}
		}
	}
}
