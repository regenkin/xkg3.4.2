using System;
using System.Data;

namespace Hidistro.Entities.Commodities
{
	public class ProductBrowseInfo
	{
		public ProductInfo Product
		{
			get;
			set;
		}

		public string BrandName
		{
			get;
			set;
		}

		public string CategoryName
		{
			get;
			set;
		}

		public DataTable DbAttribute
		{
			get;
			set;
		}

		public DataTable DbSKUs
		{
			get;
			set;
		}

		public DataTable DbCorrelatives
		{
			get;
			set;
		}

		public DataTable DBReviews
		{
			get;
			set;
		}

		public DataTable DBConsultations
		{
			get;
			set;
		}
	}
}
