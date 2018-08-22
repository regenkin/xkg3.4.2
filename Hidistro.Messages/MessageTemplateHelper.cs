using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Hidistro.Messages
{
	public static class MessageTemplateHelper
	{
		private const string CacheKey = "Message-{0}";

		private const string DistributorCacheKey = "Message-{0}-{1}";

		internal static System.Net.Mail.MailMessage GetEmailTemplate(MessageTemplate template, string emailTo)
		{
			System.Net.Mail.MailMessage result;
			if (template == null || !template.SendEmail || string.IsNullOrEmpty(emailTo))
			{
				result = null;
			}
			else
			{
				System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
				{
					IsBodyHtml = true,
					Priority = System.Net.Mail.MailPriority.High,
					Body = template.EmailBody.Trim(),
					Subject = template.EmailSubject.Trim()
				};
				mailMessage.To.Add(emailTo);
				result = mailMessage;
			}
			return result;
		}

		internal static MessageTemplate GetTemplate(string messageType)
		{
			messageType = messageType.ToLower();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			string key = string.Format("Message-{0}", messageType);
			MessageTemplate messageTemplate = HiCache.Get(key) as MessageTemplate;
			if (messageTemplate == null)
			{
				messageTemplate = MessageTemplateHelper.GetMessageTemplate(messageType);
				if (messageTemplate != null)
				{
					HiCache.Max(key, messageTemplate);
				}
			}
			return messageTemplate;
		}

		internal static MessageTemplate GetFuwuTemplate(string messageType)
		{
			messageType = messageType.ToLower();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			string key = string.Format("Message-{0}", "fuwu" + messageType);
			MessageTemplate messageTemplate = HiCache.Get(key) as MessageTemplate;
			MessageTemplate result;
			if (messageTemplate == null)
			{
				if (string.IsNullOrEmpty(messageType))
				{
					result = null;
					return result;
				}
				messageTemplate = new MessageTemplateDao().GetFuwuMessageTemplate(messageType);
				if (messageTemplate != null)
				{
					HiCache.Max(key, messageTemplate);
				}
			}
			result = messageTemplate;
			return result;
		}

		public static void UpdateSettings(System.Collections.Generic.IList<MessageTemplate> templates)
		{
			if (templates != null && templates.Count != 0)
			{
				new MessageTemplateDao().UpdateSettings(templates);
				foreach (MessageTemplate current in templates)
				{
					HiCache.Remove(string.Format("Message-{0}", current.MessageType.ToLower()));
				}
			}
		}

		public static void UpdateTemplate(MessageTemplate template)
		{
			if (template != null)
			{
				new MessageTemplateDao().UpdateTemplate(template);
				HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
			}
		}

		public static MessageTemplate GetMessageTemplate(string messageType)
		{
			MessageTemplate result;
			if (string.IsNullOrEmpty(messageType))
			{
				result = null;
			}
			else
			{
				result = new MessageTemplateDao().GetMessageTemplate(messageType);
			}
			return result;
		}

		public static string GetUserOpenIdByUserId(int UserId)
		{
			string result;
			if (UserId <= 0)
			{
				result = "";
			}
			else
			{
				result = new MessageTemplateDao().GetUserOpenIdByUserId(UserId);
			}
			return result;
		}

		public static MessageTemplate GetMessageTemplateByDetailType(string DetailType)
		{
			MessageTemplate result;
			if (string.IsNullOrEmpty(DetailType))
			{
				result = null;
			}
			else
			{
				result = new MessageTemplateDao().GetMessageTemplateByDetailType(DetailType);
			}
			return result;
		}

		public static MessageTemplate GetFuwuMessageTemplateByDetailType(string DetailType)
		{
			MessageTemplate result;
			if (string.IsNullOrEmpty(DetailType))
			{
				result = null;
			}
			else
			{
				result = new MessageTemplateDao().GetFuwuMessageTemplateByDetailType(DetailType);
			}
			return result;
		}

		public static System.Collections.Generic.IList<MessageTemplate> GetMessageTemplates()
		{
			return new MessageTemplateDao().GetMessageTemplates();
		}

		public static System.Collections.Generic.List<string> GetAdminUserMsgList(string FieldName)
		{
			return new MessageTemplateDao().GetAdminUserMsgList(FieldName);
		}

		public static System.Collections.Generic.List<string> GetFuwuAdminUserMsgList(string FieldName)
		{
			return new MessageTemplateDao().GetFuwuAdminUserMsgList(FieldName);
		}
	}
}
