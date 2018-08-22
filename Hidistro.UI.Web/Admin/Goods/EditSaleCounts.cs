using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.EditProducts)]
	public class EditSaleCounts : AdminPage
	{
		private string productIds = string.Empty;

		protected System.Web.UI.WebControls.TextBox txtSaleCounts;

		protected System.Web.UI.WebControls.Button btnAddOK;

		protected OperationDropDownList ddlOperation;

		protected System.Web.UI.WebControls.TextBox txtOperationSaleCounts;

		protected System.Web.UI.WebControls.Button btnOperationOK;

		protected Grid grdSelectedProducts;

		protected System.Web.UI.WebControls.Button btnSaveInfo;

		protected EditSaleCounts() : base("m01", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveInfo.Click += new System.EventHandler(this.btnSaveInfo_Click);
			this.btnAddOK.Click += new System.EventHandler(this.btnAddOK_Click);
			this.btnOperationOK.Click += new System.EventHandler(this.btnOperationOK_Click);
			if (!this.Page.IsPostBack)
			{
				this.ddlOperation.DataBind();
				this.ddlOperation.SelectedValue = "+";
				this.BindProduct();
			}
		}

		private void btnAddOK_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			if (!int.TryParse(this.txtSaleCounts.Text.Trim(), out num) || num < 0)
			{
				this.ShowMsgToTarget("销售数量只能是正整数，请输入正确的销售数量", false, "parent");
				return;
			}
			if (ProductHelper.UpdateShowSaleCounts(this.productIds, num))
			{
				this.ShowMsgToTarget("成功调整了前台显示的销售数量", true, "parent");
			}
			else
			{
				this.ShowMsgToTarget("调整前台显示的销售数量失败", false, "parent");
			}
			this.BindProduct();
		}

		private void btnOperationOK_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			if (!int.TryParse(this.txtOperationSaleCounts.Text.Trim(), out num) || num < 0)
			{
				this.ShowMsgToTarget("销售数量只能是正整数，请输入正确的销售数量", false, "parent");
				return;
			}
			if (ProductHelper.UpdateShowSaleCounts(this.productIds, num, this.ddlOperation.SelectedValue))
			{
				this.ShowMsgToTarget("成功调整了前台显示的销售数量", true, "parent");
			}
			else
			{
				this.ShowMsgToTarget("调整前台显示的销售数量失败", false, "parent");
			}
			this.BindProduct();
		}

		private void btnSaveInfo_Click(object sender, System.EventArgs e)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			dataTable.Columns.Add("ProductId");
			dataTable.Columns.Add("ShowSaleCounts");
			if (this.grdSelectedProducts.Rows.Count > 0)
			{
				int num = 0;
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdSelectedProducts.Rows)
				{
					int num2 = (int)this.grdSelectedProducts.DataKeys[gridViewRow.RowIndex].Value;
					System.Web.UI.WebControls.TextBox textBox = gridViewRow.FindControl("txtShowSaleCounts") as System.Web.UI.WebControls.TextBox;
					if (int.TryParse(textBox.Text.Trim(), out num) && num >= 0)
					{
						System.Data.DataRow dataRow = dataTable.NewRow();
						dataRow["ProductId"] = num2;
						dataRow["ShowSaleCounts"] = num;
						dataTable.Rows.Add(dataRow);
					}
				}
				if (ProductHelper.UpdateShowSaleCounts(dataTable))
				{
					string text = Globals.RequestQueryStr("reurl");
					if (string.IsNullOrEmpty(text))
					{
						text = "productonsales.aspx";
					}
					this.ShowMsgAndReUrl("成功调整了前台显示的销售数量", true, text, "parent");
					return;
				}
				this.ShowMsgToTarget("调整前台显示的销售数量失败", false, "parent");
				this.BindProduct();
			}
		}

		private void BindProduct()
		{
			string value = this.Page.Request.QueryString["ProductIds"];
			if (!string.IsNullOrEmpty(value))
			{
				this.grdSelectedProducts.DataSource = ProductHelper.GetProductBaseInfo(value);
				this.grdSelectedProducts.DataBind();
			}
		}
	}
}
