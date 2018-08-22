using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMyCouponLists : VMemberTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtShowTabNum;

		private VshopTemplatedRepeater rptActivity;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VMyCouponLists.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("我的优惠券");
			this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.txtShowTabNum = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtShowTabNum");
			this.rptActivity = (VshopTemplatedRepeater)this.FindControl("rptActivity");
			string value = "0";
			this.txtShowTabNum.Value = value;
			PrizesDeliveQuery prizesDeliveQuery = new PrizesDeliveQuery();
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
			MemberCouponsSearch memberCouponsSearch = new MemberCouponsSearch();
			memberCouponsSearch.CouponName = "";
			memberCouponsSearch.Status = "0";
			memberCouponsSearch.MemberId = Globals.GetCurrentMemberUserId();
			memberCouponsSearch.IsCount = true;
			memberCouponsSearch.PageIndex = pageIndex;
			memberCouponsSearch.PageSize = pageSize;
			memberCouponsSearch.SortBy = "CouponId";
			memberCouponsSearch.SortOrder = SortAction.Desc;
			int num = 0;
			DataTable memberCoupons = CouponHelper.GetMemberCoupons(memberCouponsSearch, ref num);
			if (memberCoupons != null)
			{
				if (memberCoupons.Rows.Count > 0)
				{
					memberCoupons.Columns.Add("useConditon");
					memberCoupons.Columns.Add("sStatus");
					for (int i = 0; i < memberCoupons.Rows.Count; i++)
					{
						decimal d = decimal.Parse(memberCoupons.Rows[i]["ConditionValue"].ToString());
						if (d == 0m)
						{
							memberCoupons.Rows[i]["useConditon"] = "无消费限制";
						}
						else
						{
							memberCoupons.Rows[i]["useConditon"] = "满" + d.ToString("F2") + "可使用";
						}
						memberCoupons.Rows[i]["sStatus"] = ((int.Parse(memberCoupons.Rows[i]["Status"].ToString()) == 0) ? "已领用" : "已使用");
					}
				}
			}
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			this.rptActivity.DataSource = memberCoupons;
			this.rptActivity.DataBind();
			this.txtTotal.SetWhenIsNotNull(num.ToString());
		}
	}
}
