using Hidistro.Core;
using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Commodities
{
	public class ProductQuery : Pagination
	{
		[HtmlCoding]
		public string Keywords
		{
			get;
			set;
		}

		[HtmlCoding]
		public string ProductCode
		{
			get;
			set;
		}

		public int? CategoryId
		{
			get;
			set;
		}

		public string MaiCategoryPath
		{
			get;
			set;
		}

		public int? BrandId
		{
			get;
			set;
		}

		public int? TagId
		{
			get;
			set;
		}

		public decimal? MinSalePrice
		{
			get;
			set;
		}

		public decimal? MaxSalePrice
		{
			get;
			set;
		}

		public ProductSaleStatus SaleStatus
		{
			get;
			set;
		}

		public string TwoSaleStatus
		{
			get;
			set;
		}

		public int? IsMakeTaobao
		{
			get;
			set;
		}

		public bool? IsIncludePromotionProduct
		{
			get;
			set;
		}

		public System.DateTime? StartDate
		{
			get;
			set;
		}

		public System.DateTime? EndDate
		{
			get;
			set;
		}

		public int? TypeId
		{
			get;
			set;
		}

		public int? Stock
		{
			get;
			set;
		}

		public int? TopicId
		{
			get;
			set;
		}

		public bool? IsIncludeHomeProduct
		{
			get;
			set;
		}

		public decimal? minPrice
		{
			get;
			set;
		}

		public decimal? maxPrice
		{
			get;
			set;
		}

		public string selectQuery
		{
			get;
			set;
		}
	}
}
