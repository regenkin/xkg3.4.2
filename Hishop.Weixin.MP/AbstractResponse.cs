using System;

namespace Hishop.Weixin.MP
{
	public class AbstractResponse
	{
		public string ToUserName
		{
			get;
			set;
		}

		public string FromUserName
		{
			get;
			set;
		}

		public DateTime CreateTime
		{
			get;
			set;
		}

		public bool FuncFlag
		{
			get;
			set;
		}

		public virtual ResponseMsgType MsgType
		{
			get;
			set;
		}

		public AbstractResponse()
		{
			this.CreateTime = DateTime.Now;
		}
	}
}
