using System;

namespace Hidistro.Entities.Members
{
	public enum IntegralDetailStatus
	{
		OrderToIntegral = 1,
		SignToIntegral,
		ActivityToIntegral,
		ReturnAndExchangeToIntegral,
		NowArrived,
		ActivityConsumption,
		IntegralExchange,
		Other = 0,
		AdjustingIntegral
	}
}
