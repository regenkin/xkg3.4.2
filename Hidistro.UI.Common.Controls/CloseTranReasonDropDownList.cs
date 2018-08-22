using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class CloseTranReasonDropDownList : DropDownList
	{
		public CloseTranReasonDropDownList()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("请选择关闭的理由", "请选择关闭的理由"));
			this.Items.Add(new ListItem("联系不到买家", "联系不到买家"));
			this.Items.Add(new ListItem("买家不想买了", "买家不想买了"));
			this.Items.Add(new ListItem("已同城见面交易", "已同城见面交易"));
			this.Items.Add(new ListItem("暂时缺货", "暂时缺货"));
			this.Items.Add(new ListItem("其他原因", "其他原因"));
		}
	}
}
