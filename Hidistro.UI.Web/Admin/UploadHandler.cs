using Hidistro.Core;
using System;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.Admin
{
	public class UploadHandler : System.Web.IHttpHandler
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
			System.Web.HttpRequest request = context.Request;
			string text = request["action"];
			string a;
			if ((a = text) != null)
			{
				if (a == "upload")
				{
					this.UploadImage();
					return;
				}
				if (a == "delete")
				{
					this.DeleteImage();
					return;
				}
			}
			context.Response.Write("false");
		}

		private void UploadImage()
		{
			try
			{
				System.Web.HttpPostedFile httpPostedFile = System.Web.HttpContext.Current.Request.Files["Filedata"];
				string str = System.DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
				string text = System.Web.HttpContext.Current.Request["uploadpath"];
				string str2 = str + System.IO.Path.GetExtension(httpPostedFile.FileName);
				if (string.IsNullOrEmpty(text))
				{
					text = Globals.GetVshopSkinPath(null) + "/images/ad/";
					str2 = "imgCustomBg" + System.IO.Path.GetExtension(httpPostedFile.FileName);
					string[] files = System.IO.Directory.GetFiles(Globals.MapPath(text), "imgCustomBg.*");
					string[] array = files;
					for (int i = 0; i < array.Length; i++)
					{
						string path = array[i];
						System.IO.File.Delete(path);
					}
				}
				if (!System.IO.Directory.Exists(Globals.MapPath(text)))
				{
					System.IO.Directory.CreateDirectory(Globals.MapPath(text));
				}
				httpPostedFile.SaveAs(Globals.MapPath(text + str2));
				System.Web.HttpContext.Current.Response.Write(text + str2);
			}
			catch (System.Exception)
			{
				System.Web.HttpContext.Current.Response.Write("服务器错误");
				System.Web.HttpContext.Current.Response.End();
			}
		}

		private void DeleteImage()
		{
			string path = System.Web.HttpContext.Current.Request.Form["del"];
			string path2 = Globals.PhysicalPath(path);
			try
			{
				if (System.IO.File.Exists(path2))
				{
					System.IO.File.Delete(path2);
					System.Web.HttpContext.Current.Response.Write("true");
				}
			}
			catch (System.Exception)
			{
				System.Web.HttpContext.Current.Response.Write("false");
			}
		}
	}
}
