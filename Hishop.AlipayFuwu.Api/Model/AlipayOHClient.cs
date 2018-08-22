using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using System;
using System.Web;

namespace Hishop.AlipayFuwu.Api.Model
{
	public class AlipayOHClient
	{
		private const string APP_ID = "app_id";

		private const string FORMAT = "format";

		private const string METHOD = "method";

		private const string TIMESTAMP = "timestamp";

		private const string VERSION = "version";

		private const string SIGN_TYPE = "sign_type";

		private const string ACCESS_TOKEN = "auth_token";

		private const string SIGN = "sign";

		private const string TERMINAL_TYPE = "terminal_type";

		private const string TERMINAL_INFO = "terminal_info";

		private const string PROD_CODE = "prod_code";

		private const string BIZ_CONTENT = "biz_content";

		private const string SING = "sign";

		private const string CONTENT = "biz_content";

		private const string SING_TYPE = "sign_type";

		private const string SERVICE = "service";

		private const string CHARSET = "charset";

		private string version;

		private string format;

		private string signType = "RSA";

		private string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";

		private string serverUrl;

		private string appId;

		private string privateKey;

		private string pubKey;

		private string aliPubKey;

		private string charset;

		private WebUtils webUtils = new WebUtils();

		private HttpContext context;

		public AlipayOHClient(string url, string appId, string aliPubKey, string priKey, string pubKey, string charset = "UTF-8")
		{
			this.serverUrl = url;
			this.appId = appId;
			this.privateKey = priKey;
			this.charset = charset;
			this.pubKey = pubKey;
			this.aliPubKey = aliPubKey;
		}

		public AlipaySystemOauthTokenResponse OauthTokenRequest(string authCode)
		{
			AlipaySystemOauthTokenRequest alipaySystemOauthTokenRequest = new AlipaySystemOauthTokenRequest();
			alipaySystemOauthTokenRequest.GrantType = "authorization_code";
			alipaySystemOauthTokenRequest.Code = authCode;
			AlipaySystemOauthTokenResponse result = null;
			try
			{
				IAopClient aopClient = new DefaultAopClient(this.serverUrl, this.appId, this.privateKey);
				result = aopClient.Execute<AlipaySystemOauthTokenResponse>(alipaySystemOauthTokenRequest);
			}
			catch (AopException var_3_41)
			{
			}
			return result;
		}

		public AlipayUserUserinfoShareResponse GetAliUserInfo(string accessToken)
		{
			AlipayUserUserinfoShareRequest request = new AlipayUserUserinfoShareRequest();
			IAopClient aopClient = new DefaultAopClient(this.serverUrl, this.appId, this.privateKey);
			return aopClient.Execute<AlipayUserUserinfoShareResponse>(request, accessToken);
		}
	}
}
