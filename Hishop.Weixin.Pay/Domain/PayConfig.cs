using System;

namespace Hishop.Weixin.Pay.Domain
{
	public class PayConfig
	{
		public string PROXY_URL = "http://10.152.18.220:8080";

		public string AppId
		{
			get;
			set;
		}

		public string AppSecret
		{
			get;
			set;
		}

		public string MchID
		{
			get;
			set;
		}

		public string Key
		{
			get;
			set;
		}

		public string OpenId
		{
			get;
			set;
		}

		public string sub_appid
		{
			get;
			set;
		}

		public string sub_mch_id
		{
			get;
			set;
		}

		public string IPAddress
		{
			get;
			set;
		}

		public string SignType
		{
			get;
			private set;
		}

		public string SSLCERT_PATH
		{
			get;
			set;
		}

		public string SSLCERT_PASSWORD
		{
			get;
			set;
		}

		public string NOTIFY_URL
		{
			get;
			set;
		}

		public int REPORT_LEVENL
		{
			get;
			set;
		}

		public int LOG_LEVENL
		{
			get;
			set;
		}

		public PayConfig()
		{
			this.IPAddress = "127.0.0.1";
			this.SignType = "MD5";
			this.REPORT_LEVENL = 0;
			this.LOG_LEVENL = 0;
			this.SSLCERT_PATH = "";
			this.SSLCERT_PASSWORD = "";
		}
	}
}
