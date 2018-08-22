using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Handler1 : System.Web.IHttpHandler
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
			context.Response.Write("Hello World");
		}
	}
}
