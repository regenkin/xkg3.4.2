using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
	public class BrandCategoryDao
	{
		private Database database;

		public BrandCategoryDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public int AddBrandCategory(BrandCategoryInfo brandCategory)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_BrandCategories;INSERT INTO Hishop_BrandCategories(BrandName, Logo, CompanyUrl,RewriteName,MetaKeywords,MetaDescription, Description, DisplaySequence) VALUES(@BrandName, @Logo, @CompanyUrl,@RewriteName,@MetaKeywords,@MetaDescription, @Description, @DisplaySequence); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "BrandName", System.Data.DbType.String, brandCategory.BrandName);
			this.database.AddInParameter(sqlStringCommand, "Logo", System.Data.DbType.String, brandCategory.Logo);
			this.database.AddInParameter(sqlStringCommand, "CompanyUrl", System.Data.DbType.String, brandCategory.CompanyUrl);
			this.database.AddInParameter(sqlStringCommand, "RewriteName", System.Data.DbType.String, brandCategory.RewriteName);
			this.database.AddInParameter(sqlStringCommand, "MetaKeywords", System.Data.DbType.String, brandCategory.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "MetaDescription", System.Data.DbType.String, brandCategory.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, brandCategory.Description);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public void AddBrandProductTypes(int brandId, IList<int> productTypes)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandId);
			foreach (int current in productTypes)
			{
				this.database.SetParameterValue(sqlStringCommand, "ProductTypeId", current);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}

		public bool DeleteBrandProductTypes(int brandId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypeBrands WHERE BrandId=@BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandId);
			bool result;
			try
			{
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public System.Data.DataTable GetBrandCategories(string brandName)
		{
			string text = "1=1";
			if (!string.IsNullOrEmpty(brandName))
			{
				text = text + " AND BrandName LIKE '%" + DataHelper.CleanSearchString(brandName) + "%'";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories  WHERE " + text + " ORDER BY DisplaySequence");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetBrandCategories()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories ORDER BY DisplaySequence");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public BrandCategoryInfo GetBrandCategory(int brandId)
		{
			BrandCategoryInfo brandCategoryInfo = new BrandCategoryInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories WHERE BrandId = @BrandId;SELECT * FROM Hishop_ProductTypeBrands WHERE BrandId = @BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				brandCategoryInfo = ReaderConvert.ReaderToModel<BrandCategoryInfo>(dataReader);
				IList<int> list = new List<int>();
				dataReader.NextResult();
				while (dataReader.Read())
				{
					list.Add((int)dataReader["ProductTypeId"]);
				}
				brandCategoryInfo.ProductTypes = list;
			}
			return brandCategoryInfo;
		}

		public bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_BrandCategories SET BrandName = @BrandName, Logo = @Logo, CompanyUrl = @CompanyUrl,RewriteName=@RewriteName,MetaKeywords=@MetaKeywords,MetaDescription=@MetaDescription, Description = @Description WHERE BrandId = @BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandCategory.BrandId);
			this.database.AddInParameter(sqlStringCommand, "BrandName", System.Data.DbType.String, brandCategory.BrandName);
			this.database.AddInParameter(sqlStringCommand, "Logo", System.Data.DbType.String, brandCategory.Logo);
			this.database.AddInParameter(sqlStringCommand, "CompanyUrl", System.Data.DbType.String, brandCategory.CompanyUrl);
			this.database.AddInParameter(sqlStringCommand, "RewriteName", System.Data.DbType.String, brandCategory.RewriteName);
			this.database.AddInParameter(sqlStringCommand, "MetaKeywords", System.Data.DbType.String, brandCategory.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "MetaDescription", System.Data.DbType.String, brandCategory.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, brandCategory.Description);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool BrandHvaeProducts(int brandId)
		{
			bool result = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(ProductName) FROM Hishop_Products Where BrandId=@BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = (dataReader.GetInt32(0) > 0);
				}
			}
			return result;
		}

		public DbQueryResult Query(BrandQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1 ");
			if (!string.IsNullOrEmpty(query.Name))
			{
				stringBuilder.AppendFormat("and BrandName like '%{0}%'  ", query.Name);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_BrandCategories", "BrandId", stringBuilder.ToString(), "*");
		}

		public bool DeleteBrandCategory(int brandId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_BrandCategories WHERE BrandId = @BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void UpdateBrandCategoryDisplaySequence(int brandId, SortAction action)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_BrandCategory_DisplaySequence");
			this.database.AddInParameter(storedProcCommand, "BrandId", System.Data.DbType.Int32, brandId);
			this.database.AddInParameter(storedProcCommand, "Sort", System.Data.DbType.Int32, action);
			this.database.ExecuteNonQuery(storedProcCommand);
		}

		public bool UpdateBrandCategoryDisplaySequence(int brandId, int displaysequence)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_BrandCategories set DisplaySequence=@DisplaySequence where BrandId=@BrandId");
			this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", System.Data.DbType.Int32, displaysequence);
			this.database.AddInParameter(sqlStringCommand, "@BrandId", System.Data.DbType.Int32, brandId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetBrandCategoryThemes(int brandid, string themeName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_BrandCategories set Theme = @Theme where BrandId = @BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandid);
			this.database.AddInParameter(sqlStringCommand, "Theme", System.Data.DbType.String, themeName);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
	}
}
