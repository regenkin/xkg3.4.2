using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SqlDal.Comments;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Orders;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.SaleSystem.Vshop
{
	public static class ProductBrowser
	{
		public static System.Data.DataTable GetProducts(MemberInfo member, int? topicId, int? categoryId, string keyWord, int pageNumber, int maxNum, out int total, string sort, string order, string pIds = "", bool isLimitedTimeDiscountId = false)
		{
			return new ProductBrowseDao().GetProducts(member, topicId, categoryId, Globals.GetCurrentDistributorId(), keyWord, pageNumber, maxNum, out total, sort, order == "asc", pIds, isLimitedTimeDiscountId);
		}

		public static System.Data.DataTable GetProducts(MemberInfo member, int? topicId, int? categoryId, string keyWord, int pageNumber, int maxNum, out int total, string sort, string order, bool isselect)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			int num = 0;
			int currentMemberUserId = Globals.GetCurrentMemberUserId();
			if (currentMemberUserId > 0)
			{
				result = new ProductBrowseDao().GetProducts(member, topicId, categoryId, currentMemberUserId, keyWord, pageNumber, maxNum, out num, sort, order == "asc", isselect);
			}
			total = num;
			return result;
		}

		public static int GetProductsNumber(bool isselect = true)
		{
			int currentDistributorId = Globals.GetCurrentDistributorId();
			int result = 0;
			if (currentDistributorId > 0)
			{
				result = new ProductBrowseDao().GetProductsNumber(currentDistributorId, isselect);
			}
			return result;
		}

		public static ProductInfo GetProduct(MemberInfo member, int productId)
		{
			return new ProductBrowseDao().GetProduct(member, productId);
		}

		public static System.Data.DataTable GetAllFull(int ActivitiesType)
		{
			return new ProductBrowseDao().GetAllFull(ActivitiesType);
		}

		public static System.Data.DataTable GetActivitie(int ActivitiesId)
		{
			return new ProductBrowseDao().GetActivitie(ActivitiesId);
		}

		public static System.Data.DataTable GetActiviOne(int ActivitiesType, decimal MeetMoney)
		{
			return new ProductBrowseDao().GetActiviOne(ActivitiesType, MeetMoney);
		}

		public static System.Data.DataTable GetAllFull()
		{
			return new HomeProductDao().GetAllFull();
		}

		public new static System.Data.DataTable GetType()
		{
			return new ProductBrowseDao().GetType();
		}

		public static DbQueryResult GetHomeProduct(MemberInfo member, ProductQuery query)
		{
			DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(true);
			DbQueryResult homeProducts;
			if (currentDistributors != null && currentDistributors.UserId != 0)
			{
				homeProducts = new HomeProductDao().GetHomeProducts(member, query, true);
			}
			else
			{
				homeProducts = new HomeProductDao().GetHomeProducts(member, query, false);
			}
			return homeProducts;
		}

		public static System.Data.DataTable GetBrandProducts(MemberInfo member, int? brandId, int pageNumber, int maxNum, out int total)
		{
			return new ProductBrowseDao().GetBrandProducts(member, brandId, pageNumber, maxNum, out total);
		}

		public static System.Data.DataTable GetSkus(int productId)
		{
			return new SkuDao().GetSkus(productId);
		}

		public static System.Data.DataTable GetExpandAttributes(int productId)
		{
			return new SkuDao().GetExpandAttributes(productId);
		}

		public static DbQueryResult GetProductReviews(ProductReviewQuery reviewQuery)
		{
			return new ProductReviewDao().GetProductReviews(reviewQuery);
		}

		public static bool InsertProductReview(ProductReviewInfo review)
		{
			return new ProductReviewDao().InsertProductReview(review);
		}

		public static int GetProductReviewsCount(int productId)
		{
			return new ProductReviewDao().GetProductReviewsCount(productId);
		}

		public static LineItemInfo GetReturnMoneyByOrderIDAndProductID(string orderId, string skuid, int itemid)
		{
			return new LineItemDao().GetReturnMoneyByOrderIDAndProductID(orderId, skuid, itemid);
		}

		public static LineItemInfo GetLatestOrderItemByProductIDAndUserID(int productid, int userid)
		{
			return new ProductReviewDao().GetLatestOrderItemByProductIDAndUserID(productid, userid);
		}

		public static int GetWaitCommentByUserID(int userid)
		{
			return new ProductReviewDao().GetWaitCommentByUserID(userid);
		}

		public static IList<LineItemInfo> GetOrderItemsList(int userid)
		{
			return new ProductReviewDao().GetOrderItemsList(userid);
		}

		public static DbQueryResult GetOrderMemberComment(OrderQuery query, int userId, int orderItemsStatus)
		{
			return new ProductReviewDao().GetOrderMemberComment(query, userId, orderItemsStatus);
		}

		public static bool IsReview(string orderid, string skuid, int productid, int userid)
		{
			return new ProductReviewDao().IsReview(orderid, skuid, productid, userid);
		}

		public static void LoadProductReview(int productId, int userId, out int buyNum, out int reviewNum)
		{
			new ProductReviewDao().LoadProductReview(productId, userId, out buyNum, out reviewNum);
		}

		public static DbQueryResult GetProductConsultations(ProductConsultationAndReplyQuery consultationQuery)
		{
			return new ProductConsultationDao().GetConsultationProducts(consultationQuery);
		}

		public static int GetProductConsultationsCount(int productId, bool includeUnReplied)
		{
			return new ProductConsultationDao().GetProductConsultationsCount(productId, includeUnReplied);
		}

		public static bool InsertProductConsultation(ProductConsultationInfo productConsultation)
		{
			return new ProductConsultationDao().InsertProductConsultation(productConsultation);
		}

		public static bool AddProductToFavorite(int productId, int userId)
		{
			FavoriteDao favoriteDao = new FavoriteDao();
			return favoriteDao.ExistsProduct(productId, userId) || favoriteDao.AddProductToFavorite(productId, userId);
		}

		public static bool ExistsProduct(int productId, int userId)
		{
			return new FavoriteDao().ExistsProduct(productId, userId);
		}

		public static int UpdateFavorite(int favoriteId, string tags, string remark)
		{
			return new FavoriteDao().UpdateFavorite(favoriteId, tags, remark);
		}

		public static int DeleteFavorite(int favoriteId)
		{
			return new FavoriteDao().DeleteFavorite(favoriteId);
		}

		public static System.Data.DataTable GetFavorites(MemberInfo member)
		{
			return new FavoriteDao().GetFavorites(member);
		}

		public static bool CheckHasCollect(int memberId, int productId)
		{
			return new FavoriteDao().CheckHasCollect(memberId, productId);
		}

		public static bool DeleteFavorites(string ids)
		{
			return new FavoriteDao().DeleteFavorites(ids);
		}

		public static bool UpdateVisitCounts(int productId)
		{
			return new ProductBatchDao().UpdateVisitCounts(productId);
		}

		public static System.Data.DataTable GetProductCategories(int prouctId)
		{
			return new ProductDao().GetProductCategories(prouctId);
		}

		public static string GetProductTagName(int productId)
		{
			return new TagDao().GetProductTagName(productId);
		}
	}
}
