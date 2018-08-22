using ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_PointExChange : System.Web.IHttpHandler
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
			int totalRecords = 0;
			System.Data.DataTable exChangeTable = this.GetExChangeTable(context, ref totalRecords);
			int pageCount = TemplatePageControl.GetPageCount(totalRecords, 10);
			if (exChangeTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetGamesListJson(exChangeTable, context) + ",";
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

		public string GetGamesListJson(System.Data.DataTable dt, System.Web.HttpContext context)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("\"list\":[");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"game_id\":\"" + dt.Rows[i]["Id"].ToString() + "\",");
				stringBuilder.Append("\"title\":\"" + dt.Rows[i]["Name"].ToString() + "\",");
				stringBuilder.Append("\"create_time\":\"" + System.DateTime.Now + "\",");
				stringBuilder.Append("\"type\":\"0\",");
				stringBuilder.Append("\"link\":\"/ExchangeList.aspx?id=" + dt.Rows[i]["Id"].ToString() + "\"");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
			return str + "]";
		}

		public System.Data.DataTable GetExChangeTable(System.Web.HttpContext context, ref int pageCount)
		{
			return PointExChangeHelper.Query(this.GetExChangeSearch(context), ref pageCount);
		}

		public ExChangeSearch GetExChangeSearch(System.Web.HttpContext context)
		{
			return new ExChangeSearch
			{
				PageIndex = (context.Request.Form["p"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["p"]),
				SortOrder = SortAction.Desc,
				status = ExchangeStatus.In,
				SortBy = "Id"
			};
		}
	}
}
