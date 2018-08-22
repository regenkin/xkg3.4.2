using Hidistro.Core;
using System;

namespace Hidistro.Entities.Promotions
{
	public class GroupBuyInfo
	{
		public int GroupBuyId
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public decimal NeedPrice
		{
			get;
			set;
		}

		public int Count
		{
			get;
			set;
		}

		public decimal Price
		{
			get;
			set;
		}

		public System.DateTime StartDate
		{
			get;
			set;
		}

		public System.DateTime EndDate
		{
			get;
			set;
		}

		public int MaxCount
		{
			get;
			set;
		}

		[HtmlCoding]
		public string Content
		{
			get;
			set;
		}

		public GroupBuyStatus Status
		{
			get;
			set;
		}

		public int SoldCount
		{
			get;
			set;
		}

		public int ProdcutQuantity
		{
			get;
			set;
		}
	}
}
