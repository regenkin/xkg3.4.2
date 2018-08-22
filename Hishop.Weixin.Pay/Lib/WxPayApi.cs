using Hishop.Weixin.Pay.Domain;
using System;

namespace Hishop.Weixin.Pay.Lib
{
	public class WxPayApi
	{
		public static WxPayData Micropay(WxPayData inputObj, PayConfig config, int timeOut = 10)
		{
			string url = "https://api.mch.weixin.qq.com/pay/micropay";
			inputObj.SetValue("spbill_create_ip", config.IPAddress);
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, url, false, config, timeOut);
			DateTime now2 = DateTime.Now;
			int num = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			return wxPayData;
		}

		public static WxPayData OrderQuery(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string url = "https://api.mch.weixin.qq.com/pay/orderquery";
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, url, false, config, timeOut);
			DateTime now2 = DateTime.Now;
			int num = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			return wxPayData;
		}

		public static WxPayData Reverse(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string url = "https://api.mch.weixin.qq.com/secapi/pay/reverse";
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, url, true, config, timeOut);
			DateTime now2 = DateTime.Now;
			int num = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			return wxPayData;
		}

		public static WxPayData Refund(WxPayData inputObj, PayConfig config, int timeOut = 60)
		{
			string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string text = HttpService.Post(xml, url, true, config, timeOut);
			WxPayData wxPayData = new WxPayData();
			if (!text.StartsWith("POSTERROR"))
			{
				wxPayData.FromXml(text, config.Key);
			}
			else
			{
				wxPayData.SetValue("return_msg", text.Replace("POSTERROR", "").Replace("\r", "").Replace("\n", ""));
			}
			return wxPayData;
		}

		public static WxPayData RefundQuery(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string url = "https://api.mch.weixin.qq.com/pay/refundquery";
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, url, false, config, timeOut);
			DateTime now2 = DateTime.Now;
			int num = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			return wxPayData;
		}

		public static WxPayData DownloadBill(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string url = "https://api.mch.weixin.qq.com/pay/downloadbill";
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			string text = HttpService.Post(xml, url, false, config, timeOut);
			WxPayData wxPayData = new WxPayData();
			if (text.Substring(0, 5) == "<xml>")
			{
				wxPayData.FromXml(text, config.Key);
			}
			else
			{
				wxPayData.SetValue("result", text);
			}
			return wxPayData;
		}

		public static WxPayData ShortUrl(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string url = "https://api.mch.weixin.qq.com/tools/shorturl";
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, url, false, config, timeOut);
			DateTime now2 = DateTime.Now;
			int num = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			return wxPayData;
		}

		public static WxPayData UnifiedOrder(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
			if (!inputObj.IsSet("NOTIFY_URL"))
			{
				inputObj.SetValue("NOTIFY_URL", config.NOTIFY_URL);
			}
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("spbill_create_ip", config.IPAddress);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, url, false, config, timeOut);
			DateTime now2 = DateTime.Now;
			int num = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			return wxPayData;
		}

		public static WxPayData CloseOrder(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string url = "https://api.mch.weixin.qq.com/pay/closeorder";
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, url, false, config, timeOut);
			DateTime now2 = DateTime.Now;
			int num = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			return wxPayData;
		}

		public static string GenerateOutTradeNo(PayConfig config)
		{
			Random random = new Random();
			return string.Format("{0}{1}{2}", config.MchID, DateTime.Now.ToString("yyyyMMddHHmmss"), random.Next(999));
		}

		public static string GenerateTimeStamp()
		{
			return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
		}

		public static string GenerateNonceStr()
		{
			return Guid.NewGuid().ToString().Replace("-", "");
		}
	}
}
