using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Promotions;
using Hidistro.SqlDal.VShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.SaleSystem.Vshop
{
	public static class VshopBrowser
	{
		public static MessageInfo GetMessage(int messageId)
		{
			return new ReplyDao().GetMessage(messageId);
		}

		public static System.Data.DataTable GetHomeProducts()
		{
			return new HomeProductDao().GetHomeProducts();
		}

		public static System.Data.DataTable GetVote(int voteId, out string voteName, out int checkNum, out int voteNum)
		{
			return new VoteDao().LoadVote(voteId, out voteName, out checkNum, out voteNum);
		}

		public static bool Vote(int voteId, string itemIds)
		{
			return new VoteDao().Vote(voteId, itemIds);
		}

		public static bool IsVote(int voteId)
		{
			return new VoteDao().IsVote(voteId);
		}

		public static Hidistro.Entities.VShop.ActivityInfo GetActivity(int activityId)
		{
			return new Hidistro.SqlDal.VShop.ActivityDao().GetActivity(activityId);
		}

		public static IList<BannerInfo> GetAllBanners()
		{
			return new BannerDao().GetAllBanners();
		}

		public static IList<NavigateInfo> GetAllNavigate()
		{
			return new BannerDao().GetAllNavigate();
		}

		public static string GetLimitedTimeDiscountName(int limitedTimeDiscountId)
		{
			string result = string.Empty;
			LimitedTimeDiscountInfo discountInfo = new LimitedTimeDiscountDao().GetDiscountInfo(limitedTimeDiscountId);
			if (discountInfo != null)
			{
				result = discountInfo.ActivityName;
			}
			return result;
		}

		public static string GetLimitedTimeDiscountNameStr(int limitedTimeDiscountId)
		{
			string text = VshopBrowser.GetLimitedTimeDiscountName(limitedTimeDiscountId);
			if (!string.IsNullOrEmpty(text))
			{
				text = "<span style='background-color: rgb(246, 187, 66); border-color: rgb(246, 187, 66); color: rgb(255, 255, 255);'>" + HttpContext.Current.Server.HtmlEncode(text) + "</span>";
			}
			return text;
		}
	}
}
