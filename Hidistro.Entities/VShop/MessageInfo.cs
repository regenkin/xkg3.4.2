using System;

namespace Hidistro.Entities.VShop
{
	[System.Serializable]
	public class MessageInfo
	{
		public int MsgId
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public string Description
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

		public int UrlType
		{
			get;
			set;
		}
	}
}
