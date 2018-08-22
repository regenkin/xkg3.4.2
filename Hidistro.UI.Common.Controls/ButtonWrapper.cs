using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ButtonWrapper : IButton, IText
	{
		private Button _button;

		public event EventHandler Click
		{
			add
			{
				this._button.Click += value;
			}
			remove
			{
				this._button.Click -= value;
			}
		}

		public event CommandEventHandler Command
		{
			add
			{
				this._button.Command += value;
			}
			remove
			{
				this._button.Command -= value;
			}
		}

		public bool CausesValidation
		{
			get
			{
				return this._button.CausesValidation;
			}
			set
			{
				this._button.CausesValidation = value;
			}
		}

		public Control Control
		{
			get
			{
				return this._button;
			}
		}

		public bool Visible
		{
			get
			{
				return this._button.Visible;
			}
			set
			{
				this._button.Visible = value;
			}
		}

		public string Text
		{
			get
			{
				return this._button.Text;
			}
			set
			{
				this._button.Text = value;
			}
		}

		public AttributeCollection Attributes
		{
			get
			{
				return this._button.Attributes;
			}
		}

		public string CommandArgument
		{
			get
			{
				return this._button.CommandArgument;
			}
			set
			{
				this._button.CommandArgument = value;
			}
		}

		public string CommandName
		{
			get
			{
				return this._button.CommandName;
			}
			set
			{
				this._button.CommandName = value;
			}
		}

		internal ButtonWrapper(Button button)
		{
			this._button = button;
		}
	}
}
