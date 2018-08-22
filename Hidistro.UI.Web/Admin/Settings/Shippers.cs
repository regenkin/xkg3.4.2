using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class Shippers : AdminPage
	{
		protected Pager pager;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Repeater ShipperList;

		protected System.Web.UI.WebControls.Literal editType;

		protected System.Web.UI.WebControls.HiddenField Task;

		protected System.Web.UI.WebControls.HiddenField ShipperId;

		protected System.Web.UI.WebControls.CheckBoxList txtShipperType;

		protected System.Web.UI.WebControls.TextBox txtShipperName;

		protected RegionSelector ddlReggion;

		protected System.Web.UI.WebControls.TextBox txtAddress;

		protected System.Web.UI.WebControls.TextBox txtTelPhone;

		protected System.Web.UI.WebControls.Button btnSave;

		protected Script Script4;

		protected Shippers() : base("m09", "szp09")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSave.Click += new System.EventHandler(this.btnAddShipper_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindShippers();
			}
		}

		private void BindShippers()
		{
			this.ShipperList.DataSource = SalesHelper.GetShippers(false);
			this.ShipperList.DataBind();
		}

		private void btnAddShipper_Click(object sender, System.EventArgs e)
		{
			ShippersInfo shippersInfo = new ShippersInfo();
			string shipperTag = "0";
			if (this.txtShipperType.Items[0].Selected && !this.txtShipperType.Items[1].Selected)
			{
				shipperTag = "1";
			}
			else if (!this.txtShipperType.Items[0].Selected && this.txtShipperType.Items[1].Selected)
			{
				shipperTag = "2";
			}
			else if (this.txtShipperType.Items[0].Selected && this.txtShipperType.Items[1].Selected)
			{
				shipperTag = "3";
			}
			shippersInfo.ShipperTag = shipperTag;
			shippersInfo.ShipperName = this.txtShipperName.Text.Trim();
			if (!this.ddlReggion.GetSelectedRegionId().HasValue)
			{
				this.ShowMsg("请选择地区", false);
				return;
			}
			shippersInfo.RegionId = this.ddlReggion.GetSelectedRegionId().Value;
			shippersInfo.Address = this.txtAddress.Text.Trim();
			shippersInfo.CellPhone = this.txtTelPhone.Text.Trim();
			shippersInfo.TelPhone = this.txtTelPhone.Text.Trim();
			shippersInfo.Zipcode = "";
			shippersInfo.IsDefault = true;
			shippersInfo.Remark = "";
			int shipperId = 0;
			int.TryParse(this.ShipperId.Value, out shipperId);
			shippersInfo.ShipperId = shipperId;
			if (!this.ValidationShipper(shippersInfo))
			{
				return;
			}
			if (string.IsNullOrEmpty(shippersInfo.CellPhone) && string.IsNullOrEmpty(shippersInfo.TelPhone))
			{
				this.ShowMsg("手机号码和电话号码必填其一", false);
				return;
			}
			if (SalesHelper.AddShipper(shippersInfo))
			{
				if (this.Task.Value == "EDIT")
				{
					this.Task.Value = "ADD";
					this.txtShipperType.SelectedValue = "";
					this.txtShipperType.Enabled = true;
					this.txtShipperName.Text = "";
					this.ddlReggion.SetSelectedRegionId(new int?(0));
					this.txtAddress.Text = "";
					this.ShipperId.Value = "";
					this.txtTelPhone.Text = "";
					this.txtShipperType.ClearSelection();
					this.editType.Text = "新增发货地址信息";
					this.ShowMsg("成功修改了一个发货信息", true);
				}
				else
				{
					this.ShowMsg("成功添加了一个发货信息", true);
				}
				this.BindShippers();
				return;
			}
			this.ShowMsg("添加发货信息失败", false);
		}

		private void ShipperList_RowDeleting(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
		}

		private bool ValidationShipper(ShippersInfo shipper)
		{
			ValidationResults validationResults = Validation.Validate<ShippersInfo>(shipper, new string[]
			{
				"Valshipper"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}

		protected void DeleteShiper_Click(object sender, System.EventArgs e)
		{
			int shipperId = 0;
			if (!int.TryParse(((System.Web.UI.WebControls.Button)sender).CommandArgument, out shipperId))
			{
				this.ShowMsg("非正常删除！", true);
				return;
			}
			if (SalesHelper.DeleteShipper(shipperId))
			{
				this.BindShippers();
				this.ShowMsg("已经成功删除选择的发货信息", true);
				return;
			}
			this.ShowMsg("非正常删除！", true);
		}

		protected void EditShiper(object sender, System.EventArgs e)
		{
			int shipperId = 0;
			if (int.TryParse(((System.Web.UI.WebControls.LinkButton)sender).CommandArgument, out shipperId))
			{
				ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
				if (shipper == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				Globals.EntityCoding(shipper, false);
				string shipperTag = shipper.ShipperTag;
				this.txtShipperType.ClearSelection();
				if (shipperTag == "1")
				{
					this.txtShipperType.Items[0].Selected = true;
				}
				if (shipperTag == "2")
				{
					this.txtShipperType.Items[1].Selected = true;
				}
				if (shipperTag == "3")
				{
					this.txtShipperType.Items[1].Selected = true;
					this.txtShipperType.Items[0].Selected = true;
				}
				this.txtShipperName.Text = shipper.ShipperName;
				this.ddlReggion.SetSelectedRegionId(new int?(shipper.RegionId));
				this.txtAddress.Text = shipper.Address;
				this.txtTelPhone.Text = shipper.TelPhone;
				this.ShipperId.Value = shipperId.ToString();
				this.Task.Value = "EDIT";
				this.editType.Text = "修改发货地址信息";
			}
		}

		protected void SwapShiper(object sender, System.EventArgs e)
		{
			int shipperId = 0;
			if (int.TryParse(((System.Web.UI.WebControls.LinkButton)sender).CommandArgument, out shipperId))
			{
				ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
				if (shipper == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				if (SalesHelper.SwapShipper(shipperId, shipper.ShipperTag))
				{
					this.BindShippers();
					this.ShowMsg("已成功交换了退发货地址信息！", true);
					return;
				}
				this.ShowMsg("交换地址信息异常！", true);
			}
		}
	}
}
