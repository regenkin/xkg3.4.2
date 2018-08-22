using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace Hidistro.UI.Web.API
{
	public class GetQRCode : System.Web.IHttpHandler
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
			if (!string.IsNullOrEmpty(text))
			{
				System.Drawing.Image image = new QRCodeEncoder
				{
					QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
					QRCodeScale = 4,
					QRCodeVersion = 8,
					QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M
				}.Encode(text);
				System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
				image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
				string filename = context.Server.MapPath("/Storage/master/QRcord.jpg");
				System.Drawing.Image imgBack = System.Drawing.Image.FromFile(filename);
				System.IO.MemoryStream memoryStream2 = new System.IO.MemoryStream();
				GetQRCode.CombinImage(imgBack, image).Save(memoryStream2, System.Drawing.Imaging.ImageFormat.Png);
				context.Response.ClearContent();
				context.Response.ContentType = "image/png";
				context.Response.BinaryWrite(memoryStream2.ToArray());
				memoryStream.Dispose();
				memoryStream2.Dispose();
			}
			context.Response.Flush();
			context.Response.End();
		}

		public static System.Drawing.Image CombinImage(System.Drawing.Image imgBack, System.Drawing.Image img)
		{
			if (img.Height != 65 || img.Width != 65)
			{
				img = GetQRCode.KiResizeImage(img, 250, 250, 0);
			}
			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(imgBack);
			graphics.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);
			graphics.DrawImage(img, imgBack.Width / 2 - img.Width / 2 + 10, imgBack.Width / 2 - img.Width / 2 + 85, 136, 136);
			System.GC.Collect();
			return imgBack;
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
