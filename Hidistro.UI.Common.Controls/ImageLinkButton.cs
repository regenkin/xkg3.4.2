using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ImageLinkButton : LinkButton
	{
		private string imageFormat = "<img border=\"0\" src=\"{0}\" alt=\"{1}\" />";

		private ImagePosition position;

		private bool isShow;

		private string deleteMsg = "确定要执行该删除操作吗？删除后将不可以恢复！";

		private string alt;

		private bool showText = true;

		public bool IsShow
		{
			get
			{
				return this.isShow;
			}
			set
			{
				this.isShow = value;
			}
		}

		public string DeleteMsg
		{
			get
			{
				return this.deleteMsg;
			}
			set
			{
				this.deleteMsg = value;
			}
		}

		public string Alt
		{
			get
			{
				return this.alt;
			}
			set
			{
				this.alt = value;
			}
		}

		public bool ShowText
		{
			get
			{
				return this.showText;
			}
			set
			{
				this.showText = value;
			}
		}

		public ImagePosition ImagePosition
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		public string ImageUrl
		{
			get
			{
				if (this.ViewState["Src"] != null)
				{
					return (string)this.ViewState["Src"];
				}
				return null;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					string text = value;
					if (text.StartsWith("~"))
					{
						text = base.ResolveUrl(text);
					}
					this.ViewState["Src"] = text;
					return;
				}
				this.ViewState["Src"] = null;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string arg_05_0 = string.Empty;
			if (this.IsShow)
			{
				string value = string.Format("return   confirm('{0}');", this.DeleteMsg);
				base.Attributes.Add("OnClick", value);
			}
			base.Attributes.Add("name", this.NamingContainer.UniqueID + "$" + this.ID);
			string imageTag = this.GetImageTag();
			if (!this.ShowText)
			{
				base.Text = "";
			}
			if (this.ImagePosition == ImagePosition.Right)
			{
				base.Text += imageTag;
			}
			else
			{
				base.Text = imageTag + base.Text;
			}
			base.Render(writer);
		}

		private string GetImageTag()
		{
			if (string.IsNullOrEmpty(this.ImageUrl))
			{
				return string.Empty;
			}
			return string.Format(CultureInfo.InvariantCulture, this.imageFormat, new object[]
			{
				this.ImageUrl,
				this.Alt
			});
		}
	}
}
