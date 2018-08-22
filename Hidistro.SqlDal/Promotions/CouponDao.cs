using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class CouponDao
	{
		private Database database;

		public CouponDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public CouponActionStatus CreateCoupon(CouponInfo coupon)
		{
			CouponActionStatus couponActionStatus = CouponActionStatus.UnknowError;
			CouponActionStatus result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupon_Coupons WHERE CouponName=@CouponName and Finished=0");
				this.database.AddInParameter(sqlStringCommand, "CouponName", System.Data.DbType.String, coupon.CouponName);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
					couponActionStatus = CouponActionStatus.DuplicateName;
					result = couponActionStatus;
					return result;
				}
				sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_Coupon_Coupons] ([CouponName],[CouponValue],[ConditionValue],[BeginDate],[EndDate],[StockNum],[ReceiveNum],[UsedNum],[MemberGrades],[DefualtGroup],[CustomGroup],[ImgUrl],[ProductNumber],[Finished],[IsAllProduct],maxReceivNum,CouponTypes) VALUES (@CouponName,@CouponValue,@ConditionValue,@BeginDate,@EndDate,@StockNum,@ReceiveNum,@UsedNum,@MemberGrades,@DefualtGroup,@CustomGroup,@ImgUrl,@ProductNumber,@Finished,@IsAllProduct,@maxReceivNum,@CouponTypes)");
				this.database.AddInParameter(sqlStringCommand, "CouponName", System.Data.DbType.String, coupon.CouponName);
				this.database.AddInParameter(sqlStringCommand, "CouponValue", System.Data.DbType.Decimal, coupon.CouponValue);
				this.database.AddInParameter(sqlStringCommand, "ConditionValue", System.Data.DbType.Decimal, coupon.ConditionValue);
				this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.DateTime, coupon.BeginDate);
				this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, coupon.EndDate);
				this.database.AddInParameter(sqlStringCommand, "StockNum", System.Data.DbType.Int32, coupon.StockNum);
				this.database.AddInParameter(sqlStringCommand, "ReceiveNum", System.Data.DbType.Int32, coupon.ReceiveNum);
				this.database.AddInParameter(sqlStringCommand, "UsedNum", System.Data.DbType.Int32, coupon.UsedNum);
				this.database.AddInParameter(sqlStringCommand, "MemberGrades", System.Data.DbType.String, coupon.MemberGrades);
				this.database.AddInParameter(sqlStringCommand, "DefualtGroup", System.Data.DbType.String, coupon.DefualtGroup);
				this.database.AddInParameter(sqlStringCommand, "CustomGroup", System.Data.DbType.String, coupon.CustomGroup);
				this.database.AddInParameter(sqlStringCommand, "ImgUrl", System.Data.DbType.String, coupon.ImgUrl);
				this.database.AddInParameter(sqlStringCommand, "ProductNumber", System.Data.DbType.Int32, coupon.ProductNumber);
				this.database.AddInParameter(sqlStringCommand, "Finished", System.Data.DbType.Boolean, coupon.Finished);
				this.database.AddInParameter(sqlStringCommand, "IsAllProduct", System.Data.DbType.Boolean, coupon.IsAllProduct);
				this.database.AddInParameter(sqlStringCommand, "maxReceivNum", System.Data.DbType.Int32, coupon.maxReceivNum);
				this.database.AddInParameter(sqlStringCommand, "CouponTypes", System.Data.DbType.String, coupon.CouponTypes);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				couponActionStatus = CouponActionStatus.Success;
			}
			catch (Exception var_3_263)
			{
				couponActionStatus = CouponActionStatus.CreateClaimCodeError;
			}
			result = couponActionStatus;
			return result;
		}

		public bool setCouponFinished(int couponId, bool bfinished)
		{
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupon_Coupons WHERE  CouponId=@CouponId ");
				this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
					sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Coupon_Coupons SET Finished=@Finished WHERE CouponId=@CouponId");
					this.database.AddInParameter(sqlStringCommand, "Finished", System.Data.DbType.Boolean, bfinished);
					this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public CouponInfo GetCouponDetails(string couponName)
		{
			CouponInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Coupon_Coupons WHERE CouponName = @CouponName");
			this.database.AddInParameter(sqlStringCommand, "CouponName", System.Data.DbType.String, couponName);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<CouponInfo>(dataReader);
			}
			return result;
		}

		public string UpdateCoupon(int couponId, CouponEdit content, ref string msg)
		{
			string result;
			try
			{
				CouponInfo couponDetails = this.GetCouponDetails(couponId);
				if (couponDetails != null)
				{
					string text = "";
					if (content.maxReceivNum.HasValue)
					{
						text += string.Format(",maxReceivNum={0}", content.maxReceivNum);
					}
					if (content.totalNum.HasValue)
					{
						if (content.totalNum < couponDetails.ReceiveNum)
						{
							result = "修改的优惠券总量少于已发放的数量";
							return result;
						}
						text += string.Format(",StockNum={0}", content.totalNum);
					}
					if (content.begin.HasValue)
					{
						text += string.Format(",BeginDate='{0}'", content.begin.Value.ToString("yyyy-MM-dd HH:mm:ss"));
					}
					if (content.end.HasValue)
					{
						text += string.Format(",EndDate='{0}'", content.end.Value.ToString("yyyy-MM-dd HH:mm:ss"));
					}
					if (text.Length > 1)
					{
						System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Coupon_Coupons SET {0} WHERE CouponId={1}", text.Substring(1), couponId));
						this.database.ExecuteNonQuery(sqlStringCommand);
						result = "1";
					}
					else
					{
						result = "没有找到编辑内容";
					}
				}
				else
				{
					result = "没有这个优惠券";
				}
			}
			catch (Exception ex)
			{
				result = "编辑优惠券失败(" + ex.Message + ")";
			}
			return result;
		}

		public bool DeleteCoupon(int couponId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Coupon_Coupons WHERE CouponId = @CouponId and ReceiveNum=0");
			this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DbQueryResult GetCouponInfos(CouponsSearch search)
		{
			DbQueryResult result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(" 1=1 ");
				if (!string.IsNullOrEmpty(search.CouponName))
				{
					stringBuilder.Append(" and CouponName like '%" + search.CouponName.ReplaceSingleQuoteMark() + "%'  ");
				}
				if (search.beginDate.HasValue)
				{
					stringBuilder.Append(" and beginDate<='" + search.beginDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' ");
				}
				if (search.endDate.HasValue)
				{
					stringBuilder.Append(" and endDate>='" + search.endDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' ");
				}
				if (search.Finished.HasValue)
				{
					if (search.Finished.Value)
					{
						stringBuilder.Append(" and (Finished=1 or getdate()>endDate)");
					}
					else
					{
						stringBuilder.Append(" and Finished=0 and getdate()<=endDate");
					}
				}
				if (search.minValue.HasValue)
				{
					stringBuilder.Append(" and CouponValue>=" + search.minValue.Value);
				}
				if (search.maxValue.HasValue)
				{
					stringBuilder.Append(" and CouponValue<=" + search.maxValue.Value);
				}
				if (search.SearchType.HasValue && search.SearchType.Value != 0)
				{
					stringBuilder.Append(" and CouponTypes like '%" + search.SearchType.Value + "%'");
				}
				result = DataHelper.PagingByRownumber(search.PageIndex, search.PageSize, search.SortBy, search.SortOrder, search.IsCount, "Hishop_Coupon_Coupons", "CouponId", stringBuilder.ToString(), "*");
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		public System.Data.DataTable GetUnFinishedCoupon(DateTime end, CouponType? type = null)
		{
			string text = string.Format("select * from Hishop_Coupon_Coupons where EndDate>'{0}' and Finished=0 ", end.ToString("yyyy-MM-dd HH:mm:ss"));
			if (type.HasValue)
			{
				text = text + " and CouponTypes like '%" + ((int)type.Value).ToString() + "%'";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				result = dataTable;
			}
			return result;
		}

		public DbQueryResult GetMemberCoupons(MemberCouponsSearch search)
		{
			DbQueryResult result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(" 1=1 ");
				if (!string.IsNullOrEmpty(search.CouponName))
				{
					stringBuilder.Append(" and CouponName like '% " + search.CouponName.ReplaceSingleQuoteMark() + " %'  ");
				}
				if (!string.IsNullOrEmpty(search.OrderNo))
				{
					stringBuilder.Append(" and OrderNo='" + search.OrderNo.ReplaceSingleQuoteMark() + "' ");
				}
				result = DataHelper.PagingByRownumber(search.PageIndex, search.PageSize, search.SortBy, search.SortOrder, search.IsCount, "Hishop_Coupon_MemberCoupons", "Id", stringBuilder.ToString(), "*");
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		public int GetMemberCouponsNumbyUserId(int UserId)
		{
			int result = 0;
			string query = "select count(id) as total from Hishop_Coupon_MemberCoupons where getdate()<=EndDate and Status=0 and MemberId=" + UserId.ToString();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				result = int.Parse(dataTable.Rows[0][0].ToString());
			}
			return result;
		}

		public bool CheckCouponsIsUsed(int MemberCouponsId)
		{
			string query = "select Status from Hishop_Coupon_MemberCoupons where id=@MemberCouponsId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "MemberCouponsId", System.Data.DbType.Int32, MemberCouponsId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int num = 0;
			if (obj != null && obj != DBNull.Value)
			{
				num = (int)obj;
			}
			return num > 0;
		}

		public string GetCouponsProductIds(int CouponId)
		{
			string query = "select ProductId from Hishop_Coupon_Products where CouponId=" + CouponId.ToString();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable dataTable = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			string text = "";
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					text = text + "_" + dataRow["ProductId"].ToString();
				}
			}
			if (text != "")
			{
				text = text.Remove(0, 1);
			}
			return text;
		}

		public string GetCouponsProductIdsByMemberCouponIDByRedPagerId(int redPagerId)
		{
			string query = "select ProductId from Hishop_Coupon_Products where CouponId=(select top 1 CouponId from Hishop_Coupon_MemberCoupons where Id=" + redPagerId.ToString() + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable dataTable = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			string text = "";
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					text = text + "_" + dataRow["ProductId"].ToString();
				}
			}
			if (text != "")
			{
				text = text.Remove(0, 1);
			}
			return text;
		}

		public System.Data.DataTable GetMemberCoupons(MemberCouponsSearch search, ref int total)
		{
			System.Data.DataTable result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(" 1=1 ");
				total = 0;
				if (!string.IsNullOrEmpty(search.CouponName))
				{
					stringBuilder.Append(" and a.CouponName like '%" + search.CouponName.ReplaceSingleQuoteMark() + "%'  ");
				}
				if (!string.IsNullOrEmpty(search.OrderNo))
				{
					stringBuilder.Append(" and OrderNo='" + search.OrderNo.ReplaceSingleQuoteMark() + "' ");
				}
				if (search.MemberId > 0)
				{
					stringBuilder.Append(" and MemberId='" + search.MemberId.ToString() + "' ");
				}
				int num = 0;
				if (!string.IsNullOrEmpty(search.Status) && int.TryParse(search.Status, out num))
				{
					stringBuilder.Append(" and a.Status='" + num.ToString() + "' ");
				}
				string query = "select count(id) as total from Hishop_Coupon_MemberCoupons a where  " + stringBuilder.ToString();
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
					total = int.Parse(dataTable.Rows[0][0].ToString());
				}
				if (total <= 0)
				{
					result = null;
				}
				else
				{
					int num2 = 0;
					int num3 = search.PageIndex * search.PageSize;
					if (search.PageIndex != 0 && search.PageSize != 0)
					{
						num2 = search.PageSize;
						if ((double)search.PageIndex >= Math.Ceiling((double)total / (double)search.PageSize))
						{
							search.PageIndex = int.Parse(Math.Ceiling((double)total / (double)search.PageSize).ToString());
						}
						int num4 = search.PageIndex * search.PageSize;
						if (num4 > total)
						{
							num2 = search.PageSize - (num4 - total);
						}
					}
					string str = "order by  a.CouponId desc ";
					string text = "order by  CouponId desc";
					string text2 = "order by  CouponId desc ";
					string str2 = string.Format("select top {0} a.* , b.userName,c.IsAllProduct  from Hishop_Coupon_MemberCoupons a inner join Hishop_Coupon_Coupons c on  c.CouponId=a.CouponId left join aspnet_Members b on a.memberid=b.userId  where ", num3);
					string query2 = string.Format("select * from ( select top {0} * from ( {1} ) as t1 {2} ) as t2 {3} ", new object[]
					{
						num2,
						str2 + stringBuilder.ToString() + str,
						text2,
						text
					});
					sqlStringCommand = this.database.GetSqlStringCommand(query2);
					using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
					{
						System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
						result = dataTable;
					}
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		public bool CreateMemberCouponsInfo(MemberCouponsInfo mCoupons)
		{
			return false;
		}

		public CouponInfo GetCouponDetails(int couponId)
		{
			CouponInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Coupon_Coupons WHERE CouponId = @CouponId");
			this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<CouponInfo>(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetCouponProducts(int couponId)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select couponId, a.ProductId , b.ProductName , a.status from Hishop_Coupon_Products a\tjoin Hishop_Products b on a.ProductId=b.ProductId where a.couponId=@couponId");
			this.database.AddInParameter(sqlStringCommand, "couponId", System.Data.DbType.Int32, couponId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public bool AddCouponProducts(int couponId, bool IsAllProduct, IList<string> productIDs)
		{
			bool result;
			try
			{
				CouponInfo couponDetails = this.GetCouponDetails(couponId);
				if (couponDetails != null)
				{
					if (IsAllProduct)
					{
						couponDetails.IsAllProduct = true;
						couponDetails.ProductNumber = 0;
					}
					else
					{
						couponDetails.IsAllProduct = false;
						couponDetails.ProductNumber = productIDs.Count;
					}
					if (IsAllProduct)
					{
						System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Coupon_Coupons set IsAllProduct=@IsAllProduct , ProductNumber=@ProductNumber WHERE CouponId = @CouponId");
						this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
						this.database.AddInParameter(sqlStringCommand, "ProductNumber", System.Data.DbType.Int32, couponDetails.ProductNumber);
						this.database.AddInParameter(sqlStringCommand, "IsAllProduct", System.Data.DbType.Boolean, couponDetails.IsAllProduct);
						this.database.ExecuteNonQuery(sqlStringCommand);
						sqlStringCommand = this.database.GetSqlStringCommand("Delete from  Hishop_Coupon_Products WHERE CouponId = @CouponId");
						this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
						this.database.ExecuteNonQuery(sqlStringCommand);
						result = true;
					}
					else
					{
						string text = "";
						if (productIDs.Count > 0)
						{
							foreach (string current in productIDs)
							{
								text = text + "," + current;
							}
						}
						text = text.Substring(1);
						System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Delete from  Hishop_Coupon_Products WHERE CouponId ={0} and ProductId in ( {1} )", couponId, text));
						this.database.ExecuteNonQuery(sqlStringCommand);
						StringBuilder stringBuilder = new StringBuilder();
						foreach (string current2 in productIDs)
						{
							stringBuilder.AppendFormat(" insert into Hishop_Coupon_Products(couponId , ProductId) values({0} , {1}) ", couponDetails.CouponId, current2);
						}
						sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
						this.database.ExecuteNonQuery(sqlStringCommand);
						sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Coupon_Coupons set IsAllProduct=@IsAllProduct , ProductNumber=(select count(productId) from Hishop_Coupon_Products  where CouponId = @CouponId) WHERE CouponId = @CouponId");
						this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
						this.database.AddInParameter(sqlStringCommand, "IsAllProduct", System.Data.DbType.Boolean, couponDetails.IsAllProduct);
						this.database.ExecuteNonQuery(sqlStringCommand);
						result = true;
					}
				}
				else
				{
					result = false;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool AddCouponProducts(int couponId, int productID)
		{
			bool result;
			try
			{
				CouponInfo couponDetails = this.GetCouponDetails(couponId);
				if (couponDetails != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendFormat(" insert into Hishop_Coupon_Products(couponId , ProductId) values({0} , {1}) ", couponDetails.CouponId, productID);
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
					this.database.ExecuteNonQuery(sqlStringCommand);
					sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Coupon_Coupons set IsAllProduct=0 , ProductNumber=(select count(productId) from Hishop_Coupon_Products  where CouponId = @CouponId) WHERE CouponId = @CouponId");
					this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
					this.database.ExecuteNonQuery(sqlStringCommand);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool DeleteProducts(int couponId, string ProductIds)
		{
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Delete from  Hishop_Coupon_Products WHERE CouponId ={0} and ProductId in ( {1} )", couponId, ProductIds.ReplaceSingleQuoteMark()));
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public bool SetProductsStatus(int couponId, int status, string ProductIds)
		{
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Update Hishop_Coupon_Products set status={0}   WHERE CouponId ={1} and ProductId in ( {2} )", status, couponId, ProductIds.ReplaceSingleQuoteMark()));
				int num = this.database.ExecuteNonQuery(sqlStringCommand);
				result = (num > 0);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public System.Data.DataTable GetCoupon(decimal orderAmount)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Name, ClaimCode,Amount,DiscountValue FROM Hishop_Coupon_Coupons c INNER  JOIN Hishop_CouponItems ci ON ci.CouponId = c.CouponId Where  @DateTime>c.StartTime and @DateTime <c.ClosingTime AND ((Amount>0 and @orderAmount>=Amount) or (Amount=0 and @orderAmount>=DiscountValue))    and  CouponStatus=0  AND UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "DateTime", System.Data.DbType.DateTime, DateTime.UtcNow);
			this.database.AddInParameter(sqlStringCommand, "orderAmount", System.Data.DbType.Decimal, orderAmount);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, Globals.GetCurrentMemberUserId());
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public DbQueryResult GetNewCoupons(Pagination page)
		{
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Coupon_Coupons", "CouponId", string.Empty, "*");
		}

		public DbQueryResult GetCouponsList(CouponItemInfoQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.CouponId.HasValue)
			{
				stringBuilder.AppendFormat("CouponId = {0}", query.CouponId.Value);
			}
			if (!string.IsNullOrEmpty(query.CounponName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("Name = '{0}'", query.CounponName);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("Orderid='{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			if (query.CouponStatus.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" CouponStatus={0} ", query.CouponStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CouponInfo", "ClaimCode", stringBuilder.ToString(), "*");
		}

		public System.Data.DataTable GetCouponsListByIds(int[] CouponIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" CouponId in({0})", string.Join<int>(",", CouponIds));
			return (System.Data.DataTable)DataHelper.PagingByRownumber(1, 1000, "CouponId", SortAction.Desc, false, "Hishop_Coupon_Coupons", "CouponId", stringBuilder.ToString(), "*").Data;
		}

		public System.Data.DataTable GetUserCoupons()
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT m.*,c.IsAllProduct,c.MemberGrades from  Hishop_Coupon_MemberCoupons as m left join Hishop_Coupon_Coupons as c on c.CouponId=m.CouponId WHERE m.MemberId = @UserId and m.BeginDate<=getdate() and getdate()<=m.EndDate and m.Status=0");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, Globals.GetCurrentMemberUserId());
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetCouponByProducts(int couponId, int ProductId)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select  ProductId   from Hishop_Coupon_Products where CouponId=@CouponId and status=0 and ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, ProductId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetUserCoupons(int userId, int useType = 0)
		{
			System.Data.DataTable result = null;
			string str = "";
			if (useType == 1)
			{
				str = "AND ci.CouponStatus = 0 AND ci.UsedTime is NULL and c.ClosingTime > @ClosingTime";
			}
			else if (useType == 2)
			{
				str = " AND ci.UsedTime is not NULL and c.ClosingTime > @ClosingTime";
			}
			else if (useType == 3)
			{
				str = " AND c.ClosingTime<getdate()";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT c.*, ci.ClaimCode,ci.CouponStatus  FROM Hishop_CouponItems ci INNER JOIN Hishop_Coupon_Coupons c ON c.CouponId = ci.CouponId WHERE ci.UserId = @UserId " + str);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(sqlStringCommand, "ClosingTime", System.Data.DbType.DateTime, DateTime.Now);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public bool SendClaimCodes(int couponId, CouponItemInfo couponItem)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_CouponItems(CouponId, ClaimCode,LotNumber, GenerateTime, UserId,UserName,EmailAddress,CouponStatus) VALUES(@CouponId, @ClaimCode,@LotNumber, @GenerateTime, @UserId, @UserName,@EmailAddress,@CouponStatus)");
			this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
			this.database.AddInParameter(sqlStringCommand, "ClaimCode", System.Data.DbType.String, couponItem.ClaimCode);
			this.database.AddInParameter(sqlStringCommand, "GenerateTime", System.Data.DbType.DateTime, couponItem.GenerateTime);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand, "LotNumber", System.Data.DbType.Guid, Guid.NewGuid());
			if (couponItem.UserId.HasValue)
			{
				this.database.SetParameterValue(sqlStringCommand, "UserId", couponItem.UserId.Value);
			}
			else
			{
				this.database.SetParameterValue(sqlStringCommand, "UserId", DBNull.Value);
			}
			if (!string.IsNullOrEmpty(couponItem.UserName))
			{
				this.database.SetParameterValue(sqlStringCommand, "UserName", couponItem.UserName);
			}
			else
			{
				this.database.SetParameterValue(sqlStringCommand, "UserName", DBNull.Value);
			}
			this.database.AddInParameter(sqlStringCommand, "EmailAddress", System.Data.DbType.String, couponItem.EmailAddress);
			this.database.AddInParameter(sqlStringCommand, "CouponStatus", System.Data.DbType.String, 0);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public bool AddCouponUseRecord(OrderInfo orderinfo, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update  Hishop_CouponItems  set userName=@UserName,Userid=@Userid,Orderid=@Orderid,CouponStatus=@CouponStatus,EmailAddress=@EmailAddress,UsedTime=@UsedTime WHERE ClaimCode=@ClaimCode and CouponStatus!=1");
			this.database.AddInParameter(sqlStringCommand, "ClaimCode", System.Data.DbType.String, orderinfo.CouponCode);
			this.database.AddInParameter(sqlStringCommand, "userName", System.Data.DbType.String, orderinfo.Username);
			this.database.AddInParameter(sqlStringCommand, "userid", System.Data.DbType.Int32, orderinfo.UserId);
			this.database.AddInParameter(sqlStringCommand, "CouponStatus", System.Data.DbType.Int32, 1);
			this.database.AddInParameter(sqlStringCommand, "UsedTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "EmailAddress", System.Data.DbType.String, orderinfo.EmailAddress);
			this.database.AddInParameter(sqlStringCommand, "Orderid", System.Data.DbType.String, orderinfo.OrderId);
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

		public SendCouponResult SendCouponToMember(int couponId, int userId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_SendCouponToMember");
			this.database.AddInParameter(storedProcCommand, "@CouponsId", System.Data.DbType.Int32, couponId);
			this.database.AddInParameter(storedProcCommand, "@UserId", System.Data.DbType.Int32, userId);
			this.database.AddOutParameter(storedProcCommand, "@Result", System.Data.DbType.Int32, 4);
			SendCouponResult result;
			try
			{
				this.database.ExecuteNonQuery(storedProcCommand);
				object value = storedProcCommand.Parameters["@Result"].Value;
				if (value != null && !string.IsNullOrEmpty(value.ToString()))
				{
					SendCouponResult sendCouponResult = (SendCouponResult)int.Parse(value.ToString());
					result = sendCouponResult;
					return result;
				}
			}
			catch (Exception)
			{
				throw;
			}
			result = SendCouponResult.其它错误;
			return result;
		}

		public SendCouponResult IsCanSendCouponToMember(int couponId, int userId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_MemberCanReceiveCoupon");
			this.database.AddInParameter(storedProcCommand, "@CouponsId", System.Data.DbType.Int32, couponId);
			this.database.AddInParameter(storedProcCommand, "@UserId", System.Data.DbType.Int32, userId);
			this.database.AddOutParameter(storedProcCommand, "@Result", System.Data.DbType.Int32, 4);
			SendCouponResult result;
			try
			{
				this.database.ExecuteNonQuery(storedProcCommand);
				object value = storedProcCommand.Parameters["@Result"].Value;
				if (value != null && !string.IsNullOrEmpty(value.ToString()))
				{
					SendCouponResult sendCouponResult = (SendCouponResult)int.Parse(value.ToString());
					result = sendCouponResult;
					return result;
				}
			}
			catch (Exception)
			{
				throw;
			}
			result = SendCouponResult.其它错误;
			return result;
		}

		public bool SendCouponToMemebers(int couponId, int adminId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Insert into Hishop_Coupon_MemberCoupons(CouponId,MemberId,ReceiveDate,[Status],CouponName,ConditionValue,BeginDate,EndDate,CouponValue) ");
			stringBuilder.Append("select c.CouponId,UserId as MemberId,GETDATE() as ReceiveDate,0 as [Status], CouponName,ConditionValue,BeginDate,EndDate,CouponValue ");
			stringBuilder.Append(" from  Hishop_Coupon_Coupons as c right join ");
			stringBuilder.AppendFormat(" (select UserId,{0} as CouponId from Hishop_TempSendCouponUserLists where AdminId={1}) as u ", couponId, adminId);
			stringBuilder.AppendFormat(" on c.CouponId = u.CouponId where c.CouponId={0}", couponId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			int num = this.database.ExecuteNonQuery(sqlStringCommand);
			bool result;
			if (num > 0)
			{
				sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Update Hishop_Coupon_Coupons set ReceiveNum=ReceiveNum+{0} where CouponId={1};Delete From Hishop_TempSendCouponUserLists where AdminId={2}; ", num, couponId, adminId));
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public bool SendCouponToMemebers(int couponId, List<int> userIds)
		{
			int num = userIds.Count<int>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Insert into Hishop_Coupon_MemberCoupons(CouponId,MemberId,ReceiveDate,[Status],CouponName,ConditionValue,BeginDate,EndDate,CouponValue) values ");
			CouponInfo couponDetails = this.GetCouponDetails(couponId);
			for (int i = 0; i < num; i++)
			{
				stringBuilder.AppendFormat("({0},{1},GETDATE(),0,'{2}',{3},'{4}','{5}',{6})", new object[]
				{
					couponId,
					userIds[i],
					couponDetails.CouponName,
					couponDetails.ConditionValue,
					couponDetails.BeginDate,
					couponDetails.EndDate,
					couponDetails.CouponValue
				});
				if (i != num - 1)
				{
					stringBuilder.Append(",");
				}
				else
				{
					stringBuilder.Append(";");
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			int num2 = this.database.ExecuteNonQuery(sqlStringCommand);
			bool result;
			if (num2 > 0)
			{
				sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Update Hishop_Coupon_Coupons set ReceiveNum=ReceiveNum+{0} where CouponId={1}; ", num2, couponId));
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public bool SelectCouponWillExpiredOpenId(int DayLimit, ref List<string> SendToUserList)
		{
			string query = string.Format("\r\n                select \r\n                 datediff( day,  GETDATE(), EndDate  ),\r\n                b.OpenId  , a.* \r\n                from Hishop_Coupon_MemberCoupons a\r\n                left join aspnet_Members b on a.MemberId= b.UserId \r\n                where \r\n                 a.Status=0  --未使用过\r\n                and EndDate>= GETDATE()\r\n                and ISNULL(ExpiredPromptTimes,0)=0\r\n                and datediff( day,  GETDATE(), EndDate  ) between 0 and {0}\r\n                and ( b.OpenId<>'' and b.OpenId is not null)\r\n            ", DayLimit);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataTable dataTable = dataSet.Tables[0];
			foreach (System.Data.DataRow dataRow in dataTable.Rows)
			{
				SendToUserList.Add(dataRow["OpenId"].ToString());
			}
			return true;
		}

		public bool SelectCouponWillExpiredList(int DayLimit, ref List<CouponInfo_MemberWeiXin> SendToUserList)
		{
			string query = string.Format("\r\n                select \r\n                 datediff( day,  GETDATE(), a.EndDate  ) as ValidDays,\r\n                b.OpenId, \r\n                c.IsAllProduct,\r\n                a.* from Hishop_Coupon_MemberCoupons a\r\n                left join aspnet_Members b on a.MemberId= b.UserId \r\n                left join Hishop_Coupon_Coupons c on a.CouponId= c.CouponId \r\n                where \r\n                        a.Status=0  --未使用过\r\n                    and a.EndDate>= GETDATE()\r\n                    and ISNULL(ExpiredPromptTimes,0)=0\r\n                    and datediff( day,  GETDATE(), a.EndDate  ) between 0 and {0}\r\n                    and ( b.OpenId<>'' and b.OpenId is not null)\r\n\r\n            ", DayLimit);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataTable dataTable = dataSet.Tables[0];
			foreach (System.Data.DataRow dataRow in dataTable.Rows)
			{
				CouponInfo_MemberWeiXin couponInfo_MemberWeiXin = new CouponInfo_MemberWeiXin();
				couponInfo_MemberWeiXin.CouponName = dataRow["CouponName"].ToString();
				couponInfo_MemberWeiXin.CouponValue = Convert.ToDecimal(dataRow["CouponValue"].ToString());
				couponInfo_MemberWeiXin.ConditionValue = Convert.ToDecimal(dataRow["ConditionValue"].ToString());
				couponInfo_MemberWeiXin.IsAllProduct = (dataRow["IsAllProduct"].ToString() == "1");
				couponInfo_MemberWeiXin.ValidDays = dataRow["ValidDays"].ToString();
				couponInfo_MemberWeiXin.OpenId = dataRow["OpenId"].ToString();
				couponInfo_MemberWeiXin.EndDate = Convert.ToDateTime(dataRow["EndDate"].ToString());
				couponInfo_MemberWeiXin.Id = Convert.ToInt32(dataRow["Id"].ToString());
				SendToUserList.Add(couponInfo_MemberWeiXin);
			}
			return true;
		}

		public bool SaveWeiXinPromptInfo(CouponInfo_MemberWeiXin info)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update  Hishop_Coupon_MemberCoupons  set ExpiredPromptTimes= isnull(ExpiredPromptTimes,0) + 1  where id=" + info.Id.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
