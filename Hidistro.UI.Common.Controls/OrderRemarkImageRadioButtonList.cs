using Hidistro.Entities.Orders;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderRemarkImageRadioButtonList : RadioButtonList
	{
		public new OrderMark? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return new OrderMark?((OrderMark)Enum.Parse(typeof(OrderMark), base.SelectedValue));
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(((int)value.Value).ToString(CultureInfo.InvariantCulture)));
					return;
				}
				base.SelectedIndex = -1;
			}
		}

		public OrderRemarkImageRadioButtonList()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("<span class=\"glyphicon glyphicon-ok help\" style=\"color:#309930\"></span>", 1.ToString()));
			this.Items.Add(new ListItem("<span class=\"glyphicon glyphicon-exclamation-sign help\" style=\"color:#CB1E02\"></span>", 2.ToString()));
			this.Items.Add(new ListItem("<span class=\"glyphicon glyphicon-flag help\" style=\"color:#CB1E02\"></span>", 3.ToString()));
			this.Items.Add(new ListItem("<span class=\"glyphicon glyphicon-flag help\" style=\"color:#4E994E\"></span>", 4.ToString()));
			this.Items.Add(new ListItem("<span class=\"glyphicon glyphicon-flag help\" style=\"color:#FFC500\"></span>", 5.ToString()));
			this.Items.Add(new ListItem("<span class=\"glyphicon glyphicon-flag help\" style=\"color:#ABABAB\"></span>", 6.ToString()));
			this.RepeatDirection = RepeatDirection.Horizontal;
		}
	}
}
