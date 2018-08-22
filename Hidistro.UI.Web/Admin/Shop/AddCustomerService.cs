using ControlPanel.Settings;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Settings;
using Hishop.MeiQia.Api.Api;
using Hishop.MeiQia.Api.Util;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class AddCustomerService : System.Web.IHttpHandler
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
			CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
			string unit = masterSettings.unit;
			string text = context.Request["userver"].ToString();
			string password = context.Request["password"].ToString();
			string text2 = context.Request["nickname"].ToString();
			string value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
			string tokenValue = TokenApi.GetTokenValue(masterSettings.AppId, masterSettings.AppSecret);
			if (string.IsNullOrEmpty(tokenValue))
			{
				context.Response.Write("{\"type\":\"error\",\"data\":\"获取access_token失败！\"}");
				return;
			}
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			dictionary.Add("unit", unit);
			dictionary.Add("userver", text);
			dictionary.Add("password", value);
			dictionary.Add("nickname", text2);
			dictionary.Add("realname", "");
			dictionary.Add("level", "");
			dictionary.Add("tel", "");
			string empty = string.Empty;
			CustomerServiceInfo customerServiceInfo = new CustomerServiceInfo();
			if (num != 0)
			{
				string text3 = CustomerApi.UpdateCustomer(tokenValue, dictionary);
				if (string.IsNullOrWhiteSpace(text3))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"修改客服信息失败！\"}");
					return;
				}
				string jsonValue = Hishop.MeiQia.Api.Util.Common.GetJsonValue(text3, "errcode");
				string jsonValue2 = Hishop.MeiQia.Api.Util.Common.GetJsonValue(text3, "errmsg");
				if (!(jsonValue == "0"))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"" + jsonValue2 + "\"}");
					return;
				}
				customerServiceInfo = CustomerServiceHelper.GetCustomer(num);
				customerServiceInfo.unit = unit;
				customerServiceInfo.userver = text;
				customerServiceInfo.password = password;
				customerServiceInfo.nickname = text2;
				bool flag = CustomerServiceHelper.UpdateCustomer(customerServiceInfo, ref empty);
				if (flag)
				{
					context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
					return;
				}
				context.Response.Write("{\"type\":\"error\",\"data\":\"" + empty + "\"}");
				return;
			}
			else
			{
				string text4 = CustomerApi.CreateCustomer(tokenValue, dictionary);
				if (string.IsNullOrWhiteSpace(text4))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"注册客服用户失败！\"}");
					return;
				}
				string jsonValue3 = Hishop.MeiQia.Api.Util.Common.GetJsonValue(text4, "errcode");
				string jsonValue4 = Hishop.MeiQia.Api.Util.Common.GetJsonValue(text4, "errmsg");
				if (!(jsonValue3 == "0"))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"" + jsonValue4 + "\"}");
					return;
				}
				customerServiceInfo.unit = unit;
				customerServiceInfo.userver = text;
				customerServiceInfo.password = password;
				customerServiceInfo.nickname = text2;
				int num2 = CustomerServiceHelper.CreateCustomer(customerServiceInfo, ref empty);
				if (num2 > 0)
				{
					context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
					return;
				}
				context.Response.Write("{\"type\":\"error\",\"data\":\"" + empty + "\"}");
				return;
			}
		}
	}
}
