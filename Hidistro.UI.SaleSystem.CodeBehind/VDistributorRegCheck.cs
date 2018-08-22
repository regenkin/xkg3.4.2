using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class VDistributorRegCheck : VshopTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlInputHidden litExpenditure;

		private System.Web.UI.HtmlControls.HtmlInputHidden litIsMember;

		private System.Web.UI.HtmlControls.HtmlInputHidden litIsEnable;

		private System.Web.UI.HtmlControls.HtmlInputHidden litminMoney;

		private System.Web.UI.HtmlControls.HtmlInputHidden litProds;

		private System.Web.UI.HtmlControls.HtmlInputHidden litProdOK;

		private System.Web.UI.HtmlControls.HtmlInputHidden UserBindName;

		protected string IsEnable = "0";

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VDistributorRegCheck.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litIsEnable = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litIsEnable");
			this.litIsMember = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litIsMember");
			this.litExpenditure = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litExpenditure");
			this.litminMoney = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litminMoney");
			this.litProds = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litProds");
			this.litProdOK = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litProdOK");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			int currentMemberUserId = Globals.GetCurrentMemberUserId();
			if (currentMemberUserId > 0)
			{
				this.litIsMember.Value = "1";
				DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				if (currentMember == null)
				{
					this.Page.Response.Redirect("/Vshop/DistributorCenter.aspx");
					return;
				}
				this.UserBindName = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("UserBindName");
				this.UserBindName.Value = currentMember.UserBindName;
				decimal d = currentMember.Expenditure;
				if (userIdDistributors != null)
				{
					if (userIdDistributors.ReferralStatus == 0)
					{
						this.IsEnable = "1";
						this.Context.Response.Redirect("/Vshop/DistributorCenter.aspx");
						this.Context.Response.End();
					}
					else if (userIdDistributors.ReferralStatus == 1)
					{
						this.IsEnable = "3";
					}
					else if (userIdDistributors.ReferralStatus == 9)
					{
						this.IsEnable = "9";
					}
				}
				else
				{
					decimal num = 0m;
					DataTable userOrderPaidWaitFinish = OrderHelper.GetUserOrderPaidWaitFinish(currentMemberUserId);
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
					d += num;
					if (!masterSettings.DistributorApplicationCondition)
					{
						bool flag = SystemAuthorizationHelper.CheckDistributorIsCanAuthorization();
						if (flag)
						{
							this.IsEnable = "2";
						}
						else
						{
							this.IsEnable = "4";
						}
					}
					else
					{
						int finishedOrderMoney = masterSettings.FinishedOrderMoney;
						this.litminMoney.Value = finishedOrderMoney.ToString();
						if (finishedOrderMoney > 0 && d >= finishedOrderMoney)
						{
							bool flag = SystemAuthorizationHelper.CheckDistributorIsCanAuthorization();
							if (flag)
							{
								this.IsEnable = "2";
							}
							else
							{
								this.IsEnable = "4";
							}
						}
						if (masterSettings.EnableDistributorApplicationCondition)
						{
							if (!string.IsNullOrEmpty(masterSettings.DistributorProductsDate))
							{
								if (!string.IsNullOrEmpty(masterSettings.DistributorProducts))
								{
									this.litProds.Value = masterSettings.DistributorProducts;
									if (masterSettings.DistributorProductsDate.Contains("|"))
									{
										System.DateTime value = default(System.DateTime);
										System.DateTime value2 = default(System.DateTime);
										System.DateTime.TryParse(masterSettings.DistributorProductsDate.Split(new char[]
										{
											'|'
										})[0].ToString(), out value);
										System.DateTime.TryParse(masterSettings.DistributorProductsDate.Split(new char[]
										{
											'|'
										})[1].ToString(), out value2);
										if (value.CompareTo(System.DateTime.Now) > 0 || value2.CompareTo(System.DateTime.Now) < 0)
										{
											this.litProds.Value = "";
											this.litIsEnable.Value = "0";
										}
										else if (MemberProcessor.CheckMemberIsBuyProds(currentMemberUserId, this.litProds.Value, new System.DateTime?(value), new System.DateTime?(value2)))
										{
											bool flag = SystemAuthorizationHelper.CheckDistributorIsCanAuthorization();
											if (flag)
											{
												this.IsEnable = "2";
												this.litProdOK.Value = "(已购买指定商品,在" + value2.ToString("yyyy-MM-dd") + "之前申请有效)";
											}
											else
											{
												this.IsEnable = "4";
											}
										}
									}
								}
								else
								{
									this.IsEnable = "6";
								}
							}
						}
					}
				}
				this.litExpenditure.Value = d.ToString("F2");
			}
			else
			{
				this.litIsMember.Value = "0";
			}
			this.litIsEnable.Value = this.IsEnable;
			PageTitle.AddSiteNameTitle("申请分销商");
		}
	}
}
