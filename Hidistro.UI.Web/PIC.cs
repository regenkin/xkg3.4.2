using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Hidistro.UI.Web
{
	public class PIC
	{
		private System.Drawing.Bitmap _outBmp;

		public System.Drawing.Bitmap OutBMP
		{
			get
			{
				return this._outBmp;
			}
		}

		private static System.Drawing.Size NewSize(int maxWidth, int maxHeight, int width, int height)
		{
			double num = System.Convert.ToDouble(width);
			double num2 = System.Convert.ToDouble(height);
			double num3 = System.Convert.ToDouble(maxWidth);
			double num4 = System.Convert.ToDouble(maxHeight);
			double num5;
			double num6;
			if (num < num3 && num2 < num4)
			{
				num5 = num;
				num6 = num2;
			}
			else if (num / num2 > num3 / num4)
			{
				num5 = (double)maxWidth;
				num6 = num5 * num2 / num;
			}
			else
			{
				num6 = (double)maxHeight;
				num5 = num6 * num / num2;
			}
			return new System.Drawing.Size(System.Convert.ToInt32(num5), System.Convert.ToInt32(num6));
		}

		public static void SendSmallImage(string fileName, string newFile, int maxHeight, int maxWidth)
		{
			System.Drawing.Image image = null;
			System.Drawing.Bitmap bitmap = null;
			System.Drawing.Graphics graphics = null;
			try
			{
				image = System.Drawing.Image.FromFile(fileName);
				System.Drawing.Imaging.ImageFormat rawFormat = image.RawFormat;
				System.Drawing.Size size = PIC.NewSize(maxWidth, maxHeight, image.Width, image.Height);
				bitmap = new System.Drawing.Bitmap(size.Width, size.Height);
				graphics = System.Drawing.Graphics.FromImage(bitmap);
				graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(image, new System.Drawing.Rectangle(0, 0, size.Width, size.Height), 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel);
				if (graphics != null)
				{
					graphics.Dispose();
				}
				System.Drawing.Imaging.EncoderParameters encoderParameters = new System.Drawing.Imaging.EncoderParameters();
				long[] value = new long[]
				{
					100L
				};
				System.Drawing.Imaging.EncoderParameter encoderParameter = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, value);
				encoderParameters.Param[0] = encoderParameter;
				System.Drawing.Imaging.ImageCodecInfo[] imageEncoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
				System.Drawing.Imaging.ImageCodecInfo imageCodecInfo = null;
				for (int i = 0; i < imageEncoders.Length; i++)
				{
					if (imageEncoders[i].FormatDescription.Equals("JPEG"))
					{
						imageCodecInfo = imageEncoders[i];
						break;
					}
				}
				if (imageCodecInfo != null)
				{
					bitmap.Save(newFile, imageCodecInfo, encoderParameters);
				}
				else
				{
					bitmap.Save(newFile, rawFormat);
				}
			}
			catch
			{
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
				if (bitmap != null)
				{
					bitmap.Dispose();
				}
			}
		}

		public void Dispose()
		{
			if (this._outBmp != null)
			{
				this._outBmp.Dispose();
				this._outBmp = null;
			}
		}

		public void SendSmallImage(string fileName, int maxHeight, int maxWidth)
		{
			System.Drawing.Image image = null;
			this._outBmp = null;
			System.Drawing.Graphics graphics = null;
			try
			{
				image = System.Drawing.Image.FromFile(fileName);
				System.Drawing.Imaging.ImageFormat arg_18_0 = image.RawFormat;
				System.Drawing.Size size = PIC.NewSize(maxWidth, maxHeight, image.Width, image.Height);
				this._outBmp = new System.Drawing.Bitmap(size.Width, size.Height);
				graphics = System.Drawing.Graphics.FromImage(this._outBmp);
				graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(image, new System.Drawing.Rectangle(0, 0, size.Width, size.Height), 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel);
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
			catch
			{
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
			}
		}

		public System.IO.MemoryStream AddImageSignPic(System.Drawing.Image img, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
		{
			System.Drawing.Graphics graphics = null;
			System.Drawing.Image image = null;
			System.Drawing.Imaging.ImageAttributes imageAttributes = null;
			System.IO.MemoryStream result;
			try
			{
				graphics = System.Drawing.Graphics.FromImage(img);
				image = new System.Drawing.Bitmap(watermarkFilename);
				imageAttributes = new System.Drawing.Imaging.ImageAttributes();
				System.Drawing.Imaging.ColorMap[] map = new System.Drawing.Imaging.ColorMap[]
				{
					new System.Drawing.Imaging.ColorMap
					{
						OldColor = System.Drawing.Color.FromArgb(255, 0, 255, 0),
						NewColor = System.Drawing.Color.FromArgb(0, 0, 0, 0)
					}
				};
				imageAttributes.SetRemapTable(map, System.Drawing.Imaging.ColorAdjustType.Bitmap);
				float num = 0.5f;
				if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
				{
					num = (float)watermarkTransparency / 10f;
				}
				float[][] array = new float[5][];
				float[][] arg_A1_0 = array;
				int arg_A1_1 = 0;
				float[] array2 = new float[5];
				array2[0] = 1f;
				arg_A1_0[arg_A1_1] = array2;
				float[][] arg_B8_0 = array;
				int arg_B8_1 = 1;
				float[] array3 = new float[5];
				array3[1] = 1f;
				arg_B8_0[arg_B8_1] = array3;
				float[][] arg_CF_0 = array;
				int arg_CF_1 = 2;
				float[] array4 = new float[5];
				array4[2] = 1f;
				arg_CF_0[arg_CF_1] = array4;
				float[][] arg_E3_0 = array;
				int arg_E3_1 = 3;
				float[] array5 = new float[5];
				array5[3] = num;
				arg_E3_0[arg_E3_1] = array5;
				array[4] = new float[]
				{
					0f,
					0f,
					0f,
					0f,
					1f
				};
				float[][] newColorMatrix = array;
				System.Drawing.Imaging.ColorMatrix newColorMatrix2 = new System.Drawing.Imaging.ColorMatrix(newColorMatrix);
				imageAttributes.SetColorMatrix(newColorMatrix2, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);
				int x = 0;
				int y = 0;
				switch (watermarkStatus)
				{
				case 1:
					x = (int)((float)img.Width * 0.01f);
					y = (int)((float)img.Height * 0.01f);
					break;
				case 2:
					x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
					y = (int)((float)img.Height * 0.01f);
					break;
				case 3:
					x = (int)((float)img.Width * 0.99f - (float)image.Width);
					y = (int)((float)img.Height * 0.01f);
					break;
				case 4:
					x = (int)((float)img.Width * 0.01f);
					y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
					break;
				case 5:
					x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
					y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
					break;
				case 6:
					x = (int)((float)img.Width * 0.99f - (float)image.Width);
					y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
					break;
				case 7:
					x = (int)((float)img.Width * 0.01f);
					y = (int)((float)img.Height * 0.99f - (float)image.Height);
					break;
				case 8:
					x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
					y = (int)((float)img.Height * 0.99f - (float)image.Height);
					break;
				case 9:
					x = (int)((float)img.Width * 0.99f - (float)image.Width);
					y = (int)((float)img.Height * 0.99f - (float)image.Height);
					break;
				}
				graphics.DrawImage(image, new System.Drawing.Rectangle(x, y, image.Width, image.Height), 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel, imageAttributes);
				System.Drawing.Imaging.ImageCodecInfo[] imageEncoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
				System.Drawing.Imaging.ImageCodecInfo imageCodecInfo = null;
				System.Drawing.Imaging.ImageCodecInfo[] array6 = imageEncoders;
				for (int i = 0; i < array6.Length; i++)
				{
					System.Drawing.Imaging.ImageCodecInfo imageCodecInfo2 = array6[i];
					if (imageCodecInfo2.MimeType.IndexOf("jpeg") > -1)
					{
						imageCodecInfo = imageCodecInfo2;
					}
				}
				System.Drawing.Imaging.EncoderParameters encoderParameters = new System.Drawing.Imaging.EncoderParameters();
				long[] array7 = new long[1];
				if (quality < 0 || quality > 100)
				{
					quality = 80;
				}
				array7[0] = (long)quality;
				System.Drawing.Imaging.EncoderParameter encoderParameter = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, array7);
				encoderParameters.Param[0] = encoderParameter;
				System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
				if (imageCodecInfo != null)
				{
					img.Save(memoryStream, imageCodecInfo, encoderParameters);
				}
				result = memoryStream;
			}
			catch
			{
				System.IO.MemoryStream memoryStream = null;
				result = memoryStream;
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (img != null)
				{
					img.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
				if (imageAttributes != null)
				{
					imageAttributes.Dispose();
				}
			}
			return result;
		}
	}
}
