using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Weibo
{
	public class ArticleInfo
	{
		public int ArticleId
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public ArticleType ArticleType
		{
			get;
			set;
		}

		public LinkType LinkType
		{
			get;
			set;
		}

		public string Content
		{
			get;
			set;
		}

		public string ImageUrl
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public string Memo
		{
			get;
			set;
		}

		public System.DateTime PubTime
		{
			get;
			set;
		}

		public System.Collections.Generic.IList<ArticleItemsInfo> ItemsInfo
		{
			get;
			set;
		}

		public string MediaId
		{
			get;
			set;
		}

		public bool IsShare
		{
			get;
			set;
		}
	}
}
