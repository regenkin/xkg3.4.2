using System;

namespace Hishop.Weixin.Pay.Domain
{
	public class SendRedPackInfo
	{
		public int SendRedpackRecordID
		{
			get;
			set;
		}

		public string PartnerKey
		{
			get;
			set;
		}

		public string Mch_Id
		{
			get;
			set;
		}

		public string Sub_Mch_Id
		{
			get;
			set;
		}

		public string Main_AppId
		{
			get;
			set;
		}

		public string Main_Mch_ID
		{
			get;
			set;
		}

		public string Main_PayKey
		{
			get;
			set;
		}

		public bool EnableSP
		{
			get;
			set;
		}

		public string WXAppid
		{
			get;
			set;
		}

		public string Nick_Name
		{
			get;
			set;
		}

		public string Send_Name
		{
			get;
			set;
		}

		public string Re_Openid
		{
			get;
			set;
		}

		public int Total_Amount
		{
			get;
			set;
		}

		public int Total_Num
		{
			get;
			set;
		}

		public string Wishing
		{
			get;
			set;
		}

		public string Client_IP
		{
			get;
			set;
		}

		public string Act_Name
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public string WeixinCertPath
		{
			get;
			set;
		}

		public string WeixinCertPassword
		{
			get;
			set;
		}

		public string Mch_BillNo
		{
			get;
			set;
		}

		public SendRedPackInfo()
		{
			this.Total_Num = 1;
		}
	}
}
