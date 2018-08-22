using System;

namespace Hidistro.Entities.Orders
{
	public enum OrderStatus
	{
		All,
		WaitBuyerPay,
		BuyerAlreadyPaid,
		SellerAlreadySent,
		Closed,
		Finished,
		ApplyForRefund,
		ApplyForReturns,
		ApplyForReplacement,
		Refunded,
		Returned,
		Today,
		Deleted,
		History = 99
	}
}
