using Hidistro.ControlPanel.Store;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_AddFolder : System.Web.IHttpHandler
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
			context.Response.Write(this.InsertFolder());
		}

		public string InsertFolder()
		{
			int num = GalleryHelper.AddPhotoCategory2("新建文件夹");
			return "{\"status\":1,\"data\":" + num + ",\"msg\":\"\"}";
		}
	}
}
