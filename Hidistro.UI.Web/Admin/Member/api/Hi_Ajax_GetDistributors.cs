using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.Member.api
{
	public class Hi_Ajax_GetDistributors : System.Web.IHttpHandler
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
			context.Response.Write(this.GetJson(context));
		}

		public string GetJson(System.Web.HttpContext context)
		{
			System.Data.DataTable dataTable = DistributorsBrower.SelectDistributors(new DistributorsQuery
			{
				StoreName = (context.Request.QueryString["userName"] != null) ? context.Request.QueryString["userName"] : ""
			});
			string text = "[";
			if (dataTable.Rows.Count > 0)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					text += "{";
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						"\"StoreName\":\"",
						dataTable.Rows[i]["StoreName"],
						"\","
					});
					object obj2 = text;
					text = string.Concat(new object[]
					{
						obj2,
						"\"UserId\":\"",
						dataTable.Rows[i]["UserId"],
						"\""
					});
					text += "},";
				}
				text = text.TrimEnd(new char[]
				{
					','
				});
			}
			return text + "]";
		}
	}
}
