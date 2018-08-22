using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Caching;

namespace Hidistro.SaleSystem.Vshop
{
	public static class CategoryBrowser
	{
		private const string MainCategoriesCachekey = "DataCache-Categories";

		public static IList<CategoryInfo> GetMaxSubCategories(int parentCategoryId, int maxNum = 1000)
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			System.Data.DataTable categories = CategoryBrowser.GetCategories();
			System.Data.DataRow[] array = categories.Select("ParentCategoryId = " + parentCategoryId);
			int num = 0;
			while (num < maxNum && num < array.Length)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[num]));
				num++;
			}
			return list;
		}

		public static IList<CategoryInfo> GetMaxMainCategories(int maxNum = 1000)
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			System.Data.DataTable categories = CategoryBrowser.GetCategories();
			System.Data.DataRow[] array = categories.Select("Depth = 1");
			int num = 0;
			while (num < maxNum && num < array.Length)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[num]));
				num++;
			}
			return list;
		}

		public static System.Data.DataTable GetCategories()
		{
			System.Data.DataTable dataTable = HiCache.Get("DataCache-Categories") as System.Data.DataTable;
			if (dataTable == null)
			{
				dataTable = new CategoryDao().GetCategories();
				HiCache.Insert("DataCache-Categories", dataTable, 360, CacheItemPriority.Normal);
			}
			return dataTable;
		}

		public static System.Data.DataTable GetAllCategories()
		{
			return new CategoryDao().GetCategories();
		}

		public static System.Data.DataSet GetCategoryList()
		{
			System.Data.DataSet dataSet = HiCache.Get("DataCache-CategoryList") as System.Data.DataSet;
			if (dataSet == null)
			{
				dataSet = new CategoryDao().GetCategoryList();
				HiCache.Insert("DataCache-CategoryList", dataSet, 360, CacheItemPriority.Normal);
			}
			return dataSet;
		}

		public static CategoryInfo GetCategory(int categoryId)
		{
			return new CategoryDao().GetCategory(categoryId);
		}

		public static System.Data.DataTable GetBrandCategories()
		{
			return new BrandCategoryDao().GetBrandCategories();
		}

		public static BrandCategoryInfo GetBrandCategory(int brandId)
		{
			return new BrandCategoryDao().GetBrandCategory(brandId);
		}
	}
}
