using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Web.Caching;

namespace Hidistro.ControlPanel.Store
{
	public static class SystemAuthorizationHelper
	{
		private static readonly string authorizationUrl = "http://ysc.kuaidiantong.cn/wfxvalid.ashx";

		public static readonly string noticeMsg = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body>\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的系统已过授权有效期，无法登录后台管理。请续费。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.ApplicationPath + "/Default.aspx\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2015 pufang.net all Rights Reserved. 本产品资源均为 Hishop 版权所有</div>\r\n</div>\r\n</body>\r\n</html>";

		public static readonly string licenseMsg = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body>\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的系统未经官方授权，无法登录后台管理。请联系官方购买软件使用权。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.ApplicationPath + "/Default.aspx\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2015 pufang.net all Rights Reserved. 本产品资源均为 Hishop 版权所有</div>\r\n</div>\r\n</body>\r\n</html>";

		public static SystemAuthorizationInfo GetSystemAuthorization(bool iscreate)
		{
			string key = "DataCache-SystemAuthorizationInfo";
			SystemAuthorizationInfo systemAuthorizationInfo = HiCache.Get(key) as SystemAuthorizationInfo;
			if (systemAuthorizationInfo == null || iscreate)
			{
				string value = SystemAuthorizationHelper.PostData(SystemAuthorizationHelper.authorizationUrl, "host=" + Globals.DomainName);
				if (!string.IsNullOrEmpty(value))
				{
					TempAuthorizationInfo tempAuthorizationInfo = JsonConvert.DeserializeObject<TempAuthorizationInfo>(value);
					systemAuthorizationInfo = new SystemAuthorizationInfo
					{
						state = (SystemAuthorizationState)tempAuthorizationInfo.state,
						DistributorCount = tempAuthorizationInfo.count,
						type = tempAuthorizationInfo.type,
						IsShowJixuZhiChi = tempAuthorizationInfo.isshowjszc == "1"
					};
					HiCache.Insert(key, systemAuthorizationInfo, 360, CacheItemPriority.Normal);
				}
			}
			return systemAuthorizationInfo;
		}

		public static bool CheckDistributorIsCanAuthorization()
		{
			int num = 0;
			return SystemAuthorizationHelper.CheckDistributorIsCanAuthorization(1, out num);
		}

		public static bool CheckDistributorIsCanAuthorization(int number, out int leftNumber)
		{
			leftNumber = 0;
			SystemAuthorizationInfo systemAuthorization = SystemAuthorizationHelper.GetSystemAuthorization(false);
			bool result;
			if (systemAuthorization.DistributorCount > 0)
			{
				int systemDistributorsCount = MemberHelper.GetSystemDistributorsCount();
				leftNumber = systemAuthorization.DistributorCount - systemDistributorsCount;
				result = (systemAuthorization.DistributorCount >= systemDistributorsCount + number);
			}
			else
			{
				result = true;
			}
			return result;
		}

		public static string PostData(string url, string postData)
		{
			string result = string.Empty;
			try
			{
				Uri requestUri = new Uri(url);
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
				Encoding uTF = Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = (long)bytes.Length;
				using (Stream requestStream = httpWebRequest.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream responseStream = httpWebResponse.GetResponseStream())
					{
						Encoding uTF2 = Encoding.UTF8;
						Stream stream = responseStream;
						if (httpWebResponse.ContentEncoding.ToLower() == "gzip")
						{
							stream = new GZipStream(responseStream, CompressionMode.Decompress);
						}
						else if (httpWebResponse.ContentEncoding.ToLower() == "deflate")
						{
							stream = new DeflateStream(responseStream, CompressionMode.Decompress);
						}
						using (StreamReader streamReader = new StreamReader(stream, uTF2))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception var_11_158)
			{
				result = string.Empty;
			}
			return result;
		}
	}
}
