using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class YesNoRadioButtonList : RadioButtonList
	{
		public string YesText
		{
			get;
			set;
		}

		public string NoText
		{
			get;
			set;
		}

		public new bool SelectedValue
		{
			get
			{
				return bool.Parse(base.SelectedValue);
			}
			set
			{
				base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value ? "True" : "False"));
			}
		}

		public YesNoRadioButtonList()
		{
			this.NoText = "否";
			this.YesText = "是";
			this.Items.Clear();
			this.Items.Add(new ListItem(this.YesText, "True"));
			this.Items.Add(new ListItem(this.NoText, "False"));
			this.RepeatDirection = RepeatDirection.Horizontal;
			this.SelectedValue = true;
		}
	}
}
