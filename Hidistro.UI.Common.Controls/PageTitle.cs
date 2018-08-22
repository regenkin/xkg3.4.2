using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hishop.Weixin.MP.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(false), PersistChildren(true)]
	public class PageTitle : Control
	{
		private const string titleKey = "Hishop.Title.Value";

		private const string descKey = "Hishop.Desc.Value";

		private string _GetTokenError = "";

		public static void AddTitle(string title)
		{
			if (HttpContext.Current == null)
			{
				throw new ArgumentNullException("context");
			}
			HttpContext.Current.Items["Hishop.Title.Value"] = title;
		}

		public static void AddDescrption(string desc)
		{
			if (HttpContext.Current == null)
			{
				throw new ArgumentNullException("context");
			}
			HttpContext.Current.Items["Hishop.Desc.Value"] = desc;
		}

		public static void AddSiteNameTitle(string title)
		{
			PageTitle.AddTitle(string.Format(CultureInfo.InvariantCulture, "{0}", new object[]
			{
				title
			}));
		}

		public static void AddSiteDescription(string desc)
		{
			PageTitle.AddDescrption(string.Format(CultureInfo.InvariantCulture, "{0}", new object[]
			{
				desc
			}));
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				string value = httpCookie.Value;
				DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(Globals.ToNum(value));
				if (distributorInfo != null && distributorInfo.ReferralStatus != 9)
				{
					text = distributorInfo.StoreName;
					text2 = distributorInfo.StoreDescription;
					text3 = distributorInfo.Logo;
				}
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			if (string.IsNullOrEmpty(text))
			{
				text = masterSettings.SiteName;
				text2 = masterSettings.ShopIntroduction;
				text3 = masterSettings.DistributorLogoPic;
			}
			string text4 = this.Context.Items["Hishop.Title.Value"] as string;
			string text5 = this.Context.Items["Hishop.Desc.Value"] as string;
			if (string.IsNullOrEmpty(text5))
			{
				text5 = text2;
			}
			if (string.IsNullOrEmpty(text4))
			{
				writer.WriteLine("<title>{0}</title>", text);
			}
			else
			{
				writer.WriteLine("<title>{0}</title>", text4 + " - " + text);
				writer.WriteLine("<meta name=\"keywords\" content=\"{0}\" />", text4);
			}
			writer.WriteLine("<meta name=\"description\" content=\"{0}\" />", text5);
			string userAgent = this.Page.Request.UserAgent;
			if (userAgent.ToLower().Contains("micromessenger") || Globals.RequestQueryNum("istest") == 1)
			{
				Uri url = HttpContext.Current.Request.Url;
				string text6 = url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
				if (!text3.StartsWith("http"))
				{
					text3 = text6 + text3;
				}
				string text7 = this.ConvertDateTimeInt(DateTime.Now).ToString();
				string empty = string.Empty;
				string text8 = "QoN4FvGbxdTi7mnffL";
				string cacheToken = this.GetCacheToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
				string signature = this.GetSignature(cacheToken, text7, text8, out empty);
				writer.WriteLine(string.Concat(new object[]
				{
					"<script src=\"http://res.wx.qq.com/open/js/jweixin-1.0.0.js\"></script><script>wx.config({ debug: false,appId: '",
					masterSettings.WeixinAppId,
					"',timestamp: '",
					text7,
					"', nonceStr: '",
					text8,
					"',signature: '",
					signature,
					"',jsApiList: ['checkJsApi','onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo','chooseWXPay']});var _GetTokenError='",
					this._GetTokenError,
					"'; var wxinshare_title='",
					HttpContext.Current.Server.HtmlEncode(text.Replace("\n", " ").Replace("\r", "")),
					"';var wxinshare_desc='",
					HttpContext.Current.Server.HtmlEncode(text2.Replace("\n", " ").Replace("\r", "")),
					"';var wxinshare_link='",
					text6,
					"/default.aspx?ReferralId=",
					Globals.GetCurrentDistributorId(),
					"';var fxShopName='",
					HttpContext.Current.Server.HtmlEncode(text.Replace("\n", " ").Replace("\r", "")),
					"';var wxinshare_imgurl='",
					text3,
					"'</script><script src=\"/templates/common/script/WeiXinShare.js?2016\"></script>"
				}));
				return;
			}
			writer.WriteLine("<script>var _GetTokenError=''; var wxinshare_title='';var wxinshare_desc='';var wxinshare_link='';var wxinshare_imgurl='';;var fxShopName=''</script>");
		}

		private string GetCacheToken(string appid, string secret)
		{
			string text = TokenApi.GetToken_Message(appid, secret);
			if (!string.IsNullOrEmpty(text) && text.Contains("errmsg") && text.Contains("errcode"))
			{
				Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
				if (dictionary != null && dictionary.ContainsKey("errcode") && dictionary.ContainsKey("errmsg"))
				{
					this._GetTokenError = dictionary["errcode"] + "|" + dictionary["errmsg"];
				}
				else
				{
					this._GetTokenError = "";
				}
			}
			else if (string.IsNullOrEmpty(text))
			{
				text = "";
				this._GetTokenError = "获取令牌失败";
			}
			return text;
		}

		private string GetSignature(string token, string timestamp, string nonce, out string str)
		{
			string str2 = this.Page.Request.Url.ToString().Replace(":" + this.Page.Request.Url.Port.ToString(), "");
			string jsApi_ticket = this.GetJsApi_ticket(token);
			string text = "jsapi_ticket=" + jsApi_ticket;
			string text2 = "noncestr=" + nonce;
			string text3 = "timestamp=" + timestamp;
			string text4 = "url=" + str2;
			string[] value = new string[]
			{
				text,
				text2,
				text3,
				text4
			};
			str = string.Join("&", value);
			string text5 = str;
			text5 = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
			return text5.ToLower();
		}

		public string GetJsApi_ticket(string token)
		{
			string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", token);
			string value = this.DoGet(url);
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
			if (dictionary != null && dictionary.ContainsKey("ticket"))
			{
				return dictionary["ticket"];
			}
			return string.Empty;
		}

		public string DoGet(string url)
		{
			HttpWebRequest webRequest = this.GetWebRequest(url, "GET");
			webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
			string result;
			try
			{
				HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
				Encoding uTF = Encoding.UTF8;
				result = this.GetResponseAsString(rsp, uTF);
			}
			catch (Exception ex)
			{
				Globals.Debuglog("获取信息ticket错误：" + ex.Message, "_Debuglog.txt");
				result = string.Empty;
			}
			return result;
		}

		public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
		{
			Stream stream = null;
			StreamReader streamReader = null;
			string result;
			try
			{
				stream = rsp.GetResponseStream();
				streamReader = new StreamReader(stream, encoding);
				result = streamReader.ReadToEnd();
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
				if (stream != null)
				{
					stream.Close();
				}
				if (rsp != null)
				{
					rsp.Close();
				}
			}
			return result;
		}

		private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		private HttpWebRequest GetWebRequest(string url, string method)
		{
			int timeout = 100000;
			HttpWebRequest httpWebRequest;
			if (url.Contains("https"))
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
				httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
			}
			else
			{
				httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			}
			httpWebRequest.ServicePoint.Expect100Continue = false;
			httpWebRequest.Method = method;
			httpWebRequest.KeepAlive = true;
			httpWebRequest.UserAgent = "Hishop";
			httpWebRequest.Timeout = timeout;
			return httpWebRequest;
		}

		private int ConvertDateTimeInt(DateTime time)
		{
			DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			return (int)(time - d).TotalSeconds;
		}
	}
}
