using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class YearDropDownList : DropDownList
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
				return DateTime.Now.Year;
			}
			set
			{
				base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString(CultureInfo.InvariantCulture)));
			}
		}

		public YearDropDownList()
		{
			int year = DateTime.Now.Year;
			int num = year - 10;
			for (int i = num; i <= year; i++)
			{
				this.Items.Add(new ListItem(i.ToString(), i.ToString()));
			}
			this.SelectedValue = year;
		}
	}
}
