using Hidistro.SaleSystem.Vshop;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ExpandAttributes : WebControl
	{
		public int ProductId
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			DataTable expandAttributes = ProductBrowser.GetExpandAttributes(this.ProductId);
			StringBuilder stringBuilder = new StringBuilder();
			if (expandAttributes != null && expandAttributes.Rows.Count > 0)
			{
				foreach (DataRow dataRow in expandAttributes.Rows)
				{
					stringBuilder.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", dataRow["AttributeName"], dataRow["ValueStr"]);
				}
			}
			writer.Write(stringBuilder.ToString());
		}
	}
}
