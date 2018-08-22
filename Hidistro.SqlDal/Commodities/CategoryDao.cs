using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
	public class CategoryDao
	{
		private Database database;

		public CategoryDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public System.Data.DataTable GetCategories()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CategoryId,Name,IconUrl,DisplaySequence,ParentCategoryId,Depth,[Path],RewriteName,HasChildren,FirstCommission,SecondCommission,ThirdCommission FROM Hishop_Categories ORDER BY DisplaySequence");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataSet GetCategoryList()
		{
			string text = "select * from Hishop_Categories where ParentCategoryId=0 order by DisplaySequence asc,CategoryId asc ;";
			text += "select * from Hishop_Categories where ParentCategoryId<>0 order by DisplaySequence asc,CategoryId asc;";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataColumn parentColumn = dataSet.Tables[0].Columns["CategoryId"];
			System.Data.DataColumn childColumn = dataSet.Tables[1].Columns["ParentCategoryId"];
			System.Data.DataRelation relation = new System.Data.DataRelation("SubCategories", parentColumn, childColumn);
			dataSet.Relations.Add(relation);
			return dataSet;
		}

		public DbQueryResult Query(CategoriesQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (!string.IsNullOrEmpty(query.Name))
			{
				stringBuilder.AppendFormat("and Name like '%{0}%'  ", query.Name);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, 10000, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Categories", "VoteId", stringBuilder.ToString(), "*");
		}

		public CategoryInfo GetCategory(int categoryId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Categories WHERE CategoryId =@CategoryId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			CategoryInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<CategoryInfo>(dataReader);
			}
			return result;
		}

		public int CreateCategory(CategoryInfo category)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Category_Create");
			this.database.AddOutParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, category.Name);
			this.database.AddInParameter(storedProcCommand, "SKUPrefix", System.Data.DbType.String, category.SKUPrefix);
			this.database.AddInParameter(storedProcCommand, "DisplaySequence", System.Data.DbType.Int32, category.DisplaySequence);
			if (!string.IsNullOrEmpty(category.IconUrl))
			{
				this.database.AddInParameter(storedProcCommand, "IconUrl", System.Data.DbType.String, category.IconUrl);
			}
			if (!string.IsNullOrEmpty(category.MetaTitle))
			{
				this.database.AddInParameter(storedProcCommand, "Meta_Title", System.Data.DbType.String, category.MetaTitle);
			}
			if (!string.IsNullOrEmpty(category.MetaDescription))
			{
				this.database.AddInParameter(storedProcCommand, "Meta_Description", System.Data.DbType.String, category.MetaDescription);
			}
			if (!string.IsNullOrEmpty(category.MetaKeywords))
			{
				this.database.AddInParameter(storedProcCommand, "Meta_Keywords", System.Data.DbType.String, category.MetaKeywords);
			}
			if (!string.IsNullOrEmpty(category.Notes1))
			{
				this.database.AddInParameter(storedProcCommand, "Notes1", System.Data.DbType.String, category.Notes1);
			}
			if (!string.IsNullOrEmpty(category.Notes2))
			{
				this.database.AddInParameter(storedProcCommand, "Notes2", System.Data.DbType.String, category.Notes2);
			}
			if (!string.IsNullOrEmpty(category.Notes3))
			{
				this.database.AddInParameter(storedProcCommand, "Notes3", System.Data.DbType.String, category.Notes3);
			}
			if (!string.IsNullOrEmpty(category.Notes4))
			{
				this.database.AddInParameter(storedProcCommand, "Notes4", System.Data.DbType.String, category.Notes4);
			}
			if (!string.IsNullOrEmpty(category.Notes5))
			{
				this.database.AddInParameter(storedProcCommand, "Notes5", System.Data.DbType.String, category.Notes5);
			}
			if (category.ParentCategoryId.HasValue)
			{
				this.database.AddInParameter(storedProcCommand, "ParentCategoryId", System.Data.DbType.Int32, category.ParentCategoryId.Value);
			}
			else
			{
				this.database.AddInParameter(storedProcCommand, "ParentCategoryId", System.Data.DbType.Int32, 0);
			}
			if (category.AssociatedProductType.HasValue)
			{
				this.database.AddInParameter(storedProcCommand, "AssociatedProductType", System.Data.DbType.Int32, category.AssociatedProductType.Value);
			}
			if (!string.IsNullOrEmpty(category.RewriteName))
			{
				this.database.AddInParameter(storedProcCommand, "RewriteName", System.Data.DbType.String, category.RewriteName);
			}
			this.database.AddInParameter(storedProcCommand, "FirstCommission", System.Data.DbType.String, category.FirstCommission);
			this.database.AddInParameter(storedProcCommand, "SecondCommission", System.Data.DbType.String, category.SecondCommission);
			this.database.AddInParameter(storedProcCommand, "ThirdCommission", System.Data.DbType.String, category.ThirdCommission);
			this.database.ExecuteNonQuery(storedProcCommand);
			return (int)this.database.GetParameterValue(storedProcCommand, "CategoryId");
		}

		public CategoryActionStatus UpdateCategory(CategoryInfo category)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Categories SET [Name] = @Name, SKUPrefix = @SKUPrefix,AssociatedProductType = @AssociatedProductType, Meta_Title=@Meta_Title,Meta_Description = @Meta_Description, IconUrl = @IconUrl,Meta_Keywords = @Meta_Keywords, Notes1 = @Notes1, Notes2 = @Notes2, Notes3 = @Notes3,  Notes4 = @Notes4, Notes5 = @Notes5, RewriteName = @RewriteName,FirstCommission=@FirstCommission,SecondCommission=@SecondCommission,ThirdCommission=@ThirdCommission WHERE CategoryId = @CategoryId;UPDATE Hishop_Categories SET FirstCommission=@FirstCommission,SecondCommission=@SecondCommission,ThirdCommission=@ThirdCommission WHERE Path like '%" + category.CategoryId + "|%'");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, category.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, category.Name);
			this.database.AddInParameter(sqlStringCommand, "SKUPrefix", System.Data.DbType.String, category.SKUPrefix);
			this.database.AddInParameter(sqlStringCommand, "AssociatedProductType", System.Data.DbType.Int32, category.AssociatedProductType);
			this.database.AddInParameter(sqlStringCommand, "Meta_Title", System.Data.DbType.String, category.MetaTitle);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", System.Data.DbType.String, category.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "IconUrl", System.Data.DbType.String, category.IconUrl);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", System.Data.DbType.String, category.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "Notes1", System.Data.DbType.String, category.Notes1);
			this.database.AddInParameter(sqlStringCommand, "Notes2", System.Data.DbType.String, category.Notes2);
			this.database.AddInParameter(sqlStringCommand, "Notes3", System.Data.DbType.String, category.Notes3);
			this.database.AddInParameter(sqlStringCommand, "Notes4", System.Data.DbType.String, category.Notes4);
			this.database.AddInParameter(sqlStringCommand, "Notes5", System.Data.DbType.String, category.Notes5);
			this.database.AddInParameter(sqlStringCommand, "RewriteName", System.Data.DbType.String, category.RewriteName);
			this.database.AddInParameter(sqlStringCommand, "FirstCommission", System.Data.DbType.String, category.FirstCommission);
			this.database.AddInParameter(sqlStringCommand, "SecondCommission", System.Data.DbType.String, category.SecondCommission);
			this.database.AddInParameter(sqlStringCommand, "ThirdCommission", System.Data.DbType.String, category.ThirdCommission);
			return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1) ? CategoryActionStatus.Success : CategoryActionStatus.UnknowError;
		}

		public bool IsExitProduct(string CategoryId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(ProductId) FROM Hishop_Products WHERE CategoryId = @CategoryId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.String, CategoryId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public bool DeleteCategory(int categoryId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Category_Delete");
			this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			return this.database.ExecuteNonQuery(storedProcCommand) > 0;
		}

		public int DisplaceCategory(int oldCategoryId, int newCategory)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId=@newCategory, MainCategoryPath=(SELECT Path FROM Hishop_Categories WHERE CategoryId=@newCategory)+'|' WHERE CategoryId=@oldCategoryId");
			this.database.AddInParameter(sqlStringCommand, "oldCategoryId", System.Data.DbType.Int32, oldCategoryId);
			this.database.AddInParameter(sqlStringCommand, "newCategory", System.Data.DbType.Int32, newCategory);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool SwapCategorySequence(int categoryId, int displaysequence)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Categories  set DisplaySequence=@DisplaySequence where CategoryId=@CategoryId");
			this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", System.Data.DbType.Int32, displaysequence);
			this.database.AddInParameter(sqlStringCommand, "@CategoryId", System.Data.DbType.Int32, categoryId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetProductExtendCategory(int productId, string extendCategoryPath)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET ExtendCategoryPath = @ExtendCategoryPath WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "ExtendCategoryPath", System.Data.DbType.String, extendCategoryPath);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool SetCategoryThemes(int categoryId, string themeName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Categories SET Theme = @Theme WHERE CategoryId = @CategoryId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			this.database.AddInParameter(sqlStringCommand, "Theme", System.Data.DbType.String, themeName);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
	}
}
