using System;

namespace Hidistro.Entities.Sales
{
	[System.Serializable]
	public class StatisticsInfo
	{
		public int OrderNumbWaitConsignment
		{
			get;
			set;
		}

		public int GroupBuyNumWaitRefund
		{
			get;
			set;
		}

		public int OrderNumbToday
		{
			get;
			set;
		}

		public decimal OrderPriceToday
		{
			get;
			set;
		}

		public decimal OrderProfitToday
		{
			get;
			set;
		}

		public int UserNewAddToday
		{
			get;
			set;
		}

		public int OrderNumbYesterday
		{
			get;
			set;
		}

		public decimal OrderPriceYesterday
		{
			get;
			set;
		}

		public decimal OrderProfitYesterday
		{
			get;
			set;
		}

		public int UserNumb
		{
			get;
			set;
		}

		public int ProductNumbOnSale
		{
			get;
			set;
		}

		public int ProductNumbInStock
		{
			get;
			set;
		}

		public int UserNumbBirthdayToday
		{
			get;
			set;
		}

		public decimal AlreadyPaidOrdersNum
		{
			get;
			set;
		}

		public decimal AreadyPaidOrdersAmount
		{
			get;
			set;
		}

		public int TodayFinishOrder
		{
			get;
			set;
		}

		public int YesterdayFinishOrder
		{
			get;
			set;
		}

		public int UserNewAddYesterToday
		{
			get;
			set;
		}

		public int TotalMembers
		{
			get;
			set;
		}

		public int TotalProducts
		{
			get;
			set;
		}

		public decimal OrderPriceMonth
		{
			get;
			set;
		}

		public int TodayVipCardNumber
		{
			get;
			set;
		}

		public int YesterTodayVipCardNumber
		{
			get;
			set;
		}
	}
}
