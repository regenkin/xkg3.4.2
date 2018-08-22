using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class DecimalLengthDropDownList : DropDownList
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
				return 2;
			}
			set
			{
				base.SelectedValue = value.ToString();
			}
		}

		public DecimalLengthDropDownList()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("2位", "2"));
			this.Items.Add(new ListItem("1位", "1"));
			this.Items.Add(new ListItem("0位", "0"));
		}
	}
}
