using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.SqlDal.Promotions;
using System;

namespace Hidistro.ControlPanel.Promotions
{
	public class LimitedTimeDiscountHelper
	{
		private static LimitedTimeDiscountDao _act = new LimitedTimeDiscountDao();

		public static int AddLimitedTimeDiscount(LimitedTimeDiscountInfo info)
		{
			return LimitedTimeDiscountHelper._act.AddLimitedTimeDiscount(info);
		}

		public static DbQueryResult GetDiscountQuery(ActivitySearch query)
		{
			return LimitedTimeDiscountHelper._act.GetDiscountQuery(query);
		}

		public static bool UpdateDiscountStatus(int Id, DiscountStatus status)
		{
			return LimitedTimeDiscountHelper._act.UpdateDiscountStatus(Id, status);
		}

		public static bool UpdateLimitedTimeDiscount(LimitedTimeDiscountInfo info)
		{
			return LimitedTimeDiscountHelper._act.UpdateLimitedTimeDiscount(info);
		}

		public static LimitedTimeDiscountProductInfo GetDiscountProductInfoByProductId(int productId)
		{
			return LimitedTimeDiscountHelper._act.GetDiscountProductInfoByProductId(productId);
		}

		public static LimitedTimeDiscountInfo GetDiscountInfo(int Id)
		{
			return LimitedTimeDiscountHelper._act.GetDiscountInfo(Id);
		}

		public static LimitedTimeDiscountProductInfo GetDiscountProductInfoById(int id)
		{
			return LimitedTimeDiscountHelper._act.GetDiscountProductInfoById(id);
		}

		public static DbQueryResult GetDiscountProduct(ProductQuery query)
		{
			return LimitedTimeDiscountHelper._act.GetDiscountProduct(query);
		}

		public static DbQueryResult GetDiscountProducted(ProductQuery query, int discountId)
		{
			return LimitedTimeDiscountHelper._act.GetDiscountProducted(query, discountId);
		}

		public static bool ChangeDiscountProductStatus(string ids, int status)
		{
			return LimitedTimeDiscountHelper._act.ChangeDiscountProductStatus(ids, status);
		}

		public static bool DeleteDiscountProduct(string ids)
		{
			return LimitedTimeDiscountHelper._act.DeleteDiscountProduct(ids);
		}

		public static bool AddLimitedTimeDiscountProduct(LimitedTimeDiscountProductInfo info)
		{
			return LimitedTimeDiscountHelper._act.AddLimitedTimeDiscountProduct(info);
		}

		public static bool UpdateLimitedTimeDiscountProduct(LimitedTimeDiscountProductInfo info)
		{
			return LimitedTimeDiscountHelper._act.UpdateLimitedTimeDiscountProduct(info);
		}

		public static bool UpdateLimitedTimeDiscountProductById(LimitedTimeDiscountProductInfo info)
		{
			return LimitedTimeDiscountHelper._act.UpdateLimitedTimeDiscountProductById(info);
		}
	}
}
