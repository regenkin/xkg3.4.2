using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.Jobs
{
	public class OrderJob : IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			Database database = DatabaseFactory.CreateDatabase();
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE I set OrderItemsStatus=4 from Hishop_Orders O,Hishop_OrderItems I where  I.OrderId=O.OrderId and  O.OrderStatus=1 AND O.OrderDate <= @OrderDate; UPDATE Hishop_Orders SET OrderStatus=4,CloseReason='到期自动关闭' WHERE OrderStatus=1 AND OrderDate <= @OrderDate;");
			database.AddInParameter(sqlStringCommand, "OrderDate", System.Data.DbType.DateTime, DateTime.Now.AddDays((double)(-(double)masterSettings.CloseOrderDays)));
			database.ExecuteNonQuery(sqlStringCommand);
			string query = string.Format("SELECT OrderId FROM  Hishop_Orders WHERE  OrderStatus=3 AND ShippingDate <= '" + DateTime.Now.AddDays((double)(-(double)masterSettings.FinishOrderDays)) + "'", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand2 = database.GetSqlStringCommand(query);
			System.Data.DataTable dataTable = database.ExecuteDataSet(sqlStringCommand2).Tables[0];
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				bool flag = false;
				OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(dataTable.Rows[i][0].ToString());
				Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
				LineItemInfo lineItemInfo = new LineItemInfo();
				foreach (KeyValuePair<string, LineItemInfo> current in lineItems)
				{
					lineItemInfo = current.Value;
					if (orderInfo.Gateway.Trim() == "hishop.plugins.payment.podrequest" || lineItemInfo.OrderItemsStatus == OrderStatus.ApplyForRefund || lineItemInfo.OrderItemsStatus == OrderStatus.ApplyForReturns)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					System.Data.Common.DbCommand sqlStringCommand3 = database.GetSqlStringCommand(" UPDATE Hishop_Orders SET FinishDate = getdate(), OrderStatus = 5,CloseReason='订单自动完成' WHERE OrderStatus=3 AND ShippingDate <= @ShippingDate AND OrderId=@OrderId");
					database.AddInParameter(sqlStringCommand3, "ShippingDate", System.Data.DbType.DateTime, DateTime.Now.AddDays((double)(-(double)masterSettings.FinishOrderDays)));
					database.AddInParameter(sqlStringCommand3, "OrderId", System.Data.DbType.String, orderInfo.OrderId);
					int num = database.ExecuteNonQuery(sqlStringCommand3);
					if (num > 0)
					{
						orderInfo.OrderStatus = OrderStatus.Finished;
						DistributorsBrower.UpdateCalculationCommission(orderInfo);
						foreach (LineItemInfo current2 in orderInfo.LineItems.Values)
						{
							if (current2.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
							{
								System.Data.Common.DbCommand sqlStringCommand4 = database.GetSqlStringCommand("delete from Hishop_OrderReturns where orderid=@orderid and HandleStatus<>2 and HandleStatus<>8;update Hishop_OrderItems set OrderItemsStatus=@OrderItemsStatus where orderid=@orderid and skuid=@skuid");
								database.AddInParameter(sqlStringCommand4, "OrderItemsStatus", System.Data.DbType.Int32, 5);
								database.AddInParameter(sqlStringCommand4, "skuid", System.Data.DbType.String, current2.SkuId);
								database.AddInParameter(sqlStringCommand4, "orderid", System.Data.DbType.String, orderInfo.OrderId);
								database.ExecuteNonQuery(sqlStringCommand4);
							}
						}
					}
				}
			}
		}
	}
}
