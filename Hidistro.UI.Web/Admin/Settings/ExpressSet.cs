using Hidistro.Entities.Sales;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.Vshop;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class ExpressSet : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txtKey;

		protected System.Web.UI.WebControls.TextBox txtUrl;

		protected System.Web.UI.WebControls.Button btnSave;

		protected ExpressSet() : base("m09", "szp13")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				Hidistro.Entities.Sales.ExpressSet expressSet = ExpressHelper.GetExpressSet();
				if (expressSet != null)
				{
					this.txtKey.Text = expressSet.NewKey;
					this.txtUrl.Text = expressSet.Url;
				}
			}
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			string key = this.txtKey.Text.Trim();
			string text = this.txtUrl.Text.Trim();
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("物理查询地址不允许为空！", false);
				return;
			}
			ExpressHelper.UpdateExpressUrlAndKey(key, text);
			this.ShowMsg("修改物流查询地址和快递100 key成功！", true);
		}
	}
}
