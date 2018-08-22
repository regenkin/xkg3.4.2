using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Core.Urls;
using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security;

namespace Hidistro.Core
{
	public static class Globals
	{
		public const string UserVerifyCookieName = "Vshop-Member-Verify";

		private static string provOrderid = "";

		private static object Orderidlock = new object();

		private static object LockLog = new object();

		public static string IPAddress
		{
			get
			{
				HttpRequest request = HttpContext.Current.Request;
				string text;
				if (string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
				{
					text = request.ServerVariables["REMOTE_ADDR"];
				}
				else
				{
					text = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				}
				if (string.IsNullOrEmpty(text))
				{
					text = request.UserHostAddress;
				}
				return text;
			}
		}

		public static string ApplicationPath
		{
			get
			{
				string text = "/";
				if (HttpContext.Current != null)
				{
					try
					{
						text = HttpContext.Current.Request.ApplicationPath;
					}
					catch
					{
						text = AppDomain.CurrentDomain.BaseDirectory;
					}
				}
				string result;
				if (text == "/")
				{
					result = string.Empty;
				}
				else
				{
					result = text.ToLower(CultureInfo.InvariantCulture);
				}
				return result;
			}
		}

		public static string DomainName
		{
			get
			{
				string result;
				if (HttpContext.Current == null)
				{
					result = string.Empty;
				}
				else
				{
					result = HttpContext.Current.Request.Url.Host;
				}
				return result;
			}
		}

		public static string GetCurrentWXOpenId
		{
			get
			{
				string result = string.Empty;
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies.Get(Globals.GetCurrentWXOpenIdCookieName());
				if (httpCookie != null)
				{
					result = httpCookie.Value;
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					HttpCookie httpCookie = new HttpCookie(Globals.GetCurrentWXOpenIdCookieName());
					httpCookie.Value = value.Trim();
					httpCookie.Expires = DateTime.Now.AddYears(1);
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
			}
		}

		public static void RefreshWeiXinToken()
		{
			string key = "weixinToken";
			HttpRuntime.Cache.Remove(key);
		}

		public static void SetDistributorCookie(int distributorid)
		{
			HttpCookie httpCookie = new HttpCookie("Vshop-ReferralId");
			httpCookie.Value = distributorid.ToString();
			httpCookie.Expires = DateTime.Now.AddYears(1);
			HttpContext.Current.Response.Cookies.Add(httpCookie);
		}

		public static void ClearReferralIdCookie()
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value) && int.Parse(httpCookie.Value) == 0)
			{
				httpCookie.Value = null;
				httpCookie.Expires = DateTime.Now.AddYears(-1);
				HttpContext.Current.Response.Cookies.Set(httpCookie);
			}
		}

		public static void ClearUserCookie()
		{
			try
			{
				Globals.ClearFWCookie();
				Globals.ClearWXCookie();
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Vshop-Member"];
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					httpCookie.Value = null;
					httpCookie.Expires = DateTime.Now.AddYears(-1);
					HttpContext.Current.Response.Cookies.Set(httpCookie);
				}
				HttpCookie httpCookie2 = HttpContext.Current.Request.Cookies["Vshop-Member-Verify"];
				if (httpCookie2 != null && !string.IsNullOrEmpty(httpCookie2.Value))
				{
					httpCookie2.Value = null;
					httpCookie2.Expires = DateTime.Now.AddYears(-1);
					HttpContext.Current.Response.Cookies.Set(httpCookie2);
				}
				Globals.ClearReferralIdCookie();
			}
			catch
			{
			}
		}

		public static void ClearFWCookie()
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies["fwfollow"];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				httpCookie.Value = null;
				httpCookie.Expires = DateTime.Now.AddYears(-1);
				HttpContext.Current.Response.Cookies.Set(httpCookie);
			}
		}

		public static void ClearWXCookie()
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies["wxfollow"];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				httpCookie.Value = null;
				httpCookie.Expires = DateTime.Now.AddYears(-1);
				HttpContext.Current.Response.Cookies.Set(httpCookie);
			}
			HttpCookie httpCookie2 = HttpContext.Current.Request.Cookies[Globals.GetCurrentWXOpenIdCookieName()];
			if (httpCookie2 != null && !string.IsNullOrEmpty(httpCookie2.Value))
			{
				httpCookie2.Value = null;
				httpCookie2.Expires = DateTime.Now.AddYears(-1);
				HttpContext.Current.Response.Cookies.Set(httpCookie2);
			}
		}

		public static int GetCommitionType()
		{
			return 0;
		}

		public static string GetWebUrlStart()
		{
			Uri url = HttpContext.Current.Request.Url;
			return url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
		}

		public static string GetBarginShow(object bargainDetialId)
		{
			string result = string.Empty;
			if (Globals.ToNum(bargainDetialId) > 0)
			{
				result = "<span class='red' bid='" + bargainDetialId + "'> [砍价]</span>";
			}
			return result;
		}

		public static string ServerIP()
		{
			string text = HttpContext.Current.Request.ServerVariables.Get("Local_Addr").ToString();
			if (text.Length < 7)
			{
				ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
				ManagementObjectCollection instances = managementClass.GetInstances();
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						if ((bool)managementObject["IPEnabled"])
						{
							string[] array = (string[])managementObject["IPAddress"];
							if (array.Length > 0)
							{
								text = array[0];
							}
							break;
						}
					}
				}
			}
			return text;
		}

		public static int GetClientShortType()
		{
			int result = 0;
			string userAgent = HttpContext.Current.Request.UserAgent;
			if (userAgent.ToLower().Contains("alipay"))
			{
				result = 2;
			}
			else if (userAgent.ToLower().Contains("micromessenger"))
			{
				result = 1;
			}
			return result;
		}

		public static int GetPoint(decimal money)
		{
			decimal d = 1m;
			int num = 0;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			int result;
			if (!masterSettings.shopping_score_Enable)
			{
				result = 0;
			}
			else
			{
				if (money * d / masterSettings.PointsRate > 2147483647m)
				{
					num = 2147483647;
				}
				else if (masterSettings.PointsRate != 0m)
				{
					num = (int)Math.Round(money * d / masterSettings.PointsRate, 0);
				}
				if (masterSettings.shopping_reward_Enable && money >= (decimal)masterSettings.shopping_reward_OrderValue)
				{
					num += masterSettings.shopping_reward_Score;
					if (num > 2147483647)
					{
						num = 2147483647;
					}
				}
				result = num;
			}
			return result;
		}

		public static void ValidateSecureConnection(SSLSettings ssl, HttpContext context)
		{
			if (HiConfiguration.GetConfig().SSL == ssl)
			{
				Globals.RedirectToSSL(context);
			}
		}

		public static SiteUrls GetSiteUrls()
		{
			return SiteUrls.Instance();
		}

		public static string PhysicalPath(string path)
		{
			string result;
			if (null == path)
			{
				result = string.Empty;
			}
			else
			{
				result = Globals.RootPath().TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar
				}) + Path.DirectorySeparatorChar.ToString() + path.TrimStart(new char[]
				{
					Path.DirectorySeparatorChar
				});
			}
			return result;
		}

		private static string RootPath()
		{
			string text = AppDomain.CurrentDomain.BaseDirectory;
			string text2 = Path.DirectorySeparatorChar.ToString();
			text = text.Replace("/", text2);
			string text3 = HiConfiguration.GetConfig().FilesPath;
			if (text3 != null)
			{
				text3 = text3.Replace("/", text2);
				if (text3.Length > 0 && text3.StartsWith(text2) && text.EndsWith(text2))
				{
					text += text3.Substring(1);
				}
				else
				{
					text += text3;
				}
			}
			return text;
		}

		public static string MapPath(string path)
		{
			string result;
			if (string.IsNullOrEmpty(path))
			{
				result = string.Empty;
			}
			else
			{
				HttpContext current = HttpContext.Current;
				if (current != null)
				{
					result = current.Request.MapPath(path);
				}
				else
				{
					result = Globals.PhysicalPath(path.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("~", ""));
				}
			}
			return result;
		}

		public static void RedirectToSSL(HttpContext context)
		{
			if (null != context)
			{
				if (!context.Request.IsSecureConnection)
				{
					Uri url = context.Request.Url;
					context.Response.Redirect("https://" + url.ToString().Substring(7));
				}
			}
		}

		public static string AppendQuerystring(string url, string querystring)
		{
			return Globals.AppendQuerystring(url, querystring, false);
		}

		public static string AppendQuerystring(string url, string querystring, bool urlEncoded)
		{
			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentNullException("url");
			}
			string str = "?";
			if (url.IndexOf('?') > -1)
			{
				if (!urlEncoded)
				{
					str = "&";
				}
				else
				{
					str = "&amp;";
				}
			}
			return url + str + querystring;
		}

		public static void EntityCoding(object entity, bool encode)
		{
			if (entity != null)
			{
				Type type = entity.GetType();
				PropertyInfo[] properties = type.GetProperties();
				PropertyInfo[] array = properties;
				for (int i = 0; i < array.Length; i++)
				{
					PropertyInfo propertyInfo = array[i];
					if (propertyInfo.GetCustomAttributes(typeof(HtmlCodingAttribute), true).Length != 0)
					{
						if (!propertyInfo.CanWrite || !propertyInfo.CanRead)
						{
							throw new Exception("使用HtmlEncodeAttribute修饰的属性必须是可读可写的");
						}
						if (!propertyInfo.PropertyType.Equals(typeof(string)))
						{
							throw new Exception("非字符串类型的属性不能使用HtmlEncodeAttribute修饰");
						}
						string text = propertyInfo.GetValue(entity, null) as string;
						if (!string.IsNullOrEmpty(text))
						{
							if (encode)
							{
								propertyInfo.SetValue(entity, Globals.HtmlEncode(text), null);
							}
							else
							{
								propertyInfo.SetValue(entity, Globals.HtmlDecode(text), null);
							}
						}
					}
				}
			}
		}

		public static string HtmlDecode(string textToFormat)
		{
			string result;
			if (string.IsNullOrEmpty(textToFormat))
			{
				result = textToFormat;
			}
			else
			{
				result = HttpUtility.HtmlDecode(textToFormat);
			}
			return result;
		}

		public static string HtmlEncode(string textToFormat)
		{
			string result;
			if (string.IsNullOrEmpty(textToFormat))
			{
				result = textToFormat;
			}
			else
			{
				result = HttpUtility.HtmlEncode(textToFormat);
			}
			return result;
		}

		public static string UrlEncode(string urlToEncode)
		{
			string result;
			if (string.IsNullOrEmpty(urlToEncode))
			{
				result = urlToEncode;
			}
			else
			{
				result = HttpUtility.UrlEncode(urlToEncode, Encoding.UTF8);
			}
			return result;
		}

		public static string UrlDecode(string urlToDecode)
		{
			string result;
			if (string.IsNullOrEmpty(urlToDecode))
			{
				result = urlToDecode;
			}
			else
			{
				result = HttpUtility.UrlDecode(urlToDecode, Encoding.UTF8);
			}
			return result;
		}

		public static string StripScriptTags(string content)
		{
			content = Regex.Replace(content, "<script((.|\n)*?)</script>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			content = Regex.Replace(content, "'javascript:", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			return Regex.Replace(content, "\"javascript:", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}

		public static string StripAllTags(string strToStrip)
		{
			strToStrip = Regex.Replace(strToStrip, "</p(?:\\s*)>(?:\\s*)<p(?:\\s*)>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			strToStrip = Regex.Replace(strToStrip, "<br(?:\\s*)/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			strToStrip = Regex.Replace(strToStrip, "\"", "''", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			strToStrip = Globals.StripHtmlXmlTags(strToStrip);
			return strToStrip;
		}

		public static string StripHtmlXmlTags(string content)
		{
			return Regex.Replace(content, "<[^>]+>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}

		public static string HostPath(Uri uri)
		{
			string result;
			if (uri == null)
			{
				result = string.Empty;
			}
			else
			{
				string text = (uri.Port == 80) ? string.Empty : (":" + uri.Port.ToString(CultureInfo.InvariantCulture));
				result = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[]
				{
					uri.Scheme,
					uri.Host,
					text
				});
			}
			return result;
		}

		public static string FullPath(string local)
		{
			string result;
			if (string.IsNullOrEmpty(local))
			{
				result = local;
			}
			else if (local.ToLower(CultureInfo.InvariantCulture).StartsWith("http://"))
			{
				result = local;
			}
			else if (HttpContext.Current == null)
			{
				result = local;
			}
			else
			{
				result = Globals.FullPath(Globals.HostPath(HttpContext.Current.Request.Url), local);
			}
			return result;
		}

		public static string FullPath(string hostPath, string local)
		{
			return hostPath + local;
		}

		public static string UnHtmlEncode(string formattedPost)
		{
			RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
			formattedPost = Regex.Replace(formattedPost, "&quot;", "\"", options);
			formattedPost = Regex.Replace(formattedPost, "&lt;", "<", options);
			formattedPost = Regex.Replace(formattedPost, "&gt;", ">", options);
			return formattedPost;
		}

		public static string StripForPreview(string content)
		{
			content = Regex.Replace(content, "<br>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			content = Regex.Replace(content, "<br/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			content = Regex.Replace(content, "<br />", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			content = Regex.Replace(content, "<p>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			content = content.Replace("'", "&#39;");
			return Globals.StripHtmlXmlTags(content);
		}

		public static string ToDelimitedString(ICollection collection, string delimiter)
		{
			string result;
			if (collection == null)
			{
				result = string.Empty;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (collection is Hashtable)
				{
					foreach (object current in ((Hashtable)collection).Keys)
					{
						stringBuilder.Append(current.ToString() + delimiter);
					}
				}
				if (collection is ArrayList)
				{
					foreach (object current in ((ArrayList)collection))
					{
						stringBuilder.Append(current.ToString() + delimiter);
					}
				}
				if (collection is string[])
				{
					string[] array = (string[])collection;
					for (int i = 0; i < array.Length; i++)
					{
						string str = array[i];
						stringBuilder.Append(str + delimiter);
					}
				}
				if (collection is MailAddressCollection)
				{
					foreach (MailAddress current2 in ((MailAddressCollection)collection))
					{
						stringBuilder.Append(current2.Address + delimiter);
					}
				}
				result = stringBuilder.ToString().TrimEnd(new char[]
				{
					Convert.ToChar(delimiter, CultureInfo.InvariantCulture)
				});
			}
			return result;
		}

		public static string GetAdminAbsolutePath(string path)
		{
			string result;
			if (path.StartsWith("/"))
			{
				result = Globals.ApplicationPath + "/" + HiConfiguration.GetConfig().AdminFolder + path;
			}
			else
			{
				result = string.Concat(new string[]
				{
					Globals.ApplicationPath,
					"/",
					HiConfiguration.GetConfig().AdminFolder,
					"/",
					path
				});
			}
			return result;
		}

		public static string FormatMoney(decimal money)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}", new object[]
			{
				money.ToString("F", CultureInfo.InvariantCulture)
			});
		}

		public static int[] BubbleSort(int[] r)
		{
			for (int i = 0; i < r.Length; i++)
			{
				bool flag = false;
				for (int j = r.Length - 2; j >= i; j--)
				{
					if (r[j + 1] > r[j])
					{
						int num = r[j + 1];
						r[j + 1] = r[j];
						r[j] = num;
						flag = true;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			return r;
		}

		public static string GetGenerateId()
		{
			string result;
			lock (Globals.Orderidlock)
			{
				string text = DateTime.Now.ToString("yyMMddHHmmssfff");
				if (text == Globals.provOrderid)
				{
					Thread.Sleep(1);
					text = DateTime.Now.ToString("yyMMddHHmmssfff");
				}
				Globals.provOrderid = text;
				result = text;
			}
			return result;
		}

		public static int GetCurrentMemberUserId()
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies.Get("Vshop-Member");
			int result;
			if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
			{
				result = 0;
			}
			else
			{
				HttpCookie httpCookie2 = HttpContext.Current.Request.Cookies.Get("Vshop-Member-Verify");
				if (httpCookie2 == null || string.IsNullOrEmpty(httpCookie2.Value))
				{
					result = 0;
				}
				else
				{
					int num = 0;
					if (Globals.EncryptStr(httpCookie.Value) == httpCookie2.Value)
					{
						int.TryParse(httpCookie.Value, out num);
					}
					result = num;
				}
			}
			return result;
		}

		public static string EncryptStr(string str)
		{
			string sKey = "hishoptk";
			string text = ConfigurationManager.AppSettings["IV"];
			if (!string.IsNullOrEmpty(text) && text.Length >= 8)
			{
				sKey = text.Substring(0, 8);
			}
			return HiCryptographer.Md5Encrypt(DesSecurity.DesEncrypt(str, sKey));
		}

		public static string GetCurrentWXOpenIdCookieName()
		{
			return "xkdopenidv304";
		}

		public static int GetCurrentDistributorId()
		{
			int num = 0;
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies.Get("Vshop-ReferralId");
			int result;
			if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
			{
				result = 0;
			}
			else
			{
				int.TryParse(httpCookie.Value, out num);
				result = num;
			}
			return result;
		}

		public static int GetCurrentManagerUserId()
		{
			int num = 0;
			bool flag = false;
			return Globals.GetCurrentManagerUserId(out num, out flag);
		}

		public static int GetCurrentManagerUserId(out int roleId, out bool isDefault)
		{
			roleId = 0;
			isDefault = false;
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies.Get(string.Format("{0}{1}", Globals.DomainName, FormsAuthentication.FormsCookieName));
			int result;
			if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
			{
				result = 0;
			}
			else
			{
				int num = 0;
				FormsAuthenticationTicket formsAuthenticationTicket = FormsAuthentication.Decrypt(httpCookie.Value);
				try
				{
					int.TryParse(formsAuthenticationTicket.Name, out num);
					string[] array = formsAuthenticationTicket.UserData.Split(new char[]
					{
						'_'
					});
					int.TryParse(array[0], out roleId);
					bool.TryParse(array[1], out isDefault);
					result = num;
				}
				catch (Exception)
				{
					result = 0;
				}
			}
			return result;
		}

		public static string CreateVerifyCode(int length)
		{
			string text = string.Empty;
			Random random = new Random();
			while (text.Length < length)
			{
				int num = random.Next();
				char c;
				if (num % 3 == 0)
				{
					c = (char)(97 + (ushort)(num % 26));
				}
				else if (num % 4 == 0)
				{
					c = (char)(65 + (ushort)(num % 26));
				}
				else
				{
					c = (char)(48 + (ushort)(num % 10));
				}
				if (c != '0' && c != 'o' && c != '1' && c != '7' && c != 'l' && c != '9' && c != 'g' && c != 'I')
				{
					text += c.ToString();
				}
			}
			Globals.RemoveVerifyCookie();
			HttpCookie httpCookie = new HttpCookie("VerifyCode");
			httpCookie.Value = HiCryptographer.Encrypt(text);
			HttpContext.Current.Response.Cookies.Add(httpCookie);
			return text;
		}

		public static bool CheckVerifyCode(string verifyCode)
		{
			bool result;
			if (HttpContext.Current.Request.Cookies["VerifyCode"] == null)
			{
				Globals.RemoveVerifyCookie();
				result = false;
			}
			else
			{
				bool flag = string.Compare(HiCryptographer.Decrypt(HttpContext.Current.Request.Cookies["VerifyCode"].Value), verifyCode, true, CultureInfo.InvariantCulture) == 0;
				Globals.RemoveVerifyCookie();
				result = flag;
			}
			return result;
		}

		private static void RemoveVerifyCookie()
		{
			HttpContext.Current.Response.Cookies["VerifyCode"].Value = null;
			HttpContext.Current.Response.Cookies["VerifyCode"].Expires = new DateTime(1911, 10, 12);
		}

		public static string GetStoragePath()
		{
			return "/Storage/master";
		}

		public static string GetVshopSkinPath(string themeName = null)
		{
			return (Globals.ApplicationPath + "/Templates/common").ToLower(CultureInfo.InvariantCulture);
		}

		public static bool IsOrdersID(string lstr)
		{
			string pattern = "^(\\d{15}|\\d{19})(?:-\\d+)?$";
			return !string.IsNullOrEmpty(lstr) && Regex.IsMatch(lstr, pattern);
		}

		public static bool IsNumeric(string lstr)
		{
			return !string.IsNullOrEmpty(lstr) && Regex.IsMatch(lstr, "^\\d+(\\.)?\\d*$");
		}

		public static bool isUnsignedInteger(string lstr)
		{
			return !string.IsNullOrEmpty(lstr) && Regex.IsMatch(lstr, "^([1-9]//d*|0)$");
		}

		public static int RequestFormNum(string sTemp)
		{
			string s = HttpContext.Current.Request.Form[sTemp];
			return Globals.ToNum(s);
		}

		public static string RequestFormStr(string sTemp)
		{
			string text = HttpContext.Current.Request.Form[sTemp];
			string result;
			if (string.IsNullOrEmpty(text))
			{
				result = "";
			}
			else
			{
				result = text.Trim();
			}
			return result;
		}

		public static int ToNum(object s)
		{
			string text = (s == null) ? "0" : s.ToString();
			int result;
			if (!string.IsNullOrEmpty(text) && Globals.IsNumeric(text))
			{
				try
				{
					result = Convert.ToInt32(text);
					return result;
				}
				catch
				{
					result = 0;
					return result;
				}
			}
			result = 0;
			return result;
		}

		public static int RequestQueryNum(string sTemp)
		{
			string s = HttpContext.Current.Request.QueryString[sTemp];
			return Globals.ToNum(s);
		}

		public static decimal RequestQueryDecimal(string sTemp)
		{
			decimal result = 0m;
			string s = HttpContext.Current.Request.QueryString[sTemp];
			decimal.TryParse(s, out result);
			return result;
		}

		public static string RequestQueryStr(string sTemp)
		{
			string text = HttpContext.Current.Request.QueryString[sTemp];
			string result;
			if (string.IsNullOrEmpty(text))
			{
				result = "";
			}
			else
			{
				result = text.Trim();
			}
			return result;
		}

		public static bool CheckReg(string str, string reg)
		{
			return Regex.IsMatch(str, reg);
		}

		public static string LinkUrl(string url)
		{
			string result;
			if (url.Contains("?"))
			{
				result = url + "&ReferralId=" + Globals.RequestQueryStr("UserId");
			}
			else
			{
				result = url + "?ReferralId=" + Globals.RequestQueryStr("UserId");
			}
			return result;
		}

		public static int GetStrLen(string strData)
		{
			Encoding encoding = Encoding.GetEncoding("GB2312");
			return encoding.GetByteCount(strData);
		}

		public static string SubStr(string s, int i, string smore)
		{
			int num = 0;
			int num2 = 0;
			string result;
			if (Globals.GetStrLen(s) > i)
			{
				for (int j = 0; j < s.Length; j++)
				{
					char c = s[j];
					if (num >= i)
					{
						break;
					}
					num2++;
					if (c > '\u007f')
					{
						num += 2;
					}
					else
					{
						num++;
					}
				}
				string str = s.Substring(0, num2 - Globals.GetStrLen(smore));
				result = str + smore;
			}
			else
			{
				result = s;
			}
			return result;
		}

		public static string String2Json(string s)
		{
			string result;
			if (string.IsNullOrEmpty(s))
			{
				result = "";
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				int i = 0;
				while (i < s.Length)
				{
					char c = s.ToCharArray()[i];
					char c2 = c;
					if (c2 <= '"')
					{
						switch (c2)
						{
						case '\b':
							stringBuilder.Append("\\b");
							break;
						case '\t':
							stringBuilder.Append("\\t");
							break;
						case '\n':
							stringBuilder.Append("\\n");
							break;
						case '\v':
							goto IL_E3;
						case '\f':
							stringBuilder.Append("\\f");
							break;
						case '\r':
							stringBuilder.Append("\\r");
							break;
						default:
							if (c2 != '"')
							{
								goto IL_E3;
							}
							stringBuilder.Append("\\\"");
							break;
						}
					}
					else if (c2 != '/')
					{
						if (c2 != '\\')
						{
							goto IL_E3;
						}
						stringBuilder.Append("\\\\");
					}
					else
					{
						stringBuilder.Append("\\/");
					}
					IL_ED:
					i++;
					continue;
					IL_E3:
					stringBuilder.Append(c);
					goto IL_ED;
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		public static void Debuglog(string log, string logname = "_Debuglog.txt")
		{
			lock (Globals.LockLog)
			{
				try
				{
					string str = DateTime.Now.ToString("yyyyMMdd") + logname;
					string path = HttpRuntime.AppDomainAppPath.ToString() + "App_Data/" + str;
					StreamWriter streamWriter = File.AppendText(path);
					streamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + log);
					streamWriter.WriteLine("---------------");
					streamWriter.Close();
				}
				catch (Exception var_3_88)
				{
				}
			}
		}
	}
}
