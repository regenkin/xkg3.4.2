using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SqlDal.Promotions;
using System;

namespace Hidistro.SaleSystem.Vshop
{
	public static class CouponProcessor
	{
		public static SendCouponResult SendCouponToMember(int couponId, int userId)
		{
			return new CouponDao().SendCouponToMember(couponId, userId);
		}

		public static void RegisterSendCoupon(string sessionId)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			if (masterSettings.IsRegisterSendCoupon)
			{
				DateTime now = DateTime.Now;
				if (masterSettings.RegisterSendCouponBeginTime.HasValue)
				{
					if (masterSettings.RegisterSendCouponBeginTime.Value > now)
					{
						return;
					}
				}
				if (masterSettings.RegisterSendCouponEndTime.HasValue)
				{
					if (masterSettings.RegisterSendCouponEndTime.Value < now)
					{
						return;
					}
				}
				MemberInfo member = MemberProcessor.GetMember(sessionId);
				if (member != null)
				{
					new CouponDao().SendCouponToMember(masterSettings.RegisterSendCouponId, member.UserId);
				}
			}
		}
	}
}
