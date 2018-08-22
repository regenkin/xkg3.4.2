using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class GiftStatusRadioButtonList : RadioButtonList
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

		public GiftStatusRadioButtonList()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("全部", "True"));
			this.Items.Add(new ListItem("有效的礼品", "False"));
			this.RepeatDirection = RepeatDirection.Horizontal;
			this.SelectedValue = true;
		}
	}
}
