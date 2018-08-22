using ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Weibo;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_Graphics : System.Web.IHttpHandler
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
			DbQueryResult graphicesTable = this.GetGraphicesTable(context);
			int pageCount = TemplatePageControl.GetPageCount(graphicesTable.TotalRecords, 10);
			if (graphicesTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetGraphicesListJson(graphicesTable, context) + ",";
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
				stringBuilder.Append("\"item_id\":\"" + Globals.String2Json(dataTable.Rows[i]["ArticleId"].ToString()) + "\",");
				stringBuilder.Append("\"title\":\"" + Globals.String2Json(dataTable.Rows[i]["Title"].ToString()) + "\",");
				stringBuilder.Append("\"create_time\":\"" + System.Convert.ToDateTime(dataTable.Rows[i]["PubTime"]).ToString("yyyy-MM-dd HH:mm:ss") + "\",");
				if (dataTable.Rows[i]["ArticleType"].ToString() == "4")
				{
					stringBuilder.Append("\"link\":\"/vshop/ArticleShow.aspx?id=" + dataTable.Rows[i]["ArticleId"].ToString() + "\",");
				}
				else
				{
					stringBuilder.Append("\"link\":\"" + Globals.String2Json(dataTable.Rows[i]["Url"].ToString()) + "\",");
				}
				stringBuilder.Append("\"pic\":\"" + dataTable.Rows[i]["ImageUrl"] + "\"");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
			return str + "]";
		}

		public DbQueryResult GetGraphicesTable(System.Web.HttpContext context)
		{
			return ArticleHelper.GetArticleRequest(this.GetGraphiceSearch(context));
		}

		public ArticleQuery GetGraphiceSearch(System.Web.HttpContext context)
		{
			return new ArticleQuery
			{
				Title = (context.Request.Form["title"] == null) ? "" : context.Request.Form["title"],
				PageIndex = (context.Request.Form["p"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["p"]),
				SortOrder = SortAction.Desc,
				SortBy = "PubTime"
			};
		}
	}
}
