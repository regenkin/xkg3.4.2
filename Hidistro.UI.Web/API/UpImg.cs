using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class UpImg : System.Web.IHttpHandler
	{
		private string uploaderId;

		private string uploadType;

		private string action;

		private string snailtype;

		private string uploadSize;

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
			this.uploaderId = context.Request.QueryString["uploaderId"];
			this.uploadType = context.Request.QueryString["uploadType"];
			this.action = context.Request.QueryString["action"];
			this.snailtype = context.Request.QueryString["snailtype"];
			this.uploadSize = context.Request.QueryString["uploadSize"];
			context.Response.Clear();
			context.Response.ClearHeaders();
			context.Response.ClearContent();
			context.Response.Expires = -1;
			try
			{
				if (this.action.Equals("upload"))
				{
					this.DoUpload(context, this.snailtype);
				}
				else if (this.action.Equals("delete"))
				{
					this.DoDelete(context, this.snailtype);
				}
			}
			catch (System.Exception ex)
			{
				this.WriteBackError(context, ex.Message);
			}
		}

		private void DoUpload(System.Web.HttpContext context, string snailtype)
		{
			if (context.Request.Files.Count == 0)
			{
				this.WriteBackError(context, "没有检测到任何文件");
				return;
			}
			System.Web.HttpPostedFile httpPostedFile = context.Request.Files[0];
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
			if (!httpPostedFile.ContentType.ToLower().StartsWith("image/") || !System.Text.RegularExpressions.Regex.IsMatch(System.IO.Path.GetExtension(httpPostedFile.FileName.ToLower()), "\\.(jpg|gif|png|bmp|jpeg)$", System.Text.RegularExpressions.RegexOptions.Compiled))
			{
				this.WriteBackError(context, "文件类型错误，请选择有效的图片文件");
				return;
			}
			this.UploadImage(context, httpPostedFile, snailtype);
		}

		private void DoDelete(System.Web.HttpContext context, string snailtype)
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
			if (System.IO.File.Exists(text))
			{
				System.IO.File.Delete(text);
			}
			if (snailtype == "1")
			{
				if (System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);
				}
				if (System.IO.File.Exists(path2))
				{
					System.IO.File.Delete(path2);
				}
				if (System.IO.File.Exists(path3))
				{
					System.IO.File.Delete(path3);
				}
				if (System.IO.File.Exists(path4))
				{
					System.IO.File.Delete(path4);
				}
				if (System.IO.File.Exists(path5))
				{
					System.IO.File.Delete(path5);
				}
				if (System.IO.File.Exists(path6))
				{
					System.IO.File.Delete(path6);
				}
				if (System.IO.File.Exists(path7))
				{
					System.IO.File.Delete(path7);
				}
				if (System.IO.File.Exists(path8))
				{
					System.IO.File.Delete(path8);
				}
			}
			context.Response.Write("<script type=\"text/javascript\">window.parent.DeleteCallback('" + this.uploaderId + "');</script>");
		}

		private void UploadImage(System.Web.HttpContext context, System.Web.HttpPostedFile file, string snailtype)
		{
			string str = "/storage/data/grade";
			string text = System.Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) + System.IO.Path.GetExtension(file.FileName);
			string str2 = str + "/images/" + text;
			if (this.uploadType.ToLower() == UploadType.SharpPic.ToString().ToLower())
			{
				str = "/Storage/data/Sharp/";
				text = System.DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
				str2 = str + text;
			}
			else if (this.uploadType.ToLower() == UploadType.Vote.ToString().ToLower())
			{
				str = "/Storage/master/vote/";
				str2 = str + text;
			}
			else if (this.uploadType.ToLower() == UploadType.Topic.ToString().ToLower())
			{
				str = "/Storage/master/topic/";
				str2 = str + text;
			}
			else if (this.uploadType.ToLower() == UploadType.Weibo.ToString().ToLower())
			{
				str = "/Storage/master/Weibo/";
				str2 = str + text;
			}
			if (this.uploadType.ToLower() == UploadType.Brand.ToString().ToLower())
			{
				str = "/Storage/master/brand/";
				str2 = str + text;
			}
			else if (this.uploadType.ToLower() == UploadType.ShopMenu.ToString().ToLower())
			{
				str = "/Storage/master/ShopMenu/";
				str2 = str + text;
				int num = 0;
				if (!string.IsNullOrEmpty(this.uploadSize))
				{
					if (!int.TryParse(this.uploadSize, out num))
					{
						this.WriteBackError(context, "UploadSize属性值只能是数字！");
						return;
					}
					if (file.ContentLength > num)
					{
						this.WriteBackError(context, "文件大小不超过10KB!");
						return;
					}
				}
				string text2 = System.IO.Path.GetExtension(file.FileName).ToLower();
				if (!text2.Equals(".gif") && !text2.Equals(".jpg") && !text2.Equals(".jpeg") && !text2.Equals(".png") && !text2.Equals(".bmp"))
				{
					this.WriteBackError(context, "请上传正确的图片文件。");
					return;
				}
			}
			string str3 = str + "/thumbs40/40_" + text;
			string str4 = str + "/thumbs60/60_" + text;
			string str5 = str + "/thumbs100/100_" + text;
			string str6 = str + "/thumbs160/160_" + text;
			string str7 = str + "/thumbs180/180_" + text;
			string str8 = str + "/thumbs220/220_" + text;
			string str9 = str + "/thumbs310/310_" + text;
			string str10 = str + "/thumbs410/410_" + text;
			file.SaveAs(context.Request.MapPath(Globals.ApplicationPath + str2));
			string sourceFilename = context.Request.MapPath(Globals.ApplicationPath + str2);
			if (snailtype == "1")
			{
				ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str3), 40, 40);
				ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str4), 60, 60);
				ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str5), 100, 100);
				ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str6), 160, 160);
				ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str7), 180, 180);
				ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str8), 220, 220);
				ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str9), 310, 310);
				ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str10), 410, 410);
			}
			string[] value = new string[]
			{
				"'" + this.uploadType + "'",
				"'" + this.uploaderId + "'",
				"'" + str2 + "'",
				string.Concat(new string[]
				{
					"'",
					snailtype,
					"','",
					this.uploadSize,
					"'"
				})
			};
			context.Response.Write("<script type=\"text/javascript\">window.parent.UploadCallback(" + string.Join(",", value) + ");</script>");
		}

		private void WriteBackError(System.Web.HttpContext context, string error)
		{
			string[] value = new string[]
			{
				"'" + this.uploadType + "'",
				"'" + this.uploaderId + "'",
				"'" + error + "'",
				"'" + this.snailtype + "'",
				"'" + this.uploadSize + "'"
			};
			context.Response.Write("<script type=\"text/javascript\">window.parent.ErrorCallback(" + string.Join(",", value) + ");</script>");
		}
	}
}
