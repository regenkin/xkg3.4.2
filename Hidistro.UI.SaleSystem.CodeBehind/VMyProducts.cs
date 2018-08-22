using Hidistro.Core;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMyProducts : VMemberTemplatedWebControl
	{
		private int categoryId;

		private string keyWord = string.Empty;

		private VshopTemplatedRepeater rpChooseProducts;

		private VshopTemplatedRepeater rpCategorys;

		private System.Web.UI.HtmlControls.HtmlInputText txtkeywords;

		private System.Web.UI.WebControls.Literal Puserid;

		private System.Web.UI.WebControls.Literal litProtuctSelNum;

		private System.Web.UI.WebControls.Literal litProtuctNoSelNum;

		protected override void OnInit(System.EventArgs e)
		{
			string a = System.Web.HttpContext.Current.Request["task"];
			if (a == "next")
			{
				int.TryParse(System.Web.HttpContext.Current.Request["categoryId"], out this.categoryId);
				this.keyWord = System.Web.HttpContext.Current.Request["keyWord"];
				string text = System.Web.HttpContext.Current.Request["pgSize"];
				string text2 = System.Web.HttpContext.Current.Request["pgIndex"];
				if (string.IsNullOrEmpty(text))
				{
					text = "20";
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 = "2";
				}
				int num;
				DataTable products = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, new int?(this.categoryId), this.keyWord, int.Parse(text2), int.Parse(text), out num, "DisplaySequence", "desc", true);
				System.Web.HttpContext.Current.Response.Write(JsonConvert.SerializeObject(products));
				System.Web.HttpContext.Current.Response.End();
			}
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-MyProducts.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("已上架商品");
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			this.keyWord = this.Page.Request["keyWord"];
			if (!string.IsNullOrWhiteSpace(this.keyWord))
			{
				this.keyWord = this.keyWord.Trim();
			}
			this.txtkeywords = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("keywords");
			this.rpChooseProducts = (VshopTemplatedRepeater)this.FindControl("rpChooseProducts");
			this.rpCategorys = (VshopTemplatedRepeater)this.FindControl("rpCategorys");
			this.Puserid = (System.Web.UI.WebControls.Literal)this.FindControl("Puserid");
			this.litProtuctSelNum = (System.Web.UI.WebControls.Literal)this.FindControl("litProtuctSelNum");
			this.litProtuctNoSelNum = (System.Web.UI.WebControls.Literal)this.FindControl("litProtuctNoSelNum");
			this.Puserid.Text = Globals.GetCurrentMemberUserId().ToString();
			this.DataBindSoruce();
		}

		private void DataBindSoruce()
		{
			this.txtkeywords.Value = this.keyWord;
			this.rpCategorys.DataSource = CategoryBrowser.GetCategories();
			this.rpCategorys.DataBind();
			int num;
			this.rpChooseProducts.DataSource = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, new int?(this.categoryId), this.keyWord, 1, 20, out num, "DisplaySequence", "desc", true);
			this.rpChooseProducts.DataBind();
			this.litProtuctSelNum.Text = "（" + ProductBrowser.GetProductsNumber(true).ToString() + "）";
			this.litProtuctNoSelNum.Text = "（" + ProductBrowser.GetProductsNumber(false).ToString() + "）";
		}
	}
}
