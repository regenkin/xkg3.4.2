using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_RenameImg : System.Web.IHttpHandler
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
			if (Globals.RequestFormNum("type") != 3)
			{
				context.Response.Write(this.ReName(context));
				return;
			}
			context.Response.Write("{\"status\": 0,\"msg\":\"商品图片名称请到商品管理页面修改！\"}");
		}

		public string ReName(System.Web.HttpContext context)
		{
			GalleryHelper.RenamePhoto(System.Convert.ToInt32(context.Request.Form["file_id"]), context.Request.Form["file_name"]);
			return "{\"status\": 1,\"msg\":\"\"}";
		}
	}
}
