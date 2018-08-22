using System;

namespace Hishop.Weixin.Pay
{
	public class PayAccount
	{
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

		public bool EnableSP
		{
			get;
			set;
		}

		public string Sub_appid
		{
			get;
			set;
		}

		public string Sub_mch_id
		{
			get;
			set;
		}

		public string PartnerId
		{
			get;
			set;
		}

		public string PartnerKey
		{
			get;
			set;
		}

		public PayAccount()
		{
		}

		public PayAccount(string appId, string appSecret, string partnerId, string partnerKey, bool enableSP, string sub_appid, string sub_mch_id)
		{
			this.AppId = appId;
			this.AppSecret = appSecret;
			this.PartnerId = partnerId;
			this.PartnerKey = partnerKey;
			this.Sub_appid = sub_appid;
			this.Sub_mch_id = sub_mch_id;
			this.EnableSP = enableSP;
		}
	}
}
