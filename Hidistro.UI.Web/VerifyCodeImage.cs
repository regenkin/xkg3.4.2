using Hidistro.Core;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web
{
	public class VerifyCodeImage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				base.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
				string text = Globals.CreateVerifyCode(4);
				int num = 45;
				int num2 = text.Length * 20;
				System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(num2 - 3, 40);
				System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
				graphics.Clear(System.Drawing.Color.AliceBlue);
				graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Gray, 0f), 0, 0, bitmap.Width - 1, bitmap.Height - 3);
				System.Random random = new System.Random();
				System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.LightGray, 0f);
				for (int i = 0; i < 50; i++)
				{
					int x = random.Next(0, bitmap.Width);
					int y = random.Next(0, bitmap.Height);
					graphics.DrawRectangle(pen, x, y, 1, 1);
				}
				char[] array = text.ToCharArray();
				System.Drawing.StringFormat stringFormat = new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.NoClip);
				stringFormat.Alignment = System.Drawing.StringAlignment.Center;
				stringFormat.LineAlignment = System.Drawing.StringAlignment.Center;
				System.Drawing.Color[] array2 = new System.Drawing.Color[]
				{
					System.Drawing.Color.Black,
					System.Drawing.Color.Red,
					System.Drawing.Color.DarkBlue,
					System.Drawing.Color.Green,
					System.Drawing.Color.Brown,
					System.Drawing.Color.DarkCyan,
					System.Drawing.Color.Purple,
					System.Drawing.Color.DarkGreen
				};
				for (int j = 0; j < array.Length; j++)
				{
					int num3 = random.Next(7);
					random.Next(4);
					System.Drawing.Font font = new System.Drawing.Font("Microsoft Sans Serif", 17f, System.Drawing.FontStyle.Bold);
					System.Drawing.Brush brush = new System.Drawing.SolidBrush(array2[num3]);
					System.Drawing.Point point = new System.Drawing.Point(14, 11);
					float num4 = (float)random.Next(-num, num);
					graphics.TranslateTransform((float)point.X, (float)point.Y);
					graphics.RotateTransform(num4);
					graphics.DrawString(array[j].ToString(), font, brush, 1f, 10f, stringFormat);
					graphics.RotateTransform(-num4);
					graphics.TranslateTransform(2f, (float)(-(float)point.Y));
				}
				System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
				bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Gif);
				base.Response.ClearContent();
				base.Response.ContentType = "image/gif";
				base.Response.BinaryWrite(memoryStream.ToArray());
				graphics.Dispose();
				bitmap.Dispose();
			}
			catch
			{
			}
		}
	}
}
