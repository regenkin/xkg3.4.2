using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class DistributorRequest : VMemberTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlInputHidden litIsEnable;

		private System.Web.UI.HtmlControls.HtmlImage idImg;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VDistributorRequest.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("申请分销");
			this.Page.Session["stylestatus"] = "2";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (string.IsNullOrEmpty(currentMember.UserBindName))
			{
				this.Page.Response.Redirect("/BindUserMessage.aspx?returnUrl=/Vshop/DistributorValid.aspx", true);
				this.Page.Response.End();
			}
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMember.UserId);
			if (userIdDistributors != null && userIdDistributors.ReferralStatus == 0)
			{
				this.Page.Response.Redirect("/Vshop/DistributorCenter.aspx", true);
				this.Page.Response.End();
			}
			bool flag = false;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			if (masterSettings.DistributorApplicationCondition)
			{
				decimal expenditure = currentMember.Expenditure;
				int finishedOrderMoney = masterSettings.FinishedOrderMoney;
				if (finishedOrderMoney > 0)
				{
					decimal num = 0m;
					DataTable userOrderPaidWaitFinish = OrderHelper.GetUserOrderPaidWaitFinish(currentMember.UserId);
					for (int i = 0; i < userOrderPaidWaitFinish.Rows.Count; i++)
					{
						OrderInfo orderInfo = OrderHelper.GetOrderInfo(userOrderPaidWaitFinish.Rows[i]["orderid"].ToString());
						if (orderInfo != null)
						{
							decimal total = orderInfo.GetTotal();
							if (total > 0m)
							{
								num += total;
							}
						}
					}
					if (currentMember.Expenditure + num >= finishedOrderMoney)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					if (masterSettings.EnableDistributorApplicationCondition)
					{
						if (!string.IsNullOrEmpty(masterSettings.DistributorProductsDate) && !string.IsNullOrEmpty(masterSettings.DistributorProducts))
						{
							if (masterSettings.DistributorProductsDate.Contains("|"))
							{
								System.DateTime value = default(System.DateTime);
								System.DateTime value2 = default(System.DateTime);
								bool flag2 = System.DateTime.TryParse(masterSettings.DistributorProductsDate.Split(new char[]
								{
									'|'
								})[0].ToString(), out value);
								bool flag3 = System.DateTime.TryParse(masterSettings.DistributorProductsDate.Split(new char[]
								{
									'|'
								})[1].ToString(), out value2);
								if (flag2 && flag3 && System.DateTime.Now.CompareTo(value) >= 0 && System.DateTime.Now.CompareTo(value2) < 0)
								{
									if (MemberProcessor.CheckMemberIsBuyProds(currentMember.UserId, masterSettings.DistributorProducts, new System.DateTime?(value), new System.DateTime?(value2)))
									{
										flag = true;
									}
								}
							}
						}
					}
				}
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				this.Page.Response.Redirect("/Vshop/DistributorRegCheck.aspx", true);
				this.Page.Response.End();
			}
			int num2 = 0;
			this.idImg = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("idImg");
			string text = string.Empty;
			if (int.TryParse(this.Page.Request.QueryString["ReferralId"], out num2))
			{
				if (num2 > 0)
				{
					DistributorsInfo userIdDistributors2 = DistributorsBrower.GetUserIdDistributors(num2);
					if (userIdDistributors2 != null)
					{
						if (!string.IsNullOrEmpty(userIdDistributors2.Logo))
						{
							text = userIdDistributors2.Logo;
						}
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				text = masterSettings.DistributorLogoPic;
			}
			this.idImg.Src = text;
			if (userIdDistributors != null && userIdDistributors.ReferralStatus != 0)
			{
				this.litIsEnable = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litIsEnable");
				this.litIsEnable.Value = userIdDistributors.ReferralStatus.ToString();
			}
		}
	}
}
