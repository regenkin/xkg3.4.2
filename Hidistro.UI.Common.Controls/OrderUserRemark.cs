using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderUserRemark : Label
	{
		private string dataField = "remark";

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

		protected override void OnDataBinding(EventArgs e)
		{
			object obj = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
			if (obj != null && obj != DBNull.Value && !string.IsNullOrEmpty(obj.ToString()))
			{
				base.Text = "<img src=\"../images/xi.gif\" />";
				base.ToolTip = obj.ToString();
			}
			else
			{
				base.Text = "-";
			}
			base.OnDataBinding(e);
		}
	}
}
