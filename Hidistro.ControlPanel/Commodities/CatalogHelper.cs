using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace Hidistro.ControlPanel.Commodities
{
	public sealed class CatalogHelper
	{
		private const string CategoriesCachekey = "DataCache-Categories";

		private CatalogHelper()
		{
		}

		public static IList<CategoryInfo> GetMainCategories()
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			System.Data.DataTable categories = CatalogHelper.GetCategories();
			System.Data.DataRow[] array = categories.Select("Depth = 1");
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
			}
			return list;
		}

		public static IList<CategoryInfo> GetSubCategories(int parentCategoryId)
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			string filterExpression = "ParentCategoryId = " + parentCategoryId.ToString(CultureInfo.InvariantCulture);
			System.Data.DataTable categories = CatalogHelper.GetCategories();
			System.Data.DataRow[] array = categories.Select(filterExpression);
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
			}
			return list;
		}

		public static CategoryInfo GetCategory(int categoryId)
		{
			return new CategoryDao().GetCategory(categoryId);
		}

		public static string GetFullCategory(int categoryId)
		{
			CategoryInfo category = CatalogHelper.GetCategory(categoryId);
			string result;
			if (category == null)
			{
				result = null;
			}
			else
			{
				string text = category.Name;
				while (category != null && category.ParentCategoryId.HasValue)
				{
					category = CatalogHelper.GetCategory(category.ParentCategoryId.Value);
					if (category != null)
					{
						text = category.Name + " &raquo; " + text;
					}
				}
				result = text;
			}
			return result;
		}

		public static DbQueryResult Query(CategoriesQuery query)
		{
			return new CategoryDao().Query(query);
		}

		public static System.Data.DataTable GetCategories()
		{
			System.Data.DataTable dataTable = HiCache.Get("DataCache-Categories") as System.Data.DataTable;
			if (null == dataTable)
			{
				dataTable = new CategoryDao().GetCategories();
				HiCache.Insert("DataCache-Categories", dataTable, 360, CacheItemPriority.Normal);
			}
			return dataTable;
		}

		public static IList<CategoryInfo> GetSequenceCategories()
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			IList<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
			foreach (CategoryInfo current in mainCategories)
			{
				list.Add(current);
				CatalogHelper.LoadSubCategorys(current.CategoryId, list);
			}
			return list;
		}

		private static void LoadSubCategorys(int parentCategoryId, IList<CategoryInfo> categories)
		{
			IList<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(parentCategoryId);
			if (subCategories != null && subCategories.Count > 0)
			{
				foreach (CategoryInfo current in subCategories)
				{
					categories.Add(current);
					CatalogHelper.LoadSubCategorys(current.CategoryId, categories);
				}
			}
		}

		public static CategoryActionStatus AddCategory(CategoryInfo category)
		{
			CategoryActionStatus result;
			if (null == category)
			{
				result = CategoryActionStatus.UnknowError;
			}
			else
			{
				Globals.EntityCoding(category, true);
				int num = new CategoryDao().CreateCategory(category);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.AddProductCategory, string.Format(CultureInfo.InvariantCulture, "创建了一个新的店铺分类:”{0}”", new object[]
					{
						category.Name
					}));
					HiCache.Remove("DataCache-Categories");
				}
				result = CategoryActionStatus.Success;
			}
			return result;
		}

		public static CategoryActionStatus UpdateCategory(CategoryInfo category)
		{
			CategoryActionStatus result;
			if (null == category)
			{
				result = CategoryActionStatus.UnknowError;
			}
			else
			{
				Globals.EntityCoding(category, true);
				CategoryActionStatus categoryActionStatus = new CategoryDao().UpdateCategory(category);
				if (categoryActionStatus == CategoryActionStatus.Success)
				{
					EventLogs.WriteOperationLog(Privilege.EditProductCategory, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的店铺分类", new object[]
					{
						category.CategoryId
					}));
					HiCache.Remove("DataCache-Categories");
				}
				result = categoryActionStatus;
			}
			return result;
		}

		public static bool SwapCategorySequence(int categoryId, int displaysequence)
		{
			return new CategoryDao().SwapCategorySequence(categoryId, displaysequence);
		}

		public static bool IsExitProduct(string CategoryId)
		{
			return new CategoryDao().IsExitProduct(CategoryId);
		}

		public static bool DeleteCategory(int categoryId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProductCategory);
			bool flag = new CategoryDao().DeleteCategory(categoryId);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.DeleteProductCategory, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的店铺分类", new object[]
				{
					categoryId
				}));
				HiCache.Remove("DataCache-Categories");
			}
			return flag;
		}

		public static int DisplaceCategory(int oldCategoryId, int newCategory)
		{
			return new CategoryDao().DisplaceCategory(oldCategoryId, newCategory);
		}

		public static bool SetProductExtendCategory(int productId, string extendCategoryPath)
		{
			return new CategoryDao().SetProductExtendCategory(productId, extendCategoryPath);
		}

		public static bool SetCategoryThemes(int categoryId, string themeName)
		{
			bool flag = new CategoryDao().SetCategoryThemes(categoryId, themeName);
			if (flag)
			{
				HiCache.Remove("DataCache-Categories");
			}
			return false;
		}

		public static bool AddBrandCategory(BrandCategoryInfo brandCategory)
		{
			int num = new BrandCategoryDao().AddBrandCategory(brandCategory);
			bool result;
			if (num <= 0)
			{
				result = false;
			}
			else
			{
				if (brandCategory.ProductTypes.Count > 0)
				{
					new BrandCategoryDao().AddBrandProductTypes(num, brandCategory.ProductTypes);
				}
				result = true;
			}
			return result;
		}

		public static System.Data.DataTable GetBrandCategories()
		{
			return new BrandCategoryDao().GetBrandCategories();
		}

		public static DbQueryResult GetBrandQuery(BrandQuery query)
		{
			return new BrandCategoryDao().Query(query);
		}

		public static BrandCategoryInfo GetBrandCategory(int brandId)
		{
			return new BrandCategoryDao().GetBrandCategory(brandId);
		}

		public static bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
		{
			bool flag = new BrandCategoryDao().UpdateBrandCategory(brandCategory);
			if (flag && new BrandCategoryDao().DeleteBrandProductTypes(brandCategory.BrandId))
			{
				new BrandCategoryDao().AddBrandProductTypes(brandCategory.BrandId, brandCategory.ProductTypes);
			}
			return flag;
		}

		public static bool BrandHvaeProducts(int brandId)
		{
			return new BrandCategoryDao().BrandHvaeProducts(brandId);
		}

		public static bool DeleteBrandCategory(int brandId)
		{
			return new BrandCategoryDao().DeleteBrandCategory(brandId);
		}

		public static void UpdateBrandCategorieDisplaySequence(int brandId, SortAction action)
		{
			new BrandCategoryDao().UpdateBrandCategoryDisplaySequence(brandId, action);
		}

		public static bool UpdateBrandCategoryDisplaySequence(int barndId, int displaysequence)
		{
			return new BrandCategoryDao().UpdateBrandCategoryDisplaySequence(barndId, displaysequence);
		}

		public static string UploadBrandCategorieImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
			{
				result = string.Empty;
			}
			else
			{
				string text = Globals.GetStoragePath() + "/brand/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}

		public static bool SetBrandCategoryThemes(int brandid, string themeName)
		{
			bool flag = new BrandCategoryDao().SetBrandCategoryThemes(brandid, themeName);
			if (flag)
			{
				HiCache.Remove("DataCache-Categories");
			}
			return flag;
		}

		public static System.Data.DataTable GetBrandCategories(string brandName)
		{
			return new BrandCategoryDao().GetBrandCategories(brandName);
		}

		public static System.Data.DataTable GetTags()
		{
			return new TagDao().GetTags();
		}

		public static string GetTagName(int tagId)
		{
			return new TagDao().GetTagName(tagId);
		}

		public static int AddTags(string tagName)
		{
			int result = 0;
			if (new TagDao().GetTags(tagName) <= 0)
			{
				result = new TagDao().AddTags(tagName);
			}
			return result;
		}

		public static bool UpdateTags(int tagId, string tagName)
		{
			bool result = false;
			int tags = new TagDao().GetTags(tagName);
			if (tags == tagId || tags <= 0)
			{
				result = new TagDao().UpdateTags(tagId, tagName);
			}
			return result;
		}

		public static bool DeleteTags(int tagId)
		{
			return new TagDao().DeleteTags(tagId);
		}

		public static bool AddProductTags(int productId, IList<int> tagsId, System.Data.Common.DbTransaction dbtran)
		{
			return new TagDao().AddProductTags(productId, tagsId, dbtran);
		}

		public static bool DeleteProductTags(int productId, System.Data.Common.DbTransaction tran)
		{
			return new TagDao().DeleteProductTags(productId, tran);
		}
	}
}
