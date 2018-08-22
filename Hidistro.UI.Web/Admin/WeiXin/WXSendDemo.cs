using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class WXSendDemo : AdminPage
	{
		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.TextBox txtOrderId;

		protected System.Web.UI.WebControls.Label lbMsg;

		protected System.Web.UI.WebControls.Button btnSend_NewOrder;

		protected System.Web.UI.WebControls.Button btnSend_NewProduct;

		protected WXSendDemo() : base("m06", "wxp01")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			bool arg_0B_0 = this.Page.IsPostBack;
		}

		protected void btnSend_NewOrder_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.txtOrderId.Text.Trim());
			if (orderInfo == null)
			{
				this.lbMsg.Text = "订单不存在！" + System.DateTime.Now.ToString();
				return;
			}
			this.lbMsg.Text = Messenger.SendWeiXinMsg_OrderCreate(orderInfo);
		}

		protected void btnSend_NewProduct_Click(object sender, System.EventArgs e)
		{
			ProductHelper.SendWXMessage_AddNewProduct(new ProductInfo
			{
				ProductName = "三星手机G9260"
			});
		}
	}
}
