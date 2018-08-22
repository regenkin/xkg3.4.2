using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VActivityDetail : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rptProducts;

		private System.Web.UI.WebControls.Literal litdescription;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vActivityDetail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
			this.litdescription = (System.Web.UI.WebControls.Literal)this.FindControl("litdescription");
			ProductQuery productQuery = new ProductQuery();
			int value = 0;
			int activitiesId = 0;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]) && !string.IsNullOrEmpty(this.Page.Request.QueryString["ActivitiesId"]))
			{
				if (!int.TryParse(this.Page.Request.QueryString["CategoryId"], out value))
				{
					this.Page.Response.Redirect("Default.aspx");
				}
				else
				{
					int.TryParse(this.Page.Request.QueryString["ActivitiesId"], out activitiesId);
					productQuery.CategoryId = new int?(value);
					DataTable activitie = ProductBrowser.GetActivitie(activitiesId);
					if (activitie.Rows.Count > 0)
					{
						this.litdescription.Text = activitie.Rows[0]["ActivitiesDescription"].ToString();
						if (this.rptProducts != null)
						{
							if (productQuery.CategoryId > 0)
							{
								productQuery.PageSize = 20;
								productQuery.PageIndex = 1;
								DbQueryResult homeProduct = ProductBrowser.GetHomeProduct(MemberProcessor.GetCurrentMember(), productQuery);
								this.rptProducts.DataSource = homeProduct.Data;
								this.rptProducts.DataBind();
							}
						}
					}
					else
					{
						this.Page.Response.Redirect("Default.aspx");
					}
				}
			}
			PageTitle.AddSiteNameTitle("满减活动");
		}
	}
}
