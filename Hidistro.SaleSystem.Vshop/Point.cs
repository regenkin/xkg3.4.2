using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SqlDal.Members;
using System;

namespace Hidistro.SaleSystem.Vshop
{
	public static class Point
	{
		public static void SetPointByOrderId(OrderInfo orderInfo)
		{
			int num = 0;
			if (orderInfo.PointExchange > 0)
			{
				num = orderInfo.PointExchange;
			}
			else if (orderInfo.LineItems.Count > 0)
			{
				foreach (LineItemInfo current in orderInfo.LineItems.Values)
				{
					if (current.PointNumber > 0)
					{
						num += current.PointNumber;
					}
				}
			}
			if (num > 0)
			{
				IntegralDetailInfo integralDetailInfo = new IntegralDetailInfo();
				integralDetailInfo.IntegralChange = num;
				integralDetailInfo.IntegralSource = "订单取消，积分返还-订单号：" + orderInfo.OrderId;
				integralDetailInfo.IntegralSourceType = 1;
				integralDetailInfo.IntegralStatus = 4;
				integralDetailInfo.Userid = orderInfo.UserId;
				integralDetailInfo.Remark = "订单取消，积分返还";
				new IntegralDetailDao().AddIntegralDetail(integralDetailInfo, null);
			}
		}
	}
}
