using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Orders;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Trade
{
	public class BatchPrintSendOrderGoods : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl divContent;

		protected BatchPrintSendOrderGoods() : base("m03", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request["OrderIds"].Trim(new char[]
			{
				','
			});
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			foreach (OrderInfo current in this.GetPrintData(text))
			{
				System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
				htmlGenericControl.Attributes["class"] = "order print";
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("");
				System.DateTime dateTime = current.FinishDate.HasValue ? System.DateTime.Parse(current.FinishDate.ToString()) : current.OrderDate;
				stringBuilder.AppendFormat("<div class=\"info clear\"><ul class=\"sub-info\"><li><span>订单号： </span>{2}</li><li><span>成交时间： </span>{1}</li><li><span>收货人姓名： </span>{0}</li></ul></div>", current.ShipTo, dateTime.ToString("yyyy-MM-dd HH:mm:ss"), current.OrderId);
				stringBuilder.Append("<table><col class=\"col-1\" /><col class=\"col-3\" /><col class=\"col-3\" /><col class=\"col-3\" /><col class=\"col-4\" /><col class=\"col-5\" /><thead><tr><th>商品信息</th><th>商家编码</th><th>单价</th><th>数量</th><th>小计</th><th>总价</th></tr></thead><tbody>");
				System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = current.LineItems;
				if (lineItems != null)
				{
					int num = 0;
					foreach (string current2 in lineItems.Keys)
					{
						LineItemInfo lineItemInfo = lineItems[current2];
						string str = string.Empty;
						if (lineItemInfo.OrderItemsStatus == OrderStatus.Returned)
						{
							str = "(已退货，金额￥" + lineItemInfo.ReturnMoney.ToString("F2") + ")";
						}
						else if (lineItemInfo.OrderItemsStatus == OrderStatus.Refunded)
						{
							str = "(已退款，金额￥" + lineItemInfo.ReturnMoney.ToString("F2") + ")";
						}
						stringBuilder.AppendFormat("<tr><td>{0}</td>", lineItemInfo.ItemDescription + str + (string.IsNullOrEmpty(lineItemInfo.SKUContent) ? "" : ("<br>" + lineItemInfo.SKUContent)));
						stringBuilder.AppendFormat("<td style='text-align:center;'>{0}</td>", string.IsNullOrEmpty(lineItemInfo.SKU) ? "-" : lineItemInfo.SKU);
						stringBuilder.AppendFormat("<td>￥{0}</td>", System.Math.Round(lineItemInfo.ItemListPrice, 2));
						stringBuilder.AppendFormat("<td style='padding-left:15px;'>{0}</td>", lineItemInfo.ShipmentQuantity);
						stringBuilder.AppendFormat("<td style='border-left:1px solid #858585;'>￥{0}</td>", System.Math.Round(lineItemInfo.GetSubTotal() - lineItemInfo.DiscountAverage, 2));
						if (num == 0)
						{
							string str2 = string.Empty;
							System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
							if (!string.IsNullOrEmpty(current.ActivitiesName))
							{
								stringBuilder2.Append(string.Concat(new string[]
								{
									"<p>",
									current.ActivitiesName,
									":￥",
									current.DiscountAmount.ToString("F2"),
									"</p>"
								}));
							}
							if (!string.IsNullOrEmpty(current.ReducedPromotionName))
							{
								stringBuilder2.Append(string.Concat(new string[]
								{
									"<p>",
									current.ReducedPromotionName,
									":￥",
									current.ReducedPromotionAmount.ToString("F2"),
									"</p>"
								}));
							}
							if (!string.IsNullOrEmpty(current.CouponName))
							{
								stringBuilder2.Append(string.Concat(new string[]
								{
									"<p>",
									current.CouponName,
									":￥",
									current.CouponAmount.ToString("F2"),
									"</p>"
								}));
							}
							if (!string.IsNullOrEmpty(current.RedPagerActivityName))
							{
								stringBuilder2.Append(string.Concat(new string[]
								{
									"<p>",
									current.RedPagerActivityName,
									":￥",
									current.RedPagerAmount.ToString("F2"),
									"</p>"
								}));
							}
							if (current.PointToCash > 0m)
							{
								stringBuilder2.Append("<p>积分抵现:￥" + current.PointToCash.ToString("F2") + "</p>");
							}
							decimal adjustCommssion = current.GetAdjustCommssion();
							if (adjustCommssion > 0m)
							{
								stringBuilder2.Append("<p>管理员调价优惠:￥" + adjustCommssion.ToString("F2") + "</p>");
							}
							else if (adjustCommssion < 0m)
							{
								stringBuilder2.Append("<p>管理员调价增加:￥" + adjustCommssion.ToString("F2").Trim(new char[]
								{
									'-'
								}) + "</p>");
							}
							str2 = stringBuilder2.ToString();
							stringBuilder.AppendFormat("<td rowspan='{0}' colspan='1' style='border-left:1px solid #858585;'>{1}</td>", lineItems.Keys.Count, string.Format("<p><strong>￥{0}</strong></p>" + str2 + "<p>含运费￥{1}</p><p></p>", current.GetTotal().ToString("F2"), current.AdjustedFreight.ToString("F2")));
						}
						stringBuilder.Append("</tr>");
						num++;
					}
				}
				stringBuilder.Append("</tbody></table><br class=\"clear\" /><br>");
				htmlGenericControl.InnerHtml = stringBuilder.ToString();
				this.divContent.Controls.AddAt(0, htmlGenericControl);
			}
		}

		private System.Collections.Generic.List<OrderInfo> GetPrintData(string orderIds)
		{
			System.Collections.Generic.List<OrderInfo> list = new System.Collections.Generic.List<OrderInfo>();
			string[] array = orderIds.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string orderId = array[i];
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
				list.Add(orderInfo);
			}
			return list;
		}
	}
}
