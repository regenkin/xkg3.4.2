using ASPNET.WebControls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class HiImage : Image
	{
		private string dataField;

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

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(base.ImageUrl))
			{
				if (!string.IsNullOrEmpty(base.ImageUrl) && !Utils.IsUrlAbsolute(base.ImageUrl.ToLower()) && Utils.ApplicationPath.Length > 0 && !base.ImageUrl.StartsWith(Utils.ApplicationPath))
				{
					base.ImageUrl = Utils.ApplicationPath + base.ImageUrl;
				}
				base.Render(writer);
			}
		}

		protected override void OnDataBinding(EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.DataField))
			{
				object obj = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
				if (obj != null && obj != DBNull.Value)
				{
					base.ImageUrl = (string)obj;
				}
			}
		}
	}
}
