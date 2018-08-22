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
	public class Hi_Ajax_Brands : System.Web.IHttpHandler
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
			DbQueryResult brandsTable = this.GetBrandsTable(context);
			int pageCount = TemplatePageControl.GetPageCount(brandsTable.TotalRecords, 10);
			if (brandsTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetGraphicesListJson(brandsTable, context) + ",";
				str = str + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"";
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
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"item_id\":\"" + dataTable.Rows[i]["BrandId"] + "\",");
				stringBuilder.Append("\"title\":\"" + dataTable.Rows[i]["BrandName"] + "\",");
				stringBuilder.Append("\"create_time\":\"" + System.DateTime.Now + "\",");
				stringBuilder.Append("\"link\":\"/BrandDetail.aspx?BrandId=" + dataTable.Rows[i]["BrandId"] + "\",");
				stringBuilder.Append("\"pic\":\"" + dataTable.Rows[i]["Logo"] + "\"");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
			return str + "]";
		}

		public DbQueryResult GetBrandsTable(System.Web.HttpContext context)
		{
			return CatalogHelper.GetBrandQuery(this.GetBrandSearch(context));
		}

		public BrandQuery GetBrandSearch(System.Web.HttpContext context)
		{
			return new BrandQuery
			{
				Name = (context.Request.Form["title"] == null) ? "" : context.Request.Form["title"],
				PageIndex = (context.Request.Form["p"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["p"]),
				SortOrder = SortAction.Desc,
				SortBy = "BrandId"
			};
		}
	}
}
