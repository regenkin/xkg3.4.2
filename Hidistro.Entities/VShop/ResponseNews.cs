using System;
using System.Collections.Generic;

namespace Hidistro.Entities.VShop
{
	public class ResponseNews : AbstractResponseMessage
	{
		public int ArticleCount
		{
			get
			{
				return (this.MessageInfo == null) ? 0 : this.MessageInfo.Count;
			}
		}

		public System.Collections.Generic.IList<MessageInfo> MessageInfo
		{
			get;
			set;
		}

		public ResponseNews()
		{
			base.MsgType = "news";
		}
	}
}
