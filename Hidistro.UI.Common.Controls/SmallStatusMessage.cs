using Hidistro.Core;
using System;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	public class SmallStatusMessage : LiteralControl
	{
		private bool success = true;

		private string width;

		public bool Success
		{
			get
			{
				return this.success;
			}
			set
			{
				this.success = value;
			}
		}

		public string Width
		{
			get
			{
				if (string.IsNullOrEmpty(this.width))
				{
					return "100%";
				}
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		public SmallStatusMessage()
		{
			this.Visible = false;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!this.Visible)
			{
				return;
			}
			if (this.success)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "MessageSuccess");
			}
			else
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "MessageError");
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, this.Width);
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding-right: 3px;");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding-right: 3px;");
			if (this.success)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, Globals.GetVshopSkinPath(null) + "/images/success.gif");
			}
			else
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, Globals.GetVshopSkinPath(null) + "/images/warning.gif");
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("<nobr>" + this.Text + "<nobr/>");
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
		}
	}
}
