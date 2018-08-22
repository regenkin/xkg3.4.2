using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Jobs;
using Hishop.AlipayFuwu.Api.Model;
using System;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Web;

namespace Hidistro.UI.Web
{
	public class Global : System.Web.HttpApplication
	{
		private static string strUrl = "";

		protected void Application_Start(object sender, System.EventArgs e)
		{
			if (ConfigurationManager.AppSettings["Installer"] != null)
			{
				Globals.Debuglog("到这里了，！网站未安装！", "_Debuglog.txt");
				return;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			AlipayFuwuConfig.CommSetConfig(masterSettings.AlipayAppid, base.Server.MapPath("~/"), "GBK");
			AlipayFuwuConfig.SetWriteLog(true);
			if (string.IsNullOrEmpty(Global.strUrl))
			{
				string text = System.Web.HttpContext.Current.Request.Url.Port.ToString();
				text = ((text == "80") ? "" : (":" + text));
				Global.strUrl = string.Format("http://{0}/UserLogin.aspx", System.Web.HttpContext.Current.Request.Url.Host + text);
			}
			JobsHelp.start(base.Server.MapPath("/config/JobConfig.xml"));
			string AppPath = base.Server.MapPath("/");
			bool result;
			new System.Threading.Thread(() =>
            {
				AsyncWorkDelegate_TongJi asyncWorkDelegate_TongJi = new AsyncWorkDelegate_TongJi();
				asyncWorkDelegate_TongJi.CalData(AppPath, out result);
			}).Start();
		}

		protected void Application_End(object sender, System.EventArgs e)
		{
			if (ConfigurationManager.AppSettings["Installer"] != null)
			{
				return;
			}
			try
			{
				JobsHelp.stop();
				if (string.IsNullOrEmpty(Global.strUrl))
				{
					System.Threading.Thread.Sleep(1000);
					System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(Global.strUrl);
					using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse())
					{
						using (httpWebResponse.GetResponseStream())
						{
						}
					}
				}
			}
			catch
			{
				Globals.Debuglog("重启动Application_start失败！", "_Debuglog.txt");
			}
		}

		protected void Session_Start(object sender, System.EventArgs e)
		{
		}

		protected void Application_BeginRequest(object sender, System.EventArgs e)
		{
			try
			{
				string name = "ASPSESSID";
				string cookie_name = "ASP.NET_SESSIONID";
				if (System.Web.HttpContext.Current.Request.Form[name] != null)
				{
					this.UpdateCookie(cookie_name, System.Web.HttpContext.Current.Request.Form[name]);
				}
				else if (System.Web.HttpContext.Current.Request.QueryString[name] != null)
				{
					this.UpdateCookie(cookie_name, System.Web.HttpContext.Current.Request.QueryString[name]);
				}
			}
			catch (System.Exception)
			{
				base.Response.StatusCode = 500;
				base.Response.Write("Error Initializing Session");
			}
		}

		private void UpdateCookie(string cookie_name, string cookie_value)
		{
			System.Web.HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies.Get(cookie_name);
			if (httpCookie == null)
			{
				httpCookie = new System.Web.HttpCookie(cookie_name);
				System.Web.HttpContext.Current.Request.Cookies.Add(httpCookie);
			}
			httpCookie.Value = cookie_value;
			System.Web.HttpContext.Current.Request.Cookies.Set(httpCookie);
		}

		protected void Application_AuthenticateRequest(object sender, System.EventArgs e)
		{
		}

		protected void Application_Error(object sender, System.EventArgs e)
		{
		}

		protected void Session_End(object sender, System.EventArgs e)
		{
		}
	}
}
