using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Trade
{
	[PrivilegeCheck(Privilege.EditOrders)]
	public class SetLogistics : AdminPage
	{
		protected string orderIds = string.Empty;

		protected string Reurl = string.Empty;

		protected System.Web.UI.WebControls.Literal litOrdersCount;

		protected SetLogistics() : base("m03", "ddp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string a = Globals.RequestFormStr("posttype");
			this.Reurl = Globals.RequestQueryStr("reurl");
			if (string.IsNullOrEmpty(this.Reurl))
			{
				this.Reurl = "BuyerAlreadyPaid.aspx";
			}
			if (a == "savelogistics")
			{
				base.Response.ContentType = "application/json";
				string s = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
				string expressCompanyAbb = Globals.RequestFormStr("selid");
				string expressCompanyName = Globals.RequestFormStr("selname");
				string text = Globals.RequestFormStr("orderlist");
				string[] array = text.Split(new char[]
				{
					','
				});
				int num = 0;
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					if (Globals.IsOrdersID(text2) && OrderHelper.SetPrintOrderExpress(text2, expressCompanyName, expressCompanyAbb, ""))
					{
						num++;
					}
				}
				if (num > 0)
				{
					s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
				}
				base.Response.Write(s);
				base.Response.End();
				return;
			}
			this.orderIds = Globals.RequestQueryStr("OrderId").Trim(new char[]
			{
				','
			});
			this.litOrdersCount.Text = this.orderIds.Split(new char[]
			{
				','
			}).Length.ToString();
		}
	}
}
