using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.EditProducts)]
	public class EditStocks : AdminPage
	{
		private string productIds = string.Empty;

		protected System.Web.UI.WebControls.TextBox txtTagetStock;

		protected System.Web.UI.WebControls.Button btnTargetOK;

		protected System.Web.UI.WebControls.TextBox txtAddStock;

		protected System.Web.UI.WebControls.Button btnOperationOK;

		protected Grid grdSelectedProducts;

		protected System.Web.UI.WebControls.Button btnSaveStock;

		protected EditStocks() : base("m01", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveStock.Click += new System.EventHandler(this.btnSaveStock_Click);
			this.btnTargetOK.Click += new System.EventHandler(this.btnTargetOK_Click);
			this.btnOperationOK.Click += new System.EventHandler(this.btnOperationOK_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
			}
		}

		private void btnOperationOK_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.productIds))
			{
				this.ShowMsgToTarget("没有要修改的商品", false, "parent");
				return;
			}
			int addStock = 0;
			if (!int.TryParse(this.txtAddStock.Text, out addStock))
			{
				this.ShowMsgToTarget("请输入正确的库存格式", false, "parent");
				return;
			}
			if (ProductHelper.AddSkuStock(this.productIds, addStock))
			{
				this.BindProduct();
				this.ShowMsgToTarget("修改商品的库存成功", true, "parent");
				return;
			}
			this.ShowMsgToTarget("修改商品的库存失败", false, "parent");
		}

		private void btnTargetOK_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.productIds))
			{
				this.ShowMsgToTarget("没有要修改的商品", false, "parent");
				return;
			}
			int num = 0;
			if (!int.TryParse(this.txtTagetStock.Text, out num))
			{
				this.ShowMsgToTarget("请输入正确的库存格式", false, "parent");
				return;
			}
			if (num < 0)
			{
				this.ShowMsgToTarget("商品库存不能小于0", false, "parent");
				return;
			}
			if (ProductHelper.UpdateSkuStock(this.productIds, num))
			{
				this.BindProduct();
				this.ShowMsgToTarget("修改商品的库存成功", true, "parent");
				return;
			}
			this.ShowMsgToTarget("修改商品的库存失败", false, "parent");
		}

		private void btnSaveStock_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.Dictionary<string, int> dictionary = null;
			if (this.grdSelectedProducts.Rows.Count > 0)
			{
				dictionary = new System.Collections.Generic.Dictionary<string, int>();
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdSelectedProducts.Rows)
				{
					int value = 0;
					System.Web.UI.WebControls.TextBox textBox = gridViewRow.FindControl("txtStock") as System.Web.UI.WebControls.TextBox;
					if (int.TryParse(textBox.Text, out value))
					{
						string key = (string)this.grdSelectedProducts.DataKeys[gridViewRow.RowIndex].Value;
						dictionary.Add(key, value);
					}
				}
				if (dictionary.Count > 0)
				{
					if (ProductHelper.UpdateSkuStock(dictionary))
					{
						string text = Globals.RequestQueryStr("reurl");
						if (string.IsNullOrEmpty(text))
						{
							text = "productonsales.aspx";
						}
						this.ShowMsgAndReUrl("保存成功", true, text, "parent");
						return;
					}
					this.ShowMsgToTarget("批量修改库存失败", false, "parent");
					this.BindProduct();
				}
			}
		}

		private void BindProduct()
		{
			string value = this.Page.Request.QueryString["ProductIds"];
			if (!string.IsNullOrEmpty(value))
			{
				this.grdSelectedProducts.DataSource = ProductHelper.GetSkuStocks(value);
				this.grdSelectedProducts.DataBind();
			}
		}
	}
}
