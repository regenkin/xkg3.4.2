using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hidistro.UI.Web.Admin.Trade
{
	[PrivilegeCheck(Privilege.Orders)]
	public class OrderSplit : AdminPage
	{
		protected string reUrl = Globals.RequestQueryStr("reurl");

		protected string orderId = Globals.RequestQueryStr("OrderId");

		protected OrderSplit() : base("m03", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = Globals.RequestFormStr("posttype");
			if (string.IsNullOrEmpty(this.reUrl))
			{
				this.reUrl = "manageorder.aspx";
			}
			string key;
			switch (key = text)
			{
			case "getordersplit":
			{
				base.Response.ContentType = "application/json";
				string s = "[]";
				this.orderId = Globals.RequestFormStr("orderid");
				System.Collections.Generic.IList<OrderSplitInfo> orderSplitItems = OrderSplitHelper.GetOrderSplitItems(this.orderId);
				if (orderSplitItems.Count > 0)
				{
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					stringBuilder.Append("[");
					int num2 = 0;
					foreach (OrderSplitInfo current in orderSplitItems)
					{
						if (num2 != 0)
						{
							stringBuilder.Append(",");
						}
						num2++;
						stringBuilder.Append(string.Concat(new object[]
						{
							"{\"id\":",
							current.Id,
							",\"orderid\":\"",
							this.FormatNewOrderID(current.OldOrderId, current.OrderIDNum.ToString()),
							"\",\"adjustedfreight\":",
							current.AdjustedFreight.ToString("F2"),
							",\"data\":[",
							this.GetOrderItemListData(current.ItemList, current.OldOrderId),
							"]}"
						}));
					}
					stringBuilder.Append("]");
					s = stringBuilder.ToString();
				}
				else
				{
					OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
					System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
					if (orderInfo != null)
					{
						if (orderInfo.SplitState > 0)
						{
							stringBuilder2.Append("{\"id\":\"splited\"}");
						}
						else
						{
							stringBuilder2.Append("[");
							if (orderInfo != null)
							{
								stringBuilder2.Append(string.Concat(new string[]
								{
									"{\"id\":0,\"orderid\":\"",
									orderInfo.OrderId,
									"\",\"adjustedfreight\":",
									orderInfo.AdjustedFreight.ToString("F2"),
									",\"data\":[",
									this.GetOrderItemListByOrder(orderInfo),
									"]}"
								}));
							}
							stringBuilder2.Append("]");
						}
					}
					s = stringBuilder2.ToString();
				}
				base.Response.Write(s);
				base.Response.End();
				return;
			}
			case "savesplit":
			{
				base.Response.ContentType = "application/json";
				string text2 = Globals.RequestFormStr("toorderid");
				string text3 = Globals.RequestFormStr("fromorderid");
				int num3 = Globals.RequestFormNum("itemid");
				string skuid = Globals.RequestFormStr("fromskuid");
				string s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
				OrderInfo orderInfo2 = OrderHelper.GetOrderInfo(text3);
				if (orderInfo2 != null)
				{
					if (orderInfo2.OrderStatus == OrderStatus.WaitBuyerPay || orderInfo2.OrderStatus == OrderStatus.BuyerAlreadyPaid)
					{
						decimal d = orderInfo2.GetTotal() - orderInfo2.AdjustedFreight;
						decimal d2 = 0m;
						foreach (LineItemInfo current2 in orderInfo2.LineItems.Values)
						{
							if (current2.ID == num3)
							{
								if (current2.Type != 1)
								{
									d2 = current2.ItemAdjustedPrice * current2.Quantity - current2.ItemAdjustedCommssion - current2.DiscountAverage;
									break;
								}
								break;
							}
						}
						if (d2 >= d)
						{
							s = "{\"type\":\"0\",\"tips\":\"订单拆分后，原订单的价格将不大于0！\"}";
						}
						else if (d2 == 0m && text2 == "0")
						{
							s = "{\"type\":\"0\",\"tips\":\"订单拆分后，新订单的价格必须大于0！\"}";
						}
						else
						{
							string text4 = OrderSplitHelper.OrderSplitToTemp(orderInfo2, skuid, text2, num3);
							string a;
							if ((a = text4) != null)
							{
								if (!(a == "1"))
								{
									if (!(a == "-1"))
									{
										if (!(a == "-2"))
										{
											if (a == "-3")
											{
												s = "{\"type\":\"0\",\"tips\":\"拆分出去的订单价格必须大于0！\"}";
											}
										}
										else
										{
											s = "{\"type\":\"0\",\"tips\":\"非法数据！\"}";
										}
									}
									else
									{
										s = "{\"type\":\"0\",\"tips\":\"订单只有一条记录，不允许拆分！\"}";
									}
								}
								else
								{
									s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
								}
							}
						}
					}
					else
					{
						OrderSplitHelper.DelOrderSplitByOrderID(text3, null);
						s = "{\"type\":\"3\",\"tips\":\"当前订单状态不允许拆分！\"}";
					}
				}
				base.Response.Write(s);
				base.Response.End();
				return;
			}
			case "cancelordersplit":
			{
				base.Response.ContentType = "application/json";
				string text3 = Globals.RequestFormStr("fromorderid");
				int num3 = Globals.RequestFormNum("itemid");
				int num4 = Globals.RequestFormNum("fromsplitid");
				string s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
				OrderInfo orderInfo2 = OrderHelper.GetOrderInfo(text3);
				if (orderInfo2 != null && num3 > 0 && num4 > 0)
				{
					string text5 = OrderSplitHelper.CancelSplitOrderByID(text3, num4, num3);
					string a2;
					if ((a2 = text5) != null && a2 == "1")
					{
						s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
					}
				}
				base.Response.Write(s);
				base.Response.End();
				return;
			}
			case "editfright":
			{
				base.Response.ContentType = "application/json";
				string s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
				int id = Globals.RequestFormNum("id");
				decimal num5 = 0m;
				decimal.TryParse(Globals.RequestFormStr("val"), out num5);
				if (num5 >= 0m)
				{
					OrderSplitInfo orderSplitInfo = OrderSplitHelper.GetOrderSplitInfo(id);
					if (orderSplitInfo != null)
					{
						orderSplitInfo.AdjustedFreight = num5;
						if (OrderSplitHelper.UpdateOrderSplitFright(orderSplitInfo))
						{
							s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
						}
					}
				}
				base.Response.Write(s);
				base.Response.End();
				return;
			}
			case "cancelsplittoorder":
			{
				base.Response.ContentType = "application/json";
				string s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
				string text3 = Globals.RequestFormStr("fromorderid");
				if (OrderSplitHelper.DelOrderSplitByOrderID(text3, null))
				{
					s = "{\"type\":\"1\",\"tips\":\"取消成功！\"}";
				}
				base.Response.Write(s);
				base.Response.End();
				return;
			}
			case "savesplittoorder":
			{
				base.Response.ContentType = "application/json";
				string s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
				string text3 = Globals.RequestFormStr("fromorderid");
				System.Collections.Generic.IList<OrderSplitInfo> orderSplitItems2 = OrderSplitHelper.GetOrderSplitItems(text3);
				if (orderSplitItems2.Count > 1)
				{
					OrderInfo orderInfo3 = OrderHelper.GetOrderInfo(text3);
					if (orderInfo3 != null)
					{
						if (orderInfo3.OrderStatus == OrderStatus.WaitBuyerPay || orderInfo3.OrderStatus == OrderStatus.BuyerAlreadyPaid)
						{
							string text6 = OrderSplitHelper.UpdateAndCreateOrderByOrderSplitInfo(orderSplitItems2, orderInfo3);
							if (text6 == "1")
							{
								s = "{\"type\":\"1\",\"tips\":\"订单拆分成功！\"}";
							}
							else
							{
								s = "{\"type\":\"0\",\"tips\":\"订单拆分失败，原因是" + text6 + "！\"}";
							}
						}
						else
						{
							s = "{\"type\":\"0\",\"tips\":\"待付款和待发货状态的订单才允许拆分！\"}";
						}
					}
					else
					{
						s = "{\"type\":\"0\",\"tips\":\"主订单已不存在，不能保存！\"}";
					}
				}
				else
				{
					s = "{\"type\":\"0\",\"tips\":\"订单未拆分，不能保存！\"}";
				}
				base.Response.Write(s);
				base.Response.End();
				break;
			}

				return;
			}
		}

		private string FormatNewOrderID(string oldorderid, string stradd)
		{
			string result = string.Empty;
			if (stradd == "1")
			{
				result = oldorderid;
			}
			else
			{
				result = oldorderid + "-" + stradd;
			}
			return result;
		}

		private string GetItemStateName(OrderStatus status)
		{
			string result = string.Empty;
			switch (status)
			{
			case OrderStatus.BuyerAlreadyPaid:
				result = "已付款";
				break;
			case OrderStatus.Finished:
				result = "已完成";
				break;
			case OrderStatus.ApplyForRefund:
				result = "退款中";
				break;
			case OrderStatus.ApplyForReturns:
				result = "退货中";
				break;
			case OrderStatus.Refunded:
				result = "已退款";
				break;
			case OrderStatus.Returned:
				result = "已退货";
				break;
			}
			return result;
		}

		private string GetOrderItemListByOrder(OrderInfo order)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string arg_0B_0 = string.Empty;
			foreach (LineItemInfo current in order.LineItems.Values)
			{
				stringBuilder.Append(string.Concat(new object[]
				{
					",{\"ID\":\"",
					current.ID,
					"\",\"ProductID\":\"",
					current.ProductId.ToString(),
					"\",\"SkuID\":\"",
					current.SkuId,
					"\",\"Quantity\":\"",
					current.Quantity.ToString(),
					"\",\"ItemListPrice\":\"",
					current.ItemListPrice.ToString("F2"),
					"\",\"SKUContent\":\"",
					Globals.String2Json(current.SKUContent),
					"\",\"ThumbnailsUrl\":\"",
					Globals.String2Json(current.ThumbnailsUrl),
					"\",\"OrderItemsStatus\":\"",
					this.GetItemStateName(current.OrderItemsStatus),
					"\",\"ItemDescription\":\"",
					Globals.String2Json(current.ItemDescription),
					"\"}"
				}));
			}
			return stringBuilder.ToString().Trim(new char[]
			{
				','
			});
		}

		private string GetOrderItemListData(string itemlist, string orderid)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (!string.IsNullOrEmpty(itemlist) && !string.IsNullOrEmpty(orderid))
			{
				string[] array = itemlist.Split(new char[]
				{
					','
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string s = array2[i];
					int num = Globals.ToNum(s);
					if (num > 0)
					{
						LineItemInfo lineItemInfo = OrderSplitHelper.GetLineItemInfo(num, orderid);
						if (lineItemInfo != null)
						{
							stringBuilder.Append(string.Concat(new object[]
							{
								",{\"ID\":\"",
								lineItemInfo.ID,
								"\",\"ProductID\":\"",
								lineItemInfo.ProductId.ToString(),
								"\",\"SkuID\":\"",
								lineItemInfo.SkuId,
								"\",\"Quantity\":\"",
								lineItemInfo.Quantity.ToString(),
								"\",\"ItemListPrice\":\"",
								lineItemInfo.ItemListPrice.ToString("F2"),
								"\",\"SKUContent\":\"",
								Globals.String2Json(lineItemInfo.SKUContent),
								"\",\"ThumbnailsUrl\":\"",
								Globals.String2Json(lineItemInfo.ThumbnailsUrl),
								"\",\"OrderItemsStatus\":\"",
								this.GetItemStateName(lineItemInfo.OrderItemsStatus),
								"\",\"ItemDescription\":\"",
								Globals.String2Json(lineItemInfo.ItemDescription),
								"\"}"
							}));
						}
					}
				}
			}
			return stringBuilder.ToString().Trim(new char[]
			{
				','
			});
		}
	}
}
