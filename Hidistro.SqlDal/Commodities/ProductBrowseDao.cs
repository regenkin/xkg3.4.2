using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.SqlDal.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
	public class ProductBrowseDao
	{
		private Database database;

		public ProductBrowseDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public ProductInfo GetProduct(MemberInfo member, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (member != null)
			{
				int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
			}
			else
			{
				stringBuilder.Append("SalePrice");
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId =@ProductId;SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice, " + stringBuilder.ToString() + " FROM Hishop_SKUs s WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			ProductInfo productInfo = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					productInfo = DataMapper.PopulateProduct(dataReader);
				}
				if (dataReader.NextResult())
				{
					while (dataReader.Read())
					{
						productInfo.Skus.Add((string)dataReader["SkuId"], DataMapper.PopulateSKU(dataReader));
					}
				}
			}
			return productInfo;
		}

		public System.Data.DataTable GetAllFull(int ActivitiesType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType,ActivitiesDescription from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 and (ActivitiesType=0 or ActivitiesType=" + ActivitiesType + ")  order by MeetMoney asc");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public System.Data.DataTable GetActivitie(int ActivitiesId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType,ActivitiesDescription from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0  and ActivitiesId=" + ActivitiesId + "  order by MeetMoney asc");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public System.Data.DataTable GetActiviOne(int ActivitiesType, decimal MeetMoney)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(new object[]
			{
				"select top 1 ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 and (ActivitiesType=0 or ActivitiesType=",
				ActivitiesType,
				") and MeetMoney<=",
				MeetMoney,
				"  order by MeetMoney desc"
			}));
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public new System.Data.DataTable GetType()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select distinct ActivitiesType  from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public System.Data.DataTable GetProducts(MemberInfo member, int? topicId, int? categoryId, int distributorId, string keyWord, int pageNumber, int maxNum, out int toal, string sort, bool isAsc = false, string pIds = "", bool isLimitedTimeDiscountId = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,MaxShowPrice,", maxNum);
			stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,");
			if (member != null)
			{
				int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = vw_Hishop_BrowseProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = vw_Hishop_BrowseProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
			}
			else
			{
				stringBuilder.Append("SalePrice");
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(" SaleStatus=1");
			if (categoryId.HasValue)
			{
				CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
				if (category != null)
				{
					stringBuilder2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
				}
			}
			if (!string.IsNullOrEmpty(keyWord))
			{
				stringBuilder2.AppendFormat(" AND (ProductName LIKE '%{0}%')", keyWord);
			}
			if (!string.IsNullOrEmpty(pIds))
			{
				string arg = pIds.Replace("_", ",");
				stringBuilder2.AppendFormat(" AND ProductId IN ({0})", arg);
			}
			if (isLimitedTimeDiscountId)
			{
				stringBuilder2.Append(" AND ProductId in(select ProductId from Hishop_LimitedTimeDiscountProduct where BeginTime<getdate() and getdate()<EndTime and Status=1)");
			}
			if (string.IsNullOrWhiteSpace(sort))
			{
				sort = "ProductId";
			}
			DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Hishop_BrowseProductList", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
			System.Data.DataTable result = (System.Data.DataTable)dbQueryResult.Data;
			toal = dbQueryResult.TotalRecords;
			return result;
		}

		public int GetProductsNumber(int distributorId, bool isselect = true)
		{
			int result = 0;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("select count(ProductId) as num from vw_Hishop_BrowseProductList where  SaleStatus=1 and ProductId ", new object[0]);
			if (!isselect)
			{
				stringBuilder.AppendFormat("NOT", new object[0]);
			}
			stringBuilder.AppendFormat(" IN(select ProductId from Hishop_DistributorProducts where UserId={0} )", distributorId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(), out result);
			return result;
		}

		public System.Data.DataTable GetProducts(MemberInfo member, int? topicId, int? categoryId, int distributorId, string keyWord, int pageNumber, int maxNum, out int toal, string sort, bool isAsc = false, bool isselect = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", maxNum);
			stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,");
			if (member != null)
			{
				int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = vw_Hishop_BrowseProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = vw_Hishop_BrowseProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
			}
			else
			{
				stringBuilder.Append("SalePrice");
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(" SaleStatus=1");
			if (categoryId.HasValue)
			{
				CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
				if (category != null)
				{
					stringBuilder2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
				}
			}
			if (!string.IsNullOrEmpty(keyWord))
			{
				stringBuilder2.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", keyWord);
			}
			if (isselect)
			{
				stringBuilder2.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", distributorId);
			}
			else
			{
				stringBuilder2.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", distributorId);
			}
			if (string.IsNullOrWhiteSpace(sort))
			{
				sort = "ProductId";
			}
			DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Hishop_BrowseProductList", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
			System.Data.DataTable result = (System.Data.DataTable)dbQueryResult.Data;
			toal = dbQueryResult.TotalRecords;
			return result;
		}

		public System.Data.DataTable GetBrandProducts(MemberInfo member, int? brandId, int pageNumber, int maxNum, out int total)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,ShowSaleCounts,");
			stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,");
			if (member != null)
			{
				int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
			}
			else
			{
				stringBuilder.Append("SalePrice");
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(" SaleStatus=1");
			if (brandId.HasValue)
			{
				stringBuilder2.AppendFormat(" AND BrandId = {0}", brandId);
			}
			DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, "DisplaySequence", SortAction.Desc, true, "vw_Hishop_BrowseProductList s", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
			System.Data.DataTable result = (System.Data.DataTable)dbQueryResult.Data;
			total = dbQueryResult.TotalRecords;
			return result;
		}
	}
}
