using ControlPanel.Promotions;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VSubmmitOrder : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litShipTo;

		private System.Web.UI.WebControls.Literal litCellPhone;

		private System.Web.UI.WebControls.Literal litAddress;

		private System.Web.UI.WebControls.Literal litShowMes;

		private System.Web.UI.HtmlControls.HtmlInputControl groupbuyHiddenBox;

		private VshopTemplatedRepeater rptCartProducts;

		private VshopTemplatedRepeater rptAddress;

		private System.Web.UI.WebControls.Literal litOrderTotal;

		private System.Web.UI.WebControls.Literal litPointNumber;

		private System.Web.UI.WebControls.Literal litDisplayPointNumber;

		private System.Web.UI.HtmlControls.HtmlInputHidden selectShipTo;

		private System.Web.UI.HtmlControls.HtmlInputHidden regionId;

		private System.Web.UI.HtmlControls.HtmlAnchor aLinkToShipping;

		private System.Web.UI.WebControls.Literal litAddAddress;

		private int buyAmount;

		private string productSku;

		public DataTable GetUserCoupons = null;

		private DataTable dtActivities = ActivityHelper.GetActivities();

		private bool isbargain = Globals.RequestQueryNum("bargainDetialId") > 0;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VSubmmitOrder.html";
			}
			base.OnInit(e);
		}

		private void rptCartProducts_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Collections.Generic.List<ShoppingCartItemInfo> list = (System.Collections.Generic.List<ShoppingCartItemInfo>)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "LineItems");
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("LitCoupon");
				System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litExemption");
				System.Web.UI.WebControls.Literal literal3 = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litoldExemption");
				System.Web.UI.WebControls.Literal literal4 = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litoldTotal");
				System.Web.UI.WebControls.Literal literal5 = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litTotal");
				System.Web.UI.WebControls.Literal literal6 = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litbFreeShipping");
				string text = "";
				string text2 = " <div class=\"btn-group coupon\">";
				object obj = text2;
				text2 = string.Concat(new object[]
				{
					obj,
					"<button type=\"button\" class=\"btn btn-default dropdown-toggle coupondropdown\" data-toggle=\"dropdown\"   id='coupondropdown",
					System.Web.UI.DataBinder.Eval(e.Item.DataItem, "TemplateId"),
					"'>选择优惠券<span class=\"caret\"></span></button>"
				});
				obj = text2;
				text2 = string.Concat(new object[]
				{
					obj,
					"<ul id=\"coupon",
					System.Web.UI.DataBinder.Eval(e.Item.DataItem, "TemplateId"),
					"\" class=\"dropdown-menu\" role=\"menu\">"
				});
				if (this.GetUserCoupons.Rows.Count > 0 && !this.isbargain)
				{
					obj = text;
					text = string.Concat(new object[]
					{
						obj,
						"<li><a onclick=\"Couponasetselect('",
						System.Web.UI.DataBinder.Eval(e.Item.DataItem, "TemplateId"),
						"','不使用','0',0,'0')\"   value=\"0\">不使用</a></li>"
					});
				}
				if (!this.isbargain)
				{
					for (int i = 0; i < this.GetUserCoupons.Rows.Count; i++)
					{
						if (this.GetUserCoupons.Rows[i]["MemberGrades"].ToString() == "0" || this.GetUserCoupons.Rows[i]["MemberGrades"].ToString() == this.CurrentMemberInfo.GradeId.ToString())
						{
							if (bool.Parse(this.GetUserCoupons.Rows[i]["IsAllProduct"].ToString()))
							{
								decimal num = 0m;
								foreach (ShoppingCartItemInfo current in list)
								{
									if (current.Type == 0)
									{
										num += current.SubTotal;
									}
								}
								if (decimal.Parse(this.GetUserCoupons.Rows[i]["ConditionValue"].ToString()) <= num)
								{
									obj = text;
									text = string.Concat(new object[]
									{
										obj,
										"<li><a onclick=\"Couponasetselect('",
										System.Web.UI.DataBinder.Eval(e.Item.DataItem, "TemplateId"),
										"','",
										this.GetUserCoupons.Rows[i]["CouponValue"],
										"元现金券','",
										this.GetUserCoupons.Rows[i]["CouponValue"],
										"',",
										this.GetUserCoupons.Rows[i]["Id"],
										",'",
										this.GetUserCoupons.Rows[i]["CouponValue"],
										"元现金券|",
										this.GetUserCoupons.Rows[i]["Id"],
										"|",
										this.GetUserCoupons.Rows[i]["ConditionValue"],
										"|",
										this.GetUserCoupons.Rows[i]["CouponValue"],
										"')\" id=\"acoupon",
										System.Web.UI.DataBinder.Eval(e.Item.DataItem, "TemplateId"),
										this.GetUserCoupons.Rows[i]["Id"],
										"\" value=\"",
										this.GetUserCoupons.Rows[i]["Id"],
										"\">",
										this.GetUserCoupons.Rows[i]["CouponValue"],
										"元现金券</a></li>"
									});
								}
							}
							else
							{
								decimal num = 0m;
								bool flag = false;
								foreach (ShoppingCartItemInfo current in list)
								{
									if (current.Type == 0)
									{
										DataTable dataTable = MemberProcessor.GetCouponByProducts(int.Parse(this.GetUserCoupons.Rows[i]["CouponId"].ToString()), current.ProductId);
										if (dataTable.Rows.Count > 0)
										{
											num += current.SubTotal;
											flag = true;
										}
									}
								}
								if (flag && decimal.Parse(this.GetUserCoupons.Rows[i]["ConditionValue"].ToString()) <= num)
								{
									obj = text;
									text = string.Concat(new object[]
									{
										obj,
										"<li><a onclick=\"Couponasetselect('",
										System.Web.UI.DataBinder.Eval(e.Item.DataItem, "TemplateId"),
										"','",
										this.GetUserCoupons.Rows[i]["CouponValue"],
										"元现金券','",
										this.GetUserCoupons.Rows[i]["CouponValue"],
										"',",
										this.GetUserCoupons.Rows[i]["Id"],
										",'",
										this.GetUserCoupons.Rows[i]["CouponValue"],
										"元现金券|",
										this.GetUserCoupons.Rows[i]["Id"],
										"|",
										this.GetUserCoupons.Rows[i]["ConditionValue"],
										"|",
										this.GetUserCoupons.Rows[i]["CouponValue"],
										"')\" id=\"acoupon",
										System.Web.UI.DataBinder.Eval(e.Item.DataItem, "TemplateId"),
										this.GetUserCoupons.Rows[i]["Id"],
										"\" value=\"",
										this.GetUserCoupons.Rows[i]["Id"],
										"\">",
										this.GetUserCoupons.Rows[i]["CouponValue"],
										"元现金券</a></li>"
									});
								}
							}
						}
					}
				}
				text2 += text;
				obj = text2;
				text2 = string.Concat(new object[]
				{
					obj,
					"</ul></div><input type=\"hidden\"  class=\"ClassCoupon\"   id=\"selectCoupon",
					System.Web.UI.DataBinder.Eval(e.Item.DataItem, "TemplateId"),
					"\"/>  "
				});
				if (!string.IsNullOrEmpty(text))
				{
					literal.Text = string.Concat(new object[]
					{
						text2,
						"<input type=\"hidden\"   id='selectCouponValue",
						System.Web.UI.DataBinder.Eval(e.Item.DataItem, "TemplateId"),
						"' class=\"selectCouponValue\" />"
					});
				}
				else
				{
					literal.Text = "<input type=\"hidden\"   id='selectCouponValue" + System.Web.UI.DataBinder.Eval(e.Item.DataItem, "TemplateId") + "' class=\"selectCouponValue\" />";
				}
				decimal d = 0m;
				decimal num2 = 0m;
				decimal num3 = 0m;
				decimal d2 = 0m;
				decimal num4 = 0m;
				int num5 = 0;
				foreach (ShoppingCartItemInfo current2 in list)
				{
					if (current2.Type == 0)
					{
						num4 += current2.SubTotal;
						num5 += current2.Quantity;
					}
				}
				d2 = num4;
				if (!this.isbargain)
				{
					for (int j = 0; j < this.dtActivities.Rows.Count; j++)
					{
						if (int.Parse(this.dtActivities.Rows[j]["attendTime"].ToString()) == 0 || int.Parse(this.dtActivities.Rows[j]["attendTime"].ToString()) > ActivityHelper.GetActivitiesMember(this.CurrentMemberInfo.UserId, int.Parse(this.dtActivities.Rows[j]["ActivitiesId"].ToString())))
						{
							decimal num = 0m;
							int num6 = 0;
							DataTable activities_Detail = ActivityHelper.GetActivities_Detail(int.Parse(this.dtActivities.Rows[j]["ActivitiesId"].ToString()));
							foreach (ShoppingCartItemInfo current2 in list)
							{
								if (current2.Type == 0)
								{
									DataTable dataTable = ActivityHelper.GetActivitiesProducts(int.Parse(this.dtActivities.Rows[j]["ActivitiesId"].ToString()), current2.ProductId);
									if (dataTable.Rows.Count > 0)
									{
										num += current2.SubTotal;
										num6 += current2.Quantity;
									}
								}
							}
							bool flag2 = false;
							if (activities_Detail.Rows.Count > 0)
							{
								for (int i = 0; i < activities_Detail.Rows.Count; i++)
								{
									if (MemberHelper.CheckCurrentMemberIsInRange(activities_Detail.Rows[i]["MemberGrades"].ToString(), activities_Detail.Rows[i]["DefualtGroup"].ToString(), activities_Detail.Rows[i]["CustomGroup"].ToString(), this.CurrentMemberInfo.UserId))
									{
										if (bool.Parse(this.dtActivities.Rows[j]["isAllProduct"].ToString()))
										{
											if (decimal.Parse(activities_Detail.Rows[i]["MeetMoney"].ToString()) > 0m)
											{
												if (num4 != 0m && num4 >= decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString()))
												{
													num2 = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString());
													d = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ReductionMoney"].ToString());
													literal6.Text = activities_Detail.Rows[activities_Detail.Rows.Count - 1]["bFreeShipping"].ToString();
													break;
												}
												if (num4 != 0m && num4 <= decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString()))
												{
													num2 = decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString());
													d = decimal.Parse(activities_Detail.Rows[0]["ReductionMoney"].ToString());
													break;
												}
												if (num4 != 0m && num4 >= decimal.Parse(activities_Detail.Rows[i]["MeetMoney"].ToString()))
												{
													num2 = decimal.Parse(activities_Detail.Rows[i]["MeetMoney"].ToString());
													d = decimal.Parse(activities_Detail.Rows[i]["ReductionMoney"].ToString());
													literal6.Text = activities_Detail.Rows[i]["bFreeShipping"].ToString();
												}
											}
											else
											{
												if (num5 != 0 && num5 >= int.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetNumber"].ToString()))
												{
													num2 = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString());
													num3 = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ReductionMoney"].ToString());
													flag2 = true;
													literal6.Text = activities_Detail.Rows[activities_Detail.Rows.Count - 1]["bFreeShipping"].ToString();
													break;
												}
												if (num5 != 0 && num5 <= int.Parse(activities_Detail.Rows[0]["MeetNumber"].ToString()))
												{
													num2 = decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString());
													num3 = decimal.Parse(activities_Detail.Rows[0]["ReductionMoney"].ToString());
													flag2 = true;
													break;
												}
												if (num5 != 0 && num5 >= int.Parse(activities_Detail.Rows[i]["MeetNumber"].ToString()))
												{
													num2 = decimal.Parse(activities_Detail.Rows[i]["MeetMoney"].ToString());
													num3 = decimal.Parse(activities_Detail.Rows[i]["ReductionMoney"].ToString());
													flag2 = true;
													literal6.Text = activities_Detail.Rows[i]["bFreeShipping"].ToString();
												}
											}
										}
										else
										{
											num4 = num;
											num5 = num6;
											if (decimal.Parse(activities_Detail.Rows[i]["MeetMoney"].ToString()) > 0m)
											{
												if (num4 != 0m && num4 >= decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString()))
												{
													num2 = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString());
													d = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ReductionMoney"].ToString());
													literal6.Text = activities_Detail.Rows[activities_Detail.Rows.Count - 1]["bFreeShipping"].ToString();
													break;
												}
												if (num4 != 0m && num4 <= decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString()))
												{
													num2 = decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString());
													d = decimal.Parse(activities_Detail.Rows[0]["ReductionMoney"].ToString());
													break;
												}
												if (num4 != 0m && num4 >= decimal.Parse(activities_Detail.Rows[i]["MeetMoney"].ToString()))
												{
													num2 = decimal.Parse(activities_Detail.Rows[i]["MeetMoney"].ToString());
													d = decimal.Parse(activities_Detail.Rows[i]["ReductionMoney"].ToString());
													literal6.Text = activities_Detail.Rows[i]["bFreeShipping"].ToString();
												}
											}
											else
											{
												if (num5 != 0 && num5 >= int.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetNumber"].ToString()))
												{
													num2 = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString());
													d = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ReductionMoney"].ToString());
													flag2 = true;
													literal6.Text = activities_Detail.Rows[activities_Detail.Rows.Count - 1]["bFreeShipping"].ToString();
													break;
												}
												if (num5 != 0 && num5 <= int.Parse(activities_Detail.Rows[0]["MeetNumber"].ToString()))
												{
													num2 = decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString());
													d = decimal.Parse(activities_Detail.Rows[0]["ReductionMoney"].ToString());
													flag2 = true;
													break;
												}
												if (num5 != 0 && num5 >= int.Parse(activities_Detail.Rows[i]["MeetNumber"].ToString()))
												{
													num2 = decimal.Parse(activities_Detail.Rows[i]["MeetMoney"].ToString());
													d = decimal.Parse(activities_Detail.Rows[i]["ReductionMoney"].ToString());
													flag2 = true;
													literal6.Text = activities_Detail.Rows[i]["bFreeShipping"].ToString();
												}
											}
										}
									}
								}
								if (flag2)
								{
									if (num5 > 0)
									{
										num3 += d;
									}
								}
								else if (num4 != 0m && num2 != 0m && num4 >= num2)
								{
									num3 += d;
								}
							}
						}
					}
				}
				literal2.Text = num3.ToString("F2");
				literal3.Text = num3.ToString("F2");
				literal5.Text = (d2 - num3).ToString("F2");
				literal4.Text = (d2 - num3).ToString("F2");
			}
		}

		protected override void AttachChildControls()
		{
			this.litShipTo = (System.Web.UI.WebControls.Literal)this.FindControl("litShipTo");
			this.litCellPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litCellPhone");
			this.litAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litAddress");
			this.litShowMes = (System.Web.UI.WebControls.Literal)this.FindControl("litShowMes");
			this.GetUserCoupons = MemberProcessor.GetUserCoupons();
			this.rptCartProducts = (VshopTemplatedRepeater)this.FindControl("rptCartProducts");
			this.rptCartProducts.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptCartProducts_ItemDataBound);
			this.litOrderTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderTotal");
			this.litPointNumber = (System.Web.UI.WebControls.Literal)this.FindControl("litPointNumber");
			this.litDisplayPointNumber = (System.Web.UI.WebControls.Literal)this.FindControl("litDisplayPointNumber");
			this.aLinkToShipping = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("aLinkToShipping");
			this.groupbuyHiddenBox = (System.Web.UI.HtmlControls.HtmlInputControl)this.FindControl("groupbuyHiddenBox");
			this.rptAddress = (VshopTemplatedRepeater)this.FindControl("rptAddress");
			this.selectShipTo = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("selectShipTo");
			this.regionId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("regionId");
			this.litAddAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litAddAddress");
			System.Collections.Generic.IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
			this.rptAddress.DataSource = from item in shippingAddresses
			orderby item.IsDefault
			select item;
			this.rptAddress.DataBind();
			ShippingAddressInfo shippingAddressInfo = shippingAddresses.FirstOrDefault((ShippingAddressInfo item) => item.IsDefault);
			if (shippingAddressInfo == null)
			{
				shippingAddressInfo = ((shippingAddresses.Count > 0) ? shippingAddresses[0] : null);
			}
			if (shippingAddressInfo != null)
			{
				this.litShipTo.Text = shippingAddressInfo.ShipTo;
				this.litCellPhone.Text = shippingAddressInfo.CellPhone;
				this.litAddress.Text = shippingAddressInfo.Address;
				this.selectShipTo.SetWhenIsNotNull(shippingAddressInfo.ShippingId.ToString());
				this.regionId.SetWhenIsNotNull(shippingAddressInfo.RegionId.ToString());
			}
			this.litAddAddress.Text = " href='/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString()) + "'";
			if (shippingAddresses == null || shippingAddresses.Count == 0)
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString()));
			}
			else
			{
				this.aLinkToShipping.HRef = Globals.ApplicationPath + "/Vshop/ShippingAddresses.aspx?returnUrl=" + Globals.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString());
				System.Collections.Generic.List<ShoppingCartInfo> list = new System.Collections.Generic.List<ShoppingCartInfo>();
				if (int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"]) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && (this.Page.Request.QueryString["from"] == "signBuy" || this.Page.Request.QueryString["from"] == "groupBuy"))
				{
					this.productSku = this.Page.Request.QueryString["productSku"];
					if (this.isbargain)
					{
						int bargainDetialId = Globals.RequestQueryNum("bargainDetialId");
						list = ShoppingCartProcessor.GetListShoppingCart(this.productSku, this.buyAmount, bargainDetialId, 0);
					}
					else
					{
						int num = this.buyAmount;
						int num2 = Globals.RequestQueryNum("limitedTimeDiscountId");
						if (num2 > 0)
						{
							bool flag = true;
							LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(num2);
							if (discountInfo == null)
							{
								flag = false;
							}
							if (flag)
							{
								if (MemberHelper.CheckCurrentMemberIsInRange(discountInfo.ApplyMembers, discountInfo.DefualtGroup, discountInfo.CustomGroup, this.CurrentMemberInfo.UserId))
								{
									if (discountInfo.LimitNumber != 0)
									{
										int limitedTimeDiscountUsedNum = ShoppingCartProcessor.GetLimitedTimeDiscountUsedNum(num2, this.productSku, 0, this.CurrentMemberInfo.UserId, false);
										if (this.buyAmount > discountInfo.LimitNumber - limitedTimeDiscountUsedNum)
										{
											num = discountInfo.LimitNumber - limitedTimeDiscountUsedNum;
										}
									}
								}
								else
								{
									num2 = 0;
								}
							}
							else
							{
								num2 = 0;
							}
						}
						if (num2 > 0)
						{
							ShoppingCartProcessor.RemoveLineItem(this.productSku, 0, num2);
						}
						if (num == 0 && num2 > 0)
						{
							num = this.buyAmount;
							num2 = 0;
						}
						list = ShoppingCartProcessor.GetListShoppingCart(this.productSku, num, 0, num2);
					}
				}
				else
				{
					list = ShoppingCartProcessor.GetOrderSummitCart();
				}
				if (list == null)
				{
					System.Web.HttpContext.Current.Response.Write("<script>alert('商品已下架或没有需要结算的订单！');location.href='/Vshop/ShoppingCart.aspx'</script>");
				}
				else
				{
					if (list.Count > 1)
					{
						this.litShowMes.Text = "<div style=\"color: #F60; \"><img  src=\"/Utility/pics/u77.png\">您所购买的商品不支持同一个物流规则发货，系统自动拆分成多个子订单处理</div>";
					}
					this.rptCartProducts.DataSource = list;
					this.rptCartProducts.DataBind();
					decimal d = 0m;
					decimal num3 = 0m;
					decimal d2 = 0m;
					int num4 = 0;
					foreach (ShoppingCartInfo current in list)
					{
						num4 += current.GetPointNumber;
						d += current.Total;
						num3 += current.Exemption;
						d2 += current.ShipCost;
					}
					decimal d3 = num3;
					this.litOrderTotal.Text = (d - d3).ToString("F2");
					if (num4 == 0)
					{
						this.litDisplayPointNumber.Text = "style=\"display:none;\"";
					}
					this.litPointNumber.Text = num4.ToString();
					PageTitle.AddSiteNameTitle("订单确认");
				}
			}
		}

		public decimal DiscountMoney(System.Collections.Generic.List<ShoppingCartInfo> infoList)
		{
			decimal d = 0m;
			decimal num = 0m;
			decimal num2 = 0m;
			decimal d2 = 0m;
			int num3 = 0;
			foreach (ShoppingCartInfo current in infoList)
			{
				foreach (ShoppingCartItemInfo current2 in current.LineItems)
				{
					if (current2.Type == 0)
					{
						d2 += current2.SubTotal;
						num3 += current2.Quantity;
					}
				}
			}
			for (int i = 0; i < this.dtActivities.Rows.Count; i++)
			{
				decimal num4 = 0m;
				int num5 = 0;
				DataTable activities_Detail = ActivityHelper.GetActivities_Detail(int.Parse(this.dtActivities.Rows[i]["ActivitiesId"].ToString()));
				foreach (ShoppingCartInfo current in infoList)
				{
					foreach (ShoppingCartItemInfo current2 in current.LineItems)
					{
						if (current2.Type == 0)
						{
							DataTable activitiesProducts = ActivityHelper.GetActivitiesProducts(int.Parse(this.dtActivities.Rows[i]["ActivitiesId"].ToString()), current2.ProductId);
							if (activitiesProducts.Rows.Count > 0)
							{
								num4 += current2.SubTotal;
								num5 += current2.Quantity;
							}
						}
					}
				}
				if (activities_Detail.Rows.Count > 0)
				{
					for (int j = 0; j < activities_Detail.Rows.Count; j++)
					{
						if (bool.Parse(this.dtActivities.Rows[i]["isAllProduct"].ToString()))
						{
							if (decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString()) > 0m)
							{
								if (d2 >= decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString()))
								{
									num = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString());
									d = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ReductionMoney"].ToString());
									break;
								}
								if (d2 <= decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString()))
								{
									num = decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString());
									d = decimal.Parse(activities_Detail.Rows[0]["ReductionMoney"].ToString());
									break;
								}
								if (d2 >= decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString()))
								{
									num = decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString());
									d = decimal.Parse(activities_Detail.Rows[j]["ReductionMoney"].ToString());
								}
							}
							else
							{
								if (num3 >= int.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetNumber"].ToString()))
								{
									num = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString());
									num2 = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ReductionMoney"].ToString());
									break;
								}
								if (num3 <= int.Parse(activities_Detail.Rows[0]["MeetNumber"].ToString()))
								{
									num = decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString());
									num2 = decimal.Parse(activities_Detail.Rows[0]["ReductionMoney"].ToString());
									break;
								}
								if (num3 >= int.Parse(activities_Detail.Rows[j]["MeetNumber"].ToString()))
								{
									num = decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString());
									num2 = decimal.Parse(activities_Detail.Rows[j]["ReductionMoney"].ToString());
								}
							}
						}
						else
						{
							d2 = num4;
							num3 = num5;
							if (decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString()) > 0m)
							{
								if (d2 >= decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString()))
								{
									num = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString());
									d = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ReductionMoney"].ToString());
									break;
								}
								if (d2 <= decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString()))
								{
									num = decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString());
									d = decimal.Parse(activities_Detail.Rows[0]["ReductionMoney"].ToString());
									break;
								}
								if (d2 >= decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString()))
								{
									num = decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString());
									d = decimal.Parse(activities_Detail.Rows[j]["ReductionMoney"].ToString());
								}
							}
							else
							{
								if (num3 >= int.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetNumber"].ToString()))
								{
									num = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString());
									d = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ReductionMoney"].ToString());
									break;
								}
								if (num3 <= int.Parse(activities_Detail.Rows[0]["MeetNumber"].ToString()))
								{
									num = decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString());
									d = decimal.Parse(activities_Detail.Rows[0]["ReductionMoney"].ToString());
									break;
								}
								if (num3 >= int.Parse(activities_Detail.Rows[j]["MeetNumber"].ToString()))
								{
									num = decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString());
									d = decimal.Parse(activities_Detail.Rows[j]["ReductionMoney"].ToString());
								}
							}
						}
					}
					if (d2 >= num || num == 0m)
					{
						num2 += d;
					}
				}
			}
			return num2;
		}
	}
}
