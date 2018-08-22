using System;

namespace Hidistro.Entities.Weibo
{
	public class ArticleItemsInfo
	{
		public int Id
		{
			get;
			set;
		}

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

		public System.DateTime PubTime
		{
			get;
			set;
		}

		public LinkType LinkType
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
