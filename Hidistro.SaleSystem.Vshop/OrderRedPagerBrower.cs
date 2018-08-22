using Hidistro.Entities.VShop;
using Hidistro.SqlDal.VShop;
using System;

namespace Hidistro.SaleSystem.Vshop
{
	public class OrderRedPagerBrower
	{
		public static bool CreateOrderRedPager(string orderid, decimal ordertotalprice, int userid)
		{
			return new OrderRedPagerDao().CreateOrderRedPager(orderid, ordertotalprice, userid);
		}

		public static bool CreateOrderRedPager(OrderRedPagerInfo orderredpager)
		{
			return new OrderRedPagerDao().CreateOrderRedPager(orderredpager);
		}

		public static bool UpdateOrderRedPager(OrderRedPagerInfo orderredpager)
		{
			return new OrderRedPagerDao().UpdateOrderRedPager(orderredpager);
		}

		public static OrderRedPagerInfo GetOrderRedPagerInfo(string orderid)
		{
			return new OrderRedPagerDao().GetOrderRedPagerInfo(orderid);
		}

		public static bool SetIsOpen(int orderredpagerid, bool isopen)
		{
			return new OrderRedPagerDao().SetIsOpen(orderredpagerid, isopen);
		}

		public static bool DelOrderRedPager(int orderid)
		{
			return new OrderRedPagerDao().DelOrderRedPager(orderid);
		}
	}
}
