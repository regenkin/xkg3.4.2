using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class LiteralWrapper : IText
	{
		private Literal _literal;

		public bool Visible
		{
			get
			{
				return this._literal.Visible;
			}
			set
			{
				this._literal.Visible = value;
			}
		}

		public string Text
		{
			get
			{
				return this._literal.Text;
			}
			set
			{
				this._literal.Text = value;
			}
		}

		public Control Control
		{
			get
			{
				return this._literal;
			}
		}

		internal LiteralWrapper(Literal literal)
		{
			this._literal = literal;
		}
	}
}
