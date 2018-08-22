using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VCategoryList : VshopTemplatedWebControl
	{
		private VshopTemplatedRepeater rptCategories;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VCategoryList.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
			DataSet categoryList = CategoryBrowser.GetCategoryList();
			this.rptCategories.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptCategories_ItemDataBound);
			this.rptCategories.DataSource = categoryList;
			this.rptCategories.DataBind();
			PageTitle.AddSiteNameTitle("商品分类");
		}

		private void rptCategories_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			DataView dataView = (DataView)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "SubCategories");
			DataRowView dataRowView = (DataRowView)e.Item.DataItem;
			int num = System.Convert.ToInt32(dataRowView["CategoryId"]);
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litPlus");
			if (dataView == null || dataView.ToTable().Rows.Count == 0)
			{
				literal.Visible = false;
			}
			else
			{
				literal.Visible = true;
			}
		}
	}
}
