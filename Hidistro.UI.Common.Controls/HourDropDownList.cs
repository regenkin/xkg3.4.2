using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class HourDropDownList : DropDownList
	{
		public new int? SelectedValue
		{
			get
			{
				int value = 0;
				int.TryParse(base.SelectedValue, out value);
				return new int?(value);
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedValue = value.Value.ToString();
				}
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			for (int i = 0; i <= 23; i++)
			{
				this.Items.Add(new ListItem(i + "时", i.ToString()));
			}
		}
	}
}
