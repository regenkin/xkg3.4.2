using Hidistro.ControlPanel.Bargain;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Bargain;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class VBargainList : VshopTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlInputHidden hiddTotal;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddPageIndex;

		private VshopTemplatedRepeater rpMyMemberList;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VBargainList.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("砍价活动");
			this.rpMyMemberList = (VshopTemplatedRepeater)this.FindControl("rpBargainList");
			this.hiddTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddTotal");
			this.hiddPageIndex = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiddPageIndex");
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
			int num = Globals.RequestQueryNum("status");
			BargainQuery bargainQuery = new BargainQuery();
			bargainQuery.Type = num.ToString();
			bargainQuery.PageSize = pageSize;
			bargainQuery.PageIndex = pageIndex;
			int total = BargainHelper.GetTotal(bargainQuery);
			DbQueryResult bargainList = BargainHelper.GetBargainList(bargainQuery);
			this.hiddTotal.Value = ((DataTable)bargainList.Data).Rows.Count.ToString();
			this.hiddPageIndex.Value = pageIndex.ToString();
			this.rpMyMemberList.DataSource = bargainList.Data;
			this.rpMyMemberList.DataBind();
		}
	}
}
