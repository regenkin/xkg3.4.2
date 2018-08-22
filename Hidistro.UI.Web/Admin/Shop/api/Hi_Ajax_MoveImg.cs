using Hidistro.ControlPanel.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_MoveImg : System.Web.IHttpHandler
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
			context.Response.Write(this.ModelFolder(context));
		}

		public string ModelFolder(System.Web.HttpContext context)
		{
			string text = context.Request.Form["file_id[]"];
			System.Collections.Generic.List<int> pList = (from x in text.Split(new char[]
			{
				','
			}).ToList<string>()
			select int.Parse(x)).ToList<int>();
			if (GalleryHelper.MovePhotoType(pList, System.Convert.ToInt32(context.Request.Form["cate_id"])) > 0)
			{
				return "{\"status\":1,\"msg\":\"\"}";
			}
			return "{\"status\":0,\"msg\":\"请选择一个分类\"}";
		}
	}
}
