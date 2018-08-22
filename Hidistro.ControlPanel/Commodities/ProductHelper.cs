using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SqlDal.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.ControlPanel.Commodities
{
	public static class ProductHelper
	{
		public delegate void WorkDelegate(ProductInfo product, out bool result);

		public class AsyncWorkDelegate
		{
			public void CalData(ProductInfo product, out bool result)
			{
				result = false;
				try
				{
					Messenger.SendWeiXinMsg_ProductCreate(product.ProductName);
				}
				catch (Exception var_1_1A)
				{
				}
			}
		}

		public static ProductInfo GetProductDetails(int productId, out Dictionary<int, IList<int>> attrs, out IList<int> tagsId)
		{
			ProductDao productDao = new ProductDao();
			attrs = productDao.GetProductAttributes(productId);
			tagsId = productDao.GetProductTags(productId);
			return productDao.GetProductDetails(productId);
		}

		public static ProductInfo GetProductDetails(int productId)
		{
			return new ProductDao().GetProductDetails(productId);
		}

		public static ProductInfo GetBrowseProductListByView(int productId)
		{
			return new ProductDao().GetBrowseProductListByView(productId);
		}

		public static DbQueryResult GetProducts(ProductQuery query)
		{
			return new ProductDao().GetProducts(query);
		}

		public static DbQueryResult GetProductsImgList(ProductQuery query)
		{
			return new ProductDao().GetProductsImgList(query);
		}

		public static long GetProductSumStock(int productId)
		{
			return new ProductDao().GetProductSumStock(productId);
		}

		public static System.Data.DataTable GetProductNum()
		{
			return new ProductDao().GetProductNum();
		}

		public static System.Data.DataTable GetTopProductOrder(int top, string showOrder)
		{
			if (top < 1)
			{
				top = 6;
			}
			if (string.IsNullOrEmpty(showOrder))
			{
				showOrder = " ProductId DESC";
			}
			return new ProductDao().GetTopProductOrder(top, showOrder);
		}

		public static DbQueryResult GetProductsFromGroup(ProductQuery query, string productIds)
		{
			return new ProductDao().GetProductsFromGroup(query, productIds);
		}

		public static System.Data.DataTable GetProducts(string products)
		{
			return new ProductDao().GetProducts(products);
		}

		public static System.Data.DataTable GetGroupBuyProducts(ProductQuery query)
		{
			return new ProductDao().GetGroupBuyProducts(query);
		}

		public static IList<ProductInfo> GetProducts(IList<int> productIds, bool Resort = false)
		{
			return new ProductDao().GetProducts(productIds, Resort);
		}

		public static IList<int> GetProductIds(ProductQuery query)
		{
			return new ProductDao().GetProductIds(query);
		}

		public static decimal GetProductSalePrice(int productId)
		{
			return new ProductDao().GetProductSalePrice(productId);
		}

		public static ProductActionStatus AddProduct(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> tagsId)
		{
			ProductActionStatus result;
			if (null == product)
			{
				result = ProductActionStatus.UnknowError;
			}
			else
			{
				Globals.EntityCoding(product, true);
				int decimalLength = SettingsManager.GetMasterSettings(true).DecimalLength;
				if (product.MarketPrice.HasValue)
				{
					product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
				}
				ProductActionStatus productActionStatus = ProductActionStatus.UnknowError;
				Database database = DatabaseFactory.CreateDatabase();
				using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						ProductDao productDao = new ProductDao();
						int num = productDao.AddProduct(product, dbTransaction);
						if (num == 0)
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.DuplicateSKU;
							return result;
						}
						product.ProductId = num;
						if (skus != null && skus.Count > 0)
						{
							if (!productDao.AddProductSKUs(num, skus, dbTransaction))
							{
								dbTransaction.Rollback();
								result = ProductActionStatus.SKUError;
								return result;
							}
						}
						if (attrs != null && attrs.Count > 0)
						{
							if (!productDao.AddProductAttributes(num, attrs, dbTransaction))
							{
								dbTransaction.Rollback();
								result = ProductActionStatus.AttributeError;
								return result;
							}
						}
						if (tagsId != null && tagsId.Count > 0)
						{
							if (!new TagDao().AddProductTags(num, tagsId, dbTransaction))
							{
								dbTransaction.Rollback();
								result = ProductActionStatus.ProductTagEroor;
								return result;
							}
						}
						dbTransaction.Commit();
						productActionStatus = ProductActionStatus.Success;
					}
					catch (Exception var_7_18D)
					{
						dbTransaction.Rollback();
					}
					finally
					{
						dbConnection.Close();
					}
				}
				if (productActionStatus == ProductActionStatus.Success)
				{
					EventLogs.WriteOperationLog(Privilege.AddProducts, string.Format(CultureInfo.InvariantCulture, "上架了一个新商品:”{0}”", new object[]
					{
						product.ProductName
					}));
				}
				result = productActionStatus;
			}
			return result;
		}

		public static void SendWXMessage_AddNewProduct(ProductInfo product)
		{
			ProductHelper.AsyncWorkDelegate @object = new ProductHelper.AsyncWorkDelegate();
			ProductHelper.WorkDelegate workDelegate = new ProductHelper.WorkDelegate(@object.CalData);
			bool flag;
			IAsyncResult asyncResult = workDelegate.BeginInvoke(product, out flag, null, null);
		}

		public static string AddProductNew(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> tagsId)
		{
			string text = string.Empty;
			string result;
			if (null == product)
			{
				result = "未知错误";
			}
			else
			{
				Globals.EntityCoding(product, true);
				int decimalLength = SettingsManager.GetMasterSettings(true).DecimalLength;
				if (product.MarketPrice.HasValue)
				{
					product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
				}
				ProductActionStatus productActionStatus = ProductActionStatus.UnknowError;
				Database database = DatabaseFactory.CreateDatabase();
				using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						ProductDao productDao = new ProductDao();
						int num = productDao.AddProduct(product, dbTransaction);
						if (num == 0)
						{
							dbTransaction.Rollback();
							result = "货号重复";
							return result;
						}
						text = num.ToString();
						product.ProductId = num;
						if (skus != null && skus.Count > 0)
						{
							if (!productDao.AddProductSKUs(num, skus, dbTransaction))
							{
								dbTransaction.Rollback();
								result = "添加SUK出错";
								return result;
							}
						}
						if (attrs != null && attrs.Count > 0)
						{
							if (!productDao.AddProductAttributes(num, attrs, dbTransaction))
							{
								dbTransaction.Rollback();
								result = "添加商品属性出错";
								return result;
							}
						}
						if (tagsId != null && tagsId.Count > 0)
						{
							if (!new TagDao().AddProductTags(num, tagsId, dbTransaction))
							{
								dbTransaction.Rollback();
								result = "添加商品标签出错";
								return result;
							}
						}
						dbTransaction.Commit();
						productActionStatus = ProductActionStatus.Success;
					}
					catch (Exception var_8_1B3)
					{
						dbTransaction.Rollback();
					}
					finally
					{
						dbConnection.Close();
					}
				}
				if (productActionStatus == ProductActionStatus.Success)
				{
					EventLogs.WriteOperationLog(Privilege.AddProducts, string.Format(CultureInfo.InvariantCulture, "上架了一个新商品:”{0}”", new object[]
					{
						product.ProductName
					}));
				}
				result = text;
			}
			return result;
		}

		public static int GetMaxSequence()
		{
			return new ProductDao().GetMaxSequence();
		}

		public static string UpdateProductContent(int productid, string content)
		{
			return new ProductDao().UpdateProductContent(productid, content);
		}

		public static ProductActionStatus UpdateProduct(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> tagIds)
		{
			ProductActionStatus result;
			if (null == product)
			{
				result = ProductActionStatus.UnknowError;
			}
			else
			{
				Globals.EntityCoding(product, true);
				int decimalLength = SettingsManager.GetMasterSettings(true).DecimalLength;
				if (product.MarketPrice.HasValue)
				{
					product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
				}
				ProductActionStatus productActionStatus = ProductActionStatus.UnknowError;
				Database database = DatabaseFactory.CreateDatabase();
				using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						ProductDao productDao = new ProductDao();
						if (!productDao.UpdateProduct(product, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.DuplicateSKU;
							return result;
						}
						if (!productDao.DeleteProductSKUS(product.ProductId, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.SKUError;
							return result;
						}
						if (skus != null && skus.Count > 0)
						{
							if (!productDao.AddProductSKUs(product.ProductId, skus, dbTransaction))
							{
								dbTransaction.Rollback();
								result = ProductActionStatus.SKUError;
								return result;
							}
						}
						if (!productDao.AddProductAttributes(product.ProductId, attrs, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.AttributeError;
							return result;
						}
						if (!new TagDao().DeleteProductTags(product.ProductId, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.ProductTagEroor;
							return result;
						}
						if (tagIds.Count > 0)
						{
							if (!new TagDao().AddProductTags(product.ProductId, tagIds, dbTransaction))
							{
								dbTransaction.Rollback();
								result = ProductActionStatus.ProductTagEroor;
								return result;
							}
						}
						dbTransaction.Commit();
						productActionStatus = ProductActionStatus.Success;
					}
					catch (Exception var_6_1B2)
					{
						dbTransaction.Rollback();
					}
					finally
					{
						dbConnection.Close();
					}
				}
				if (productActionStatus == ProductActionStatus.Success)
				{
					EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的商品", new object[]
					{
						product.ProductId
					}));
				}
				result = productActionStatus;
			}
			return result;
		}

		public static bool UpdateProductCategory(int productId, int newCategoryId)
		{
			bool flag;
			if (newCategoryId != 0)
			{
				flag = new ProductDao().UpdateProductCategory(productId, newCategoryId, CatalogHelper.GetCategory(newCategoryId).Path + "|");
			}
			else
			{
				flag = new ProductDao().UpdateProductCategory(productId, newCategoryId, null);
			}
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改编号 “{0}” 的店铺分类为 “{1}”", new object[]
				{
					productId,
					newCategoryId
				}));
			}
			return flag;
		}

		public static int DeleteProduct(string productIds, bool isDeleteImage)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
			int result;
			if (string.IsNullOrEmpty(productIds))
			{
				result = 0;
			}
			else
			{
				string[] array = productIds.Split(new char[]
				{
					','
				});
				IList<int> list = new List<int>();
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string s = array2[i];
					list.Add(int.Parse(s));
				}
				IList<ProductInfo> products = new ProductDao().GetProducts(list, false);
				int num = new ProductDao().DeleteProduct(productIds);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.DeleteProducts, string.Format(CultureInfo.InvariantCulture, "删除了 “{0}” 件商品", new object[]
					{
						list.Count
					}));
					if (isDeleteImage)
					{
						foreach (ProductInfo current in products)
						{
							try
							{
								ProductHelper.DeleteProductImage(current);
							}
							catch
							{
							}
						}
					}
				}
				result = num;
			}
			return result;
		}

		public static int UpShelf(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.UpShelfProducts);
			int result;
			if (string.IsNullOrEmpty(productIds))
			{
				result = 0;
			}
			else
			{
				int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.OnSale);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.UpShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量上架了 “{0}” 件商品", new object[]
					{
						num
					}));
				}
				result = num;
			}
			return result;
		}

		public static int OffShelf(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.OffShelfProducts);
			int result;
			if (string.IsNullOrEmpty(productIds))
			{
				result = 0;
			}
			else
			{
				int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.UnSale);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量下架了 “{0}” 件商品", new object[]
					{
						num
					}));
				}
				result = num;
			}
			return result;
		}

		public static int SetFreeShip(string productIds, bool isFree)
		{
			int result;
			if (string.IsNullOrEmpty(productIds))
			{
				result = 0;
			}
			else
			{
				int num = new ProductDao().UpdateProductShipFree(productIds, isFree);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "{0}了“{1}” 件商品包邮", new object[]
					{
						isFree ? "设置" : "取消",
						num
					}));
				}
				result = num;
			}
			return result;
		}

		public static int UpdateProductFreightTemplate(string productIds, int FreightTemplateId)
		{
			int result;
			if (string.IsNullOrEmpty(productIds))
			{
				result = 0;
			}
			else
			{
				int num = new ProductDao().UpdateProductFreightTemplate(productIds, FreightTemplateId);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "{0}了“{1}” 件商品运费", new object[]
					{
						"设置",
						num
					}));
				}
				result = num;
			}
			return result;
		}

		public static int InStock(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.InStockProduct);
			int result;
			if (string.IsNullOrEmpty(productIds))
			{
				result = 0;
			}
			else
			{
				int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.OnStock);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量入库了 “{0}” 件商品", new object[]
					{
						num
					}));
				}
				result = num;
			}
			return result;
		}

		public static int RemoveProduct(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
			int result;
			if (string.IsNullOrEmpty(productIds))
			{
				result = 0;
			}
			else
			{
				int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.Delete);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量删除了 “{0}” 件商品到回收站", new object[]
					{
						num
					}));
				}
				result = num;
			}
			return result;
		}

		public static System.Data.DataTable GetProductBaseInfo(string productIds)
		{
			return new ProductBatchDao().GetProductBaseInfo(productIds);
		}

		public static ProductInfo GetProductBaseInfo(int productId)
		{
			return new ProductBatchDao().GetProductBaseInfo(productId);
		}

		public static bool UpdateProductNames(string productIds, string prefix, string suffix)
		{
			return new ProductBatchDao().UpdateProductNames(productIds, prefix, suffix);
		}

		public static bool ReplaceProductNames(string productIds, string oldWord, string newWord)
		{
			return new ProductBatchDao().ReplaceProductNames(productIds, oldWord, newWord);
		}

		public static bool UpdateProductBaseInfo(System.Data.DataTable dt)
		{
			return dt != null && dt.Rows.Count > 0 && new ProductBatchDao().UpdateProductBaseInfo(dt);
		}

		public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
		{
			return new ProductBatchDao().UpdateShowSaleCounts(productIds, showSaleCounts);
		}

		public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
		{
			return new ProductBatchDao().UpdateShowSaleCounts(productIds, showSaleCounts, operation);
		}

		public static bool UpdateShowSaleCounts(System.Data.DataTable dt)
		{
			return dt != null && dt.Rows.Count > 0 && new ProductBatchDao().UpdateShowSaleCounts(dt);
		}

		public static System.Data.DataTable GetSkuStocks(string productIds)
		{
			return new ProductBatchDao().GetSkuStocks(productIds);
		}

		public static bool UpdateSkuStock(string productIds, int stock)
		{
			return new ProductBatchDao().UpdateSkuStock(productIds, stock);
		}

		public static bool AddSkuStock(string productIds, int addStock)
		{
			return new ProductBatchDao().AddSkuStock(productIds, addStock);
		}

		public static bool UpdateSkuStock(Dictionary<string, int> skuStocks)
		{
			return new ProductBatchDao().UpdateSkuStock(skuStocks);
		}

		public static System.Data.DataTable GetSkuMemberPrices(string productIds)
		{
			return new ProductBatchDao().GetSkuMemberPrices(productIds);
		}

		public static bool CheckPrice(string productIds, int baseGradeId, decimal checkPrice, bool isMember)
		{
			return new ProductBatchDao().CheckPrice(productIds, baseGradeId, checkPrice, isMember);
		}

		public static bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price)
		{
			return new ProductBatchDao().UpdateSkuMemberPrices(productIds, gradeId, price);
		}

		public static bool UpdateSkuMemberPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
		{
			return new ProductBatchDao().UpdateSkuMemberPrices(productIds, gradeId, baseGradeId, operation, price);
		}

		public static bool GetSKUMemberPrice(string productIds, int gradeId)
		{
			return new ProductBatchDao().GetSKUMemberPrice(productIds, gradeId);
		}

		public static bool UpdateSkuMemberPrices(System.Data.DataSet ds)
		{
			return new ProductBatchDao().UpdateSkuMemberPrices(ds);
		}

		public static System.Data.DataSet GetTaobaoProductDetails(int productId)
		{
			return new TaobaoProductDao().GetTaobaoProductDetails(productId);
		}

		public static bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct)
		{
			return new TaobaoProductDao().UpdateToaobProduct(taobaoProduct);
		}

		public static bool IsExitTaobaoProduct(long taobaoProductId)
		{
			return new TaobaoProductDao().IsExitTaobaoProduct(taobaoProductId);
		}

		public static string UploadDefaltProductImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
			{
				result = string.Empty;
			}
			else
			{
				string text = Globals.GetStoragePath() + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}

		private static void DeleteProductImage(ProductInfo product)
		{
			if (product != null)
			{
				if (!string.IsNullOrEmpty(product.ImageUrl1))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl1);
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
				if (!string.IsNullOrEmpty(product.ImageUrl2))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl2);
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
				if (!string.IsNullOrEmpty(product.ImageUrl3))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl3);
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
				if (!string.IsNullOrEmpty(product.ImageUrl4))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl4);
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
				if (!string.IsNullOrEmpty(product.ImageUrl5))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl5);
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
			}
		}

		public static DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
		{
			return new ProductDao().GetExportProducts(query, removeProductIds);
		}

		public static System.Data.DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
		{
			System.Data.DataSet exportProducts = new ProductDao().GetExportProducts(query, includeCostPrice, includeStock, removeProductIds);
			exportProducts.Tables[0].TableName = "types";
			exportProducts.Tables[1].TableName = "attributes";
			exportProducts.Tables[2].TableName = "values";
			exportProducts.Tables[3].TableName = "products";
			exportProducts.Tables[4].TableName = "skus";
			exportProducts.Tables[5].TableName = "skuItems";
			exportProducts.Tables[6].TableName = "productAttributes";
			exportProducts.Tables[7].TableName = "taobaosku";
			return exportProducts;
		}

		public static void EnsureMapping(System.Data.DataSet mappingSet)
		{
			new ProductDao().EnsureMapping(mappingSet);
		}

		public static void ImportProducts(System.Data.DataTable productData, int categoryId, int lineId, int? brandId, ProductSaleStatus saleStatus, bool isImportFromTaobao)
		{
			if (productData != null && productData.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in productData.Rows)
				{
					ProductInfo productInfo = new ProductInfo();
					productInfo.CategoryId = categoryId;
					productInfo.MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|";
					productInfo.ProductName = (string)dataRow["ProductName"];
					productInfo.ProductCode = (string)dataRow["SKU"];
					productInfo.BrandId = brandId;
					if (dataRow["Description"] != DBNull.Value)
					{
						productInfo.Description = (string)dataRow["Description"];
					}
					productInfo.MarketPrice = new decimal?((decimal)dataRow["SalePrice"]);
					productInfo.AddedDate = DateTime.Now;
					productInfo.SaleStatus = saleStatus;
					productInfo.HasSKU = false;
					HttpContext current = HttpContext.Current;
					if (dataRow["ImageUrl1"] != DBNull.Value)
					{
						productInfo.ImageUrl1 = (string)dataRow["ImageUrl1"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl1) && productInfo.ImageUrl1.Length > 0)
					{
						string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl1);
						productInfo.ThumbnailUrl40 = array[0];
						productInfo.ThumbnailUrl60 = array[1];
						productInfo.ThumbnailUrl100 = array[2];
						productInfo.ThumbnailUrl160 = array[3];
						productInfo.ThumbnailUrl180 = array[4];
						productInfo.ThumbnailUrl220 = array[5];
						productInfo.ThumbnailUrl310 = array[6];
						productInfo.ThumbnailUrl410 = array[7];
					}
					if (dataRow["ImageUrl2"] != DBNull.Value)
					{
						productInfo.ImageUrl2 = (string)dataRow["ImageUrl2"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl2) && productInfo.ImageUrl2.Length > 0)
					{
						string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl2);
					}
					if (dataRow["ImageUrl3"] != DBNull.Value)
					{
						productInfo.ImageUrl3 = (string)dataRow["ImageUrl3"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl3) && productInfo.ImageUrl3.Length > 0)
					{
						string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl3);
					}
					if (dataRow["ImageUrl4"] != DBNull.Value)
					{
						productInfo.ImageUrl4 = (string)dataRow["ImageUrl4"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl4) && productInfo.ImageUrl4.Length > 0)
					{
						string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl4);
					}
					if (dataRow["ImageUrl5"] != DBNull.Value)
					{
						productInfo.ImageUrl5 = (string)dataRow["ImageUrl5"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl5) && productInfo.ImageUrl5.Length > 0)
					{
						string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl5);
					}
					SKUItem sKUItem = new SKUItem();
					sKUItem.SkuId = "0";
					sKUItem.SKU = (string)dataRow["SKU"];
					if (dataRow["Stock"] != DBNull.Value)
					{
						sKUItem.Stock = (int)dataRow["Stock"];
					}
					if (dataRow["Weight"] != DBNull.Value)
					{
						sKUItem.Weight = (decimal)dataRow["Weight"];
					}
					sKUItem.SalePrice = (decimal)dataRow["SalePrice"];
					ProductActionStatus productActionStatus = ProductHelper.AddProduct(productInfo, new Dictionary<string, SKUItem>
					{
						{
							sKUItem.SkuId,
							sKUItem
						}
					}, null, null);
					ProductDao productDao = new ProductDao();
					if (productActionStatus == ProductActionStatus.Success)
					{
						productDao.AddProductMinPriceAndMaxPrice(productInfo.ProductId);
					}
					if (isImportFromTaobao && productActionStatus == ProductActionStatus.Success)
					{
						TaobaoProductInfo taobaoProductInfo = new TaobaoProductInfo();
						taobaoProductInfo.ProductId = productInfo.ProductId;
						taobaoProductInfo.ProTitle = productInfo.ProductName;
						taobaoProductInfo.Cid = (long)dataRow["Cid"];
						if (dataRow["StuffStatus"] != DBNull.Value)
						{
							taobaoProductInfo.StuffStatus = (string)dataRow["StuffStatus"];
						}
						taobaoProductInfo.Num = (long)dataRow["Num"];
						taobaoProductInfo.LocationState = (string)dataRow["LocationState"];
						taobaoProductInfo.LocationCity = (string)dataRow["LocationCity"];
						taobaoProductInfo.FreightPayer = (string)dataRow["FreightPayer"];
						if (dataRow["PostFee"] != DBNull.Value)
						{
							taobaoProductInfo.PostFee = (decimal)dataRow["PostFee"];
						}
						if (dataRow["ExpressFee"] != DBNull.Value)
						{
							taobaoProductInfo.ExpressFee = (decimal)dataRow["ExpressFee"];
						}
						if (dataRow["EMSFee"] != DBNull.Value)
						{
							taobaoProductInfo.EMSFee = (decimal)dataRow["EMSFee"];
						}
						taobaoProductInfo.HasInvoice = (bool)dataRow["HasInvoice"];
						taobaoProductInfo.HasWarranty = (bool)dataRow["HasWarranty"];
						taobaoProductInfo.HasDiscount = (bool)dataRow["HasDiscount"];
						taobaoProductInfo.ValidThru = (long)dataRow["ValidThru"];
						if (dataRow["ListTime"] != DBNull.Value)
						{
							taobaoProductInfo.ListTime = (DateTime)dataRow["ListTime"];
						}
						else
						{
							taobaoProductInfo.ListTime = DateTime.Now;
						}
						if (dataRow["PropertyAlias"] != DBNull.Value)
						{
							taobaoProductInfo.PropertyAlias = (string)dataRow["PropertyAlias"];
						}
						if (dataRow["InputPids"] != DBNull.Value)
						{
							taobaoProductInfo.InputPids = (string)dataRow["InputPids"];
						}
						if (dataRow["InputStr"] != DBNull.Value)
						{
							taobaoProductInfo.InputStr = (string)dataRow["InputStr"];
						}
						if (dataRow["SkuProperties"] != DBNull.Value)
						{
							taobaoProductInfo.SkuProperties = (string)dataRow["SkuProperties"];
						}
						if (dataRow["SkuQuantities"] != DBNull.Value)
						{
							taobaoProductInfo.SkuQuantities = (string)dataRow["SkuQuantities"];
						}
						if (dataRow["SkuPrices"] != DBNull.Value)
						{
							taobaoProductInfo.SkuPrices = (string)dataRow["SkuPrices"];
						}
						if (dataRow["SkuOuterIds"] != DBNull.Value)
						{
							taobaoProductInfo.SkuOuterIds = (string)dataRow["SkuOuterIds"];
						}
						ProductHelper.UpdateToaobProduct(taobaoProductInfo);
					}
				}
			}
		}

		public static void ImportProducts(System.Data.DataSet productData, int categoryId, int lineId, int? bandId, ProductSaleStatus saleStatus, bool includeCostPrice, bool includeStock, bool includeImages)
		{
			foreach (System.Data.DataRow dataRow in productData.Tables["products"].Rows)
			{
				int mappedProductId = (int)dataRow["ProductId"];
				ProductInfo product = ProductHelper.ConverToProduct(dataRow, categoryId, lineId, bandId, saleStatus, includeImages);
				Dictionary<string, SKUItem> skus = ProductHelper.ConverToSkus(mappedProductId, productData, includeCostPrice, includeStock);
				Dictionary<int, IList<int>> attrs = ProductHelper.ConvertToAttributes(mappedProductId, productData);
				ProductActionStatus productActionStatus = ProductHelper.AddProduct(product, skus, attrs, null);
			}
		}

		private static Dictionary<int, IList<int>> ConvertToAttributes(int mappedProductId, System.Data.DataSet productData)
		{
			System.Data.DataRow[] array = productData.Tables["attributes"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
			Dictionary<int, IList<int>> result;
			if (array.Length == 0)
			{
				result = null;
			}
			else
			{
				Dictionary<int, IList<int>> dictionary = new Dictionary<int, IList<int>>();
				System.Data.DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow = array2[i];
					int key = (int)dataRow["SelectedAttributeId"];
					if (!dictionary.ContainsKey(key))
					{
						IList<int> value = new List<int>();
						dictionary.Add(key, value);
					}
					dictionary[key].Add((int)dataRow["SelectedValueId"]);
				}
				result = dictionary;
			}
			return result;
		}

		private static Dictionary<string, SKUItem> ConverToSkus(int mappedProductId, System.Data.DataSet productData, bool includeCostPrice, bool includeStock)
		{
			System.Data.DataRow[] array = productData.Tables["skus"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
			Dictionary<string, SKUItem> result;
			if (array.Length == 0)
			{
				result = null;
			}
			else
			{
				Dictionary<string, SKUItem> dictionary = new Dictionary<string, SKUItem>();
				System.Data.DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow = array2[i];
					string text = (string)dataRow["NewSkuId"];
					SKUItem sKUItem = new SKUItem
					{
						SkuId = text,
						SKU = (string)dataRow["SKU"],
						SalePrice = (decimal)dataRow["SalePrice"]
					};
					if (dataRow["Weight"] != DBNull.Value)
					{
						sKUItem.Weight = (decimal)dataRow["Weight"];
					}
					if (includeCostPrice && dataRow["CostPrice"] != DBNull.Value)
					{
						sKUItem.CostPrice = (decimal)dataRow["CostPrice"];
					}
					if (includeStock)
					{
						sKUItem.Stock = (int)dataRow["Stock"];
					}
					System.Data.DataRow[] array3 = productData.Tables["skuItems"].Select("NewSkuId='" + text + "' AND MappedProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
					System.Data.DataRow[] array4 = array3;
					for (int j = 0; j < array4.Length; j++)
					{
						System.Data.DataRow dataRow2 = array4[j];
						sKUItem.SkuItems.Add((int)dataRow2["SelectedAttributeId"], (int)dataRow2["SelectedValueId"]);
					}
					dictionary.Add(text, sKUItem);
				}
				result = dictionary;
			}
			return result;
		}

		private static ProductInfo ConverToProduct(System.Data.DataRow productRow, int categoryId, int lineId, int? bandId, ProductSaleStatus saleStatus, bool includeImages)
		{
			ProductInfo productInfo = new ProductInfo
			{
				CategoryId = categoryId,
				TypeId = new int?((int)productRow["SelectedTypeId"]),
				ProductName = (string)productRow["ProductName"],
				ProductCode = (string)productRow["ProductCode"],
				BrandId = bandId,
				Unit = (string)productRow["Unit"],
				ShortDescription = (string)productRow["ShortDescription"],
				Description = (string)productRow["Description"],
				AddedDate = DateTime.Now,
				SaleStatus = saleStatus,
				HasSKU = (bool)productRow["HasSKU"],
				MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|",
				ImageUrl1 = (string)productRow["ImageUrl1"],
				ImageUrl2 = (string)productRow["ImageUrl2"],
				ImageUrl3 = (string)productRow["ImageUrl3"],
				ImageUrl4 = (string)productRow["ImageUrl4"],
				ImageUrl5 = (string)productRow["ImageUrl5"]
			};
			if (productRow["MarketPrice"] != DBNull.Value)
			{
				productInfo.MarketPrice = new decimal?((decimal)productRow["MarketPrice"]);
			}
			if (includeImages)
			{
				HttpContext current = HttpContext.Current;
				if (!string.IsNullOrEmpty(productInfo.ImageUrl1) && productInfo.ImageUrl1.Length > 0)
				{
					string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl1);
					productInfo.ThumbnailUrl40 = array[0];
					productInfo.ThumbnailUrl60 = array[1];
					productInfo.ThumbnailUrl100 = array[2];
					productInfo.ThumbnailUrl160 = array[3];
					productInfo.ThumbnailUrl180 = array[4];
					productInfo.ThumbnailUrl220 = array[5];
					productInfo.ThumbnailUrl310 = array[6];
					productInfo.ThumbnailUrl410 = array[7];
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl2) && productInfo.ImageUrl2.Length > 0)
				{
					string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl2);
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl3) && productInfo.ImageUrl3.Length > 0)
				{
					string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl3);
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl4) && productInfo.ImageUrl4.Length > 0)
				{
					string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl4);
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl5) && productInfo.ImageUrl5.Length > 0)
				{
					string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl5);
				}
			}
			return productInfo;
		}

		private static string[] ProcessImages(HttpContext context, string originalSavePath)
		{
			string fileName = Path.GetFileName(originalSavePath);
			string text = "/Storage/master/product/thumbs40/40_" + fileName;
			string text2 = "/Storage/master/product/thumbs60/60_" + fileName;
			string text3 = "/Storage/master/product/thumbs100/100_" + fileName;
			string text4 = "/Storage/master/product/thumbs160/160_" + fileName;
			string text5 = "/Storage/master/product/thumbs180/180_" + fileName;
			string text6 = "/Storage/master/product/thumbs220/220_" + fileName;
			string text7 = "/Storage/master/product/thumbs310/310_" + fileName;
			string text8 = "/Storage/master/product/thumbs410/410_" + fileName;
			string text9 = context.Request.MapPath(Globals.ApplicationPath + originalSavePath);
			if (File.Exists(text9))
			{
				try
				{
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text), 40, 40);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text2), 60, 60);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text3), 100, 100);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text4), 160, 160);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text5), 180, 180);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text6), 220, 220);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text7), 310, 310);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text8), 410, 410);
				}
				catch
				{
				}
			}
			return new string[]
			{
				text,
				text2,
				text3,
				text4,
				text5,
				text6,
				text7,
				text8
			};
		}

		public static int GetProductsCount()
		{
			return new ProductDao().GetProductsCount();
		}

		public static int GetProductsCountByDistributor(int rid)
		{
			return new ProductDao().GetProductsCountByDistributor(rid);
		}

		public static bool GetProductHasSku(string skuid, int quantity)
		{
			return new ProductDao().GetProductHasSku(skuid, quantity);
		}
	}
}
