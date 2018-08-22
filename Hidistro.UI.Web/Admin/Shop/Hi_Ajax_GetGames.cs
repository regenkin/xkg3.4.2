using ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GetGames : System.Web.IHttpHandler
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
			string arg_25_0 = context.Request.Form["id"];
			context.Response.Write(this.GetModelJson(context));
		}

		public string GetModelJson(System.Web.HttpContext context)
		{
			DbQueryResult gamesTable = this.GetGamesTable(context);
			int pageCount = TemplatePageControl.GetPageCount(gamesTable.TotalRecords, 10);
			if (gamesTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetGamesListJson(gamesTable, context) + ",";
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

		public string GetGamesListJson(DbQueryResult CouponsTable, System.Web.HttpContext context)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("\"list\":[");
			System.Data.DataTable dataTable = (System.Data.DataTable)CouponsTable.Data;
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"game_id\":\"" + dataTable.Rows[i]["GameId"].ToString() + "\",");
				stringBuilder.Append("\"title\":\"" + dataTable.Rows[i]["GameTitle"].ToString() + "\",");
				stringBuilder.Append("\"create_time\":\"" + System.DateTime.Now + "\",");
				stringBuilder.Append("\"type\":\"" + dataTable.Rows[i]["GameType"].ToString() + "\",");
				stringBuilder.Append("\"link\":\"" + dataTable.Rows[i]["GameUrl"].ToString() + "\"");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
			return str + "]";
		}

		public DbQueryResult GetGamesTable(System.Web.HttpContext context)
		{
			return GameHelper.GetGameList(this.GetGameSearch(context));
		}

		public GameSearch GetGameSearch(System.Web.HttpContext context)
		{
			return new GameSearch
			{
				Status = "0",
				GameType = new int?((context.Request.Form["type"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["type"])),
				PageIndex = (context.Request.Form["p"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["p"]),
				SortOrder = SortAction.Desc,
				EndTime = new System.DateTime?(System.DateTime.Now),
				SortBy = "GameId"
			};
		}
	}
}
