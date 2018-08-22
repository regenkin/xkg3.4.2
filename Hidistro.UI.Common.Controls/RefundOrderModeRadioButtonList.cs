using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class RefundOrderModeRadioButtonList : RadioButtonList
	{
		public RefundOrderModeRadioButtonList()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("我已经跟买家联系，使用线下操作完成退款。", "1"));
			this.Items.Add(new ListItem("使用预付款功能退款到买家的预付款账户。", "2"));
			this.Items[0].Selected = true;
		}
	}
}
