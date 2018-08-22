using Hidistro.Core;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ProductFlashUpload : WebControl
	{
		private string _FlashUploadType = "product";

		private string _Value = string.Empty;

		private string _OldValue = string.Empty;

		private int _MaxNum = 5;

		public string FlashUploadType
		{
			get
			{
				return this._FlashUploadType;
			}
			set
			{
				this._FlashUploadType = value;
			}
		}

		public string Value
		{
			get
			{
				return this.UpdatePhotoList(this.Context.Request.Form[this.ID + "_hdPhotoList"]);
			}
			set
			{
				string text = this.FilterUrlStart(value);
				this._Value = text;
				this._OldValue = text;
			}
		}

		[Browsable(false)]
		public string OldValue
		{
			get
			{
				return this.Context.Request.Form[this.ID + "_hdPhotoListOriginal"];
			}
			set
			{
				this._OldValue = value;
			}
		}

		public override ControlCollection Controls
		{
			get
			{
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		public int MaxNum
		{
			get
			{
				return this._MaxNum;
			}
			set
			{
				this._MaxNum = value;
			}
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			WebControl webControl = new WebControl(HtmlTextWriterTag.Input);
			webControl.Attributes.Add("id", this.ID.ToString() + "_hdPhotoListOriginal");
			webControl.Attributes.Add("name", this.ID.ToString() + "_hdPhotoListOriginal");
			webControl.Attributes.Add("type", "hidden");
			webControl.Attributes.Add("value", this._Value);
			WebControl webControl2 = new WebControl(HtmlTextWriterTag.Input);
			webControl2.Attributes.Add("id", this.ID + "_hdPhotoList");
			webControl2.Attributes.Add("name", this.ID + "_hdPhotoList");
			webControl2.Attributes.Add("type", "hidden");
			webControl2.Attributes.Add("value", this._Value);
			WebControl webControl3 = new WebControl(HtmlTextWriterTag.Div);
			webControl3.Attributes.Add("id", this.ID + "_divFileProgressContainer");
			webControl3.Attributes.Add("style", "height: 75px;display:none;");
			Literal literal = new Literal();
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = this._Value.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string img = array2[i];
				stringBuilder.Append(this.GetOneImgHtml(img));
			}
			literal.Text = stringBuilder.ToString();
			Literal literal2 = new Literal();
			literal2.Text = string.Concat(new string[]
			{
				"<div id=\"",
				this.ID,
				"_divImgList\"><div class=\"picfirst\"></div>",
				stringBuilder.ToString(),
				"</div>"
			});
			WebControl webControl4 = new WebControl(HtmlTextWriterTag.Div);
			webControl4.Attributes.Add("id", this.ID + "_divFlashUploadHolder");
			webControl4.Attributes.Add("style", "width: 91px; margin: 0px 10px;float: left;");
			this.Controls.Add(webControl);
			this.Controls.Add(webControl2);
			this.Controls.Add(webControl3);
			this.Controls.Add(literal2);
			this.Controls.Add(webControl4);
			int num = array.Length;
			if (string.IsNullOrEmpty(this._Value))
			{
				num = 0;
			}
			else if (this._MaxNum <= num)
			{
				webControl4.Style.Add("display", "none");
			}
			Literal literal3 = new Literal();
			literal3.Text = string.Concat(new object[]
			{
				"<script type=\"text/javascript\">var obj",
				this.ID,
				"_hdPhotoList = new FlashUploadObject(\"",
				this.ID,
				"_hdPhotoList\", \"",
				this.ID,
				"_divImgList\", \"",
				this.ID,
				"_divFlashUploadHolder\", \"picfirst\", \"",
				this.ID,
				"_divFileProgressContainer\", ",
				this.MaxNum,
				",",
				num,
				");obj",
				this.ID,
				"_hdPhotoList.upfilebuttonload();obj",
				this.ID,
				"_hdPhotoList.GetPhotoValue();</script>"
			});
			this.Controls.Add(literal3);
		}

		public override void RenderControl(HtmlTextWriter writer)
		{
			foreach (Control control in this.Controls)
			{
				control.RenderControl(writer);
				writer.WriteLine();
			}
			writer.WriteLine();
		}

		private string GetOneImgHtml(string img)
		{
			string result = string.Empty;
			if (img.Length > 10)
			{
				result = "<div class=\"uploadimages\"><div class=\"preview\"><div class=\"divoperator\"><a href=\"javascript:;\" class=\"leftmove\" title=\"左移\">&lt;</a><a href=\"javascript:;\" class=\"rightmove\" title=\"右移\">&gt;</a><a href=\"javascript:;\" class=\"photodel\" title=\"删除\">X</a></div><img style=\"width: 85px; height: 85px;\" src=\"" + img + "\"></div><div class=\"actionBox\"><a href=\"javascript:;\" class=\"actions\">设为默认</a></div></div>";
			}
			return result;
		}

		private string UpdatePhotoList(string photolist)
		{
			string[] array = this.FilterUrlStart(photolist).Trim().Trim(new char[]
			{
				','
			}).Split(new char[]
			{
				','
			});
			StringBuilder stringBuilder = new StringBuilder();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				stringBuilder.Append(this.UpdateOnePhoto(text.Trim()));
			}
			string text2 = this.Context.Server.UrlDecode(stringBuilder.ToString()).Trim(new char[]
			{
				','
			});
			string text3 = this.OldValue;
			if (!string.IsNullOrEmpty(text3))
			{
				text3 = this.Context.Server.UrlDecode(text3);
			}
			this.DeleteNoUsePhotos(this.OldValue, text2);
			return text2;
		}

		private string UpdateOnePhoto(string photo)
		{
			string text = photo;
			if (text.Contains("/temp/") && text.Length > 10)
			{
				string[] array = photo.Split(new char[]
				{
					'.'
				});
				if (array.Length <= 1)
				{
					return "";
				}
				string str = array[array.Length - 1].Trim().ToLower();
				string str2 = Globals.GetStoragePath() + "/" + this._FlashUploadType;
				string str3 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "." + str;
				string text2 = text;
				string[] array2 = photo.Split(new char[]
				{
					'/'
				});
				if (text2.StartsWith("http://"))
				{
					text2 = text2.Replace("http://" + array2[2], "");
				}
				text2 = this.Context.Server.MapPath(text2);
				string text3 = str2 + "/images/" + str3;
				File.Copy(text2, this.Context.Server.MapPath(text3), true);
				string sourceFilename = this.Context.Request.MapPath(Globals.ApplicationPath + text3);
				string str4 = str2 + "/thumbs40/40_" + str3;
				string str5 = str2 + "/thumbs60/60_" + str3;
				string str6 = str2 + "/thumbs100/100_" + str3;
				string str7 = str2 + "/thumbs160/160_" + str3;
				string str8 = str2 + "/thumbs180/180_" + str3;
				string str9 = str2 + "/thumbs220/220_" + str3;
				string str10 = str2 + "/thumbs310/310_" + str3;
				string str11 = str2 + "/thumbs410/410_" + str3;
				text = text3;
				ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str4), 40, 40);
				ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str5), 60, 60);
				ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str6), 100, 100);
				ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str7), 160, 160);
				ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str8), 180, 180);
				ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str9), 220, 220);
				ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str10), 310, 310);
				ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str11), 410, 410);
			}
			if (text.Length > 10)
			{
				text = "," + text;
			}
			return text;
		}

		private string FilterUrlStart(string photolist)
		{
			string result = string.Empty;
			if (!string.IsNullOrEmpty(photolist))
			{
				string[] array = photolist.Trim().Trim(new char[]
				{
					','
				}).Split(new char[]
				{
					','
				});
				StringBuilder stringBuilder = new StringBuilder();
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					if (text.StartsWith("http://") && !text.StartsWith("http://images.net.92hidc.com"))
					{
						string[] array3 = text.Split(new char[]
						{
							'/'
						});
						stringBuilder.Append(text.Replace("http://" + array3[2], "").Trim() + ",");
					}
					else
					{
						stringBuilder.Append(text + ",");
					}
				}
				result = stringBuilder.ToString().Trim(new char[]
				{
					','
				});
			}
			return result;
		}

		private void DeleteNoUsePhotos(string originalphotolist, string nowphotolist)
		{
			if (string.IsNullOrEmpty(originalphotolist))
			{
				return;
			}
			string[] array = originalphotolist.Split(new char[]
			{
				','
			});
			string text = "," + nowphotolist + ",";
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				if (!text.Contains("," + text2 + ",") && text2.StartsWith("/Storage/"))
				{
					this.DoDelete(this.Context.Server.MapPath(text2));
				}
			}
		}

		private void DoDelete(string originalPath)
		{
			if ((this._FlashUploadType == "product" || this._FlashUploadType.Equals("gift")) && this.CheckFileFormatOrPath(originalPath))
			{
				string path = originalPath.Replace("\\images\\", "\\thumbs40\\40_");
				string path2 = originalPath.Replace("\\images\\", "\\thumbs60\\60_");
				string path3 = originalPath.Replace("\\images\\", "\\thumbs100\\100_");
				string path4 = originalPath.Replace("\\images\\", "\\thumbs160\\160_");
				string path5 = originalPath.Replace("\\images\\", "\\thumbs180\\180_");
				string path6 = originalPath.Replace("\\images\\", "\\thumbs220\\220_");
				string path7 = originalPath.Replace("\\images\\", "\\thumbs310\\310_");
				string path8 = originalPath.Replace("\\images\\", "\\thumbs410\\410_");
				if (File.Exists(originalPath))
				{
					File.Delete(originalPath);
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
			}
		}

		private bool CheckFileExists(string imageUrl)
		{
			return this.CheckFileFormat(imageUrl) && (imageUrl.ToLower().IndexOf("http://") >= 0 || File.Exists(this.Page.Request.MapPath(Globals.ApplicationPath + imageUrl)));
		}

		private bool CheckFileFormat(string imageUrl)
		{
			if (!string.IsNullOrEmpty(imageUrl))
			{
				string text = imageUrl.ToUpper();
				if (text.Contains(".JPG") || text.Contains(".GIF") || text.Contains(".PNG") || text.Contains(".BMP") || text.Contains(".JPEG"))
				{
					return true;
				}
			}
			return false;
		}

		private bool CheckFileFormatOrPath(string imageUrl)
		{
			if (!string.IsNullOrEmpty(imageUrl))
			{
				string text = imageUrl.ToUpper();
				if ((text.Contains(".JPG") || text.Contains(".GIF") || text.Contains(".PNG") || text.Contains(".BMP") || text.Contains(".JPEG")) && (imageUrl.Contains(this.Context.Server.MapPath(Globals.GetStoragePath() + "/" + this._FlashUploadType)) || imageUrl.Contains(this.Context.Server.MapPath("/utility/pics/none.gif"))))
				{
					return true;
				}
			}
			return false;
		}
	}
}
