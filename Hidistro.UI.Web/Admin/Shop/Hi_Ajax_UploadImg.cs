using System;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_UploadImg : System.Web.IHttpHandler
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
			context.Response.Charset = "utf-8";
			System.Web.HttpPostedFile httpPostedFile = context.Request.Files["Filedata"];
			string text = System.Web.HttpContext.Current.Server.MapPath(context.Request["folder"]) + "\\";
			if (httpPostedFile != null)
			{
				if (!System.IO.Directory.Exists(text))
				{
					System.IO.Directory.CreateDirectory(text);
				}
				httpPostedFile.SaveAs(text + httpPostedFile.FileName);
				context.Response.Write("1");
				return;
			}
			context.Response.Write("0");
		}
	}
}
