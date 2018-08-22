using Hidistro.ControlPanel.Bargain;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
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
	public class AddBargain : AdminPage
	{
		protected string productInfoHtml = "活动商品尚未设置,请选择活动商品";

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Label lbtitle;

		protected System.Web.UI.WebControls.Image productImage;

		protected System.Web.UI.WebControls.Label lbProductName;

		protected System.Web.UI.WebControls.TextBox txtTitle;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.HiddenField hidpic;

		protected System.Web.UI.WebControls.HiddenField hidpicdel;

		protected System.Web.UI.WebControls.TextBox txt_img;

		protected System.Web.UI.WebControls.TextBox txtRemarks;

		protected System.Web.UI.WebControls.HiddenField hiddProductId;

		protected System.Web.UI.WebControls.TextBox txtActivityStock;

		protected System.Web.UI.WebControls.TextBox txtPurchaseNumber;

		protected System.Web.UI.WebControls.TextBox txtInitialPrice;

		protected System.Web.UI.WebControls.TextBox txtFloorPrice;

		protected System.Web.UI.WebControls.CheckBox ckIsCommission;

		protected System.Web.UI.WebControls.RadioButton rbtBargainTypeOne;

		protected System.Web.UI.WebControls.TextBox txtBargainTypeOneValue;

		protected System.Web.UI.WebControls.RadioButton rbtBargainTypeTwo;

		protected System.Web.UI.WebControls.TextBox txtBargainTypeTwoValue1;

		protected System.Web.UI.WebControls.TextBox txtBargainTypeTwoValue2;

		protected System.Web.UI.WebControls.Button btnSave;

		public AddBargain() : base("m08", "yxp21")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			if (!base.IsPostBack)
			{
				this.LoadData();
			}
		}

		private void LoadData()
		{
			int id = 0;
			if (int.TryParse(this.Page.Request.QueryString["Id"], out id))
			{
				BargainInfo bargainInfo = BargainHelper.GetBargainInfo(id);
				if (bargainInfo != null)
				{
					if (bargainInfo.ProductId > 0)
					{
						ProductInfo productDetails = ProductHelper.GetProductDetails(bargainInfo.ProductId);
						this.productImage.ImageUrl = (string.IsNullOrEmpty(productDetails.ImageUrl1) ? "/utility/pics/none.gif" : productDetails.ImageUrl1);
						this.lbProductName.Text = productDetails.ProductName;
						this.lbtitle.Text = bargainInfo.Title;
						this.productInfoHtml = this.GetProductInfoHtml(productDetails);
					}
					this.txtTitle.Text = bargainInfo.Title;
					this.calendarStartDate.Text = bargainInfo.BeginDate.ToString();
					this.calendarEndDate.Text = bargainInfo.EndDate.ToString();
					this.hidpic.Value = bargainInfo.ActivityCover;
					this.hiddProductId.Value = bargainInfo.ProductId.ToString();
					this.txtRemarks.Text = bargainInfo.Remarks;
					this.txtActivityStock.Text = bargainInfo.ActivityStock.ToString();
					this.txtPurchaseNumber.Text = bargainInfo.PurchaseNumber.ToString();
					this.txtInitialPrice.Text = bargainInfo.InitialPrice.ToString("f2");
					this.txtFloorPrice.Text = bargainInfo.FloorPrice.ToString("f2");
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
			stringBuilder.Append("<img src='" + (string.IsNullOrEmpty(product.ImageUrl1) ? "/utility/pics/none.gif" : product.ImageUrl1) + "' width='60' height='60' >");
			stringBuilder.Append("</div>");
			stringBuilder.Append("<div class='shop-username fl ml10'>");
			stringBuilder.Append("<p>" + product.ProductName + "</p>");
			stringBuilder.Append("</div>");
			stringBuilder.Append(" <p class='fl ml20'>现价：￥" + product.MarketPrice.Value.ToString("f2") + "</p>");
			stringBuilder.Append(" <p class='fl ml20'>库存：" + ProductHelper.GetProductSumStock(product.ProductId) + "</p>");
			return stringBuilder.ToString();
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			int num = Globals.RequestQueryNum("id");
			string text = this.txtTitle.Text;
			if (string.IsNullOrWhiteSpace(text))
			{
				this.ShowMsg("分享标题不能为空！", false);
				return;
			}
			string text2 = this.calendarStartDate.Text;
			string text3 = this.calendarEndDate.Text;
			if (string.IsNullOrWhiteSpace(text2) || string.IsNullOrWhiteSpace(text3))
			{
				this.ShowMsg("活动时间不能为空！", false);
				return;
			}
			if (System.DateTime.Parse(text2) >= System.DateTime.Parse(text3))
			{
				this.ShowMsg("结束时间必须大于开始时间！", false);
				return;
			}
			string value = this.hidpic.Value;
			if (string.IsNullOrWhiteSpace(value))
			{
				this.ShowMsg("请先上传活动封面！", false);
				return;
			}
			string text4 = this.txtRemarks.Text;
			if (text4.Length > 200)
			{
				this.ShowMsg("活动说明不能超过200个字节！", false);
				return;
			}
			int num2 = Globals.ToNum(this.hiddProductId.Value);
			if (num2 <= 0)
			{
				this.ShowMsg("请先选择参加活动的产品！", false);
				return;
			}
			int num3 = Globals.ToNum(this.txtActivityStock.Text.Trim());
			if (num3 <= 0)
			{
				this.ShowMsg("活动库存必须为大于0的正整数！", false);
				return;
			}
			int num4 = Globals.ToNum(this.txtPurchaseNumber.Text.Trim());
			if (num4 <= 0)
			{
				this.ShowMsg("限购数量必须为大于0的正整数！", false);
				return;
			}
			float num5 = 0f;
			if (!float.TryParse(this.txtInitialPrice.Text, out num5))
			{
				this.ShowMsg("初始价格输入不正确！", false);
				return;
			}
			float num6 = 0f;
			if (!float.TryParse(this.txtFloorPrice.Text, out num6))
			{
				this.ShowMsg("活动底价输入不正确！", false);
				return;
			}
			if (num6 <= 0f || num5 <= 0f)
			{
				this.ShowMsg("活动底价必须或初始价格必须大于0！", false);
				return;
			}
			if (num5 < num6)
			{
				this.ShowMsg("初始价格不能小于活动底价！", false);
				return;
			}
			long productSumStock = ProductHelper.GetProductSumStock(num2);
			if ((long)num3 > productSumStock)
			{
				this.ShowMsg("活动库存不能大于商品库存！", false);
				return;
			}
			if (num4 > num3)
			{
				this.ShowMsg("限购数量不能大于活动库存！", false);
				return;
			}
			int num7 = this.rbtBargainTypeOne.Checked ? 0 : 1;
			BargainInfo bargainInfo = new BargainInfo();
			bargainInfo.Title = text;
			bargainInfo.BeginDate = System.DateTime.Parse(text2);
			bargainInfo.EndDate = System.DateTime.Parse(text3);
			bargainInfo.ActivityCover = value;
			bargainInfo.Remarks = text4;
			bargainInfo.ProductId = num2;
			bargainInfo.ActivityStock = num3;
			bargainInfo.PurchaseNumber = num4;
			bargainInfo.TranNumber = 0;
			bargainInfo.InitialPrice = (decimal)num5;
			bargainInfo.FloorPrice = (decimal)num6;
			bargainInfo.BargainType = num7;
			bargainInfo.CreateDate = System.DateTime.Now;
			bargainInfo.IsCommission = this.ckIsCommission.Checked;
			if (num7 == 0)
			{
				string text5 = this.txtBargainTypeOneValue.Text;
				if (string.IsNullOrWhiteSpace(text5))
				{
					this.ShowMsg("每次砍掉价格不能为空！", false);
					return;
				}
				bargainInfo.BargainTypeMinVlue = float.Parse(text5);
			}
			else
			{
				string text6 = this.txtBargainTypeTwoValue1.Text;
				string text7 = this.txtBargainTypeTwoValue2.Text;
				if (string.IsNullOrWhiteSpace(text6) || string.IsNullOrWhiteSpace(text7))
				{
					this.ShowMsg("随机砍价最小值或最大值不能为空！", false);
					return;
				}
				float num8 = 0f;
				float num9 = 0f;
				if (!float.TryParse(text6, out num8))
				{
					this.ShowMsg("随机砍价最小值必须为数值！", false);
					return;
				}
				if (!float.TryParse(text7, out num9))
				{
					this.ShowMsg("随机砍价最大值必须为数值！", false);
					return;
				}
				if (num8 > num9)
				{
					this.ShowMsg("随机砍价最大值必须大于最小值！", false);
					return;
				}
				if (num8 < 0f || num9 < 0f)
				{
					this.ShowMsg("随机砍价最大值,最小值都必须大于零！", false);
					return;
				}
				bargainInfo.BargainTypeMinVlue = num8;
				bargainInfo.BargainTypeMaxVlue = num9;
			}
			if (num > 0)
			{
				bargainInfo.Id = num;
				bool flag = BargainHelper.UpdateBargain(bargainInfo);
				if (flag)
				{
					this.ShowMsgAndReUrl("修改成功", true, "ManagerBargain.aspx?Type=0");
					return;
				}
			}
			else
			{
				bool flag2 = BargainHelper.InsertBargain(bargainInfo);
				if (flag2)
				{
					this.ShowMsgAndReUrl("添加成功", true, "ManagerBargain.aspx?Type=0");
				}
			}
		}
	}
}
