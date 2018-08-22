using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Util;
using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Serialization;

namespace Hishop.Weixin.MP.Api
{
	public class TokenApi
	{
		private static object lockobj = new object();

		public string AppId
		{
			get
			{
				return ConfigurationManager.AppSettings.Get("AppId");
			}
		}

		public string AppSecret
		{
			get
			{
				return ConfigurationManager.AppSettings.Get("AppSecret");
			}
		}

		public static string GetToken_Message(string appid, string secret)
		{
			string text = TokenApi.GetToken(appid, secret);
			if (text.Contains("access_token"))
			{
				text = new JavaScriptSerializer().Deserialize<Token>(text).access_token;
			}
			return text;
		}

		public static string GetToken(string appid, string secret)
		{
			string text = string.Empty;
			int num = 600;
			text = (HttpRuntime.Cache.Get("weixinToken") as string);
			if (string.IsNullOrEmpty(text))
			{
				lock (TokenApi.lockobj)
				{
					if (string.IsNullOrEmpty(text))
					{
						string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
						text = new WebUtils().DoGet(url, null);
						HttpRuntime.Cache.Insert("weixinToken", text, null, DateTime.Now.AddSeconds((double)num), Cache.NoSlidingExpiration);
					}
				}
			}
			return text;
		}

		public static bool CheckIsRightToken(string Token)
		{
			bool result = true;
			if (Token.Contains("errcode") || Token.Contains("errmsg"))
			{
				result = false;
			}
			return result;
		}
	}
}
