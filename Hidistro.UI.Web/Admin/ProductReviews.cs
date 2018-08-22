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

namespace Hidistro.UI.Web.Admin
{
	public class ProductReviews : AdminPage
	{
		private string keywords = string.Empty;

		private int? categoryId;

		private string productCode;

		protected System.Web.UI.WebControls.TextBox txtSearchText;

		protected ProductCategoriesDropDownList dropCategories;

		protected System.Web.UI.WebControls.TextBox txtSKU;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected PageSize hrefPageSize;

		protected Pager pager1;

		protected Grid dlstPtReviews;

		protected Pager pager;

		protected ProductReviews() : base("m02", "spp13")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.dlstPtReviews.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.dlstPtReviews_RowDeleting);
			this.dlstPtReviews.ReBindData += new Grid.ReBindDataEventHandler(this.dlstPtReviews_ReBindData);
			this.SetSearchControl();
		}

		private void dlstPtReviews_ReBindData(object sender)
		{
			this.BindPtReview();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductReviews(true);
		}

		private void dlstPtReviews_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int num = int.Parse(this.dlstPtReviews.DataKeys[e.RowIndex].Value.ToString());
			if (ProductCommentHelper.DeleteProductReview((long)num) > 0)
			{
				this.ShowMsg("成功删除了选择的客户评论", true);
				this.BindPtReview();
				return;
			}
			this.ShowMsg("删除失败", false);
		}

		private void BindPtReview()
		{
			ProductReviewQuery productReviewQuery = new ProductReviewQuery();
			productReviewQuery.Keywords = this.keywords;
			productReviewQuery.CategoryId = this.categoryId;
			productReviewQuery.ProductCode = this.productCode;
			productReviewQuery.PageIndex = this.pager.PageIndex;
			productReviewQuery.PageSize = this.pager.PageSize;
			productReviewQuery.SortOrder = SortAction.Desc;
			productReviewQuery.SortBy = "ReviewDate";
			Globals.EntityCoding(productReviewQuery, true);
			DbQueryResult productReviews = ProductCommentHelper.GetProductReviews(productReviewQuery);
			this.dlstPtReviews.DataSource = productReviews.Data;
			this.dlstPtReviews.DataBind();
			this.pager.TotalRecords = productReviews.TotalRecords;
		}

		private void ReloadProductReviews(bool isSearch)
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
					this.keywords = base.Server.UrlDecode(this.Page.Request.QueryString["Keywords"]);
				}
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out value))
				{
					this.categoryId = new int?(value);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
				{
					this.productCode = base.Server.UrlDecode(this.Page.Request.QueryString["productCode"]);
				}
				this.txtSearchText.Text = this.keywords;
				this.txtSKU.Text = this.productCode;
				this.dropCategories.DataBind();
				this.dropCategories.SelectedValue = this.categoryId;
				this.BindPtReview();
				return;
			}
			this.keywords = this.txtSearchText.Text.Trim();
			this.productCode = this.txtSKU.Text.Trim();
			this.categoryId = this.dropCategories.SelectedValue;
		}
	}
}
