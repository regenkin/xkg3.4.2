using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Trade
{
	public class ShipAddress : AdminPage
	{
		private string orderId;

		private string action = "update";

		protected System.Web.UI.WebControls.Literal lblOriAddress;

		protected System.Web.UI.WebControls.TextBox txtShipTo;

		protected System.Web.UI.WebControls.TextBox txtCellPhone;

		protected System.Web.UI.WebControls.TextBox txtTelPhone;

		protected RegionSelector dropRegions;

		protected System.Web.UI.WebControls.TextBox txtAddress;

		protected System.Web.UI.WebControls.TextBox txtZipcode;

		protected System.Web.UI.WebControls.Button btnMondifyAddress;

		protected ShipAddress() : base("m01", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.orderId = Globals.RequestQueryStr("OrderId");
			if (string.IsNullOrEmpty(this.orderId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!base.IsPostBack)
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
				this.BindUpdateSippingAddress(orderInfo);
			}
			this.btnMondifyAddress.Click += new System.EventHandler(this.btnMondifyAddress_Click);
		}

		private void BindUpdateSippingAddress(OrderInfo order)
		{
			this.txtShipTo.Text = order.ShipTo;
			this.dropRegions.SetSelectedRegionId(new int?(order.RegionId));
			this.txtAddress.Text = order.Address;
			this.txtZipcode.Text = order.ZipCode;
			this.txtTelPhone.Text = order.TelPhone;
			this.txtCellPhone.Text = order.CellPhone;
			string text = order.OldAddress;
			if (string.IsNullOrEmpty(text))
			{
				if (!string.IsNullOrEmpty(order.ShippingRegion))
				{
					text = order.ShippingRegion.Replace('，', ' ');
				}
				if (!string.IsNullOrEmpty(order.Address))
				{
					text += order.Address;
				}
				if (!string.IsNullOrEmpty(order.ShipTo))
				{
					text = text + "，" + order.ShipTo;
				}
				if (!string.IsNullOrEmpty(order.TelPhone))
				{
					text = text + "，" + order.TelPhone;
				}
				if (!string.IsNullOrEmpty(order.CellPhone))
				{
					text = text + "，" + order.CellPhone;
				}
			}
			this.lblOriAddress.Text = text;
		}

		private void btnMondifyAddress_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			orderInfo.ShipTo = this.txtShipTo.Text.Trim();
			orderInfo.RegionId = this.dropRegions.GetSelectedRegionId().Value;
			orderInfo.Address = this.txtAddress.Text.Trim();
			orderInfo.TelPhone = this.txtTelPhone.Text.Trim();
			orderInfo.CellPhone = this.txtCellPhone.Text.Trim();
			orderInfo.ZipCode = this.txtZipcode.Text.Trim();
			orderInfo.ShippingRegion = this.dropRegions.SelectedRegions;
			if (string.IsNullOrEmpty(orderInfo.OldAddress))
			{
				orderInfo.OldAddress = this.lblOriAddress.Text;
			}
			if (string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()))
			{
				this.ShowMsgToTarget("电话号码和手机号码必填其一", false, "parent");
				return;
			}
			if (this.action == "update")
			{
				orderInfo.OrderId = this.orderId;
				if (OrderHelper.MondifyAddress(orderInfo))
				{
					OrderHelper.GetOrderInfo(this.orderId);
					this.ShowMsgAndReUrl("收货地址修改成功", true, "OrderDetails.aspx?OrderId=" + this.orderId, "parent");
					return;
				}
				this.ShowMsgToTarget("收货地址修改失败", false, "parent");
			}
		}
	}
}
