using Hidistro.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using ThoughtWorks.QRCode.Codec;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class StoreCardCreater
	{
		private string SetJson = "";

		private JObject resultObj = null;

		private string UserHead = "";

		private string StoreLogo = "";

		private string CodeUrl = "";

		private string StoreName = "";

		private string UserName = "";

		private int userId = 0;

		public StoreCardCreater(string _SetJson, string _UserHeadPath, string _StoreLogoPath, string _CodeUrl, string _UserName, string _StoreName, int _userid)
		{
			this.SetJson = _SetJson;
			this.UserHead = _UserHeadPath;
			this.StoreLogo = _StoreLogoPath;
			this.CodeUrl = _CodeUrl;
			this.UserName = _UserName;
			this.StoreName = _StoreName;
			this.userId = _userid;
		}

		public bool ReadJson()
		{
			this.resultObj = (JsonConvert.DeserializeObject(this.SetJson) as JObject);
			bool result = false;
			if (this.resultObj != null && this.resultObj["writeDate"] != null && this.resultObj["posList"] != null && this.resultObj["DefaultHead"] != null && this.resultObj["myusername"] != null && this.resultObj["shopname"] != null)
			{
				result = true;
			}
			return result;
		}

		private System.Drawing.Bitmap getNetImg(string imgUrl)
		{
			System.Drawing.Bitmap result;
			try
			{
				System.Random random = new System.Random();
				System.Net.WebRequest webRequest = System.Net.WebRequest.Create(imgUrl + "?aid=&" + random.NextDouble());
				System.Net.WebResponse response = webRequest.GetResponse();
				System.IO.Stream responseStream = response.GetResponseStream();
				System.Drawing.Image image = System.Drawing.Image.FromStream(responseStream);
				responseStream.Close();
				responseStream.Dispose();
				result = (System.Drawing.Bitmap)image;
			}
			catch (System.Exception var_5_57)
			{
				result = new System.Drawing.Bitmap(100, 100);
			}
			return result;
		}

		public bool CreadCard(out string imgUrl)
		{
			bool flag = false;
			bool result;
			if (this.resultObj == null || this.resultObj["BgImg"] == null)
			{
				imgUrl = "掌柜名片模板未设置，无法生成名片！";
				result = flag;
			}
			else
			{
				imgUrl = "生成失败";
				System.Drawing.Bitmap bitmap = new QRCodeEncoder
				{
					QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
					QRCodeScale = 8,
					QRCodeVersion = 8,
					QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M
				}.Encode(this.CodeUrl);
				int num = int.Parse(this.resultObj["DefaultHead"].ToString());
				if (string.IsNullOrEmpty(this.UserHead) || (!this.UserHead.ToLower().StartsWith("http") && !System.IO.File.Exists(Globals.MapPath(this.UserHead))))
				{
					this.UserHead = "/Utility/pics/imgnopic.jpg";
				}
				if (!this.StoreLogo.ToLower().StartsWith("http") && !System.IO.File.Exists(Globals.MapPath(this.StoreLogo)))
				{
					this.StoreLogo = "/Utility/pics/headLogo.jpg";
				}
				if (num == 2)
				{
					this.UserHead = "";
				}
				else if (num == 1)
				{
					this.UserHead = this.StoreLogo;
				}
				System.Drawing.Image image;
				if (this.UserHead.ToLower().StartsWith("http"))
				{
					image = this.getNetImg(this.UserHead);
				}
				else if (System.IO.File.Exists(Globals.MapPath(this.UserHead)))
				{
					image = System.Drawing.Image.FromFile(Globals.MapPath(this.UserHead));
				}
				else
				{
					image = new System.Drawing.Bitmap(100, 100);
				}
				System.Drawing.Drawing2D.GraphicsPath graphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
				graphicsPath.AddEllipse(new System.Drawing.Rectangle(0, 0, image.Width, image.Width));
				System.Drawing.Bitmap bitmap2 = new System.Drawing.Bitmap(image.Width, image.Width);
				using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap2))
				{
					graphics.SetClip(graphicsPath);
					graphics.DrawImage(image, 0, 0, image.Width, image.Width);
				}
				image.Dispose();
				bitmap = StoreCardCreater.CombinImage(bitmap, bitmap2, 80);
				System.Drawing.Bitmap bitmap3 = new System.Drawing.Bitmap(480, 735);
				System.Drawing.Graphics graphics2 = System.Drawing.Graphics.FromImage(bitmap3);
				graphics2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				graphics2.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				graphics2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
				graphics2.Clear(System.Drawing.Color.White);
				System.Drawing.Bitmap bitmap4 = new System.Drawing.Bitmap(100, 100);
				if (this.resultObj["BgImg"] != null && System.IO.File.Exists(Globals.MapPath(this.resultObj["BgImg"].ToString())))
				{
					bitmap4 = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(Globals.MapPath(this.resultObj["BgImg"].ToString()));
					bitmap4 = StoreCardCreater.GetThumbnail(bitmap4, 735, 480);
				}
				graphics2.DrawImage(bitmap4, 0, 0, 480, 735);
				System.Drawing.Font font = new System.Drawing.Font("微软雅黑", (float)((int)this.resultObj["myusernameSize"] * 6 / 5));
				System.Drawing.Font font2 = new System.Drawing.Font("微软雅黑", (float)((int)this.resultObj["shopnameSize"] * 6 / 5));
				graphics2.DrawImage(bitmap2, (int)((decimal)this.resultObj["posList"][0]["left"] * 480m), (int)this.resultObj["posList"][0]["top"] * 735 / 490, (int)((decimal)this.resultObj["posList"][0]["width"] * 480m), (int)((decimal)this.resultObj["posList"][0]["width"] * 480m));
				System.Drawing.StringFormat format = new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.DisplayFormatControl);
				string text = this.resultObj["myusername"].ToString().Replace("{{昵称}}", "$");
				string text2 = this.resultObj["shopname"].ToString().Replace("{{店铺名称}}", "$");
				string[] array = text.Split(new char[]
				{
					'$'
				});
				string[] array2 = text2.Split(new char[]
				{
					'$'
				});
				graphics2.DrawString(array[0], font, new System.Drawing.SolidBrush(System.Drawing.ColorTranslator.FromHtml(this.resultObj["myusernameColor"].ToString())), (float)((int)((decimal)this.resultObj["posList"][1]["left"] * 480m)), (float)((int)this.resultObj["posList"][1]["top"] * 735 / 490), format);
				if (array.Length > 1)
				{
					System.Drawing.SizeF sizeF = graphics2.MeasureString(" ", font);
					System.Drawing.SizeF sizeF2 = graphics2.MeasureString(array[0], font);
					graphics2.DrawString(this.UserName, font, new System.Drawing.SolidBrush(System.Drawing.ColorTranslator.FromHtml(this.resultObj["nickNameColor"].ToString())), (float)((int)((decimal)this.resultObj["posList"][1]["left"] * 480m)) + sizeF2.Width - sizeF.Width, (float)((int)this.resultObj["posList"][1]["top"] * 735 / 490), format);
					System.Drawing.SizeF sizeF3 = graphics2.MeasureString(this.UserName, font);
					graphics2.DrawString(array[1], font, new System.Drawing.SolidBrush(System.Drawing.ColorTranslator.FromHtml(this.resultObj["myusernameColor"].ToString())), (float)((int)((decimal)this.resultObj["posList"][1]["left"] * 480m)) + sizeF2.Width - sizeF.Width * 2f + sizeF3.Width, (float)((int)this.resultObj["posList"][1]["top"] * 735 / 490), format);
				}
				graphics2.DrawString(array2[0], font2, new System.Drawing.SolidBrush(System.Drawing.ColorTranslator.FromHtml(this.resultObj["shopnameColor"].ToString())), (float)((int)((decimal)this.resultObj["posList"][2]["left"] * 480m)), (float)((int)this.resultObj["posList"][2]["top"] * 735 / 490));
				if (array2.Length > 1)
				{
					System.Drawing.SizeF sizeF4 = graphics2.MeasureString(" ", font2);
					System.Drawing.SizeF sizeF5 = graphics2.MeasureString(array2[0], font2);
					graphics2.DrawString(this.StoreName, font2, new System.Drawing.SolidBrush(System.Drawing.ColorTranslator.FromHtml(this.resultObj["storeNameColor"].ToString())), (float)((int)((decimal)this.resultObj["posList"][2]["left"] * 480m)) + sizeF5.Width - sizeF4.Width, (float)((int)this.resultObj["posList"][2]["top"] * 735 / 490), format);
					System.Drawing.SizeF sizeF6 = graphics2.MeasureString(this.StoreName, font2);
					graphics2.DrawString(array2[1], font2, new System.Drawing.SolidBrush(System.Drawing.ColorTranslator.FromHtml(this.resultObj["shopnameColor"].ToString())), (float)((int)((decimal)this.resultObj["posList"][2]["left"] * 480m)) + sizeF5.Width - sizeF4.Width * 2f + sizeF6.Width, (float)((int)this.resultObj["posList"][2]["top"] * 735 / 490), format);
				}
				graphics2.DrawImage(bitmap, (int)((decimal)this.resultObj["posList"][3]["left"] * 480m), (int)this.resultObj["posList"][3]["top"] * 735 / 490, (int)((decimal)this.resultObj["posList"][3]["width"] * 480m), (int)((decimal)this.resultObj["posList"][3]["width"] * 480m));
				bitmap.Dispose();
				bitmap3.Save(Globals.MapPath(string.Concat(new object[]
				{
					Globals.GetStoragePath(),
					"/DistributorCards/StoreCard",
					this.userId,
					".jpg"
				})), System.Drawing.Imaging.ImageFormat.Jpeg);
				bitmap3.Dispose();
				flag = true;
				imgUrl = string.Concat(new object[]
				{
					Globals.GetStoragePath(),
					"/DistributorCards/StoreCard",
					this.userId,
					".jpg"
				});
				result = flag;
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

		public static System.Drawing.Bitmap CombinImage(System.Drawing.Bitmap QRimg, System.Drawing.Image Logoimg, int logoW)
		{
			System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(QRimg.Width + 20, QRimg.Height + 20);
			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
			graphics.Clear(System.Drawing.Color.White);
			graphics.DrawImage(QRimg, 10, 10, QRimg.Width, QRimg.Height);
			graphics.DrawImage(Logoimg, (bitmap.Width - logoW) / 2, (bitmap.Height - logoW) / 2, logoW, logoW);
			return bitmap;
		}

		private string getSpaceFill(string ystring)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 0; i < ystring.Length; i++)
			{
				if (ystring[i] > '\u007f')
				{
					stringBuilder.Append("\u3000");
				}
				else
				{
					stringBuilder.Append(" ");
				}
			}
			return stringBuilder.ToString();
		}

		public static System.Drawing.Bitmap GetThumbnail(System.Drawing.Bitmap b, int destHeight, int destWidth)
		{
			System.Drawing.Imaging.ImageFormat rawFormat = b.RawFormat;
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
