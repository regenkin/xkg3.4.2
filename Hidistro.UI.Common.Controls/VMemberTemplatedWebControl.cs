using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true), PersistChildren(false)]
	public abstract class VMemberTemplatedWebControl : VshopTemplatedWebControl
	{
		public MemberInfo CurrentMemberInfo;

		protected VMemberTemplatedWebControl()
		{
			string userAgent = this.Page.Request.UserAgent;
			this.CurrentMemberInfo = MemberProcessor.GetCurrentMember();
			if (this.CurrentMemberInfo != null && this.CurrentMemberInfo.Status == Convert.ToInt32(UserStatus.DEL))
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/logout.aspx");
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			if (this.CurrentMemberInfo == null || this.Page.Session["userid"] == null || this.Page.Session["userid"].ToString() != this.CurrentMemberInfo.UserId.ToString())
			{
				if (userAgent.ToLower().Contains("alipay"))
				{
					if (masterSettings.AlipayAppid.Length > 14)
					{
						base.AlipayLoginAction(masterSettings);
					}
					else
					{
						this.Page.Response.Redirect(Globals.ApplicationPath + "/UserLogin.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
					}
				}
				else if (userAgent.ToLower().Contains("micromessenger"))
				{
					if (masterSettings.IsValidationService)
					{
						base.WeixinLoginAction(masterSettings, true);
					}
					else
					{
						this.Page.Response.Redirect(Globals.ApplicationPath + "/UserLogin.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
					}
				}
				else if (this.CurrentMemberInfo == null || this.Page.Request.Cookies["Vshop-Member"] == null)
				{
					string url = Globals.ApplicationPath + "/UserLogin.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString());
					this.Page.Response.Redirect(url);
				}
			}
			if (this.CurrentMemberInfo == null)
			{
				this.CurrentMemberInfo = MemberProcessor.GetCurrentMember();
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
		}

		public override void RenderEndTag(HtmlTextWriter writer)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (masterSettings.MemberDefault)
			{
				writer.WriteLine("<div style='padding-top:60px'></div><script>  ");
				writer.WriteLine("$(function () { ");
				writer.WriteLine("  jQuery.getJSON(\"/api/Hi_Ajax_NavMenu.ashx\", function (settingjson) {  ");
				writer.WriteLine(" $(_.template($(\"#menu\").html())(settingjson)).appendTo('body');");
				writer.WriteLine(" GetUICss();   ");
				writer.WriteLine("    });  ");
				writer.WriteLine("})  ");
				writer.WriteLine("</script>  ");
			}
			base.RenderEndTag(writer);
		}

		private string GetResponseResult(string url)
		{
			WebRequest webRequest = WebRequest.Create(url);
			string result;
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse())
			{
				using (Stream responseStream = httpWebResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}
			return result;
		}
	}
}
