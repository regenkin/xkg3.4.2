using Hidistro.Core;
using System;
using System.ComponentModel;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class UpImg : WebControl
	{
		private string uploadedImageUrl = string.Empty;

		private bool isNeedThumbnail = true;

		private string thumbnailUrl40;

		private string thumbnailUrl60;

		private string thumbnailUrl100;

		private string thumbnailUrl160;

		private string thumbnailUrl180;

		private string thumbnailUrl220;

		private string thumbnailUrl310;

		private string thumbnailUrl410;

		public UploadType UploadType
		{
			get;
			set;
		}

		public string UploadSize
		{
			get;
			set;
		}

		[Browsable(false)]
		public string UploadedImageUrl
		{
			get
			{
				return this.uploadedImageUrl;
			}
			set
			{
				if (this.CheckFileExists(value))
				{
					this.uploadedImageUrl = value;
					if (this.isNeedThumbnail)
					{
						this.thumbnailUrl40 = value.Replace("/images/", "/thumbs40/40_");
						this.thumbnailUrl60 = value.Replace("/images/", "/thumbs60/60_");
						this.thumbnailUrl100 = value.Replace("/images/", "/thumbs100/100_");
						this.thumbnailUrl160 = value.Replace("/images/", "/thumbs160/160_");
						this.thumbnailUrl180 = value.Replace("/images/", "/thumbs180/180_");
						this.thumbnailUrl220 = value.Replace("/images/", "/thumbs220/220_");
						this.thumbnailUrl310 = value.Replace("/images/", "/thumbs310/310_");
						this.thumbnailUrl410 = value.Replace("/images/", "/thumbs410/410_");
					}
				}
			}
		}

		public bool IsNeedThumbnail
		{
			get
			{
				return this.isNeedThumbnail;
			}
			set
			{
				this.isNeedThumbnail = value;
			}
		}

		[Browsable(false)]
		public string ThumbnailUrl40
		{
			get
			{
				return this.thumbnailUrl40;
			}
		}

		[Browsable(false)]
		public string ThumbnailUrl60
		{
			get
			{
				return this.thumbnailUrl60;
			}
		}

		[Browsable(false)]
		public string ThumbnailUrl100
		{
			get
			{
				return this.thumbnailUrl100;
			}
		}

		[Browsable(false)]
		public string ThumbnailUrl160
		{
			get
			{
				return this.thumbnailUrl160;
			}
		}

		[Browsable(false)]
		public string ThumbnailUrl180
		{
			get
			{
				return this.thumbnailUrl180;
			}
		}

		[Browsable(false)]
		public string ThumbnailUrl220
		{
			get
			{
				return this.thumbnailUrl220;
			}
		}

		[Browsable(false)]
		public string ThumbnailUrl310
		{
			get
			{
				return this.thumbnailUrl310;
			}
		}

		[Browsable(false)]
		public string ThumbnailUrl410
		{
			get
			{
				return this.thumbnailUrl410;
			}
		}

		public bool IsUploaded
		{
			get
			{
				return !string.IsNullOrEmpty(this.UploadedImageUrl);
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

		public UpImg()
		{
			this.UploadType = UploadType.Product;
			this.UploadSize = "";
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

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.UploadedImageUrl = this.Context.Request.Form[this.ID + "_uploadedImageUrl"];
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			string webResourceUrl = this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.ImageUploader.images.upload.png");
			WebControl webControl = new WebControl(HtmlTextWriterTag.Div);
			string value = "";
			webControl.Attributes.Add("id", this.ID + "_preview");
			webControl.Attributes.Add("style", value);
			webControl.Attributes.Add("class", "preview");
			WebControl webControl2 = new WebControl(HtmlTextWriterTag.Div);
			webControl2.Attributes.Add("id", this.ID + "_upload");
			webControl2.Attributes.Add("class", "actionBox");
			if (Globals.GetCurrentManagerUserId() == 0 || this.IsUploaded)
			{
				webControl2.Attributes.Add("style", "display:none;");
			}
			WebControl webControl3 = new WebControl(HtmlTextWriterTag.A);
			webControl3.Attributes.Add("href", "javascript:void(0);");
			webControl3.Attributes.Add("style", "background-image: url(" + webResourceUrl + ");");
			webControl3.Attributes.Add("class", "files");
			webControl3.Attributes.Add("id", this.ID + "_content");
			webControl2.Controls.Add(webControl3);
			WebControl webControl4 = new WebControl(HtmlTextWriterTag.Div);
			WebControl webControl5 = new WebControl(HtmlTextWriterTag.A);
			webControl4.Attributes.Add("id", this.ID + "_delete");
			webControl4.Attributes.Add("class", "actionBox");
			if (Globals.GetCurrentManagerUserId() == 0 || !this.IsUploaded)
			{
				webControl4.Attributes.Add("style", "display:none;");
			}
			webControl5.Attributes.Add("href", string.Concat(new string[]
			{
				"javascript:DeleteImage('",
				this.ID,
				"','",
				this.UploadType.ToString().ToLower(),
				"',",
				this.isNeedThumbnail ? "1" : "0",
				");"
			}));
			webControl5.Attributes.Add("style", "background-image: url(" + webResourceUrl + ");");
			webControl5.Attributes.Add("class", "actions");
			webControl4.Controls.Add(webControl5);
			this.Controls.Add(webControl);
			this.Controls.Add(webControl2);
			this.Controls.Add(webControl4);
			if (this.Page.Header.FindControl("uploaderStyle") == null)
			{
				WebControl webControl6 = new WebControl(HtmlTextWriterTag.Link);
				webControl6.Attributes.Add("rel", "stylesheet");
				webControl6.Attributes.Add("href", this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.ImageUploader.css.style.css"));
				webControl6.Attributes.Add("type", "text/css");
				webControl6.Attributes.Add("media", "screen");
				webControl6.ID = "uploaderStyle";
				this.Page.Header.Controls.Add(webControl6);
			}
		}

		public override void RenderControl(HtmlTextWriter writer)
		{
			foreach (Control control in this.Controls)
			{
				control.RenderControl(writer);
				writer.WriteLine();
			}
			if (!this.Page.ClientScript.IsStartupScriptRegistered(base.GetType(), "UploadScript"))
			{
				string script = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.ImageUploader.script.upimg.js"));
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "UploadScript", script, false);
			}
			if (!this.Page.ClientScript.IsStartupScriptRegistered(base.GetType(), this.ID + "_InitScript"))
			{
				string text = string.Concat(new string[]
				{
					"$(document).ready(function() { InitUploader(\"",
					this.ID,
					"\", \"",
					this.UploadType.ToString().ToLower(),
					"\",",
					this.isNeedThumbnail ? "1" : "0",
					",'",
					this.UploadSize,
					"');"
				});
				if (this.IsUploaded)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"UpdatePreview('",
						this.ID,
						"', '",
						this.uploadedImageUrl,
						"');"
					});
				}
				text = text + "});" + Environment.NewLine;
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), this.ID + "_InitScript", text, true);
			}
			writer.WriteLine();
			writer.AddAttribute("id", this.ID + "_uploadedImageUrl");
			writer.AddAttribute("name", this.ID + "_uploadedImageUrl");
			writer.AddAttribute("value", this.UploadedImageUrl);
			writer.AddAttribute("type", "hidden");
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
		}
	}
}
