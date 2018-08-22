using Hidistro.Core;
using System;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	public class WangWangConversations : Control
	{
		public string WangWangAccounts
		{
			get
			{
				if (this.ViewState["wangWangAccounts"] == null)
				{
					return null;
				}
				return (string)this.ViewState["wangWangAccounts"];
			}
			set
			{
				this.ViewState["wangWangAccounts"] = Globals.UrlEncode(value);
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.WangWangAccounts))
			{
				writer.WriteLine(string.Format("<a target=\"_blank\" href=\"http://amos1.taobao.com/msg.ww?v=2&uid={0}&s=1\" ><img border=\"0\" src=\"http://amos1.taobao.com/online.ww?v=2&uid={0}&s=1\" alt=\"点击这里给我发消息\" /></a>", this.WangWangAccounts));
			}
		}
	}
}
