using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Vshop
{
	public class _default : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			base.Response.StatusCode = 301;
			base.Response.Status = "301 Moved Permanently";
			base.Response.AppendHeader("Location", "/default.aspx");
			base.Response.AppendHeader("Cache-Control", "no-cache");
			base.Response.End();
		}
	}
}
