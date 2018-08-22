using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_Categories : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Write(this.GetModelJson(context));
		}

		public string GetModelJson(System.Web.HttpContext context)
		{
			DbQueryResult categoriesTable = this.GetCategoriesTable(context);
			TemplatePageControl.GetPageCount(categoriesTable.TotalRecords, 10000);
			if (categoriesTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetGraphicesListJson(categoriesTable, context) + ",";
				str += "\"page\":\"\"";
				return str + "}";
			}
			return "{\"status\":1,\"list\":[],\"page\":\"\"}";
		}

		public string GetPageHtml(int pageCount, System.Web.HttpContext context)
		{
			int pageIndex = (context.Request.Form["p"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["p"]);
			return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
		}

		public string GetGraphicesListJson(DbQueryResult GraphicesTable, System.Web.HttpContext context)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("\"list\":[");
			System.Data.DataTable dataTable = (System.Data.DataTable)GraphicesTable.Data;
			dataTable.DefaultView.RowFilter = "ParentCategoryId=0";
			System.Data.DataTable dataTable2 = dataTable.DefaultView.ToTable();
			for (int i = 0; i < dataTable2.Rows.Count; i++)
			{
				string text = dataTable2.Rows[i]["CategoryId"].ToString();
				stringBuilder.Append("{");
				stringBuilder.Append("\"item_id\":\"" + Globals.String2Json(text) + "\",");
				stringBuilder.Append("\"title\":\"" + Globals.String2Json(dataTable2.Rows[i]["Name"].ToString()) + "\",");
				stringBuilder.Append("\"create_time\":\"" + System.DateTime.Now + "\",");
				stringBuilder.Append("\"link\":\"/ProductList.aspx?categoryId=" + dataTable2.Rows[i]["CategoryId"] + "\",");
				stringBuilder.Append("\"pic\":\"\",");
				stringBuilder.Append("\"children\":[" + this.GetChildGraphicesListJson(dataTable, text) + "]");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
			return str + "]";
		}

		private string GetChildGraphicesListJson(System.Data.DataTable dt, string categoryId)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string result = "";
			dt.DefaultView.RowFilter = "ParentCategoryId=" + categoryId;
			System.Data.DataTable dataTable = dt.DefaultView.ToTable();
			if (dataTable.Rows.Count > 0)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					stringBuilder.Append("{");
					stringBuilder.Append("\"item_id\":\"" + dataTable.Rows[i]["CategoryId"] + "\",");
					stringBuilder.Append("\"title\":\"" + Globals.String2Json(dataTable.Rows[i]["Name"].ToString()) + "\",");
					stringBuilder.Append("\"create_time\":\"" + System.DateTime.Now + "\",");
					stringBuilder.Append("\"link\":\"/ProductList.aspx?categoryId=" + dataTable.Rows[i]["CategoryId"] + "\",");
					stringBuilder.Append("\"pic\":\"\"");
					stringBuilder.Append("},");
				}
				result = stringBuilder.ToString().TrimEnd(new char[]
				{
					','
				});
			}
			return result;
		}

		public DbQueryResult GetCategoriesTable(System.Web.HttpContext context)
		{
			return CatalogHelper.Query(this.GetCategoriesSearch(context));
		}

		public CategoriesQuery GetCategoriesSearch(System.Web.HttpContext context)
		{
			return new CategoriesQuery
			{
				Name = (context.Request.Form["title"] == null) ? "" : context.Request.Form["title"],
				PageIndex = (context.Request.Form["p"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["p"]),
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence"
			};
		}
	}
}
