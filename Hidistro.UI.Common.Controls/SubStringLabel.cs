using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SubStringLabel : Literal
	{
		private int _strLength;

		private string _strReplace = "...";

		private string field;

		public int StrLength
		{
			get
			{
				return this._strLength;
			}
			set
			{
				this._strLength = value;
			}
		}

		public string StrReplace
		{
			get
			{
				return this._strReplace;
			}
			set
			{
				this._strReplace = value;
			}
		}

		public string Field
		{
			get
			{
				return this.field;
			}
			set
			{
				this.field = value;
			}
		}

		protected override void OnDataBinding(EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Field))
			{
				object obj = DataBinder.Eval(this.Page.GetDataItem(), this.Field);
				if (obj != null && obj != DBNull.Value)
				{
					base.Text = (string)obj;
				}
			}
			base.OnDataBinding(e);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.StrLength > 0 && this.StrLength < base.Text.Length)
			{
				base.Text = base.Text.Substring(0, this.StrLength) + this.StrReplace;
			}
			base.Render(writer);
		}
	}
}
