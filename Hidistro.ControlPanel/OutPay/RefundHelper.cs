using Hidistro.ControlPanel.OutPay.App;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;

namespace Hidistro.ControlPanel.OutPay
{
	public static class RefundHelper
	{
		public static string GenerateRefundOrderId()
		{
			string text = string.Empty;
			Random random = new Random();
			for (int i = 0; i < 7; i++)
			{
				int num = random.Next();
				text += ((char)(48 + (ushort)(num % 10))).ToString();
			}
			return DateTime.Now.ToString("yyyyMMdd") + text;
		}

		public static string SendWxRefundRequest(string out_trade_no, decimal orderTotal, decimal RefundMoney, string RefundOrderId, out string WxRefundNum)
		{
			if (RefundMoney == 0m)
			{
				RefundMoney = orderTotal;
			}
			RefundInfo refundInfo = new RefundInfo();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			WxRefundNum = "";
			string result;
			if (!masterSettings.EnableWeiXinRequest)
			{
				result = "微信支付功能未开启";
			}
			else
			{
				refundInfo.out_refund_no = RefundOrderId;
				refundInfo.out_trade_no = out_trade_no;
				refundInfo.RefundFee = new decimal?((int)(RefundMoney * 100m));
				refundInfo.TotalFee = new decimal?((int)(orderTotal * 100m));
				PayConfig payConfig = new PayConfig();
				WxRefundNum = "";
				string text = "";
				try
				{
					if (masterSettings.EnableSP)
					{
						payConfig.AppId = masterSettings.Main_AppId;
						payConfig.MchID = masterSettings.Main_Mch_ID;
						payConfig.Key = masterSettings.Main_PayKey;
						payConfig.sub_appid = masterSettings.WeixinAppId;
						payConfig.sub_mch_id = masterSettings.WeixinPartnerID;
					}
					else
					{
						payConfig.AppId = masterSettings.WeixinAppId;
						payConfig.MchID = masterSettings.WeixinPartnerID;
						payConfig.Key = masterSettings.WeixinPartnerKey;
						payConfig.sub_appid = "";
						payConfig.sub_mch_id = "";
					}
					payConfig.AppSecret = masterSettings.WeixinAppSecret;
					payConfig.SSLCERT_PATH = masterSettings.WeixinCertPath;
					payConfig.SSLCERT_PASSWORD = masterSettings.WeixinCertPassword;
					if (payConfig.AppId == "" || payConfig.MchID == "" || payConfig.AppSecret == "" || payConfig.Key == "")
					{
						text = "微信公众号配置参数错误，不能为空！";
					}
					else if (payConfig.SSLCERT_PATH == "" && payConfig.SSLCERT_PASSWORD == "")
					{
						text = "微信证书以及密码不能为空！解决办法:请到微信-->微信红包-->上传微信证书和填写证书密码。";
					}
					else
					{
						text = Refund.SendRequest(refundInfo, payConfig, out WxRefundNum);
					}
				}
				catch (Exception var_4_213)
				{
					text = "ERROR";
				}
				if (text.ToUpper() == "SUCCESS")
				{
					text = "";
				}
				result = text;
			}
			return result;
		}

		public static string AlipayRefundRequest(string _notify_url, List<alipayReturnInfo> RefundList)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			string result;
			if (!masterSettings.EnableAlipayRequest)
			{
				result = "支付宝支付功能未开启，无法完成支付！";
			}
			else if (masterSettings.Alipay_Pid == "" || masterSettings.Alipay_Key == "" || masterSettings.Alipay_mid == "" || masterSettings.Alipay_mName == "")
			{
				result = "支付宝参数设置错误，请检查支付宝配置参数！";
			}
			else
			{
				string alipay_Pid = masterSettings.Alipay_Pid;
				string alipay_Key = masterSettings.Alipay_Key;
				string alipay_mid = masterSettings.Alipay_mid;
				string alipay_mName = masterSettings.Alipay_mName;
				string text = "utf-8";
				App.Core.setConfig(alipay_Pid, "MD5", alipay_Key, text);
				string alipay_mid2 = masterSettings.Alipay_mid;
				string value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				string value2 = RefundHelper.GenerateRefundOrderId();
				string value3 = RefundList.Count.ToString();
				List<string> list = new List<string>();
				foreach (alipayReturnInfo current in RefundList)
				{
					list.Add(string.Concat(new string[]
					{
						current.alipaynum,
						"^",
						current.refundNum.ToString("F2"),
						"^",
						current.Remark
					}));
				}
				string value4 = string.Join("#", list);
				string text2 = App.Core.BuildRequest(new SortedDictionary<string, string>
				{
					{
						"partner",
						alipay_Pid
					},
					{
						"_input_charset",
						text
					},
					{
						"service",
						"refund_fastpay_by_platform_pwd"
					},
					{
						"notify_url",
						_notify_url
					},
					{
						"seller_email",
						alipay_mid2
					},
					{
						"refund_date",
						value
					},
					{
						"batch_no",
						value2
					},
					{
						"batch_num",
						value3
					},
					{
						"detail_data",
						value4
					}
				}, "get", "确认");
				result = text2;
			}
			return result;
		}
	}
}
