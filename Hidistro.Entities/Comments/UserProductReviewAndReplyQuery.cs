using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Comments
{
	public class UserProductReviewAndReplyQuery : Pagination
	{
		public int ProductId
		{
			get;
			set;
		}
	}
}
