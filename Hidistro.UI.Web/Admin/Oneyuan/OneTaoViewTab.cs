using ASPNET.WebControls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Oneyuan
{
	public class OneTaoViewTab : System.Web.UI.UserControl
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl mytabl;

		protected System.Web.UI.HtmlControls.HtmlGenericControl LiViewTab1;

		protected System.Web.UI.HtmlControls.HtmlAnchor ViewTab1;

		protected System.Web.UI.HtmlControls.HtmlGenericControl LiViewTab2;

		protected System.Web.UI.HtmlControls.HtmlAnchor ViewTab2;

		protected System.Web.UI.HtmlControls.HtmlGenericControl LiViewTab3;

		protected System.Web.UI.HtmlControls.HtmlAnchor ViewTab3;

		protected System.Web.UI.HtmlControls.HtmlGenericControl pageSizeSet;

		protected PageSize hrefPageSize;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.QueryString["vaid"];
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = this.Page.Request.Url.ToString();
				this.pageSizeSet.Visible = false;
				if (text2.Contains(this.ViewTab1.HRef))
				{
					this.LiViewTab1.Attributes.Add("class", "active");
				}
				else if (text2.Contains(this.ViewTab2.HRef))
				{
					this.LiViewTab2.Attributes.Add("class", "active");
				}
				else if (text2.Contains(this.ViewTab3.HRef))
				{
					this.LiViewTab3.Attributes.Add("class", "active");
					this.pageSizeSet.Visible = true;
				}
				this.ViewTab1.HRef = this.ViewTab1.HRef + "?vaid=" + text;
				this.ViewTab2.HRef = this.ViewTab2.HRef + "?vaid=" + text;
				this.ViewTab3.HRef = this.ViewTab3.HRef + "?vaid=" + text;
				return;
			}
			this.mytabl.Visible = false;
		}
	}
}
