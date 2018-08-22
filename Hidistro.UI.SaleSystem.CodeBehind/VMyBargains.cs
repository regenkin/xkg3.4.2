using Hidistro.ControlPanel.Bargain;
using Hidistro.Core.Entities;
using Hidistro.Entities.Bargain;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class VMyBargains : VshopTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlInputHidden hiddTotal;

		private System.Web.UI.HtmlControls.HtmlInputHidden hiddPageIndex;

		private VshopTemplatedRepeater rpMyBargainList;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMyBargains.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("我的砍价");
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember != null)
			{
				this.rpMyBargainList = (VshopTemplatedRepeater)this.FindControl("rpMyBargainList");
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
				int status = 0;
				if (!int.TryParse(this.Page.Request.QueryString["status"], out status))
				{
					status = 0;
				}
				DbQueryResult myBargainList = BargainHelper.GetMyBargainList(new BargainQuery
				{
					PageSize = pageSize,
					PageIndex = pageIndex,
					UserId = currentMember.UserId,
					Status = status
				});
				this.hiddTotal.Value = ((DataTable)myBargainList.Data).Rows.Count.ToString();
				this.hiddPageIndex.Value = pageIndex.ToString();
				this.rpMyBargainList.DataSource = myBargainList.Data;
				this.rpMyBargainList.DataBind();
			}
		}
	}
}
