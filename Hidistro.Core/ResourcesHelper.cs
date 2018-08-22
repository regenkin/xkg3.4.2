using Hidistro.Core.Enums;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.Core
{
	public static class ResourcesHelper
	{
		private static FileClass[] imageFileClass = new FileClass[]
		{
			FileClass.jpg,
			FileClass.gif,
			FileClass.bmp,
			FileClass.png
		};

		private static FileClass[] accessoryFileClass = new FileClass[]
		{
			FileClass.rar,
			FileClass.txt,
			FileClass.doc,
			FileClass.doc,
			FileClass.doc,
			FileClass.htm,
			FileClass.html,
			FileClass.zip
		};

		private static FileClass[] mediaFileClass = new FileClass[]
		{
			FileClass.wmv,
			FileClass.mid,
			FileClass.mp3,
			FileClass.mpg,
			FileClass.rmvb,
			FileClass.xv
		};

		private static FileClass[] flasFileClass = new FileClass[]
		{
			FileClass.swf,
			FileClass.f4v
		};

		public static void CreateThumbnail(string sourceFilename, string destFilename, int width, int height)
		{
			Image image = Image.FromFile(sourceFilename);
			if (image.Width <= width && image.Height <= height)
			{
				File.Copy(sourceFilename, destFilename, true);
				image.Dispose();
			}
			else
			{
				int width2 = image.Width;
				int height2 = image.Height;
				float num = (float)height / (float)height2;
				if ((float)width / (float)width2 < num)
				{
					num = (float)width / (float)width2;
				}
				width = (int)((float)width2 * num);
				height = (int)((float)height2 * num);
				Image image2 = new Bitmap(width, height);
				Graphics graphics = Graphics.FromImage(image2);
				graphics.Clear(Color.White);
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, width2, height2), GraphicsUnit.Pixel);
				EncoderParameters encoderParameters = new EncoderParameters();
				EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, 100L);
				encoderParameters.Param[0] = encoderParameter;
				ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo encoder = null;
				for (int i = 0; i < imageEncoders.Length; i++)
				{
					if (imageEncoders[i].FormatDescription.Equals("JPEG"))
					{
						encoder = imageEncoders[i];
						break;
					}
				}
				image2.Save(destFilename, encoder, encoderParameters);
				encoderParameters.Dispose();
				encoderParameter.Dispose();
				image.Dispose();
				image2.Dispose();
				graphics.Dispose();
			}
		}

		public static bool CheckPostedFile(HttpPostedFile postedFile, string dir = "image")
		{
			bool result;
			if (postedFile == null || postedFile.ContentLength == 0)
			{
				result = false;
			}
			else
			{
				int num = 0;
				int.TryParse(ResourcesHelper.GetFileClassCode(postedFile), out num);
				FileClass[] array = ResourcesHelper.imageFileClass;
				if (dir != null)
				{
					if (!(dir == "image"))
					{
						if (!(dir == "file"))
						{
							if (!(dir == "media"))
							{
								if (dir == "flash")
								{
									array = ResourcesHelper.flasFileClass;
								}
							}
							else
							{
								array = ResourcesHelper.mediaFileClass;
							}
						}
						else
						{
							array = ResourcesHelper.accessoryFileClass;
						}
					}
					else
					{
						array = ResourcesHelper.imageFileClass;
					}
				}
				FileClass[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					FileClass fileClass = array2[i];
					if (fileClass == (FileClass)num)
					{
						result = true;
						return result;
					}
				}
				result = false;
			}
			return result;
		}

		public static string GetFileClassCode(HttpPostedFile postedFile)
		{
			int contentLength = postedFile.ContentLength;
			byte[] buffer = new byte[contentLength];
			postedFile.InputStream.Read(buffer, 0, contentLength);
			MemoryStream memoryStream = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			string text = "";
			try
			{
				text = binaryReader.ReadByte().ToString();
				text += binaryReader.ReadByte().ToString();
			}
			catch
			{
			}
			binaryReader.Close();
			memoryStream.Close();
			return text;
		}

		public static string GetFileClassCode(string FileUrl)
		{
			string text = "";
			string result;
			try
			{
				FileStream fileStream = new FileStream(FileUrl, FileMode.Open, FileAccess.Read);
				BinaryReader binaryReader = new BinaryReader(fileStream);
				text = binaryReader.ReadByte().ToString();
				text += binaryReader.ReadByte().ToString();
				binaryReader.Close();
				fileStream.Close();
			}
			catch
			{
				result = "";
				return result;
			}
			result = text;
			return result;
		}

		public static string GenerateFilename(string extension)
		{
			return Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + extension;
		}

		public static void DeleteImage(string imageUrl)
		{
			if (!string.IsNullOrEmpty(imageUrl))
			{
				try
				{
					string path = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + imageUrl);
					if (File.Exists(path))
					{
						File.Delete(path);
					}
				}
				catch
				{
				}
			}
		}
	}
}
