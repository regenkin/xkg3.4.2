using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hidistro.Entities.Commodities
{
	public class ProductInfo
	{
		private System.Collections.Generic.Dictionary<string, SKUItem> skus;

		private SKUItem defaultSku;

		public SKUItem DefaultSku
		{
			get
			{
				SKUItem arg_24_0;
				if ((arg_24_0 = this.defaultSku) == null)
				{
					arg_24_0 = (this.defaultSku = this.Skus.Values.First<SKUItem>());
				}
				return arg_24_0;
			}
		}

		public System.Collections.Generic.Dictionary<string, SKUItem> Skus
		{
			get
			{
				System.Collections.Generic.Dictionary<string, SKUItem> arg_19_0;
				if ((arg_19_0 = this.skus) == null)
				{
					arg_19_0 = (this.skus = new System.Collections.Generic.Dictionary<string, SKUItem>());
				}
				return arg_19_0;
			}
		}

		public string SkuId
		{
			get
			{
				return this.DefaultSku.SkuId;
			}
		}

		public string SKU
		{
			get
			{
				return this.DefaultSku.SKU;
			}
		}

		public decimal Weight
		{
			get
			{
				return this.DefaultSku.Weight;
			}
		}

		public int Stock
		{
			get
			{
				return this.Skus.Values.Sum((SKUItem sku) => sku.Stock);
			}
		}

		public decimal CostPrice
		{
			get
			{
				return this.DefaultSku.CostPrice;
			}
		}

		public decimal MinSalePrice
		{
			get
			{
				decimal[] minSalePrice = new decimal[]
				{
					79228162514264337593543950335m
				};
				foreach (SKUItem current in from sku in this.Skus.Values
				where sku.SalePrice < minSalePrice[0]
				select sku)
				{
					minSalePrice[0] = current.SalePrice;
				}
				return minSalePrice[0];
			}
		}

		public decimal MaxSalePrice
        {
            get
            {
                decimal[] maxSalePrice = new decimal[1];
                foreach (SKUItem current in from sku in this.Skus.Values
                                            where sku.SalePrice > maxSalePrice[0]
                                            select sku)
                {
                    maxSalePrice[0] = current.SalePrice;
                }
                return maxSalePrice[0];
            }
        }

		public decimal SalePrice
		{
			get;
			set;
		}

		public int? TypeId
		{
			get;
			set;
		}

		public int CategoryId
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		[HtmlCoding]
		public string ProductName
		{
			get;
			set;
		}

		[HtmlCoding]
		public string ProductShortName
		{
			get;
			set;
		}

		public string ProductCode
		{
			get;
			set;
		}

		[HtmlCoding]
		public string ShortDescription
		{
			get;
			set;
		}

		public string Unit
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public ProductSaleStatus SaleStatus
		{
			get;
			set;
		}

		public System.DateTime AddedDate
		{
			get;
			set;
		}

		public int VistiCounts
		{
			get;
			set;
		}

		public int SaleCounts
		{
			get;
			set;
		}

		public int ShowSaleCounts
		{
			get;
			set;
		}

		public int DisplaySequence
		{
			get;
			set;
		}

		public string ImageUrl1
		{
			get;
			set;
		}

		public string ImageUrl2
		{
			get;
			set;
		}

		public string ImageUrl3
		{
			get;
			set;
		}

		public string ImageUrl4
		{
			get;
			set;
		}

		public string ImageUrl5
		{
			get;
			set;
		}

		public string ThumbnailUrl40
		{
			get;
			set;
		}

		public string ThumbnailUrl60
		{
			get;
			set;
		}

		public string ThumbnailUrl100
		{
			get;
			set;
		}

		public string ThumbnailUrl160
		{
			get;
			set;
		}

		public string ThumbnailUrl180
		{
			get;
			set;
		}

		public string ThumbnailUrl220
		{
			get;
			set;
		}

		public string ThumbnailUrl310
		{
			get;
			set;
		}

		public string ThumbnailUrl410
		{
			get;
			set;
		}

		public decimal? MarketPrice
		{
			get;
			set;
		}

		public int? BrandId
		{
			get;
			set;
		}

		public string MainCategoryPath
		{
			get;
			set;
		}

		public string ExtendCategoryPath
		{
			get;
			set;
		}

		public bool HasSKU
		{
			get;
			set;
		}

		public bool IsfreeShipping
		{
			get;
			set;
		}

		public long TaobaoProductId
		{
			get;
			set;
		}

		public string Source
		{
			get;
			set;
		}

		public decimal MinShowPrice
		{
			get;
			set;
		}

		public decimal MaxShowPrice
		{
			get;
			set;
		}

		public int FreightTemplateId
		{
			get;
			set;
		}

		public decimal FirstCommission
		{
			get;
			set;
		}

		public decimal SecondCommission
		{
			get;
			set;
		}

		public decimal ThirdCommission
		{
			get;
			set;
		}

		public bool IsSetCommission
		{
			get;
			set;
		}

		public decimal CubicMeter
		{
			get;
			set;
		}

		public decimal FreightWeight
		{
			get;
			set;
		}
	}
}
