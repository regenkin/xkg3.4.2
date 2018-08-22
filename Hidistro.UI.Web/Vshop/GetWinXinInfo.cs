using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web.Vshop
{
	public class GetWinXinInfo : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string return_url = Globals.RequestQueryStr("return_url");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			this.GetWeiXinInfoByAuth(masterSettings, return_url);
			base.Response.Write(System.Web.HttpContext.Current.Request.Url.ToString());
		}

		private string GetResponseResult(string url)
		{
			System.Net.WebRequest webRequest = System.Net.WebRequest.Create(url);
			string result;
			using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webRequest.GetResponse())
			{
				using (System.IO.Stream responseStream = httpWebResponse.GetResponseStream())
				{
					using (System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}
			return result;
		}

		public void GetWeiXinInfoByAuth(SiteSettings site, string return_url)
		{
			string text = this.Page.Request.QueryString["code"];
			if (!string.IsNullOrEmpty(text))
			{
				string responseResult = this.GetResponseResult(string.Concat(new string[]
				{
					"https://api.weixin.qq.com/sns/oauth2/access_token?appid=",
					site.WeixinAppId,
					"&secret=",
					site.WeixinAppSecret,
					"&code=",
					text,
					"&grant_type=authorization_code"
				}));
				if (!responseResult.Contains("access_token"))
				{
					base.Response.Redirect(return_url);
					return;
				}
				JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
				jObject["openid"].ToString();
				string responseResult2 = this.GetResponseResult(string.Concat(new string[]
				{
					"https://api.weixin.qq.com/sns/userinfo?access_token=",
					jObject["access_token"].ToString(),
					"&openid=",
					jObject["openid"].ToString(),
					"&lang=zh_CN"
				}));
				if (responseResult2.Contains("nickname"))
				{
					JObject jObject2 = JsonConvert.DeserializeObject(responseResult2) as JObject;
					if (MemberHelper.SetUserHeadAndUserName(jObject2["openid"].ToString(), jObject2["headimgurl"].ToString(), Globals.UrlDecode(jObject2["nickname"].ToString())))
					{
						Globals.Debuglog("重写成功！" + responseResult2, "_Debuglog.txt");
					}
					base.Response.Redirect(return_url);
					return;
				}
				base.Response.Redirect(return_url);
				return;
			}
			else
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
				{
					base.Response.Redirect("用户拒绝授权");
					return;
				}
				string text2 = "snsapi_userinfo";
				string url = string.Concat(new string[]
				{
					"https://open.weixin.qq.com/connect/oauth2/authorize?appid=",
					site.WeixinAppId,
					"&redirect_uri=",
					Globals.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString().Replace(":" + System.Web.HttpContext.Current.Request.Url.Port, "")),
					"&response_type=code&scope=",
					text2,
					"&state=STATE#wechat_redirect"
				});
				this.Page.Response.Redirect(url);
				return;
			}
		}
	}
}
