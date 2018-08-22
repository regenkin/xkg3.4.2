using ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VGetRedShare : VshopTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litItemParams;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VGetRedShare.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litItemParams = (System.Web.UI.WebControls.Literal)this.FindControl("litItemParams");
			string text = System.Web.HttpContext.Current.Request.QueryString.Get("orderid");
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (!string.IsNullOrEmpty(text) && currentMember != null)
			{
				DataTable orderRedPager = ShareActHelper.GetOrderRedPager(text, currentMember.UserId);
				if (orderRedPager != null && orderRedPager.Rows.Count > 0)
				{
					DataView defaultView = orderRedPager.DefaultView;
					if (defaultView.Count > 0)
					{
						ShareActivityInfo act = ShareActHelper.GetAct(System.Convert.ToInt32(defaultView[0]["RedPagerActivityId"]));
						if (act != null)
						{
							string text2 = act.ShareTitle;
							string text3 = act.Description;
							if (text2.Contains("{{店铺名称}}") || text3.Contains("{{店铺名称}}"))
							{
								System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Vshop-ReferralId");
								if (httpCookie != null && httpCookie.Value != null)
								{
									DistributorsInfo distributorsInfo = new DistributorsInfo();
									distributorsInfo = DistributorsBrower.GetUserIdDistributors(int.Parse(httpCookie.Value));
									text3 = text3.Replace("{{店铺名称}}", distributorsInfo.StoreName);
									text2 = text2.Replace("{{店铺名称}}", distributorsInfo.StoreName);
								}
								else
								{
									SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
									text3 = text3.Replace("{{店铺名称}}", masterSettings.SiteName);
									text2 = text2.Replace("{{店铺名称}}", masterSettings.SiteName);
								}
							}
							if (text2.Contains("{{微信昵称}}"))
							{
								text2 = text2.Replace("{{微信昵称}}", currentMember.UserName);
							}
							if (text3.Contains("{{微信昵称}}"))
							{
								text3 = text3.Replace("{{微信昵称}}", currentMember.UserName);
							}
							string webUrlStart = Globals.GetWebUrlStart();
							this.litItemParams.Text = string.Concat(new object[]
							{
								webUrlStart,
								act.ImgUrl,
								"|",
								text2.Replace("|", "｜"),
								"|",
								Globals.GetWebUrlStart(),
								"/Vshop/getredpager.aspx?id=",
								defaultView[0]["RedPagerActivityId"],
								"&userid=",
								currentMember.UserId,
								"&ReferralId=",
								Globals.GetCurrentDistributorId(),
								"|",
								text3.Replace("|", "｜")
							});
						}
					}
					else
					{
						System.Web.HttpContext.Current.Response.Redirect("/vshop/MemberCenter.aspx?t=1");
					}
				}
				else
				{
					orderRedPager = ShareActHelper.GetOrderRedPager(text, -100);
					if (orderRedPager.Rows.Count > 0)
					{
						System.Web.HttpContext.Current.Response.Redirect(string.Concat(new object[]
						{
							"/Vshop/getredpager.aspx?id=",
							orderRedPager.Rows[0]["RedPagerActivityId"].ToString(),
							"&userid=",
							currentMember.UserId,
							"&ReferralId=",
							Globals.GetCurrentDistributorId()
						}));
						System.Web.HttpContext.Current.Response.End();
					}
					else
					{
						System.Web.HttpContext.Current.Response.Redirect("/vshop/MemberCenter.aspx?t=2");
					}
				}
			}
			else
			{
				System.Web.HttpContext.Current.Response.Redirect("/default.aspx");
				System.Web.HttpContext.Current.Response.End();
			}
			PageTitle.AddSiteNameTitle("分享助力");
		}
	}
}
