using System;
using System.Web;

namespace Hidistro.UI.Common.Controls
{
	public class HtmlDecodeTextBox : TrimTextBox
	{
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = ((!string.IsNullOrEmpty(value)) ? HttpUtility.HtmlDecode(value) : value);
			}
		}
	}
}
