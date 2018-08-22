using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class LabelWrapper : IText
	{
		private Label _label;

		public bool Visible
		{
			get
			{
				return this._label.Visible;
			}
			set
			{
				this._label.Visible = value;
			}
		}

		public string Text
		{
			get
			{
				return this._label.Text;
			}
			set
			{
				this._label.Text = value;
			}
		}

		public Control Control
		{
			get
			{
				return this._label;
			}
		}

		internal LabelWrapper(Label label)
		{
			this._label = label;
		}
	}
}
