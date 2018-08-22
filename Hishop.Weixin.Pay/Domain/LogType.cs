using System;

namespace Hishop.Weixin.Pay.Domain
{
	public enum LogType
	{
		Pay,
		PayNotify,
		NativePay,
		NativePayNotify,
		MicroPay,
		MicroPayNotify,
		Refund,
		RefundNotify,
		RefundQuery,
		OrderQuery,
		DownLoadBill,
		CloseOrder,
		GetTokenOrOpenID,
		GetOrEditAddress,
		ShortUrl,
		UnifiedOrder,
		Report,
		Error,
		GetPrepayID
	}
}
