using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	public class ProductConsultations : AdminPage
	{
		private string keywords = string.Empty;

		private int? categoryId;

		private string productCode;

		protected System.Web.UI.WebControls.TextBox txtSearchText;

		protected ProductCategoriesDropDownList dropCategories;

		protected System.Web.UI.WebControls.TextBox txtSKU;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected Grid grdConsultation;

		protected Pager pager;

		protected ProductConsultations() : base("m02", "spp09")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.SetSearchControl();
			this.grdConsultation.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdConsultation_RowDeleting);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.grdConsultation.ReBindData += new Grid.ReBindDataEventHandler(this.grdConsultation_ReBindData);
		}

		private void grdConsultation_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int consultationId = (int)this.grdConsultation.DataKeys[e.RowIndex].Value;
			if (ProductCommentHelper.DeleteProductConsultation(consultationId) > 0)
			{
				this.ShowMsg("成功删除了选择的商品咨询", true);
				this.BindConsultation();
				return;
			}
			this.ShowMsg("删除商品咨询失败", false);
		}

		private void grdConsultation_ReBindData(object sender)
		{
			this.BindConsultation();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductConsultations(true);
		}

		private void BindConsultation()
		{
			ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
			productConsultationAndReplyQuery.Keywords = this.keywords;
			productConsultationAndReplyQuery.CategoryId = this.categoryId;
			productConsultationAndReplyQuery.ProductCode = this.productCode;
			productConsultationAndReplyQuery.PageIndex = this.pager.PageIndex;
			productConsultationAndReplyQuery.PageSize = this.pager.PageSize;
			productConsultationAndReplyQuery.SortOrder = SortAction.Desc;
			productConsultationAndReplyQuery.SortBy = "ReplyDate";
			productConsultationAndReplyQuery.Type = ConsultationReplyType.NoReply;
			Globals.EntityCoding(productConsultationAndReplyQuery, true);
			DbQueryResult consultationProducts = ProductCommentHelper.GetConsultationProducts(productConsultationAndReplyQuery);
			this.grdConsultation.DataSource = consultationProducts.Data;
			this.grdConsultation.DataBind();
			this.pager.TotalRecords = consultationProducts.TotalRecords;
		}

		private void ReloadProductConsultations(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("Keywords", this.txtSearchText.Text.Trim());
			nameValueCollection.Add("CategoryId", this.dropCategories.SelectedValue.ToString());
			nameValueCollection.Add("productCode", this.txtSKU.Text.Trim());
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			base.ReloadPage(nameValueCollection);
		}

		private void SetSearchControl()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Keywords"]))
				{
					this.keywords = this.Page.Request.QueryString["Keywords"];
				}
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out value))
				{
					this.categoryId = new int?(value);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
				{
					this.productCode = this.Page.Request.QueryString["productCode"];
				}
				this.txtSearchText.Text = this.keywords;
				this.txtSKU.Text = this.productCode;
				this.dropCategories.DataBind();
				this.dropCategories.SelectedValue = this.categoryId;
				this.BindConsultation();
				return;
			}
			this.keywords = this.txtSearchText.Text.Trim();
			this.productCode = this.txtSKU.Text.Trim();
			this.categoryId = this.dropCategories.SelectedValue;
		}
	}
}
