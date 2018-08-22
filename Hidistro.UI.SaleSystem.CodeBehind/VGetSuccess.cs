using ControlPanel.Promotions;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VGetSuccess : VshopTemplatedWebControl
	{
		private System.Web.UI.WebControls.Panel divNoLogin;

		private System.Web.UI.WebControls.Panel divNoNum;

		private System.Web.UI.WebControls.Panel divSuccess;

		private System.Web.UI.WebControls.Panel divError;

		private System.Web.UI.WebControls.Literal ltGetTotal;

		private System.Web.UI.WebControls.Literal ltOrderAmountCanUse;

		private System.Web.UI.WebControls.Literal ltExpiryTime;

		private System.Web.UI.WebControls.Literal ltRedPagerActivityName;

		private System.Web.UI.WebControls.Literal ltRedPagerActivityNameForOrders;

		private System.Web.UI.WebControls.Literal ltRedPagerLimit;

		private System.Web.UI.WebControls.Literal ltErrorMessage;

		private System.Web.UI.WebControls.HyperLink hlinkLogin;

		private System.Web.UI.HtmlControls.HtmlInputHidden hdCondition;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VGetSuccess.html";
			}
			base.OnInit(e);
		}

		private string GetResponseResult(string url)
		{
			System.Net.WebRequest webRequest = System.Net.WebRequest.Create(url);
			string result;
			using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webRequest.GetResponse())
			{
				using (System.IO.Stream responseStream = httpWebResponse.GetResponseStream())
				{
					using (System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}
			return result;
		}

		public string getopenid()
		{
			return "";
		}

		protected override void AttachChildControls()
		{
			string text = System.Web.HttpContext.Current.Request.QueryString.Get("m");
			string text2 = System.Web.HttpContext.Current.Request.QueryString.Get("type");
			this.ltGetTotal = (System.Web.UI.WebControls.Literal)this.FindControl("ltGetTotal");
			this.ltOrderAmountCanUse = (System.Web.UI.WebControls.Literal)this.FindControl("ltOrderAmountCanUse");
			this.ltExpiryTime = (System.Web.UI.WebControls.Literal)this.FindControl("ltExpiryTime");
			this.ltRedPagerActivityName = (System.Web.UI.WebControls.Literal)this.FindControl("ltRedPagerActivityName");
			this.ltRedPagerActivityNameForOrders = (System.Web.UI.WebControls.Literal)this.FindControl("ltRedPagerActivityNameForOrders");
			this.ltRedPagerLimit = (System.Web.UI.WebControls.Literal)this.FindControl("ltRedPagerLimit");
			this.ltErrorMessage = (System.Web.UI.WebControls.Literal)this.FindControl("ltErrorMessage");
			this.divNoLogin = (System.Web.UI.WebControls.Panel)this.FindControl("divNoLogin");
			this.divNoNum = (System.Web.UI.WebControls.Panel)this.FindControl("divNoNum");
			this.divSuccess = (System.Web.UI.WebControls.Panel)this.FindControl("divSuccess");
			this.divError = (System.Web.UI.WebControls.Panel)this.FindControl("divError");
			this.hdCondition = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdCondition");
			this.hlinkLogin = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkLogin");
			if (text2 == "1" || text2 == "5")
			{
				int num = 0;
				int.TryParse(text, out num);
				if (num > 0)
				{
					int id = 0;
					int.TryParse(System.Web.HttpContext.Current.Request["id"], out id);
					ShareActivityInfo act = ShareActHelper.GetAct(id);
					if (act != null)
					{
						CouponInfo coupon = CouponHelper.GetCoupon(act.CouponId);
						this.ltGetTotal.Text = coupon.CouponValue.ToString("F2").Replace(".00", "");
						this.ltOrderAmountCanUse.Text = coupon.ConditionValue.ToString("F2").Replace(".00", "");
						this.hdCondition.SetWhenIsNotNull(coupon.ConditionValue.ToString("F2").Replace(".00", ""));
						this.ltExpiryTime.Text = coupon.EndDate.ToString("yyyy-MM-dd");
						if (text2 == "5")
						{
							this.ltRedPagerActivityName.Text = "该券已经到你的钱包了</div><div class='get-red-explain'><a href='/Vshop/MyCouponLists.aspx'>点击查看</a>";
						}
						else
						{
							this.ltRedPagerActivityName.Text = (coupon.CouponName ?? "");
						}
						if (coupon.IsAllProduct)
						{
							this.ltRedPagerLimit.Text = "该券可用于任意商品的抵扣";
						}
						else
						{
							string couponProductIds = CouponHelper.GetCouponProductIds(act.CouponId);
							this.ltRedPagerLimit.Text = "该券可用于部分商品的抵扣</div><div class='get-red-explain'><a href='/ProductList.aspx?pIds='" + couponProductIds + ">查看商品</a>";
						}
						this.divSuccess.Visible = true;
					}
				}
				PageTitle.AddSiteNameTitle("成功获取优惠券");
			}
			else
			{
				string text3 = text2;
				if (text3 != null)
				{
					if (!(text3 == "-1"))
					{
						if (!(text3 == "-2"))
						{
							if (!(text3 == "-4"))
							{
								if (text3 == "-3")
								{
									this.divNoNum.Visible = true;
								}
							}
							else
							{
								this.divNoLogin.Visible = true;
							}
						}
						else
						{
							this.ltErrorMessage.Text = text;
							this.divError.Visible = true;
						}
					}
					else
					{
						int id = 0;
						int.TryParse(System.Web.HttpContext.Current.Request["id"], out id);
						ShareActivityInfo act = ShareActHelper.GetAct(id);
						if (act != null)
						{
							CouponInfo coupon = CouponHelper.GetCoupon(act.CouponId);
							if (coupon != null)
							{
								this.ltRedPagerActivityNameForOrders.Text = coupon.CouponName;
								string s = string.Concat(new object[]
								{
									Globals.GetWebUrlStart(),
									"/Vshop/GetRedPager.aspx?id=",
									id.ToString(),
									"&userid=",
									Globals.GetCurrentMemberUserId(),
									"&ReferralId=",
									Globals.GetCurrentDistributorId()
								});
								this.hlinkLogin.NavigateUrl = "/UserLogining.aspx?returnUrl=" + System.Web.HttpContext.Current.Server.UrlEncode(s);
								this.divNoLogin.Visible = true;
							}
							else
							{
								System.Web.HttpContext.Current.Response.Redirect("/default.aspx");
								System.Web.HttpContext.Current.Response.End();
							}
						}
						else
						{
							System.Web.HttpContext.Current.Response.Redirect("/default.aspx");
							System.Web.HttpContext.Current.Response.End();
						}
					}
				}
				PageTitle.AddSiteNameTitle("获取优惠券");
			}
		}
	}
}
