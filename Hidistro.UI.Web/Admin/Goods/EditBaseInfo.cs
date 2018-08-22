using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.EditProducts)]
	public class EditBaseInfo : AdminPage
	{
		private string productIds = string.Empty;

		protected System.Web.UI.WebControls.TextBox txtPrefix;

		protected System.Web.UI.WebControls.TextBox txtSuffix;

		protected System.Web.UI.WebControls.Button btnAddOK;

		protected System.Web.UI.WebControls.TextBox txtOleWord;

		protected System.Web.UI.WebControls.TextBox txtNewWord;

		protected System.Web.UI.WebControls.Button btnReplaceOK;

		protected Grid grdSelectedProducts;

		protected System.Web.UI.WebControls.Button btnSaveInfo;

		protected EditBaseInfo() : base("m02", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveInfo.Click += new System.EventHandler(this.btnSaveInfo_Click);
			this.btnAddOK.Click += new System.EventHandler(this.btnAddOK_Click);
			this.btnReplaceOK.Click += new System.EventHandler(this.btnReplaceOK_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
			}
		}

		private void btnAddOK_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtPrefix.Text.Trim()) && string.IsNullOrEmpty(this.txtSuffix.Text.Trim()))
			{
				this.ShowMsgToTarget("前后缀不能同时为空", false, "parent");
				return;
			}
			if (ProductHelper.UpdateProductNames(this.productIds, this.txtPrefix.Text.Trim(), this.txtSuffix.Text.Trim()))
			{
				this.ShowMsgToTarget("为商品名称添加前后缀成功", true, "parent");
			}
			else
			{
				this.ShowMsgToTarget("为商品名称添加前后缀失败", false, "parent");
			}
			this.BindProduct();
		}

		private void btnReplaceOK_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtOleWord.Text.Trim()))
			{
				this.ShowMsgToTarget("查找字符串不能为空", false, "parent");
				return;
			}
			if (ProductHelper.ReplaceProductNames(this.productIds, this.txtOleWord.Text.Trim(), this.txtNewWord.Text.Trim()))
			{
				this.ShowMsgToTarget("为商品名称替换字符串缀成功", true, "parent");
			}
			else
			{
				this.ShowMsgToTarget("为商品名称替换字符串缀失败", false, "parent");
			}
			this.BindProduct();
		}

		private void btnSaveInfo_Click(object sender, System.EventArgs e)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			dataTable.Columns.Add("ProductId");
			dataTable.Columns.Add("ProductName");
			dataTable.Columns.Add("ProductCode");
			dataTable.Columns.Add("MarketPrice");
			if (this.grdSelectedProducts.Rows.Count > 0)
			{
				decimal num = 0m;
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdSelectedProducts.Rows)
				{
					int num2 = (int)this.grdSelectedProducts.DataKeys[gridViewRow.RowIndex].Value;
					System.Web.UI.WebControls.TextBox textBox = gridViewRow.FindControl("txtProductName") as System.Web.UI.WebControls.TextBox;
					System.Web.UI.WebControls.TextBox textBox2 = gridViewRow.FindControl("txtProductCode") as System.Web.UI.WebControls.TextBox;
					System.Web.UI.WebControls.TextBox textBox3 = gridViewRow.FindControl("txtMarketPrice") as System.Web.UI.WebControls.TextBox;
					if (!string.IsNullOrEmpty(textBox3.Text.Trim()) && !decimal.TryParse(textBox3.Text.Trim(), out num))
					{
						break;
					}
					if (string.IsNullOrEmpty(textBox3.Text.Trim()))
					{
						num = 0m;
					}
					System.Data.DataRow dataRow = dataTable.NewRow();
					dataRow["ProductId"] = num2;
					dataRow["ProductName"] = Globals.HtmlEncode(textBox.Text.Trim());
					dataRow["ProductCode"] = Globals.HtmlEncode(textBox2.Text.Trim());
					if (num >= 0m)
					{
						dataRow["MarketPrice"] = num;
					}
					dataTable.Rows.Add(dataRow);
				}
				if (ProductHelper.UpdateProductBaseInfo(dataTable))
				{
					string text = Globals.RequestQueryStr("reurl");
					if (string.IsNullOrEmpty(text))
					{
						text = "productonsales.aspx";
					}
					this.ShowMsgAndReUrl("修改成功", true, text, "parent");
					return;
				}
				this.ShowMsgToTarget("批量修改商品信息失败,原价输入数据错误！", false, "parent");
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
