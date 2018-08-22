using System;
using System.Collections.Generic;

namespace Hidistro.Entities.VShop
{
	public class NewsReplyInfo : ReplyInfo
	{
		public System.Collections.Generic.IList<NewsMsgInfo> NewsMsg
		{
			get;
			set;
		}

		public NewsReplyInfo()
		{
			base.MessageType = MessageType.List;
		}
	}
}
