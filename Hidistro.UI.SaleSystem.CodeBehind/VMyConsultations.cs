using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMyConsultations : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rptProducts;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMyConsultations.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
			this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			int pageIndex;
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int pageSize;
			if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
			{
				pageSize = 20;
			}
			DbQueryResult productConsultations = ProductBrowser.GetProductConsultations(new ProductConsultationAndReplyQuery
			{
				UserId = currentMember.UserId,
				IsCount = true,
				PageIndex = pageIndex,
				PageSize = pageSize,
				SortBy = "ConsultationId",
				SortOrder = SortAction.Desc
			});
			this.rptProducts.DataSource = productConsultations.Data;
			this.rptProducts.DataBind();
			this.txtTotal.SetWhenIsNotNull(productConsultations.TotalRecords.ToString());
			PageTitle.AddSiteNameTitle("商品咨询");
		}
	}
}
