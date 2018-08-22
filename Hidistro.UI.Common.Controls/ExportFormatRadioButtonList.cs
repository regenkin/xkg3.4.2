using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ExportFormatRadioButtonList : RadioButtonList
	{
		private RepeatDirection repeatDirection;

		public override RepeatDirection RepeatDirection
		{
			get
			{
				return this.repeatDirection;
			}
			set
			{
				this.repeatDirection = value;
			}
		}

		public ExportFormatRadioButtonList()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("CSV格式", "csv"));
			this.Items.Add(new ListItem("TXT格式", "txt"));
			base.SelectedIndex = 0;
		}
	}
}
