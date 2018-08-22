using Hidistro.Core;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace Hidistro.UI.Web.API
{
	public class CreatQRCode : System.Web.IHttpHandler
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
			string text = context.Request["code"];
			string text2 = context.Request["Combin"];
			string text3 = context.Request["Logo"];
			if (!string.IsNullOrEmpty(text))
			{
				System.Drawing.Image image = new QRCodeEncoder
				{
					QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
					QRCodeScale = 6,
					QRCodeVersion = 5,
					QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M
				}.Encode(text);
				System.Drawing.Image image2 = null;
				if (!string.IsNullOrEmpty(text3))
				{
					if (!text3.ToLower().StartsWith("http") && System.IO.File.Exists(context.Server.MapPath(text3)))
					{
						image2 = System.Drawing.Image.FromFile(context.Server.MapPath(text3));
					}
					else if (text3.ToLower().StartsWith("http"))
					{
						image2 = this.getNetImg(text3);
					}
				}
				System.Drawing.Bitmap bitmap = null;
				if (image2 != null)
				{
					bitmap = new System.Drawing.Bitmap(200, 200);
					System.Drawing.Drawing2D.GraphicsPath clip = CreatQRCode.CreateRoundedRectanglePath(new System.Drawing.Rectangle(0, 0, 200, 200), 20);
					using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap))
					{
						graphics.SetClip(clip);
						graphics.Clear(System.Drawing.Color.White);
						clip = CreatQRCode.CreateRoundedRectanglePath(new System.Drawing.Rectangle(14, 14, 172, 172), 14);
						graphics.SetClip(clip);
						graphics.DrawImage(image2, 0, 0, 200, 200);
					}
					image2.Dispose();
				}
				image = CreatQRCode.CombinImage(image, bitmap, 80, 30);
				System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
				image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
				context.Response.ClearContent();
				context.Response.ContentType = "image/png";
				context.Response.BinaryWrite(memoryStream.ToArray());
				memoryStream.Dispose();
				image.Dispose();
			}
			else if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
			{
				System.Drawing.Image image3 = null;
				if (!text3.ToLower().StartsWith("http") && System.IO.File.Exists(context.Server.MapPath(text3)))
				{
					image3 = System.Drawing.Image.FromFile(context.Server.MapPath(text3));
				}
				else if (text3.ToLower().StartsWith("http"))
				{
					image3 = this.getNetImg(text3);
				}
				else
				{
					context.Response.End();
				}
				System.Drawing.Image image4 = null;
				if (!text2.ToLower().StartsWith("http") && System.IO.File.Exists(context.Server.MapPath(text2)))
				{
					image4 = System.Drawing.Image.FromFile(context.Server.MapPath(text2));
				}
				else if (text2.ToLower().StartsWith("http"))
				{
					image4 = this.getNetImg(Globals.UrlDecode(text2));
				}
				else
				{
					context.Response.End();
				}
				System.Drawing.Bitmap bitmap2 = null;
				if (image3 != null)
				{
					bitmap2 = new System.Drawing.Bitmap(200, 200);
					System.Drawing.Drawing2D.GraphicsPath clip2 = CreatQRCode.CreateRoundedRectanglePath(new System.Drawing.Rectangle(0, 0, 200, 200), 20);
					using (System.Drawing.Graphics graphics2 = System.Drawing.Graphics.FromImage(bitmap2))
					{
						graphics2.SetClip(clip2);
						graphics2.Clear(System.Drawing.Color.White);
						clip2 = CreatQRCode.CreateRoundedRectanglePath(new System.Drawing.Rectangle(14, 14, 172, 172), 14);
						graphics2.SetClip(clip2);
						graphics2.DrawImage(image3, 0, 0, 200, 200);
					}
					image3.Dispose();
				}
				image4 = CreatQRCode.CombinImage(image4, bitmap2, 80, 0);
				System.IO.MemoryStream memoryStream2 = new System.IO.MemoryStream();
				image4.Save(memoryStream2, System.Drawing.Imaging.ImageFormat.Png);
				context.Response.ClearContent();
				context.Response.ContentType = "image/png";
				context.Response.BinaryWrite(memoryStream2.ToArray());
				memoryStream2.Dispose();
				image4.Dispose();
			}
			context.Response.Flush();
			context.Response.End();
		}

		private System.Drawing.Image getNetImg(string imgUrl)
		{
			System.Drawing.Image result;
			try
			{
				if (imgUrl.ToLower().StartsWith("https"))
				{
					System.Net.ServicePointManager.ServerCertificateValidationCallback = ((object se, System.Security.Cryptography.X509Certificates.X509Certificate ert, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslerror) => true);
				}
				System.Random random = new System.Random();
				if (imgUrl.Contains("?"))
				{
					imgUrl = imgUrl + "&aid=&" + random.NextDouble();
				}
				else
				{
					imgUrl = imgUrl + "?aid=&" + random.NextDouble();
				}
				System.Net.WebRequest webRequest = System.Net.WebRequest.Create(imgUrl);
				System.Net.WebResponse response = webRequest.GetResponse();
				System.IO.Stream responseStream = response.GetResponseStream();
				System.Drawing.Image image = System.Drawing.Image.FromStream(responseStream);
				responseStream.Close();
				result = image;
			}
			catch (System.Exception ex)
			{
				Globals.Debuglog(imgUrl + ";读取网络图片异常" + ex.Message, "_Debuglog.txt");
				result = new System.Drawing.Bitmap(100, 100);
			}
			return result;
		}

		internal static System.Drawing.Drawing2D.GraphicsPath CreateRoundedRectanglePath(System.Drawing.Rectangle rect, int cornerRadius)
		{
			System.Drawing.Drawing2D.GraphicsPath graphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
			graphicsPath.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180f, 90f);
			graphicsPath.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
			graphicsPath.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270f, 90f);
			graphicsPath.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
			graphicsPath.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0f, 90f);
			graphicsPath.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
			graphicsPath.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90f, 90f);
			graphicsPath.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		public static System.Drawing.Image CombinImage(System.Drawing.Image QRimg, System.Drawing.Image Logoimg, int logoW, int WhiteSpace)
		{
			System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(430, 430);
			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
			graphics.Clear(System.Drawing.Color.White);
			graphics.DrawImage(QRimg, WhiteSpace, WhiteSpace, 430 - WhiteSpace * 2, 430 - WhiteSpace * 2);
			if (Logoimg != null)
			{
				graphics.DrawImage(Logoimg, (bitmap.Width - logoW) / 2, (bitmap.Height - logoW) / 2, logoW, logoW);
			}
			return bitmap;
		}

		public static System.Drawing.Image KiResizeImage(System.Drawing.Image bmp, int newW, int newH, int Mode)
		{
			System.Drawing.Image result;
			try
			{
				System.Drawing.Image image = new System.Drawing.Bitmap(newW, newH);
				System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(image);
				graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, newW, newH), new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.GraphicsUnit.Pixel);
				graphics.Dispose();
				result = image;
			}
			catch
			{
				result = null;
			}
			return result;
		}
	}
}
