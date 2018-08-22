using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.SqlDal.Orders;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal
{
	public class RefundDao
	{
		private Database database;

		public RefundDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public void GetRefundType(string orderId, out int refundType, out string remark)
		{
			refundType = 0;
			remark = "";
			string query = "select RefundType,RefundRemark from Hishop_OrderRefund where OrderId='" + orderId + "'";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					refundType = ((dataReader["RefundType"] != DBNull.Value) ? ((int)dataReader["RefundType"]) : 0);
					remark = (string)dataReader["RefundRemark"];
				}
			}
		}

		public RefundInfo GetByOrderIdAndProductID(string orderId, int productid, string skuid, int orderitemid)
		{
			RefundInfo result = null;
			string text = string.Empty;
			if (orderitemid > 0)
			{
				text = "select top 1 * from Hishop_OrderReturns where OrderId=@OrderId and OrderItemID=@OrderItemID";
			}
			else if (!string.IsNullOrEmpty(skuid))
			{
				text = "select top 1 * from Hishop_OrderReturns where OrderId=@OrderId and SkuID=@SkuID";
			}
			else if (productid > 0)
			{
				text = "select  top 1 * from Hishop_OrderReturns where OrderId=@OrderId and ProductId=" + productid;
			}
			if (!string.IsNullOrEmpty(text))
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
				this.database.AddInParameter(sqlStringCommand, "SkuID", System.Data.DbType.String, skuid);
				this.database.AddInParameter(sqlStringCommand, "OrderItemID", System.Data.DbType.String, orderitemid);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					result = ReaderConvert.ReaderToModel<RefundInfo>(dataReader);
				}
			}
			return result;
		}

		public RefundInfo GetOrderReturnsByReturnsID(int returnsid)
		{
			RefundInfo result = null;
			string query = "select  top 1 * from Hishop_OrderReturns where ReturnsId=" + returnsid;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<RefundInfo>(dataReader);
			}
			return result;
		}

		public bool UpdateRefundMoney(string orderid, int productid, decimal refundMoney)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_OrderReturns set RefundMoney=@RefundMoney where OrderId =@OrderId and ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productid);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderid);
			this.database.AddInParameter(sqlStringCommand, "RefundMoney", System.Data.DbType.Decimal, refundMoney);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void UpdateByOrderId(RefundInfo refundInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_OrderRefund set AdminRemark=@AdminRemark,ApplyForTime=@ApplyForTime,HandleStatus=@HandleStatus,HandleTime=@HandleTime,Operator=@Operator,RefundRemark=@RefundRemark where OrderId =@OrderId");
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, refundInfo.AdminRemark);
			this.database.AddInParameter(sqlStringCommand, "ApplyForTime", System.Data.DbType.String, refundInfo.ApplyForTime);
			this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, refundInfo.HandleStatus);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, refundInfo.HandleTime);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, refundInfo.Operator);
			this.database.AddInParameter(sqlStringCommand, "RefundRemark", System.Data.DbType.String, refundInfo.RefundRemark);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, refundInfo.OrderId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool UpdateByReturnsId(RefundInfo refundInfo)
		{
			string str = string.Empty;
			if (refundInfo.HandleStatus == RefundInfo.Handlestatus.Refunded)
			{
				str = ",ItemsCommission=0,SecondItemsCommission=0,ThirdItemsCommission=0,ReturnMoney=@RefundMoney ";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_OrderItems set IsHandled=1" + str + " where OrderId =@OrderId and SkuId=@SkuId and Type=0;update Hishop_OrderReturns set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,Operator=@Operator,RefundTime=@RefundTime,RefundMoney=@RefundMoney where ReturnsId =@ReturnsId");
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, refundInfo.AdminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, (int)refundInfo.HandleStatus);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, refundInfo.HandleTime);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, refundInfo.Operator);
			this.database.AddInParameter(sqlStringCommand, "RefundTime", System.Data.DbType.String, refundInfo.RefundTime);
			this.database.AddInParameter(sqlStringCommand, "RefundMoney", System.Data.DbType.Decimal, refundInfo.RefundMoney);
			this.database.AddInParameter(sqlStringCommand, "ReturnsId", System.Data.DbType.Int32, refundInfo.RefundId);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, refundInfo.OrderId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, refundInfo.ProductId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, refundInfo.SkuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateByAuditReturnsId(RefundInfo refundInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_OrderReturns set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,Operator=@Operator,AuditTime=@AuditTime,RefundMoney=@RefundMoney where ReturnsId =@ReturnsId");
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, refundInfo.AdminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, (int)refundInfo.HandleStatus);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, refundInfo.HandleTime);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, refundInfo.Operator);
			this.database.AddInParameter(sqlStringCommand, "AuditTime", System.Data.DbType.String, refundInfo.AuditTime);
			this.database.AddInParameter(sqlStringCommand, "RefundMoney", System.Data.DbType.Decimal, refundInfo.RefundMoney);
			this.database.AddInParameter(sqlStringCommand, "ReturnsId", System.Data.DbType.Int32, refundInfo.RefundId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool InsertOrderRefund(RefundInfo refundInfo)
		{
			bool result = false;
			LineItemDao lineItemDao = new LineItemDao();
			LineItemInfo returnMoneyByOrderIDAndProductID = lineItemDao.GetReturnMoneyByOrderIDAndProductID(refundInfo.OrderId, refundInfo.SkuId, refundInfo.OrderItemID);
			if (returnMoneyByOrderIDAndProductID != null)
			{
				decimal num = returnMoneyByOrderIDAndProductID.GetSubTotal() - returnMoneyByOrderIDAndProductID.DiscountAverage - returnMoneyByOrderIDAndProductID.ItemAdjustedCommssion;
				if (num < 0m)
				{
					num = 0m;
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into Hishop_OrderReturns(OrderId,ApplyForTime,Comments,HandleStatus,Account,RefundMoney,RefundType,ProductId,UserId,AuditTime,SkuId,OrderItemID) values(@OrderId,@ApplyForTime,@Comments,@HandleStatus,@Account,@RefundMoney,@RefundType,@ProductId,@UserId,@AuditTime,@SkuId,@OrderItemID)");
				this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, refundInfo.OrderId);
				this.database.AddInParameter(sqlStringCommand, "ApplyForTime", System.Data.DbType.DateTime, refundInfo.ApplyForTime);
				this.database.AddInParameter(sqlStringCommand, "Comments", System.Data.DbType.String, refundInfo.Comments);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, (int)refundInfo.HandleStatus);
				this.database.AddInParameter(sqlStringCommand, "Account", System.Data.DbType.String, refundInfo.Account);
				this.database.AddInParameter(sqlStringCommand, "RefundMoney", System.Data.DbType.Decimal, num);
				this.database.AddInParameter(sqlStringCommand, "RefundType", System.Data.DbType.Int32, refundInfo.RefundType);
				this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, refundInfo.ProductId);
				this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, refundInfo.UserId);
				this.database.AddInParameter(sqlStringCommand, "AuditTime", System.Data.DbType.String, refundInfo.AuditTime);
				this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, refundInfo.SkuId);
				this.database.AddInParameter(sqlStringCommand, "OrderItemID", System.Data.DbType.Int32, refundInfo.OrderItemID);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}

		public bool UpdateRefundOrderStock(string Stock, string SkuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new string[]
			{
				"Update Hishop_SKUs Set Stock = Stock + ",
				Stock,
				" where SkuId='",
				SkuId,
				"'"
			}));
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public bool UpdateOrderGoodStatu(string orderid, string skuid, int OrderItemsStatus, int itemid)
		{
			string arg = string.Empty;
			if (5 == OrderItemsStatus)
			{
				arg = "delete from Hishop_OrderReturns where orderid=@orderid and HandleStatus<>2 and HandleStatus<>8;";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(arg + "update Hishop_OrderItems set OrderItemsStatus=@OrderItemsStatus where orderid=@orderid and skuid=@skuid and id=" + itemid);
			this.database.AddInParameter(sqlStringCommand, "OrderItemsStatus", System.Data.DbType.Int32, OrderItemsStatus);
			this.database.AddInParameter(sqlStringCommand, "skuid", System.Data.DbType.String, skuid);
			this.database.AddInParameter(sqlStringCommand, "orderid", System.Data.DbType.String, orderid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public System.Data.DataTable GetOrderReturnTable(int userid, string ReturnsId, int type)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(ReturnsId))
			{
				text = " and ReturnsId=" + ReturnsId;
			}
			switch (type)
			{
			case 0:
				text += " and HandleStatus!=2 and HandleStatus!=8 ";
				break;
			case 1:
				text += " and (HandleStatus=2 or HandleStatus=8) ";
				break;
			}
			string query = string.Empty;
			query = "select Hishop_OrderReturns.*,isnull(((select ProductName from Hishop_Products where ProductId=Hishop_OrderReturns.ProductId)),'该商品已删除') as ProductName from Hishop_OrderReturns where UserId=@UserId " + text + " order by ApplyForTime desc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userid);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetOrderItemsReFundByOrderID(string orderid)
		{
			string query = "select a.*,b.id,b.ItemDescription,b.SKUContent,b.ThumbnailsUrl from Hishop_OrderReturns a left join Hishop_OrderItems b on a.OrderId=b.orderid and a.OrderItemID=b.id where a.OrderId =@OrderId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderid);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public bool GetReturnMes(int userid, string OrderId, int ProductId, int HandleStatus)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ReturnsId from Hishop_OrderReturns where OrderId=@OrderId and ProductId=@ProductId and UserId=@UserId and HandleStatus=@HandleStatus");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, OrderId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, ProductId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userid);
			this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, HandleStatus);
			System.Data.DataTable dataTable;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dataTable.Rows.Count > 0;
		}

		public bool GetReturnInfo(int userid, string OrderId, int ProductId, string SkuID)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ReturnsId from Hishop_OrderReturns where OrderId=@OrderId and ((SkuID=@SkuID and SkuID is not null) or( ProductId=@ProductId and SkuID is null)) and UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, OrderId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, ProductId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userid);
			this.database.AddInParameter(sqlStringCommand, "SkuID", System.Data.DbType.String, SkuID);
			System.Data.DataTable dataTable;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dataTable.Rows.Count > 0;
		}

		public DbQueryResult GetReturnOrderAll(ReturnsApplyQuery returnsapplyquery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" 1=1 ", new object[0]);
			if (!string.IsNullOrEmpty(returnsapplyquery.HandleStatus.ToString()))
			{
				if (returnsapplyquery.HandleStatus > 0)
				{
					stringBuilder.AppendFormat(" AND HandleStatus = {0}", returnsapplyquery.HandleStatus);
				}
				else if (returnsapplyquery.HandleStatus == -1)
				{
					stringBuilder.AppendFormat(" AND (HandleStatus=6 or HandleStatus=2 or HandleStatus=8 ) ", new object[0]);
				}
				else
				{
					stringBuilder.AppendFormat(" AND (HandleStatus=4 or HandleStatus=5 or HandleStatus=7 ) ", new object[0]);
				}
			}
			if (!string.IsNullOrEmpty(returnsapplyquery.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId LIKE '%{0}%'", DataHelper.CleanSearchString(returnsapplyquery.OrderId));
			}
			if (!string.IsNullOrEmpty(returnsapplyquery.ReturnsId))
			{
				stringBuilder.AppendFormat(" AND ReturnsId ={0}", DataHelper.CleanSearchString(returnsapplyquery.ReturnsId));
			}
			return DataHelper.PagingByRownumber(returnsapplyquery.PageIndex, returnsapplyquery.PageSize, returnsapplyquery.SortBy, returnsapplyquery.SortOrder, returnsapplyquery.IsCount, "Hishop_OrderReturns", "ReturnsId", stringBuilder.ToString(), "Hishop_OrderReturns.*,(select Username from aspnet_Members where userid=Hishop_OrderReturns.userid)as Username,(select Username from aspnet_Managers where userid=Hishop_OrderReturns.Operator)as OperatorName,(select ProductName from Hishop_Products where ProductId=Hishop_OrderReturns.ProductId)as ProductName");
		}

		public bool DelRefundApply(int ReturnsId)
		{
			string query = string.Format("DELETE FROM Hishop_OrderReturns WHERE ReturnsId={0} and (HandleStatus=2 or HandleStatus=5 or HandleStatus=7 or HandleStatus=8) ", ReturnsId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
