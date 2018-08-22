using System;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.Handler
{
	public class flashupload : System.Web.IHttpHandler
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
			context.Response.Clear();
			context.Response.ClearHeaders();
			context.Response.ClearContent();
			context.Response.Expires = -1;
			try
			{
				System.Web.HttpPostedFile httpPostedFile = context.Request.Files["Filedata"];
				string path = "/Storage/temp/";
				string str = "/Storage/temp/";
				System.IO.FileInfo fileInfo = new System.IO.FileInfo(httpPostedFile.FileName);
				string str2 = System.DateTime.Now.ToString("yyyyMM");
				string text = context.Server.MapPath(path) + str2 + "/";
				if (!System.IO.Directory.Exists(text))
				{
					System.IO.Directory.CreateDirectory(text);
				}
				string a = fileInfo.Extension.ToLower();
				if (a == ".gif" || a == ".jpg" || a == ".png")
				{
					string text2 = string.Concat(new object[]
					{
						"product_",
						System.DateTime.Now.ToString("HHmmss"),
						System.DateTime.Now.Millisecond,
						fileInfo.Extension
					});
					if (System.IO.File.Exists(text + text2))
					{
						System.IO.File.Delete(text + text2);
					}
					httpPostedFile.SaveAs(text + text2);
					context.Response.StatusCode = 200;
					context.Response.Write(str + str2 + "/" + text2);
				}
			}
			catch (System.Exception ex)
			{
				context.Response.StatusCode = 200;
				context.Response.Write(ex.ToString());
			}
		}
	}
}
