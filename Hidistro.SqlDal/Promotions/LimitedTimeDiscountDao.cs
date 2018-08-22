using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.SqlDal.Promotions
{
	public class LimitedTimeDiscountDao
	{
		private Database database;

		public LimitedTimeDiscountDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool UpdateDiscountStatus(int Id, DiscountStatus status)
		{
			bool result = false;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_LimitedTimeDiscount set Status=@Status WHERE LimitedTimeDiscountId = @Id");
				this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, Id);
				this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, (int)status);
				System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("update Hishop_LimitedTimeDiscountProduct set Status=@Status WHERE LimitedTimeDiscountId = @Id");
				this.database.AddInParameter(sqlStringCommand2, "Id", System.Data.DbType.Int32, Id);
				this.database.AddInParameter(sqlStringCommand2, "Status", System.Data.DbType.Int32, (int)status);
				try
				{
					this.database.ExecuteNonQuery(sqlStringCommand, dbTransaction);
					this.database.ExecuteNonQuery(sqlStringCommand2, dbTransaction);
					dbTransaction.Commit();
					result = true;
				}
				catch
				{
					dbTransaction.Rollback();
					result = false;
				}
				finally
				{
					if (dbTransaction.Connection != null)
					{
						dbConnection.Close();
					}
				}
			}
			return result;
		}

		public bool ChangeDiscountProductStatus(string ids, int status)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[]
			{
				"update Hishop_LimitedTimeDiscountProduct set Status=",
				status,
				" WHERE LimitedTimeDiscountProductId in (",
				ids,
				")"
			}));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteDiscountProduct(string ids)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete Hishop_LimitedTimeDiscountProduct WHERE LimitedTimeDiscountProductId in (" + ids + ")");
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteDiscountProduct(int limitedTimeDiscountId, int prdocutId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete Hishop_LimitedTimeDiscountProduct WHERE LimitedTimeDiscountId =@LimitedTimeDiscountId and ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountId", System.Data.DbType.Int32, limitedTimeDiscountId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, prdocutId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public LimitedTimeDiscountProductInfo GetLimitedTimeDiscountProductByLimitIdAndProductIdAndUserId(int limitedTimeDiscountId, int productId, int userId)
		{
			LimitedTimeDiscountProductInfo result = null;
			LimitedTimeDiscountProductInfo limitedTimeDiscountProductByLimitIdAndProductId = this.GetLimitedTimeDiscountProductByLimitIdAndProductId(limitedTimeDiscountId, productId);
			if (limitedTimeDiscountProductByLimitIdAndProductId != null)
			{
				LimitedTimeDiscountInfo discountInfo = this.GetDiscountInfo(limitedTimeDiscountProductByLimitIdAndProductId.LimitedTimeDiscountId);
				if (discountInfo != null && new MemberDao().CheckCurrentMemberIsInRange(discountInfo.ApplyMembers, discountInfo.DefualtGroup, discountInfo.CustomGroup, userId))
				{
					result = limitedTimeDiscountProductByLimitIdAndProductId;
				}
			}
			return result;
		}

		public string GetLimitedTimeDiscountIdByProductId(int userid, string skuId, int productId)
		{
			int num = 0;
			LimitedTimeDiscountProductInfo limitedTimeDiscountProductByProductId = this.GetLimitedTimeDiscountProductByProductId(productId);
			if (limitedTimeDiscountProductByProductId != null)
			{
				int limitedTimeDiscountId = limitedTimeDiscountProductByProductId.LimitedTimeDiscountId;
				LimitedTimeDiscountInfo discountInfo = new LimitedTimeDiscountDao().GetDiscountInfo(limitedTimeDiscountId);
				if (discountInfo != null)
				{
					bool flag = false;
					if (new MemberDao().CheckCurrentMemberIsInRange(discountInfo.ApplyMembers, discountInfo.DefualtGroup, discountInfo.CustomGroup, userid))
					{
						int limitNumber = discountInfo.LimitNumber;
						if (limitNumber != 0)
						{
							int num2 = limitNumber - new ShoppingCartDao().GetLimitedTimeDiscountUsedNum(limitedTimeDiscountId, skuId, productId, userid, true);
							if (num2 > 0)
							{
								flag = true;
							}
						}
						else
						{
							flag = true;
						}
					}
					if (flag)
					{
						num = limitedTimeDiscountId;
					}
				}
			}
			return num.ToString();
		}

		public LimitedTimeDiscountProductInfo GetLimitedTimeDiscountProductByLimitIdAndProductId(int limitedTimeDiscountId, int productId)
		{
			LimitedTimeDiscountProductInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_LimitedTimeDiscountProduct WHERE LimitedTimeDiscountId = @LimitedTimeDiscountId and ProductId=@ProductId and Status=1 and BeginTime<=getdate() and getdate()< EndTime order by LimitedTimeDiscountProductId desc");
			this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountId", System.Data.DbType.Int32, limitedTimeDiscountId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<LimitedTimeDiscountProductInfo>(dataReader);
			}
			return result;
		}

		public LimitedTimeDiscountProductInfo GetLimitedTimeDiscountProductByProductId(int productId)
		{
			LimitedTimeDiscountProductInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_LimitedTimeDiscountProduct WHERE ProductId=@ProductId and Status=1 and BeginTime<=getdate() and getdate()< EndTime order by LimitedTimeDiscountProductId desc");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<LimitedTimeDiscountProductInfo>(dataReader);
			}
			return result;
		}

		public DbQueryResult GetDiscountQuery(ActivitySearch query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1 ");
			if (query.status != ActivityStatus.All)
			{
				if (query.status == ActivityStatus.In)
				{
					stringBuilder.AppendFormat("and [BeginTime] <= '{0}' and  [EndTime] >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
				else if (query.status == ActivityStatus.End)
				{
					stringBuilder.AppendFormat("and [EndTime] < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
				else if (query.status == ActivityStatus.unBegin)
				{
					stringBuilder.AppendFormat("and [BeginTime] > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
			}
			if (query.begin.HasValue)
			{
				stringBuilder.AppendFormat("and [BeginTime] >= '{0}'", query.begin.Value.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			if (query.end.HasValue)
			{
				stringBuilder.AppendFormat("and [EndTime] <= '{0}'", query.end.Value.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			if (!string.IsNullOrEmpty(query.Name))
			{
				stringBuilder.AppendFormat("and ActivityName like '%{0}%'  ", query.Name);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_hishop_DiscountList", "LimitedTimeDiscountId", stringBuilder.ToString(), "*");
		}

		public int AddLimitedTimeDiscount(LimitedTimeDiscountInfo info)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_LimitedTimeDiscount] ([ActivityName],[BeginTime],[EndTime],[Description],[LimitNumber],[ApplyMembers],[DefualtGroup],[CustomGroup],[CreateTime],[Status]) VALUES (@ActivityName,@BeginTime,@EndTime,@Description,@LimitNumber,@ApplyMembers,@DefualtGroup,@CustomGroup,@CreateTime,@Status); SELECT CAST(scope_identity() AS int);");
			this.database.AddInParameter(sqlStringCommand, "ActivityName", System.Data.DbType.String, info.ActivityName);
			this.database.AddInParameter(sqlStringCommand, "BeginTime", System.Data.DbType.DateTime, info.BeginTime);
			this.database.AddInParameter(sqlStringCommand, "EndTime", System.Data.DbType.DateTime, info.EndTime);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, info.Description);
			this.database.AddInParameter(sqlStringCommand, "LimitNumber", System.Data.DbType.Int32, info.LimitNumber);
			this.database.AddInParameter(sqlStringCommand, "ApplyMembers", System.Data.DbType.String, info.ApplyMembers);
			this.database.AddInParameter(sqlStringCommand, "DefualtGroup", System.Data.DbType.String, info.DefualtGroup);
			this.database.AddInParameter(sqlStringCommand, "CustomGroup", System.Data.DbType.String, info.CustomGroup);
			this.database.AddInParameter(sqlStringCommand, "CreateTime", System.Data.DbType.DateTime, info.CreateTime);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, info.Status);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}

		public bool AddLimitedTimeDiscountProduct(LimitedTimeDiscountProductInfo info)
		{
			this.DeleteDiscountProduct(info.LimitedTimeDiscountId, info.ProductId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_LimitedTimeDiscountProduct] ([LimitedTimeDiscountId],[ProductId],[Discount],[Minus],[IsDehorned],[IsChamferPoint],[FinalPrice],[CreateTime],[BeginTime],[EndTime],[Status]) VALUES (@LimitedTimeDiscountId,@ProductId,@Discount,@Minus,@IsDehorned,@IsChamferPoint,@FinalPrice,@CreateTime,@BeginTime,@EndTime,@Status);");
			this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountId", System.Data.DbType.Int32, info.LimitedTimeDiscountId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, info.ProductId);
			this.database.AddInParameter(sqlStringCommand, "Discount", System.Data.DbType.Decimal, info.Discount);
			this.database.AddInParameter(sqlStringCommand, "Minus", System.Data.DbType.Decimal, info.Minus);
			this.database.AddInParameter(sqlStringCommand, "IsDehorned", System.Data.DbType.Int32, info.IsDehorned);
			this.database.AddInParameter(sqlStringCommand, "IsChamferPoint", System.Data.DbType.Int32, info.IsChamferPoint);
			this.database.AddInParameter(sqlStringCommand, "FinalPrice", System.Data.DbType.Decimal, info.FinalPrice);
			this.database.AddInParameter(sqlStringCommand, "CreateTime", System.Data.DbType.DateTime, info.CreateTime);
			this.database.AddInParameter(sqlStringCommand, "BeginTime", System.Data.DbType.DateTime, info.BeginTime);
			this.database.AddInParameter(sqlStringCommand, "EndTime", System.Data.DbType.DateTime, info.EndTime);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, info.Status);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateLimitedTimeDiscountProduct(LimitedTimeDiscountProductInfo info)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  [Hishop_LimitedTimeDiscountProduct] SET   [Discount]=@Discount,[Minus]=@Minus,[IsDehorned]=@IsDehorned,[IsChamferPoint]=@IsChamferPoint,[FinalPrice]=@FinalPrice   WHERE LimitedTimeDiscountId=@LimitedTimeDiscountId and ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "Discount", System.Data.DbType.Decimal, info.Discount);
			this.database.AddInParameter(sqlStringCommand, "Minus", System.Data.DbType.Decimal, info.Minus);
			this.database.AddInParameter(sqlStringCommand, "IsDehorned", System.Data.DbType.Int32, info.IsDehorned);
			this.database.AddInParameter(sqlStringCommand, "IsChamferPoint", System.Data.DbType.Int32, info.IsChamferPoint);
			this.database.AddInParameter(sqlStringCommand, "FinalPrice", System.Data.DbType.Decimal, info.FinalPrice);
			this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountId", System.Data.DbType.Int32, info.LimitedTimeDiscountId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, info.ProductId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateLimitedTimeDiscountProductById(LimitedTimeDiscountProductInfo info)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  [Hishop_LimitedTimeDiscountProduct] SET   [Discount]=@Discount,[Minus]=@Minus,[IsDehorned]=@IsDehorned,[IsChamferPoint]=@IsChamferPoint,[FinalPrice]=@FinalPrice   WHERE LimitedTimeDiscountProductId=@LimitedTimeDiscountProductId");
			this.database.AddInParameter(sqlStringCommand, "Discount", System.Data.DbType.Decimal, info.Discount);
			this.database.AddInParameter(sqlStringCommand, "Minus", System.Data.DbType.Decimal, info.Minus);
			this.database.AddInParameter(sqlStringCommand, "IsDehorned", System.Data.DbType.Int32, info.IsDehorned);
			this.database.AddInParameter(sqlStringCommand, "IsChamferPoint", System.Data.DbType.Int32, info.IsChamferPoint);
			this.database.AddInParameter(sqlStringCommand, "FinalPrice", System.Data.DbType.Decimal, info.FinalPrice);
			this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountProductId", System.Data.DbType.Int32, info.LimitedTimeDiscountProductId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateLimitedTimeDiscount(LimitedTimeDiscountInfo info)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  [Hishop_LimitedTimeDiscount] SET   [ActivityName]=@ActivityName,[BeginTime]=@BeginTime,[EndTime]=@EndTime,[Description]=@Description,[LimitNumber]=@LimitNumber,[ApplyMembers]=@ApplyMembers,[DefualtGroup]=@DefualtGroup,[CustomGroup]=@CustomGroup,[CreateTime]=@CreateTime,[Status]=@Status  WHERE LimitedTimeDiscountId=@LimitedTimeDiscountId");
			this.database.AddInParameter(sqlStringCommand, "ActivityName", System.Data.DbType.String, info.ActivityName);
			this.database.AddInParameter(sqlStringCommand, "BeginTime", System.Data.DbType.DateTime, info.BeginTime);
			this.database.AddInParameter(sqlStringCommand, "EndTime", System.Data.DbType.DateTime, info.EndTime);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, info.Description);
			this.database.AddInParameter(sqlStringCommand, "LimitNumber", System.Data.DbType.Int32, info.LimitNumber);
			this.database.AddInParameter(sqlStringCommand, "ApplyMembers", System.Data.DbType.String, info.ApplyMembers);
			this.database.AddInParameter(sqlStringCommand, "DefualtGroup", System.Data.DbType.String, info.DefualtGroup);
			this.database.AddInParameter(sqlStringCommand, "CustomGroup", System.Data.DbType.String, info.CustomGroup);
			this.database.AddInParameter(sqlStringCommand, "CreateTime", System.Data.DbType.DateTime, info.CreateTime);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, info.Status);
			this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountId", System.Data.DbType.Int32, info.LimitedTimeDiscountId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public LimitedTimeDiscountInfo GetDiscountInfo(int Id)
		{
			LimitedTimeDiscountInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_LimitedTimeDiscount WHERE LimitedTimeDiscountId = @ID and Status!=2");
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, Id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<LimitedTimeDiscountInfo>(dataReader);
			}
			return result;
		}

		public LimitedTimeDiscountProductInfo GetDiscountProductInfoByProductId(int productId)
		{
			LimitedTimeDiscountProductInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_LimitedTimeDiscountProduct WHERE ProductId = @productId and Status=1 and BeginTime <=GETDATE() and EndTime>=GETDATE() ");
			this.database.AddInParameter(sqlStringCommand, "productId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<LimitedTimeDiscountProductInfo>(dataReader);
			}
			return result;
		}

		public LimitedTimeDiscountProductInfo GetDiscountProductInfoById(int id)
		{
			LimitedTimeDiscountProductInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_LimitedTimeDiscountProduct WHERE LimitedTimeDiscountProductId = @LimitedTimeDiscountProductId");
			this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountProductId", System.Data.DbType.Int32, id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<LimitedTimeDiscountProductInfo>(dataReader);
			}
			return result;
		}

		public DbQueryResult GetDiscountProduct(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			stringBuilder.AppendFormat(" AND SaleStatus <> ({0})", 0);
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				int num = 1;
				while (num < array.Length && num <= 4)
				{
					stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
					num++;
				}
			}
			if (query.CategoryId.HasValue)
			{
				if (query.CategoryId.Value > 0)
				{
					stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%') ", query.MaiCategoryPath);
				}
				else
				{
					stringBuilder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
				}
			}
			string selectFields = "SaleCounts,ThumbnailUrl60,ThumbnailUrl310,ProductId, ProductCode,IsMakeTaobao,ProductName,ProductShortName, ThumbnailUrl40, MarketPrice, ShortDescription,MinShowPrice,MaxShowPrice,SkuNum,SalePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,SaleStatus,AddedDate,ActivityName,LimitedTimeDiscountId,productws,Discount,Minus,FinalPrice";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_DiscountProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public DbQueryResult GetDiscountProducted(ProductQuery query, int discountId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			stringBuilder.AppendFormat(" AND SaleStatus <> ({0})", 0);
			stringBuilder.AppendFormat(" AND LimitedTimeDiscountId = ({0})", discountId);
			stringBuilder.AppendFormat(" AND Status!= 2", new object[0]);
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				int num = 1;
				while (num < array.Length && num <= 4)
				{
					stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
					num++;
				}
			}
			if (query.CategoryId.HasValue)
			{
				if (query.CategoryId.Value > 0)
				{
					stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%') ", query.MaiCategoryPath);
				}
				else
				{
					stringBuilder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
				}
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_DiscountProducted p", "ProductId", stringBuilder.ToString(), "*");
		}
	}
}
