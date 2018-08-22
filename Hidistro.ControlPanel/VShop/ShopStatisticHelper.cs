using Hidistro.Core.Entities;
using Hidistro.Entities.StatisticsReport;
using Hidistro.SqlDal.VShop;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.ControlPanel.VShop
{
	public static class ShopStatisticHelper
	{
		public static System.Data.DataRow ShopGlobal_GetOrderCountByDate(DateTime dDate)
		{
			return new ShopStatisticDao().ShopGlobal_GetOrderCountByDate(dDate);
		}

		public static System.Data.DataRow ShopGlobal_GetMemberCount()
		{
			return new ShopStatisticDao().ShopGlobal_GetMemberCount();
		}

		public static System.Data.DataTable ShopGlobal_GetTrendDataList(DateTime BeginDate, int Days)
		{
			return new ShopStatisticDao().ShopGlobal_GetTrendDataList(BeginDate, Days);
		}

		public static System.Data.DataTable ShopGlobal_GetSortList_Member(DateTime BeginDate, int TopCount)
		{
			return new ShopStatisticDao().ShopGlobal_GetSortList_Member(BeginDate, TopCount);
		}

		public static System.Data.DataTable ShopGlobal_GetSortList_Distributor(DateTime BeginDate, int TopCount)
		{
			return new ShopStatisticDao().ShopGlobal_GetSortList_Distributor(BeginDate, TopCount);
		}

		public static System.Data.DataTable MemberGlobal_GetStatisticList(int FuncID)
		{
			return new ShopStatisticDao().MemberGlobal_GetStatisticList(FuncID);
		}

		public static System.Data.DataRow MemberGlobal_GetCountInfo()
		{
			return new ShopStatisticDao().MemberGlobal_GetCountInfo();
		}

		public static System.Data.DataRow Distributor_GetGlobal(DateTime RecDate)
		{
			return new ShopStatisticDao().Distributor_GetGlobal(RecDate);
		}

		public static System.Data.DataRow Distributor_GetGlobalTotal(DateTime dYesterday)
		{
			return new ShopStatisticDao().Distributor_GetGlobalTotal(dYesterday);
		}

		public static System.Data.DataTable GetTrendDataList_FX(DateTime BeginDate, int Days)
		{
			return new ShopStatisticDao().GetTrendDataList_FX(BeginDate, Days);
		}

		public static System.Data.DataRow GetOrder_Member_CountInfo(DateTime BeginDate, DateTime EndDate)
		{
			return new ShopStatisticDao().GetOrder_Member_CountInfo(BeginDate, EndDate);
		}

		public static System.Data.DataTable GetSaleReport(DateTime BeginDate, DateTime EndDate)
		{
			return new ShopStatisticDao().GetSaleReport(BeginDate, EndDate);
		}

		public static DbQueryResult GetOrderStatisticReport(OrderStatisticsQuery Query)
		{
			return new ShopStatisticDao().GetOrderStatisticReport(Query);
		}

		public static System.Data.DataRow GetOrderStatisticReportGlobalByAgentID(OrderStatisticsQuery_UnderShop Query)
		{
			return new ShopStatisticDao().GetOrderStatisticReportGlobalByAgentID(Query);
		}

		public static DbQueryResult GetOrderStatisticReport_UnderShop(OrderStatisticsQuery_UnderShop Query)
		{
			return new ShopStatisticDao().GetOrderStatisticReport_UnderShop(Query);
		}

		public static System.Data.DataSet GetOrder_Member_Rebuy(DateTime BeginDate, DateTime EndDate)
		{
			return new ShopStatisticDao().GetOrder_Member_Rebuy(BeginDate, EndDate);
		}

		public static System.Data.DataTable Product_GetStatisticReport_NoPage(OrderStatisticsQuery query, IList<string> fields)
		{
			return new ShopStatisticDao().Product_GetStatisticReport_NoPage(query, fields);
		}

		public static DbQueryResult Product_GetStatisticReport(OrderStatisticsQuery Query)
		{
			return new ShopStatisticDao().Product_GetStatisticReport(Query);
		}

		public static System.Data.DataTable Member_GetStatisticReport_NoPage(OrderStatisticsQuery Query, IList<string> fields)
		{
			return new ShopStatisticDao().Member_GetStatisticReport_NoPage(Query, fields);
		}

		public static DbQueryResult Member_GetStatisticReport(OrderStatisticsQuery Query)
		{
			return new ShopStatisticDao().Member_GetStatisticReport(Query);
		}

		public static DbQueryResult Member_GetRegionReport(OrderStatisticsQuery query)
		{
			return new ShopStatisticDao().Member_GetRegionReport(query);
		}

		public static System.Data.DataTable Member_GetInCreateReport(OrderStatisticsQuery query)
		{
			return new ShopStatisticDao().Member_GetInCreateReport(query);
		}

		private static bool AutoStatisticsOrders(out string RetInfo)
		{
			return new ShopStatisticDao().AutoStatisticsOrders(out RetInfo);
		}

		public static bool AutoStatisticsOrdersV2(string AppPath, out string RetInfo)
		{
			return new ShopStatisticDao().AutoStatisticsOrdersV2(AppPath, out RetInfo);
		}

		public static bool StatisticsOrdersByRecDate(DateTime RecDate, UpdateAction FuncAction, int IsUpdateLog, out string RetInfo)
		{
			return new ShopStatisticDao().StatisticsOrdersByRecDate(RecDate, FuncAction, IsUpdateLog, out RetInfo);
		}

		public static bool StatisticsOrdersByNotify(DateTime RecDate, UpdateAction FuncAction, string ActionDesc, out string RetInfo)
		{
			return new ShopStatisticDao().StatisticsOrdersByNotify(RecDate, FuncAction, ActionDesc, out RetInfo);
		}
	}
}
