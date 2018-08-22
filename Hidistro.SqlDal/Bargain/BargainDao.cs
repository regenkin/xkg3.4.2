using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Bargain;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Bargain
{
	public class BargainDao
	{
		private Database database;

		public BargainDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public int GetTotal(BargainQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" IsDelete=0 ");
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
			if (!string.IsNullOrEmpty(query.Title))
			{
				stringBuilder.AppendFormat(" AND Title LIKE '%{0}%'", DataHelper.CleanSearchString(query.Title));
			}
			string type = query.Type;
			if (type != null)
			{
				if (!(type == "0"))
				{
					if (!(type == "1"))
					{
						if (!(type == "2"))
						{
							if (type == "3")
							{
								stringBuilder.AppendFormat(" AND bargainstatus='未开始' ", new object[0]);
							}
						}
						else
						{
							stringBuilder.AppendFormat(" AND bargainstatus='已结束' ", new object[0]);
						}
					}
					else
					{
						stringBuilder.AppendFormat(" AND bargainstatus='进行中'", new object[0]);
					}
				}
			}
			string query2 = string.Format(" SELECT count(*) FROM vw_Hishop_BargainList where {0}", stringBuilder.ToString());
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query2);
			int result = 0;
			if (this.database.ExecuteScalar(sqlStringCommand) != null)
			{
				result = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
			}
			return result;
		}

		public DbQueryResult GetBargainList(BargainQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" IsDelete=0 and SaleStatus=1");
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
			if (!string.IsNullOrEmpty(query.Title))
			{
				stringBuilder.AppendFormat(" AND Title LIKE '%{0}%'", DataHelper.CleanSearchString(query.Title));
			}
			string type = query.Type;
			if (type != null)
			{
				if (!(type == "0"))
				{
					if (!(type == "1"))
					{
						if (!(type == "2"))
						{
							if (type == "3")
							{
								stringBuilder.AppendFormat(" AND bargainstatus='未开始' ", new object[0]);
							}
						}
						else
						{
							stringBuilder.AppendFormat(" AND bargainstatus='已结束' ", new object[0]);
						}
					}
					else
					{
						stringBuilder.AppendFormat(" AND bargainstatus='进行中'", new object[0]);
					}
				}
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BargainList", "Id", stringBuilder.ToString(), "*");
		}

		public DbQueryResult GetMyBargainList(BargainQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" IsDelete=0 AND bargainstatus='{0}' ", "进行中");
			if (query.Status > 0)
			{
				stringBuilder.AppendFormat("AND bargainDetialID in (select bargainDetialID from Hishop_HelpBargainDetial where UserId={0})", query.UserId);
				stringBuilder.AppendFormat("and userid!={0}", query.UserId);
			}
			else
			{
				stringBuilder.AppendFormat(" AND UserId={0}", query.UserId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_MyBargainList", "Id", stringBuilder.ToString(), "*");
		}

		public System.Data.DataTable GetAllBargain()
		{
			string query = "select * from Hishop_Bargain where IsDelete=0 ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public bool DeleteBargainById(string ids)
		{
			string query = "update Hishop_Bargain set IsDelete=1 where id  in (" + ids + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateNumberById(int bargainDetialId, int number)
		{
			string query = "update Hishop_BargainDetial set Number=@number where id  =@id";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "number", System.Data.DbType.Int32, number);
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, bargainDetialId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public System.Data.DataTable GetBargainById(string ids)
		{
			string query = "select * from Hishop_Bargain where id  in (" + ids + ") and BeginDate< GETDATE() AND  GETDATE()< EndDate AND  IsDelete=0";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public string IsCanBuyByBarginId(int bargainId)
		{
			string result = "1";
			string query = "select top 1 id,BeginDate,EndDate from Hishop_Bargain where id=" + bargainId + " and ActivityStock>TranNumber";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable dataTable = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
			if (dataTable.Rows.Count > 0)
			{
				DateTime t = DateTime.Parse(dataTable.Rows[0]["EndDate"].ToString());
				DateTime t2 = DateTime.Parse(dataTable.Rows[0]["BeginDate"].ToString());
				if (t < DateTime.Now)
				{
					result = "该活动已结束";
				}
				else if (t2 > DateTime.Now)
				{
					result = "该活动还未开始";
				}
			}
			else
			{
				result = "该活动商品无库存";
			}
			return result;
		}

		public string IsCanBuyByBarginDetailId(int bargainDetailId)
		{
			string result = "1";
			string query = "select top 1 id,BeginDate,EndDate from Hishop_Bargain where id =(select BargainId from Hishop_BargainDetial where id=" + bargainDetailId + " AND IsDelete=0) and ActivityStock>TranNumber";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable dataTable = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
			if (dataTable.Rows.Count > 0)
			{
				DateTime t = DateTime.Parse(dataTable.Rows[0]["EndDate"].ToString());
				DateTime t2 = DateTime.Parse(dataTable.Rows[0]["BeginDate"].ToString());
				if (t < DateTime.Now)
				{
					result = "该活动已结束";
				}
				else if (t2 > DateTime.Now)
				{
					result = "该活动还未开始";
				}
			}
			else
			{
				result = "该活动商品无库存";
			}
			return result;
		}

		public bool InsertBargain(BargainInfo bargain)
		{
			string query = "insert into Hishop_Bargain(Title,ActivityCover,BeginDate,EndDate,Remarks,CreateDate,ProductId,ActivityStock,PurchaseNumber,BargainType,BargainTypeMaxVlue,BargainTypeMinVlue,InitialPrice,IsCommission,FloorPrice) values(@Title,@ActivityCover,@BeginDate,@EndDate,@Remarks,@CreateDate,@ProductId,@ActivityStock,@PurchaseNumber,@BargainType,@BargainTypeMaxVlue,@BargainTypeMinVlue,@InitialPrice,@IsCommission,@FloorPrice)";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, bargain.Title);
			this.database.AddInParameter(sqlStringCommand, "ActivityCover", System.Data.DbType.String, bargain.ActivityCover);
			this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.DateTime, bargain.BeginDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, bargain.EndDate);
			this.database.AddInParameter(sqlStringCommand, "CreateDate", System.Data.DbType.DateTime, bargain.CreateDate);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, bargain.ProductId);
			this.database.AddInParameter(sqlStringCommand, "ActivityStock", System.Data.DbType.Int32, bargain.ActivityStock);
			this.database.AddInParameter(sqlStringCommand, "PurchaseNumber", System.Data.DbType.Int32, bargain.PurchaseNumber);
			this.database.AddInParameter(sqlStringCommand, "BargainType", System.Data.DbType.Int32, bargain.BargainType);
			this.database.AddInParameter(sqlStringCommand, "BargainTypeMaxVlue", System.Data.DbType.Double, bargain.BargainTypeMaxVlue);
			this.database.AddInParameter(sqlStringCommand, "BargainTypeMinVlue", System.Data.DbType.Double, bargain.BargainTypeMinVlue);
			this.database.AddInParameter(sqlStringCommand, "InitialPrice", System.Data.DbType.Decimal, bargain.InitialPrice);
			this.database.AddInParameter(sqlStringCommand, "IsCommission", System.Data.DbType.Boolean, bargain.IsCommission);
			this.database.AddInParameter(sqlStringCommand, "FloorPrice", System.Data.DbType.Decimal, bargain.FloorPrice);
			this.database.AddInParameter(sqlStringCommand, "Remarks", System.Data.DbType.String, bargain.Remarks);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool InsertBargainDetial(BargainDetialInfo bargainDetial, out int bargainDetialId)
		{
			bargainDetialId = 0;
			string query = "insert into Hishop_BargainDetial(UserId,BargainId,Number,Price,NumberOfParticipants,CreateDate,Sku) values(@UserId,@BargainId,@Number,@Price,@NumberOfParticipants,@CreateDate,@Sku)";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, bargainDetial.UserId);
			this.database.AddInParameter(sqlStringCommand, "BargainId", System.Data.DbType.Int32, bargainDetial.BargainId);
			this.database.AddInParameter(sqlStringCommand, "Number", System.Data.DbType.Int32, bargainDetial.Number);
			this.database.AddInParameter(sqlStringCommand, "Price", System.Data.DbType.Decimal, bargainDetial.Price);
			this.database.AddInParameter(sqlStringCommand, "NumberOfParticipants", System.Data.DbType.Int32, bargainDetial.NumberOfParticipants);
			this.database.AddInParameter(sqlStringCommand, "CreateDate", System.Data.DbType.DateTime, bargainDetial.CreateDate);
			this.database.AddInParameter(sqlStringCommand, "Sku", System.Data.DbType.String, bargainDetial.Sku);
			bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
			if (flag)
			{
				string query2 = "select max(id) from Hishop_BargainDetial ";
				sqlStringCommand = this.database.GetSqlStringCommand(query2);
				bargainDetialId = (int)this.database.ExecuteScalar(sqlStringCommand);
			}
			return flag;
		}

		public bool InsertHelpBargainDetial(HelpBargainDetialInfo helpBargainDetial)
		{
			string query = "insert into Hishop_HelpBargainDetial(BargainDetialId,UserId,BargainPrice,CreateDate,BargainId) values(@BargainDetialId,@UserId,@BargainPrice,@CreateDate,@BargainId)";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "BargainDetialId", System.Data.DbType.Int32, helpBargainDetial.BargainDetialId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, helpBargainDetial.UserId);
			this.database.AddInParameter(sqlStringCommand, "BargainPrice", System.Data.DbType.Decimal, helpBargainDetial.BargainPrice);
			this.database.AddInParameter(sqlStringCommand, "CreateDate", System.Data.DbType.DateTime, helpBargainDetial.CreateDate);
			this.database.AddInParameter(sqlStringCommand, "BargainId", System.Data.DbType.Int32, helpBargainDetial.BargainId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateBargainDetial(HelpBargainDetialInfo helpBargainDetial)
		{
			string query = "update Hishop_BargainDetial set  Price=Price-@BargainPrice,NumberOfParticipants=NumberOfParticipants+1 where id=@id";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "BargainPrice", System.Data.DbType.Decimal, helpBargainDetial.BargainPrice);
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, helpBargainDetial.BargainDetialId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ExistsHelpBargainDetial(HelpBargainDetialInfo helpBargainDetial)
		{
			string query = "select count(*) from Hishop_HelpBargainDetial where BargainDetialId=@BargainDetialId and UserId=@UserId and BargainId=@BargainId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "BargainDetialId", System.Data.DbType.Int32, helpBargainDetial.BargainDetialId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, helpBargainDetial.UserId);
			this.database.AddInParameter(sqlStringCommand, "BargainId", System.Data.DbType.Int32, helpBargainDetial.BargainId);
			return int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString()) > 0;
		}

		public int HelpBargainCount(int bargainId)
		{
			string query = "select count(*) from Hishop_HelpBargainDetial where BargainId=@BargainId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "BargainId", System.Data.DbType.Int32, bargainId);
			return int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
		}

		public bool UpdateBargain(BargainInfo bargain)
		{
			string query = "update  Hishop_Bargain set  Title=@Title,ActivityCover=@ActivityCover,BeginDate=@BeginDate,EndDate=@EndDate,Remarks=@Remarks,CreateDate=@CreateDate,ProductId=@ProductId,ActivityStock=@ActivityStock,PurchaseNumber=@PurchaseNumber,BargainType=@BargainType,BargainTypeMaxVlue=@BargainTypeMaxVlue,BargainTypeMinVlue=@BargainTypeMinVlue,InitialPrice=@InitialPrice,IsCommission=@IsCommission,FloorPrice=@FloorPrice where id=@id";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, bargain.Title);
			this.database.AddInParameter(sqlStringCommand, "ActivityCover", System.Data.DbType.String, bargain.ActivityCover);
			this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.DateTime, bargain.BeginDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, bargain.EndDate);
			this.database.AddInParameter(sqlStringCommand, "CreateDate", System.Data.DbType.DateTime, bargain.CreateDate);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, bargain.ProductId);
			this.database.AddInParameter(sqlStringCommand, "ActivityStock", System.Data.DbType.Int32, bargain.ActivityStock);
			this.database.AddInParameter(sqlStringCommand, "PurchaseNumber", System.Data.DbType.Int32, bargain.PurchaseNumber);
			this.database.AddInParameter(sqlStringCommand, "BargainType", System.Data.DbType.Int32, bargain.BargainType);
			this.database.AddInParameter(sqlStringCommand, "BargainTypeMaxVlue", System.Data.DbType.Double, bargain.BargainTypeMaxVlue);
			this.database.AddInParameter(sqlStringCommand, "BargainTypeMinVlue", System.Data.DbType.Double, bargain.BargainTypeMinVlue);
			this.database.AddInParameter(sqlStringCommand, "InitialPrice", System.Data.DbType.Decimal, bargain.InitialPrice);
			this.database.AddInParameter(sqlStringCommand, "IsCommission", System.Data.DbType.Boolean, bargain.IsCommission);
			this.database.AddInParameter(sqlStringCommand, "FloorPrice", System.Data.DbType.Decimal, bargain.FloorPrice);
			this.database.AddInParameter(sqlStringCommand, "Remarks", System.Data.DbType.String, bargain.Remarks);
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.String, bargain.Id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateBargain(int bargainId, DateTime endDate)
		{
			string query = "update  Hishop_Bargain set  EndDate=@EndDate where id=@id";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, endDate);
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.String, bargainId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public BargainInfo GetBargainInfo(int id)
		{
			BargainInfo result = new BargainInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Bargain WHERE id = @id AND  IsDelete=0");
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<BargainInfo>(dataReader);
			}
			return result;
		}

		public BargainInfo GetBargainInfoByDetialId(int bargainDetialId)
		{
			BargainInfo result = new BargainInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_Bargain where id =(select BargainId from Hishop_BargainDetial where id=@id AND IsDelete=0) AND IsDelete=0");
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, bargainDetialId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<BargainInfo>(dataReader);
			}
			return result;
		}

		public HelpBargainDetialInfo GeHelpBargainDetialInfo(int bargainDetialId, int userId)
		{
			HelpBargainDetialInfo result = new HelpBargainDetialInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_HelpBargainDetial WHERE BargainDetialId = @BargainDetialId and UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "BargainDetialId", System.Data.DbType.Int32, bargainDetialId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<HelpBargainDetialInfo>(dataReader);
			}
			return result;
		}

		public BargainDetialInfo GetBargainDetialInfo(int id)
		{
			BargainDetialInfo result = new BargainDetialInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BargainDetial WHERE id = @id AND IsDelete=0");
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<BargainDetialInfo>(dataReader);
			}
			return result;
		}

		public BargainDetialInfo GetBargainDetialInfo(int bargainId, int userId)
		{
			BargainDetialInfo result = new BargainDetialInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BargainDetial WHERE BargainId = @bargainId and UserId=@UserId AND IsDelete=0");
			this.database.AddInParameter(sqlStringCommand, "bargainId", System.Data.DbType.Int32, bargainId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<BargainDetialInfo>(dataReader);
			}
			return result;
		}

		public BargainStatisticalData GetBargainStatisticalDataInfo(int bargainId)
		{
			BargainStatisticalData result = new BargainStatisticalData();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select (SELECT COUNT(*) FROM Hishop_HelpBargainDetial WHERE BargainId=@id) AS NumberOfParticipants,(SELECT COUNT(*)  from ( SELECT   a.UserId  FROM Hishop_Orders a join Hishop_BargainDetial b on a.BargainDetialId=b.id where BargainId =@id and  (OrderStatus=5 OR OrderStatus=2 OR (OrderStatus=1 and Gateway='hishop.plugins.payment.podrequest')) group by a.UserId ) as w) as SingleMember, (SELECT SUM(b.Number)  FROM Hishop_Orders a join Hishop_BargainDetial b on a.BargainDetialId=b.id where BargainId =@id and  (OrderStatus=5 OR OrderStatus=2 OR (OrderStatus=1 and Gateway='hishop.plugins.payment.podrequest')) ) as ActivitySales, (SELECT ActivityStock  FROM  Hishop_Bargain  where id=@id) as ActivityStock, (SELECT sum(Number*price)  FROM Hishop_Orders a join Hishop_BargainDetial b on a.BargainDetialId=b.id where BargainId =@id and  (OrderStatus=5 OR OrderStatus=2 OR (OrderStatus=1 and Gateway='hishop.plugins.payment.podrequest')) ) as AverageTransactionPrice, CASE WHEN BeginDate <GETDATE() and GETDATE()<EndDate  THEN '进行中' WHEN BeginDate >GETDATE()  THEN '未开始' WHEN EndDate < GETDATE()  THEN '已结束' ELSE NULL END ActiveState  from Hishop_Bargain where id=@id");
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, bargainId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<BargainStatisticalData>(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetHelpBargainDetials(int bargainDetialId)
		{
			string query = "select  b.UserName,a.* from Hishop_HelpBargainDetial a join aspnet_Members b on a.userid=b.userid where BargainDetialId  = " + bargainDetialId + " order by a.CreateDate desc ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public int GetHelpBargainDetialCount(int bargainDetialId)
		{
			string query = "select count(*)  from Hishop_HelpBargainDetial where BargainDetialId=" + bargainDetialId;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
		}

		public bool ActionIsEnd(int bargainDetialId)
		{
			string query = "select count(0) from Hishop_Orders where BargainDetialId=" + bargainDetialId;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "BargainDetialId", System.Data.DbType.Int32, bargainDetialId);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)) > 0;
		}
	}
}
