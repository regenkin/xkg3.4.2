using Hidistro.Entities.VShop;
using System;

namespace Hidistro.Entities.WeiXin
{
	public class SendAllInfo
	{
		public int Id
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public MessageType MessageType
		{
			get;
			set;
		}

		public int ArticleID
		{
			get;
			set;
		}

		public string Content
		{
			get;
			set;
		}

		public int SendState
		{
			get;
			set;
		}

		public System.DateTime SendTime
		{
			get;
			set;
		}

		public int SendCount
		{
			get;
			set;
		}

		public string MsgID
		{
			get;
			set;
		}

		public SendAllInfo()
		{
			this.ArticleID = 0;
			this.SendCount = 0;
			this.SendTime = System.DateTime.Now;
		}
	}
}
