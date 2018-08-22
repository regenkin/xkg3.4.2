using System;
using System.Web.Security;

namespace Hishop.Weixin.MP.Util
{
	public class CheckSignature
	{
		public static readonly string Token = "weixin_test";

		public static bool Check(string signature, string timestamp, string nonce, string token)
		{
			token = (token ?? CheckSignature.Token);
			string[] array = new string[]
			{
				timestamp,
				nonce,
				token
			};
			Array.Sort<string>(array);
			string text = string.Join("", array);
			text = FormsAuthentication.HashPasswordForStoringInConfigFile(text, "SHA1");
			return signature == text.ToLower();
		}
	}
}
