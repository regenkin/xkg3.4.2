using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Orders
{
	public class OrderDao
	{
		private Database database;

		public OrderDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public LineItemInfo GetNewLineItemInfo(LineItemInfo info)
		{
			ProductInfo productDetails = new ProductDao().GetProductDetails(info.ProductId);
			if (productDetails != null)
			{
				info.IsSetCommission = productDetails.IsSetCommission;
				info.FirstCommission = productDetails.FirstCommission;
				info.SecondCommission = productDetails.SecondCommission;
				info.ThirdCommission = productDetails.ThirdCommission;
			}
			return info;
		}

		public OrderInfo GetUserLastOrder(int userId)
		{
			OrderInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_Orders Where UserId=@UserId and OrderStatus=5 order by orderdate desc");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<OrderInfo>(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetUserOrderPaidWaitFinish(int userId)
		{
			string str = " AND (OrderStatus = 2 or (OrderStatus = 3 and Gateway<>'hishop.plugins.payment.podrequest')) ";
			string text = "SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId";
			text += str;
			text += " ORDER BY OrderDate DESC";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public System.Data.DataSet GetUserOrder(int userId, OrderQuery query)
		{
			string text = string.Empty;
			if (query.Status == OrderStatus.WaitBuyerPay)
			{
				text += " AND OrderStatus = 1 AND Gateway <> 'hishop.plugins.payment.podrequest'";
			}
			else if (query.Status == OrderStatus.BuyerAlreadyPaid)
			{
				text += " AND (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))";
			}
			else if (query.Status == OrderStatus.SellerAlreadySent)
			{
				text += " AND OrderStatus = 3 ";
			}
			string text2 = "SELECT OrderId,OrderMarking, OrderDate, OrderStatus,PaymentTypeId, OrderTotal,   Gateway,(SELECT count(0) FROM vshop_OrderRedPager WHERE OrderId = o.OrderId and ExpiryDays<getdate() and AlreadyGetTimes<MaxGetTimes) as HasRedPage,(SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum FROM Hishop_Orders o WHERE UserId = @UserId";
			text2 += text;
			text2 += " ORDER BY OrderDate DESC";
			text2 = text2 + " SELECT OrderId, ThumbnailsUrl, ItemDescription, SKUContent, SKU,OrderItemsStatus, ProductId,Quantity,ReturnMoney,SkuID FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId" + text + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text2);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataColumn parentColumn = dataSet.Tables[0].Columns["OrderId"];
			System.Data.DataColumn childColumn = dataSet.Tables[1].Columns["OrderId"];
			System.Data.DataRelation relation = new System.Data.DataRelation("OrderItems", parentColumn, childColumn);
			dataSet.Relations.Add(relation);
			return dataSet;
		}

		public DbQueryResult GetUserOrderByPage(int userId, OrderQuery query)
		{
			string text = string.Empty;
			if (query.Status == OrderStatus.WaitBuyerPay)
			{
				text += " AND OrderStatus = 1 AND Gateway <> 'hishop.plugins.payment.podrequest'";
			}
			else if (query.Status == OrderStatus.BuyerAlreadyPaid)
			{
				text += " AND (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))";
			}
			else if (query.Status == OrderStatus.SellerAlreadySent)
			{
				text += " AND OrderStatus = 3 ";
			}
			else
			{
				text += " AND OrderStatus<>12 ";
			}
			int num = (query.PageIndex - 1) * query.PageSize + 1;
			int num2 = num + query.PageSize - 1;
			string text2 = "SELECT OrderId FROM Hishop_Orders  WHERE OrderId in ( SELECT orderid from ( SELECT ROW_NUMBER() OVER (ORDER BY OrderDate DESC) AS ordernumber,OrderId  FROM Hishop_Orders o WHERE UserId = @UserId";
			text2 += text;
			object obj = text2;
			text2 = string.Concat(new object[]
			{
				obj,
				") AS W where W.ordernumber between ",
				num,
				" AND  ",
				num2,
				")"
			});
			string text3 = "SELECT * from( SELECT ROW_NUMBER() OVER (ORDER BY OrderDate DESC) AS ordernumber,OrderId,OrderMarking,OrderDate,OrderStatus,PaymentTypeId,OrderTotal,Gateway,(SELECT count(0) FROM vshop_OrderRedPager WHERE OrderId = o.OrderId and ExpiryDays<getdate() and AlreadyGetTimes<MaxGetTimes   ) as HasRedPage,(SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum FROM Hishop_Orders o WHERE UserId = @UserId";
			text3 += text;
			obj = text3;
			text3 = string.Concat(new object[]
			{
				obj,
				") AS W where W.ordernumber between ",
				num,
				" AND  ",
				num2
			});
			text3 = text3 + " SELECT ID, OrderId, ThumbnailsUrl, ItemDescription, SKUContent, SKU,OrderItemsStatus, ProductId,Quantity,ReturnMoney,SkuID,ItemAdjustedPrice,Type,PointNumber,LimitedTimeDiscountId FROM Hishop_OrderItems WHERE OrderId IN  (" + text2 + ")   ";
			text3 += "  SELECT COUNT(*) FROM Hishop_Orders WHERE UserId = @UserId";
			text3 += text;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text3);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataColumn parentColumn = dataSet.Tables[0].Columns["OrderId"];
			System.Data.DataColumn childColumn = dataSet.Tables[1].Columns["OrderId"];
			System.Data.DataRelation relation = new System.Data.DataRelation("OrderItems", parentColumn, childColumn);
			dataSet.Relations.Add(relation);
			return new DbQueryResult
			{
				Data = dataSet,
				TotalRecords = int.Parse(dataSet.Tables[2].Rows[0][0].ToString())
			};
		}

		public int GetOrderReferralUserId(string OrderId)
		{
			string query = "select ReferralUserId from Hishop_Orders where OrderId=@OrderId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, OrderId);
			int result = 0;
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			return result;
		}

		public OrderInfo GetCalculadtionCommission(OrderInfo order, int isModifyOrders)
		{
			DistributorsDao distributorsDao = new DistributorsDao();
			DistributorGradeDao distributorGradeDao = new DistributorGradeDao();
			DistributorsInfo distributorsInfo = null;
			if (order.ReferralUserId > 0)
			{
				distributorsInfo = distributorsDao.GetDistributorInfo(order.ReferralUserId);
			}
			if (distributorsInfo != null)
			{
				decimal d = 0m;
				decimal d2 = 0m;
				decimal d3 = 0m;
				bool flag = false;
				bool flag2 = false;
				System.Data.DataView defaultView = distributorGradeDao.GetAllDistributorGrade().DefaultView;
				bool flag3 = distributorsInfo.ReferralStatus == 0;
				if (distributorsInfo.DistriGradeId.ToString() != "0")
				{
					defaultView.RowFilter = " GradeId=" + distributorsInfo.DistriGradeId;
					if (defaultView.Count > 0)
					{
						d = decimal.Parse(defaultView[0]["FirstCommissionRise"].ToString());
					}
				}
				if (!string.IsNullOrEmpty(distributorsInfo.ReferralPath) && distributorsInfo.ReferralPath != "0")
				{
					string[] array = distributorsInfo.ReferralPath.Split(new char[]
					{
						'|'
					});
					if (array.Length == 1)
					{
						DistributorsInfo distributorInfo = distributorsDao.GetDistributorInfo(Globals.ToNum(array[0]));
						if (distributorInfo != null)
						{
							flag = (distributorInfo.ReferralStatus == 0);
							if (distributorInfo.DistriGradeId.ToString() != "0")
							{
								defaultView.RowFilter = " GradeId=" + distributorInfo.DistriGradeId;
								if (defaultView.Count > 0)
								{
									d2 = decimal.Parse(defaultView[0]["SecondCommissionRise"].ToString());
								}
							}
						}
					}
					else
					{
						DistributorsInfo distributorInfo = distributorsDao.GetDistributorInfo(Globals.ToNum(array[1]));
						if (distributorInfo != null)
						{
							flag = (distributorInfo.ReferralStatus == 0);
							if (distributorInfo.DistriGradeId.ToString() != "0")
							{
								defaultView.RowFilter = " GradeId=" + distributorInfo.DistriGradeId;
								if (defaultView.Count > 0)
								{
									d2 = decimal.Parse(defaultView[0]["SecondCommissionRise"].ToString());
								}
							}
						}
						DistributorsInfo distributorInfo2 = distributorsDao.GetDistributorInfo(Globals.ToNum(array[0]));
						if (distributorInfo2 != null)
						{
							flag2 = (distributorInfo2.ReferralStatus == 0);
							if (distributorInfo2.DistriGradeId.ToString() != "0")
							{
								defaultView.RowFilter = " GradeId=" + distributorInfo2.DistriGradeId;
								if (defaultView.Count > 0)
								{
									d3 = decimal.Parse(defaultView[0]["ThirdCommissionRise"].ToString());
								}
							}
						}
					}
				}
				if (flag || flag2)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
					if (!masterSettings.EnableCommission)
					{
						flag = false;
						flag2 = false;
					}
				}
				Dictionary<string, LineItemInfo> lineItems = order.LineItems;
				LineItemInfo lineItemInfo = new LineItemInfo();
				System.Data.DataView defaultView2 = new CategoryDao().GetCategories().DefaultView;
				foreach (KeyValuePair<string, LineItemInfo> current in lineItems)
				{
					lineItemInfo = current.Value;
					if (lineItemInfo.Type == 0)
					{
						lineItemInfo.ItemsCommission = 0m;
						lineItemInfo.SecondItemsCommission = 0m;
						lineItemInfo.ThirdItemsCommission = 0m;
						decimal num = lineItemInfo.GetSubTotal() - lineItemInfo.DiscountAverage - lineItemInfo.ItemAdjustedCommssion;
						if (num > 0m)
						{
							lineItemInfo = this.GetNewLineItemInfo(lineItemInfo);
							if (lineItemInfo.IsSetCommission)
							{
								order.FirstCommission = (lineItemInfo.FirstCommission + d) / 100m;
								order.SecondCommission = (lineItemInfo.SecondCommission + d2) / 100m;
								order.ThirdCommission = (lineItemInfo.ThirdCommission + d3) / 100m;
								lineItemInfo.ItemsCommission = (flag3 ? ((lineItemInfo.FirstCommission > 0m) ? (order.FirstCommission * num) : 0m) : 0m);
								lineItemInfo.SecondItemsCommission = (flag ? ((lineItemInfo.SecondCommission > 0m) ? (order.SecondCommission * num) : 0m) : 0m);
								lineItemInfo.ThirdItemsCommission = (flag2 ? ((lineItemInfo.ThirdCommission > 0m) ? (order.ThirdCommission * num) : 0m) : 0m);
							}
							else
							{
								System.Data.DataTable productCategories = new ProductDao().GetProductCategories(lineItemInfo.ProductId);
								if (productCategories.Rows.Count > 0)
								{
									if (productCategories.Rows[0][0].ToString() != "0")
									{
										defaultView2.RowFilter = " CategoryId=" + productCategories.Rows[0][0];
										string text = defaultView2[0]["FirstCommission"].ToString();
										string text2 = defaultView2[0]["SecondCommission"].ToString();
										string text3 = defaultView2[0]["ThirdCommission"].ToString();
										order.FirstCommission = (decimal.Parse(text) + d) / 100m;
										order.SecondCommission = (decimal.Parse(text2) + d2) / 100m;
										order.ThirdCommission = (decimal.Parse(text3) + d3) / 100m;
										if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
										{
											lineItemInfo.ItemsCommission = (flag3 ? ((decimal.Parse(text) > 0m) ? (order.FirstCommission * num) : 0m) : 0m);
											lineItemInfo.SecondItemsCommission = (flag ? ((decimal.Parse(text2) > 0m) ? (order.SecondCommission * num) : 0m) : 0m);
											lineItemInfo.ThirdItemsCommission = (flag2 ? ((decimal.Parse(text3) > 0m) ? (order.ThirdCommission * num) : 0m) : 0m);
										}
									}
								}
							}
						}
						else
						{
							lineItemInfo.ItemsCommission = 0m;
							lineItemInfo.SecondItemsCommission = 0m;
							lineItemInfo.ThirdItemsCommission = 0m;
						}
					}
					if (!string.IsNullOrEmpty(distributorsInfo.ReferralPath) && distributorsInfo.ReferralPath != "0")
					{
						string[] array = distributorsInfo.ReferralPath.Split(new char[]
						{
							'|'
						});
						if (array.Length == 1)
						{
							lineItemInfo.ThirdItemsCommission = 0m;
						}
					}
					else
					{
						lineItemInfo.SecondItemsCommission = 0m;
						lineItemInfo.ThirdItemsCommission = 0m;
					}
				}
			}
			return order;
		}

		public System.Data.DataSet GetUserOrderReturn(int userId, OrderQuery query)
		{
			string text = string.Empty;
			text += " AND (OrderStatus = 2 OR OrderStatus = 3) ";
			string text2 = "SELECT OrderId, OrderDate, OrderStatus,PaymentTypeId, OrderTotal, (SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum FROM Hishop_Orders o WHERE UserId = @UserId";
			text2 += text;
			text2 += " ORDER BY OrderDate DESC";
			text2 = text2 + " SELECT OrderId, ThumbnailsUrl,Quantity, ItemDescription,OrderItemsStatus, SKUContent, SKU, ProductId,SkuID FROM Hishop_OrderItems WHERE IsHandled=0 and Type=0 and (OrderItemsStatus=2 OR OrderItemsStatus=3) AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId " + text + ") ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text2);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataColumn parentColumn = dataSet.Tables[0].Columns["OrderId"];
			System.Data.DataColumn childColumn = dataSet.Tables[1].Columns["OrderId"];
			System.Data.DataRelation relation = new System.Data.DataRelation("OrderItems", parentColumn, childColumn);
			dataSet.Relations.Add(relation);
			return dataSet;
		}

		public int GetUserOrderReturnCount(int userId)
		{
			string text = string.Empty;
			object obj = text;
			text = string.Concat(new object[]
			{
				obj,
				" AND (OrderItemsStatus = ",
				6,
				" OR OrderItemsStatus =",
				7,
				")"
			});
			string text2 = "SELECT COUNT(*) FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId=@UserId)";
			text2 += text;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text2);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}

		public System.Data.DataSet GetDistributorOrder(OrderQuery query)
		{
			string text = string.Empty;
			if (query.Status == OrderStatus.Finished)
			{
				text = text + " AND OrderStatus=" + (int)query.Status;
			}
			string text2 = "SELECT OrderId, OrderDate,FinishDate, OrderStatus,PaymentTypeId, OrderTotal,Gateway,FirstCommission,SecondCommission,ThirdCommission FROM Hishop_Orders o WHERE ReferralUserId = @UserId";
			text2 += text;
			text2 += " ORDER BY OrderDate DESC";
			text2 = text2 + " SELECT ID,OrderId,SkuId, ThumbnailsUrl, ItemDescription, SKUContent, SKU, ProductId,Quantity,ItemListPrice,ItemAdjustedCommssion,OrderItemsStatus,ItemsCommission,Type,ReturnMoney,IsAdminModify,LimitedTimeDiscountId FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE ReferralUserId = @UserId" + text + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text2);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, query.UserId);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataColumn parentColumn = dataSet.Tables[0].Columns["OrderId"];
			System.Data.DataColumn childColumn = dataSet.Tables[1].Columns["OrderId"];
			System.Data.DataRelation relation = new System.Data.DataRelation("OrderItems", parentColumn, childColumn);
			dataSet.Relations.Add(relation);
			return dataSet;
		}

		public System.Data.DataSet GetDistributorOrderByDetials(OrderQuery query)
		{
			string text = string.Empty;
			if (query.Status == OrderStatus.Finished)
			{
				text = text + " AND OrderStatus=" + (int)query.Status;
			}
			string text2 = "SELECT OrderId, OrderDate,FinishDate, OrderStatus,PaymentTypeId, OrderTotal,Gateway,FirstCommission,SecondCommission,ThirdCommission FROM Hishop_Orders o WHERE OrderId in (select OrderId from Hishop_Commissions where UserId=@UserId and ReferralUserId=@ReferralUserId) ";
			text2 += text;
			text2 += " ORDER BY OrderDate DESC";
			text2 += " SELECT OrderId,SkuId, ThumbnailsUrl, ItemDescription, SKUContent, SKU, ProductId,Quantity,ItemListPrice,ItemAdjustedCommssion,OrderItemsStatus,ItemsCommission,Type,ReturnMoney,IsAdminModify,LimitedTimeDiscountId FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Commissions WHERE ReferralUserId = @ReferralUserId and  UserId=@UserId ) ORDER BY OrderId DESC";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text2);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, query.UserId);
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int32, query.ReferralUserId);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataColumn parentColumn = dataSet.Tables[0].Columns["OrderId"];
			System.Data.DataColumn childColumn = dataSet.Tables[1].Columns["OrderId"];
			System.Data.DataRelation relation = new System.Data.DataRelation("OrderItems", parentColumn, childColumn);
			dataSet.Relations.Add(relation);
			return dataSet;
		}

		public DbQueryResult GetDistributorOrderByStatus(OrderQuery query, int userId)
		{
			string text = string.Empty;
			if (userId > 0)
			{
				text += string.Format("  UserId={0}", userId);
			}
			if (query.Status == OrderStatus.Finished)
			{
				text = text + " AND OrderStatus=" + (int)query.Status;
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_UserOrderByPage", "OrderId", text, "*");
		}

		public bool InsertCalculationCommission(ArrayList UserIdList, ArrayList ReferralBlanceList, string orderid, ArrayList OrdersTotalList, string userid)
		{
			string text = "";
			text += "begin try  ";
			text += "  begin tran TranUpdate";
			for (int i = 0; i < UserIdList.Count; i++)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					" INSERT INTO [Hishop_Commissions]([UserId],[ReferralUserId],[OrderId],[OrderTotal],[CommTotal],[CommType],[State])VALUES(",
					UserIdList[i],
					",",
					userid,
					",'",
					orderid,
					"',",
					OrdersTotalList[i],
					",",
					ReferralBlanceList[i],
					",1,0);"
				});
			}
			text += " COMMIT TRAN TranUpdate";
			text += "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int GetUserOrderCount(int userId, OrderQuery query)
		{
			string text = string.Empty;
			if (query.Status == OrderStatus.WaitBuyerPay)
			{
				text += " AND OrderStatus = 1 AND Gateway <> 'hishop.plugins.payment.podrequest'";
			}
			else if (query.Status == OrderStatus.SellerAlreadySent)
			{
				text += " AND OrderStatus = 3  ";
			}
			else if (query.Status == OrderStatus.BuyerAlreadyPaid)
			{
				text += " AND (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))";
			}
			string text2 = "SELECT COUNT(1)  FROM Hishop_Orders o WHERE UserId = @UserId";
			text2 += text;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text2);
			sqlStringCommand.CommandType = System.Data.CommandType.Text;
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}

		public int GetDistributorOrderCount(OrderQuery query)
		{
			string text = string.Empty;
			OrderStatus status = query.Status;
			if (status != OrderStatus.Finished)
			{
				if (status == OrderStatus.Today)
				{
					string str = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
					text = text + " AND OrderDate>='" + str + "'";
				}
			}
			else
			{
				text = text + " AND OrderStatus=" + (int)query.Status;
			}
			string text2 = "SELECT COUNT(*)  FROM Hishop_Orders o WHERE ReferralUserId = @ReferralUserId";
			text2 += text;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text2);
			sqlStringCommand.CommandType = System.Data.CommandType.Text;
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int32, query.UserId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}

		public bool UpdateOrderCompany(string orderId, string companycode, string companyname, string shipNumber)
		{
			string query = "UPDATE Hishop_Orders SET ShipOrderNumber=@ShipOrderNumber,ExpressCompanyAbb=@ExpressCompanyAbb,ExpressCompanyName=@ExpressCompanyName WHERE OrderId =@OrderId";
			if (string.IsNullOrEmpty(shipNumber))
			{
				query = "UPDATE Hishop_Orders SET ExpressCompanyAbb=@ExpressCompanyAbb,ExpressCompanyName=@ExpressCompanyName WHERE OrderId =@OrderId";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, companycode);
			this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, shipNumber);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, companyname);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool EditOrderShipNumber(string orderId, string shipNumber)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShipOrderNumber=@ShipOrderNumber WHERE OrderId =@OrderId");
			this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, shipNumber);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DbQueryResult GetDeleteOrders(OrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (query.Type.HasValue)
			{
				if (query.Type.Value == OrderQuery.OrderType.GroupBuy)
				{
					stringBuilder.Append(" And GroupBuyId > 0 ");
				}
				else
				{
					stringBuilder.Append(" And GroupBuyId is null ");
				}
			}
			if (query.OrderId != string.Empty && query.OrderId != null)
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = '{0}'", query.UserId.Value);
			}
			if (query.PaymentType.HasValue)
			{
				stringBuilder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
			}
			if (query.GroupBuyId.HasValue)
			{
				stringBuilder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
			}
			if (query.OrderItemsStatus.HasValue)
			{
				if (query.OrderItemsStatus.Value == OrderStatus.ApplyForRefund)
				{
					stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus in({0},{1}))", (int)query.OrderItemsStatus.Value, 7);
				}
				else
				{
					stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus={0})", (int)query.OrderItemsStatus.Value);
				}
			}
			if (!string.IsNullOrEmpty(query.ShipTo))
			{
				stringBuilder.AppendFormat(" AND (ShipTo LIKE '%{0}%' or CellPhone='{0}')", DataHelper.CleanSearchString(query.ShipTo));
			}
			if (query.RegionId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
			}
			stringBuilder.AppendFormat(" AND OrderStatus = {0}", 12);
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.ShippingModeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
			}
			if (query.IsPrinted.HasValue)
			{
				stringBuilder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
			}
			if (query.ShippingModeId > 0)
			{
				stringBuilder.AppendFormat(" AND ShippingModeId={0}", query.ShippingModeId);
			}
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				stringBuilder.AppendFormat(" AND StoreName like '%{0}%' ", DataHelper.CleanSearchString(query.StoreName));
			}
			if (!string.IsNullOrEmpty(query.Gateway))
			{
				stringBuilder.AppendFormat(" AND Gateway='{0}' ", DataHelper.CleanSearchString(query.Gateway));
			}
			if (query.DeleteBeforeState > 0)
			{
				stringBuilder.AppendFormat(" AND DeleteBeforeState='{0}' ", DataHelper.CleanSearchString(query.DeleteBeforeState.ToString()));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_Order", "OrderId", stringBuilder.ToString(), "*");
		}

		public DbQueryResult GetOrders(OrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (query.Type.HasValue)
			{
				if (query.Type.Value == OrderQuery.OrderType.GroupBuy)
				{
					stringBuilder.Append(" And GroupBuyId > 0 ");
				}
				else
				{
					stringBuilder.Append(" And GroupBuyId is null ");
				}
			}
			if (query.OrderId != string.Empty && query.OrderId != null)
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = '{0}'", query.UserId.Value);
			}
			if (query.PaymentType.HasValue)
			{
				stringBuilder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
			}
			if (query.GroupBuyId.HasValue)
			{
				stringBuilder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
			}
			if (query.OrderItemsStatus.HasValue)
			{
				if (query.OrderItemsStatus.Value == OrderStatus.ApplyForRefund)
				{
					stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus in({0},{1}))", (int)query.OrderItemsStatus.Value, 7);
				}
				else
				{
					stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus={0})", (int)query.OrderItemsStatus.Value);
				}
			}
			if (!string.IsNullOrEmpty(query.ShipTo))
			{
				stringBuilder.AppendFormat(" AND (ShipTo LIKE '%{0}%' or CellPhone='{0}')", DataHelper.CleanSearchString(query.ShipTo));
			}
			if (query.RegionId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.Status == OrderStatus.History)
			{
				stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", new object[]
				{
					1,
					4,
					9,
					DateTime.Now.AddMonths(-3)
				});
			}
			else if (query.Status == OrderStatus.BuyerAlreadyPaid)
			{
				stringBuilder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))", (int)query.Status);
			}
			else if (query.Status != OrderStatus.All)
			{
				stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
			}
			stringBuilder.AppendFormat(" AND OrderStatus != {0}", 12);
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.ShippingModeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
			}
			if (query.IsPrinted.HasValue)
			{
				stringBuilder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
			}
			if (query.ShippingModeId > 0)
			{
				stringBuilder.AppendFormat(" AND ShippingModeId={0}", query.ShippingModeId);
			}
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				stringBuilder.AppendFormat(" AND StoreName like '%{0}%' ", DataHelper.CleanSearchString(query.StoreName));
			}
			if (!string.IsNullOrEmpty(query.Gateway))
			{
				stringBuilder.AppendFormat(" AND Gateway='{0}' ", DataHelper.CleanSearchString(query.Gateway));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_Order", "OrderId", stringBuilder.ToString(), "*");
		}

		public System.Data.DataTable GetSendGoodsOrders(string orderIds)
		{
			if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
			{
				orderIds = "'" + orderIds.Replace(",", "','") + "'";
			}
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Hishop_Orders WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) AND OrderId IN ({0}) order by OrderDate desc", orderIds));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataSet GetOrdersAndLines(string orderIds)
		{
			if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
			{
				orderIds = "'" + orderIds.Replace(",", "','") + "'";
			}
			this.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT * FROM Hishop_Orders WHERE  OrderId IN ({0}) order by  ShipOrderNumber asc,OrderDate desc ", orderIds);
			stringBuilder.AppendFormat(" SELECT * FROM Hishop_OrderItems WHERE OrderId IN ({0});", orderIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand);
		}

		public System.Data.DataSet GetOrderGoods(string orderIds)
		{
			if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
			{
				orderIds = "'" + orderIds.Replace(",", "','") + "'";
			}
			this.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT OrderId, ItemDescription AS ProductName, SKU, SKUContent, ShipmentQuantity,");
			stringBuilder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + oi.ShipmentQuantity AS Stock, (SELECT Remark FROM Hishop_Orders WHERE OrderId = oi.OrderId) AS Remark");
			stringBuilder.Append(" FROM Hishop_OrderItems oi WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
			stringBuilder.Append(" AND (OrderItemsStatus=2 OR OrderItemsStatus=1)");
			stringBuilder.AppendFormat(" AND OrderId IN ({0}) ORDER BY OrderId;", orderIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand);
		}

		public System.Data.DataSet GetProductGoods(string orderIds)
		{
			if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
			{
				orderIds = "'" + orderIds.Replace(",", "','") + "'";
			}
			this.database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ItemDescription AS ProductName, SKU, SKUContent, sum(ShipmentQuantity) as ShipmentQuantity,");
			stringBuilder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + sum(ShipmentQuantity) AS Stock FROM Hishop_OrderItems oi");
			stringBuilder.Append(" WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
			stringBuilder.Append(" AND OrderItemsStatus=2");
			stringBuilder.AppendFormat(" AND OrderId in ({0}) GROUP BY ItemDescription, SkuId, SKU, SKUContent;", orderIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand);
		}

		public System.Data.DataSet GetOrdersByOrderIDList(string orderIds)
		{
			if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
			{
				orderIds = "'" + orderIds.Replace(",", "','") + "'";
			}
			this.database = DatabaseFactory.CreateDatabase();
			string query = string.Empty;
			string text = " OrderId,ShipTo,RegionId,ExpressCompanyName,ExpressCompanyAbb,ShipOrderNumber,Remark,OrderStatus,ShippingRegion,Address";
			query = string.Format(string.Concat(new string[]
			{
				"with v as (SELECT ",
				text,
				", row_number() over (partition by ShipTo+CONVERT(VARCHAR(11), RegionId)+ExpressCompanyAbb+[Address]+CellPhone order by  RegionId desc) as rownumber from Hishop_Orders where   OrderId in ({0})) select ",
				text,
				",OrderStatus,rownumber from v"
			}), orderIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand);
		}

		public OrderInfo GetOrderInfo(string orderId)
		{
			OrderInfo orderInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Orders Where OrderId = @OrderId;  SELECT i.*,o.OrderStatus FROM Hishop_OrderItems i,Hishop_Orders o Where i.OrderId=o.OrderId AND i.OrderId = @OrderId ");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					orderInfo = DataMapper.PopulateOrder(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					orderInfo.LineItems.Add(dataReader["Id"].ToString(), DataMapper.PopulateLineItem(dataReader));
				}
			}
			return orderInfo;
		}

		public OrderInfo GetOrderInfoForLineItems(string orderId)
		{
			OrderInfo orderInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select isnull(b.OpenId,'') as  BuyerWXOpenId, isnull(c.OpenId,'') as  SalerWXOpenId  ,a.*\r\n                    from Hishop_Orders a  \r\n                    left join aspnet_Members b on a.UserId= b.UserId\r\n                    left join aspnet_Members c on a.ReferralUserId= c.UserId\r\n                    where a.OrderId = @OrderId;  \r\n               SELECT i.*,o.OrderStatus FROM Hishop_OrderItems i,Hishop_Orders o Where i.OrderId=o.OrderId AND i.OrderId = @OrderId ");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					orderInfo = DataMapper.PopulateOrder(dataReader);
				}
				dataReader.NextResult();
				orderInfo.ItemCount = 0;
				while (dataReader.Read())
				{
					orderInfo.LineItems.Add(dataReader["ID"].ToString(), DataMapper.PopulateLineItem(dataReader));
					orderInfo.ItemCount++;
				}
			}
			return orderInfo;
		}

		public int RealDeleteOrders(string orderIds)
		{
			if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
			{
				orderIds = "'" + orderIds.Replace(",", "','") + "'";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_OrderItems WHERE OrderId IN({0});DELETE FROM Hishop_OrderReturns WHERE OrderId IN({0});", orderIds));
			this.database.ExecuteNonQuery(sqlStringCommand);
			sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_Orders WHERE OrderId IN({0})", orderIds));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int RestoreOrders(string orderIds)
		{
			if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
			{
				orderIds = "'" + orderIds.Replace(",", "','") + "'";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("update Hishop_OrderItems set OrderItemsStatus =DeleteBeforeState  WHERE OrderId IN({0}); update Hishop_Orders set OrderStatus= DeleteBeforeState  WHERE OrderId IN({0}) ", orderIds));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int DeleteOrders(string orderIds)
		{
			if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
			{
				orderIds = "'" + orderIds.Replace(",", "','") + "'";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("update Hishop_OrderItems set DeleteBeforeState=OrderItemsStatus  WHERE OrderId IN({0});update Hishop_Orders set DeleteBeforeState=OrderStatus  WHERE OrderId IN({0})", orderIds));
			this.database.ExecuteNonQuery(sqlStringCommand);
			sqlStringCommand = this.database.GetSqlStringCommand(string.Format("update  Hishop_OrderItems set OrderItemsStatus={0} WHERE OrderId IN({1})", 12, orderIds));
			this.database.ExecuteNonQuery(sqlStringCommand);
			sqlStringCommand = this.database.GetSqlStringCommand(string.Format("update Hishop_Orders set OrderStatus={0}  WHERE OrderId IN({1})", 12, orderIds));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int GetCouponId(int ActDid, decimal OrderTotal, int OrderNumber)
		{
			string query = "select top 1 CouponId from Hishop_Activities_Detail where ActivitiesId=@ActivitiesId and  MeetMoney=<@OrderTotal and MeetNumber<=@OrderNumber order by  MeetMoney,MeetNumber DESC";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, ActDid);
			this.database.AddInParameter(sqlStringCommand, "OrderTotal", System.Data.DbType.Decimal, OrderTotal);
			this.database.AddInParameter(sqlStringCommand, "OrderNumber", System.Data.DbType.Int32, OrderNumber);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result = 0;
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			return result;
		}

		public bool CreatOrder(OrderInfo orderInfo, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_CreateOrder");
			this.database.AddInParameter(storedProcCommand, "OrderId", System.Data.DbType.String, orderInfo.OrderId);
			this.database.AddInParameter(storedProcCommand, "OrderMarking", System.Data.DbType.String, orderInfo.OrderMarking);
			this.database.AddInParameter(storedProcCommand, "OrderDate", System.Data.DbType.DateTime, orderInfo.OrderDate);
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, orderInfo.UserId);
			this.database.AddInParameter(storedProcCommand, "UserName", System.Data.DbType.String, orderInfo.Username);
			this.database.AddInParameter(storedProcCommand, "Wangwang", System.Data.DbType.String, orderInfo.Wangwang);
			this.database.AddInParameter(storedProcCommand, "RealName", System.Data.DbType.String, orderInfo.RealName);
			this.database.AddInParameter(storedProcCommand, "EmailAddress", System.Data.DbType.String, orderInfo.EmailAddress);
			this.database.AddInParameter(storedProcCommand, "Remark", System.Data.DbType.String, orderInfo.Remark);
			this.database.AddInParameter(storedProcCommand, "ClientShortType", System.Data.DbType.Int32, orderInfo.ClientShortType);
			this.database.AddInParameter(storedProcCommand, "AdjustedDiscount", System.Data.DbType.Currency, orderInfo.AdjustedDiscount);
			this.database.AddInParameter(storedProcCommand, "OrderStatus", System.Data.DbType.Int32, (int)orderInfo.OrderStatus);
			this.database.AddInParameter(storedProcCommand, "ShippingRegion", System.Data.DbType.String, orderInfo.ShippingRegion);
			this.database.AddInParameter(storedProcCommand, "Address", System.Data.DbType.String, orderInfo.Address);
			this.database.AddInParameter(storedProcCommand, "ZipCode", System.Data.DbType.String, orderInfo.ZipCode);
			this.database.AddInParameter(storedProcCommand, "ShipTo", System.Data.DbType.String, orderInfo.ShipTo);
			this.database.AddInParameter(storedProcCommand, "TelPhone", System.Data.DbType.String, orderInfo.TelPhone);
			this.database.AddInParameter(storedProcCommand, "CellPhone", System.Data.DbType.String, orderInfo.CellPhone);
			this.database.AddInParameter(storedProcCommand, "ShipToDate", System.Data.DbType.String, orderInfo.ShipToDate);
			this.database.AddInParameter(storedProcCommand, "ShippingModeId", System.Data.DbType.Int32, orderInfo.ShippingModeId);
			this.database.AddInParameter(storedProcCommand, "ModeName", System.Data.DbType.String, orderInfo.ModeName);
			this.database.AddInParameter(storedProcCommand, "RegionId", System.Data.DbType.Int32, orderInfo.RegionId);
			this.database.AddInParameter(storedProcCommand, "Freight", System.Data.DbType.Currency, orderInfo.Freight);
			this.database.AddInParameter(storedProcCommand, "AdjustedFreight", System.Data.DbType.Currency, orderInfo.AdjustedFreight);
			this.database.AddInParameter(storedProcCommand, "ShipOrderNumber", System.Data.DbType.String, orderInfo.ShipOrderNumber);
			this.database.AddInParameter(storedProcCommand, "Weight", System.Data.DbType.Int32, orderInfo.Weight);
			this.database.AddInParameter(storedProcCommand, "ExpressCompanyName", System.Data.DbType.String, orderInfo.ExpressCompanyName);
			this.database.AddInParameter(storedProcCommand, "ExpressCompanyAbb", System.Data.DbType.String, orderInfo.ExpressCompanyAbb);
			this.database.AddInParameter(storedProcCommand, "PaymentTypeId", System.Data.DbType.Int32, orderInfo.PaymentTypeId);
			this.database.AddInParameter(storedProcCommand, "PaymentType", System.Data.DbType.String, orderInfo.PaymentType);
			this.database.AddInParameter(storedProcCommand, "PayCharge", System.Data.DbType.Currency, orderInfo.PayCharge);
			this.database.AddInParameter(storedProcCommand, "RefundStatus", System.Data.DbType.Int32, (int)orderInfo.RefundStatus);
			this.database.AddInParameter(storedProcCommand, "Gateway", System.Data.DbType.String, orderInfo.Gateway);
			this.database.AddInParameter(storedProcCommand, "OrderTotal", System.Data.DbType.Currency, orderInfo.GetTotal());
			this.database.AddInParameter(storedProcCommand, "OrderPoint", System.Data.DbType.Int32, orderInfo.Points);
			this.database.AddInParameter(storedProcCommand, "OrderCostPrice", System.Data.DbType.Currency, orderInfo.GetCostPrice());
			this.database.AddInParameter(storedProcCommand, "OrderProfit", System.Data.DbType.Currency, orderInfo.GetProfit());
			this.database.AddInParameter(storedProcCommand, "Amount", System.Data.DbType.Currency, orderInfo.GetAmount());
			this.database.AddInParameter(storedProcCommand, "ReducedPromotionId", System.Data.DbType.Int32, orderInfo.ReducedPromotionId);
			this.database.AddInParameter(storedProcCommand, "ReducedPromotionName", System.Data.DbType.String, orderInfo.ReducedPromotionName);
			this.database.AddInParameter(storedProcCommand, "ReducedPromotionAmount", System.Data.DbType.Currency, orderInfo.ReducedPromotionAmount);
			this.database.AddInParameter(storedProcCommand, "IsReduced", System.Data.DbType.Boolean, orderInfo.IsReduced);
			this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionId", System.Data.DbType.Int32, orderInfo.SentTimesPointPromotionId);
			this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionName", System.Data.DbType.String, orderInfo.SentTimesPointPromotionName);
			this.database.AddInParameter(storedProcCommand, "TimesPoint", System.Data.DbType.Currency, orderInfo.TimesPoint);
			this.database.AddInParameter(storedProcCommand, "IsSendTimesPoint", System.Data.DbType.Boolean, orderInfo.IsSendTimesPoint);
			this.database.AddInParameter(storedProcCommand, "FreightFreePromotionId", System.Data.DbType.Int32, orderInfo.FreightFreePromotionId);
			this.database.AddInParameter(storedProcCommand, "FreightFreePromotionName", System.Data.DbType.String, orderInfo.FreightFreePromotionName);
			this.database.AddInParameter(storedProcCommand, "IsFreightFree", System.Data.DbType.Boolean, orderInfo.IsFreightFree);
			this.database.AddInParameter(storedProcCommand, "CouponName", System.Data.DbType.String, orderInfo.CouponName);
			this.database.AddInParameter(storedProcCommand, "CouponCode", System.Data.DbType.String, orderInfo.CouponCode);
			this.database.AddInParameter(storedProcCommand, "CouponAmount", System.Data.DbType.Currency, orderInfo.CouponAmount);
			this.database.AddInParameter(storedProcCommand, "CouponValue", System.Data.DbType.Currency, orderInfo.CouponValue);
			this.database.AddInParameter(storedProcCommand, "RedPagerActivityName", System.Data.DbType.String, orderInfo.RedPagerActivityName);
			this.database.AddInParameter(storedProcCommand, "RedPagerID", System.Data.DbType.String, orderInfo.RedPagerID);
			this.database.AddInParameter(storedProcCommand, "RedPagerOrderAmountCanUse", System.Data.DbType.Currency, orderInfo.RedPagerOrderAmountCanUse);
			this.database.AddInParameter(storedProcCommand, "RedPagerAmount", System.Data.DbType.Currency, orderInfo.RedPagerAmount);
			if (orderInfo.GroupBuyId > 0)
			{
				this.database.AddInParameter(storedProcCommand, "GroupBuyId", System.Data.DbType.Int32, orderInfo.GroupBuyId);
				this.database.AddInParameter(storedProcCommand, "NeedPrice", System.Data.DbType.Currency, orderInfo.NeedPrice);
				this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(storedProcCommand, "GroupBuyId", System.Data.DbType.Int32, DBNull.Value);
				this.database.AddInParameter(storedProcCommand, "NeedPrice", System.Data.DbType.Currency, DBNull.Value);
				this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", System.Data.DbType.Int32, DBNull.Value);
			}
			if (orderInfo.CountDownBuyId > 0)
			{
				this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", System.Data.DbType.Int32, orderInfo.CountDownBuyId);
			}
			else
			{
				this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", System.Data.DbType.Int32, DBNull.Value);
			}
			if (orderInfo.BundlingID > 0)
			{
				this.database.AddInParameter(storedProcCommand, "BundlingID ", System.Data.DbType.Int32, orderInfo.BundlingID);
				this.database.AddInParameter(storedProcCommand, "BundlingPrice", System.Data.DbType.Currency, orderInfo.BundlingPrice);
			}
			else
			{
				this.database.AddInParameter(storedProcCommand, "BundlingID ", System.Data.DbType.Int32, DBNull.Value);
				this.database.AddInParameter(storedProcCommand, "BundlingPrice", System.Data.DbType.Currency, DBNull.Value);
			}
			this.database.AddInParameter(storedProcCommand, "Tax", System.Data.DbType.Currency, orderInfo.Tax);
			this.database.AddInParameter(storedProcCommand, "InvoiceTitle", System.Data.DbType.String, orderInfo.InvoiceTitle);
			this.database.AddInParameter(storedProcCommand, "ReferralUserId", System.Data.DbType.Int32, orderInfo.ReferralUserId);
			this.database.AddInParameter(storedProcCommand, "ReferralPath", System.Data.DbType.String, orderInfo.ReferralPath);
			this.database.AddInParameter(storedProcCommand, "DiscountAmount", System.Data.DbType.Decimal, orderInfo.DiscountAmount);
			this.database.AddInParameter(storedProcCommand, "ActivitiesId", System.Data.DbType.String, orderInfo.ActivitiesId);
			this.database.AddInParameter(storedProcCommand, "ActivitiesName", System.Data.DbType.String, orderInfo.ActivitiesName);
			this.database.AddInParameter(storedProcCommand, "FirstCommission", System.Data.DbType.Decimal, orderInfo.FirstCommission);
			this.database.AddInParameter(storedProcCommand, "SecondCommission", System.Data.DbType.Decimal, orderInfo.SecondCommission);
			this.database.AddInParameter(storedProcCommand, "ThirdCommission", System.Data.DbType.Decimal, orderInfo.ThirdCommission);
			this.database.AddInParameter(storedProcCommand, "PointToCash", System.Data.DbType.Decimal, orderInfo.PointToCash);
			this.database.AddInParameter(storedProcCommand, "PointExchange", System.Data.DbType.Int32, orderInfo.PointExchange);
			this.database.AddInParameter(storedProcCommand, "BargainDetialId", System.Data.DbType.Int32, orderInfo.BargainDetialId);
			return this.database.ExecuteNonQuery(storedProcCommand, dbTran) > 0;
		}

		public bool UpdateCoupon_MemberCoupons(OrderInfo orderinfo, System.Data.Common.DbTransaction dbTran)
		{
			string query = "update Hishop_Coupon_MemberCoupons set OrderNo=@OrderNo, Status=@Status,UsedDate=@UsedDate WHERE Id=@Id;\r\n                        update Hishop_Coupon_Coupons set UsedNum=isnull(UsedNum,0)+1 where CouponId=(select top 1 CouponId From Hishop_Coupon_MemberCoupons where Id=@Id);";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderNo", System.Data.DbType.String, orderinfo.OrderId);
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, orderinfo.RedPagerID);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, 1);
			this.database.AddInParameter(sqlStringCommand, "UsedDate", System.Data.DbType.DateTime, DateTime.Now);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}

		public bool InsertPointExchange_Changed(PointExchangeChangedInfo info, System.Data.Common.DbTransaction dbTran, int itemCount = 1)
		{
			bool result;
			if (itemCount < 1)
			{
				result = false;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < itemCount; i++)
				{
					stringBuilder.Append("INSERT INTO  Hishop_PointExchange_Changed ([exChangeId],[exChangeName],[ProductId],[PointNumber],[Date],[MemberID],[MemberGrades]) VALUES (@exChangeId,@exChangeName,@ProductId,@PointNumber,@Date,@MemberID,@MemberGrades);");
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				this.database.AddInParameter(sqlStringCommand, "exChangeId", System.Data.DbType.Int32, info.exChangeId);
				this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, info.ProductId);
				this.database.AddInParameter(sqlStringCommand, "exChangeName", System.Data.DbType.String, info.exChangeName);
				this.database.AddInParameter(sqlStringCommand, "PointNumber", System.Data.DbType.Int32, info.PointNumber);
				this.database.AddInParameter(sqlStringCommand, "Date", System.Data.DbType.DateTime, DateTime.Now);
				this.database.AddInParameter(sqlStringCommand, "MemberID", System.Data.DbType.Int32, info.MemberID);
				this.database.AddInParameter(sqlStringCommand, "MemberGrades", System.Data.DbType.String, info.MemberGrades);
				if (dbTran != null)
				{
					result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
				}
				else
				{
					result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
				}
			}
			return result;
		}

		public bool AddMemberPointNumber(int PointNumber, OrderInfo orderInfo, System.Data.Common.DbTransaction dbTran)
		{
			IntegralDetailInfo integralDetailInfo = new IntegralDetailInfo();
			integralDetailInfo.IntegralChange = PointNumber;
			integralDetailInfo.IntegralSource = "获取积分-订单号：" + orderInfo.OrderId;
			integralDetailInfo.IntegralSourceType = 1;
			string activitiesName = orderInfo.ActivitiesName;
			if (!string.IsNullOrEmpty(activitiesName))
			{
				integralDetailInfo.Remark = "活动送积分：" + activitiesName;
			}
			else
			{
				integralDetailInfo.Remark = "购物获取积分";
			}
			integralDetailInfo.Userid = orderInfo.UserId;
			integralDetailInfo.GoToUrl = Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + orderInfo.OrderId;
			integralDetailInfo.IntegralStatus = Convert.ToInt32(IntegralDetailStatus.OrderToIntegral);
			bool result;
			if (!new IntegralDetailDao().AddIntegralDetail(integralDetailInfo, dbTran))
			{
				dbTran.Rollback();
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public bool UpdateCalculadtionCommission(OrderInfo orderinfo, System.Data.Common.DbTransaction dbTran = null)
		{
			foreach (LineItemInfo current in orderinfo.LineItems.Values)
			{
				if (current.OrderItemsStatus == OrderStatus.Refunded || current.OrderItemsStatus == OrderStatus.Returned)
				{
					new LineItemDao().UpdateCommissionItem(current.ID, 0m, 0m, 0m, dbTran);
				}
				else
				{
					new LineItemDao().UpdateCommissionItem(current.ID, current.ItemsCommission, current.SecondItemsCommission, current.ThirdItemsCommission, dbTran);
				}
			}
			return true;
		}

		public bool UpdateOrder(OrderInfo order, System.Data.Common.DbTransaction dbTran = null)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET  OrderStatus = @OrderStatus, CloseReason=@CloseReason, PayDate = @PayDate, ShippingDate=@ShippingDate, FinishDate = @FinishDate, RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone, ShippingModeId=@ShippingModeId ,ModeName=@ModeName, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, ShipOrderNumber = @ShipOrderNumber,  ExpressCompanyName = @ExpressCompanyName,ExpressCompanyAbb = @ExpressCompanyAbb, PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType, Gateway = @Gateway, ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark,IsPrinted=@IsPrinted, OrderTotal = @OrderTotal, OrderProfit=@OrderProfit,Amount=@Amount,OrderCostPrice=@OrderCostPrice, AdjustedFreight = @AdjustedFreight, PayCharge = @PayCharge, AdjustedDiscount=@AdjustedDiscount,OrderPoint=@OrderPoint,GatewayOrderId=@GatewayOrderId,OldAddress=@OldAddress WHERE OrderId = @OrderId");
			decimal total = order.GetTotal();
			decimal num = Globals.GetPoint(total);
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, (int)order.OrderStatus);
			this.database.AddInParameter(sqlStringCommand, "CloseReason", System.Data.DbType.String, order.CloseReason);
			this.database.AddInParameter(sqlStringCommand, "PayDate", System.Data.DbType.DateTime, order.PayDate);
			this.database.AddInParameter(sqlStringCommand, "ShippingDate", System.Data.DbType.DateTime, order.ShippingDate);
			this.database.AddInParameter(sqlStringCommand, "FinishDate", System.Data.DbType.DateTime, order.FinishDate);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.String, order.RegionId);
			this.database.AddInParameter(sqlStringCommand, "ShippingRegion", System.Data.DbType.String, order.ShippingRegion);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, order.Address);
			this.database.AddInParameter(sqlStringCommand, "ZipCode", System.Data.DbType.String, order.ZipCode);
			this.database.AddInParameter(sqlStringCommand, "ShipTo", System.Data.DbType.String, order.ShipTo);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, order.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, order.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "ShippingModeId", System.Data.DbType.Int32, order.ShippingModeId);
			this.database.AddInParameter(sqlStringCommand, "ModeName", System.Data.DbType.String, order.ModeName);
			this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", System.Data.DbType.Int32, order.RealShippingModeId);
			this.database.AddInParameter(sqlStringCommand, "RealModeName", System.Data.DbType.String, order.RealModeName);
			this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, order.ShipOrderNumber);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, order.ExpressCompanyName);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, order.ExpressCompanyAbb);
			this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", System.Data.DbType.Int32, order.PaymentTypeId);
			this.database.AddInParameter(sqlStringCommand, "PaymentType", System.Data.DbType.String, order.PaymentType);
			this.database.AddInParameter(sqlStringCommand, "Gateway", System.Data.DbType.String, order.Gateway);
			this.database.AddInParameter(sqlStringCommand, "ManagerMark", System.Data.DbType.Int32, order.ManagerMark);
			this.database.AddInParameter(sqlStringCommand, "ManagerRemark", System.Data.DbType.String, order.ManagerRemark);
			this.database.AddInParameter(sqlStringCommand, "IsPrinted", System.Data.DbType.Boolean, order.IsPrinted);
			this.database.AddInParameter(sqlStringCommand, "OrderTotal", System.Data.DbType.Currency, total);
			this.database.AddInParameter(sqlStringCommand, "OrderProfit", System.Data.DbType.Currency, order.GetProfit());
			this.database.AddInParameter(sqlStringCommand, "Amount", System.Data.DbType.Currency, order.GetAmount());
			this.database.AddInParameter(sqlStringCommand, "OrderCostPrice", System.Data.DbType.Currency, order.GetCostPrice());
			this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", System.Data.DbType.Currency, order.AdjustedFreight);
			this.database.AddInParameter(sqlStringCommand, "PayCharge", System.Data.DbType.Currency, order.PayCharge);
			this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", System.Data.DbType.Currency, order.AdjustedDiscount);
			this.database.AddInParameter(sqlStringCommand, "OrderPoint", System.Data.DbType.Int32, num);
			this.database.AddInParameter(sqlStringCommand, "GatewayOrderId", System.Data.DbType.String, order.GatewayOrderId);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "OldAddress", System.Data.DbType.String, order.OldAddress);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}

		public bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
		{
			if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
			{
				orderIds = "'" + orderIds.Replace(",", "','") + "'";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET RealShippingModeId=@RealShippingModeId,RealModeName=@RealModeName WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
			this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", System.Data.DbType.Int32, realShippingModeId);
			this.database.AddInParameter(sqlStringCommand, "RealModeName", System.Data.DbType.String, realModeName);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetPrintOrderExpress(string orderId, string expressCompanyName, string expressCompanyAbb, string shipOrderNumber)
		{
			string query = string.Empty;
			if (string.IsNullOrEmpty(shipOrderNumber))
			{
				query = "UPDATE Hishop_Orders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE  OrderId=@OrderId";
			}
			else
			{
				query = "UPDATE Hishop_Orders SET IsPrinted=1,ShipOrderNumber=@ShipOrderNumber,ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE  OrderId=@OrderId";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, shipOrderNumber);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, expressCompanyName);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, expressCompanyAbb);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
		{
			if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
			{
				orderIds = "'" + orderIds.Replace(",", "','") + "'";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, expressCompanyName);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, expressCompanyAbb);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void UpdatePayOrderStock(OrderInfo orderinfo)
		{
			int bargainDetialId = orderinfo.BargainDetialId;
			if (bargainDetialId > 0)
			{
				int num = 0;
				using (Dictionary<string, LineItemInfo>.ValueCollection.Enumerator enumerator = orderinfo.LineItems.Values.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						LineItemInfo current = enumerator.Current;
						num = current.Quantity;
					}
				}
				if (num > 0)
				{
					string query = "update Hishop_Bargain set TranNumber=TranNumber+@Num where Id=(select BargainId from Hishop_BargainDetial where id=" + bargainDetialId + " AND IsDelete=0)";
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
					this.database.AddInParameter(sqlStringCommand, "Num", System.Data.DbType.Int32, num);
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
			}
			this.UpdatePayOrderStock(orderinfo.OrderId);
		}

		public void UpdatePayOrderStock(string orderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void UpdateItemsStatus(string orderId, int status, string ItemStr)
		{
			string query = string.Empty;
			if (ItemStr == "all")
			{
				query = "Update Hishop_OrderItems Set OrderItemsStatus=@OrderItemsStatus Where OrderId =@OrderId";
			}
			else
			{
				query = "Update Hishop_OrderItems Set OrderItemsStatus=@OrderItemsStatus Where OrderId =@OrderId and SkuId IN (" + ItemStr + ")";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderItemsStatus", System.Data.DbType.Int32, status);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool UpdateRefundOrderStock(string orderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public bool CheckRefund(string orderId, string Operator, string adminRemark, int refundType, bool accept)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
			stringBuilder.Append(" update Hishop_OrderRefund set Operator=@Operator,AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId;");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 9);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 2);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 2);
			}
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, Operator);
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, adminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ExistsOrderByBargainDetialId(int userId, int bargainDetialId)
		{
			string query = "select count(*) from Hishop_Orders where BargainDetialId=@BargainDetialId and UserId=@UserId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "BargainDetialId", System.Data.DbType.Int32, bargainDetialId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString()) > 0;
		}

		public string GetexChangeName(int exChangeId)
		{
			string query = "select Name from Hishop_PointExChange_PointExChanges where id=" + exChangeId;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj == null || obj is DBNull)
			{
				result = "";
			}
			else
			{
				result = obj.ToString();
			}
			return result;
		}

		public string GetReplaceComments(string orderId)
		{
			string query = "select Comments from Hishop_OrderReplace where HandleStatus=0 and OrderId='" + orderId + "'";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj == null || obj is DBNull)
			{
				result = "";
			}
			else
			{
				result = obj.ToString();
			}
			return result;
		}

		public decimal GetCommossionByOrderId(string orderId, int userId)
		{
			string query = "select CommTotal from Hishop_Commissions WHERE OrderId=@OrderId AND UserId=@UserId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int16, userId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			decimal result;
			if (obj == null || obj is DBNull)
			{
				result = 0m;
			}
			else
			{
				result = (decimal)obj;
			}
			return result;
		}

		public int GetUserOrders(int userId)
		{
			string query = "select count(OrderId) from Hishop_Orders WHERE UserId=@UserId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int16, userId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj == null || obj is DBNull)
			{
				result = 0;
			}
			else
			{
				result = (int)obj;
			}
			return result;
		}

		public System.Data.DataTable GetAllOrderID()
		{
			string query = "select OrderId,IsPrinted,OrderStatus,Gateway from Hishop_Orders with (nolock) ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public int GetCountOrderIDByStatus(OrderStatus? orderstatus, OrderStatus? itemstatus)
		{
			string text = string.Empty;
			if (orderstatus.HasValue)
			{
				text = " OrderStatus=" + (int)orderstatus.Value;
			}
			if (itemstatus.HasValue)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += " and ";
				}
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					" OrderId in(SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus=",
					(int)itemstatus.Value,
					")"
				});
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = " where " + text;
			}
			string query = "select count(0) from Hishop_Orders with (nolock) " + text;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
		}

		public System.Data.DataTable GetOrderMarkingAllOrderID(string OrderMarking)
		{
			string query = "select OrderId from Hishop_Orders where OrderStatus=1 and OrderMarking='" + OrderMarking + "'";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public bool UpdateOrderSplitState(string orderid, int splitstate, System.Data.Common.DbTransaction dbTran = null)
		{
			string query = "update Hishop_Orders set SplitState=@SplitState where OrderID=@OrderID";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderid);
			this.database.AddInParameter(sqlStringCommand, "SplitState", System.Data.DbType.Int32, splitstate);
			return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
		}

		public bool DeleteReturnRecordForSendGoods(string orderid)
		{
			string query = string.Concat(new string[]
			{
				"delete from Hishop_OrderReturns where OrderID=@OrderID and (HandleStatus=",
				1.ToString(),
				" or HandleStatus=",
				6.ToString(),
				")"
			});
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool CombineOrderToPay(string orderIds, string orderMarking)
		{
			if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
			{
				orderIds = "'" + orderIds.Replace(",", "','") + "'";
			}
			string query = string.Format("update Hishop_Orders set OrderMarking=@OrderMarking WHERE OrderId IN({0})", orderIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderMarking", System.Data.DbType.String, orderMarking);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public string GetFirstProductName(string OrderId)
		{
			string query = string.Format("select top 1 ItemDescription from Hishop_OrderItems where OrderId= '{0}'", OrderId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Convert.ToString(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool GetOrderUserOpenId(string OrderId, out string BuyerWXOpenId, out string SalerWXOpenId)
		{
			BuyerWXOpenId = "";
			SalerWXOpenId = "";
			string query = string.Format("select top 1 isnull(b.OpenId,'') as  BuyerWXOpenId, isnull(c.OpenId,'') as  SalerWXOpenId  from Hishop_Orders a   left join aspnet_Members b on a.UserId= b.UserId  left join aspnet_Members c on a.ReferralUserId= c.UserId  where a.OrderId = '{0}'", OrderId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable dataTable = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
			bool result;
			if (dataTable.Rows.Count > 0)
			{
				BuyerWXOpenId = Convert.ToString(dataTable.Rows[0]["BuyerWXOpenId"]);
				SalerWXOpenId = Convert.ToString(dataTable.Rows[0]["SalerWXOpenId"]);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public bool GetOrderUserAliOpenId(string OrderId, out string BuyerAliOpenId, out string SalerAliOpenId)
		{
			BuyerAliOpenId = "";
			SalerAliOpenId = "";
			string query = string.Format("select top 1 isnull(b.AlipayOpenid,'') as  BuyerWXOpenId, isnull(c.AlipayOpenid,'') as  SalerWXOpenId  from Hishop_Orders a   left join aspnet_Members b on a.UserId= b.UserId  left join aspnet_Members c on a.ReferralUserId= c.UserId  where a.OrderId = '{0}'", OrderId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable dataTable = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
			bool result;
			if (dataTable.Rows.Count > 0)
			{
				BuyerAliOpenId = Convert.ToString(dataTable.Rows[0]["BuyerWXOpenId"]);
				SalerAliOpenId = Convert.ToString(dataTable.Rows[0]["SalerWXOpenId"]);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
