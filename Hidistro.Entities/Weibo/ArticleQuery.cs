using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Weibo
{
	public class ArticleQuery : Pagination
	{
		public int ArticleType
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string Memo
		{
			get;
			set;
		}

		public int IsShare
		{
			get;
			set;
		}

		public ArticleQuery()
		{
			this.IsShare = -1;
		}
	}
}
