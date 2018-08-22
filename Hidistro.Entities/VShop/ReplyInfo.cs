using System;

namespace Hidistro.Entities.VShop
{
	public class ReplyInfo
	{
		public int Id
		{
			get;
			set;
		}

		public string Keys
		{
			get;
			set;
		}

		public MatchType MatchType
		{
			get;
			set;
		}

		public ReplyType ReplyType
		{
			get;
			set;
		}

		public MessageType MessageType
		{
			get;
			set;
		}

		public bool IsDisable
		{
			get;
			set;
		}

		public System.DateTime LastEditDate
		{
			get;
			set;
		}

		public string LastEditor
		{
			get;
			set;
		}

		public int ActivityId
		{
			get;
			set;
		}

		public string MessageTypeName
		{
			get
			{
				return this.MessageType.ToShowText();
			}
		}

		public int ArticleID
		{
			get;
			set;
		}

		public ReplyInfo()
		{
			this.LastEditDate = System.DateTime.Now;
			this.MatchType = MatchType.Like;
			this.MessageType = MessageType.Text;
		}
	}
}
