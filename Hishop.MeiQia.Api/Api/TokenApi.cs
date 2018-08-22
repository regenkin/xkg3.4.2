using Hishop.MeiQia.Api.Domain;
using Hishop.MeiQia.Api.Util;
using System;
using System.Web.Script.Serialization;

namespace Hishop.MeiQia.Api.Api
{
	public class TokenApi
	{
		public static string GetTokenValue(string appid, string secret)
		{
			string url = string.Format("http://open.meiqia.com/cgi-bin/token?grant_type=client_credential&&appid={0}&secret={1}", appid, secret);
			string text = new WebUtils().DoGet(url, null);
			string result;
			if (text.Contains("access_token"))
			{
				result = new JavaScriptSerializer().Deserialize<Token>(text).access_token;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		public static string GetToken(string appid, string secret)
		{
			string url = string.Format("http://open.meiqia.com/cgi-bin/token?grant_type=client_credential&&appid={0}&secret={1}", appid, secret);
			return new WebUtils().DoGet(url, null);
		}
	}
}
