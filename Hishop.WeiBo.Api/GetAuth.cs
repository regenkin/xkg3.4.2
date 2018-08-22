using System;

namespace Hishop.WeiBo.Api
{
	public class GetAuth
	{
		public SinaWeiboClient GetOpenAuthClient(string accessToken)
		{
			string appKey = "";
			string appSecret = "";
			string callbackUrl = "";
			string uid = "";
			return new SinaWeiboClient(appKey, appSecret, callbackUrl, accessToken, uid);
		}
	}
}
