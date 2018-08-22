using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_RemoveImgByFolder : System.Web.IHttpHandler
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
			context.Response.Write(this.MoveImgByFolder(context));
		}

		public string MoveImgByFolder(System.Web.HttpContext context)
		{
			DbQueryResult photoList = GalleryHelper.GetPhotoList("", new int?(System.Convert.ToInt32(context.Request.Form["cid"])), 1, 100000000, PhotoListOrder.UploadTimeDesc, 0);
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			System.Data.DataTable dataTable = (System.Data.DataTable)photoList.Data;
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				list.Add(System.Convert.ToInt32(dataTable.Rows[i]["PhotoId"]));
			}
			if (GalleryHelper.MovePhotoType(list, System.Convert.ToInt32(context.Request.Form["cate_id"])) > 0)
			{
				return "{\"status\":1,\"msg\":\"\"}";
			}
			return "{\"status\":0,\"msg\":\"请选择一个分类\"}";
		}
	}
}
