using Hidistro.Core;
using System;
using System.Web.UI;

namespace Hidistro.UI.Web
{
	public class loginEntry : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = this.Page.Request.QueryString["returnUrl"].ToLower();
			if (!string.IsNullOrEmpty(text) && text.StartsWith(Globals.GetSiteUrls().Locations["admin"].ToLower()))
			{
				if (text.EndsWith("/admin") || text.EndsWith("/admin/"))
				{
					text = "/admin/default.aspx";
				}
				base.Response.Redirect(Globals.GetAdminAbsolutePath("/login.aspx?returnUrl=" + text), true);
				return;
			}
			base.Response.Redirect(Globals.GetSiteUrls().Login, true);
		}
	}
}
