using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class TrimTextBox : TextBox
	{
		public override string Text
		{
			get
			{
				return base.Text.Trim();
			}
			set
			{
				base.Text = value;
			}
		}
	}
}
