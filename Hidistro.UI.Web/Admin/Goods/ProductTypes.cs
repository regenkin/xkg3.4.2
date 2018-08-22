using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.ProductTypes)]
	public class ProductTypes : AdminPage
	{
		private string searchkey;

		protected System.Web.UI.WebControls.TextBox txtSearchText;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected Grid grdProductTypes;

		protected Pager pager;

		protected ProductTypes() : base("m02", "spp07")
		{
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.grdProductTypes.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProductTypes_RowDataBound);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.grdProductTypes.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProductTypes_RowDeleting);
		}

		private void grdProductTypes_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Row.FindControl("lbbrand");
				label.Text = ProductTypeHelper.GetBrandName(int.Parse(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "TypeId").ToString()));
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindTypes();
			}
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
				{
					this.searchkey = Globals.UrlDecode(this.Page.Request.QueryString["searchKey"]);
				}
				this.txtSearchText.Text = this.searchkey;
				return;
			}
			this.searchkey = this.txtSearchText.Text.Trim();
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("searchKey", this.txtSearchText.Text);
			nameValueCollection.Add("pageSize", "10");
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}

		private void BindTypes()
		{
			DbQueryResult productTypes = ProductTypeHelper.GetProductTypes(new ProductTypeQuery
			{
				TypeName = this.searchkey,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize
			});
			this.grdProductTypes.DataSource = productTypes.Data;
			this.grdProductTypes.DataBind();
			this.pager.TotalRecords = productTypes.TotalRecords;
		}

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}

		private void grdProductTypes_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int typeId = (int)this.grdProductTypes.DataKeys[e.RowIndex].Value;
			System.Collections.Generic.IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(typeId, AttributeUseageMode.Choose);
			if (ProductTypeHelper.DeleteProductType(typeId))
			{
				foreach (AttributeInfo current in attributes)
				{
					foreach (AttributeValueInfo current2 in current.AttributeValues)
					{
						StoreHelper.DeleteImage(current2.ImageUrl);
					}
				}
				this.BindTypes();
				this.ShowMsg("成功删除了一个商品类型", true);
				return;
			}
			this.ShowMsg("删除商品类型失败, 可能有商品正在使用该类型", false);
		}
	}
}
