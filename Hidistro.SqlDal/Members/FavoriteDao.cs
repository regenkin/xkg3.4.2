using Hidistro.Core;
using Hidistro.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Members
{
	public class FavoriteDao
	{
		private Database database;

		public FavoriteDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool AddProductToFavorite(int productId, int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Favorite(ProductId, UserId, Tags, Remark)VALUES(@ProductId, @UserId, '', '')");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ExistsProduct(int productId, int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_Favorite WHERE UserId=@UserId AND ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public int UpdateFavorite(int favoriteId, string tags, string remark)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Favorite SET Tags = @Tags, Remark = @Remark WHERE FavoriteId = @FavoriteId");
			this.database.AddInParameter(sqlStringCommand, "Tags", System.Data.DbType.String, tags);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, remark);
			this.database.AddInParameter(sqlStringCommand, "FavoriteId", System.Data.DbType.Int32, favoriteId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int DeleteFavorite(int favoriteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Favorite WHERE FavoriteId = @FavoriteId");
			this.database.AddInParameter(sqlStringCommand, "FavoriteId", System.Data.DbType.Int32, favoriteId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public System.Data.DataTable GetFavorites(MemberInfo member)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT a.*, b.ProductName, b.ThumbnailUrl60, b.MarketPrice,b.ShortDescription,");
			if (member != null)
			{
				int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = b.SkuId AND GradeId = {0}) = 1", member.GradeId);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = b.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
			}
			else
			{
				stringBuilder.Append("SalePrice");
			}
			stringBuilder.AppendFormat(" FROM Hishop_Favorite a left join vw_Hishop_BrowseProductList b on a.ProductId = b.ProductId WHERE a.UserId={0} ORDER BY a.FavoriteId DESC", member.UserId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public bool CheckHasCollect(int memberId, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT COUNT(1)");
			stringBuilder.AppendFormat(" FROM Hishop_Favorite WHERE UserId={0} AND ProductId ={1} ", memberId, productId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			int num = (int)this.database.ExecuteScalar(sqlStringCommand);
			return num > 0;
		}

		public bool DeleteFavorites(string ids)
		{
			string query = "DELETE from Hishop_Favorite WHERE FavoriteId IN (" + ids + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
