using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using Hidistro.SqlDal.Store;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;

namespace Hidistro.Messages
{
	public static class Emails
	{
		internal static void EnqueuEmail(System.Net.Mail.MailMessage email, SiteSettings settings)
		{
			if (email != null && email.To != null && email.To.Count > 0)
			{
				new EmailQueueDao().QueueEmail(email);
			}
		}

		public static void SendQueuedEmails(int failureInterval, int maxNumberOfTries, SiteSettings settings)
		{
			if (settings != null)
			{
				HiConfiguration config = HiConfiguration.GetConfig();
				System.Collections.Generic.Dictionary<System.Guid, System.Net.Mail.MailMessage> dictionary = new EmailQueueDao().DequeueEmail();
				System.Collections.Generic.IList<System.Guid> list = new System.Collections.Generic.List<System.Guid>();
				EmailSender emailSender = Messenger.CreateEmailSender(settings);
				if (emailSender != null)
				{
					int num = 0;
					short smtpServerConnectionLimit = config.SmtpServerConnectionLimit;
					foreach (System.Guid current in dictionary.Keys)
					{
						if (Messenger.SendMail(dictionary[current], emailSender))
						{
							new EmailQueueDao().DeleteQueuedEmail(current);
							if (smtpServerConnectionLimit != -1 && ++num >= (int)smtpServerConnectionLimit)
							{
								System.Threading.Thread.Sleep(new System.TimeSpan(0, 0, 0, 15, 0));
								num = 0;
							}
						}
						else
						{
							list.Add(current);
						}
					}
					if (list.Count > 0)
					{
						new EmailQueueDao().QueueSendingFailure(list, failureInterval, maxNumberOfTries);
					}
				}
			}
		}
	}
}
