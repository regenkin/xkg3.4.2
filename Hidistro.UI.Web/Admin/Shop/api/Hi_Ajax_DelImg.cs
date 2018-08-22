using Hidistro.ControlPanel.Store;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_DelImg : System.Web.IHttpHandler
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
			context.Response.Write(this.DelImg(context));
		}

		public string DelImg(System.Web.HttpContext context)
		{
			string text = context.Request.Form["file_id[]"];
			if (string.IsNullOrEmpty(text))
			{
				return "{\"status\": 0,\"msg\":\"请勾选图片\"}";
			}
			if (ManagerHelper.GetCurrentManager() == null)
			{
				return "{\"status\": 0,\"msg\":\"请先登录\"}";
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string value = array2[i];
				GalleryHelper.DeletePhoto(System.Convert.ToInt32(value));
			}
			return "{\"status\": 1,\"msg\":\"\"}";
		}
	}
}
