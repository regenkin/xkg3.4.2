using Hidistro.SaleSystem.Vshop;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_CouponSelect : WebControl
	{
		public decimal CartTotal
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			DataTable coupon = ShoppingProcessor.GetCoupon(this.CartTotal);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择一张优惠券<span class=\"caret\"></span></button>");
			stringBuilder.AppendLine("<ul class=\"dropdown-menu\" role=\"menu\">");
			if (coupon != null)
			{
				foreach (DataRow dataRow in coupon.Rows)
				{
					stringBuilder.AppendFormat("<li><a href=\"#\" name=\"{0}\" value=\"{3}\">{1}(满{2}减{3})</a></li>", new object[]
					{
						dataRow["ClaimCode"],
						dataRow["Name"],
						((decimal)dataRow["Amount"]).ToString("F2"),
						((decimal)dataRow["DiscountValue"]).ToString("F2")
					}).AppendLine();
				}
			}
			stringBuilder.AppendLine("</ul>");
			writer.Write(stringBuilder.ToString());
		}
	}
}
