using Hidistro.Entities.VShop;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_PrizeNames : WebControl
	{
		public LotteryActivityInfo Activity
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.Activity != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (PrizeSetting current in this.Activity.PrizeSettingList)
				{
					stringBuilder.AppendFormat("{0}：{1} ({2}名)<br/>", current.PrizeLevel, current.PrizeName, current.PrizeNum);
				}
				writer.Write(stringBuilder.ToString());
			}
		}
	}
}
