using ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_Votes : System.Web.IHttpHandler
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
			DbQueryResult votesTable = this.GetVotesTable(context);
			int pageCount = TemplatePageControl.GetPageCount(votesTable.TotalRecords, 10);
			if (votesTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetGamesListJson(votesTable, context) + ",";
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

		public string GetGamesListJson(DbQueryResult votesTable, System.Web.HttpContext context)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("\"list\":[");
			System.Data.DataTable dataTable = (System.Data.DataTable)votesTable.Data;
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"game_id\":\"" + dataTable.Rows[i]["VoteId"].ToString() + "\",");
				stringBuilder.Append("\"title\":\"" + dataTable.Rows[i]["VoteName"].ToString() + "\",");
				stringBuilder.Append("\"create_time\":\"" + System.DateTime.Now + "\",");
				stringBuilder.Append("\"type\":\"0\",");
				stringBuilder.Append("\"link\":\"" + context.Server.UrlPathEncode(this.GetUrl(dataTable.Rows[i]["VoteId"])) + "\"");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
			return str + "]";
		}

		public DbQueryResult GetVotesTable(System.Web.HttpContext context)
		{
			return VoteHelper.Query(this.GetVoteSearch(context));
		}

		public VoteSearch GetVoteSearch(System.Web.HttpContext context)
		{
			return new VoteSearch
			{
				status = VoteStatus.In,
				PageIndex = (context.Request.Form["p"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["p"]),
				SortOrder = SortAction.Desc,
				SortBy = "VoteId"
			};
		}

		public string GetUrl(object voteId)
		{
			return string.Concat(new object[]
			{
				"http://",
				Globals.DomainName,
				Globals.ApplicationPath,
				"/BeginVote.aspx?voteId=",
				voteId
			});
		}
	}
}
