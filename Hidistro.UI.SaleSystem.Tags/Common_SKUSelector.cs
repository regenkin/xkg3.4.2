using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SKUSelector : WebControl
	{
		public int ProductId
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			DataTable skus = ProductBrowser.GetSkus(this.ProductId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<input type=\"hidden\" id=\"hiddenSkuId\" value=\"{0}_0\"  />", this.ProductId).AppendLine();
			if (skus != null && skus.Rows.Count > 0)
			{
				IList<string> list = new List<string>();
				stringBuilder.AppendFormat("<input type=\"hidden\" id=\"hiddenProductId\" value=\"{0}\" />", this.ProductId).AppendLine();
				stringBuilder.AppendLine("<div class=\"specification\">");
				foreach (DataRow dataRow in skus.Rows)
				{
					if (!list.Contains((string)dataRow["AttributeName"]))
					{
						list.Add((string)dataRow["AttributeName"]);
						stringBuilder.AppendFormat("<div class=\"title text-muted\">{0}：</div><input type=\"hidden\" name=\"skuCountname\" AttributeName=\"{0}\" id=\"skuContent_{1}\" />", dataRow["AttributeName"], dataRow["AttributeId"]);
						stringBuilder.AppendFormat("<div class=\"list clearfix\" id=\"skuRow_{0}\">", dataRow["AttributeId"]);
						IList<string> list2 = new List<string>();
						foreach (DataRow dataRow2 in skus.Rows)
						{
							if (string.Compare((string)dataRow["AttributeName"], (string)dataRow2["AttributeName"]) == 0 && !list2.Contains((string)dataRow2["ValueStr"]))
							{
								string text = string.Concat(new object[]
								{
									"skuValueId_",
									dataRow["AttributeId"],
									"_",
									dataRow2["ValueId"]
								});
								list2.Add((string)dataRow2["ValueStr"]);
								stringBuilder.AppendFormat("<div class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\">{3}</div>", new object[]
								{
									text,
									dataRow["AttributeId"],
									dataRow2["ValueId"],
									(dataRow2["ImageUrl"].ToString() != "") ? ("<img src='" + dataRow2["ImageUrl"] + "' width='50px' height='35px'></img>") : dataRow2["ValueStr"]
								});
							}
						}
						stringBuilder.AppendLine("</div>");
					}
				}
				stringBuilder.AppendLine("</div>");
			}
			writer.Write(stringBuilder.ToString());
		}
	}
}
