using Hidistro.Core;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin
{
	public class LoginExit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Web.HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies.Get(string.Format("{0}{1}", Globals.DomainName, System.Web.Security.FormsAuthentication.FormsCookieName));
			httpCookie.Expires = System.DateTime.Now;
			System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
			base.Response.Redirect("Login.aspx", true);
		}
	}
}
