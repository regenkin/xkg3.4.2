using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.VShop
{
	public class OrderRedPagerDao
	{
		private Database database;

		public OrderRedPagerDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool CreateOrderRedPager(string orderid, decimal ordertotalprice, int userid)
		{
			string query = "select top 1 ID,CouponId,CouponNumber,CouponName,ActivityName,ImgUrl,ShareTitle,Description from Hishop_ShareActivity where MeetValue<=@MeetValue and BeginDate<=GETDATE() and EndDate>=GETDATE() order by MeetValue desc ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "MeetValue", System.Data.DbType.Decimal, ordertotalprice);
			System.Data.DataTable dataTable = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
			bool result;
			if (!string.IsNullOrEmpty(orderid) && userid > 0 && dataTable.Rows.Count > 0)
			{
				int num = Globals.ToNum(dataTable.Rows[0]["CouponId"]);
				if (num > 0)
				{
					CouponInfo couponDetails = new CouponDao().GetCouponDetails(num);
					if (couponDetails != null)
					{
						result = this.CreateOrderRedPager(new OrderRedPagerInfo
						{
							OrderID = orderid,
							RedPagerActivityId = int.Parse(dataTable.Rows[0]["ID"].ToString()),
							RedPagerActivityName = dataTable.Rows[0]["ActivityName"].ToString(),
							MaxGetTimes = int.Parse(dataTable.Rows[0]["CouponNumber"].ToString()),
							AlreadyGetTimes = 0,
							ItemAmountLimit = couponDetails.CouponValue,
							OrderAmountCanUse = couponDetails.ConditionValue,
							ExpiryDays = (couponDetails.EndDate - DateTime.Now).Days,
							UserID = userid
						});
						return result;
					}
				}
			}
			result = false;
			return result;
		}

		public bool CreateOrderRedPager(OrderRedPagerInfo orderredpager)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO vshop_OrderRedPager(OrderID,RedPagerActivityId,RedPagerActivityName,MaxGetTimes,AlreadyGetTimes,ItemAmountLimit,OrderAmountCanUse,ExpiryDays,UserID) VALUES(@OrderID,@RedPagerActivityId,@RedPagerActivityName,@MaxGetTimes,@AlreadyGetTimes,@ItemAmountLimit,@OrderAmountCanUse,@ExpiryDays,@UserID);select @@identity");
			this.database.AddInParameter(sqlStringCommand, "OrderID", System.Data.DbType.String, orderredpager.OrderID);
			this.database.AddInParameter(sqlStringCommand, "RedPagerActivityId", System.Data.DbType.Int32, orderredpager.RedPagerActivityId);
			this.database.AddInParameter(sqlStringCommand, "RedPagerActivityName", System.Data.DbType.String, orderredpager.RedPagerActivityName);
			this.database.AddInParameter(sqlStringCommand, "MaxGetTimes", System.Data.DbType.Int32, orderredpager.MaxGetTimes);
			this.database.AddInParameter(sqlStringCommand, "AlreadyGetTimes", System.Data.DbType.Int32, orderredpager.AlreadyGetTimes);
			this.database.AddInParameter(sqlStringCommand, "ItemAmountLimit", System.Data.DbType.Decimal, orderredpager.ItemAmountLimit);
			this.database.AddInParameter(sqlStringCommand, "OrderAmountCanUse", System.Data.DbType.Decimal, orderredpager.OrderAmountCanUse);
			this.database.AddInParameter(sqlStringCommand, "ExpiryDays", System.Data.DbType.Int32, orderredpager.ExpiryDays);
			this.database.AddInParameter(sqlStringCommand, "UserID", System.Data.DbType.Int32, orderredpager.UserID);
			int num = this.database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public bool UpdateOrderRedPager(OrderRedPagerInfo orderredpager)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_OrderRedPager SET RedPagerActivityId=@RedPagerActivityId,MaxGetTimes=@MaxGetTimes,AlreadyGetTimes=@AlreadyGetTimes,ItemAmountLimit=@ItemAmountLimit,OrderAmountCanUse=@OrderAmountCanUse,ExpiryDays=@ExpiryDays,UserID=@UserID WHERE OrderID=@OrderID");
			this.database.AddInParameter(sqlStringCommand, "OrderID", System.Data.DbType.String, orderredpager.OrderID);
			this.database.AddInParameter(sqlStringCommand, "RedPagerActivityId", System.Data.DbType.Int32, orderredpager.RedPagerActivityId);
			this.database.AddInParameter(sqlStringCommand, "MaxGetTimes", System.Data.DbType.Int32, orderredpager.MaxGetTimes);
			this.database.AddInParameter(sqlStringCommand, "AlreadyGetTimes", System.Data.DbType.Int32, orderredpager.AlreadyGetTimes);
			this.database.AddInParameter(sqlStringCommand, "ItemAmountLimit", System.Data.DbType.Decimal, orderredpager.ItemAmountLimit);
			this.database.AddInParameter(sqlStringCommand, "OrderAmountCanUse", System.Data.DbType.Decimal, orderredpager.OrderAmountCanUse);
			this.database.AddInParameter(sqlStringCommand, "ExpiryDays", System.Data.DbType.Int32, orderredpager.ExpiryDays);
			this.database.AddInParameter(sqlStringCommand, "UserID", System.Data.DbType.Int32, orderredpager.UserID);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public OrderRedPagerInfo GetOrderRedPagerInfo(string orderid)
		{
			OrderRedPagerInfo result;
			if (string.IsNullOrEmpty(orderid))
			{
				result = null;
			}
			else
			{
				OrderRedPagerInfo orderRedPagerInfo = new OrderRedPagerInfo();
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_OrderRedPager where OrderID=@OrderID");
				this.database.AddInParameter(sqlStringCommand, "OrderID", System.Data.DbType.String, orderid);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						orderRedPagerInfo = DataMapper.PopulateOrderRedPagerInfo(dataReader);
					}
				}
				result = orderRedPagerInfo;
			}
			return result;
		}

		public bool SetIsOpen(int orderredpagerid, bool isopen)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_OrderRedPager set IsOpen=@IsOpen where OrderRedPagerId=@OrderRedPagerId");
			this.database.AddInParameter(sqlStringCommand, "OrderRedPagerId", System.Data.DbType.Int32, orderredpagerid);
			this.database.AddInParameter(sqlStringCommand, "IsOpen", System.Data.DbType.Boolean, isopen);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DelOrderRedPager(int orderid)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete  vshop_OrderRedPager  where OrderID=@OrderID");
			this.database.AddInParameter(sqlStringCommand, "OrderID", System.Data.DbType.Int32, orderid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
