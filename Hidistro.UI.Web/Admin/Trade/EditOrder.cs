using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Trade
{
	[PrivilegeCheck(Privilege.EditOrders)]
	public class EditOrder : AdminPage
	{
		protected string orderId;

		protected string ReUrl = Globals.RequestQueryStr("reurl");

		private OrderInfo order;

		protected System.Web.UI.WebControls.Literal litOrderGoodsTotalPrice;

		protected System.Web.UI.WebControls.Repeater rptItemList;

		protected System.Web.UI.WebControls.Literal litLogistic;

		protected System.Web.UI.WebControls.TextBox txtAdjustedFreight;

		protected System.Web.UI.WebControls.Literal litItemTotalPrice;

		protected System.Web.UI.WebControls.Literal litOrderTotal;

		protected System.Web.UI.WebControls.Literal litAdjustedCommssion;

		protected EditOrder() : base("m03", "ddp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string a = Globals.RequestFormStr("posttype");
			if (string.IsNullOrEmpty(this.ReUrl))
			{
				this.ReUrl = "manageorder.aspx";
			}
			if (a == "updateorder")
			{
				string value = Globals.RequestFormStr("data");
				base.Response.ContentType = "application/json";
				string s = "{\"type\":\"0\",\"tips\":\"修改失败！\"}";
				JArray jArray = (JArray)JsonConvert.DeserializeObject(value);
				OrderInfo orderInfo = null;
				using (System.Collections.Generic.IEnumerator<JToken> enumerator = jArray.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JObject jObject = (JObject)enumerator.Current;
						string text = jObject["o"].ToString();
						decimal num = decimal.Parse(jObject["f"].ToString());
						JArray jArray2 = (JArray)jObject["data"];
						if (jArray2.Count > 0)
						{
							string itemid = string.Empty;
							if (!string.IsNullOrEmpty(text))
							{
								orderInfo = OrderHelper.GetOrderInfo(text);
								if (orderInfo != null && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
								{
									using (System.Collections.Generic.IEnumerator<JToken> enumerator2 = jArray2.GetEnumerator())
									{
										while (enumerator2.MoveNext())
										{
											JObject jObject2 = (JObject)enumerator2.Current;
											itemid = jObject2["skuid"].ToString();
											decimal d = decimal.Parse(jObject2["adjustedcommssion"].ToString());
											OrderHelper.UpdateAdjustCommssions(text, itemid, d * -1m);
										}
									}
									if (num >= 0m && orderInfo != null && orderInfo.AdjustedFreight != num)
									{
										orderInfo.AdjustedFreight = num;
										OrderHelper.UpdateOrder(orderInfo);
									}
									OrderHelper.UpdateCalculadtionCommission(text);
									s = "{\"type\":\"1\",\"tips\":\"订单价格修改成功！\"}";
								}
								else
								{
									s = "{\"type\":\"0\",\"tips\":\"当前订单状态不允许修改价格！\"}";
								}
							}
						}
					}
				}
				base.Response.Write(s);
				base.Response.End();
				return;
			}
			this.orderId = Globals.RequestQueryStr("OrderId");
			if (string.IsNullOrEmpty(this.orderId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.order = OrderHelper.GetOrderInfo(this.orderId);
			if (this.order == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.rptItemList.DataSource = this.order.LineItems.Values;
			this.rptItemList.DataBind();
			this.litLogistic.Text = (string.IsNullOrEmpty(this.order.RealModeName) ? this.order.ModeName : this.order.RealModeName);
			this.txtAdjustedFreight.Text = this.order.AdjustedFreight.ToString("F", System.Globalization.CultureInfo.InvariantCulture);
			decimal num2 = 0m;
			if (!string.IsNullOrEmpty(this.order.ActivitiesName))
			{
				num2 += this.order.DiscountAmount;
			}
			if (!string.IsNullOrEmpty(this.order.ReducedPromotionName))
			{
				num2 += this.order.ReducedPromotionAmount;
			}
			if (!string.IsNullOrEmpty(this.order.CouponName))
			{
				num2 += this.order.CouponAmount;
			}
			if (!string.IsNullOrEmpty(this.order.RedPagerActivityName))
			{
				num2 += this.order.RedPagerAmount;
			}
			if (this.order.PointToCash > 0m)
			{
				num2 += this.order.PointToCash;
			}
			this.litOrderGoodsTotalPrice.Text = (this.litItemTotalPrice.Text = this.order.GetAmount().ToString("F2"));
			this.litAdjustedCommssion.Text = this.order.GetTotalDiscountAverage().ToString("F2");
			this.litOrderTotal.Text = (this.order.GetAmount() - num2).ToString("F", System.Globalization.CultureInfo.InvariantCulture);
		}

		protected string FormatAdjustedCommssion(object itemType, object itemAdjustedCommssion)
		{
			string result = string.Empty;
			if (Globals.ToNum(itemType) == 0)
			{
				result = " <input type=\"text\" name=\"adjustedcommssion\" value=\"" + (decimal.Parse(itemAdjustedCommssion.ToString()) * -1m).ToString("F2") + "\" title=\"输入负数为优惠金额，正常输入为涨价\" maxlength=\"7\" />";
			}
			else
			{
				string str = (decimal.Parse(itemAdjustedCommssion.ToString()) * -1m).ToString("F2");
				result = str + " <input type=\"hidden\" name=\"adjustedcommssion\" value=\"" + (decimal.Parse(itemAdjustedCommssion.ToString()) * -1m).ToString("F2") + "\" />";
			}
			return result;
		}
	}
}
