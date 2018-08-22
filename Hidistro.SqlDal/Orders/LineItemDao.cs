using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Orders
{
	public class LineItemDao
	{
		private Database database;

		public LineItemDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool AddOrderLineItems(string orderId, ICollection lineItems, System.Data.Common.DbTransaction dbTran)
		{
			bool result;
			if (lineItems == null || lineItems.Count == 0)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
				int num = 0;
				StringBuilder stringBuilder = new StringBuilder();
				foreach (LineItemInfo lineItemInfo in lineItems)
				{
					string text = num.ToString();
					stringBuilder.Append("INSERT INTO Hishop_OrderItems(OrderId, SkuId, ProductId, SKU, Quantity, ShipmentQuantity, CostPrice").Append(",ItemListPrice, ItemAdjustedPrice, ItemDescription, ThumbnailsUrl, Weight, SKUContent, PromotionId, PromotionName,OrderItemsStatus,ItemsCommission,SecondItemsCommission,ThirdItemsCommission,PointNumber,Type,DiscountAverage,LimitedTimeDiscountId) VALUES(@OrderId").Append(",@SkuId").Append(text).Append(",@ProductId").Append(text).Append(",@SKU").Append(text).Append(",@Quantity").Append(text).Append(",@ShipmentQuantity").Append(text).Append(",@CostPrice").Append(text).Append(",@ItemListPrice").Append(text).Append(",@ItemAdjustedPrice").Append(text).Append(",@ItemDescription").Append(text).Append(",@ThumbnailsUrl").Append(text).Append(",@Weight").Append(text).Append(",@SKUContent").Append(text).Append(",@PromotionId").Append(text).Append(",@PromotionName").Append(text).Append(",@OrderItemsStatus").Append(text).Append(",@ItemsCommission").Append(text).Append(",@SecondItemsCommission").Append(text).Append(",@ThirdItemsCommission").Append(text).Append(",@PointNumber").Append(text).Append(",@Type").Append(text).Append(",@DiscountAverage").Append(text).Append(",@LimitedTimeDiscountId").Append(text).Append(");");
					this.database.AddInParameter(sqlStringCommand, "SkuId" + text, System.Data.DbType.String, lineItemInfo.SkuId);
					this.database.AddInParameter(sqlStringCommand, "ProductId" + text, System.Data.DbType.Int32, lineItemInfo.ProductId);
					this.database.AddInParameter(sqlStringCommand, "SKU" + text, System.Data.DbType.String, lineItemInfo.SKU);
					this.database.AddInParameter(sqlStringCommand, "Quantity" + text, System.Data.DbType.Int32, lineItemInfo.Quantity);
					this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity" + text, System.Data.DbType.Int32, lineItemInfo.ShipmentQuantity);
					this.database.AddInParameter(sqlStringCommand, "CostPrice" + text, System.Data.DbType.Currency, lineItemInfo.ItemCostPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + text, System.Data.DbType.Currency, lineItemInfo.ItemListPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice" + text, System.Data.DbType.Currency, lineItemInfo.ItemAdjustedPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemDescription" + text, System.Data.DbType.String, lineItemInfo.ItemDescription);
					this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + text, System.Data.DbType.String, lineItemInfo.ThumbnailsUrl);
					this.database.AddInParameter(sqlStringCommand, "Weight" + text, System.Data.DbType.Int32, lineItemInfo.ItemWeight);
					this.database.AddInParameter(sqlStringCommand, "SKUContent" + text, System.Data.DbType.String, lineItemInfo.SKUContent);
					this.database.AddInParameter(sqlStringCommand, "PromotionId" + text, System.Data.DbType.Int32, lineItemInfo.PromotionId);
					this.database.AddInParameter(sqlStringCommand, "PromotionName" + text, System.Data.DbType.String, lineItemInfo.PromotionName);
					this.database.AddInParameter(sqlStringCommand, "OrderItemsStatus" + text, System.Data.DbType.Int32, (int)lineItemInfo.OrderItemsStatus);
					this.database.AddInParameter(sqlStringCommand, "ItemsCommission" + text, System.Data.DbType.Decimal, lineItemInfo.ItemsCommission);
					this.database.AddInParameter(sqlStringCommand, "SecondItemsCommission" + text, System.Data.DbType.Decimal, lineItemInfo.SecondItemsCommission);
					this.database.AddInParameter(sqlStringCommand, "ThirdItemsCommission" + text, System.Data.DbType.Decimal, lineItemInfo.ThirdItemsCommission);
					this.database.AddInParameter(sqlStringCommand, "PointNumber" + text, System.Data.DbType.Int32, lineItemInfo.PointNumber);
					this.database.AddInParameter(sqlStringCommand, "Type" + text, System.Data.DbType.Int32, lineItemInfo.Type);
					this.database.AddInParameter(sqlStringCommand, "DiscountAverage" + text, System.Data.DbType.Decimal, lineItemInfo.DiscountAverage);
					this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountId" + text, System.Data.DbType.Decimal, lineItemInfo.LimitedTimeDiscountId);
					num++;
					if (num == 50)
					{
						sqlStringCommand.CommandText = stringBuilder.ToString();
						int num2;
						if (dbTran != null)
						{
							num2 = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
						}
						else
						{
							num2 = this.database.ExecuteNonQuery(sqlStringCommand);
						}
						if (num2 <= 0)
						{
							result = false;
							return result;
						}
						stringBuilder.Remove(0, stringBuilder.Length);
						sqlStringCommand.Parameters.Clear();
						this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
						num = 0;
					}
				}
				if (stringBuilder.ToString().Length > 0)
				{
					sqlStringCommand.CommandText = stringBuilder.ToString();
					if (dbTran != null)
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
					}
					else
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
					}
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		public bool DeleteLineItem(string skuId, string orderId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_OrderItems WHERE OrderId=@OrderId AND SkuId=@SkuId ");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}

		public bool UpdateCommissionItem(int id, decimal itemsCommission, decimal secondItemsCommission, decimal thirdItemsCommission, System.Data.Common.DbTransaction dbTran = null)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_OrderItems Set ItemsCommission=@ItemsCommission,SecondItemsCommission=@SecondItemsCommission,ThirdItemsCommission=@ThirdItemsCommission,IsAdminModify=1 Where ID=@ID");
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, id);
			this.database.AddInParameter(sqlStringCommand, "ItemsCommission", System.Data.DbType.Currency, itemsCommission);
			this.database.AddInParameter(sqlStringCommand, "SecondItemsCommission", System.Data.DbType.Currency, secondItemsCommission);
			this.database.AddInParameter(sqlStringCommand, "ThirdItemsCommission", System.Data.DbType.Currency, thirdItemsCommission);
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

		public bool UpdateLineItemOrderID(string itemIDList, string orderid, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_OrderItems SET OrderId=@OrderId WHERE Id in(" + itemIDList + ")");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderid);
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

		public bool UpdateLineItem(string orderId, LineItemInfo lineItem, System.Data.Common.DbTransaction dbTran)
		{
			string text = string.Empty;
			if (!lineItem.IsAdminModify)
			{
				text = "IsAdminModify=0,";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[]
			{
				"UPDATE Hishop_OrderItems SET ",
				text,
				"ShipmentQuantity=@ShipmentQuantity,ItemAdjustedPrice=@ItemAdjustedPrice,ItemAdjustedCommssion=@ItemAdjustedCommssion,OrderItemsStatus=@OrderItemsStatus,ItemsCommission=@ItemsCommission,Quantity=@Quantity, PromotionId = NULL, PromotionName = NULL WHERE OrderId=@OrderId AND SkuId=@SkuId And ID=",
				lineItem.ID
			}));
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, lineItem.SkuId);
			this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity", System.Data.DbType.Int32, lineItem.ShipmentQuantity);
			this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice", System.Data.DbType.Currency, lineItem.ItemAdjustedPrice);
			this.database.AddInParameter(sqlStringCommand, "Quantity", System.Data.DbType.Int32, lineItem.Quantity);
			this.database.AddInParameter(sqlStringCommand, "ItemAdjustedCommssion", System.Data.DbType.Currency, lineItem.ItemAdjustedCommssion);
			this.database.AddInParameter(sqlStringCommand, "OrderItemsStatus", System.Data.DbType.Int16, (int)lineItem.OrderItemsStatus);
			this.database.AddInParameter(sqlStringCommand, "ItemsCommission", System.Data.DbType.Currency, lineItem.ItemsCommission);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}

		public LineItemInfo GetReturnMoneyByOrderIDAndProductID(string orderId, string skuid, int itemid)
		{
			string query = "select top 1 * from Hishop_OrderItems where OrderId=@OrderId and ID=@ID";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuid);
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, itemid);
			LineItemInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<LineItemInfo>(dataReader);
			}
			return result;
		}

		public LineItemInfo GetLineItemInfo(int id, string orderid)
		{
			string empty = string.Empty;
			if (!string.IsNullOrEmpty(orderid))
			{
			}
			string query = "select top 1 * from Hishop_OrderItems where Id=@Id ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, id);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderid);
			LineItemInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<LineItemInfo>(dataReader);
			}
			return result;
		}

		public int GetItemNumByOrderID(string orderid)
		{
			string query = "select count(0) from Hishop_OrderItems where OrderId=@OrderId ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderid);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
		}
	}
}
