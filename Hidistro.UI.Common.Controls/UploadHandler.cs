using Hidistro.Core;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace Hidistro.UI.Common.Controls
{
	public class UploadHandler : IHttpHandler
	{
		private string uploaderId;

		private string uploadType;

		private string action;

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			this.uploaderId = context.Request.QueryString["uploaderId"];
			this.uploadType = context.Request.QueryString["uploadType"];
			this.action = context.Request.QueryString["action"];
			context.Response.Clear();
			context.Response.ClearHeaders();
			context.Response.ClearContent();
			context.Response.Expires = -1;
			try
			{
				if (this.action.Equals("upload"))
				{
					this.DoUpload(context);
				}
				else if (this.action.Equals("delete"))
				{
					this.DoDelete(context);
				}
			}
			catch (Exception ex)
			{
				this.WriteBackError(context, ex.Message);
			}
		}

		private void DoUpload(HttpContext context)
		{
			if (context.Request.Files.Count == 0)
			{
				this.WriteBackError(context, "没有检测到任何文件");
				return;
			}
			HttpPostedFile httpPostedFile = context.Request.Files[0];
			int num = 1;
			while (httpPostedFile.ContentLength == 0 && num < context.Request.Files.Count)
			{
				httpPostedFile = context.Request.Files[num];
				num++;
			}
			if (httpPostedFile.ContentLength == 0)
			{
				this.WriteBackError(context, "当前文件没有任何内容");
				return;
			}
			if (!httpPostedFile.ContentType.ToLower().StartsWith("image/") || !Regex.IsMatch(Path.GetExtension(httpPostedFile.FileName.ToLower()), "\\.(jpg|gif|png|bmp|jpeg)$", RegexOptions.Compiled))
			{
				this.WriteBackError(context, "文件类型错误，请选择有效的图片文件");
				return;
			}
			this.UploadImage(context, httpPostedFile);
		}

		private void DoDelete(HttpContext context)
		{
			string text = context.Request.MapPath(Globals.ApplicationPath + context.Request.Form[this.uploaderId + "_uploadedImageUrl"]);
			string path = text.Replace("\\images\\", "\\thumbs40\\40_");
			string path2 = text.Replace("\\images\\", "\\thumbs60\\60_");
			string path3 = text.Replace("\\images\\", "\\thumbs100\\100_");
			string path4 = text.Replace("\\images\\", "\\thumbs160\\160_");
			string path5 = text.Replace("\\images\\", "\\thumbs180\\180_");
			string path6 = text.Replace("\\images\\", "\\thumbs220\\220_");
			string path7 = text.Replace("\\images\\", "\\thumbs310\\310_");
			string path8 = text.Replace("\\images\\", "\\thumbs410\\410_");
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			if (File.Exists(path2))
			{
				File.Delete(path2);
			}
			if (File.Exists(path3))
			{
				File.Delete(path3);
			}
			if (File.Exists(path4))
			{
				File.Delete(path4);
			}
			if (File.Exists(path5))
			{
				File.Delete(path5);
			}
			if (File.Exists(path6))
			{
				File.Delete(path6);
			}
			if (File.Exists(path7))
			{
				File.Delete(path7);
			}
			if (File.Exists(path8))
			{
				File.Delete(path8);
			}
			context.Response.Write("<script type=\"text/javascript\">window.parent.DeleteCallback('" + this.uploaderId + "');</script>");
		}

		private void UploadImage(HttpContext context, HttpPostedFile file)
		{
			string str = Globals.GetStoragePath() + "/" + this.uploadType;
			string str2 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + Path.GetExtension(file.FileName);
			string str3 = str + "/images/" + str2;
			string str4 = str + "/thumbs40/40_" + str2;
			string str5 = str + "/thumbs60/60_" + str2;
			string str6 = str + "/thumbs100/100_" + str2;
			string str7 = str + "/thumbs160/160_" + str2;
			string str8 = str + "/thumbs180/180_" + str2;
			string str9 = str + "/thumbs220/220_" + str2;
			string str10 = str + "/thumbs310/310_" + str2;
			string str11 = str + "/thumbs410/410_" + str2;
			file.SaveAs(context.Request.MapPath(Globals.ApplicationPath + str3));
			string sourceFilename = context.Request.MapPath(Globals.ApplicationPath + str3);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str4), 40, 40);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str5), 60, 60);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str6), 100, 100);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str7), 160, 160);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str8), 180, 180);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str9), 220, 220);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str10), 310, 310);
			ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str11), 410, 410);
			string[] value = new string[]
			{
				"'" + this.uploadType + "'",
				"'" + this.uploaderId + "'",
				"'" + str3 + "'",
				"'" + str4 + "'",
				"'" + str5 + "'",
				"'" + str6 + "'",
				"'" + str7 + "'",
				"'" + str8 + "'",
				"'" + str9 + "'",
				"'" + str10 + "'",
				"'" + str11 + "'"
			};
			context.Response.Write("<script type=\"text/javascript\">window.parent.UploadCallback(" + string.Join(",", value) + ");</script>");
		}

		private void WriteBackError(HttpContext context, string error)
		{
			string[] value = new string[]
			{
				"'" + this.uploadType + "'",
				"'" + this.uploaderId + "'",
				"'" + error + "'"
			};
			context.Response.Write("<script type=\"text/javascript\">window.parent.ErrorCallback(" + string.Join(",", value) + ");</script>");
		}
	}
}
