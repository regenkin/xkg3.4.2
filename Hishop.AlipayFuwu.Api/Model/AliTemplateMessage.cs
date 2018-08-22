using System;
using System.Collections.Generic;

namespace Hishop.AlipayFuwu.Api.Model
{
	public class AliTemplateMessage
	{
		public class MessagePart
		{
			public string Name
			{
				get;
				set;
			}

			public string Value
			{
				get;
				set;
			}

			public string Color
			{
				get;
				set;
			}

			public MessagePart()
			{
				this.Color = "#000099";
			}
		}

		public string Touser
		{
			get;
			set;
		}

		public string TemplateId
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public string Topcolor
		{
			get;
			set;
		}

		public System.Collections.Generic.IEnumerable<AliTemplateMessage.MessagePart> Data
		{
			get;
			set;
		}

		public AliTemplateMessage()
		{
			this.Topcolor = "#00FF00";
		}
	}
}
