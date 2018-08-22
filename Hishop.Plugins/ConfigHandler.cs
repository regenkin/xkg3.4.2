using System;
using System.Web;

namespace Hishop.Plugins
{
	public class ConfigHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			try
			{
				string text = context.Request["type"];
				if (text != null)
				{
					if (!(text == "PaymentRequest"))
					{
						if (!(text == "OpenIdService"))
						{
							if (!(text == "EmailSender"))
							{
								if (!(text == "SMSSender"))
								{
									if (!(text == "Logistics"))
									{
									}
								}
								else
								{
									ConfigHandler.ProcessSMSSender(context);
								}
							}
							else
							{
								ConfigHandler.ProcessEmailSender(context);
							}
						}
						else
						{
							this.ProcessOpenId(context);
						}
					}
					else
					{
						ConfigHandler.ProcessPaymentRequest(context);
					}
				}
			}
			catch
			{
			}
		}

		private void ProcessOpenId(HttpContext context)
		{
			if (context.Request["action"] == "getlist")
			{
				OpenIdPlugins openIdPlugins = OpenIdPlugins.Instance();
				context.Response.ContentType = "application/json";
				context.Response.Write(openIdPlugins.GetPlugins().ToJsonString());
			}
			else if (context.Request["action"] == "getmetadata")
			{
				context.Response.ContentType = "text/xml";
				OpenIdService openIdService = OpenIdService.CreateInstance(context.Request["name"]);
				if (openIdService == null)
				{
					context.Response.Write("<xml></xml>");
				}
				else
				{
					context.Response.Write(openIdService.GetMetaData().OuterXml);
				}
			}
		}

		private static void ProcessPaymentRequest(HttpContext context)
		{
			if (context.Request["action"] == "getlist")
			{
				PaymentPlugins paymentPlugins = PaymentPlugins.Instance();
				context.Response.ContentType = "application/json";
				context.Response.Write(paymentPlugins.GetPlugins().ToJsonString());
			}
			else if (context.Request["action"] == "getmetadata")
			{
				context.Response.ContentType = "text/xml";
				PaymentRequest paymentRequest = PaymentRequest.CreateInstance(context.Request["name"]);
				if (paymentRequest == null)
				{
					context.Response.Write("<xml></xml>");
				}
				else
				{
					context.Response.Write(paymentRequest.GetMetaData().OuterXml);
				}
			}
		}

		private static void ProcessSMSSender(HttpContext context)
		{
			if (context.Request["action"] == "getlist")
			{
				SMSPlugins sMSPlugins = SMSPlugins.Instance();
				context.Response.ContentType = "application/json";
				context.Response.Write(sMSPlugins.GetPlugins().ToJsonString());
			}
			else if (context.Request["action"] == "getmetadata")
			{
				context.Response.ContentType = "text/xml";
				SMSSender sMSSender = SMSSender.CreateInstance(context.Request["name"]);
				if (sMSSender == null)
				{
					context.Response.Write("<xml></xml>");
				}
				else
				{
					context.Response.Write(sMSSender.GetMetaData().OuterXml);
				}
			}
		}

		private static void ProcessEmailSender(HttpContext context)
		{
			if (context.Request["action"] == "getlist")
			{
				EmailPlugins emailPlugins = EmailPlugins.Instance();
				context.Response.ContentType = "application/json";
				context.Response.Write(emailPlugins.GetPlugins().ToJsonString());
			}
			else if (context.Request["action"] == "getmetadata")
			{
				context.Response.ContentType = "text/xml";
				EmailSender emailSender = EmailSender.CreateInstance(context.Request["name"]);
				if (emailSender == null)
				{
					context.Response.Write("<xml></xml>");
				}
				else
				{
					context.Response.Write(emailSender.GetMetaData().OuterXml);
				}
			}
		}
	}
}
