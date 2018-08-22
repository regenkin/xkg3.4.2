using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.Vshop;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMyLogistics : VMemberTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vMyLogistics.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string text = this.Page.Request.QueryString["orderId"];
			if (string.IsNullOrEmpty(text))
			{
				base.GotoResourceNotFound("");
			}
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(text);
			ExpressSet expressSet = ExpressHelper.GetExpressSet();
			System.Web.UI.WebControls.Literal literal = this.FindControl("litHasNewKey") as System.Web.UI.WebControls.Literal;
			System.Web.UI.WebControls.Literal literal2 = this.FindControl("litExpressUrl") as System.Web.UI.WebControls.Literal;
			System.Web.UI.WebControls.Literal control = this.FindControl("litCompanyCode") as System.Web.UI.WebControls.Literal;
			literal.Text = "0";
			literal2.Text = "";
			if (expressSet != null)
			{
				if (!string.IsNullOrEmpty(expressSet.NewKey))
				{
					literal.Text = "1";
				}
				if (!string.IsNullOrEmpty(expressSet.Url.Trim()))
				{
					literal2.Text = expressSet.Url.Trim();
				}
			}
			System.Web.UI.WebControls.Literal literal3 = this.FindControl("litOrderID") as System.Web.UI.WebControls.Literal;
			System.Web.UI.WebControls.Literal literal4 = this.FindControl("litNumberID") as System.Web.UI.WebControls.Literal;
			System.Web.UI.WebControls.Literal control2 = this.FindControl("litCompanyName") as System.Web.UI.WebControls.Literal;
			System.Web.UI.WebControls.Literal control3 = this.FindControl("litLogisticsNumber") as System.Web.UI.WebControls.Literal;
			literal3.Text = text;
			literal4.Text = orderInfo.ShipOrderNumber;
			control2.SetWhenIsNotNull(orderInfo.ExpressCompanyName);
			control3.SetWhenIsNotNull(orderInfo.ShipOrderNumber);
			control.SetWhenIsNotNull(orderInfo.ExpressCompanyAbb);
			PageTitle.AddSiteNameTitle("我的物流");
		}
	}
}
