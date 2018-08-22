using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GetItems : System.Web.IHttpHandler
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
			string arg_25_0 = context.Request.Form["p"];
			context.Response.Write(this.GetModelJson(context));
		}

		public string GetModelJson(System.Web.HttpContext context)
		{
			DbQueryResult goodsTable = this.GetGoodsTable(context);
			int pageCount = TemplatePageControl.GetPageCount(goodsTable.TotalRecords, 10);
			if (goodsTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetGoodsListJson(goodsTable) + ",";
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

		public string GetGoodsListJson(DbQueryResult GoodsTable)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("\"list\":[");
			System.Data.DataTable dataTable = (System.Data.DataTable)GoodsTable.Data;
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"item_id\":\"" + dataTable.Rows[i]["ProductId"] + "\",");
				stringBuilder.Append("\"title\":\"" + dataTable.Rows[i]["ProductName"].ToString().Replace("\\", "") + "\",");
				stringBuilder.Append("\"price\":\"" + dataTable.Rows[i]["SalePrice"] + "\",");
				stringBuilder.Append("\"original_price\":\"" + dataTable.Rows[i]["MarketPrice"] + "\",");
				stringBuilder.Append("\"create_time\":\"" + System.Convert.ToDateTime(dataTable.Rows[i]["AddedDate"]).ToString("yyyy-MM-dd HH:mm:ss") + "\",");
				stringBuilder.Append("\"link\":\"/ProductDetails.aspx?productId=" + dataTable.Rows[i]["ProductId"] + "\",");
				stringBuilder.Append("\"pic\":\"" + dataTable.Rows[i]["ThumbnailUrl310"] + "\",");
				stringBuilder.Append("\"is_compress\":0");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
			return str + "]";
		}

		public DbQueryResult GetGoodsTable(System.Web.HttpContext context)
		{
			return ProductHelper.GetProducts(this.GetProductQuery(context));
		}

		public ProductQuery GetProductQuery(System.Web.HttpContext context)
		{
			return new ProductQuery
			{
				Keywords = context.Request.Form["title"],
				PageSize = 10,
				PageIndex = (context.Request.Form["p"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["p"]),
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				SaleStatus = (ProductSaleStatus)((context.Request.Form["status"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["status"]))
			};
		}
	}
}
