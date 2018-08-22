using System;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GoodsGourp : System.Web.IHttpHandler
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
			context.Response.Write(this.GetGoodsGroupJson(context));
		}

		public string GetGoodsGroupJson(System.Web.HttpContext context)
		{
			string path = context.Server.MapPath("/Data/GoodsGroupJson.json");
			return System.IO.File.ReadAllText(path);
		}
	}
}
