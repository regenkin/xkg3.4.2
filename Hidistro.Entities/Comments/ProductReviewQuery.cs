using Hidistro.Core;
using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Comments
{
	public class ProductReviewQuery : Pagination
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

		public int productId
		{
			get;
			set;
		}
	}
}
