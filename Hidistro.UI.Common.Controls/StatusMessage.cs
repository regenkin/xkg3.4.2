using Hidistro.Core;
using System;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	public class StatusMessage : LiteralControl
	{
		private bool success = true;

		private bool isWarning;

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

		public bool IsWarning
		{
			get
			{
				return this.isWarning;
			}
			set
			{
				this.isWarning = value;
			}
		}

		public StatusMessage()
		{
			this.Visible = false;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!this.Visible)
			{
				return;
			}
			if (!this.isWarning)
			{
				if (this.success)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "CommonMessageSuccess");
				}
				else
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "CommonMessageError");
				}
			}
			else if (this.success)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "CommonMessageSuccess");
			}
			else
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "CommonWarningMessage");
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "CommonMessageSuccess");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding-right: 8px;");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding-right: 8px;");
			if (this.success)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, Globals.ApplicationPath + "/utility/pics/status-green.gif");
			}
			else if (this.isWarning)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, Globals.ApplicationPath + "/utility/pics/status-yellow.gif");
			}
			else
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, Globals.ApplicationPath + "/utility/pics/status-red.gif");
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write(this.Text);
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
		}
	}
}
