using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.SqlDal.Commodities
{
	public class ProductDao
	{
		private Database database;

		public ProductDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public DbQueryResult GetProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (string.IsNullOrEmpty(query.TwoSaleStatus))
			{
				if (query.SaleStatus != ProductSaleStatus.All)
				{
					if (query.SaleStatus == ProductSaleStatus.UnSale)
					{
						stringBuilder.AppendFormat(" AND Stock=0", 0);
					}
					else
					{
						stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
					}
				}
				else
				{
					stringBuilder.AppendFormat(" AND SaleStatus <> ({0})", 0);
				}
			}
			else
			{
				stringBuilder.AppendFormat(" AND (SaleStatus = 2 or SaleStatus=3) AND (Stock > 0 )", new object[0]);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
			}
			if (query.Stock.HasValue)
			{
				stringBuilder.AppendFormat(" AND (Stock = 0 or Stock is null )", new object[0]);
			}
			if (query.TagId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
			}
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
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
			{
				stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
			}
			if (query.IsIncludeHomeProduct.HasValue)
			{
				if (!query.IsIncludeHomeProduct.Value)
				{
					stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Vshop_HomeProducts)");
				}
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
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
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND convert(date,AddedDate) >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND convert(date,AddedDate)<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.minPrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND salePrice >={0}", query.minPrice.Value);
			}
			if (query.maxPrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND salePrice <={0}", query.maxPrice.Value);
			}
			if (!string.IsNullOrEmpty(query.selectQuery))
			{
				stringBuilder.AppendFormat(" AND {0}", query.selectQuery);
			}
			string selectFields = "SaleCounts,ThumbnailUrl60,ThumbnailUrl310,ProductId, ProductCode,IsMakeTaobao,ProductName,ProductShortName, ThumbnailUrl40, MarketPrice, ShortDescription,MaxShowPrice,SalePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,SaleStatus,AddedDate";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public DbQueryResult GetProductsImgList(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
			}
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
			if (!string.IsNullOrEmpty(query.MaiCategoryPath) && query.MaiCategoryPath != "0")
			{
				stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%') ", query.MaiCategoryPath);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND convert(date,AddedDate) >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND convert(date,AddedDate)<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			string selectFields = "Img,ProductId,ProductName,SaleStatus,MainCategoryPath,AddedDate";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductImgList", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public DbQueryResult GetProductsFromGroup(ProductQuery query, string productIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (string.IsNullOrEmpty(query.TwoSaleStatus))
			{
				if (query.SaleStatus != ProductSaleStatus.All)
				{
					stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
				}
				else
				{
					stringBuilder.AppendFormat(" AND SaleStatus <> ({0})", 0);
				}
			}
			else
			{
				stringBuilder.AppendFormat(" AND (SaleStatus = 2 or SaleStatus=3)", new object[0]);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
			}
			if (query.Stock.HasValue)
			{
				stringBuilder.AppendFormat(" AND (Stock = 0 or Stock is null )", new object[0]);
			}
			if (query.TagId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
			}
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
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
			{
				stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
			}
			if (query.IsIncludeHomeProduct.HasValue)
			{
				if (!query.IsIncludeHomeProduct.Value)
				{
					stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Vshop_HomeProducts)");
				}
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
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
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.minPrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND salePrice >={0}", query.minPrice.Value);
			}
			if (query.maxPrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND salePrice <={0}", query.maxPrice.Value);
			}
			if (!string.IsNullOrEmpty(query.selectQuery))
			{
				stringBuilder.AppendFormat(" AND {0}", query.selectQuery);
			}
			if (!string.IsNullOrEmpty(productIds))
			{
				stringBuilder.AppendFormat(" AND ProductId in ({0})", productIds.TrimEnd(new char[]
				{
					','
				}));
			}
			else
			{
				stringBuilder.AppendFormat(" AND ProductId =0", new object[0]);
			}
			string selectFields = "SaleCounts,ThumbnailUrl60,ProductId, ProductCode,IsMakeTaobao,ProductName,ProductShortName, ThumbnailUrl40, MarketPrice, ShortDescription,SalePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,SaleStatus,AddedDate";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public System.Data.DataTable GetProducts(string products)
		{
			string query = "select * from [Hishop_Products] where ProductId in (" + products + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetGroupBuyProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" WHERE SaleStatus = {0}", 1);
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
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND MainCategoryPath LIKE '{0}|%'", query.MaiCategoryPath);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId,ProductName FROM Hishop_Products" + stringBuilder.ToString());
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetTopProductOrder(int top, string showOrder)
		{
			string query = string.Concat(new object[]
			{
				" SELECT TOP ",
				top,
				" * FROM (SELECT P.*,(SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice,(SELECT TOP 1 SkuId FROM Hishop_SKUs WHERE ProductId = p.ProductId ORDER BY SalePrice) AS SkuId,(SELECT SUM(Stock) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Stock,(SELECT TOP 1 [Weight] FROM Hishop_SKUs WHERE ProductId = p.ProductId ORDER BY SalePrice) AS [Weight],(SELECT COUNT(*) FROM Taobao_Products WHERE ProductId = p.ProductId) AS IsMakeTaobao FROM Hishop_Products p) as A  WHERE A.SaleStatus=1 AND A.Stock > 0 ORDER BY ",
				showOrder
			});
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public decimal GetProductSalePrice(int productId)
		{
			string commandText = string.Format("SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = {0}", productId);
			return (decimal)this.database.ExecuteScalar(System.Data.CommandType.Text, commandText);
		}

		public long GetProductSumStock(int productId)
		{
			string commandText = string.Format("SELECT   ISNULL( sum(Stock),0)   FROM vw_Hishop_BrowseProductList  WHERE ProductId = {0}", productId);
			return Convert.ToInt64(this.database.ExecuteScalar(System.Data.CommandType.Text, commandText));
		}

		public ProductInfo GetBrowseProductListByView(int productId)
		{
			ProductInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_BrowseProductList WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ProductInfo>(dataReader);
			}
			return result;
		}

		public ProductInfo GetProductDetails(int productId)
		{
			ProductInfo productInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId = @ProductId;SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU, skus.SalePrice, skus.CostPrice, skus.Stock, skus.[Weight] FROM Hishop_SKUItems s right outer join Hishop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId = @ProductId ORDER BY (SELECT DisplaySequence FROM Hishop_Attributes WHERE AttributeId = s.AttributeId) DESC;SELECT s.SkuId, smp.GradeId, smp.MemberSalePrice FROM Hishop_SKUMemberPrice smp INNER JOIN Hishop_SKUs s ON smp.SkuId=s.SkuId WHERE s.ProductId=@ProductId;SELECT AttributeId, ValueId FROM Hishop_ProductAttributes WHERE ProductId = @ProductId;SELECT * FROM Hishop_ProductTag WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				productInfo = ReaderConvert.ReaderToModel<ProductInfo>(dataReader);
				if (productInfo != null)
				{
					dataReader.NextResult();
					while (dataReader.Read())
					{
						string key = (string)dataReader["SkuId"];
						if (!productInfo.Skus.ContainsKey(key))
						{
							productInfo.Skus.Add(key, DataMapper.PopulateSKU(dataReader));
						}
						if (dataReader["AttributeId"] != DBNull.Value && dataReader["ValueId"] != DBNull.Value)
						{
							productInfo.Skus[key].SkuItems.Add((int)dataReader["AttributeId"], (int)dataReader["ValueId"]);
						}
					}
					dataReader.NextResult();
					while (dataReader.Read())
					{
						string key = (string)dataReader["SkuId"];
						productInfo.Skus[key].MemberPrices.Add((int)dataReader["GradeId"], (decimal)dataReader["MemberSalePrice"]);
					}
				}
			}
			return productInfo;
		}

		public Dictionary<int, IList<int>> GetProductAttributes(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT AttributeId, ValueId FROM Hishop_ProductAttributes WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			Dictionary<int, IList<int>> dictionary = new Dictionary<int, IList<int>>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					int key = (int)dataReader["AttributeId"];
					int item = (int)dataReader["ValueId"];
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, new List<int>
						{
							item
						});
					}
					else
					{
						dictionary[key].Add(item);
					}
				}
			}
			return dictionary;
		}

		public IList<int> GetProductTags(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTag WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			IList<int> list = new List<int>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)dataReader["TagId"]);
				}
			}
			return list;
		}

		public IList<ProductInfo> GetProducts(IList<int> productIds, bool Resort = false)
		{
			IList<ProductInfo> list = new List<ProductInfo>();
			string text = "(";
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int i = 0; i < productIds.Count; i++)
			{
				text = text + productIds[i] + ",";
				if (!dictionary.ContainsKey(productIds[i]))
				{
					dictionary.Add(productIds[i], i);
				}
			}
			IList<ProductInfo> result;
			if (text.Length <= 1)
			{
				result = list;
			}
			else
			{
				text = text.Substring(0, text.Length - 1) + ")";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM (SELECT P.*,\r\n                (SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice,\r\n                (SELECT TOP 1 SkuId FROM Hishop_SKUs WHERE ProductId = p.ProductId ORDER BY SalePrice) AS SkuId,\r\n                (SELECT SUM(Stock) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Stock,\r\n                (SELECT TOP 1 [Weight] FROM Hishop_SKUs WHERE ProductId = p.ProductId ORDER BY SalePrice) AS [Weight],\r\n                (SELECT COUNT(*) FROM Taobao_Products WHERE ProductId = p.ProductId) AS IsMakeTaobao\r\n                FROM Hishop_Products p)  as A\r\n                 WHERE A.ProductId IN " + text + " AND A.Stock > 0 AND A.SaleStatus=1");
				System.Data.DataTable dataTable = null;
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					IEnumerator enumerator;
					if (Resort)
					{
						dataTable.Columns.Add("Sort", typeof(int));
						enumerator = dataTable.Rows.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								System.Data.DataRow dataRow = (System.Data.DataRow)enumerator.Current;
								if (dictionary.ContainsKey((int)dataRow["ProductId"]))
								{
									dataRow["Sort"] = dictionary[(int)dataRow["ProductId"]];
								}
								else
								{
									dataRow["Sort"] = 99999;
								}
							}
						}
						finally
						{
							IDisposable disposable = enumerator as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
						System.Data.DataView defaultView = dataTable.DefaultView;
						defaultView.Sort = "Sort Asc";
						dataTable = defaultView.ToTable();
					}
					enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							System.Data.DataRow dataRow = (System.Data.DataRow)enumerator.Current;
							list.Add(DataMapper.PopulateProduct(dataRow));
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				result = list;
			}
			return result;
		}

		public IList<int> GetProductIds(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT p.ProductId FROM Hishop_Products p WHERE p.SaleStatus = {0}", (int)query.SaleStatus);
			if (!string.IsNullOrEmpty(query.ProductCode) && query.ProductCode.Length > 0)
			{
				stringBuilder.AppendFormat(" AND LOWER(p.ProductCode) LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				stringBuilder.AppendFormat(" AND LOWER(p.ProductName) LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
			}
			if (query.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND (p.CategoryId = {0}  OR  p.CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '|%'))", query.CategoryId.Value);
			}
			IList<int> list = new List<int>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)dataReader["ProductId"]);
				}
			}
			return list;
		}

		public DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			if (query.IncludeOnSales)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 1);
			}
			if (query.IncludeUnSales)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 2);
			}
			if (query.IncludeInStock)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 3);
			}
			stringBuilder.Remove(stringBuilder.Length - 4, 4);
			stringBuilder.Append(")");
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao != -1)
			{
				stringBuilder.AppendFormat(" AND IsMakeTaobao={0}  ", query.IsMakeTaobao);
			}
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
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value.AddDays(1.0)));
			}
			if (!string.IsNullOrEmpty(removeProductIds))
			{
				stringBuilder.AppendFormat(" AND ProductId NOT IN ({0})", removeProductIds);
			}
			string selectFields = "ProductId, ProductCode, ProductName,ProductShortName, ThumbnailUrl40, MarketPrice, SalePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public System.Data.DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT a.[ProductId], [TypeId], [ProductName], [ProductCode], [ShortDescription], [Unit], [Description], ").Append("[SaleStatus], [ImageUrl1], [ImageUrl2], [ImageUrl3], ").Append("[ImageUrl4], [ImageUrl5], [MarketPrice], [HasSKU] ").Append("FROM Hishop_Products a  left join Taobao_Products b on a.productid=b.productid WHERE ");
			stringBuilder.Append("(");
			if (query.IncludeOnSales)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 1);
			}
			if (query.IncludeUnSales)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 2);
			}
			if (query.IncludeInStock)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 3);
			}
			stringBuilder.Remove(stringBuilder.Length - 4, 4);
			stringBuilder.Append(")");
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao != -1)
			{
				if (query.IsMakeTaobao == 1)
				{
					stringBuilder.AppendFormat(" AND a.ProductId IN (SELECT ProductId FROM Taobao_Products)", new object[0]);
				}
				else
				{
					stringBuilder.AppendFormat(" AND a.ProductId NOT IN (SELECT ProductId FROM Taobao_Products)", new object[0]);
				}
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
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
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (!string.IsNullOrEmpty(removeProductIds))
			{
				stringBuilder.AppendFormat(" AND a.ProductId NOT IN ({0})", removeProductIds);
			}
			stringBuilder.AppendFormat(" ORDER BY {0} {1}", query.SortBy, query.SortOrder);
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_GetExportList");
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, stringBuilder.ToString());
			return this.database.ExecuteDataSet(storedProcCommand);
		}

		public System.Data.DataTable GetProductNum()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Select (SELECT count(ProductId) FROM Hishop_Products where SaleStatus=1)OnSale,(SELECT count(ProductId) FROM vw_Hishop_BrowseProductList where (SaleStatus=2 or SaleStatus=3) AND (Stock > 0 ))OnStock,(SELECT count(ProductId) FROM vw_Hishop_BrowseProductList where (Stock=0 or Stock is null) and SaleStatus<>0)Zero");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public void EnsureMapping(System.Data.DataSet mappingSet)
		{
			using (System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO  Hishop_ProductTypes (TypeName, Remark) VALUES(@TypeName, @Remark);SELECT @@IDENTITY;"))
			{
				this.database.AddInParameter(sqlStringCommand, "TypeName", System.Data.DbType.String);
				this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String);
				System.Data.DataRow[] array = mappingSet.Tables["types"].Select("SelectedTypeId=0");
				System.Data.DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow = array2[i];
					this.database.SetParameterValue(sqlStringCommand, "TypeName", dataRow["TypeName"]);
					this.database.SetParameterValue(sqlStringCommand, "Remark", dataRow["Remark"]);
					dataRow["SelectedTypeId"] = this.database.ExecuteScalar(sqlStringCommand);
				}
			}
			using (System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage)  VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage);SELECT @@IDENTITY;"))
			{
				this.database.AddInParameter(sqlStringCommand2, "AttributeName", System.Data.DbType.String);
				this.database.AddInParameter(sqlStringCommand2, "TypeId", System.Data.DbType.Int32);
				this.database.AddInParameter(sqlStringCommand2, "UsageMode", System.Data.DbType.Int32);
				this.database.AddInParameter(sqlStringCommand2, "UseAttributeImage", System.Data.DbType.Boolean);
				System.Data.DataRow[] array3 = mappingSet.Tables["attributes"].Select("SelectedAttributeId=0");
				System.Data.DataRow[] array2 = array3;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow2 = array2[i];
					int num = (int)mappingSet.Tables["types"].Select(string.Format("MappedTypeId={0}", dataRow2["MappedTypeId"]))[0]["SelectedTypeId"];
					this.database.SetParameterValue(sqlStringCommand2, "AttributeName", dataRow2["AttributeName"]);
					this.database.SetParameterValue(sqlStringCommand2, "TypeId", num);
					this.database.SetParameterValue(sqlStringCommand2, "UsageMode", int.Parse(dataRow2["UsageMode"].ToString()));
					this.database.SetParameterValue(sqlStringCommand2, "UseAttributeImage", bool.Parse(dataRow2["UseAttributeImage"].ToString()));
					dataRow2["SelectedAttributeId"] = this.database.ExecuteScalar(sqlStringCommand2);
				}
			}
			using (System.Data.Common.DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues;INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl);SELECT @@IDENTITY;"))
			{
				this.database.AddInParameter(sqlStringCommand3, "AttributeId", System.Data.DbType.Int32);
				this.database.AddInParameter(sqlStringCommand3, "ValueStr", System.Data.DbType.String);
				this.database.AddInParameter(sqlStringCommand3, "ImageUrl", System.Data.DbType.String);
				System.Data.DataRow[] array4 = mappingSet.Tables["values"].Select("SelectedValueId=0");
				System.Data.DataRow[] array2 = array4;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow3 = array2[i];
					int num2 = (int)mappingSet.Tables["attributes"].Select(string.Format("MappedAttributeId={0}", dataRow3["MappedAttributeId"]))[0]["SelectedAttributeId"];
					this.database.SetParameterValue(sqlStringCommand3, "AttributeId", num2);
					this.database.SetParameterValue(sqlStringCommand3, "ValueStr", dataRow3["ValueStr"]);
					this.database.SetParameterValue(sqlStringCommand3, "ImageUrl", dataRow3["ImageUrl"]);
					dataRow3["SelectedValueId"] = this.database.ExecuteScalar(sqlStringCommand3);
				}
			}
			mappingSet.AcceptChanges();
		}

		public bool AddProductMinPriceAndMaxPrice(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" update Hishop_Products set MinShowPrice=(select isnull(min(SalePrice),0) from Hishop_SKUs where ProductId=Hishop_Products.ProductId),MaxShowPrice=(select isnull(max(SalePrice),0) from Hishop_SKUs where ProductId=Hishop_Products.ProductId) where ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int AddProduct(ProductInfo product, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_Create");
			if (product.FreightTemplateId < 1)
			{
				product.IsfreeShipping = true;
			}
			this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, product.CategoryId);
			this.database.AddInParameter(storedProcCommand, "MainCategoryPath", System.Data.DbType.String, product.MainCategoryPath);
			this.database.AddInParameter(storedProcCommand, "TypeId", System.Data.DbType.Int32, product.TypeId);
			this.database.AddInParameter(storedProcCommand, "ProductName", System.Data.DbType.String, product.ProductName);
			this.database.AddInParameter(storedProcCommand, "ProductShortName", System.Data.DbType.String, product.ProductShortName);
			this.database.AddInParameter(storedProcCommand, "ProductCode", System.Data.DbType.String, product.ProductCode);
			this.database.AddInParameter(storedProcCommand, "ShortDescription", System.Data.DbType.String, product.ShortDescription);
			this.database.AddInParameter(storedProcCommand, "Unit", System.Data.DbType.String, product.Unit);
			this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, product.Description);
			this.database.AddInParameter(storedProcCommand, "SaleStatus", System.Data.DbType.Int32, (int)product.SaleStatus);
			this.database.AddInParameter(storedProcCommand, "AddedDate", System.Data.DbType.DateTime, product.AddedDate);
			this.database.AddInParameter(storedProcCommand, "ImageUrl1", System.Data.DbType.String, product.ImageUrl1);
			this.database.AddInParameter(storedProcCommand, "ImageUrl2", System.Data.DbType.String, product.ImageUrl2);
			this.database.AddInParameter(storedProcCommand, "ImageUrl3", System.Data.DbType.String, product.ImageUrl3);
			this.database.AddInParameter(storedProcCommand, "ImageUrl4", System.Data.DbType.String, product.ImageUrl4);
			this.database.AddInParameter(storedProcCommand, "ImageUrl5", System.Data.DbType.String, product.ImageUrl5);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl40", System.Data.DbType.String, product.ThumbnailUrl40);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl60", System.Data.DbType.String, product.ThumbnailUrl60);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl100", System.Data.DbType.String, product.ThumbnailUrl100);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl160", System.Data.DbType.String, product.ThumbnailUrl160);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl180", System.Data.DbType.String, product.ThumbnailUrl180);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl220", System.Data.DbType.String, product.ThumbnailUrl220);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl310", System.Data.DbType.String, product.ThumbnailUrl310);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl410", System.Data.DbType.String, product.ThumbnailUrl410);
			this.database.AddInParameter(storedProcCommand, "MarketPrice", System.Data.DbType.Currency, product.MarketPrice);
			this.database.AddInParameter(storedProcCommand, "BrandId", System.Data.DbType.Int32, product.BrandId);
			this.database.AddInParameter(storedProcCommand, "HasSKU", System.Data.DbType.Boolean, product.HasSKU);
			this.database.AddInParameter(storedProcCommand, "IsfreeShipping", System.Data.DbType.Boolean, product.IsfreeShipping);
			this.database.AddInParameter(storedProcCommand, "TaobaoProductId", System.Data.DbType.Int64, product.TaobaoProductId);
			this.database.AddInParameter(storedProcCommand, "ShowSaleCounts", System.Data.DbType.Int32, product.ShowSaleCounts);
			this.database.AddOutParameter(storedProcCommand, "ProductId", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "MinShowPrice", System.Data.DbType.Currency, product.MinShowPrice);
			this.database.AddInParameter(storedProcCommand, "MaxShowPrice", System.Data.DbType.Currency, product.MaxShowPrice);
			this.database.AddInParameter(storedProcCommand, "FirstCommission", System.Data.DbType.Decimal, product.FirstCommission);
			this.database.AddInParameter(storedProcCommand, "SecondCommission", System.Data.DbType.Decimal, product.SecondCommission);
			this.database.AddInParameter(storedProcCommand, "ThirdCommission", System.Data.DbType.Decimal, product.ThirdCommission);
			this.database.AddInParameter(storedProcCommand, "IsSetCommission", System.Data.DbType.Boolean, product.IsSetCommission);
			this.database.AddInParameter(storedProcCommand, "CubicMeter", System.Data.DbType.Decimal, product.CubicMeter);
			this.database.AddInParameter(storedProcCommand, "FreightWeight", System.Data.DbType.Decimal, product.FreightWeight);
			this.database.AddInParameter(storedProcCommand, "FreightTemplateId", System.Data.DbType.Int32, product.FreightTemplateId);
			this.database.ExecuteNonQuery(storedProcCommand, dbTran);
			return (int)this.database.GetParameterValue(storedProcCommand, "ProductId");
		}

		private decimal Opreateion(decimal opreation1, decimal opreation2, string operation)
		{
			decimal result = 0m;
			if (operation != null)
			{
				if (!(operation == "+"))
				{
					if (!(operation == "-"))
					{
						if (!(operation == "*"))
						{
							if (operation == "/")
							{
								result = opreation1 * opreation2;
							}
						}
						else
						{
							result = opreation1 * opreation2;
						}
					}
					else
					{
						result = opreation1 - opreation2;
					}
				}
				else
				{
					result = opreation1 + opreation2;
				}
			}
			return result;
		}

		public bool AddProductSKUs(int productId, Dictionary<string, SKUItem> skus, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_SKUs(SkuId, ProductId, SKU, Weight, Stock, CostPrice, SalePrice) VALUES(@SkuId, @ProductId, @SKU, @Weight, @Stock, @CostPrice, @SalePrice)");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "SKU", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand, "Weight", System.Data.DbType.Decimal);
			this.database.AddInParameter(sqlStringCommand, "Stock", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "CostPrice", System.Data.DbType.Currency);
			this.database.AddInParameter(sqlStringCommand, "SalePrice", System.Data.DbType.Currency);
			System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("INSERT INTO Hishop_SKUItems(SkuId, AttributeId, ValueId) VALUES(@SkuId, @AttributeId, @ValueId)");
			this.database.AddInParameter(sqlStringCommand2, "SkuId", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand2, "AttributeId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand2, "ValueId", System.Data.DbType.Int32);
			System.Data.Common.DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("INSERT INTO Hishop_SKUMemberPrice(SkuId, GradeId, MemberSalePrice) VALUES(@SkuId, @GradeId, @MemberSalePrice)");
			this.database.AddInParameter(sqlStringCommand3, "SkuId", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand3, "GradeId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand3, "MemberSalePrice", System.Data.DbType.Currency);
			bool result;
			try
			{
				this.database.SetParameterValue(sqlStringCommand, "ProductId", productId);
				foreach (SKUItem current in skus.Values)
				{
					string value = productId.ToString(CultureInfo.InvariantCulture) + "_" + current.SkuId;
					this.database.SetParameterValue(sqlStringCommand, "SkuId", value);
					this.database.SetParameterValue(sqlStringCommand, "SKU", current.SKU);
					this.database.SetParameterValue(sqlStringCommand, "Weight", current.Weight);
					this.database.SetParameterValue(sqlStringCommand, "Stock", current.Stock);
					this.database.SetParameterValue(sqlStringCommand, "CostPrice", current.CostPrice);
					this.database.SetParameterValue(sqlStringCommand, "SalePrice", Math.Round(current.SalePrice, SettingsManager.GetMasterSettings(true).DecimalLength));
					if (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 0)
					{
						result = false;
						return result;
					}
					this.database.SetParameterValue(sqlStringCommand2, "SkuId", value);
					foreach (int current2 in current.SkuItems.Keys)
					{
						this.database.SetParameterValue(sqlStringCommand2, "AttributeId", current2);
						this.database.SetParameterValue(sqlStringCommand2, "ValueId", current.SkuItems[current2]);
						this.database.ExecuteNonQuery(sqlStringCommand2, dbTran);
					}
					this.database.SetParameterValue(sqlStringCommand3, "SkuId", value);
					foreach (int current3 in current.MemberPrices.Keys)
					{
						this.database.SetParameterValue(sqlStringCommand3, "GradeId", current3);
						this.database.SetParameterValue(sqlStringCommand3, "MemberSalePrice", current.MemberPrices[current3]);
						this.database.ExecuteNonQuery(sqlStringCommand3, dbTran);
					}
				}
				result = true;
			}
			catch (Exception var_7_3BD)
			{
				result = false;
			}
			return result;
		}

		public bool DeleteProductSKUS(int productId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_SKUs WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			bool result;
			try
			{
				if (dbTran == null)
				{
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
				else
				{
					this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public bool AddProductAttributes(int productId, Dictionary<int, IList<int>> attributes, System.Data.Common.DbTransaction dbTran)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DELETE FROM Hishop_ProductAttributes WHERE ProductId = {0};", productId);
			int num = 0;
			if (attributes != null)
			{
				foreach (int current in attributes.Keys)
				{
					foreach (int current2 in attributes[current])
					{
						num++;
						stringBuilder.AppendFormat(" INSERT INTO Hishop_ProductAttributes (ProductId, AttributeId, ValueId) VALUES ({0}, {1}, {2})", productId, current, current2);
					}
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			bool result;
			if (dbTran == null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0);
			}
			return result;
		}

		public int GetMaxSequence()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT MAX(DisplaySequence) FROM Hishop_Products");
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			return (obj == DBNull.Value) ? 0 : ((int)obj);
		}

		public string UpdateProductContent(int productid, string content)
		{
			string empty = string.Empty;
			string query = "update Hishop_Products set Description=@Description where ProductId=@ProductId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productid);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, content);
			return this.database.ExecuteNonQuery(sqlStringCommand).ToString();
		}

		public bool UpdateProduct(ProductInfo product, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_Update");
			if (product.FreightTemplateId < 1)
			{
				product.IsfreeShipping = true;
			}
			this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, product.CategoryId);
			this.database.AddInParameter(storedProcCommand, "MainCategoryPath", System.Data.DbType.String, product.MainCategoryPath);
			this.database.AddInParameter(storedProcCommand, "TypeId", System.Data.DbType.Int32, product.TypeId);
			this.database.AddInParameter(storedProcCommand, "ProductName", System.Data.DbType.String, product.ProductName);
			this.database.AddInParameter(storedProcCommand, "ProductShortName", System.Data.DbType.String, product.ProductShortName);
			this.database.AddInParameter(storedProcCommand, "ProductCode", System.Data.DbType.String, product.ProductCode);
			this.database.AddInParameter(storedProcCommand, "ShortDescription", System.Data.DbType.String, product.ShortDescription);
			this.database.AddInParameter(storedProcCommand, "Unit", System.Data.DbType.String, product.Unit);
			this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, product.Description);
			this.database.AddInParameter(storedProcCommand, "SaleStatus", System.Data.DbType.Int32, (int)product.SaleStatus);
			this.database.AddInParameter(storedProcCommand, "DisplaySequence", System.Data.DbType.Currency, product.DisplaySequence);
			this.database.AddInParameter(storedProcCommand, "ImageUrl1", System.Data.DbType.String, product.ImageUrl1);
			this.database.AddInParameter(storedProcCommand, "ImageUrl2", System.Data.DbType.String, product.ImageUrl2);
			this.database.AddInParameter(storedProcCommand, "ImageUrl3", System.Data.DbType.String, product.ImageUrl3);
			this.database.AddInParameter(storedProcCommand, "ImageUrl4", System.Data.DbType.String, product.ImageUrl4);
			this.database.AddInParameter(storedProcCommand, "ImageUrl5", System.Data.DbType.String, product.ImageUrl5);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl40", System.Data.DbType.String, product.ThumbnailUrl40);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl60", System.Data.DbType.String, product.ThumbnailUrl60);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl100", System.Data.DbType.String, product.ThumbnailUrl100);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl160", System.Data.DbType.String, product.ThumbnailUrl160);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl180", System.Data.DbType.String, product.ThumbnailUrl180);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl220", System.Data.DbType.String, product.ThumbnailUrl220);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl310", System.Data.DbType.String, product.ThumbnailUrl310);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl410", System.Data.DbType.String, product.ThumbnailUrl410);
			this.database.AddInParameter(storedProcCommand, "MarketPrice", System.Data.DbType.Currency, product.MarketPrice);
			this.database.AddInParameter(storedProcCommand, "BrandId", System.Data.DbType.Int32, product.BrandId);
			this.database.AddInParameter(storedProcCommand, "HasSKU", System.Data.DbType.Boolean, product.HasSKU);
			this.database.AddInParameter(storedProcCommand, "IsfreeShipping", System.Data.DbType.Boolean, product.IsfreeShipping);
			this.database.AddInParameter(storedProcCommand, "SaleCounts", System.Data.DbType.Int32, product.SaleCounts);
			this.database.AddInParameter(storedProcCommand, "ShowSaleCounts", System.Data.DbType.Int32, product.ShowSaleCounts);
			this.database.AddInParameter(storedProcCommand, "ProductId", System.Data.DbType.Int32, product.ProductId);
			this.database.AddInParameter(storedProcCommand, "MinShowPrice", System.Data.DbType.Currency, product.MinShowPrice);
			this.database.AddInParameter(storedProcCommand, "MaxShowPrice", System.Data.DbType.Currency, product.MaxShowPrice);
			this.database.AddInParameter(storedProcCommand, "FirstCommission", System.Data.DbType.Decimal, product.FirstCommission);
			this.database.AddInParameter(storedProcCommand, "SecondCommission", System.Data.DbType.Decimal, product.SecondCommission);
			this.database.AddInParameter(storedProcCommand, "ThirdCommission", System.Data.DbType.Decimal, product.ThirdCommission);
			this.database.AddInParameter(storedProcCommand, "IsSetCommission", System.Data.DbType.Boolean, product.IsSetCommission);
			this.database.AddInParameter(storedProcCommand, "CubicMeter", System.Data.DbType.Decimal, product.CubicMeter);
			this.database.AddInParameter(storedProcCommand, "FreightWeight", System.Data.DbType.Decimal, product.FreightWeight);
			this.database.AddInParameter(storedProcCommand, "FreightTemplateId", System.Data.DbType.Int32, product.FreightTemplateId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(storedProcCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(storedProcCommand) > 0);
			}
			return result;
		}

		public bool UpdateProductCategory(int productId, int newCategoryId, string mainCategoryPath)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId = @CategoryId, MainCategoryPath = @MainCategoryPath WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, newCategoryId);
			this.database.AddInParameter(sqlStringCommand, "MainCategoryPath", System.Data.DbType.String, mainCategoryPath);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public int DeleteProduct(string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_Products WHERE ProductId IN ({0});  DELETE FROM Hishop_ShoppingCarts WHERE SkuId IN (select SkuId from Hishop_SKUs where  ProductId IN ({0}));  DELETE FROM Hishop_Favorite WHERE ProductId IN ({0});  DELETE FROM Hishop_ProductTag WHERE ProductId IN ({0});", productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus)
		{
			string query = string.Empty;
			query = string.Format("UPDATE Hishop_Products SET SaleStatus = {0} WHERE ProductId IN ({1})", (int)saleStatus, productIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int UpdateProductShipFree(string productIds, bool isFree)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET IsfreeShipping = {0}, FreightTemplateId=0 WHERE ProductId IN ({1})", Convert.ToInt32(isFree), productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int UpdateProductFreightTemplate(string productIds, int FreightTemplateId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET IsfreeShipping = 0,FreightTemplateId={0} WHERE ProductId IN ({1})", FreightTemplateId, productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public System.Data.DataTable GetProductCategories(int ProductId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CategoryId   FROM Hishop_Products  WHERE ProductId=" + ProductId);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public int GetProductsCount()
		{
			string query = "SELECT COUNT(*) FROM Hishop_Products WHERE SaleStatus=1";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		public int GetProductsCountByDistributor(int rid)
		{
			string query = "SELECT COUNT (*) FROM dbo.Hishop_DistributorProducts WHERE UserId =" + rid;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool GetProductHasSku(string skuid, int quantity)
		{
			bool result = false;
			SKUItem skuItem = new SkuDao().GetSkuItem(skuid);
			if (skuItem != null)
			{
				result = (skuItem.Stock >= quantity);
			}
			return result;
		}
	}
}
