using Hidistro.ControlPanel.Sales;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.Vshop;
using System;
using System.Data;
using System.Text;

namespace Hidistro.UI.Web.Admin.Trade
{
	public class AddExpressTemplate : AdminPage
	{
		protected string ems = "";

		protected AddExpressTemplate() : base("m03", "ddp11")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request["task"];
			string text2 = base.Request["ExpressName"];
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2) && text == "IsExist")
			{
				if (SalesHelper.IsExistExpress(text2))
				{
					base.Response.Write("Y");
				}
				else
				{
					base.Response.Write("N");
				}
				base.Response.End();
			}
			if (!base.IsPostBack)
			{
				System.Data.DataTable expressTable = ExpressHelper.GetExpressTable();
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				foreach (System.Data.DataRow dataRow in expressTable.Rows)
				{
					stringBuilder.AppendFormat("<option value='{0}'>{0}</option>", dataRow["Name"]);
				}
				this.ems = stringBuilder.ToString();
			}
		}
	}
}
