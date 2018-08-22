using Hidistro.ControlPanel.Bargain;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Bargain;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Bargain
{
	public class BargainDetial : AdminPage
	{
		public string productInfoHtml = "";

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Label lbtitle;

		protected System.Web.UI.WebControls.Image productImage;

		protected System.Web.UI.WebControls.Label lbproductName;

		protected System.Web.UI.WebControls.TextBox txtTitle;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Image imgeProductName;

		protected System.Web.UI.WebControls.TextBox txtRemarks;

		protected System.Web.UI.WebControls.TextBox txtTranNumber;

		protected System.Web.UI.WebControls.TextBox txtPurchaseNumber;

		protected System.Web.UI.WebControls.TextBox txtInitialPrice;

		protected System.Web.UI.WebControls.TextBox txtFloorPrice;

		protected System.Web.UI.WebControls.CheckBox ckIsCommission;

		protected System.Web.UI.WebControls.RadioButton rbtBargainTypeOne;

		protected System.Web.UI.WebControls.TextBox txtBargainTypeOneValue;

		protected System.Web.UI.WebControls.RadioButton rbtBargainTypeTwo;

		protected System.Web.UI.WebControls.TextBox txtBargainTypeTwoValue1;

		protected System.Web.UI.WebControls.TextBox txtBargainTypeTwoValue2;

		protected System.Web.UI.WebControls.Label lbNumberOfParticipants;

		protected System.Web.UI.WebControls.Label lbmemberNumber;

		protected System.Web.UI.WebControls.Label lbSaleNumber;

		protected System.Web.UI.WebControls.Label lbStock;

		protected System.Web.UI.WebControls.Label lbSalePrice;

		protected System.Web.UI.WebControls.Label lbStatus;

		public BargainDetial() : base("m08", "yxp21")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.LoadData();
			}
		}

		private void LoadData()
		{
			if (this.Page.Request.QueryString["Id"] != null)
			{
				int id = int.Parse(this.Page.Request.QueryString["Id"]);
				BargainInfo bargainInfo = BargainHelper.GetBargainInfo(id);
				if (bargainInfo != null)
				{
					if (bargainInfo.ProductId > 0)
					{
						ProductInfo productDetails = ProductHelper.GetProductDetails(bargainInfo.ProductId);
						this.productImage.ImageUrl = (string.IsNullOrEmpty(productDetails.ImageUrl1) ? "/utility/pics/none.gif" : productDetails.ImageUrl1);
						this.lbproductName.Text = productDetails.ProductName;
						this.lbtitle.Text = bargainInfo.Title;
						this.productInfoHtml = this.GetProductInfoHtml(productDetails);
					}
					this.txtTitle.Text = bargainInfo.Title;
					this.calendarStartDate.Text = bargainInfo.BeginDate.ToString();
					this.calendarEndDate.Text = bargainInfo.EndDate.ToString();
					this.imgeProductName.ImageUrl = (string.IsNullOrEmpty(bargainInfo.ActivityCover) ? "/utility/pics/none.gif" : bargainInfo.ActivityCover);
					this.txtRemarks.Text = bargainInfo.Remarks;
					this.txtTranNumber.Text = bargainInfo.ActivityStock.ToString();
					this.txtPurchaseNumber.Text = bargainInfo.PurchaseNumber.ToString();
					this.txtFloorPrice.Text = bargainInfo.FloorPrice.ToString("f2");
					this.txtInitialPrice.Text = bargainInfo.InitialPrice.ToString("f2");
					this.ckIsCommission.Checked = bargainInfo.IsCommission;
					if (bargainInfo.BargainType == 0)
					{
						this.rbtBargainTypeOne.Checked = true;
						this.rbtBargainTypeTwo.Checked = false;
						this.txtBargainTypeOneValue.Text = bargainInfo.BargainTypeMinVlue.ToString("f2");
						return;
					}
					this.rbtBargainTypeOne.Checked = false;
					this.rbtBargainTypeTwo.Checked = true;
					this.txtBargainTypeTwoValue1.Text = bargainInfo.BargainTypeMinVlue.ToString("f2");
					this.txtBargainTypeTwoValue2.Text = bargainInfo.BargainTypeMaxVlue.ToString("f2");
				}
			}
		}

		public string GetProductInfoHtml(ProductInfo product)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("<div class='shop-img fl'>");
			stringBuilder.Append("<img src='" + (string.IsNullOrEmpty(product.ImageUrl1) ? "/utility/pics/none.gif" : product.ImageUrl1) + "'  width='60' height='60'>");
			stringBuilder.Append("</div>");
			stringBuilder.Append("<div class='shop-username fl ml10'>");
			stringBuilder.Append("<p>" + product.ProductName + "</p>");
			stringBuilder.Append("</div>");
			stringBuilder.Append(" <p class='fl ml20'>现价：￥" + product.MarketPrice.Value.ToString("f2") + "</p>");
			stringBuilder.Append(" <p class='fl ml20'>库存：" + ProductHelper.GetProductSumStock(product.ProductId) + "</p>");
			return stringBuilder.ToString();
		}
	}
}
