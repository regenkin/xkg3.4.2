using System;

namespace Hidistro.Entities.Weibo
{
	public class ReplyInfo
	{
		public int ArticleId
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public int ReplyKeyId
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public string ReceiverType
		{
			get;
			set;
		}

		public bool IsDisable
		{
			get;
			set;
		}

		public System.DateTime EditDate
		{
			get;
			set;
		}

		public string Content
		{
			get;
			set;
		}

		public string Displayname
		{
			get;
			set;
		}

		public string Summary
		{
			get;
			set;
		}

		public string Image
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}
	}
}
