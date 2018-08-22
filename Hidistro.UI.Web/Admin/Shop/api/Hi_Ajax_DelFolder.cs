using Hidistro.ControlPanel.Store;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_DelFolder : System.Web.IHttpHandler
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
			context.Response.Write(this.DelFolder(context));
		}

		public string DelFolder(System.Web.HttpContext context)
		{
			if (GalleryHelper.DeletePhotoCategory(System.Convert.ToInt32(context.Request["id"])))
			{
				return "{\"status\":1,\"msg\":\"\"}";
			}
			return "{\"status\":0,\"msg\":\"请选择一个分类\"}";
		}
	}
}
