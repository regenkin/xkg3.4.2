using Hidistro.ControlPanel.Store;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_RenameFolder : System.Web.IHttpHandler
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
			context.Response.Write(this.RenameFolder(context));
		}

		public string RenameFolder(System.Web.HttpContext context)
		{
			if (GalleryHelper.UpdatePhotoCategories(new System.Collections.Generic.Dictionary<int, string>
			{
				{
					System.Convert.ToInt32(context.Request.Form["category_img_id"]),
					context.Request.Form["name"]
				}
			}) > 0)
			{
				return "{\"status\":1,\"msg\":\"\"}";
			}
			return "{\"status\":0,\"msg\":\"请选择一个分类\"}";
		}
	}
}
