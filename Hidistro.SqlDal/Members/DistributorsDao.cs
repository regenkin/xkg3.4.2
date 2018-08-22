using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.FenXiao;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.SqlDal.Members
{
	public class DistributorsDao
	{
		private Database database;

		public DistributorsDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public string UpdateDistributorSuperior(int userid, int tosuperuserid)
		{
			string result = string.Empty;
			DistributorsInfo distributorInfo = this.GetDistributorInfo(tosuperuserid);
			if (distributorInfo != null)
			{
				string text = string.Empty;
				int referralUserId = distributorInfo.ReferralUserId;
				if (referralUserId == tosuperuserid)
				{
					text = tosuperuserid.ToString();
				}
				else
				{
					text = referralUserId.ToString() + "|" + tosuperuserid.ToString();
				}
				Database database = DatabaseFactory.CreateDatabase();
				using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
					StringBuilder stringBuilder = new StringBuilder();
					try
					{
						string query = string.Concat(new object[]
						{
							"update aspnet_Members set ReferralUserId=",
							tosuperuserid,
							" where userid=",
							userid
						});
						System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
						database.ExecuteNonQuery(sqlStringCommand, dbTransaction);
						string text2 = tosuperuserid + "|" + userid;
						query = string.Concat(new object[]
						{
							"update aspnet_Distributors set ReferralPath='",
							text2,
							"' where ReferralUserId=",
							userid
						});
						sqlStringCommand = database.GetSqlStringCommand(query);
						database.ExecuteNonQuery(sqlStringCommand, dbTransaction);
						query = string.Concat(new object[]
						{
							"update aspnet_Distributors set ReferralPath='",
							text,
							"',ReferralUserId=",
							tosuperuserid.ToString(),
							" where UserID=",
							userid
						});
						sqlStringCommand = database.GetSqlStringCommand(query);
						database.ExecuteNonQuery(sqlStringCommand, dbTransaction);
						dbTransaction.Commit();
						result = "1";
					}
					catch
					{
						result = "执行出错";
						dbTransaction.Rollback();
					}
					finally
					{
						dbConnection.Close();
					}
				}
			}
			return result;
		}

		public int GetDistributorSuperiorId(int userid)
		{
			string query = "select top 1 ReferralUserId from aspnet_Distributors where UserId=" + userid;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
		}

		public System.Data.DataTable GetDrawRequestNum(int[] CheckValues)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select IsCheck,COUNT(SerialID) as num from Hishop_BalanceDrawRequest where IsCheck in(" + string.Join<int>(",", CheckValues) + ")  group by IsCheck order by IsCheck");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public DistributorGradeInfo GetIsDefaultDistributorGradeInfo()
		{
			DistributorGradeInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_DistributorGrade where IsDefault=1 order by CommissionsLimit asc", new object[0]));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateDistributorGradeInfo(dataReader);
				}
			}
			return result;
		}

		public bool CreateDistributor(DistributorsInfo distributor)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members set CellPhone=@CellPhone where UserId=@UserId;INSERT INTO aspnet_Distributors(UserId,StoreName,Logo,BackImage,RequestAccount,GradeId,ReferralUserId,ReferralPath, ReferralOrders,ReferralBlance, ReferralRequestBalance,ReferralStatus,StoreDescription,DistributorGradeId) VALUES(@UserId,@StoreName,@Logo,@BackImage,@RequestAccount,@GradeId,@ReferralUserId,@ReferralPath,@ReferralOrders,@ReferralBlance, @ReferralRequestBalance, @ReferralStatus,@StoreDescription,@DistributorGradeId)");
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, distributor.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, distributor.UserId);
			this.database.AddInParameter(sqlStringCommand, "StoreName", System.Data.DbType.String, distributor.StoreName);
			this.database.AddInParameter(sqlStringCommand, "Logo", System.Data.DbType.String, distributor.Logo);
			this.database.AddInParameter(sqlStringCommand, "BackImage", System.Data.DbType.String, distributor.BackImage);
			this.database.AddInParameter(sqlStringCommand, "RequestAccount", System.Data.DbType.String, distributor.RequestAccount);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int64, (int)distributor.DistributorGradeId);
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int64, distributor.ParentUserId.Value);
			this.database.AddInParameter(sqlStringCommand, "ReferralPath", System.Data.DbType.String, distributor.ReferralPath);
			this.database.AddInParameter(sqlStringCommand, "ReferralOrders", System.Data.DbType.Int64, distributor.ReferralOrders);
			this.database.AddInParameter(sqlStringCommand, "ReferralBlance", System.Data.DbType.Decimal, distributor.ReferralBlance);
			this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", System.Data.DbType.Decimal, distributor.ReferralRequestBalance);
			this.database.AddInParameter(sqlStringCommand, "ReferralStatus", System.Data.DbType.Int64, distributor.ReferralStatus);
			this.database.AddInParameter(sqlStringCommand, "StoreDescription", System.Data.DbType.String, distributor.StoreDescription);
			this.database.AddInParameter(sqlStringCommand, "DistributorGradeId", System.Data.DbType.Int64, distributor.DistriGradeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int IsExiteDistributorsByStoreName(string storname)
		{
			string query = "SELECT UserId FROM aspnet_Distributors WHERE StoreName=@StoreName";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "StoreName", System.Data.DbType.String, DataHelper.CleanSearchString(storname));
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			return (obj != null) ? ((int)obj) : 0;
		}

		public bool UpdateDistributor(DistributorsInfo distributor)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET StoreName=@StoreName,Logo=@Logo,BackImage=@BackImage,RequestAccount=@RequestAccount,ReferralOrders=@ReferralOrders,ReferralBlance=@ReferralBlance, ReferralRequestBalance=@ReferralRequestBalance,StoreDescription=@StoreDescription,ReferralStatus=@ReferralStatus WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, distributor.UserId);
			this.database.AddInParameter(sqlStringCommand, "StoreName", System.Data.DbType.String, distributor.StoreName);
			this.database.AddInParameter(sqlStringCommand, "Logo", System.Data.DbType.String, distributor.Logo);
			this.database.AddInParameter(sqlStringCommand, "BackImage", System.Data.DbType.String, distributor.BackImage);
			this.database.AddInParameter(sqlStringCommand, "RequestAccount", System.Data.DbType.String, distributor.RequestAccount);
			this.database.AddInParameter(sqlStringCommand, "ReferralOrders", System.Data.DbType.Int64, distributor.ReferralOrders);
			this.database.AddInParameter(sqlStringCommand, "ReferralStatus", System.Data.DbType.Int64, distributor.ReferralStatus);
			this.database.AddInParameter(sqlStringCommand, "ReferralBlance", System.Data.DbType.Decimal, distributor.ReferralBlance);
			this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", System.Data.DbType.Decimal, distributor.ReferralRequestBalance);
			this.database.AddInParameter(sqlStringCommand, "StoreDescription", System.Data.DbType.String, distributor.StoreDescription);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateDistributorMessage(DistributorsInfo distributor)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members set CellPhone=@CellPhone where UserId=@UserId;UPDATE aspnet_Distributors SET StoreName=@StoreName,Logo=@Logo,StoreDescription=@StoreDescription,RequestAccount=@RequestAccount WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, distributor.UserId);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, distributor.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "StoreName", System.Data.DbType.String, distributor.StoreName);
			this.database.AddInParameter(sqlStringCommand, "Logo", System.Data.DbType.String, distributor.Logo);
			this.database.AddInParameter(sqlStringCommand, "StoreDescription", System.Data.DbType.String, distributor.StoreDescription);
			this.database.AddInParameter(sqlStringCommand, "RequestAccount", System.Data.DbType.String, distributor.RequestAccount);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateDistributorById(string requestAccount, int distributorId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET RequestAccount=@RequestAccount WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, distributorId);
			this.database.AddInParameter(sqlStringCommand, "RequestAccount", System.Data.DbType.String, requestAccount);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DistributorsInfo GetDistributorInfo(int distributorId)
		{
			DistributorsInfo result;
			if (distributorId <= 0)
			{
				result = null;
			}
			else
			{
				DistributorsInfo distributorsInfo = null;
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_Distributors where UserId={0} ", distributorId));
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						distributorsInfo = DataMapper.PopulateDistributorInfo(dataReader);
					}
				}
				result = distributorsInfo;
			}
			return result;
		}

		public DistributorsInfo GetDistributorInfoByStoreName(string storeName)
		{
			DistributorsInfo result;
			if (string.IsNullOrEmpty(storeName))
			{
				result = null;
			}
			else
			{
				DistributorsInfo distributorsInfo = null;
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_Distributors where storename='{0}'", storeName));
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						distributorsInfo = DataMapper.PopulateDistributorInfo(dataReader);
					}
				}
				result = distributorsInfo;
			}
			return result;
		}

		public int GetDistributorNum(DistributorGrade grade)
		{
			int result = 0;
			string text = string.Format("SELECT COUNT(*) FROM aspnet_Distributors where ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}'", Globals.GetCurrentMemberUserId());
			if (grade != DistributorGrade.All)
			{
				text = text + " AND GradeId=" + (int)grade;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = (int)dataReader[0];
					dataReader.Close();
				}
			}
			return result;
		}

		public int GetDownDistributorNum(string userid)
		{
			int result = 0;
			string query = string.Format("SELECT COUNT(*) FROM aspnet_Distributors where ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}'", userid);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = (int)dataReader[0];
					dataReader.Close();
				}
			}
			return result;
		}

		public int GetDownDistributorNumReferralOrders(string userid)
		{
			int result = 0;
			string query = string.Format("SELECT isnull(sum(ReferralOrders),0) FROM aspnet_Distributors where ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}'", userid);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			if (dataSet.Tables[0].Rows.Count > 0)
			{
				result = int.Parse(dataSet.Tables[0].Rows[0][0].ToString());
			}
			return result;
		}

		public System.Data.DataTable GetDistributorsNum()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Select (SELECT count(UserId) FROM vw_Hishop_DistributorsMembers where ReferralStatus=0) active,(SELECT count(UserId) FROM vw_Hishop_DistributorsMembers where ReferralStatus=1) frozen");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetDistributorSaleinfo(string startTime, string endTime, int[] UserIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("select UserId,StoreName,Logo,COUNT(DISTINCT case  userid when ReferralUserId then OrderId else null end) as Ordernums,COUNT(DISTINCT case  userid when ReferralUserId then BuyUserId else null end) as BuyUserIds,");
			stringBuilder.AppendLine(" SUM(case userid when ReferralUserId then OrderTotal else 0 end ) as OrderTotalSum,SUM(CommTotal) as CommTotalSum");
			stringBuilder.AppendLine(" from vw_Hishop_CommissionWithBuyUserId  where 1=1 ");
			stringBuilder.AppendLine(" and userid in(" + string.Join<int>(",", UserIds) + ") ");
			DateTime dateTime;
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				stringBuilder.AppendFormat(" and TradeTime>='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				stringBuilder.AppendFormat("  and  TradeTime<='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			stringBuilder.AppendLine(" group by UserId,StoreName,Logo ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public DbQueryResult GetCustomDistributorStatisticList()
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			string query = "select  * from dbo.Hishop_CustomDistributor order by commtotalsum desc ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable data = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				data = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			dbQueryResult.Data = data;
			return dbQueryResult;
		}

		public System.Data.DataTable GetCustomDistributorStatistic(int id)
		{
			string query = "select * from dbo.Hishop_CustomDistributor where  id = " + id;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetCustomDistributorStatistic(string storeName)
		{
			string text = "select id,storename from dbo.Hishop_CustomDistributor where  storename = '" + storeName + "'";
			text = text + "union all select UserId as id,storename from dbo.aspnet_Distributors where  storename = '" + storeName + "'";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public bool DeleteCustomDistributorStatistic(string id)
		{
			string query = "DELETE FROM Hishop_CustomDistributor WHERE id IN (" + id + ");";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateCustomDistributorStatistic(CustomDistributorStatistic custom)
		{
			string query = "update Hishop_CustomDistributor set storename=@storename,ordernums=@ordernums,commtotalsum=@commtotalsum,logo=@logo where id=@id";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "storename", System.Data.DbType.String, custom.StoreName);
			this.database.AddInParameter(sqlStringCommand, "ordernums", System.Data.DbType.Int32, custom.OrderNums);
			this.database.AddInParameter(sqlStringCommand, "commtotalsum", System.Data.DbType.Decimal, custom.CommTotalSum);
			this.database.AddInParameter(sqlStringCommand, "logo", System.Data.DbType.String, custom.Logo);
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, custom.id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool InsertCustomDistributorStatistic(CustomDistributorStatistic custom)
		{
			string query = "insert into Hishop_CustomDistributor(storename,ordernums,commtotalsum,logo) values(@storename,@ordernums,@commtotalsum,@logo)";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "storename", System.Data.DbType.String, custom.StoreName);
			this.database.AddInParameter(sqlStringCommand, "ordernums", System.Data.DbType.Int32, custom.OrderNums);
			this.database.AddInParameter(sqlStringCommand, "commtotalsum", System.Data.DbType.Decimal, custom.CommTotalSum);
			this.database.AddInParameter(sqlStringCommand, "logo", System.Data.DbType.String, custom.Logo);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DbQueryResult GetDistributorsRankings(string startTime, string endTime, int pgSize, int CurrPage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("with  cr as  ( ");
			stringBuilder.AppendLine("select top " + pgSize * (CurrPage - 1) + " UserId from vw_Hishop_CommissionWithBuyUserId  where 1=1 ");
			DateTime dateTime;
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				stringBuilder.AppendFormat(" and TradeTime>='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				stringBuilder.AppendFormat("  and  TradeTime<='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			stringBuilder.AppendLine(" group by UserId  order by SUM(case  userid when ReferralUserId then OrderTotal else 0 end) desc )");
			stringBuilder.AppendLine(" select a.*,d.StoreName ,d.Logo,d.BackImage ,d.RequestAccount ,d.AccountTime ,d.GradeId,d.ReferralUserId,d.ReferralPath,d.OrdersTotal,d.ReferralOrders,d.ReferralBlance,d.ReferralRequestBalance ,d.CreateTime,d.ReferralStatus ,d.StoreDescription ,d.DistributorGradeId,m.UserName,m.CreateDate,m.RealName,m.CellPhone,m.QQ,m.OpenId,m.MicroSignal,m.UserHead from");
			stringBuilder.AppendLine(" (select top " + pgSize + " UserId,COUNT(DISTINCT case  when userid=ReferralUserId and CommType=1  then OrderId else null end) as Ordernums,COUNT(DISTINCT case  userid when ReferralUserId then BuyUserId else null end) as BuyUserIds,");
			stringBuilder.AppendLine(" SUM(case  userid when ReferralUserId then OrderTotal else 0 end) as OrderTotalSum,SUM(CommTotal) as CommTotalSum");
			stringBuilder.AppendLine(" from vw_Hishop_CommissionWithBuyUserId  where 1=1 ");
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				stringBuilder.AppendFormat(" and TradeTime>='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				stringBuilder.AppendFormat("  and  TradeTime<='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			stringBuilder.AppendLine(" and UserId not in(select UserId from cr)");
			stringBuilder.AppendLine(" group by UserId  order by OrderTotalSum desc)  a");
			stringBuilder.AppendLine(" INNER JOIN aspnet_Members m ON a.UserId = m.UserId ");
			stringBuilder.AppendLine(" LEFT JOIN aspnet_Distributors d on a.UserId=d.UserId ");
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable data;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				data = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			string text = "select  count(DISTINCT (UserId)) from vw_Hishop_CommissionWithBuyUserId  where userid=ReferralUserId ";
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				text += string.Format(" and TradeTime>='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				text += string.Format("  and  TradeTime<='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(text);
			dbQueryResult.TotalRecords = int.Parse(this.database.ExecuteScalar(sqlStringCommand2).ToString());
			dbQueryResult.Data = data;
			return dbQueryResult;
		}

		public DbQueryResult GetDistributorsRankingsY(string startTime, string endTime, int pgSize, int CurrPage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("with  cr as  ( ");
			stringBuilder.AppendLine("select top " + pgSize * (CurrPage - 1) + " UserId from vw_Hishop_CommissionWithBuyUserId  where userid=ReferralUserId ");
			DateTime dateTime;
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				stringBuilder.AppendFormat(" and TradeTime>='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				stringBuilder.AppendFormat("  and  TradeTime<='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			stringBuilder.AppendLine(" group by UserId  order by SUM(OrderTotal) desc )");
			stringBuilder.AppendLine(" select a.*,d.StoreName ,d.Logo,d.BackImage ,d.RequestAccount ,d.AccountTime ,d.GradeId,d.ReferralUserId,d.ReferralPath,d.OrdersTotal,d.ReferralOrders,d.ReferralBlance,d.ReferralRequestBalance ,d.CreateTime,d.ReferralStatus ,d.StoreDescription ,d.DistributorGradeId,m.UserName,m.CreateDate,m.RealName,m.CellPhone,m.QQ,m.OpenId,m.MicroSignal,m.UserHead from");
			stringBuilder.AppendLine(" (select top " + pgSize + " UserId,COUNT(OrderId) as Ordernums,COUNT(DISTINCT BuyUserId) as BuyUserIds,");
			stringBuilder.AppendLine(" SUM(OrderTotal) as OrderTotalSum,SUM(CommTotal) as CommTotalSum");
			stringBuilder.AppendLine(" from vw_Hishop_CommissionWithBuyUserId  where userid=ReferralUserId ");
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				stringBuilder.AppendFormat(" and TradeTime>='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				stringBuilder.AppendFormat("  and  TradeTime<='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			stringBuilder.AppendLine(" and UserId not in(select UserId from cr)");
			stringBuilder.AppendLine(" group by UserId  order by SUM(OrderTotal) desc)  a");
			stringBuilder.AppendLine(" INNER JOIN aspnet_Members m ON a.UserId = m.UserId ");
			stringBuilder.AppendLine(" LEFT JOIN aspnet_Distributors d on a.UserId=d.UserId ");
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable data;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				data = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			string text = "select  count(DISTINCT (UserId)) from vw_Hishop_CommissionWithBuyUserId  where userid=ReferralUserId ";
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				text += string.Format(" and TradeTime>='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				text += string.Format("  and  TradeTime<='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(text);
			dbQueryResult.TotalRecords = int.Parse(this.database.ExecuteScalar(sqlStringCommand2).ToString());
			dbQueryResult.Data = data;
			return dbQueryResult;
		}

		public DbQueryResult GetSubDistributorsRankingsN(string startTime, string endTime, int pgSize, int CurrPage, int belongUserId, int grade)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("with  cr as  ( ");
			stringBuilder.AppendLine("select top " + pgSize * (CurrPage - 1) + "  UserId from vw_Hishop_CommissionRanking  where 1=1 and ");
			if (grade == 1)
			{
				stringBuilder.AppendFormat("( ReferralPath='{0}' or ReferralPath like '%|{0}' )", belongUserId);
			}
			else
			{
				stringBuilder.AppendFormat(" ReferralPath like '{0}|%'", belongUserId);
			}
			DateTime dateTime;
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				stringBuilder.AppendFormat(" and (TradeTime>='{0}' or TradeTime is null) ", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				stringBuilder.AppendFormat("  and  (TradeTime<='{0}' or  TradeTime is null) ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			stringBuilder.AppendLine(" group by UserId  order by SUM(case  userid when ReferralUserId then OrderTotal else 0 end) desc,UserId asc )");
			stringBuilder.AppendLine(" select a.*,d.StoreName ,d.Logo,d.BackImage ,d.RequestAccount ,d.AccountTime ,d.GradeId,d.ReferralUserId,d.ReferralPath,d.OrdersTotal,d.ReferralOrders,d.ReferralBlance,d.ReferralRequestBalance ,d.CreateTime,d.ReferralStatus ,d.StoreDescription ,d.DistributorGradeId,m.UserName,m.CreateDate,m.RealName,m.CellPhone,m.QQ,m.OpenId,m.MicroSignal,m.UserHead from");
			stringBuilder.AppendLine(" (select top " + pgSize + " UserId,COUNT(DISTINCT case  userid when ReferralUserId then OrderId else null end) as Ordernums,COUNT(DISTINCT case  userid when ReferralUserId then BuyUserId else null end) as BuyUserIds,");
			stringBuilder.AppendLine(" SUM(case  userid when ReferralUserId then OrderTotal else 0 end) as OrderTotalSum,SUM(CommTotal) as CommTotalSum");
			stringBuilder.AppendLine(" from vw_Hishop_CommissionRanking  where 1=1 ");
			if (grade == 1)
			{
				stringBuilder.AppendFormat(" and ( ReferralPath='{0}' or ReferralPath like '%|{0}') ", belongUserId);
			}
			else
			{
				stringBuilder.AppendFormat(" and ReferralPath like '{0}|%'", belongUserId);
			}
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				stringBuilder.AppendFormat(" and (TradeTime>='{0}' or  TradeTime is null) ", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				stringBuilder.AppendFormat("  and  (TradeTime<='{0}' or  TradeTime is null) ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			stringBuilder.AppendLine(" and UserId not in(select UserId from cr)");
			stringBuilder.AppendLine(" group by UserId  order by OrderTotalSum desc,UserId asc)  a");
			stringBuilder.AppendLine(" INNER JOIN aspnet_Members m ON a.UserId = m.UserId ");
			stringBuilder.AppendLine(" LEFT JOIN aspnet_Distributors d on a.UserId=d.UserId ");
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable data;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				data = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			string text = "select  count(DISTINCT (UserId)) from vw_Hishop_CommissionRanking  where userid=ReferralUserId ";
			if (grade == 1)
			{
				text += string.Format("  and  (ReferralPath='{0}' or ReferralPath like '%|{0}')", belongUserId);
			}
			else
			{
				text += string.Format("   and  ReferralPath like '{0}|%'", belongUserId);
			}
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				text += string.Format(" and (TradeTime>='{0}'  or TradeTime is null)", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				text += string.Format("  and  (TradeTime<='{0}' or  TradeTime is null) ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(text);
			dbQueryResult.TotalRecords = int.Parse(this.database.ExecuteScalar(sqlStringCommand2).ToString());
			dbQueryResult.Data = data;
			return dbQueryResult;
		}

		public DbQueryResult GetSubDistributorsContribute(string startTime, string endTime, int pgSize, int CurrPage, int belongUserId, int grade)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("with  cr as  ( ");
			stringBuilder.AppendFormat("select top " + pgSize * (CurrPage - 1) + " UserId from vw_Hishop_CommissionWithReferralPath  where (CUserId={0} or CUserId is null) and ", belongUserId);
			if (grade == 1)
			{
				stringBuilder.AppendFormat(" ( ReferralPath='{0}' or ReferralPath like '%|{0}' )", belongUserId);
			}
			else
			{
				stringBuilder.AppendFormat("  ReferralPath like '{0}|%'", belongUserId);
			}
			DateTime dateTime;
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				stringBuilder.AppendFormat(" and (TradeTime>='{0}' or TradeTime is null) ", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				stringBuilder.AppendFormat("  and  (TradeTime<='{0}' or  TradeTime is null) ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			stringBuilder.AppendLine(" group by UserId  order by SUM(OrderTotal) desc,UserId asc )");
			stringBuilder.AppendLine(" select a.*,d.StoreName ,d.Logo,d.BackImage ,d.RequestAccount ,d.AccountTime ,d.GradeId,d.ReferralUserId,d.ReferralPath,d.OrdersTotal,d.ReferralOrders,d.ReferralBlance,d.ReferralRequestBalance ,d.CreateTime,d.ReferralStatus ,d.StoreDescription ,d.DistributorGradeId,m.UserName,m.CreateDate,m.RealName,m.CellPhone,m.QQ,m.OpenId,m.MicroSignal,m.UserHead from");
			stringBuilder.AppendLine(" (select top " + pgSize + " UserId,COUNT(OrderId) as Ordernums,COUNT(DISTINCT BuyUserId) as BuyUserIds,");
			stringBuilder.AppendLine(" SUM(OrderTotal) as OrderTotalSum,SUM(CommTotal) as CommTotalSum");
			stringBuilder.AppendFormat(" from vw_Hishop_CommissionWithReferralPath  where (CUserId={0} or CUserId is null) and ", belongUserId);
			if (grade == 1)
			{
				stringBuilder.AppendFormat("  ( ReferralPath='{0}' or ReferralPath like '%|{0}') ", belongUserId);
			}
			else
			{
				stringBuilder.AppendFormat("  ReferralPath like '{0}|%'", belongUserId);
			}
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				stringBuilder.AppendFormat(" and (TradeTime>='{0}' or  TradeTime is null) ", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				stringBuilder.AppendFormat("  and  (TradeTime<='{0}' or  TradeTime is null) ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			stringBuilder.AppendLine(" and UserId not in(select UserId from cr)");
			stringBuilder.AppendLine(" group by UserId  order by SUM(OrderTotal) desc,UserId asc)  a");
			stringBuilder.AppendLine(" INNER JOIN aspnet_Members m ON a.UserId = m.UserId ");
			stringBuilder.AppendLine(" LEFT JOIN aspnet_Distributors d on a.UserId=d.UserId ");
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable data;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				data = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			string text = "select  count(DISTINCT (UserId)) from vw_Hishop_CommissionWithReferralPath  where   ";
			text += string.Format("  (CUserId={0} or CUserId is null) and ", belongUserId);
			if (grade == 1)
			{
				text += string.Format(" (ReferralPath='{0}' or ReferralPath like '%|{0}')", belongUserId);
			}
			else
			{
				text += string.Format("   ReferralPath like '{0}|%'", belongUserId);
			}
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				text += string.Format(" and (TradeTime>='{0}'  or TradeTime is null)", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				text += string.Format("  and  (TradeTime<='{0}' or  TradeTime is null) ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(text);
			dbQueryResult.TotalRecords = int.Parse(this.database.ExecuteScalar(sqlStringCommand2).ToString());
			dbQueryResult.Data = data;
			return dbQueryResult;
		}

		public System.Data.DataTable GetDistributorsSubStoreNum(int topUserId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Select (SELECT count(UserId) FROM vw_Hishop_DistributorsMembers where ReferralStatus!=9 and (ReferralPath='{0}' or ReferralPath like '%|{0}')) firstV,(SELECT count(UserId) FROM  vw_Hishop_DistributorsMembers where ReferralStatus!=9 and   ReferralPath like '{0}|%') secondV ,( select count(*) from aspnet_Members  s left join aspnet_Distributors d on s.userid=d.userid  where  s.Status=1 and s.ReferralUserId={0} and d.StoreName is null) memberCount", topUserId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public int GetDistributorsSubStoreNumN(int topUserId, int grade, string startTime, string endTime)
		{
			string text = "select  count(DISTINCT (UserId)) from vw_Hishop_CommissionWithBuyUserId  where userid=ReferralUserId ";
			if (grade == 1)
			{
				text += string.Format(" and ReferralPath='{0}' or ReferralPath like '%|{0}'", topUserId);
			}
			else
			{
				text += string.Format("  and ReferralPath like '{0}|%'", topUserId);
			}
			DateTime dateTime;
			if (startTime != null && DateTime.TryParse(startTime, out dateTime))
			{
				text += string.Format(" and TradeTime>='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (endTime != null && DateTime.TryParse(endTime, out dateTime))
			{
				text += string.Format("  and  TradeTime<='{0}' ", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
		}

		public DbQueryResult GetDistributors(DistributorsQuery query, string TopUserId = null, string level = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.GradeId > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("DistributorGradeId = {0}", query.GradeId);
			}
			if (query.UserId > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("UserId = {0}", query.UserId);
			}
			if (query.ReferralStatus > -1)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("ReferralStatus = '{0}'", query.ReferralStatus);
			}
			if (!string.IsNullOrEmpty(query.CellPhone))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("CellPhone='{0}'", DataHelper.CleanSearchString(query.CellPhone));
			}
			if (!string.IsNullOrEmpty(query.MicroSignal))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.MicroSignal));
			}
			if (!string.IsNullOrEmpty(query.RealName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
			}
			if (!string.IsNullOrEmpty(query.StoreName1))
			{
				DistributorsDao distributorsDao = new DistributorsDao();
				DistributorsInfo distributorInfoByStoreName = distributorsDao.GetDistributorInfoByStoreName(query.StoreName1);
				if (distributorInfoByStoreName != null)
				{
					int userId = distributorInfoByStoreName.UserId;
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendFormat(" AND ", new object[0]);
					}
					stringBuilder.AppendFormat("(ReferralPath='{0}' OR ReferralPath LIKE '%|{0}' )", DataHelper.CleanSearchString(userId.ToString()));
				}
			}
			else if (!string.IsNullOrEmpty(query.StoreName) && string.IsNullOrEmpty(query.StoreName1))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
			}
			if (!string.IsNullOrEmpty(query.ReferralPath))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("(ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}')", DataHelper.CleanSearchString(query.ReferralPath));
			}
			if (!string.IsNullOrEmpty(TopUserId) && !string.IsNullOrEmpty(level))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				if (level == "1")
				{
					stringBuilder.AppendFormat("(ReferralPath='{0}' OR ReferralPath LIKE '%|{0}' )", DataHelper.CleanSearchString(TopUserId));
				}
				else if (level == "2")
				{
					stringBuilder.AppendFormat(" ReferralPath like '{0}|%' ", DataHelper.CleanSearchString(TopUserId));
				}
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_DistributorsMembers", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public System.Data.DataTable SelectDistributors(DistributorsQuery query, string TopUserId = null, string level = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
			}
			if (!string.IsNullOrEmpty(query.MicroSignal))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" OR ");
				}
				stringBuilder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.MicroSignal));
			}
			string query2 = "Select * from vw_Hishop_DistributorsMembers Where " + stringBuilder.ToString();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query2);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			return dataSet.Tables[0];
		}

		public System.Data.DataTable GetAllDistributorsName(string keywords)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			string[] array = Regex.Split(DataHelper.CleanSearchString(keywords), "\\s+");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" StoreName LIKE '%{0}%' OR UserName LIKE '%{0}%'", DataHelper.CleanSearchString(DataHelper.CleanSearchString(array[0])));
			int num = 1;
			while (num < array.Length && num <= 5)
			{
				stringBuilder.AppendFormat(" OR StoreName LIKE '%{0}%' OR UserName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
				num++;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 10 StoreName,UserName from vw_Hishop_DistributorsMembers WHERE " + stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetDownDistributors(DistributorsQuery query, out int total)
		{
			StringBuilder stringBuilder = new StringBuilder(" ReferralStatus!=9 ");
			string text = "";
			if (query.GradeId > 0)
			{
				if (query.GradeId == 2)
				{
					stringBuilder.AppendFormat(" AND ( ReferralPath LIKE '%|{0}' OR ReferralPath='{0}')", DataHelper.CleanSearchString(query.ReferralPath));
				}
				if (query.GradeId == 3)
				{
					stringBuilder.AppendFormat(" AND ReferralPath LIKE '{0}|%' ", DataHelper.CleanSearchString(query.ReferralPath));
				}
			}
			int num = query.PageIndex * query.PageSize + 1;
			int num2 = num + query.PageSize - 1;
			if (query.UserId > 0)
			{
				text = " UserId=" + query.UserId + " AND ";
			}
			total = 0;
			string query2 = " SELECT count(*) FROM aspnet_Distributors where " + stringBuilder;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query2);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				total = (int)obj;
			}
			string query3 = string.Concat(new object[]
			{
				"select * from (select ROW_NUMBER()  OVER (ORDER BY UserId) as [RowIndex] , UserId,StoreName,GradeId,CreateTime,ReferralPath,Logo, (select StoreName from aspnet_Distributors where   UserId= s.ReferralUserId ) as LStoreName, (select count(*) from aspnet_Distributors where  ReferralUserId = s.UserId ) as disTotal, (select count(*) from aspnet_Members where ReferralUserId=s.UserId) as MemberTotal, (SELECT Name FROM aspnet_DistributorGrade WHERE GradeId = s.DistributorGradeId) AS GradeName, (select UserName from aspnet_Members where   UserId=s.UserId) as UserName, isnull((select SUM(OrderTotal) from Hishop_Commissions where ",
				text,
				" ReferralUserId=s.UserId),0) as OrderTotal, isnull((select SUM(CommTotal) from Hishop_Commissions where ",
				text,
				" ReferralUserId=s.UserId),0) as  CommTotal from aspnet_Distributors as s WHERE ",
				stringBuilder.ToString(),
				") as w where w.RowIndex BETWEEN ",
				num,
				" AND ",
				num2,
				" order by CommTotal  desc"
			});
			System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(query3);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand2);
			return dataSet.Tables[0];
		}

		public System.Data.DataTable GetThreeDistributors(DistributorsQuery query, out int total)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (query.GradeId > 0)
			{
				stringBuilder.AppendFormat(" AND ReferralUserId={0} ", query.UserId);
			}
			int num = query.PageIndex * query.PageSize + 1;
			int num2 = num + query.PageSize - 1;
			total = 0;
			string query2 = " SELECT count(*) FROM aspnet_Distributors where " + stringBuilder;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query2);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				total = (int)obj;
			}
			string query3 = string.Concat(new object[]
			{
				"select * from (select ROW_NUMBER()  OVER (ORDER BY UserId) as [RowIndex] , UserId,StoreName,GradeId,CreateTime,ReferralPath,Logo, (select StoreName from aspnet_Distributors where   UserId= s.ReferralUserId ) as LStoreName, (select count(*) from aspnet_Distributors where  ReferralUserId = s.UserId ) as disTotal, (select count(*) from aspnet_Members where ReferralUserId=s.UserId) as MemberTotal, (SELECT Name FROM aspnet_DistributorGrade WHERE GradeId = s.DistributorGradeId) AS GradeName, (select UserName from aspnet_Members where   UserId=s.UserId) as UserName, isnull((select SUM(OrderTotal) from Hishop_Commissions where  ReferralUserId=s.UserId),0) as OrderTotal, isnull((select SUM(CommTotal) from Hishop_Commissions where  ReferralUserId=s.UserId),0) as  CommTotal from aspnet_Distributors as s WHERE ",
				stringBuilder.ToString(),
				") as w where w.RowIndex BETWEEN ",
				num,
				" AND ",
				num2,
				" order by CommTotal  desc"
			});
			System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(query3);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand2);
			return dataSet.Tables[0];
		}

		public bool SelectDistributorsOpenId(ref List<string> SendToUserList)
		{
			string query = "Select  b.OpenId\r\n                from aspnet_Distributors a left join aspnet_Members  b  on a.UserId= b.UserId\r\n                Where a.ReferralStatus<=1 and (b.OpenId is not null and b.OpenId<>'' )";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataTable dataTable = dataSet.Tables[0];
			foreach (System.Data.DataRow dataRow in dataTable.Rows)
			{
				SendToUserList.Add(dataRow["OpenId"].ToString());
			}
			return true;
		}

		public bool SelectDistributorsAliOpenId(ref List<string> SendToUserList)
		{
			string query = "Select  b.AlipayOpenid\r\n                from aspnet_Distributors a left join aspnet_Members  b  on a.UserId= b.UserId\r\n                Where a.ReferralStatus<=1 and (b.AlipayOpenid is not null and b.AlipayOpenid<>'' )";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataTable dataTable = dataSet.Tables[0];
			foreach (System.Data.DataRow dataRow in dataTable.Rows)
			{
				SendToUserList.Add(dataRow["AlipayOpenid"].ToString());
			}
			return true;
		}

		public System.Data.DataTable GetDistributorsCommission(DistributorsQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1");
			string text = "";
			if (query.GradeId > 0)
			{
				stringBuilder.AppendFormat("AND GradeId = {0}", query.GradeId);
			}
			if (!string.IsNullOrEmpty(query.ReferralPath))
			{
				stringBuilder.AppendFormat(" AND (ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}')", DataHelper.CleanSearchString(query.ReferralPath));
			}
			if (query.UserId > 0)
			{
				text = " UserId=" + query.UserId + " AND ";
			}
			string query2 = string.Concat(new object[]
			{
				"select TOP ",
				query.PageSize,
				" UserId,StoreName,GradeId,CreateTime,isnull((select SUM(OrderTotal) from Hishop_Commissions where ",
				text,
				" ReferralUserId=aspnet_Distributors.UserId),0) as OrderTotal,isnull((select SUM(CommTotal) from Hishop_Commissions where ",
				text,
				" ReferralUserId=aspnet_Distributors.UserId),0) as  CommTotal from aspnet_Distributors WHERE ",
				stringBuilder.ToString(),
				" order by CreateTime  desc"
			});
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query2);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			return dataSet.Tables[0];
		}

		public decimal GetUserCommissions(int userid, DateTime fromdatetime, string enddatetime = null, string storeName = null, string OrderNum = null, string level = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" State=1 ");
			if (userid > 0)
			{
				stringBuilder.Append(" and UserID=" + userid);
			}
			bool flag = 1 == 0;
			stringBuilder.Append(" and TradeTime>='" + fromdatetime.ToString("yyyy-MM-dd") + " 00:00:00'");
			if (!string.IsNullOrEmpty(level))
			{
				if (level == "0")
				{
					stringBuilder.Append(" and UserID=ReferralUserId AND CommType not in (3,4,5)");
				}
				else if (level == "1")
				{
					stringBuilder.AppendFormat(" and (ReferralPath='{0}' or ReferralPath like '%|{0}') ", userid);
				}
				else if (level == "2")
				{
					stringBuilder.AppendFormat(" and ReferralPath like '{0}|%'", userid);
				}
			}
			if (!string.IsNullOrEmpty(storeName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("fromStoreName LIKE '%{0}%'", DataHelper.CleanSearchString(storeName));
			}
			if (!string.IsNullOrEmpty(OrderNum))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" OrderId = '{0}'", OrderNum);
			}
			DateTime dateTime;
			if (enddatetime != null && DateTime.TryParse(enddatetime, out dateTime))
			{
				stringBuilder.Append(" and TradeTime < '" + dateTime.AddDays(1.0).ToString("yyyy-MM-dd") + " 00:00:00'");
			}
			string query = " select isnull(sum(CommTotal),0) from vw_Hishop_CommissionDistributorsAddFromStore where " + stringBuilder.ToString();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			string s = this.database.ExecuteScalar(sqlStringCommand).ToString();
			return decimal.Parse(s);
		}

		public DbQueryResult GetCommissionsWithStoreName(CommissionsQuery query, string level = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" State=1 ");
			if (query.UserId > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("UserId = {0}", query.UserId);
			}
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("fromStoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
			}
			if (!string.IsNullOrEmpty(level))
			{
				if (level == "4")
				{
					stringBuilder.Append(" and UserID=ReferralUserId AND CommType in (3,4,5)");
				}
				else if (level == "5")
				{
					if (!string.IsNullOrEmpty(query.StoreName))
					{
						stringBuilder.Append(" and CommType not in (3,4,5) ");
					}
					else
					{
						stringBuilder.Append(" and UserID=ReferralUserId and CommType not in (3,4,5) ");
					}
				}
				else if (level == "0")
				{
					if (!string.IsNullOrEmpty(query.StoreName))
					{
						stringBuilder.Append(" and CommType<>3  and CommType<>5 ");
					}
					else
					{
						stringBuilder.Append(" and UserID=ReferralUserId  and CommType<>3  and CommType<>5 ");
					}
				}
				else if (level == "1")
				{
					stringBuilder.AppendFormat(" and (ReferralPath='{0}' or ReferralPath like '%|{0}') ", query.UserId);
				}
				else if (level == "2")
				{
					stringBuilder.AppendFormat(" and ReferralPath like '{0}|%'", query.UserId);
				}
				else if (level == "3")
				{
					stringBuilder.AppendFormat(" and (UserID<>ReferralUserId or CommType=3)", query.UserId);
				}
			}
			if (query.ReferralUserId > 0)
			{
				stringBuilder.AppendFormat(" and ReferralUserId = {0}", query.ReferralUserId);
			}
			if (!string.IsNullOrEmpty(query.OrderNum))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" OrderId = '{0}'", query.OrderNum);
			}
			if (!string.IsNullOrEmpty(query.StartTime.ToString()))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" datediff(dd,'{0}',TradeTime)>=0", query.StartTime);
			}
			if (!string.IsNullOrEmpty(query.EndTime.ToString()))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("  datediff(dd,'{0}',TradeTime)<=0", query.EndTime);
			}
			string table = "vw_Hishop_CommissionDistributorsAddFromStore";
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				table = "vw_Hishop_CommissionDistributorsOnlyForStoreName";
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "CommId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public DbQueryResult GetCommissions(CommissionsQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" State=1 ");
			if (query.UserId > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("UserId = {0}", query.UserId);
			}
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
			}
			if (!string.IsNullOrEmpty(query.OrderNum))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" OrderId = '{0}'", query.OrderNum);
			}
			if (!string.IsNullOrEmpty(query.StartTime.ToString()))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" datediff(dd,'{0}',TradeTime)>=0", query.StartTime);
			}
			if (!string.IsNullOrEmpty(query.EndTime.ToString()))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("  datediff(dd,'{0}',TradeTime)<=0", query.EndTime);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CommissionDistributors", "CommId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public bool IsExitsCommionsRequest(int userId)
		{
			bool result = false;
			string query = "SELECT * FROM dbo.Hishop_BalanceDrawRequest WHERE IsCheck in(0,1,3) AND UserId=@UserId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = true;
				}
			}
			return result;
		}

		public void RemoveDistributorProducts(List<int> productIds, int distributorId)
		{
			string str = string.Join<int>(",", productIds);
			string query = "DELETE FROM Hishop_DistributorProducts WHERE UserId=@UserId AND ProductId IN (" + str + ");";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, distributorId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void AddDistributorProducts(int productId, int distributorId)
		{
			string query = "INSERT INTO Hishop_DistributorProducts VALUES(@ProductId,@UserId)";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, distributorId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public BalanceDrawRequestInfo GetBalanceDrawRequestById(string SerialID)
		{
			BalanceDrawRequestInfo result;
			if (string.IsNullOrEmpty(SerialID))
			{
				result = null;
			}
			else
			{
				BalanceDrawRequestInfo balanceDrawRequestInfo = new BalanceDrawRequestInfo();
				string query = "select a.*, b.StoreName, c.OpenId as UserOpenId  from Hishop_BalanceDrawRequest  a  \r\n                     left join aspnet_Distributors b on a.UserId=b.UserId \r\n                     left join aspnet_Members c on a.UserId= c.UserId\r\n                  WHERE a.SerialID=@SerialID";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				this.database.AddInParameter(sqlStringCommand, "SerialID", System.Data.DbType.Int32, SerialID);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					balanceDrawRequestInfo = ReaderConvert.ReaderToModel<BalanceDrawRequestInfo>(dataReader);
				}
				result = balanceDrawRequestInfo;
			}
			return result;
		}

		public DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query, string[] extendCheckStatus = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
			}
			if (!string.IsNullOrEmpty(query.UserId))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" UserId = {0}", DataHelper.CleanSearchString(query.UserId));
			}
			if (!string.IsNullOrEmpty(query.RequestTime.ToString()))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" convert(varchar(10),RequestTime,120)='{0}'", query.RequestTime);
			}
			if (!string.IsNullOrEmpty(query.IsCheck.ToString()))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" IsCheck={0}", query.IsCheck);
			}
			if (extendCheckStatus != null)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.Append(" IsCheck in (" + string.Join(",", extendCheckStatus) + ") ");
			}
			if (!string.IsNullOrEmpty(query.CheckTime.ToString()) && query.CheckTime.ToString() != "CheckTime")
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" convert(varchar(10),CheckTime,120)='{0}'", query.CheckTime);
			}
			if (!string.IsNullOrEmpty(query.CheckTime.ToString()) && query.CheckTime.ToString() == "CheckTime")
			{
				if (!string.IsNullOrEmpty(query.RequestStartTime.ToString()))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" AND ");
					}
					stringBuilder.AppendFormat(" datediff(dd,'{0}',CheckTime)>=0", query.RequestStartTime);
				}
				if (!string.IsNullOrEmpty(query.RequestEndTime.ToString()))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" AND ");
					}
					stringBuilder.AppendFormat("  datediff(dd,'{0}',CheckTime)<=0", query.RequestEndTime);
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(query.RequestStartTime.ToString()))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" AND ");
					}
					stringBuilder.AppendFormat(" datediff(dd,'{0}',RequestTime)>=0", query.RequestStartTime);
				}
				if (!string.IsNullOrEmpty(query.RequestEndTime.ToString()))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" AND ");
					}
					stringBuilder.AppendFormat("  datediff(dd,'{0}',RequestTime)<=0", query.RequestEndTime);
				}
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BalanceDrawRequesDistributors ", "SerialID", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public bool AddBalanceDrawRequest(BalanceDrawRequestInfo bdrinfo)
		{
			string query = "INSERT INTO Hishop_BalanceDrawRequest(UserId,bankName,RequestType,UserName,Amount,AccountName,CellPhone,MerchantCode,Remark,RequestTime,IsCheck) VALUES(@UserId,@bankName,@RequestType,@UserName,@Amount,@AccountName,@CellPhone,@MerchantCode,@Remark,getdate(),0)";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, bdrinfo.UserId);
			this.database.AddInParameter(sqlStringCommand, "RequestType", System.Data.DbType.Int32, bdrinfo.RequesType);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, bdrinfo.UserName);
			this.database.AddInParameter(sqlStringCommand, "Amount", System.Data.DbType.Decimal, bdrinfo.Amount);
			this.database.AddInParameter(sqlStringCommand, "AccountName", System.Data.DbType.String, bdrinfo.AccountName);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, bdrinfo.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "MerchantCode", System.Data.DbType.String, bdrinfo.MerchantCode);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, bdrinfo.Remark);
			this.database.AddInParameter(sqlStringCommand, "bankName", System.Data.DbType.String, bdrinfo.BankName);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool GetBalanceDrawRequestIsCheck(int serialid)
		{
			string query = "select IsCheck from Hishop_BalanceDrawRequest where SerialID=" + serialid;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			string a = Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)).ToString();
			return a == "2";
		}

		public int GetBalanceDrawRequestIsCheckStatus(int serialid)
		{
			string query = "select IsCheck from Hishop_BalanceDrawRequest where SerialID=" + serialid;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			string s = Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)).ToString();
			return int.Parse(s);
		}

		public Dictionary<int, int> GetMulBalanceDrawRequestIsCheckStatus(int[] serialids)
		{
			string query = "select IsCheck,SerialID from Hishop_BalanceDrawRequest where SerialID in(" + string.Join<int>(",", serialids) + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable dataTable = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			if (dataTable.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					dictionary.Add((int)dataRow["SerialID"], (int)dataRow["IsCheck"]);
				}
			}
			return dictionary;
		}

		public bool SetBalanceDrawRequestIsCheckStatus(int[] serialid, int checkValue, string Remark = null, string Amount = null)
		{
			string text = "UPDATE Hishop_BalanceDrawRequest set IsCheck=@IsCheck ";
			if (Remark != null)
			{
				text += ",Remark=@Remark ";
			}
			if (Amount != null)
			{
				text += ",Amount=@Amount ";
			}
			text = text + " where SerialID in(" + string.Join<int>(",", serialid) + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "IsCheck", System.Data.DbType.Int16, checkValue);
			if (Remark != null)
			{
				this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, Remark);
			}
			if (Amount != null)
			{
				this.database.AddInParameter(sqlStringCommand, "Amount", System.Data.DbType.Decimal, Amount);
			}
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int IsExistUsers(string userIds)
		{
			string query = "SELECT count(*) FROM aspnet_Distributors WHERE UserId IN (" + userIds + ") AND ReferralStatus=0";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		public Dictionary<int, bool> GetExistDistributorList(string userIds)
		{
			string query = "SELECT Distinct(UserId) as UserId FROM aspnet_Distributors WHERE UserId IN (" + userIds + ")  ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			Dictionary<int, bool> result;
			if (dataSet == null || dataSet.Tables.Count <= 0)
			{
				result = dictionary;
			}
			else
			{
				System.Data.DataTable dataTable = dataSet.Tables[0];
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					dictionary.Add((int)dataRow[0], true);
				}
				result = dictionary;
			}
			return result;
		}

		public bool UpdateBalanceDrawRequest(int Id, string Remark, string CheckTime = null)
		{
			bool result;
			if (CheckTime == null)
			{
				CheckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			}
			else
			{
				DateTime now = DateTime.Now;
				if (!DateTime.TryParse(CheckTime, out now))
				{
					result = false;
					return result;
				}
				CheckTime = now.ToString("yyyy-MM-dd HH:mm:ss");
			}
			string query = "UPDATE Hishop_BalanceDrawRequest set Remark=@Remark,IsCheck=2,CheckTime=@CheckTime WHERE SerialID=@SerialID and IsCheck<>2 ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, Remark);
			this.database.AddInParameter(sqlStringCommand, "SerialID", System.Data.DbType.Int32, Id);
			this.database.AddInParameter(sqlStringCommand, "CheckTime", System.Data.DbType.String, CheckTime);
			result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			return result;
		}

		public string SendRedPackToBalanceDrawRequest(int serialid)
		{
			string result = string.Empty;
			string empty = string.Empty;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (!masterSettings.EnableWeiXinRequest)
			{
				result = "管理员后台未开启微信付款！";
			}
			else
			{
				string query = "select a.SerialID,a.userid,a.Amount,b.OpenID,isnull(b.OpenId,'') as OpenId from Hishop_BalanceDrawRequest a inner join aspnet_Members b on a.userid=b.userid where SerialID=@SerialID and IsCheck=1";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				this.database.AddInParameter(sqlStringCommand, "SerialID", System.Data.DbType.Int32, serialid);
				System.Data.DataTable dataTable = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
				string text = string.Empty;
				if (dataTable.Rows.Count > 0)
				{
					text = dataTable.Rows[0]["OpenId"].ToString();
					int userid = int.Parse(dataTable.Rows[0]["UserID"].ToString());
					decimal value = decimal.Parse(dataTable.Rows[0]["Amount"].ToString()) * 100m;
					int amount = Convert.ToInt32(value);
					if (string.IsNullOrEmpty(text))
					{
						result = "用户未绑定微信号";
					}
					else
					{
						query = "select top 1 ID from vshop_SendRedpackRecord where BalanceDrawRequestID=" + dataTable.Rows[0]["SerialID"].ToString();
						sqlStringCommand = this.database.GetSqlStringCommand(query);
						if (this.database.ExecuteDataSet(sqlStringCommand).Tables[0].Rows.Count > 0)
						{
							result = "-1";
						}
						else
						{
							result = (this.CreateSendRedpackRecord(serialid, userid, text, amount, "您的提现申请已成功", "恭喜您提现成功!") ? "1" : "提现操作失败");
						}
					}
				}
				else
				{
					result = "该用户没有提现申请,或者提现申请未审核";
				}
			}
			return result;
		}

		public bool CreateSendRedpackRecord(int serialid, int userid, string openid, int amount, string act_name, string wishing)
		{
			bool flag = true;
			int num = 20000;
			SendRedpackRecordInfo sendRedpackRecordInfo = new SendRedpackRecordInfo();
			sendRedpackRecordInfo.BalanceDrawRequestID = serialid;
			sendRedpackRecordInfo.UserID = userid;
			sendRedpackRecordInfo.OpenID = openid;
			sendRedpackRecordInfo.ActName = act_name;
			sendRedpackRecordInfo.Wishing = wishing;
			sendRedpackRecordInfo.ClientIP = Globals.IPAddress;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				SendRedpackRecordDao sendRedpackRecordDao = new SendRedpackRecordDao();
				try
				{
					if (amount <= num)
					{
						sendRedpackRecordInfo.Amount = amount;
						flag = sendRedpackRecordDao.AddSendRedpackRecord(sendRedpackRecordInfo, dbTransaction);
						flag = this.UpdateSendRedpackRecord(serialid, 1, dbTransaction);
					}
					else
					{
						int num2 = amount % num;
						int num3 = amount / num;
						if (num2 > 0)
						{
							sendRedpackRecordInfo.Amount = num2;
							flag = sendRedpackRecordDao.AddSendRedpackRecord(sendRedpackRecordInfo, dbTransaction);
						}
						if (flag)
						{
							for (int i = 0; i < num3; i++)
							{
								sendRedpackRecordInfo.Amount = num;
								flag = sendRedpackRecordDao.AddSendRedpackRecord(sendRedpackRecordInfo, dbTransaction);
								if (!flag)
								{
									dbTransaction.Rollback();
								}
							}
							int num4 = num3 + ((num2 > 0) ? 1 : 0);
							flag = this.UpdateSendRedpackRecord(serialid, num4, dbTransaction);
							if (!flag)
							{
								dbTransaction.Rollback();
							}
						}
						else
						{
							dbTransaction.Rollback();
						}
					}
				}
				catch
				{
					if (dbTransaction.Connection != null)
					{
						dbTransaction.Rollback();
					}
					flag = false;
				}
				finally
				{
					if (flag)
					{
						dbTransaction.Commit();
					}
					dbConnection.Close();
				}
			}
			return flag;
		}

		public bool UpdateSendRedpackRecord(int serialid, int num, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_BalanceDrawRequest set RedpackRecordNum=@RedpackRecordNum where SerialID=@SerialID");
			this.database.AddInParameter(sqlStringCommand, "RedpackRecordNum", System.Data.DbType.Int32, num);
			this.database.AddInParameter(sqlStringCommand, "SerialID", System.Data.DbType.Int32, serialid);
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

		public bool FrozenCommision(int userid, string ReferralStatus)
		{
			string query = "UPDATE aspnet_Distributors set ReferralStatus=@ReferralStatus WHERE UserId=@UserId ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ReferralStatus", System.Data.DbType.String, ReferralStatus);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int FrozenCommisionChecks(string userids, string ReferralStatus)
		{
			string query = "UPDATE aspnet_Distributors set ReferralStatus=@ReferralStatus WHERE UserId in(" + userids + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ReferralStatus", System.Data.DbType.String, ReferralStatus);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int EditCommisionsGrade(string userids, string Grade)
		{
			string query = "UPDATE aspnet_Distributors set DistributorGradeId=@DistributorGradeId WHERE UserId in(" + userids + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "DistributorGradeId", System.Data.DbType.String, Grade);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool EditDisbutosInfos(string userid, string QQNum, string CellPhone, string RealName, string Password)
		{
			string query = "UPDATE aspnet_Members set QQ=@QQ,Password=@Password,CellPhone=@CellPhone,RealName=@RealName WHERE UserId=@UserId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.String, userid);
			this.database.AddInParameter(sqlStringCommand, "QQ", System.Data.DbType.String, QQNum);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, CellPhone);
			this.database.AddInParameter(sqlStringCommand, "RealName", System.Data.DbType.String, RealName);
			this.database.AddInParameter(sqlStringCommand, "Password", System.Data.DbType.String, Password);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateBalanceDistributors(int UserId, decimal ReferralRequestBalance)
		{
			string query = "UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance-@ReferralBlance,ReferralRequestBalance=ReferralRequestBalance+@ReferralRequestBalance  WHERE UserId=@UserId ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ReferralBlance", System.Data.DbType.Decimal, ReferralRequestBalance);
			this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", System.Data.DbType.Decimal, ReferralRequestBalance);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateCalculationCommission(string UserId, string ReferralUserId, string OrderId, decimal OrderTotal, decimal resultCommTatal)
		{
			string text = "";
			text += "begin try  ";
			text += "  begin tran TranUpdate";
			object obj = text;
			text = string.Concat(new object[]
			{
				obj,
				" INSERT INTO   [Hishop_Commissions](UserId,ReferralUserId,OrderId,TradeTime,OrderTotal,CommTotal,CommType,CommRemark,State)values(",
				UserId,
				",",
				ReferralUserId,
				",'",
				OrderId,
				"','",
				DateTime.Now.ToString(),
				"',",
				OrderTotal,
				",",
				resultCommTatal,
				",1,'',1) ;"
			});
			obj = text;
			text = string.Concat(new object[]
			{
				obj,
				"  UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance+",
				resultCommTatal,
				"  WHERE UserId=",
				UserId,
				"; "
			});
			obj = text;
			text = string.Concat(new object[]
			{
				obj,
				"  UPDATE aspnet_Distributors set  OrdersTotal=OrdersTotal+",
				OrderTotal,
				",ReferralOrders=ReferralOrders+1  WHERE UserId=",
				ReferralUserId,
				"; "
			});
			text += " COMMIT TRAN TranUpdate";
			text += "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateGradeId(ArrayList GradeIdList, ArrayList ReferralUserIdList)
		{
			string text = "";
			text += "begin try  ";
			text += "  begin tran TranUpdate";
			for (int i = 0; i < ReferralUserIdList.Count; i++)
			{
				if (!GradeIdList[i].Equals(0))
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						"  UPDATE aspnet_Distributors SET DistributorGradeId=",
						GradeIdList[i],
						" where UserId=",
						ReferralUserIdList[i],
						"; "
					});
				}
			}
			text += " COMMIT TRAN TranUpdate";
			text += "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateStoreCard(int userId, string imgUrl)
		{
			string query = string.Concat(new string[]
			{
				"  UPDATE aspnet_Distributors SET StoreCard='",
				imgUrl,
				"',CardCreatTime='",
				DateTime.Now.ToString("G"),
				"' where UserId=",
				userId.ToString()
			});
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateTwoCalculationCommission(ArrayList UserIdList, string ReferralUserId, string OrderId, ArrayList OrderTotalList, ArrayList CommTatalList)
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
					" INSERT INTO   [Hishop_Commissions](UserId,ReferralUserId,OrderId,TradeTime,OrderTotal,CommTotal,CommType,CommRemark,State)values(",
					UserIdList[i],
					",",
					ReferralUserId,
					",'",
					OrderId,
					"','",
					DateTime.Now.ToString(),
					"',",
					OrderTotalList[i],
					",",
					CommTatalList[i],
					",1,'',1) ;"
				});
				obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"  UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance+",
					CommTatalList[i],
					"  WHERE UserId=",
					UserIdList[i],
					"; "
				});
				obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"  UPDATE aspnet_Distributors set  OrdersTotal=OrdersTotal+",
					OrderTotalList[i],
					",ReferralOrders=ReferralOrders+1  WHERE UserId=",
					UserIdList[i],
					"; "
				});
			}
			text += " COMMIT TRAN TranUpdate";
			text += "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<DistributorGradeInfo> GetDistributorGrades()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_DistributorGrade");
			IList<DistributorGradeInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<DistributorGradeInfo>(dataReader);
			}
			return result;
		}

		public bool UpdateTwoDistributorsOrderNum(ArrayList useridList, ArrayList OrdersTotalList)
		{
			string text = "";
			text += "begin try  ";
			text += "  begin tran TranUpdate";
			for (int i = 0; i < useridList.Count; i++)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"  UPDATE aspnet_Distributors set  OrdersTotal=OrdersTotal+",
					useridList[i],
					",ReferralOrders=ReferralOrders+1  WHERE UserId=",
					OrdersTotalList[i],
					"; "
				});
			}
			text += " COMMIT TRAN TranUpdate";
			text += "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateNotSetCalculationCommission(ArrayList UserIdList, ArrayList OrdersTotal)
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
					"  UPDATE aspnet_Distributors set OrdersTotal=OrdersTotal+",
					OrdersTotal[i],
					" WHERE UserId=",
					UserIdList[i],
					"; "
				});
			}
			text += " COMMIT TRAN TranUpdate";
			text += "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public System.Data.DataTable GetDistributorsCommosion(int userId)
		{
			string query = string.Format("SELECT  GradeId,COUNT(*),SUM(OrdersTotal) AS OrdersTotal,SUM(ReferralOrders) AS ReferralOrders,SUM(ReferralBlance) AS ReferralBlance,SUM(ReferralRequestBalance) AS ReferralRequestBalance FROM aspnet_Distributors WHERE ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}' GROUP BY GradeId", userId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataTable result;
			if (dataSet == null || dataSet.Tables.Count <= 0)
			{
				result = null;
			}
			else
			{
				result = dataSet.Tables[0];
			}
			return result;
		}

		public System.Data.DataTable GetCurrentDistributorsCommosion(int userId)
		{
			string query = string.Format("SELECT sum(OrderTotal) AS OrderTotal,sum(CommTotal) AS CommTotal from dbo.Hishop_Commissions where UserId={0} AND OrderId in (select OrderId from dbo.Hishop_Orders where ReferralUserId={0})", userId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataTable result;
			if (dataSet == null || dataSet.Tables.Count <= 0)
			{
				result = null;
			}
			else
			{
				result = dataSet.Tables[0];
			}
			return result;
		}

		public System.Data.DataTable GetDistributorsCommosion(int userId, DistributorGrade grade)
		{
			string query = string.Format("SELECT sum(OrderTotal) AS OrderTotal,sum(CommTotal) AS CommTotal from dbo.Hishop_Commissions where UserId={0} AND ReferralUserId in (select UserId from aspnet_Distributors  WHERE (ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}') and GradeId={1})", userId, (int)grade);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataTable result;
			if (dataSet == null || dataSet.Tables.Count <= 0)
			{
				result = null;
			}
			else
			{
				result = dataSet.Tables[0];
			}
			return result;
		}

		public System.Data.DataTable OrderIDGetCommosion(string orderid)
		{
			string query = string.Format("SELECT CommId,Userid,OrderTotal,CommTotal from Hishop_Commissions where OrderId='" + orderid + "' ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataTable result;
			if (dataSet == null || dataSet.Tables.Count <= 0)
			{
				result = null;
			}
			else
			{
				result = dataSet.Tables[0];
			}
			return result;
		}

		public System.Data.DataSet GetUserRanking(int userid)
		{
			string query = string.Format("select top 1 w.UserId,w.Logo,w. Blance,w.StoreName,  ( select (( select count(0) from aspnet_Distributors a where (a.ReferralBlance+a.ReferralRequestBalance)>w.Blance or (a.ReferralBlance+a.ReferralRequestBalance=w.Blance ))+(select count(0) from Hishop_CustomDistributor where commtotalsum>w.Blance or commtotalsum=w.Blance)))as ccount  from (select d.UserId,d.Logo,d.ReferralBlance+d.ReferralRequestBalance as Blance,d.StoreName from aspnet_Distributors d union all select id as UserId,Logo,commtotalsum as Blance,StoreName from  Hishop_CustomDistributor ) as w  where UserID={0} select top 10 * from (select UserId,Logo,ReferralBlance+ReferralRequestBalance as Blance,StoreName  from aspnet_Distributors where ReferralStatus!=9  union all select id+10086 as UserId,Logo,commtotalsum as Blance,StoreName from  Hishop_CustomDistributor ) as w order by Blance desc,userid asc select top 10 UserId,Logo,ReferralBlance+ReferralRequestBalance as Blance,StoreName  from aspnet_Distributors where (ReferralPath like '{0}|%' or ReferralPath like '%|{0}' or ReferralPath = '{0}') and ReferralStatus!=9  order by Blance desc", userid);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand);
		}

		public int GetSystemDistributorsCount()
		{
			string query = "select count(*) as num from aspnet_Distributors";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
			{
				result = int.Parse(obj.ToString());
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public bool AddCommission(DistributorGradeCommissionInfo info)
		{
			string text = "UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance+@Commission WHERE UserId=@UserID;";
			text += "select @@identity;INSERT INTO Hishop_Commissions(UserId,ReferralUserId,OrderId,TradeTime,OrderTotal,CommTotal,CommType,State,CommRemark)values(@UserId,@ReferralUserId,@OrderID,@PubTime,0,@Commission,@CommType,1,@Memo);";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "UserID", System.Data.DbType.Int32, info.UserId);
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int32, info.ReferralUserId);
			this.database.AddInParameter(sqlStringCommand, "Commission", System.Data.DbType.Decimal, info.Commission);
			this.database.AddInParameter(sqlStringCommand, "PubTime", System.Data.DbType.DateTime, info.PubTime);
			this.database.AddInParameter(sqlStringCommand, "OperAdmin", System.Data.DbType.String, info.OperAdmin);
			this.database.AddInParameter(sqlStringCommand, "Memo", System.Data.DbType.String, info.Memo);
			this.database.AddInParameter(sqlStringCommand, "OrderID", System.Data.DbType.String, info.OrderID);
			this.database.AddInParameter(sqlStringCommand, "OldCommissionTotal", System.Data.DbType.Decimal, info.OldCommissionTotal);
			this.database.AddInParameter(sqlStringCommand, "CommType", System.Data.DbType.Int32, info.CommType);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
