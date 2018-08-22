using Hidistro.Core;
using Hidistro.Core.Entities;
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
	public class HomeProductDao
	{
		private Database database;

		public HomeProductDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public DbQueryResult GetProducts(ProductQuery query)
		{
			query.IsIncludeHomeProduct = new bool?(false);
			return new ProductDao().GetProducts(query);
		}

		public bool AddHomeProdcut(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_HomeProducts(ProductId,DisplaySequence) VALUES (@ProductId,0)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			bool result;
			try
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public bool RemoveHomeProduct(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_HomeProducts WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool RemoveAllHomeProduct()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_HomeProducts");
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public System.Data.DataTable GetHomeProducts()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select p.ProductId, ProductCode, ProductName,ShortDescription,ThumbnailUrl40,ThumbnailUrl160,ThumbnailUrl100,MarketPrice, SalePrice,ShowSaleCounts,SaleCounts, Stock,t.DisplaySequence from vw_Hishop_BrowseProductList p inner join  Vshop_HomeProducts t on p.productid=t.ProductId ");
			stringBuilder.AppendFormat(" and SaleStatus = {0}", 1);
			stringBuilder.Append(" order by t.DisplaySequence asc");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public System.Data.DataTable GetAllFull()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select  * from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 order by MeetMoney asc    ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DbQueryResult GetHomeProducts(MemberInfo member, ProductQuery query, bool isdistributor)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int currentDistributorId = Globals.GetCurrentDistributorId();
			stringBuilder.Append("MainCategoryPath,ProductId, ProductCode,ShortDescription,ProductName,ShowSaleCounts, ThumbnailUrl60,ThumbnailUrl40,ThumbnailUrl100,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310, MarketPrice,");
			if (member != null)
			{
				int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", member.GradeId);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice, ", member.GradeId, discount);
			}
			else
			{
				stringBuilder.Append("SalePrice,");
			}
			stringBuilder.Append("SaleCounts, Stock");
			StringBuilder stringBuilder2 = new StringBuilder(" SaleStatus =" + 1);
			if (query.CategoryId > 0)
			{
				stringBuilder2.AppendFormat(" and CategoryId={0}", query.CategoryId);
			}
			if (isdistributor && currentDistributorId > 0)
			{
				stringBuilder2.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", currentDistributorId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
		}

		public bool UpdateHomeProductSequence(int ProductId, int displaysequence)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Vshop_HomeProducts  set DisplaySequence=@DisplaySequence where ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", System.Data.DbType.Int32, displaysequence);
			this.database.AddInParameter(sqlStringCommand, "@ProductId", System.Data.DbType.Int32, ProductId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
