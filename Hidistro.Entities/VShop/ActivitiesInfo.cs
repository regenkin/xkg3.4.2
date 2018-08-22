using Hidistro.Entities.Sales;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.VShop
{
	public class ActivitiesInfo
	{
		public int ActivitiesId
		{
			get;
			set;
		}

		public string ActivitiesName
		{
			get;
			set;
		}

		public int ActivitiesType
		{
			get;
			set;
		}

		public decimal MeetMoney
		{
			get;
			set;
		}

		public decimal ReductionMoney
		{
			get;
			set;
		}

		public string ActivitiesDescription
		{
			get;
			set;
		}

		public System.DateTime StartTime
		{
			get;
			set;
		}

		public System.DateTime EndTIme
		{
			get;
			set;
		}

		public string CloseRemark
		{
			get;
			set;
		}

		public System.Collections.Generic.List<ShoppingCartItemInfo> LineItems
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}
	}
}
