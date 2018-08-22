using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class UseAttributeImageRadioButtonList : RadioButtonList
	{
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

		public UseAttributeImageRadioButtonList()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("文字", "False"));
			this.RepeatDirection = RepeatDirection.Horizontal;
			this.SelectedValue = false;
		}
	}
}
