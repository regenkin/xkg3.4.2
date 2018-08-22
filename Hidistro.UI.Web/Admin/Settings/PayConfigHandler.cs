using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class PayConfigHandler : System.Web.IHttpHandler
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
			try
			{
				string a = context.Request["type"].ToString();
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				if (a == "0")
				{
					string offlinePay_BankCard_Name = context.Request["name"].ToString();
					string offlinePay_BankCard_CardNo = context.Request["card"].ToString();
					string offlinePay_BankCard_BankName = context.Request["bank"].ToString();
					masterSettings.OfflinePay_BankCard_Name = offlinePay_BankCard_Name;
					masterSettings.OfflinePay_BankCard_CardNo = offlinePay_BankCard_CardNo;
					masterSettings.OfflinePay_BankCard_BankName = offlinePay_BankCard_BankName;
				}
				else if (a == "1")
				{
					string offlinePay_Alipay_id = context.Request["mid"].ToString();
					masterSettings.OfflinePay_Alipay_id = offlinePay_Alipay_id;
				}
				else if (a == "2")
				{
					string offLinePayContent = context.Request["content"].ToString();
					masterSettings.OffLinePayContent = offLinePayContent;
				}
				else if (a == "3")
				{
					string shenPay_mid = context.Request["mid"].ToString();
					string shenPay_key = context.Request["key"].ToString();
					masterSettings.ShenPay_mid = shenPay_mid;
					masterSettings.ShenPay_key = shenPay_key;
				}
				else if (a == "4")
				{
					string alipay_mid = context.Request["mid"].ToString();
					string alipay_mName = context.Request["name"].ToString();
					string alipay_Pid = context.Request["pid"].ToString();
					string alipay_Key = context.Request["key"].ToString();
					masterSettings.Alipay_mid = alipay_mid;
					masterSettings.Alipay_mName = alipay_mName;
					masterSettings.Alipay_Pid = alipay_Pid;
					masterSettings.Alipay_Key = alipay_Key;
				}
				else if (a == "5")
				{
					string weixinAppId = context.Request["appid"].ToString();
					string weixinAppSecret = context.Request["appsecret"].ToString();
					string weixinPartnerID = context.Request["mch_id"].ToString();
					string weixinPartnerKey = context.Request["key"].ToString();
					masterSettings.WeixinAppId = weixinAppId;
					masterSettings.WeixinAppSecret = weixinAppSecret;
					masterSettings.WeixinPartnerID = weixinPartnerID;
					masterSettings.WeixinPartnerKey = weixinPartnerKey;
				}
				else if (a == "6")
				{
					string chinaBank_mid = context.Request["mid"].ToString();
					string chinaBank_MD = context.Request["md5"].ToString();
					string chinaBank_DES = context.Request["des"].ToString();
					masterSettings.ChinaBank_mid = chinaBank_mid;
					masterSettings.ChinaBank_MD5 = chinaBank_MD;
					masterSettings.ChinaBank_DES = chinaBank_DES;
				}
				else if (a == "7")
				{
					string weixinCertPassword = context.Request["key"].ToString();
					masterSettings.WeixinCertPassword = weixinCertPassword;
				}
				else if (a == "-1")
				{
					bool enablePodRequest = bool.Parse(context.Request["enable"].ToString());
					masterSettings.EnablePodRequest = enablePodRequest;
				}
				else if (a == "-2")
				{
					bool enableOffLineRequest = bool.Parse(context.Request["enable"].ToString());
					masterSettings.EnableOffLineRequest = enableOffLineRequest;
				}
				else if (a == "-3")
				{
					bool enableWapShengPay = bool.Parse(context.Request["enable"].ToString());
					masterSettings.EnableWapShengPay = enableWapShengPay;
				}
				else if (a == "-4")
				{
					bool enableAlipayRequest = bool.Parse(context.Request["enable"].ToString());
					masterSettings.EnableAlipayRequest = enableAlipayRequest;
				}
				else if (a == "-5")
				{
					bool enableWeiXinRequest = bool.Parse(context.Request["enable"].ToString());
					masterSettings.EnableWeiXinRequest = enableWeiXinRequest;
				}
				else if (a == "-6")
				{
					bool chinaBank_Enable = bool.Parse(context.Request["enable"].ToString());
					masterSettings.ChinaBank_Enable = chinaBank_Enable;
				}
				else if (a == "-7")
				{
					bool enableWeixinRed = bool.Parse(context.Request["enable"].ToString());
					masterSettings.EnableWeixinRed = enableWeixinRed;
				}
				SettingsManager.Save(masterSettings);
				context.Response.Write("保存成功");
			}
			catch (System.Exception ex)
			{
				context.Response.Write("保存失败！（" + ex.Message + ")");
			}
		}
	}
}
