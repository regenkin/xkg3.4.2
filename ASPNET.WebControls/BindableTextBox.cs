using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNET.WebControls
{
	public class BindableTextBox : TextBox
	{
		private string dataField;

		private string format;

		public string DataField
		{
			get
			{
				return this.dataField;
			}
			set
			{
				this.dataField = value;
			}
		}

		public string Format
		{
			get
			{
				return this.format;
			}
			set
			{
				this.format = value;
			}
		}

		protected override void OnDataBinding(EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.DataField))
			{
				object obj = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
				if (obj != null && obj != DBNull.Value)
				{
					base.Text = (string.IsNullOrEmpty(this.Format) ? obj.ToString() : string.Format(this.Format, obj));
				}
			}
		}
	}
}
