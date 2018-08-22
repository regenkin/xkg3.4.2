using System;

namespace Hidistro.Messages
{
	internal static class MessageType
	{
		internal const string ChangedDealPassword = "ChangedDealPassword";

		internal const string ChangedPassword = "ChangedPassword";

		internal const string ModifiedLoginPassword = "ModifiedLoginPassword";

		internal const string ForgottenPassword = "ForgottenPassword";

		internal const string NewUserAccountCreated = "NewUserAccountCreated";

		internal const string OrderCreated = "OrderCreated";

		internal const string OrderPayment = "OrderPayment";

		internal const string OrderShipping = "OrderShipping";

		internal const string OrderRefund = "OrderRefund";

		internal const string OrderClosed = "OrderClosed";

		internal const string AcceptDistributorRequest = "AcceptDistributorRequest";

		internal const string BecomeDistributor = "BecomeDistributor";

		internal const string Common = "Common";

		internal const string OrderCreate = "OrderCreate";

		internal const string OrderPay = "OrderPay";

		internal const string ServiceRequest = "ServiceRequest";

		internal const string DrawCashRequest = "DrawCashRequest";

		internal const string ProductAsk = "ProductAsk";

		internal const string DistributorCreate = "DistributorCreate";

		internal const string OrderGetCommission = "OrderGetCommission";

		internal const string ProductCreate = "ProductCreate";

		internal const string PasswordReset = "PasswordReset";

		internal const string DistributorGradeChange = "DistributorGradeChange";

		internal const string DrawCashRelease = "DrawCashRelease";

		internal const string DrawCashReject = "DrawCashReject";

		internal const string AccountLock = "AccountLock";

		internal const string AccountUnLock = "AccountUnLock";

		internal const string DistributorCancel = "DistributorCancel";

		internal const string OrderDeliver = "OrderDeliver";

		internal const string MemberGradeChange = "MemberGradeChange";

		internal const string CouponWillExpired = "CouponWillExpired";

		internal const string OrderGetPoint = "OrderGetPoint";

		internal const string OrderGetCoupon = "OrderGetCoupon";

		internal const string RefundSuccess = "RefundSuccess";

		internal const string PrizeRelease = "PrizeRelease";
	}
}
