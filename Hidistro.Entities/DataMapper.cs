using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Settings;
using Hidistro.Entities.VShop;
using System;
using System.Data;

namespace Hidistro.Entities
{
	public static class DataMapper
	{
		public static ProductInfo PopulateProduct(DataRow reader)
		{
			ProductInfo productInfo = new ProductInfo();
			productInfo.CategoryId = (int)reader["CategoryId"];
			productInfo.ProductId = (int)reader["ProductId"];
			if (System.DBNull.Value != reader["TypeId"])
			{
				productInfo.TypeId = new int?((int)reader["TypeId"]);
			}
			if (System.DBNull.Value != reader["FreightTemplateId"])
			{
				productInfo.FreightTemplateId = (int)reader["FreightTemplateId"];
			}
			else
			{
				productInfo.FreightTemplateId = 0;
			}
			productInfo.ProductName = (string)reader["ProductName"];
			if (System.DBNull.Value != reader["ProductCode"])
			{
				productInfo.ProductCode = (string)reader["ProductCode"];
			}
			if (System.DBNull.Value != reader["ShortDescription"])
			{
				productInfo.ShortDescription = (string)reader["ShortDescription"];
			}
			if (System.DBNull.Value != reader["Unit"])
			{
				productInfo.Unit = (string)reader["Unit"];
			}
			if (System.DBNull.Value != reader["Description"])
			{
				productInfo.Description = (string)reader["Description"];
			}
			productInfo.SaleStatus = (ProductSaleStatus)((int)reader["SaleStatus"]);
			productInfo.AddedDate = (System.DateTime)reader["AddedDate"];
			productInfo.VistiCounts = (int)reader["VistiCounts"];
			productInfo.SaleCounts = (int)reader["SaleCounts"];
			productInfo.ShowSaleCounts = (int)reader["ShowSaleCounts"];
			productInfo.DisplaySequence = (int)reader["DisplaySequence"];
			if (System.DBNull.Value != reader["ImageUrl1"])
			{
				productInfo.ImageUrl1 = (string)reader["ImageUrl1"];
			}
			if (System.DBNull.Value != reader["ImageUrl2"])
			{
				productInfo.ImageUrl2 = (string)reader["ImageUrl2"];
			}
			if (System.DBNull.Value != reader["ImageUrl3"])
			{
				productInfo.ImageUrl3 = (string)reader["ImageUrl3"];
			}
			if (System.DBNull.Value != reader["ImageUrl4"])
			{
				productInfo.ImageUrl4 = (string)reader["ImageUrl4"];
			}
			if (System.DBNull.Value != reader["ImageUrl5"])
			{
				productInfo.ImageUrl5 = (string)reader["ImageUrl5"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl40"])
			{
				productInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl60"])
			{
				productInfo.ThumbnailUrl60 = (string)reader["ThumbnailUrl60"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl100"])
			{
				productInfo.ThumbnailUrl100 = (string)reader["ThumbnailUrl100"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl160"])
			{
				productInfo.ThumbnailUrl160 = (string)reader["ThumbnailUrl160"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl180"])
			{
				productInfo.ThumbnailUrl180 = (string)reader["ThumbnailUrl180"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl220"])
			{
				productInfo.ThumbnailUrl220 = (string)reader["ThumbnailUrl220"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl310"])
			{
				productInfo.ThumbnailUrl310 = (string)reader["ThumbnailUrl310"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl410"])
			{
				productInfo.ThumbnailUrl410 = (string)reader["ThumbnailUrl410"];
			}
			if (System.DBNull.Value != reader["MinShowPrice"])
			{
				productInfo.MinShowPrice = (decimal)reader["MinShowPrice"];
			}
			if (System.DBNull.Value != reader["MarketPrice"])
			{
				productInfo.MarketPrice = new decimal?((decimal)reader["MarketPrice"]);
			}
			if (System.DBNull.Value != reader["BrandId"])
			{
				productInfo.BrandId = new int?((int)reader["BrandId"]);
			}
			if (reader["MainCategoryPath"] != System.DBNull.Value)
			{
				productInfo.MainCategoryPath = (string)reader["MainCategoryPath"];
			}
			if (reader["ExtendCategoryPath"] != System.DBNull.Value)
			{
				productInfo.ExtendCategoryPath = (string)reader["ExtendCategoryPath"];
			}
			productInfo.HasSKU = (bool)reader["HasSKU"];
			if (reader["TaobaoProductId"] != System.DBNull.Value)
			{
				productInfo.TaobaoProductId = (long)reader["TaobaoProductId"];
			}
			return productInfo;
		}

		public static ProductInfo PopulateProduct(IDataReader reader)
		{
			ProductInfo productInfo = new ProductInfo();
			productInfo.CategoryId = (int)reader["CategoryId"];
			productInfo.ProductId = (int)reader["ProductId"];
			if (System.DBNull.Value != reader["TypeId"])
			{
				productInfo.TypeId = new int?((int)reader["TypeId"]);
			}
			if (System.DBNull.Value != reader["FreightTemplateId"])
			{
				productInfo.FreightTemplateId = (int)reader["FreightTemplateId"];
			}
			else
			{
				productInfo.FreightTemplateId = 0;
			}
			productInfo.ProductName = (string)reader["ProductName"];
			if (System.DBNull.Value != reader["ProductCode"])
			{
				productInfo.ProductCode = (string)reader["ProductCode"];
			}
			if (System.DBNull.Value != reader["ShortDescription"])
			{
				productInfo.ShortDescription = (string)reader["ShortDescription"];
			}
			if (System.DBNull.Value != reader["Unit"])
			{
				productInfo.Unit = (string)reader["Unit"];
			}
			if (System.DBNull.Value != reader["Description"])
			{
				productInfo.Description = (string)reader["Description"];
			}
			productInfo.SaleStatus = (ProductSaleStatus)((int)reader["SaleStatus"]);
			productInfo.AddedDate = (System.DateTime)reader["AddedDate"];
			productInfo.VistiCounts = (int)reader["VistiCounts"];
			productInfo.SaleCounts = (int)reader["SaleCounts"];
			productInfo.ShowSaleCounts = (int)reader["ShowSaleCounts"];
			productInfo.DisplaySequence = (int)reader["DisplaySequence"];
			if (System.DBNull.Value != reader["ImageUrl1"])
			{
				productInfo.ImageUrl1 = (string)reader["ImageUrl1"];
			}
			if (System.DBNull.Value != reader["ImageUrl2"])
			{
				productInfo.ImageUrl2 = (string)reader["ImageUrl2"];
			}
			if (System.DBNull.Value != reader["ImageUrl3"])
			{
				productInfo.ImageUrl3 = (string)reader["ImageUrl3"];
			}
			if (System.DBNull.Value != reader["ImageUrl4"])
			{
				productInfo.ImageUrl4 = (string)reader["ImageUrl4"];
			}
			if (System.DBNull.Value != reader["ImageUrl5"])
			{
				productInfo.ImageUrl5 = (string)reader["ImageUrl5"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl40"])
			{
				productInfo.ThumbnailUrl40 = (string)reader["ThumbnailUrl40"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl60"])
			{
				productInfo.ThumbnailUrl60 = (string)reader["ThumbnailUrl60"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl100"])
			{
				productInfo.ThumbnailUrl100 = (string)reader["ThumbnailUrl100"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl160"])
			{
				productInfo.ThumbnailUrl160 = (string)reader["ThumbnailUrl160"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl180"])
			{
				productInfo.ThumbnailUrl180 = (string)reader["ThumbnailUrl180"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl220"])
			{
				productInfo.ThumbnailUrl220 = (string)reader["ThumbnailUrl220"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl310"])
			{
				productInfo.ThumbnailUrl310 = (string)reader["ThumbnailUrl310"];
			}
			if (System.DBNull.Value != reader["ThumbnailUrl410"])
			{
				productInfo.ThumbnailUrl410 = (string)reader["ThumbnailUrl410"];
			}
			if (System.DBNull.Value != reader["MinShowPrice"])
			{
				productInfo.MinShowPrice = (decimal)reader["MinShowPrice"];
			}
			if (System.DBNull.Value != reader["MarketPrice"])
			{
				productInfo.MarketPrice = new decimal?((decimal)reader["MarketPrice"]);
			}
			if (System.DBNull.Value != reader["BrandId"])
			{
				productInfo.BrandId = new int?((int)reader["BrandId"]);
			}
			if (reader["MainCategoryPath"] != System.DBNull.Value)
			{
				productInfo.MainCategoryPath = (string)reader["MainCategoryPath"];
			}
			if (reader["ExtendCategoryPath"] != System.DBNull.Value)
			{
				productInfo.ExtendCategoryPath = (string)reader["ExtendCategoryPath"];
			}
			productInfo.HasSKU = (bool)reader["HasSKU"];
			if (reader["TaobaoProductId"] != System.DBNull.Value)
			{
				productInfo.TaobaoProductId = (long)reader["TaobaoProductId"];
			}
			return productInfo;
		}

		public static CategoryInfo ConvertDataRowToProductCategory(DataRow row)
		{
			CategoryInfo categoryInfo = new CategoryInfo();
			categoryInfo.CategoryId = (int)row["CategoryId"];
			categoryInfo.Name = (string)row["Name"];
			categoryInfo.DisplaySequence = (int)row["DisplaySequence"];
			if (row["IconUrl"] != System.DBNull.Value)
			{
				categoryInfo.IconUrl = (string)row["IconUrl"];
			}
			if (row["ParentCategoryId"] != System.DBNull.Value)
			{
				categoryInfo.ParentCategoryId = new int?((int)row["ParentCategoryId"]);
			}
			categoryInfo.Depth = (int)row["Depth"];
			categoryInfo.Path = (string)row["Path"];
			if (row["RewriteName"] != System.DBNull.Value)
			{
				categoryInfo.RewriteName = (string)row["RewriteName"];
			}
			categoryInfo.HasChildren = (bool)row["HasChildren"];
			if (row["FirstCommission"] != System.DBNull.Value)
			{
				categoryInfo.FirstCommission = row["FirstCommission"].ToString();
			}
			if (row["SecondCommission"] != System.DBNull.Value)
			{
				categoryInfo.SecondCommission = row["SecondCommission"].ToString();
			}
			if (row["ThirdCommission"] != System.DBNull.Value)
			{
				categoryInfo.ThirdCommission = row["ThirdCommission"].ToString();
			}
			return categoryInfo;
		}

		public static SKUItem PopulateSKU(IDataReader reader)
		{
			SKUItem result;
			if (reader == null)
			{
				result = null;
			}
			else
			{
				SKUItem sKUItem = new SKUItem();
				sKUItem.SkuId = (string)reader["SkuId"];
				sKUItem.ProductId = (int)reader["ProductId"];
				if (reader["SKU"] != System.DBNull.Value)
				{
					sKUItem.SKU = (string)reader["SKU"];
				}
				if (reader["Weight"] != System.DBNull.Value)
				{
					sKUItem.Weight = (decimal)reader["Weight"];
				}
				sKUItem.Stock = (int)reader["Stock"];
				if (reader["CostPrice"] != System.DBNull.Value)
				{
					sKUItem.CostPrice = (decimal)reader["CostPrice"];
				}
				sKUItem.SalePrice = (decimal)reader["SalePrice"];
				result = sKUItem;
			}
			return result;
		}

		public static CouponInfo PopulateCoupon(IDataReader reader)
		{
			CouponInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				CouponInfo couponInfo = new CouponInfo();
				couponInfo.CouponId = (int)reader["CouponId"];
				couponInfo.CouponName = (string)reader["Name"];
				couponInfo.BeginDate = (System.DateTime)reader["StartTime"];
				couponInfo.EndDate = (System.DateTime)reader["ClosingTime"];
				if (reader["Description"] != System.DBNull.Value && reader["Amount"] != System.DBNull.Value)
				{
					couponInfo.ConditionValue = (decimal)reader["Amount"];
				}
				couponInfo.CouponValue = (decimal)reader["DiscountValue"];
				couponInfo.ReceiveNum = (int)reader["SentCount"];
				couponInfo.UsedNum = (int)reader["UsedCount"];
				result = couponInfo;
			}
			return result;
		}

		public static PaymentModeInfo PopulatePayment(IDataRecord reader)
		{
			PaymentModeInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				PaymentModeInfo paymentModeInfo = new PaymentModeInfo
				{
					ModeId = (int)reader["ModeId"],
					Name = (string)reader["Name"],
					DisplaySequence = (int)reader["DisplaySequence"],
					IsUseInpour = (bool)reader["IsUseInpour"],
					Charge = (decimal)reader["Charge"],
					IsPercent = (bool)reader["IsPercent"]
				};
				try
				{
					paymentModeInfo.IsUseInDistributor = (bool)reader["IsUseInDistributor"];
				}
				catch
				{
					paymentModeInfo.IsUseInDistributor = false;
				}
				if (reader["Description"] != System.DBNull.Value)
				{
					paymentModeInfo.Description = (string)reader["Description"];
				}
				if (reader["Gateway"] != System.DBNull.Value)
				{
					paymentModeInfo.Gateway = (string)reader["Gateway"];
				}
				if (reader["Settings"] != System.DBNull.Value)
				{
					paymentModeInfo.Settings = (string)reader["Settings"];
				}
				result = paymentModeInfo;
			}
			return result;
		}

		public static ShippingModeInfo PopulateShippingMode(IDataRecord reader)
		{
			ShippingModeInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ShippingModeInfo shippingModeInfo = new ShippingModeInfo();
				if (reader["ModeId"] != System.DBNull.Value)
				{
					shippingModeInfo.ModeId = (int)reader["ModeId"];
				}
				if (reader["TemplateId"] != System.DBNull.Value)
				{
					shippingModeInfo.TemplateId = (int)reader["TemplateId"];
				}
				shippingModeInfo.Name = (string)reader["Name"];
				shippingModeInfo.TemplateName = (string)reader["TemplateName"];
				if (reader["Weight"] != System.DBNull.Value)
				{
					shippingModeInfo.Weight = (decimal)reader["Weight"];
				}
				if (System.DBNull.Value != reader["AddWeight"])
				{
					shippingModeInfo.AddWeight = new decimal?((decimal)reader["AddWeight"]);
				}
				if (reader["Price"] != System.DBNull.Value)
				{
					shippingModeInfo.Price = (decimal)reader["Price"];
				}
				if (System.DBNull.Value != reader["AddPrice"])
				{
					shippingModeInfo.AddPrice = new decimal?((decimal)reader["AddPrice"]);
				}
				if (reader["Description"] != System.DBNull.Value)
				{
					shippingModeInfo.Description = (string)reader["Description"];
				}
				shippingModeInfo.DisplaySequence = (int)reader["DisplaySequence"];
				result = shippingModeInfo;
			}
			return result;
		}

		public static ShippingModeInfo PopulateShippingTemplate(IDataRecord reader)
		{
			ShippingModeInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ShippingModeInfo shippingModeInfo = new ShippingModeInfo();
				if (reader["TemplateId"] != System.DBNull.Value)
				{
					shippingModeInfo.TemplateId = (int)reader["TemplateId"];
				}
				shippingModeInfo.Name = (string)reader["TemplateName"];
				shippingModeInfo.Weight = (decimal)reader["Weight"];
				if (System.DBNull.Value != reader["AddWeight"])
				{
					shippingModeInfo.AddWeight = new decimal?((decimal)reader["AddWeight"]);
				}
				shippingModeInfo.Price = (decimal)reader["Price"];
				if (System.DBNull.Value != reader["AddPrice"])
				{
					shippingModeInfo.AddPrice = new decimal?((decimal)reader["AddPrice"]);
				}
				result = shippingModeInfo;
			}
			return result;
		}

		public static ShippingModeGroupInfo PopulateShippingModeGroup(IDataRecord reader)
		{
			ShippingModeGroupInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ShippingModeGroupInfo shippingModeGroupInfo = new ShippingModeGroupInfo();
				shippingModeGroupInfo.TemplateId = (int)reader["TemplateId"];
				shippingModeGroupInfo.GroupId = (int)reader["GroupId"];
				shippingModeGroupInfo.Price = (decimal)reader["Price"];
				if (System.DBNull.Value != reader["AddPrice"])
				{
					shippingModeGroupInfo.AddPrice = (decimal)reader["AddPrice"];
				}
				result = shippingModeGroupInfo;
			}
			return result;
		}

		public static ShippingRegionInfo PopulateShippingRegion(IDataRecord reader)
		{
			ShippingRegionInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				result = new ShippingRegionInfo
				{
					TemplateId = (int)reader["TemplateId"],
					GroupId = (int)reader["GroupId"],
					RegionId = (int)reader["RegionId"]
				};
			}
			return result;
		}

		public static OrderInfo PopulateOrder(IDataRecord reader)
		{
			OrderInfo result;
			if (reader == null)
			{
				result = null;
			}
			else
			{
				OrderInfo orderInfo = new OrderInfo();
				orderInfo.OrderId = (string)reader["OrderId"];
				if (System.DBNull.Value != reader["OrderMarking"])
				{
					orderInfo.OrderMarking = (string)reader["OrderMarking"];
				}
				if (System.DBNull.Value != reader["GatewayOrderId"])
				{
					orderInfo.GatewayOrderId = (string)reader["GatewayOrderId"];
				}
				if (System.DBNull.Value != reader["Remark"])
				{
					orderInfo.Remark = (string)reader["Remark"];
				}
				if (System.DBNull.Value != reader["ClientShortType"])
				{
					orderInfo.ClientShortType = (ClientShortType)reader["ClientShortType"];
				}
				if (System.DBNull.Value != reader["ManagerMark"])
				{
					orderInfo.ManagerMark = new OrderMark?((OrderMark)reader["ManagerMark"]);
				}
				if (System.DBNull.Value != reader["ManagerRemark"])
				{
					orderInfo.ManagerRemark = (string)reader["ManagerRemark"];
				}
				if (System.DBNull.Value != reader["AdjustedDiscount"])
				{
					orderInfo.AdjustedDiscount = (decimal)reader["AdjustedDiscount"];
				}
				if (System.DBNull.Value != reader["OrderStatus"])
				{
					orderInfo.OrderStatus = (OrderStatus)reader["OrderStatus"];
				}
				if (System.DBNull.Value != reader["CloseReason"])
				{
					orderInfo.CloseReason = (string)reader["CloseReason"];
				}
				if (System.DBNull.Value != reader["OrderPoint"])
				{
					orderInfo.Points = (int)reader["OrderPoint"];
				}
				if (System.DBNull.Value != reader["DeleteBeforeState"])
				{
					orderInfo.DeleteBeforeState = (OrderStatus)reader["DeleteBeforeState"];
				}
				orderInfo.OrderDate = (System.DateTime)reader["OrderDate"];
				if (System.DBNull.Value != reader["PayDate"])
				{
					orderInfo.PayDate = new System.DateTime?((System.DateTime)reader["PayDate"]);
				}
				if (System.DBNull.Value != reader["ShippingDate"])
				{
					orderInfo.ShippingDate = new System.DateTime?((System.DateTime)reader["ShippingDate"]);
				}
				if (System.DBNull.Value != reader["FinishDate"])
				{
					orderInfo.FinishDate = new System.DateTime?((System.DateTime)reader["FinishDate"]);
				}
				orderInfo.UserId = (int)reader["UserId"];
				orderInfo.Username = (string)reader["Username"];
				orderInfo.ReferralUserId = (int)reader["ReferralUserId"];
				if (System.DBNull.Value != reader["ReferralPath"])
				{
					orderInfo.ReferralPath = (string)reader["ReferralPath"];
				}
				if (System.DBNull.Value != reader["EmailAddress"])
				{
					orderInfo.EmailAddress = (string)reader["EmailAddress"];
				}
				if (System.DBNull.Value != reader["RealName"])
				{
					orderInfo.RealName = (string)reader["RealName"];
				}
				if (System.DBNull.Value != reader["QQ"])
				{
					orderInfo.QQ = (string)reader["QQ"];
				}
				if (System.DBNull.Value != reader["Wangwang"])
				{
					orderInfo.Wangwang = (string)reader["Wangwang"];
				}
				if (System.DBNull.Value != reader["MSN"])
				{
					orderInfo.MSN = (string)reader["MSN"];
				}
				if (System.DBNull.Value != reader["ShippingRegion"])
				{
					orderInfo.ShippingRegion = (string)reader["ShippingRegion"];
				}
				if (System.DBNull.Value != reader["Address"])
				{
					orderInfo.Address = (string)reader["Address"];
				}
				if (System.DBNull.Value != reader["ZipCode"])
				{
					orderInfo.ZipCode = (string)reader["ZipCode"];
				}
				if (System.DBNull.Value != reader["ShipTo"])
				{
					orderInfo.ShipTo = (string)reader["ShipTo"];
				}
				if (System.DBNull.Value != reader["TelPhone"])
				{
					orderInfo.TelPhone = (string)reader["TelPhone"];
				}
				if (System.DBNull.Value != reader["CellPhone"])
				{
					orderInfo.CellPhone = (string)reader["CellPhone"];
				}
				if (System.DBNull.Value != reader["ShipToDate"])
				{
					orderInfo.ShipToDate = (string)reader["ShipToDate"];
				}
				if (System.DBNull.Value != reader["ShippingModeId"])
				{
					orderInfo.ShippingModeId = (int)reader["ShippingModeId"];
				}
				if (System.DBNull.Value != reader["PointExchange"])
				{
					orderInfo.PointExchange = (int)reader["PointExchange"];
				}
				if (System.DBNull.Value != reader["ModeName"])
				{
					orderInfo.ModeName = (string)reader["ModeName"];
				}
				if (System.DBNull.Value != reader["RealShippingModeId"])
				{
					orderInfo.RealShippingModeId = (int)reader["RealShippingModeId"];
				}
				if (System.DBNull.Value != reader["RealModeName"])
				{
					orderInfo.RealModeName = (string)reader["RealModeName"];
				}
				if (System.DBNull.Value != reader["RegionId"])
				{
					orderInfo.RegionId = (int)reader["RegionId"];
				}
				if (System.DBNull.Value != reader["Freight"])
				{
					orderInfo.Freight = (decimal)reader["Freight"];
				}
				if (System.DBNull.Value != reader["AdjustedFreight"])
				{
					orderInfo.AdjustedFreight = (decimal)reader["AdjustedFreight"];
				}
				if (System.DBNull.Value != reader["ShipOrderNumber"])
				{
					orderInfo.ShipOrderNumber = (string)reader["ShipOrderNumber"];
				}
				if (System.DBNull.Value != reader["ExpressCompanyName"])
				{
					orderInfo.ExpressCompanyName = (string)reader["ExpressCompanyName"];
				}
				if (System.DBNull.Value != reader["ExpressCompanyAbb"])
				{
					orderInfo.ExpressCompanyAbb = (string)reader["ExpressCompanyAbb"];
				}
				if (System.DBNull.Value != reader["PaymentTypeId"])
				{
					orderInfo.PaymentTypeId = (int)reader["PaymentTypeId"];
				}
				if (System.DBNull.Value != reader["PaymentType"])
				{
					orderInfo.PaymentType = (string)reader["PaymentType"];
				}
				if (System.DBNull.Value != reader["PayCharge"])
				{
					orderInfo.PayCharge = (decimal)reader["PayCharge"];
				}
				if (System.DBNull.Value != reader["RefundStatus"])
				{
					orderInfo.RefundStatus = (RefundStatus)reader["RefundStatus"];
				}
				if (System.DBNull.Value != reader["RefundAmount"])
				{
					orderInfo.RefundAmount = (decimal)reader["RefundAmount"];
				}
				if (System.DBNull.Value != reader["RefundRemark"])
				{
					orderInfo.RefundRemark = (string)reader["RefundRemark"];
				}
				if (System.DBNull.Value != reader["Gateway"])
				{
					orderInfo.Gateway = (string)reader["Gateway"];
				}
				if (System.DBNull.Value != reader["ReducedPromotionId"])
				{
					orderInfo.ReducedPromotionId = (int)reader["ReducedPromotionId"];
				}
				if (System.DBNull.Value != reader["ReducedPromotionName"])
				{
					orderInfo.ReducedPromotionName = (string)reader["ReducedPromotionName"];
				}
				if (System.DBNull.Value != reader["ReducedPromotionAmount"])
				{
					orderInfo.ReducedPromotionAmount = (decimal)reader["ReducedPromotionAmount"];
				}
				if (System.DBNull.Value != reader["IsReduced"])
				{
					orderInfo.IsReduced = (bool)reader["IsReduced"];
				}
				if (System.DBNull.Value != reader["SentTimesPointPromotionId"])
				{
					orderInfo.SentTimesPointPromotionId = (int)reader["SentTimesPointPromotionId"];
				}
				if (System.DBNull.Value != reader["SentTimesPointPromotionName"])
				{
					orderInfo.SentTimesPointPromotionName = (string)reader["SentTimesPointPromotionName"];
				}
				if (System.DBNull.Value != reader["IsSendTimesPoint"])
				{
					orderInfo.IsSendTimesPoint = (bool)reader["IsSendTimesPoint"];
				}
				if (System.DBNull.Value != reader["TimesPoint"])
				{
					orderInfo.TimesPoint = (decimal)reader["TimesPoint"];
				}
				if (System.DBNull.Value != reader["FreightFreePromotionId"])
				{
					orderInfo.FreightFreePromotionId = (int)reader["FreightFreePromotionId"];
				}
				if (System.DBNull.Value != reader["FreightFreePromotionName"])
				{
					orderInfo.FreightFreePromotionName = (string)reader["FreightFreePromotionName"];
				}
				if (System.DBNull.Value != reader["IsFreightFree"])
				{
					orderInfo.IsFreightFree = (bool)reader["IsFreightFree"];
				}
				if (System.DBNull.Value != reader["DiscountAmount"])
				{
					orderInfo.DiscountAmount = (decimal)reader["DiscountAmount"];
				}
				if (System.DBNull.Value != reader["CouponName"])
				{
					orderInfo.CouponName = (string)reader["CouponName"];
				}
				if (System.DBNull.Value != reader["CouponCode"])
				{
					orderInfo.CouponCode = (string)reader["CouponCode"];
				}
				if (System.DBNull.Value != reader["CouponAmount"])
				{
					orderInfo.CouponAmount = (decimal)reader["CouponAmount"];
				}
				if (System.DBNull.Value != reader["CouponValue"])
				{
					orderInfo.CouponValue = (decimal)reader["CouponValue"];
				}
				if (System.DBNull.Value != reader["RedPagerActivityName"])
				{
					orderInfo.RedPagerActivityName = (string)reader["RedPagerActivityName"];
				}
				if (System.DBNull.Value != reader["RedPagerID"])
				{
					orderInfo.RedPagerID = new int?((int)reader["RedPagerID"]);
				}
				if (System.DBNull.Value != reader["RedPagerOrderAmountCanUse"])
				{
					orderInfo.RedPagerOrderAmountCanUse = (decimal)reader["RedPagerOrderAmountCanUse"];
				}
				if (System.DBNull.Value != reader["RedPagerAmount"])
				{
					orderInfo.RedPagerAmount = (decimal)reader["RedPagerAmount"];
				}
				if (System.DBNull.Value != reader["GroupBuyId"])
				{
					orderInfo.GroupBuyId = (int)reader["GroupBuyId"];
				}
				if (System.DBNull.Value != reader["CountDownBuyId"])
				{
					orderInfo.CountDownBuyId = (int)reader["CountDownBuyId"];
				}
				if (System.DBNull.Value != reader["Bundlingid"])
				{
					orderInfo.BundlingID = (int)reader["Bundlingid"];
				}
				if (System.DBNull.Value != reader["BundlingPrice"])
				{
					orderInfo.BundlingPrice = (decimal)reader["BundlingPrice"];
				}
				if (System.DBNull.Value != reader["NeedPrice"])
				{
					orderInfo.NeedPrice = (decimal)reader["NeedPrice"];
				}
				if (System.DBNull.Value != reader["GroupBuyStatus"])
				{
					orderInfo.GroupBuyStatus = (GroupBuyStatus)reader["GroupBuyStatus"];
				}
				if (System.DBNull.Value != reader["Tax"])
				{
					orderInfo.Tax = (decimal)reader["Tax"];
				}
				else
				{
					orderInfo.Tax = 0m;
				}
				if (System.DBNull.Value != reader["InvoiceTitle"])
				{
					orderInfo.InvoiceTitle = (string)reader["InvoiceTitle"];
				}
				else
				{
					orderInfo.InvoiceTitle = "";
				}
				if (System.DBNull.Value != reader["ReferralUserId"] && !string.IsNullOrEmpty(reader["ReferralUserId"].ToString()))
				{
					orderInfo.ReferralUserId = (int)reader["ReferralUserId"];
				}
				if (System.DBNull.Value != reader["FirstCommission"])
				{
					orderInfo.FirstCommission = (decimal)reader["FirstCommission"];
				}
				if (System.DBNull.Value != reader["SecondCommission"])
				{
					orderInfo.SecondCommission = (decimal)reader["SecondCommission"];
				}
				if (System.DBNull.Value != reader["ThirdCommission"])
				{
					orderInfo.ThirdCommission = (decimal)reader["ThirdCommission"];
				}
				if (System.DBNull.Value != reader["ActivitiesId"])
				{
					orderInfo.ActivitiesId = (string)reader["ActivitiesId"];
				}
				if (System.DBNull.Value != reader["ActivitiesName"])
				{
					orderInfo.ActivitiesName = (string)reader["ActivitiesName"];
				}
				if (System.DBNull.Value != reader["OldAddress"])
				{
					orderInfo.OldAddress = (string)reader["OldAddress"];
				}
				if (System.DBNull.Value != reader["PointToCash"])
				{
					orderInfo.PointToCash = (decimal)reader["PointToCash"];
				}
				if (System.DBNull.Value != reader["SplitState"])
				{
					orderInfo.SplitState = (int)reader["SplitState"];
				}
				orderInfo.BargainDetialId = (int)reader["BargainDetialId"];
				result = orderInfo;
			}
			return result;
		}

		public static LineItemInfo PopulateLineItem(IDataRecord reader)
		{
			LineItemInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				LineItemInfo lineItemInfo = new LineItemInfo();
				lineItemInfo.ID = (int)reader["ID"];
				lineItemInfo.SkuId = (string)reader["SkuId"];
				lineItemInfo.ProductId = (int)reader["ProductId"];
				if (reader["SKU"] != System.DBNull.Value)
				{
					lineItemInfo.SKU = (string)reader["SKU"];
				}
				lineItemInfo.Quantity = (int)reader["Quantity"];
				lineItemInfo.ShipmentQuantity = (int)reader["ShipmentQuantity"];
				lineItemInfo.ItemCostPrice = (decimal)reader["CostPrice"];
				lineItemInfo.ItemListPrice = (decimal)reader["ItemListPrice"];
				lineItemInfo.ItemAdjustedPrice = (decimal)reader["ItemAdjustedPrice"];
				lineItemInfo.ItemDescription = (string)reader["ItemDescription"];
				lineItemInfo.OrderItemsStatus = (OrderStatus)System.Enum.Parse(typeof(OrderStatus), reader["OrderItemsStatus"].ToString());
				lineItemInfo.OrderStatus = (OrderStatus)System.Enum.Parse(typeof(OrderStatus), reader["OrderStatus"].ToString());
				lineItemInfo.ItemsCommission = (decimal)reader["ItemsCommission"];
				lineItemInfo.ItemAdjustedCommssion = (decimal)reader["ItemAdjustedCommssion"];
				lineItemInfo.SecondItemsCommission = (decimal)reader["SecondItemsCommission"];
				lineItemInfo.ThirdItemsCommission = (decimal)reader["ThirdItemsCommission"];
				if (reader["ThumbnailsUrl"] != System.DBNull.Value)
				{
					lineItemInfo.ThumbnailsUrl = (string)reader["ThumbnailsUrl"];
				}
				lineItemInfo.ItemWeight = (decimal)reader["Weight"];
				if (System.DBNull.Value != reader["SKUContent"])
				{
					lineItemInfo.SKUContent = (string)reader["SKUContent"];
				}
				if (System.DBNull.Value != reader["PromotionId"])
				{
					lineItemInfo.PromotionId = (int)reader["PromotionId"];
				}
				if (System.DBNull.Value != reader["PromotionName"])
				{
					lineItemInfo.PromotionName = (string)reader["PromotionName"];
				}
				if (System.DBNull.Value != reader["PointNumber"])
				{
					lineItemInfo.PointNumber = (int)reader["PointNumber"];
				}
				if (System.DBNull.Value != reader["Type"])
				{
					lineItemInfo.Type = (int)reader["Type"];
				}
				if (System.DBNull.Value != reader["ReturnMoney"])
				{
					lineItemInfo.ReturnMoney = (decimal)reader["ReturnMoney"];
				}
				if (System.DBNull.Value != reader["DiscountAverage"])
				{
					lineItemInfo.DiscountAverage = (decimal)reader["DiscountAverage"];
				}
				if (System.DBNull.Value != reader["OrderID"])
				{
					lineItemInfo.OrderID = (string)reader["OrderID"];
				}
				if (System.DBNull.Value != reader["IsAdminModify"])
				{
					lineItemInfo.IsAdminModify = (bool)reader["IsAdminModify"];
				}
				lineItemInfo.LimitedTimeDiscountId = (int)reader["LimitedTimeDiscountId"];
				result = lineItemInfo;
			}
			return result;
		}

		public static UserStatisticsInfo PopulateUserStatistics(IDataRecord reader)
		{
			UserStatisticsInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				UserStatisticsInfo userStatisticsInfo = new UserStatisticsInfo();
				if (reader["RegionId"] != System.DBNull.Value)
				{
					userStatisticsInfo.RegionId = (long)((int)reader["RegionId"]);
				}
				if (reader["Usercounts"] != System.DBNull.Value)
				{
					userStatisticsInfo.Usercounts = (int)reader["Usercounts"];
				}
				if (reader["AllUserCounts"] != System.DBNull.Value)
				{
					userStatisticsInfo.AllUserCounts = (int)reader["AllUserCounts"];
				}
				result = userStatisticsInfo;
			}
			return result;
		}

		public static VoteInfo PopulateVote(IDataRecord reader)
		{
			VoteInfo voteInfo = new VoteInfo();
			voteInfo.VoteId = (long)reader["VoteId"];
			voteInfo.VoteName = (string)reader["VoteName"];
			voteInfo.IsBackup = (bool)reader["IsBackup"];
			voteInfo.MaxCheck = (int)reader["MaxCheck"];
			if (reader["ImageUrl"] != System.DBNull.Value)
			{
				voteInfo.ImageUrl = (string)reader["ImageUrl"];
			}
			voteInfo.StartDate = (System.DateTime)reader["StartDate"];
			voteInfo.EndDate = (System.DateTime)reader["EndDate"];
			voteInfo.Description = (string)reader["Description"];
			voteInfo.MemberGrades = (string)reader["MemberGrades"];
			voteInfo.IsMultiCheck = (bool)reader["IsMultiCheck"];
			voteInfo.VoteCounts = (int)reader["VoteCounts"];
			return voteInfo;
		}

		public static ShippingAddressInfo PopulateShippingAddress(IDataRecord reader)
		{
			ShippingAddressInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				ShippingAddressInfo shippingAddressInfo = new ShippingAddressInfo();
				shippingAddressInfo.ShippingId = (int)reader["ShippingId"];
				shippingAddressInfo.ShipTo = (string)reader["ShipTo"];
				shippingAddressInfo.RegionId = (int)reader["RegionId"];
				shippingAddressInfo.UserId = (int)reader["UserId"];
				shippingAddressInfo.Address = (string)reader["Address"];
				shippingAddressInfo.Zipcode = (string)reader["Zipcode"];
				shippingAddressInfo.IsDefault = (bool)reader["IsDefault"];
				if (reader["TelPhone"] != System.DBNull.Value)
				{
					shippingAddressInfo.TelPhone = (string)reader["TelPhone"];
				}
				if (reader["CellPhone"] != System.DBNull.Value)
				{
					shippingAddressInfo.CellPhone = (string)reader["CellPhone"];
				}
				result = shippingAddressInfo;
			}
			return result;
		}

		public static MemberClientSet PopulateMemberClientSet(IDataReader reader)
		{
			MemberClientSet result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				MemberClientSet memberClientSet = new MemberClientSet();
				memberClientSet.ClientTypeId = (int)reader["ClientTypeId"];
				if (System.DateTime.Compare((System.DateTime)reader["StartTime"], System.Convert.ToDateTime("1900-01-01")) != 0)
				{
					memberClientSet.StartTime = new System.DateTime?((System.DateTime)reader["StartTime"]);
				}
				if (System.DateTime.Compare((System.DateTime)reader["EndTime"], System.Convert.ToDateTime("1900-01-01")) != 0)
				{
					memberClientSet.EndTime = new System.DateTime?((System.DateTime)reader["EndTime"]);
				}
				memberClientSet.LastDay = (int)reader["LastDay"];
				if (reader["ClientChar"] != System.DBNull.Value)
				{
					memberClientSet.ClientChar = (string)reader["ClientChar"];
				}
				memberClientSet.ClientValue = (decimal)reader["ClientValue"];
				result = memberClientSet;
			}
			return result;
		}

		public static TopicInfo PopulateTopic(IDataReader reader)
		{
			TopicInfo topicInfo = new TopicInfo();
			topicInfo.TopicId = (int)reader["TopicId"];
			topicInfo.Title = (string)reader["Title"];
			if (reader["IconUrl"] != System.DBNull.Value)
			{
				topicInfo.IconUrl = (string)reader["IconUrl"];
			}
			topicInfo.Content = (string)reader["Content"];
			topicInfo.AddedDate = (System.DateTime)reader["AddedDate"];
			topicInfo.IsRelease = (bool)reader["IsRelease"];
			return topicInfo;
		}

		public static GroupBuyInfo PopulateGroupBuy(IDataReader reader)
		{
			GroupBuyInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				GroupBuyInfo groupBuyInfo = new GroupBuyInfo();
				groupBuyInfo.GroupBuyId = (int)reader["GroupBuyId"];
				groupBuyInfo.ProductId = (int)reader["ProductId"];
				if (System.DBNull.Value != reader["NeedPrice"])
				{
					groupBuyInfo.NeedPrice = (decimal)reader["NeedPrice"];
				}
				groupBuyInfo.MaxCount = (int)reader["MaxCount"];
				groupBuyInfo.StartDate = (System.DateTime)reader["StartDate"];
				groupBuyInfo.EndDate = (System.DateTime)reader["EndDate"];
				if (System.DBNull.Value != reader["Content"])
				{
					groupBuyInfo.Content = (string)reader["Content"];
				}
				groupBuyInfo.Status = (GroupBuyStatus)((int)reader["Status"]);
				groupBuyInfo.Price = (decimal)reader["Price"];
				groupBuyInfo.Count = (int)reader["Count"];
				groupBuyInfo.SoldCount = (int)reader["SoldCount"];
				result = groupBuyInfo;
			}
			return result;
		}

		public static DistributorsInfo PopulateDistributorInfo(IDataReader reader)
		{
			DistributorsInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				DistributorsInfo distributorsInfo = new DistributorsInfo();
				distributorsInfo.UserId = (int)reader["UserId"];
				distributorsInfo.StoreName = (string)reader["StoreName"];
				if (reader["RequestAccount"] != System.DBNull.Value)
				{
					distributorsInfo.RequestAccount = (string)reader["RequestAccount"];
				}
				if (reader["Logo"] != System.DBNull.Value)
				{
					distributorsInfo.Logo = (string)reader["Logo"];
				}
				distributorsInfo.BackImage = (string)reader["BackImage"];
				if (reader["AccountTime"] != System.DBNull.Value)
				{
					distributorsInfo.AccountTime = new System.DateTime?((System.DateTime)reader["AccountTime"]);
				}
				if (reader["GradeId"] != System.DBNull.Value)
				{
					distributorsInfo.GradeId = (int)reader["GradeId"];
				}
				distributorsInfo.OrdersTotal = (decimal)reader["OrdersTotal"];
				if (reader["ReferralPath"] != System.DBNull.Value)
				{
					distributorsInfo.ReferralPath = (string)reader["ReferralPath"];
				}
				distributorsInfo.ReferralUserId = (int)reader["ReferralUserId"];
				distributorsInfo.ReferralOrders = (int)reader["ReferralOrders"];
				distributorsInfo.ReferralBlance = (decimal)reader["ReferralBlance"];
				distributorsInfo.ReferralRequestBalance = (decimal)reader["ReferralRequestBalance"];
				distributorsInfo.CreateTime = (System.DateTime)reader["CreateTime"];
				distributorsInfo.ReferralStatus = (int)reader["ReferralStatus"];
				if (reader["StoreDescription"] != System.DBNull.Value)
				{
					distributorsInfo.StoreDescription = (string)reader["StoreDescription"];
				}
				if (reader["DistributorGradeId"] != System.DBNull.Value)
				{
					distributorsInfo.DistriGradeId = (int)reader["DistributorGradeId"];
				}
				if (reader["StoreCard"] != System.DBNull.Value)
				{
					distributorsInfo.StoreCard = (string)reader["StoreCard"];
				}
				if (reader["CardCreatTime"] != System.DBNull.Value)
				{
					distributorsInfo.CardCreatTime = (System.DateTime)reader["CardCreatTime"];
				}
				result = distributorsInfo;
			}
			return result;
		}

		public static DistributorGradeInfo PopulateDistributorGradeInfo(IDataReader reader)
		{
			DistributorGradeInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				DistributorGradeInfo distributorGradeInfo = new DistributorGradeInfo();
				distributorGradeInfo.GradeId = (int)reader["GradeId"];
				distributorGradeInfo.Name = (string)reader["Name"];
				if (reader["Description"] != System.DBNull.Value)
				{
					distributorGradeInfo.Description = (string)reader["Description"];
				}
				if (reader["CommissionsLimit"] != System.DBNull.Value)
				{
					distributorGradeInfo.CommissionsLimit = (decimal)reader["CommissionsLimit"];
				}
				if (reader["FirstCommissionRise"] != System.DBNull.Value)
				{
					distributorGradeInfo.FirstCommissionRise = (decimal)reader["FirstCommissionRise"];
				}
				if (reader["SecondCommissionRise"] != System.DBNull.Value)
				{
					distributorGradeInfo.SecondCommissionRise = (decimal)reader["SecondCommissionRise"];
				}
				if (reader["ThirdCommissionRise"] != System.DBNull.Value)
				{
					distributorGradeInfo.ThirdCommissionRise = (decimal)reader["ThirdCommissionRise"];
				}
				if (reader["IsDefault"] != System.DBNull.Value)
				{
					distributorGradeInfo.IsDefault = (bool)reader["IsDefault"];
				}
				distributorGradeInfo.Ico = (string)reader["Ico"];
				distributorGradeInfo.AddCommission = (decimal)reader["AddCommission"];
				result = distributorGradeInfo;
			}
			return result;
		}

		public static RedPagerActivityInfo PopulateRedPagerActivityInfo(IDataReader reader)
		{
			RedPagerActivityInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				result = new RedPagerActivityInfo
				{
					RedPagerActivityId = (int)reader["RedPagerActivityId"],
					Name = (string)reader["Name"],
					CategoryId = (int)reader["CategoryId"],
					MinOrderAmount = (decimal)reader["MinOrderAmount"],
					MaxGetTimes = (int)reader["MaxGetTimes"],
					ItemAmountLimit = (decimal)reader["ItemAmountLimit"],
					OrderAmountCanUse = (decimal)reader["OrderAmountCanUse"],
					ExpiryDays = (int)reader["ExpiryDays"],
					IsOpen = (bool)reader["IsOpen"]
				};
			}
			return result;
		}

		public static OrderRedPagerInfo PopulateOrderRedPagerInfo(IDataReader reader)
		{
			OrderRedPagerInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				result = new OrderRedPagerInfo
				{
					OrderID = (string)reader["OrderID"],
					RedPagerActivityId = (int)reader["RedPagerActivityId"],
					RedPagerActivityName = (string)reader["RedPagerActivityName"],
					MaxGetTimes = (int)reader["MaxGetTimes"],
					AlreadyGetTimes = (int)reader["AlreadyGetTimes"],
					ItemAmountLimit = (decimal)reader["ItemAmountLimit"],
					OrderAmountCanUse = (decimal)reader["OrderAmountCanUse"],
					ExpiryDays = (int)reader["ExpiryDays"],
					UserID = (int)reader["UserID"]
				};
			}
			return result;
		}

		public static UserRedPagerInfo PopulateUserRedPagerInfo(IDataReader reader)
		{
			UserRedPagerInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				result = new UserRedPagerInfo
				{
					RedPagerID = (int)reader["RedPagerID"],
					Amount = (decimal)reader["Amount"],
					UserID = (int)reader["UserID"],
					OrderID = (string)reader["OrderID"],
					RedPagerActivityName = (string)reader["RedPagerActivityName"],
					OrderAmountCanUse = (decimal)reader["OrderAmountCanUse"],
					CreateTime = (System.DateTime)reader["CreateTime"],
					ExpiryTime = (System.DateTime)reader["ExpiryTime"],
					IsUsed = (bool)reader["IsUsed"]
				};
			}
			return result;
		}

		public static SendRedpackRecordInfo PopulateSendRedpackRecordInfo(IDataReader reader)
		{
			SendRedpackRecordInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				SendRedpackRecordInfo sendRedpackRecordInfo = new SendRedpackRecordInfo();
				sendRedpackRecordInfo.ID = (int)reader["ID"];
				sendRedpackRecordInfo.BalanceDrawRequestID = (int)reader["BalanceDrawRequestID"];
				sendRedpackRecordInfo.UserID = (int)reader["UserID"];
				sendRedpackRecordInfo.OpenID = (string)reader["OpenID"];
				sendRedpackRecordInfo.Amount = (int)reader["Amount"];
				sendRedpackRecordInfo.ActName = (string)reader["ActName"];
				sendRedpackRecordInfo.Wishing = (string)reader["Wishing"];
				sendRedpackRecordInfo.ClientIP = (string)reader["ClientIP"];
				sendRedpackRecordInfo.IsSend = (bool)reader["IsSend"];
				if (reader["SendTime"] != System.DBNull.Value)
				{
					sendRedpackRecordInfo.SendTime = (System.DateTime)reader["SendTime"];
				}
				result = sendRedpackRecordInfo;
			}
			return result;
		}

		public static FreightTemplate PopulateTemplateMessage(IDataReader reader)
		{
			FreightTemplate result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				FreightTemplate freightTemplate = new FreightTemplate();
				freightTemplate.FreeShip = (bool)reader["FreeShip"];
				freightTemplate.HasFree = (bool)reader["HasFree"];
				if (System.DBNull.Value != reader["MUnit"])
				{
					freightTemplate.MUnit = int.Parse(reader["MUnit"].ToString());
				}
				freightTemplate.Name = (string)reader["Name"];
				freightTemplate.TemplateId = (int)reader["TemplateId"];
				result = freightTemplate;
			}
			return result;
		}
	}
}
