using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.StatisticsReport;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class ShopStatisticDao
	{
		private Database database;

		public ShopStatisticDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public System.Data.DataRow ShopGlobal_GetOrderCountByDate(DateTime dDate)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                select \r\n                (select count(*) as OrderQty from  Hishop_Orders \r\n                where OrderStatus <> 4 and\r\n                (\r\n                 (CONVERT(varchar(10),  PayDate  , 120 ) = @RecDate1 and OrderStatus <> 1 and Gateway <> 'hishop.plugins.payment.podrequest')\r\n                  or\r\n                 (CONVERT(varchar(10),  OrderDate  , 120 ) = @RecDate1 and Gateway = 'hishop.plugins.payment.podrequest' )\r\n                )\r\n                 ) as OrderQty,\r\n                (\r\n                select \r\n                 SUM( (a.ItemAdjustedPrice) * a.Quantity - a.ReturnMoney- a.DiscountAverage-a.ItemAdjustedCommssion ) as  OrderAmountFee\r\n                 --a.*, b.OrderStatus\r\n                from Hishop_OrderItems a \r\n                INNER join Hishop_Orders b on a.OrderId=b.OrderId\r\n                where \r\n                 OrderItemsStatus<>4\r\n                    and OrderStatus<>4 and ((\r\n                     CONVERT(varchar(10),  PayDate  , 120 ) = @RecDate2 and Gateway <>'hishop.plugins.payment.podrequest' and OrderStatus<>1) \r\n                     or (CONVERT(varchar(10),  OrderDate  , 120 ) = @RecDate1 and Gateway ='hishop.plugins.payment.podrequest'))\r\n                )+\r\n                (select SUM(c.AdjustedFreight) as  OrderAmountFee\r\n                from Hishop_Orders c \r\n                where OrderStatus <> 4 and (CONVERT(varchar(10),  PayDate  , 120 ) = @RecDate1 and OrderStatus<>1 and Gateway <>'hishop.plugins.payment.podrequest')\r\n                or (CONVERT(varchar(10),  OrderDate  , 120 ) = @RecDate1 and Gateway ='hishop.plugins.payment.podrequest'  )\r\n               ) as OrderAmountFee");
			this.database.AddInParameter(sqlStringCommand, "RecDate1", System.Data.DbType.String, dDate.ToString("yyyy-MM-dd"));
			this.database.AddInParameter(sqlStringCommand, "RecDate2", System.Data.DbType.String, dDate.ToString("yyyy-MM-dd"));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dataTable.Rows[0];
		}

		public System.Data.DataRow ShopGlobal_GetMemberCount()
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n              select  \r\n                ( select count(*)   from Hishop_Orders where OrderStatus=2   or ( OrderStatus=1 and  Gateway='hishop.plugins.payment.podrequest' ) ) as 'WaitSendOrderQty',\r\n                ( select count(*)  from Hishop_Products  where SaleStatus=1) as GoodsQty ,\r\n                ( select    COUNT(*)  from aspnet_Members where Status=1 ) as  MemberQty,\r\n                ( select  COUNT(*) from aspnet_Distributors where  ReferralStatus<=1 ) as DistributorQty,\r\n                (\r\n                    select COUNT(*) from\r\n                    (\r\n                    select COUNT(*) SumRec from  Hishop_OrderItems where OrderItemsStatus>=6 and  OrderItemsStatus<=8\r\n                    group by OrderId\r\n                    ) T1\r\n                ) as ServiceOrderQty\r\n               \r\n                ");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dataTable.Rows[0];
		}

		public System.Data.DataTable ShopGlobal_GetTrendDataList(DateTime BeginDate, int Days)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			dataTable.Columns.Add(new System.Data.DataColumn("ID", typeof(int)));
			dataTable.Columns.Add(new System.Data.DataColumn("RecDate", typeof(DateTime)));
			dataTable.Columns.Add(new System.Data.DataColumn("OrderCount", typeof(int)));
			dataTable.Columns.Add(new System.Data.DataColumn("NewMemberCount", typeof(int)));
			dataTable.Columns.Add(new System.Data.DataColumn("NewDistributorCount", typeof(int)));
			for (int i = 0; i < Days; i++)
			{
				System.Data.DataRow dataRow = dataTable.NewRow();
				DateTime dateTime = BeginDate.AddDays((double)i);
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                    select\r\n                    (select count(*) from  Hishop_Orders where\r\n                    OrderStatus <> 4 and\r\n                    ((CONVERT(varchar(10),  PayDate  , 120 ) = @RecDate0 and OrderStatus<>1 and Gateway <>'hishop.plugins.payment.podrequest' )\r\n                    or \r\n                      (CONVERT(varchar(10),  OrderDate  , 120 ) = @RecDate0 and Gateway ='hishop.plugins.payment.podrequest')\r\n                    ))  as OrderCount,\r\n                    ( select    COUNT(*)  from aspnet_Members where Status=1 and CONVERT(varchar(10),  CreateDate, 120 )= CONVERT(varchar(10),  @RecDate1, 120 )  ) as MemberQty,\r\n                    ( select  COUNT(*) from aspnet_Distributors where ReferralStatus<=1 and  CONVERT(varchar(10),  CreateTime, 120 )= CONVERT(varchar(10),  @RecDate2, 120 ) ) as DistributorQty \r\n                    ");
				this.database.AddInParameter(sqlStringCommand, "RecDate0", System.Data.DbType.Date, dateTime);
				this.database.AddInParameter(sqlStringCommand, "RecDate1", System.Data.DbType.Date, dateTime);
				this.database.AddInParameter(sqlStringCommand, "RecDate2", System.Data.DbType.Date, dateTime);
				System.Data.DataTable dataTable2 = new System.Data.DataTable();
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				if (dataTable2.Rows.Count > 0)
				{
					dataRow["OrderCount"] = dataTable2.Rows[0]["OrderCount"];
					dataRow["NewMemberCount"] = dataTable2.Rows[0]["MemberQty"];
					dataRow["NewDistributorCount"] = dataTable2.Rows[0]["DistributorQty"];
				}
				else
				{
					dataRow["OrderCount"] = 0;
					dataRow["NewMemberCount"] = 0;
					dataRow["NewDistributorCount"] = 0;
				}
				dataRow["RecDate"] = dateTime;
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}

		public System.Data.DataTable ShopGlobal_GetSortList_Member(DateTime BeginDate, int TopCount)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n              select   top(@TopCount ) \r\n                T1.*,   T1.UserId as UserID, b.UserName, RANK() OVER  ( ORDER BY ValidOrderTotal desc) AS Rank,\r\n                T2.OrderQty\r\n                from \r\n                (                                     \r\n\t                select \r\n\t                  b.UserId,\r\n\t                  SUM( a.ItemAdjustedPrice * a.Quantity - a.ReturnMoney- a.DiscountAverage ) as  ValidOrderTotal \r\n\t                from Hishop_OrderItems a \r\n\t                left join Hishop_Orders b on a.OrderId=b.OrderId\r\n\t                where \r\n\t                1=1 \r\n\t                and UserId>0\r\n\t                and ( OrderItemsStatus<>1 and  OrderItemsStatus<>4 and OrderItemsStatus<>9 and OrderItemsStatus<>10 )\r\n\t                and ( OrderStatus<>1 and OrderStatus<>4 and OrderStatus<>9 and OrderStatus<>10 )\r\n\t                group by b.UserId\r\n                ) T1\r\n                left join \r\n                (\r\n\t                select UserId as UserId2, COUNT(*) as OrderQty from Hishop_Orders\r\n\t                where 1=1 \r\n\t\t                and UserId>0\r\n\t\t                and ( OrderStatus<>1 and OrderStatus<>4 and OrderStatus<>9 and OrderStatus<>10 )\r\n\t                group by  UserId\r\n                ) T2\t on T1.UserId= T2.UserId2\r\n                left join  aspnet_Members  b on T1.UserId= b.UserId\r\n                where b.Status<=1\r\n                order by T1.ValidOrderTotal desc ");
			this.database.AddInParameter(sqlStringCommand, "TopCount", System.Data.DbType.Int32, TopCount);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable ShopGlobal_GetSortList_Distributor(DateTime BeginDate, int TopCount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(" select ROW_NUMBER() OVER(ORDER BY CommTotalSum DESC) AS rownum, a.*,d.StoreName  from");
			stringBuilder.AppendLine(" (select top " + TopCount + " UserId,COUNT(DISTINCT case  userid when ReferralUserId then OrderId else null end) as Ordernums,");
			stringBuilder.AppendLine(" SUM(case  userid when ReferralUserId then OrderTotal else 0 end) as OrderTotalSum,SUM(CommTotal) as CommTotalSum");
			stringBuilder.AppendLine(" from vw_Hishop_CommissionWithBuyUserId  where 1=1 ");
			stringBuilder.AppendFormat(" and TradeTime>='{0}' ", BeginDate.ToString("yyyy-MM-dd") + " 00:00:00");
			stringBuilder.AppendLine(" group by UserId  order by CommTotalSum desc)  a");
			stringBuilder.AppendLine(" INNER JOIN aspnet_Members m ON a.UserId = m.UserId ");
			stringBuilder.AppendLine(" LEFT JOIN aspnet_Distributors d on a.UserId=d.UserId ");
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable MemberGlobal_GetStatisticList(int FuncID)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand;
			System.Data.DataTable result;
			if (FuncID == 1)
			{
				sqlStringCommand = this.database.GetSqlStringCommand("\r\n                    select isnull(T1.Total ,0) as Total, isnull(T2.Name,'其它') as Name\r\n                    from\r\n                    (\r\n                    select  COUNT(*) as Total, a.GradeId\r\n\t                    from aspnet_Members a\r\n\t                    where  1=1 and a.Status=1\r\n\t                    group by a.GradeId\r\n                    ) T1\r\n                    left join aspnet_MemberGrades T2 on T1.GradeId= T2.GradeId\r\n                    ");
			}
			else
			{
				if (FuncID != 2)
				{
					result = null;
					return result;
				}
				sqlStringCommand = this.database.GetSqlStringCommand("\r\n                select v.*, ISNULL( T1.Total,0) as Total\r\n                from VShop_Region v\r\n                left join\r\n                (\r\n                select  COUNT(*) as Total, a.TopRegionId\r\n\t                from aspnet_Members a\r\n\t                where  1=1 and a.Status=1\r\n\t                group by a.TopRegionId\r\n                ) T1 on v.RegionID = T1.TopRegionId\r\n                ");
			}
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			result = dataTable;
			return result;
		}

		public System.Data.DataRow MemberGlobal_GetCountInfo()
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_Statistics_Member");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dataTable.Rows[0];
		}

		public System.Data.DataRow Distributor_GetGlobal(DateTime dDate)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                select \r\n\t                (\r\n\t\t                select   sum(ValidOrderTotal) as ValidOrderTotal  \r\n\t\t                from vw_VShop_FinishOrder_Main \r\n\t\t                where \r\n                         (Gateway<>'hishop.plugins.payment.podrequest' and CONVERT( varchar(10), PayDate, 120) =  CONVERT( varchar(10), @RecDate, 120))\r\n                         or(Gateway='hishop.plugins.payment.podrequest'  and CONVERT( varchar(10), OrderDate, 120) =  CONVERT( varchar(10), @RecDate, 120))\r\n\t                ) as ValidOrderTotal,\r\n                * from\r\n                (\r\n\t                select   sum(ValidOrderTotal) as FXValidOrderTotal,   sum( SumCommission) as FXSumCommission  , COUNT(*) as FXOrderNumber\r\n\t                from vw_VShop_FinishOrder_Main \r\n\t                where  ReferralUserId>0 and (\r\n                   (Gateway<>'hishop.plugins.payment.podrequest' and CONVERT( varchar(10), PayDate, 120) =  CONVERT( varchar(10), @RecDate, 120))\r\n                         or(Gateway='hishop.plugins.payment.podrequest'  and CONVERT( varchar(10), OrderDate, 120) =  CONVERT( varchar(10), @RecDate, 120)))\r\n                ) T1\r\n                ");
			this.database.AddInParameter(sqlStringCommand, "RecDate", System.Data.DbType.String, dDate.ToString("yyyy-MM-dd"));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dataTable.Rows[0];
		}

		public System.Data.DataRow Distributor_GetGlobalTotal(DateTime dYesterday)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n\t            select \r\n\t            (select COUNT(*) from aspnet_Distributors where ReferralStatus<=1) as DistributorNumber,\r\n\t            (\r\n\t            select  COUNT(*)\r\n\t\t            from aspnet_Distributors\r\n\t\t            where ReferralStatus<=1 and  CONVERT(varchar(10), CreateTime , 120 ) =  CONVERT(varchar(10), @RecDate  , 120 ) \r\n\t            ) as NewAgentNumber,\t\r\n\t            (\r\n\t            SELECT  ISNULL(SUM(Amount),0) from Hishop_BalanceDrawRequest   where isnull(IsCheck,0)=2  \r\n\t            ) as FinishedDrawCommissionFee,\r\n\t\r\n\t            (\r\n\t            SELECT ISNULL( SUM(isnull(Amount,0)),0) from Hishop_BalanceDrawRequest   where isnull(IsCheck,0) in(0,1) \r\n\t            ) as WaitDrawCommissionFee\r\n                ");
			this.database.AddInParameter(sqlStringCommand, "RecDate", System.Data.DbType.String, dYesterday.ToString("yyyy-MM-dd"));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dataTable.Rows[0];
		}

		public System.Data.DataTable GetTrendDataList_FX(DateTime BeginDate, int Days)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			dataTable.Columns.Add(new System.Data.DataColumn("ID", typeof(int)));
			dataTable.Columns.Add(new System.Data.DataColumn("RecDate", typeof(DateTime)));
			dataTable.Columns.Add(new System.Data.DataColumn("NewAgentCount", typeof(decimal)));
			dataTable.Columns.Add(new System.Data.DataColumn("FXAmountFee", typeof(decimal)));
			dataTable.Columns.Add(new System.Data.DataColumn("FXCommisionFee", typeof(decimal)));
			for (int i = 0; i < Days; i++)
			{
				System.Data.DataRow dataRow = dataTable.NewRow();
				DateTime dateTime = BeginDate.AddDays((double)i);
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                    select\r\n                    (select  COUNT(*) from aspnet_Distributors where ReferralStatus<=1 and  CONVERT(varchar(10),  CreateTime , 120)= CONVERT(varchar(10),  @RecDate , 120 ) ) as NewAgentCount ,\r\n                    isnull(sum(ValidOrderTotal),0) as ValidOrderTotal, isnull(sum(SumCommission),0) as SumCommission \r\n                        from vw_VShop_FinishOrder_Main where  ReferralUserId>0 and\r\n                        ((Gateway <> 'hishop.plugins.payment.podrequest' and CONVERT(varchar(10),  PayDate, 120 )= CONVERT(varchar(10),  @RecDate ,120))\r\n                          or(Gateway = 'hishop.plugins.payment.podrequest' and CONVERT(varchar(10),  OrderDate, 120 )= CONVERT(varchar(10),  @RecDate ,120))\r\n                        )\r\n               \r\n                    ");
				this.database.AddInParameter(sqlStringCommand, "RecDate", System.Data.DbType.Date, dateTime);
				System.Data.DataTable dataTable2 = new System.Data.DataTable();
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				if (dataTable2.Rows.Count > 0)
				{
					dataRow["NewAgentCount"] = dataTable2.Rows[0]["NewAgentCount"];
					dataRow["FXAmountFee"] = dataTable2.Rows[0]["ValidOrderTotal"];
					dataRow["FXCommisionFee"] = dataTable2.Rows[0]["SumCommission"];
				}
				else
				{
					dataRow["NewAgentCount"] = 0;
					dataRow["FXAmountFee"] = 0;
					dataRow["FXCommisionFee"] = 0;
				}
				dataRow["RecDate"] = dateTime;
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}

		public System.Data.DataRow GetOrder_Member_CountInfo(DateTime BeginDate, DateTime EndDate)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                select a.*, b.* from \r\n                (\r\n                select 1 as RecNO1,sum(ValidOrderTotal) as SaleAmountFee,count(OrderId) as OrderNumber,count(distinct username) as BuyerNumber, \r\n               sum(SumCommission) as CommissionAmountFee\r\n               from vw_VShop_FinishOrder_Main\r\n               where\r\n               (CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120) and\r\n               CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                and Gateway<>'hishop.plugins.payment.podrequest'\r\n                )\r\n                or(\r\n                CONVERT( varchar(10), OrderDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120) and\r\n                CONVERT( varchar(10), OrderDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                and Gateway='hishop.plugins.payment.podrequest'\r\n                )\r\n\t            )  a    \r\n                left join\r\n                (\r\n                    select  \r\n                    1 as RecNO2, \r\n                     SUM(NewAgentNumber) as NewAgentNumber, SUM(NewMemberNumber) as NewMemberNumber ,\r\n                     sum(isnull(FXOrderNumber,0)) as FXOrderNumber, SUM(FXSaleAmountFee) as FXSaleAmountFee,\r\n                    --AVG(FXResultPercent) as FXResultPercent,\r\n                     SUM ( isnull(CommissionFee,0)) as FXCommissionFee\r\n                    from vshop_Statistics_Globals\r\n                    where 1=1\r\n                        and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                        and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), @EndDate, 120) \r\n                ) b on a.RecNO1=b.RecNO2\r\n                ");
			this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.Date, BeginDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.Date, EndDate);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			System.Data.DataRow result;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				result = dataTable.Rows[0];
			}
			else
			{
				result = null;
			}
			return result;
		}

		public System.Data.DataTable GetOrderCountInfo(DateTime BeginDate, DateTime EndDate)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n            select * from \r\n                (\r\n                   select 1 as RecNO,\r\n                                count(*) as OrderNumber,  sum(   ValidOrderTotal) as SaleAmountFee\r\n                                from vw_VShop_FinishOrder_Main\r\n                                where 1=1\r\n                                 and CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                                 and CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)                   \r\n                ) T1\r\n                left join\r\n                (\r\n                   select 1 as FXRecNO,\r\n                                count(*) as FXOrderNumber,  sum(   ValidOrderTotal) as FXSaleAmountFee\r\n                                from vw_VShop_FinishOrder_Main\r\n                                where  ReferralUserId>0\r\n                                 and CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                                 and CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)                   \r\n                ) T2 on T1.RecNO= T2.FXRecNO\r\n                ");
			this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.Date, BeginDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.Date, EndDate);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataSet GetOrder_Member_Rebuy(DateTime BeginDate, DateTime EndDate)
		{
			System.Data.DataSet dataSet = new System.Data.DataSet();
			System.Data.DataTable table = null;
			System.Data.DataTable table2 = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                            with\r\n               cr as (\r\n                select  distinct Userid from vw_VShop_FinishOrder_Main a\r\n                where \r\n               \r\n                (CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                and CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10),  @EndDate, 120)\r\n                and Gateway<>'hishop.plugins.payment.podrequest'\r\n                )\r\n                \r\n                or\r\n                (CONVERT( varchar(10), OrderDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                and CONVERT( varchar(10), OrderDate, 120) <=  CONVERT( varchar(10),  @EndDate, 120)\r\n                and Gateway='hishop.plugins.payment.podrequest'\r\n                )\r\n                ),\r\n                b1 as(\r\n                select Userid,  count(Userid) as c from vw_VShop_FinishOrder_Main a group by Userid\r\n                )\r\n              select( select count(b1.userid) as c from cr,b1 where cr.UserId=b1.UserId and c>1 )as OldBuy,\r\n              (select count(cr.userid) as c from cr) as totalBuy\r\n               ");
			this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.Date, BeginDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.Date, EndDate);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				table = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			sqlStringCommand = this.database.GetSqlStringCommand("\r\n                with cr as(select a.Userid, case Gateway when 'hishop.plugins.payment.podrequest' \r\n                then \r\n                CONVERT( varchar(10), OrderDate, 120)\r\n                else\r\n                 CONVERT( varchar(10), PayDate, 120)\r\n                end\r\n                as gpDate,\r\n                b.c\r\n                 from vw_VShop_FinishOrder_Main a,(select Userid,  count(Userid) as c from vw_VShop_FinishOrder_Main a group by Userid) b\r\n                where a.UserId=b.UserId\r\n                and(\r\n                (CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                and CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                and Gateway<>'hishop.plugins.payment.podrequest'\r\n                )\r\n                \r\n                or\r\n                (CONVERT( varchar(10), OrderDate, 120) >=  CONVERT( varchar(10),@BeginDate, 120)\r\n                and CONVERT( varchar(10), OrderDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                and Gateway='hishop.plugins.payment.podrequest'\r\n                )\r\n                )\r\n                )\r\n                select COUNT(distinct UserId) as TotalBuy, \r\n                COUNT(distinct case when c>1 then UserId else null end) as OldBuy,\r\n                gpDate from cr group by gpDate\r\n                ");
			this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.Date, BeginDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.Date, EndDate);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				table2 = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			dataSet.Tables.Add(table);
			dataSet.Tables.Add(table2);
			return dataSet;
		}

		public System.Data.DataTable GetSaleReport(DateTime BeginDate, DateTime EndDate)
		{
			System.Data.DataSet dataSet = new System.Data.DataSet();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                 select  * from vshop_Statistics_Globals\r\n                    where 1=1\r\n                     and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                     and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)       \r\n\r\n                ");
			this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.Date, BeginDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.Date, EndDate);
			System.Data.DataTable table = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				table = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			dataSet.Tables.Add(table);
			return dataSet.Tables[0];
		}

		public DbQueryResult GetOrderStatisticReport(OrderStatisticsQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!query.BeginDate.HasValue)
			{
				query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
			}
			if (!query.EndDate.HasValue)
			{
				query.EndDate = new DateTime?(DateTime.Today);
			}
			string table = string.Format("\r\n                    (\r\n                        select T1.*, b.RealName, b.CellPhone, b.UserName,  b.UserHead , b.StoreName\r\n                        from \r\n                        (\r\n\t                        select AgentId,\r\n\t                        sum(OrderNumber) as OrderNumber, sum(SaleAmountFee) as SaleAmountFee, sum(BuyerNumber) as BuyerNumber, \r\n\t                        AVG(BuyerAvgPrice) as BuyerAvgPrice , sum(CommissionAmountFee)  as CommissionAmountFee\r\n\t                        from dbo.vshop_Statistics_Distributors a\r\n\t                         where 1=1\r\n\t                         and AgentID>0\r\n\t                         and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), '{0}', 120)\r\n\t                         and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), '{1}', 120) \r\n\t                         group by \t AgentID  \r\n                        ) T1\r\n                        left join vw_Hishop_DistributorsMembers b on T1.AgentId= b.UserId \r\n                       -- where b.ReferralStatus<=1\r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "AgentId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		private System.Data.DataRow GetOrderStatisticReportGlobal_UnderShop_BAD(OrderStatisticsQuery_UnderShop query)
		{
			if (!query.BeginDate.HasValue)
			{
				query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
			}
			if (!query.EndDate.HasValue)
			{
				query.EndDate = new DateTime?(DateTime.Today);
			}
			System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.Common.DbCommand command = null;
			if (query.ShopLevel == 1)
			{
				command = this.database.GetSqlStringCommand("\r\n                            select  SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                            from dbo.vshop_Statistics_Distributors\r\n                            where AgentID >0\r\n\t                             and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n\t                             and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                                 and AgentId in\r\n                                 (\r\n                                select UserId \r\n                                from aspnet_Distributors\r\n                                where ReferralUserId= @AgentId   and UserId <> ReferralUserId \r\n                                 ) \r\n\r\n                     ");
			}
			else if (query.ShopLevel == 2)
			{
				command = this.database.GetSqlStringCommand("\r\n                            select  SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                            from dbo.vshop_Statistics_Distributors\r\n                            where AgentID >0\r\n\t                             and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10),  @BeginDate, 120)\r\n\t                             and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10),  @EndDate, 120)\r\n                                 and AgentId in\r\n                                 (\r\n                                    select UserId \r\n                                    from aspnet_Distributors\r\n                                    where ReferralUserId in\r\n                                    (\r\n                                    select UserId \r\n                                    from aspnet_Distributors\r\n                                    where ReferralUserId=  @AgentId and UserId <> ReferralUserId \r\n                                    )\r\n                                    and UserId <> ReferralUserId\r\n                                 ) \r\n                    ");
			}
			this.database.AddInParameter(command, "BeginDate", System.Data.DbType.Date, query.BeginDate.Value);
			this.database.AddInParameter(command, "EndDate", System.Data.DbType.Date, query.EndDate.Value);
			this.database.AddInParameter(command, "AgentId", System.Data.DbType.Int32, query.AgentId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(command))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			System.Data.DataRow result;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				result = dataTable.Rows[0];
			}
			else
			{
				result = null;
			}
			return result;
		}

		public System.Data.DataRow GetOrderStatisticReportGlobalByAgentID(OrderStatisticsQuery_UnderShop query)
		{
			if (!query.BeginDate.HasValue)
			{
				query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
			}
			if (!query.EndDate.HasValue)
			{
				query.EndDate = new DateTime?(DateTime.Today);
			}
			System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                        select  SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                        from dbo.vshop_Statistics_Distributors\r\n                        where AgentID >0\r\n\t                            and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n\t                            and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                                and AgentId = @AgentId \r\n                                \r\n\r\n                    ");
			this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.Date, query.BeginDate.Value);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.Date, query.EndDate.Value);
			this.database.AddInParameter(sqlStringCommand, "AgentId", System.Data.DbType.Int32, query.AgentId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			System.Data.DataRow result;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				result = dataTable.Rows[0];
			}
			else
			{
				result = null;
			}
			return result;
		}

		public DbQueryResult GetOrderStatisticReport_UnderShop(OrderStatisticsQuery_UnderShop query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!query.BeginDate.HasValue)
			{
				query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
			}
			if (!query.EndDate.HasValue)
			{
				query.EndDate = new DateTime?(DateTime.Today);
			}
			string table = "";
			if (query.ShopLevel == 1)
			{
				table = string.Format("\r\n                    (\r\n                        select T1.*, b.RealName, b.CellPhone, b.UserName,  b.UserHead , b.StoreName\r\n                        from \r\n                        (\r\n                            select  AgentID, SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                            from dbo.vshop_Statistics_Distributors\r\n                            where AgentID >0\r\n\t                             and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), '{0}', 120)\r\n\t                             and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), '{1}', 120)\r\n                                 and AgentId in\r\n                                 (\r\n                                select UserId \r\n                                from aspnet_Distributors\r\n                                where ReferralUserId= {2} and UserId <> ReferralUserId\r\n                                 ) \r\n                            group by AgentID\r\n                         ) T1\r\n                        left join vw_Hishop_DistributorsMembers b on T1.AgentId= b.UserId \r\n                    ) P  ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"), query.AgentId);
			}
			else if (query.ShopLevel == 2)
			{
				table = string.Format("\r\n                    (\r\n                        select T1.*, b.RealName, b.CellPhone, b.UserName,  b.UserHead , b.StoreName\r\n                        from \r\n                        (\r\n                            select  AgentID, SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                            from dbo.vshop_Statistics_Distributors\r\n                            where AgentID >0\r\n\t                             and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), '{0}', 120)\r\n\t                             and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), '{1}', 120)\r\n                                 and AgentId in\r\n                                 (\r\n                                    select UserId \r\n                                    from aspnet_Distributors\r\n                                    where ReferralUserId in\r\n                                    (\r\n                                    select UserId \r\n                                    from aspnet_Distributors\r\n                                    where ReferralUserId= {2} and UserId <> ReferralUserId\r\n                                    )\r\n                                    and UserId <> ReferralUserId\r\n                                 ) \r\n                            group by AgentID\r\n                         ) T1\r\n                        left join vw_Hishop_DistributorsMembers b on T1.AgentId= b.UserId \r\n                    ) P  ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"), query.AgentId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "AgentId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public System.Data.DataTable Product_GetStatisticReport_NoPage(OrderStatisticsQuery query, IList<string> fields)
		{
			System.Data.DataTable result;
			if (fields.Count == 0)
			{
				result = null;
			}
			else
			{
				string text = string.Empty;
				foreach (string current in fields)
				{
					text = text + current + ",";
				}
				text = text.Substring(0, text.Length - 1);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(" 1=1 ");
				if (!query.BeginDate.HasValue)
				{
					query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
				}
				if (!query.EndDate.HasValue)
				{
					query.EndDate = new DateTime?(DateTime.Today);
				}
				string str = string.Format("\r\n                    (\r\n                    select  ROW_NUMBER() over(order by a.SaleAmountFee desc) as RankIndex  , \r\n                    a.* , b.ProductName,b.ThumbnailUrl60\r\n                    from vshop_Statistics_Products a\r\n                    left join Hishop_Products b on a.ProductID  = b.ProductId\r\n                    where CONVERT(varchar(10), RecDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), RecDate, 120) <=CONVERT(varchar(10), '{1}', 120)  \r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
				System.Data.DataTable dataTable = new System.Data.DataTable();
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select " + text + " from " + str);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				result = dataTable;
			}
			return result;
		}

		public DbQueryResult Product_GetStatisticReport(OrderStatisticsQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!query.BeginDate.HasValue)
			{
				query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
			}
			if (!query.EndDate.HasValue)
			{
				query.EndDate = new DateTime?(DateTime.Today);
			}
			string table = string.Format("\r\n                    (\r\n                    select  ROW_NUMBER() over(order by a.SaleAmountFee desc) as RankIndex  , \r\n                    a.* , b.ProductName,b.ThumbnailUrl60\r\n                    from vshop_Statistics_Products a\r\n                    left join Hishop_Products b on a.ProductID  = b.ProductId\r\n                    where CONVERT(varchar(10), RecDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), RecDate, 120) <=CONVERT(varchar(10), '{1}', 120)  \r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "ProductId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public DbQueryResult Member_GetStatisticReport(OrderStatisticsQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!query.BeginDate.HasValue)
			{
				query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
			}
			if (!query.EndDate.HasValue)
			{
				query.EndDate = new DateTime?(DateTime.Today);
			}
			string table = string.Format("\r\n                    (\r\n\t                select T1.*, b.RealName, b.CellPhone, b.UserName, b.CreateDate, b.UserHead\r\n\t                from \r\n\t                (\r\n\t                select  UserId ,COUNT(*) as OrderNumber, SUM(ValidOrderTotal) as OrderTotal, \r\n\t                case \r\n\t\t                when  SUM(ValidOrderTotal)>0 then SUM(ValidOrderTotal) * 1.0 / COUNT(*) \r\n\t\t                else 0\r\n\t                end as AvgPrice\r\n\t                from  dbo.vw_VShop_FinishOrder_Main a\r\n                    where CONVERT(varchar(10), PayDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), PayDate, 120) <=CONVERT(varchar(10), '{1}', 120)  \r\n \t                group by UserId\r\n\t                ) T1\r\n\t                left join aspnet_Members b on T1.UserID= b.UserId \r\n                    where b.Status=1\r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public System.Data.DataTable Member_GetStatisticReport_NoPage(OrderStatisticsQuery query, IList<string> fields)
		{
			System.Data.DataTable result;
			if (fields.Count == 0)
			{
				result = null;
			}
			else
			{
				string text = string.Empty;
				foreach (string current in fields)
				{
					text = text + current + ",";
				}
				text = text.Substring(0, text.Length - 1);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(" 1=1 ");
				if (!query.BeginDate.HasValue)
				{
					query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
				}
				if (!query.EndDate.HasValue)
				{
					query.EndDate = new DateTime?(DateTime.Today);
				}
				string str = string.Format("\r\n                    (\r\n\t                select   ROW_NUMBER() over(order by OrderTotal desc) as RankIndex  , \r\n                        T1.*, b.RealName, b.CellPhone, b.UserName, b.CreateDate, b.UserHead\r\n\t                from \r\n\t                (\r\n\t                select  UserId ,COUNT(*) as OrderNumber, SUM(ValidOrderTotal) as OrderTotal, \r\n\t                case \r\n\t\t                when  SUM(ValidOrderTotal)>0 then SUM(ValidOrderTotal) * 1.0 / COUNT(*) \r\n\t\t                else 0\r\n\t                end as AvgPrice\r\n\t                from  dbo.vw_VShop_FinishOrder_Main a\r\n                    where CONVERT(varchar(10), PayDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), PayDate, 120) <=CONVERT(varchar(10), '{1}', 120)  \r\n \t                group by UserId\r\n\t                ) T1\r\n\t                left join aspnet_Members b on T1.UserID= b.UserId \r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
				System.Data.DataTable dataTable = new System.Data.DataTable();
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select " + text + " from " + str);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				result = dataTable;
			}
			return result;
		}

		public DbQueryResult Member_GetRegionReport(OrderStatisticsQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			string table = string.Format("\r\n                    (\r\n                         select  ROW_NUMBER() over(order by TotalRec desc) as RowIndex  ,  * \r\n                         from \r\n                         (\r\n\t                        select isnull(TotalRec,0) as TotalRec, RegionID_Group as RegionID, RegionName\r\n\t                        from VShop_Region X1\r\n\t                        left join \r\n\t                        (\r\n\t\t                        select sum( TotalRec) as TotalRec, RegionID_Group \r\n\t\t                        from\r\n\t\t                        (\r\n\t\t\t                        select a.*, isnull(b.RegionID,0) as RegionID_Group , b.RegionName\r\n\t\t\t                        from \r\n\t\t\t                        (\r\n\t\t\t                        select COUNT(*) TotalRec,  TopRegionId\r\n\t\t\t\t                        from aspnet_Members\r\n\t\t\t\t                        where Status=1\r\n\t\t\t\t                        group by TopRegionId\r\n\t\t\t                        ) a\r\n\t\t\t                        left join VShop_Region b on a.TopRegionId= b.RegionId\r\n\t\t                        )  T1\r\n\t\t                        group by T1.RegionID_Group\r\n\t                        ) X2 on X1.RegionId= X2.RegionID_Group\r\n                         ) Y1\r\n                    ) P  \r\n                    ", query.BeginDate, query.EndDate);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "RowIndex", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public System.Data.DataTable Member_GetInCreateReport(OrderStatisticsQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!query.BeginDate.HasValue)
			{
				query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
			}
			if (!query.EndDate.HasValue)
			{
				query.EndDate = new DateTime?(DateTime.Today);
			}
			string query2 = string.Format("\r\n                    select * from   vshop_Statistics_Globals\r\n                    where CONVERT(varchar(10), RecDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), RecDate, 120) <=CONVERT(varchar(10), '{1}', 120)   \r\n                    order by RecDate \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query2);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public bool AutoStatisticsOrders(out string RetInfo)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_vshop_Statistics_Auto");
			this.database.AddInParameter(storedProcCommand, "@RecDate", System.Data.DbType.Date, DateTime.Today);
			this.database.AddOutParameter(storedProcCommand, "@RetCode", System.Data.DbType.Int32, 0);
			this.database.AddOutParameter(storedProcCommand, "@RetInfo", System.Data.DbType.String, 250);
			bool result;
			try
			{
				this.database.ExecuteNonQuery(storedProcCommand);
				RetInfo = storedProcCommand.Parameters["@RetInfo"].Value.ToString();
				result = (storedProcCommand.Parameters["@RetCode"].Value.ToString() == "1");
			}
			catch (Exception ex)
			{
				RetInfo = ex.Message;
				result = false;
			}
			return result;
		}

		private bool IsFoundSuccessStatisticRec(DateTime dDate)
		{
			string commandText = "select top 1 RecDate from vshop_Statistics_Log  WITH (NOLOCK)  where IsSuccess=1 and RecDate='" + dDate.ToString("yyyy-MM-dd") + "' ";
			string value = Convert.ToString(this.database.ExecuteScalar(System.Data.CommandType.Text, commandText));
			return !string.IsNullOrEmpty(value);
		}

		public bool AutoStatisticsOrdersV2(string AppPath, out string RetInfo)
		{
			RetInfo = "";
			DateTime dateTime = DateTime.Today;
			string commandText = "select MIN( RecDate) as RecDate  from vshop_Statistics_Log where 1=1";
			string value = Convert.ToString(this.database.ExecuteScalar(System.Data.CommandType.Text, commandText));
			if (string.IsNullOrEmpty(value))
			{
				commandText = "select isnull(MIN( CreateDate), getdate()) as RecDate  from aspnet_Members where 1=1 ";
				dateTime = Convert.ToDateTime(this.database.ExecuteScalar(System.Data.CommandType.Text, commandText));
			}
			else
			{
				commandText = "select MIN(RecDate) as RecDate  from vshop_Statistics_Log where IsSuccess<>1 ";
				value = Convert.ToString(this.database.ExecuteScalar(System.Data.CommandType.Text, commandText));
				if (!string.IsNullOrEmpty(value))
				{
					dateTime = Convert.ToDateTime(this.database.ExecuteScalar(System.Data.CommandType.Text, commandText));
				}
				else
				{
					commandText = "select top 1  Max( RecDate) as RecDate  from vshop_Statistics_Log where 1=1 ";
					value = Convert.ToString(this.database.ExecuteScalar(System.Data.CommandType.Text, commandText));
					if (!string.IsNullOrEmpty(value))
					{
						dateTime = Convert.ToDateTime(this.database.ExecuteScalar(System.Data.CommandType.Text, commandText));
					}
				}
			}
			DateTime dateTime2 = dateTime;
			while (Convert.ToInt32(dateTime2.ToString("yyyyMMdd")) < Convert.ToInt32(DateTime.Today.ToString("yyyyMMdd")))
			{
				if (!this.IsFoundSuccessStatisticRec(dateTime2))
				{
					bool flag = this.StatisticsOrdersByRecDate(dateTime2, UpdateAction.AllUpdate, 1, out RetInfo);
				}
				Globals.Debuglog("WebApplication指定日期完毕。RecDate：" + dateTime2.ToString("yyyy-MM-dd") + "  结果：" + RetInfo, "_Tonji.txt");
				dateTime2 = dateTime2.AddDays(1.0);
			}
			return true;
		}

		public bool StatisticsOrdersByRecDate(DateTime RecDate, UpdateAction FuncAction, int IsUpdateLog, out string RetInfo)
		{
			bool result;
			try
			{
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_vshop_Statistics_Daily");
				this.database.AddInParameter(storedProcCommand, "@RecDate", System.Data.DbType.Date, RecDate);
				this.database.AddInParameter(storedProcCommand, "@FuncAction", System.Data.DbType.Int32, FuncAction);
				this.database.AddInParameter(storedProcCommand, "@IsUpdateLog", System.Data.DbType.Int32, IsUpdateLog);
				this.database.AddOutParameter(storedProcCommand, "@RetCode", System.Data.DbType.Int32, 0);
				this.database.AddOutParameter(storedProcCommand, "@RetInfo", System.Data.DbType.String, 250);
				this.database.ExecuteNonQuery(storedProcCommand);
				RetInfo = storedProcCommand.Parameters["@RetInfo"].Value.ToString();
				result = (storedProcCommand.Parameters["@RetCode"].Value.ToString() == "1");
			}
			catch (Exception ex)
			{
				RetInfo = ex.Message;
				result = false;
			}
			return result;
		}

		public bool StatisticsOrdersByNotify(DateTime RecDate, UpdateAction FuncAction, string ActionDesc, out string RetInfo)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_vshop_Statistics_Notify");
			this.database.AddInParameter(storedProcCommand, "@CalDate", System.Data.DbType.Date, RecDate);
			this.database.AddInParameter(storedProcCommand, "@FuncAction", System.Data.DbType.Int32, FuncAction);
			this.database.AddInParameter(storedProcCommand, "@ActionDesc", System.Data.DbType.String, ActionDesc);
			this.database.AddOutParameter(storedProcCommand, "@RetCode", System.Data.DbType.Int32, 0);
			this.database.AddOutParameter(storedProcCommand, "@RetInfo", System.Data.DbType.String, 250);
			bool result;
			try
			{
				this.database.ExecuteNonQuery(storedProcCommand);
				RetInfo = storedProcCommand.Parameters["@RetInfo"].Value.ToString();
				result = (storedProcCommand.Parameters["@RetCode"].Value.ToString() == "1");
			}
			catch (Exception ex)
			{
				RetInfo = ex.Message;
				result = false;
			}
			return result;
		}
	}
}
