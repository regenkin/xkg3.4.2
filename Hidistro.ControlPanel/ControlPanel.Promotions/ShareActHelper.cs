using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SqlDal.Promotions;
using System;
using System.Data;

namespace ControlPanel.Promotions
{
	public class ShareActHelper
	{
		private static ShareActDao _share = new ShareActDao();

		public static int Create(ShareActivityInfo act, ref string msg)
		{
			return ShareActHelper._share.Create(act, ref msg);
		}

		public static bool Update(ShareActivityInfo act, ref string msg)
		{
			return ShareActHelper._share.Update(act, ref msg);
		}

		public static ShareActivityInfo GetAct(int Id)
		{
			return ShareActHelper._share.GetAct(Id);
		}

		public static bool Delete(int Id)
		{
			return ShareActHelper._share.Delete(Id);
		}

		public static DbQueryResult Query(ShareActivitySearch query)
		{
			return ShareActHelper._share.Query(query);
		}

		public static bool AddRecord(ShareActivityRecordInfo record)
		{
			return ShareActHelper._share.AddRecord(record);
		}

		public static int GeTAttendCount(int actId, int shareUser)
		{
			return ShareActHelper._share.GeTAttendCount(actId, shareUser);
		}

		public static bool HasAttend(int actId, int attendUser)
		{
			return ShareActHelper._share.HasAttend(actId, attendUser);
		}

		public static System.Data.DataTable GetShareActivity()
		{
			return ShareActHelper._share.GetShareActivity();
		}

		public static System.Data.DataTable GetOrderRedPager(string OrderID, int UserID)
		{
			return ShareActHelper._share.GetOrderRedPager(OrderID, UserID);
		}

		public static ShareActivityInfo GetShareActivity(int CouponId)
		{
			return ShareActHelper._share.GetShareActivity(CouponId);
		}
	}
}
