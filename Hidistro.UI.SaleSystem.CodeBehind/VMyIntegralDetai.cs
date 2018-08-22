using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMyIntegralDetai : VMemberTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtShowTabNum;

		private VshopTemplatedRepeater rptIntegarlDetail0;

		private VshopTemplatedRepeater rptIntegarlDetail1;

		private VshopTemplatedRepeater rptIntegarlDetail2;

		private System.Web.UI.WebControls.Literal litStatus0;

		private System.Web.UI.WebControls.Literal litStatus1;

		private System.Web.UI.WebControls.Literal litStatus2;

		private System.Web.UI.WebControls.Literal littableList0;

		private System.Web.UI.WebControls.Literal littableList1;

		private System.Web.UI.WebControls.Literal littableList2;

		private System.Web.UI.WebControls.Literal litSurplusIntegral;

		private System.Web.UI.WebControls.Literal litSumIntegral;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VMyIntegralDetail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litSurplusIntegral = (System.Web.UI.WebControls.Literal)this.FindControl("litSurplusIntegral");
			this.litSumIntegral = (System.Web.UI.WebControls.Literal)this.FindControl("litSumIntegral");
			this.litStatus0 = (System.Web.UI.WebControls.Literal)this.FindControl("litStatus0");
			this.litStatus1 = (System.Web.UI.WebControls.Literal)this.FindControl("litStatus1");
			this.litStatus2 = (System.Web.UI.WebControls.Literal)this.FindControl("litStatus2");
			this.littableList0 = (System.Web.UI.WebControls.Literal)this.FindControl("littableList0");
			this.littableList1 = (System.Web.UI.WebControls.Literal)this.FindControl("littableList1");
			this.littableList2 = (System.Web.UI.WebControls.Literal)this.FindControl("littableList2");
			this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.txtShowTabNum = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtShowTabNum");
			this.rptIntegarlDetail0 = (VshopTemplatedRepeater)this.FindControl("rptIntegarlDetail0");
			this.rptIntegarlDetail1 = (VshopTemplatedRepeater)this.FindControl("rptIntegarlDetail1");
			this.rptIntegarlDetail2 = (VshopTemplatedRepeater)this.FindControl("rptIntegarlDetail2");
			int num = 0;
			int pageIndex;
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int pageSize;
			if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
			{
				pageSize = 10;
			}
			IntegralDetailQuery integralDetailQuery = new IntegralDetailQuery();
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			this.litSurplusIntegral.Text = currentMember.Points.ToString();
			this.litSumIntegral.Text = System.Convert.ToInt32(MemberProcessor.GetIntegral(currentMember.UserId)).ToString();
			integralDetailQuery.UserId = currentMember.UserId;
			integralDetailQuery.PageIndex = pageIndex;
			integralDetailQuery.PageSize = pageSize;
			if (int.TryParse(this.Page.Request.QueryString["IntegralSourceType"], out num))
			{
				if (num == 0)
				{
					integralDetailQuery.IntegralSourceType = num;
					DbQueryResult integralDetail = MemberProcessor.GetIntegralDetail(integralDetailQuery);
					this.rptIntegarlDetail0.DataSource = integralDetail.Data;
					this.rptIntegarlDetail0.DataBind();
					this.litStatus0.Text = "class=\"active\"";
					this.litStatus1.Text = "";
					this.litStatus2.Text = "";
					this.littableList0.Text = "style=\"display: block;\"";
					this.littableList1.Text = "style=\"display: none;\"";
					this.littableList2.Text = "style=\"display: none;\"";
					this.txtTotal.SetWhenIsNotNull(integralDetail.TotalRecords.ToString());
				}
				else if (num == 1)
				{
					integralDetailQuery.IntegralSourceType = num;
					DbQueryResult integralDetail = MemberProcessor.GetIntegralDetail(integralDetailQuery);
					this.rptIntegarlDetail1.DataSource = integralDetail.Data;
					this.rptIntegarlDetail1.DataBind();
					this.litStatus0.Text = "";
					this.litStatus1.Text = "class=\"active\"";
					this.litStatus2.Text = "";
					this.littableList0.Text = "style=\"display: none ;\"";
					this.littableList1.Text = "style=\"display:block;\"";
					this.littableList2.Text = "style=\"display: none;\"";
					this.txtTotal.SetWhenIsNotNull(integralDetail.TotalRecords.ToString());
				}
				else
				{
					integralDetailQuery.IntegralSourceType = num;
					DbQueryResult integralDetail = MemberProcessor.GetIntegralDetail(integralDetailQuery);
					this.rptIntegarlDetail2.DataSource = integralDetail.Data;
					this.rptIntegarlDetail2.DataBind();
					this.litStatus0.Text = "";
					this.litStatus1.Text = "";
					this.litStatus2.Text = "class=\"active\"";
					this.littableList0.Text = "style=\"display: none ;\"";
					this.littableList1.Text = "style=\"display:none;\"";
					this.littableList2.Text = "style=\"display: block;\"";
					this.txtTotal.SetWhenIsNotNull(integralDetail.TotalRecords.ToString());
				}
			}
			PageTitle.AddSiteNameTitle("积分明细");
		}
	}
}
