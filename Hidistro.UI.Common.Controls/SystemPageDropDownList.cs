using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SystemPageDropDownList : DropDownList
	{
		public override void DataBind()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("--请选择--", ""));
			this.Items.Add(new ListItem("下架区", "productunslaes"));
			this.Items.Add(new ListItem("店铺交流区", "LeaveComments"));
			this.Items.Add(new ListItem("帮助中心", "AllHelps"));
			this.Items.Add(new ListItem("公告列表", "Affiches"));
			this.Items.Add(new ListItem("积分商城", "OnlineGifts"));
			this.Items.Add(new ListItem("商城资讯", "AllArticles"));
			this.Items.Add(new ListItem("限时抢购", "CountDownProducts"));
			this.Items.Add(new ListItem("团购", "GroupBuyProducts"));
			this.Items.Add(new ListItem("捆绑销售", "BundlingProducts"));
			this.Items.Add(new ListItem("品牌专卖", "brand"));
			this.Items.Add(new ListItem("优惠活动", "Promotes"));
		}
	}
}
