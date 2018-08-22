using Hishop.Weixin.MP.Util;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Web;

namespace Hishop.Weixin.MP.Api
{
	public class BarCodeApi
	{
		public static string CreateTicket(string TOKEN, string scene_id = "12399", string QRType = "QR_LIMIT_SCENE", string exSecond = "2592000")
		{
			string value = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\":" + scene_id + "}}}";
			if (QRType == "QR_SCENE")
			{
				value = string.Concat(new string[]
				{
					"{\"expire_seconds\":",
					exSecond,
					", \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\":",
					scene_id,
					"}}}"
				});
			}
			string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + TOKEN;
			string value2 = new WebUtils().DoPost(url, value);
			var type = new
			{
				ticket = "",
				url = ""
			};
			string text = JsonConvert.SerializeObject(type);
            return JsonConvert.DeserializeAnonymousType(value2, type).ticket;
        }

		public static string GetQRImageUrlByTicket(string TICKET)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			string empty3 = string.Empty;
			return "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + HttpUtility.UrlEncode(TICKET, Encoding.UTF8);
		}

		public static bool GetHeadImageUrlByOpenID(string TOKEN, string OpenID, out string RetInfo, out string NickName, out string HeadImageUrl)
		{
			NickName = "";
			HeadImageUrl = "";
			RetInfo = "";
			bool result;
			if (string.IsNullOrEmpty(OpenID))
			{
				RetInfo = "{\"errcode\":40013,\"errmsg\":\"openId为空\"}";
				result = false;
			}
			else
			{
				string url = string.Concat(new string[]
				{
					"https://api.weixin.qq.com/cgi-bin/user/info?access_token=",
					TOKEN,
					"&openid=",
					OpenID,
					"&lang=zh_CN"
				});
				string text = new WebUtils().DoGet(url, null);
				if (text.Contains("errcode"))
				{
					result = false;
				}
				else
				{
					var type = new
					{
						subscribe = "",
						nickname = "",
						headimgurl = ""
					};
					string text2 = JsonConvert.SerializeObject(type);
					type = JsonConvert.DeserializeAnonymousType(text, type);
					NickName = type.nickname;
					HeadImageUrl = type.headimgurl;
					if (type.subscribe.Trim() != "1")
					{
						RetInfo = "此用户未关注当前公众号，无法拉取信息。";
					}
					result = (type.subscribe.Trim() == "1");
				}
			}
			return result;
		}

		public static string GetUserInfosByOpenID(string TOKEN, string OpenID)
		{
			string result;
			if (string.IsNullOrEmpty(OpenID))
			{
				result = "OpenID为空。";
			}
			else
			{
				string url = string.Concat(new string[]
				{
					"https://api.weixin.qq.com/cgi-bin/user/info?access_token=",
					TOKEN,
					"&openid=",
					OpenID,
					"&lang=zh_CN"
				});
				result = new WebUtils().DoGet(url, null);
			}
			return result;
		}
	}
}
