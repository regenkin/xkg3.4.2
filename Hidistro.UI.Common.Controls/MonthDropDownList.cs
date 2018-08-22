using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class MonthDropDownList : DropDownList
	{
		public new int SelectedValue
		{
			get
			{
				int result;
				if (int.TryParse(base.SelectedValue, out result))
				{
					return result;
				}
				return DateTime.Now.Month;
			}
			set
			{
				base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString(CultureInfo.InvariantCulture)));
			}
		}

		public MonthDropDownList()
		{
			for (int i = 1; i <= 12; i++)
			{
				this.Items.Add(new ListItem(i.ToString(), i.ToString()));
			}
			this.SelectedValue = DateTime.Now.Month;
		}
	}
}
