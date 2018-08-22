using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.AddProducts)]
	public class SelectCategory : AdminPage
	{
		protected int categoryid = Globals.RequestQueryNum("categoryId");

		protected int productId = Globals.RequestQueryNum("productId");

		protected string reurl = Globals.RequestQueryStr("reurl");

		protected SelectCategory() : base("m02", "spp01")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			bool flag = !string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true";
			if (flag)
			{
				this.DoCallback();
			}
		}

		private void DoCallback()
		{
			string text = base.Request.QueryString["action"];
			base.Response.Clear();
			base.Response.ContentType = "application/json";
			if (text.Equals("getlist"))
			{
				int num = 0;
				int.TryParse(base.Request.QueryString["parentCategoryId"], out num);
				System.Collections.Generic.IList<CategoryInfo> list = (num == 0) ? CatalogHelper.GetMainCategories() : CatalogHelper.GetSubCategories(num);
				if (list == null || list.Count == 0)
				{
					base.Response.Write("{\"Status\":\"0\"}");
				}
				else
				{
					base.Response.Write(this.GenerateJson(list));
				}
			}
			else if (text.Equals("getinfo"))
			{
				int num2 = 0;
				int.TryParse(base.Request.QueryString["categoryId"], out num2);
				if (num2 <= 0)
				{
					base.Response.Write("{\"Status\":\"0\"}");
				}
				else
				{
					CategoryInfo category = CatalogHelper.GetCategory(num2);
					if (category == null)
					{
						base.Response.Write("{\"Status\":\"0\"}");
					}
					else
					{
						base.Response.Write(string.Concat(new string[]
						{
							"{\"Status\":\"OK\", \"Name\":\"",
							category.Name,
							"\", \"Path\":\"",
							category.Path,
							"\"}"
						}));
					}
				}
			}
			base.Response.End();
		}

		private string GenerateJson(System.Collections.Generic.IList<CategoryInfo> categories)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"Status\":\"OK\",");
			stringBuilder.Append("\"Categories\":[");
			foreach (CategoryInfo current in categories)
			{
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"CategoryId\":\"{0}\",", current.CategoryId.ToString(System.Globalization.CultureInfo.InvariantCulture));
				stringBuilder.AppendFormat("\"HasChildren\":\"{0}\",", current.HasChildren ? "true" : "false");
				stringBuilder.AppendFormat("\"CategoryName\":\"{0}\"", current.Name);
				stringBuilder.Append("},");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("]}");
			return stringBuilder.ToString();
		}
	}
}
