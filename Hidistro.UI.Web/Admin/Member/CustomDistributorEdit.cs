using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	public class CustomDistributorEdit : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txtGroupName;

		protected System.Web.UI.WebControls.TextBox txtShopIntroduction;

		protected CustomDistributorEdit() : base("m04", "hyp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
	}
}
