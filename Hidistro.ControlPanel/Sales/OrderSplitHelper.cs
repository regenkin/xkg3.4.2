using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SqlDal.Orders;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.ControlPanel.Sales
{
	public static class OrderSplitHelper
	{
		public static OrderSplitInfo GetOrderSplitInfo(int id)
		{
			return new OrderSplitDao().GetOrderSplitInfo(id);
		}

		public static OrderSplitInfo GetOrderSplitInfoByOrderIDAndNum(int orderidnum, string oldorderid)
		{
			return new OrderSplitDao().GetOrderSplitInfoByOrderIDAndNum(orderidnum, oldorderid);
		}

		public static bool UpdateOrderSplitInfo(OrderSplitInfo info)
		{
			return new OrderSplitDao().UpdateOrderSplitInfo(info);
		}

		public static IList<OrderSplitInfo> GetOrderSplitItems(string orderid)
		{
			return new OrderSplitDao().GetOrderSplitItems(orderid);
		}

		public static LineItemInfo GetLineItemInfo(int id, string orderid)
		{
			return new LineItemDao().GetLineItemInfo(id, orderid);
		}

		public static bool UpdateOrderSplitFright(OrderSplitInfo info)
		{
			return new OrderSplitDao().UpdateOrderSplitFright(info);
		}

		public static string UpdateAndCreateOrderByOrderSplitInfo(IList<OrderSplitInfo> infoList, OrderInfo oldorderinfo)
		{
			string text = "1";
			Database database = DatabaseFactory.CreateDatabase();
			string result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				StringBuilder stringBuilder = new StringBuilder();
				try
				{
					decimal num = 0m;
					foreach (OrderSplitInfo current in infoList)
					{
						OrderInfo orderInfo = new OrderInfo();
						if (current.OrderIDNum != 1)
						{
							string itemList = current.ItemList;
							if (string.IsNullOrEmpty(itemList))
							{
								dbTransaction.Rollback();
								result = "订单拆分失败";
								return result;
							}
							string text2 = oldorderinfo.OrderId + "-" + current.OrderIDNum.ToString();
							decimal num2 = 0m;
							decimal d = 0m;
							string[] array = itemList.Split(new char[]
							{
								','
							});
							string[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								string s = array2[i];
								LineItemInfo lineItemInfo = new LineItemDao().GetLineItemInfo(Globals.ToNum(s), "");
								if (lineItemInfo == null)
								{
									dbTransaction.Rollback();
									result = "订单详情更新失败";
									return result;
								}
								num2 += lineItemInfo.ItemWeight * lineItemInfo.Quantity;
								num += num2;
								d += lineItemInfo.ItemAdjustedPrice * lineItemInfo.Quantity;
							}
							stringBuilder.Append("," + itemList.Trim(new char[]
							{
								','
							}));
							if (!new LineItemDao().UpdateLineItemOrderID(itemList, text2, dbTransaction))
							{
								dbTransaction.Rollback();
								result = "订单详情更新失败";
								return result;
							}
							orderInfo.OrderId = text2;
							orderInfo.OrderMarking = oldorderinfo.OrderMarking;
							orderInfo.ClientShortType = oldorderinfo.ClientShortType;
							orderInfo.Remark = oldorderinfo.Remark;
							orderInfo.ManagerMark = oldorderinfo.ManagerMark;
							orderInfo.AdjustedDiscount = 0m;
							orderInfo.OrderStatus = oldorderinfo.OrderStatus;
							orderInfo.CloseReason = oldorderinfo.CloseReason;
							orderInfo.OrderDate = oldorderinfo.OrderDate;
							orderInfo.PayDate = oldorderinfo.PayDate;
							orderInfo.ShippingDate = oldorderinfo.ShippingDate;
							orderInfo.FinishDate = oldorderinfo.FinishDate;
							orderInfo.UserId = oldorderinfo.UserId;
							orderInfo.Username = oldorderinfo.Username;
							orderInfo.EmailAddress = oldorderinfo.EmailAddress;
							orderInfo.RealName = oldorderinfo.RealName;
							orderInfo.QQ = oldorderinfo.QQ;
							orderInfo.Wangwang = oldorderinfo.Wangwang;
							orderInfo.MSN = oldorderinfo.MSN;
							orderInfo.ShippingRegion = oldorderinfo.ShippingRegion;
							orderInfo.Address = oldorderinfo.Address;
							orderInfo.ZipCode = oldorderinfo.ZipCode;
							orderInfo.ShipTo = oldorderinfo.ShipTo;
							orderInfo.TelPhone = oldorderinfo.TelPhone;
							orderInfo.CellPhone = oldorderinfo.CellPhone;
							orderInfo.ShipToDate = oldorderinfo.ShipToDate;
							orderInfo.ShippingModeId = oldorderinfo.ShippingModeId;
							orderInfo.ModeName = oldorderinfo.ModeName;
							orderInfo.RealShippingModeId = oldorderinfo.RealShippingModeId;
							orderInfo.RealModeName = oldorderinfo.RealModeName;
							orderInfo.RegionId = oldorderinfo.RegionId;
							orderInfo.Freight = current.AdjustedFreight;
							orderInfo.AdjustedFreight = current.AdjustedFreight;
							orderInfo.ShipOrderNumber = oldorderinfo.ShipOrderNumber;
							orderInfo.Weight = num2;
							orderInfo.Weight = oldorderinfo.Weight;
							orderInfo.ExpressCompanyName = oldorderinfo.ExpressCompanyName;
							orderInfo.ExpressCompanyAbb = oldorderinfo.ExpressCompanyAbb;
							orderInfo.PaymentTypeId = oldorderinfo.PaymentTypeId;
							orderInfo.PaymentType = oldorderinfo.PaymentType;
							orderInfo.PayCharge = oldorderinfo.PayCharge;
							orderInfo.RefundStatus = oldorderinfo.RefundStatus;
							orderInfo.RefundAmount = oldorderinfo.RefundAmount;
							orderInfo.RefundRemark = oldorderinfo.RefundRemark;
							orderInfo.Gateway = oldorderinfo.Gateway;
							orderInfo.Points = 0;
							orderInfo.DiscountAmount = 0m;
							orderInfo.ActivitiesId = "";
							orderInfo.ActivitiesName = "";
							orderInfo.ReducedPromotionId = 0;
							orderInfo.ReducedPromotionName = "";
							orderInfo.ReducedPromotionAmount = 0m;
							orderInfo.IsReduced = false;
							orderInfo.SentTimesPointPromotionId = 0;
							orderInfo.SentTimesPointPromotionName = "";
							orderInfo.FreightFreePromotionId = 0;
							orderInfo.FreightFreePromotionName = "";
							orderInfo.IsFreightFree = oldorderinfo.IsFreightFree;
							orderInfo.GatewayOrderId = oldorderinfo.GatewayOrderId;
							orderInfo.IsPrinted = oldorderinfo.IsPrinted;
							orderInfo.InvoiceTitle = oldorderinfo.InvoiceTitle;
							orderInfo.ReferralUserId = oldorderinfo.ReferralUserId;
							orderInfo.ReferralPath = oldorderinfo.ReferralPath;
							orderInfo.RedPagerID = null;
							orderInfo.RedPagerActivityName = "";
							orderInfo.RedPagerOrderAmountCanUse = 0m;
							orderInfo.RedPagerAmount = 0m;
							orderInfo.PointToCash = 0m;
							orderInfo.PointExchange = 0;
							if (!new OrderDao().CreatOrder(orderInfo, dbTransaction))
							{
								dbTransaction.Rollback();
								result = "生成新订单失败";
								return result;
							}
							if (!new OrderDao().UpdateOrderSplitState(orderInfo.OrderId, 2, dbTransaction))
							{
								dbTransaction.Rollback();
								result = "更新订单状态失败";
								return result;
							}
						}
					}
					foreach (OrderSplitInfo current in infoList)
					{
						if (current.OrderIDNum == 1)
						{
							decimal num3 = oldorderinfo.Weight - num;
							if (num3 > 0m)
							{
								oldorderinfo.Weight = num3;
							}
							oldorderinfo.AdjustedFreight = current.AdjustedFreight;
							if (!new OrderDao().UpdateOrder(oldorderinfo, dbTransaction))
							{
								dbTransaction.Rollback();
								result = "更新订单失败";
								return result;
							}
							if (!new OrderDao().UpdateOrderSplitState(oldorderinfo.OrderId, 1, dbTransaction))
							{
								dbTransaction.Rollback();
								result = "更新主订单状态失败";
								return result;
							}
							if (!new OrderSplitDao().DelOrderSplitByOrderID(oldorderinfo.OrderId, dbTransaction))
							{
								dbTransaction.Rollback();
								result = "删除拆分记录失败";
								return result;
							}
						}
					}
					dbTransaction.Commit();
					foreach (OrderSplitInfo current in infoList)
					{
						OrderInfo orderInfo2 = new OrderDao().GetOrderInfo(current.OldOrderId + ((current.OrderIDNum == 1) ? "" : ("-" + current.OrderIDNum.ToString())));
						if (orderInfo2 != null)
						{
							if (oldorderinfo.PayDate.HasValue)
							{
								orderInfo2.PayDate = oldorderinfo.PayDate;
							}
							int num4 = 0;
							foreach (LineItemInfo current2 in orderInfo2.LineItems.Values)
							{
								if (current2.OrderItemsStatus.ToString() == OrderStatus.Refunded.ToString() || current2.OrderItemsStatus.ToString() == OrderStatus.Returned.ToString())
								{
									num4++;
								}
							}
							if (orderInfo2.LineItems.Values.Count == num4)
							{
								orderInfo2.OrderStatus = OrderStatus.Closed;
							}
							new OrderDao().UpdateOrder(orderInfo2, null);
						}
					}
				}
				catch
				{
					dbTransaction.Rollback();
					text = "系统错误";
				}
				finally
				{
					dbConnection.Close();
				}
			}
			result = text;
			return result;
		}

		public static bool DelOrderSplitByOrderID(string oldorderid, System.Data.Common.DbTransaction dbTran = null)
		{
			return new OrderSplitDao().DelOrderSplitByOrderID(oldorderid, dbTran);
		}

		public static string CancelSplitOrderByID(string oldorderid, int fromsplitid, int itemid)
		{
			string result = string.Empty;
			OrderSplitInfo orderSplitInfo = OrderSplitHelper.GetOrderSplitInfo(fromsplitid);
			if (orderSplitInfo != null)
			{
				string itemList = orderSplitInfo.ItemList;
				OrderSplitInfo orderSplitInfoByOrderIDAndNum = OrderSplitHelper.GetOrderSplitInfoByOrderIDAndNum(1, oldorderid);
				if (orderSplitInfoByOrderIDAndNum != null)
				{
					if (!itemList.Contains(','))
					{
						orderSplitInfoByOrderIDAndNum.ItemList = orderSplitInfoByOrderIDAndNum.ItemList + "," + itemid.ToString();
						orderSplitInfoByOrderIDAndNum.UpdateTime = DateTime.Now;
						new OrderSplitDao().UpdateOrderSplitInfo(orderSplitInfoByOrderIDAndNum);
						new OrderSplitDao().DelOrderSplitInfo(fromsplitid);
						result = "1";
					}
					else
					{
						orderSplitInfoByOrderIDAndNum.ItemList = orderSplitInfoByOrderIDAndNum.ItemList + "," + itemid.ToString();
						orderSplitInfoByOrderIDAndNum.UpdateTime = DateTime.Now;
						new OrderSplitDao().UpdateOrderSplitInfo(orderSplitInfoByOrderIDAndNum);
						orderSplitInfo.ItemList = ("," + orderSplitInfo.ItemList + ",").Replace("," + itemid.ToString() + ",", ",").Trim(new char[]
						{
							','
						});
						orderSplitInfo.UpdateTime = DateTime.Now;
						new OrderSplitDao().UpdateOrderSplitInfo(orderSplitInfo);
						result = "1";
					}
				}
			}
			return result;
		}

		public static string OrderSplitToTemp(OrderInfo OldOrderInfo, string skuid, string neworderid, int itemid)
		{
			string result = string.Empty;
			string orderId = OldOrderInfo.OrderId;
			string text = string.Empty;
			string text2 = string.Empty;
			if (OldOrderInfo != null)
			{
				if (neworderid == "0")
				{
					IList<OrderSplitInfo> orderSplitItems = new OrderSplitDao().GetOrderSplitItems(orderId);
					if (orderSplitItems.Count == 0)
					{
						foreach (LineItemInfo current in OldOrderInfo.LineItems.Values)
						{
							if (current.ID == itemid)
							{
								text = current.ID.ToString();
							}
							else
							{
								text2 = text2 + "," + current.ID.ToString();
							}
						}
						text2 = text2.Trim(new char[]
						{
							','
						});
						OrderSplitInfo orderSplitInfo = new OrderSplitInfo();
						int num = 1;
						orderSplitInfo.OldOrderId = orderId;
						orderSplitInfo.OrderIDNum = num;
						orderSplitInfo.ItemList = text2;
						orderSplitInfo.UpdateTime = DateTime.Now;
						orderSplitInfo.AdjustedFreight = OldOrderInfo.AdjustedFreight;
						new OrderSplitDao().NewOrderSplit(orderSplitInfo);
						orderSplitInfo.ItemList = text;
						orderSplitInfo.OrderIDNum = num + 1;
						orderSplitInfo.UpdateTime = DateTime.Now;
						orderSplitInfo.AdjustedFreight = 0m;
						new OrderSplitDao().NewOrderSplit(orderSplitInfo);
						result = "1";
					}
					else
					{
						string text3 = string.Empty;
						int id = 0;
						foreach (OrderSplitInfo current2 in orderSplitItems)
						{
							if (current2.OrderIDNum == 1)
							{
								text3 = current2.ItemList;
								id = current2.Id;
								break;
							}
						}
						LineItemInfo returnMoneyByOrderIDAndProductID = new LineItemDao().GetReturnMoneyByOrderIDAndProductID(orderId, skuid, itemid);
						if (returnMoneyByOrderIDAndProductID != null && ("," + text3 + ",").Contains("," + returnMoneyByOrderIDAndProductID.ID + ","))
						{
							decimal d = 0m;
							decimal num2 = 0m;
							string[] array = text3.Split(new char[]
							{
								','
							});
							if (array.Length > 1)
							{
								string[] array2 = array;
								for (int i = 0; i < array2.Length; i++)
								{
									string s = array2[i];
									LineItemInfo lineItemInfo = new LineItemDao().GetLineItemInfo(Globals.ToNum(s), orderId);
									if (lineItemInfo != null)
									{
										decimal num3 = 0m;
										if (lineItemInfo.Type == 0)
										{
											num3 = lineItemInfo.ItemAdjustedPrice * lineItemInfo.Quantity - lineItemInfo.ItemAdjustedCommssion - lineItemInfo.DiscountAverage;
										}
										if (lineItemInfo.ID == itemid)
										{
											num2 = num3;
											text = lineItemInfo.ID.ToString();
										}
										d += num3;
									}
								}
								if (d > num2 && num2 > 0m)
								{
									OrderSplitInfo orderSplitInfo = new OrderSplitInfo();
									int num = new OrderSplitDao().GetMaxOrderIDNum(orderId);
									orderSplitInfo.Id = id;
									orderSplitInfo.OldOrderId = orderId;
									orderSplitInfo.OrderIDNum = 1;
									orderSplitInfo.ItemList = ("," + text3 + ",").Replace("," + returnMoneyByOrderIDAndProductID.ID.ToString() + ",", ",").Trim(new char[]
									{
										','
									});
									orderSplitInfo.UpdateTime = DateTime.Now;
									orderSplitInfo.AdjustedFreight = OldOrderInfo.AdjustedFreight;
									new OrderSplitDao().UpdateOrderSplitInfo(orderSplitInfo);
									orderSplitInfo.AdjustedFreight = 0m;
									orderSplitInfo.ItemList = returnMoneyByOrderIDAndProductID.ID.ToString();
									orderSplitInfo.OrderIDNum = num + 1;
									orderSplitInfo.UpdateTime = DateTime.Now;
									new OrderSplitDao().NewOrderSplit(orderSplitInfo);
									result = "1";
								}
								else
								{
									result = "-3";
								}
							}
							else
							{
								result = "-1";
							}
						}
						else
						{
							result = "-2";
						}
					}
				}
				else
				{
					IList<OrderSplitInfo> orderSplitItems = new OrderSplitDao().GetOrderSplitItems(orderId);
					if (orderSplitItems.Count > 0)
					{
						string text3 = string.Empty;
						int id = 0;
						int id2 = 0;
						int orderIDNum = 0;
						string str = string.Empty;
						int num4 = 0;
						foreach (OrderSplitInfo current2 in orderSplitItems)
						{
							if (current2.OrderIDNum == 1)
							{
								text3 = current2.ItemList;
								id = current2.Id;
								num4++;
							}
							if (current2.Id.ToString() == neworderid)
							{
								str = current2.ItemList;
								id2 = current2.Id;
								orderIDNum = current2.OrderIDNum;
								num4++;
							}
							if (num4 == 2)
							{
								break;
							}
						}
						decimal d = 0m;
						decimal num2 = 0m;
						string[] array = text3.Split(new char[]
						{
							','
						});
						if (array.Length > 1)
						{
							string[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								string s = array2[i];
								LineItemInfo lineItemInfo = new LineItemDao().GetLineItemInfo(Globals.ToNum(s), orderId);
								if (lineItemInfo != null)
								{
									decimal num3 = 0m;
									if (lineItemInfo.Type == 0)
									{
										num3 = lineItemInfo.ItemAdjustedPrice * lineItemInfo.Quantity - lineItemInfo.ItemAdjustedCommssion - lineItemInfo.DiscountAverage;
									}
									if (lineItemInfo.ID == itemid)
									{
										num2 = num3;
										text = lineItemInfo.ID.ToString();
									}
									d += num3;
								}
							}
							if (d > num2 && num2 > 0m)
							{
								OrderSplitInfo orderSplitInfo = new OrderSplitInfo();
								orderSplitInfo.Id = id;
								orderSplitInfo.OldOrderId = orderId;
								orderSplitInfo.OrderIDNum = 1;
								orderSplitInfo.ItemList = ("," + text3 + ",").Replace("," + text + ",", ",").Trim(new char[]
								{
									','
								});
								orderSplitInfo.UpdateTime = DateTime.Now;
								orderSplitInfo.AdjustedFreight = OldOrderInfo.AdjustedFreight;
								new OrderSplitDao().UpdateOrderSplitInfo(orderSplitInfo);
								orderSplitInfo.Id = id2;
								orderSplitInfo.AdjustedFreight = 0m;
								orderSplitInfo.ItemList = str + "," + text;
								orderSplitInfo.OrderIDNum = orderIDNum;
								orderSplitInfo.UpdateTime = DateTime.Now;
								new OrderSplitDao().UpdateOrderSplitInfo(orderSplitInfo);
								result = "1";
							}
							else
							{
								result = "-3";
							}
						}
						else
						{
							result = "-1";
						}
					}
					else
					{
						result = "-2";
					}
				}
			}
			return result;
		}
	}
}
