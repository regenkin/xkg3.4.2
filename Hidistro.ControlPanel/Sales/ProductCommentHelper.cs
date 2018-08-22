using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.SqlDal.Comments;
using System;
using System.Collections.Generic;

namespace Hidistro.ControlPanel.Sales
{
	public sealed class ProductCommentHelper
	{
		private ProductCommentHelper()
		{
		}

		public static DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
		{
			return new ProductConsultationDao().GetConsultationProducts(consultationQuery);
		}

		public static ProductConsultationInfo GetProductConsultation(int consultationId)
		{
			return new ProductConsultationDao().GetProductConsultation(consultationId);
		}

		public static bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
		{
			return new ProductConsultationDao().ReplyProductConsultation(productConsultation);
		}

		public static int DeleteProductConsultation(int consultationId)
		{
			return new ProductConsultationDao().DeleteProductConsultation(consultationId);
		}

		public static int DeleteReview(IList<long> reviews)
		{
			int result;
			if (reviews == null || reviews.Count == 0)
			{
				result = 0;
			}
			else
			{
				int num = 0;
				foreach (long current in reviews)
				{
					new ProductReviewDao().DeleteProductReview(current);
					num++;
				}
				result = num;
			}
			return result;
		}

		public static DbQueryResult GetProductReviews(ProductReviewQuery reviewQuery)
		{
			return new ProductReviewDao().GetProductReviews(reviewQuery);
		}

		public static ProductReviewInfo GetProductReview(int reviewId)
		{
			return new ProductReviewDao().GetProductReview(reviewId);
		}

		public static int DeleteProductReview(long reviewId)
		{
			return new ProductReviewDao().DeleteProductReview(reviewId);
		}
	}
}
