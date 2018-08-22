using Hidistro.Core;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class VMyluckNum : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litTitle;

		private System.Web.UI.WebControls.Literal litList;

		private System.Web.UI.WebControls.Literal litBuysum;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMyluckNum.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litBuysum = (System.Web.UI.WebControls.Literal)this.FindControl("litBuysum");
			this.litList = (System.Web.UI.WebControls.Literal)this.FindControl("litList");
			this.litTitle = (System.Web.UI.WebControls.Literal)this.FindControl("litTitle");
			string text = Globals.RequestQueryStr("vaid");
			if (string.IsNullOrEmpty(text))
			{
				base.GotoResourceNotFound("");
			}
			OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(text);
			if (oneyuanTaoInfoById == null)
			{
				base.GotoResourceNotFound("");
			}
			this.litTitle.Text = oneyuanTaoInfoById.ProductTitle;
			System.Collections.Generic.IList<LuckInfo> luckInfoListByAId = OneyuanTaoHelp.getLuckInfoListByAId(text, this.CurrentMemberInfo.UserId);
			if (luckInfoListByAId == null)
			{
				base.GotoResourceNotFound("");
			}
			this.litBuysum.Text = luckInfoListByAId.Count.ToString();
			System.DateTime buyTime = luckInfoListByAId[0].BuyTime;
			string text2 = "<p class='timeline'>" + buyTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "</p><ul class='luckul'>";
			foreach (LuckInfo current in luckInfoListByAId)
			{
				if (buyTime != current.BuyTime)
				{
					buyTime = current.BuyTime;
					text2 = "<p class='timeline'>" + buyTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "</p><ul class='luckul'>";
				}
				text2 += "<li";
				if (current.IsWin)
				{
					text2 += " class='Win' ";
				}
				text2 += ">";
				text2 = text2 + current.PrizeNum + "</li>";
			}
			this.litList.Text = text2;
			PageTitle.AddSiteNameTitle("我的号码");
		}
	}
}
