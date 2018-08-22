using ControlPanel.Settings;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Settings;
using Hishop.MeiQia.Api.Api;
using Hishop.MeiQia.Api.Util;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class DeleteCustomerService : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			int num = 0;
			int.TryParse(context.Request["id"].ToString(), out num);
			if (num > 0)
			{
				CustomerServiceInfo customer = CustomerServiceHelper.GetCustomer(num);
				CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
				string tokenValue = TokenApi.GetTokenValue(masterSettings.AppId, masterSettings.AppSecret);
				if (!string.IsNullOrEmpty(tokenValue))
				{
					string text = CustomerApi.DeleteCustomer(tokenValue, customer.unit, customer.userver);
					if (string.IsNullOrWhiteSpace(text))
					{
						context.Response.Write("{\"type\":\"error\",\"data\":\"删除客服失败！\"}");
						return;
					}
					string jsonValue = Hishop.MeiQia.Api.Util.Common.GetJsonValue(text, "errcode");
					string jsonValue2 = Hishop.MeiQia.Api.Util.Common.GetJsonValue(text, "errmsg");
					if (!(jsonValue == "0"))
					{
						context.Response.Write("{\"type\":\"error\",\"data\":\"" + jsonValue2 + "\"}");
						return;
					}
					bool flag = CustomerServiceHelper.DeletCustomer(num);
					if (flag)
					{
						context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
						return;
					}
					context.Response.Write("{\"type\":\"error\",\"data\":\"删除客服失败！\"}");
					return;
				}
				else
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"获取access_token失败！\"}");
				}
			}
		}
	}
}
