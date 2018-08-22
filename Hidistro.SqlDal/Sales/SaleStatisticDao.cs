using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Sales
{
	public class SaleStatisticDao
	{
		private Database database;

		public SaleStatisticDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public System.Data.DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSales_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, productSale.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, productSale.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, productSale.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SaleStatisticDao.BuildProductSaleQuery(productSale));
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}

		public System.Data.DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSalesNoPage_Get");
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SaleStatisticDao.BuildProductSaleQuery(productSale));
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}

		public IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalRegionsUsers)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TopRegionId as RegionId,COUNT(UserId) as UserCounts,(select count(*) from aspnet_Members) as AllUserCounts FROM aspnet_Members  GROUP BY TopRegionId ");
			IList<UserStatisticsInfo> list = new List<UserStatisticsInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				UserStatisticsInfo userStatisticsInfo = null;
				while (dataReader.Read())
				{
					userStatisticsInfo = DataMapper.PopulateUserStatistics(dataReader);
					list.Add(userStatisticsInfo);
				}
				if (userStatisticsInfo != null)
				{
					totalRegionsUsers = int.Parse(userStatisticsInfo.AllUserCounts.ToString());
				}
				else
				{
					totalRegionsUsers = 0;
				}
			}
			return list;
		}

		public OrderStatisticsInfo GetUserOrders(OrderQuery userOrder)
		{
			OrderStatisticsInfo orderStatisticsInfo = new OrderStatisticsInfo();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_OrderStatistics_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, userOrder.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, userOrder.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, userOrder.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SaleStatisticDao.BuildUserOrderQuery(userOrder));
			this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", System.Data.DbType.Int32, 4);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				orderStatisticsInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["OrderTotal"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfPage += (decimal)dataReader["OrderTotal"];
					}
					if (dataReader["Profits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfPage += (decimal)dataReader["Profits"];
					}
				}
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["OrderTotal"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfSearch += (decimal)dataReader["OrderTotal"];
					}
					if (dataReader["Profits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfSearch += (decimal)dataReader["Profits"];
					}
				}
			}
			orderStatisticsInfo.TotalCount = (int)this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
			return orderStatisticsInfo;
		}

		public OrderStatisticsInfo GetUserOrdersNoPage(OrderQuery userOrder)
		{
			OrderStatisticsInfo orderStatisticsInfo = new OrderStatisticsInfo();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_OrderStatisticsNoPage_Get");
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SaleStatisticDao.BuildUserOrderQuery(userOrder));
			this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", System.Data.DbType.Int32, 4);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				orderStatisticsInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["OrderTotal"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfSearch += (decimal)dataReader["OrderTotal"];
					}
					if (dataReader["Profits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfSearch += (decimal)dataReader["Profits"];
					}
				}
			}
			orderStatisticsInfo.TotalCount = (int)this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
			return orderStatisticsInfo;
		}

		public System.Data.DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_MemberStatistics_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SaleStatisticDao.BuildMemberStatisticsQuery(query));
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}

		public System.Data.DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(SaleStatisticDao.BuildMemberStatisticsQuery(query));
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductVisitAndBuyStatistics_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SaleStatisticDao.BuildProductVisitAndBuyStatisticsQuery(query));
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}

		public System.Data.DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ProductName,VistiCounts,SaleCounts as BuyCount ,(SaleCounts/(case when VistiCounts=0 then 1 else VistiCounts end))*100 as BuyPercentage ");
			stringBuilder.Append("FROM Hishop_Products WHERE SaleCounts>0 ORDER BY BuyPercentage DESC;");
			stringBuilder.Append("SELECT COUNT(*) as TotalProductSales FROM Hishop_Products WHERE SaleCounts>0;");
			sqlStringCommand.CommandText = stringBuilder.ToString();
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					result = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					totalProductSales = (int)dataReader["TotalProductSales"];
				}
				else
				{
					totalProductSales = 0;
				}
			}
			return result;
		}

		public DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat("orderDate >= '{0}'", query.StartDate.Value);
			}
			if (query.EndDate.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("orderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" AND ");
			}
			stringBuilder.AppendFormat("OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_SaleDetails", "OrderId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_SaleDetails WHERE 1=1");
			if (query.StartDate.HasValue)
			{
				System.Data.Common.DbCommand expr_31 = sqlStringCommand;
				expr_31.CommandText += string.Format(" AND OrderDate >= '{0}'", query.StartDate);
			}
			if (query.EndDate.HasValue)
			{
				System.Data.Common.DbCommand expr_70 = sqlStringCommand;
				expr_70.CommandText += string.Format(" AND OrderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
			}
			System.Data.Common.DbCommand expr_B2 = sqlStringCommand;
			expr_B2.CommandText += string.Format("AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dbQueryResult;
		}

		public DbQueryResult GetSaleTargets()
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			string query = string.Empty;
			query = string.Format("select (select Count(OrderId) from Hishop_orders WHERE OrderStatus != {0} AND OrderStatus != {1}  AND OrderStatus != {2}) as OrderNumb,", 1, 4, 9) + string.Format("(select isnull(sum(OrderTotal),0) - isnull(sum(RefundAmount),0) from hishop_orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}) as OrderPrice, ", 1, 4, 9) + " (select COUNT(*) from aspnet_Members) as UserNumb,  (select count(*) from aspnet_Members where UserID in (select userid from Hishop_orders)) as UserOrderedNumb,  ISNULL((select sum(VistiCounts) from Hishop_products),0) as ProductVisitNumb ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dbQueryResult;
		}

		private static string BuildProductSaleQuery(SaleStatisticsQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ProductId, SUM(o.Quantity) AS ProductSaleCounts, SUM(o.ItemAdjustedPrice * o.Quantity) AS ProductSaleTotals,");
			stringBuilder.Append("  (SUM(o.ItemAdjustedPrice * o.Quantity) - SUM(o.CostPrice * o.ShipmentQuantity) )AS ProductProfitsTotals ");
			stringBuilder.AppendFormat(" FROM Hishop_OrderItems o  WHERE 0=0 ", new object[0]);
			stringBuilder.AppendFormat(" AND OrderId IN (SELECT  OrderId FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2})", 1, 4, 9);
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderDate >= '{0}')", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderDate <= '{0}')", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			stringBuilder.Append(" GROUP BY ProductId HAVING ProductId IN");
			stringBuilder.Append(" (SELECT ProductId FROM Hishop_Products)");
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}

		private static string BuildRegionsUserQuery(Pagination page)
		{
			if (null == page)
			{
				throw new ArgumentNullException("page");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT r.RegionId, r.RegionName, SUM(au.UserCount) AS Usercounts,");
			stringBuilder.Append(" (SELECT (SELECT SUM(COUNT) FROM aspnet_Members)) AS AllUserCounts ");
			stringBuilder.Append(" FROM vw_Allregion_Members au, Hishop_Regions r ");
			stringBuilder.Append(" WHERE (r.AreaId IS NOT NULL) AND ((au.path LIKE r.path + LTRIM(RTRIM(STR(r.RegionId))) + ',%') OR au.RegionId = r.RegionId)");
			stringBuilder.Append(" group by r.RegionId, r.RegionName ");
			stringBuilder.Append(" UNION SELECT 0, '0', sum(au.Usercount) AS Usercounts,");
			stringBuilder.Append(" (SELECT (SELECT count(*) FROM aspnet_Members)) AS AllUserCounts ");
			stringBuilder.Append(" FROM vw_Allregion_Members au, Hishop_Regions r  ");
			stringBuilder.Append(" WHERE au.regionid IS NULL OR au.regionid = 0 group by r.RegionId, r.RegionName");
			if (!string.IsNullOrEmpty(page.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(page.SortBy), page.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}

		private static string BuildUserOrderQuery(OrderQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
			string result;
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
				result = stringBuilder.ToString();
			}
			else
			{
				if (!string.IsNullOrEmpty(query.UserName))
				{
					stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
				}
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
				}
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND  OrderDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND  OrderDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				if (!string.IsNullOrEmpty(query.SortBy))
				{
					stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		private static string BuildOrdersQuery(OrderQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE 1 = 1 ", new object[0]);
			if (query.OrderId != string.Empty && query.OrderId != null)
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			else
			{
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
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
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
			}
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}

		private static string BuildMemberStatisticsQuery(SaleStatisticsQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT UserId, UserName ");
			if (query.StartDate.HasValue || query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(",  ( select isnull(SUM(OrderTotal),0) from Hishop_Orders where OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				stringBuilder.Append(" and userId = aspnet_Members.UserId) as SaleTotals");
				stringBuilder.AppendFormat(",(select Count(OrderId) from Hishop_Orders where OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				stringBuilder.Append(" and userId = aspnet_Members.UserId) as OrderCount ");
			}
			else
			{
				stringBuilder.Append(",ISNULL(Expenditure,0) as SaleTotals,ISNULL(OrderNumber,0) as OrderCount ");
			}
			stringBuilder.Append(" from aspnet_Members where Expenditure > 0");
			if (query.StartDate.HasValue || query.EndDate.HasValue)
			{
			}
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}

		private static string BuildProductVisitAndBuyStatisticsQuery(SaleStatisticsQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ProductId,(SaleCounts*100/(case when VistiCounts=0 then 1 else VistiCounts end)) as BuyPercentage");
			stringBuilder.Append(" FROM Hishop_products where SaleCounts>0");
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
