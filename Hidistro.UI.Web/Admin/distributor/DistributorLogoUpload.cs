using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.distributor
{
	public class DistributorLogoUpload : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (base.Request.QueryString["delimg"] != null)
			{
				string path = base.Server.HtmlEncode(base.Request.QueryString["delimg"]);
				path = base.Server.MapPath(path);
				if (System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);
				}
				base.Response.Write("0");
				base.Response.End();
			}
			int num = int.Parse(base.Request.QueryString["imgurl"]);
			try
			{
				if (num < 1)
				{
					System.Web.HttpPostedFile httpPostedFile = base.Request.Files["Filedata"];
					string str = System.DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					string str2 = "/Storage/data/DistributorLogoPic/";
					string text = str + System.IO.Path.GetExtension(httpPostedFile.FileName);
					httpPostedFile.SaveAs(Globals.MapPath(str2 + text));
					try
					{
						System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(Globals.MapPath(str2 + text));
						if (bitmap.Height > 200 || bitmap.Width > 200)
						{
							bitmap = DistributorLogoUpload.GetThumbnail(bitmap, 200, 200);
						}
						bitmap.Save(Globals.MapPath("/Utility/pics/headLogo.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
						bitmap.Dispose();
					}
					catch (System.Exception)
					{
					}
					base.Response.StatusCode = 200;
					base.Response.Write(str + "|/Storage/data/DistributorLogoPic/" + text);
					SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
					string path2 = masterSettings.DistributorLogoPic;
					path2 = base.Server.MapPath(path2);
					if (System.IO.File.Exists(path2))
					{
						System.IO.File.Delete(path2);
					}
					masterSettings.DistributorLogoPic = "/Storage/data/DistributorLogoPic/" + text;
					SettingsManager.Save(masterSettings);
				}
				else
				{
					base.Response.Write("0");
				}
			}
			catch (System.Exception)
			{
				base.Response.StatusCode = 500;
				base.Response.Write("服务器错误");
				base.Response.End();
			}
			finally
			{
				base.Response.End();
			}
		}

		public static System.Drawing.Bitmap GetThumbnail(System.Drawing.Bitmap b, int destHeight, int destWidth)
		{
			System.Drawing.Imaging.ImageFormat arg_08_0 = b.RawFormat;
			int width = b.Width;
			int height = b.Height;
			int num;
			int num2;
			if (height > destHeight || width > destWidth)
			{
				if (width * destHeight < height * destWidth)
				{
					num = destWidth;
					num2 = destWidth * height / width;
				}
				else
				{
					num2 = destHeight;
					num = width * destHeight / height;
				}
			}
			else
			{
				num = destWidth;
				num2 = destHeight;
			}
			System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(destWidth, destHeight);
			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
			graphics.Clear(System.Drawing.Color.Transparent);
			graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			graphics.DrawImage(b, new System.Drawing.Rectangle((destWidth - num) / 2, (destHeight - num2) / 2, num, num2), 0, 0, b.Width, b.Height, System.Drawing.GraphicsUnit.Pixel);
			graphics.Dispose();
			System.Drawing.Imaging.EncoderParameters encoderParameters = new System.Drawing.Imaging.EncoderParameters();
			long[] value = new long[]
			{
				100L
			};
			System.Drawing.Imaging.EncoderParameter encoderParameter = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, value);
			encoderParameters.Param[0] = encoderParameter;
			b.Dispose();
			return bitmap;
		}
	}
}
