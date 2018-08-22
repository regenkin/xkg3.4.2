using System;
using System.Collections.Generic;
using System.IO;

namespace Hishop.AlipayFuwu.Api.Model
{
	public class AlipayFuwuConfig
	{
		public static string alipay_public_key = "";

		public static string merchant_private_key = "";

		public static string merchant_public_key = "";

		public static string charset = "GBK";

		public static string appId = "";

		public static bool writeLog = false;

		public static string serverUrl = "https://openapi.alipay.com/gateway.do";

		public static string errstr = "";

		public static System.Collections.Generic.Dictionary<string, string> BindAdmin = new System.Collections.Generic.Dictionary<string, string>();

		public static void SetConfig(string pubkeyfilepath, string privatefilepath, string alipay_public_keypath, string _appId, string _charset = "GBK")
		{
			AlipayFuwuConfig.merchant_private_key = privatefilepath;
			AlipayFuwuConfig.merchant_public_key = pubkeyfilepath;
			AlipayFuwuConfig.alipay_public_key = alipay_public_keypath;
			AlipayFuwuConfig.appId = _appId;
			AlipayFuwuConfig.charset = _charset;
		}

		public static bool CommSetConfig(string _appId, string HostPath, string _charset = "GBK")
		{
			AlipayFuwuConfig.merchant_private_key = HostPath + "\\config\\rsa_private_key.pem";
			AlipayFuwuConfig.alipay_public_key = HostPath + "\\config\\alipay_pubKey.pem";
			AlipayFuwuConfig.merchant_public_key = HostPath + "\\config\\rsa_public_key.pem";
			bool result = false;
			if (_appId.Length > 15 && System.IO.File.Exists(AlipayFuwuConfig.merchant_private_key) && System.IO.File.Exists(AlipayFuwuConfig.merchant_private_key) && System.IO.File.Exists(AlipayFuwuConfig.merchant_private_key))
			{
				AlipayFuwuConfig.appId = _appId;
				AlipayFuwuConfig.charset = _charset;
				AlipayFuwuConfig.errstr = "";
				result = true;
			}
			else
			{
				AlipayFuwuConfig.merchant_private_key = "";
				AlipayFuwuConfig.alipay_public_key = "";
				AlipayFuwuConfig.merchant_public_key = "";
				AlipayFuwuConfig.errstr = "服务窗参数配置错误！";
			}
			return result;
		}

		public static void SetWriteLog(bool _writeLog)
		{
			AlipayFuwuConfig.writeLog = _writeLog;
		}
	}
}
