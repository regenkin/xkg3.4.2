using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Orders;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Comments
{
	public class ProductReviewDao
	{
		private Database database;

		public ProductReviewDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public DbQueryResult GetProductReviews(ProductReviewQuery reviewQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.Keywords));
			if (!string.IsNullOrEmpty(reviewQuery.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.ProductCode));
			}
			if (reviewQuery.productId > 0)
			{
				stringBuilder.AppendFormat(" AND ProductId = {0}", reviewQuery.productId);
			}
			if (reviewQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND (CategoryId = {0}", reviewQuery.CategoryId.Value);
				stringBuilder.AppendFormat(" OR  CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '%'))", reviewQuery.CategoryId.Value);
			}
			return DataHelper.PagingByRownumber(reviewQuery.PageIndex, reviewQuery.PageSize, reviewQuery.SortBy, reviewQuery.SortOrder, reviewQuery.IsCount, "vw_Hishop_ProductReviews", "ProductId", stringBuilder.ToString(), "*");
		}

		public int GetProductReviewsCount(int productId)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT count(1) FROM Hishop_ProductReviews WHERE ProductId =" + productId);
			return (int)this.database.ExecuteScalar(System.Data.CommandType.Text, stringBuilder.ToString());
		}

		public ProductReviewInfo GetProductReview(int reviewId)
		{
			ProductReviewInfo result = new ProductReviewInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductReviews WHERE ReviewId=@ReviewId");
			this.database.AddInParameter(sqlStringCommand, "ReviewId", System.Data.DbType.Int32, reviewId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = ReaderConvert.ReaderToModel<ProductReviewInfo>(dataReader);
				}
			}
			return result;
		}

		public bool InsertProductReview(ProductReviewInfo review)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductReviews (ProductId, UserId, ReviewText, UserName, UserEmail, ReviewDate,OrderID,SkuID,OrderItemID) VALUES(@ProductId, @UserId, @ReviewText, @UserName, @UserEmail, @ReviewDate,@OrderID,@SkuID,@OrderItemID)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, review.ProductId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, review.UserId);
			this.database.AddInParameter(sqlStringCommand, "ReviewText", System.Data.DbType.String, review.ReviewText);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, review.UserName);
			this.database.AddInParameter(sqlStringCommand, "UserEmail", System.Data.DbType.String, review.UserEmail);
			this.database.AddInParameter(sqlStringCommand, "ReviewDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "OrderID", System.Data.DbType.String, review.OrderId);
			this.database.AddInParameter(sqlStringCommand, "SkuID", System.Data.DbType.String, review.SkuId);
			this.database.AddInParameter(sqlStringCommand, "OrderItemID", System.Data.DbType.Int32, review.OrderItemID);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int DeleteProductReview(long reviewId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductReviews WHERE ReviewId = @ReviewId");
			this.database.AddInParameter(sqlStringCommand, "ReviewId", System.Data.DbType.Int64, reviewId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public LineItemInfo GetLatestOrderItemByProductIDAndUserID(int productid, int userid)
		{
			string query = string.Format(string.Concat(new object[]
			{
				"select top 1 a.* from Hishop_OrderItems a left join Hishop_ProductReviews b on a.skuid= b.skuid and a.orderid=b.orderid left join Hishop_Orders c on a.orderid=c.orderid where c.userid=",
				userid,
				" and a.productid=",
				productid,
				" and a.OrderItemsStatus = {0} and b.orderid is null order by a.Id desc"
			}), 5);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			LineItemInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<LineItemInfo>(dataReader);
			}
			return result;
		}

		public int GetWaitCommentByUserID(int userid)
		{
			string query = string.Format("select count(0) from Hishop_OrderItems a left join Hishop_ProductReviews b on a.skuid= b.skuid and a.orderid=b.orderid left join Hishop_Orders c on a.orderid=c.orderid where c.userid=" + userid + "  and a.OrderItemsStatus = {0} and b.orderid is null", 5);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
		}

		public IList<LineItemInfo> GetOrderItemsList(int userid)
		{
			string query = string.Format("select a.* from Hishop_OrderItems a left join Hishop_ProductReviews b on a.skuid= b.skuid and a.orderid=b.orderid left join Hishop_Orders c on a.orderid=c.orderid where c.userid=" + userid + " and  a.OrderItemsStatus = {0} and b.orderid is null order by a.Id desc", 5);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			IList<LineItemInfo> result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<LineItemInfo>(dataReader);
			}
			return result;
		}

		public DbQueryResult GetOrderMemberComment(OrderQuery query, int userId, int orderItemsStatus)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (userId > 0)
			{
				stringBuilder.Append(" UserId=" + userId);
			}
			if (orderItemsStatus > 0)
			{
				stringBuilder.Append(" AND OrderItemsStatus=" + orderItemsStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_OrderMmemberComment", "Id", stringBuilder.ToString(), "*");
		}

		public bool IsReview(string orderid, string skuid, int productid, int userid)
		{
			string query = string.Empty;
			bool result;
			if (!string.IsNullOrEmpty(skuid) && !string.IsNullOrEmpty(orderid))
			{
				query = "SELECT COUNT(0) FROM Hishop_ProductReviews WHERE OrderID=@OrderID AND SkuId=@SkuId AND SkuId is not null";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productid);
				this.database.AddInParameter(sqlStringCommand, "OrderID", System.Data.DbType.String, orderid);
				this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuid);
				object s = this.database.ExecuteScalar(sqlStringCommand);
				bool flag = Globals.ToNum(s) > 0;
				result = flag;
			}
			else
			{
				result = (userid > 0 && this.GetLatestOrderItemByProductIDAndUserID(productid, userid) == null);
			}
			return result;
		}

		public void LoadProductReview(int productId, int userId, out int buyNum, out int reviewNum)
		{
			buyNum = 0;
			reviewNum = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_ProductReviews WHERE ProductId=@ProductId AND UserId = @UserId SELECT ISNULL(SUM(Quantity), 0) FROM Hishop_OrderItems WHERE ProductId=@ProductId AND OrderId IN" + string.Format(" (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId AND OrderStatus = {0})", 5));
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					reviewNum = (int)dataReader[0];
				}
				dataReader.NextResult();
				if (dataReader.Read())
				{
					buyNum = (int)dataReader[0];
				}
			}
		}
	}
}
