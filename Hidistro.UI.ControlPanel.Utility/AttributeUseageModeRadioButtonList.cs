using Hidistro.Entities.Commodities;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public sealed class AttributeUseageModeRadioButtonList : RadioButtonList
	{
		public new AttributeUseageMode SelectedValue
		{
			get
			{
				return (AttributeUseageMode)int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
			}
			set
			{
				int num = (int)value;
				base.SelectedValue = num.ToString(CultureInfo.InvariantCulture);
			}
		}

		public AttributeUseageModeRadioButtonList()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("供客户查看", 1.ToString(CultureInfo.InvariantCulture)));
			this.Items.Add(new ListItem("客户可选规格", 2.ToString(CultureInfo.InvariantCulture)));
			this.Items[0].Selected = true;
		}
	}
}
