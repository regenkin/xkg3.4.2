using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Default : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			base.Response.Write(";");
			base.Response.End();
		}
	}
}
