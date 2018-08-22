using Hidistro.Entities.Sales;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SaleStatisticsTypeRadioButtonList : RadioButtonList
	{
		public new SaleStatisticsType SelectedValue
		{
			get
			{
				return (SaleStatisticsType)Enum.Parse(typeof(SaleStatisticsType), base.SelectedValue);
			}
			set
			{
				ListItemCollection arg_1A_0 = base.Items;
				ListItemCollection arg_15_0 = base.Items;
				int num = (int)value;
				int selectedIndex = arg_1A_0.IndexOf(arg_15_0.FindByValue(num.ToString()));
				base.SelectedIndex = selectedIndex;
			}
		}

		public SaleStatisticsTypeRadioButtonList()
		{
			this.Items.Add(new ListItem("交易量", 1.ToString(CultureInfo.InvariantCulture)));
			this.Items.Add(new ListItem("交易额", 2.ToString(CultureInfo.InvariantCulture)));
			this.Items.Add(new ListItem("利润", 3.ToString(CultureInfo.InvariantCulture)));
			this.RepeatDirection = RepeatDirection.Horizontal;
			this.SelectedIndex = 0;
		}
	}
}
