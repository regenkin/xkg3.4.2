using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	[ToolboxData("<{0}:ListRadioButton runat=server></{0}:ListRadioButton>")]
	public class ListRadioButton : RadioButton, IPostBackDataHandler
	{
		private string Value
		{
			get
			{
				string text = base.Attributes["value"];
				if (text == null)
				{
					text = this.UniqueID;
				}
				return text;
			}
		}

		protected override void Render(HtmlTextWriter output)
		{
			this.RenderInputTag(output);
		}

		private void RenderInputTag(HtmlTextWriter htw)
		{
			htw.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
			htw.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
			htw.AddAttribute(HtmlTextWriterAttribute.Name, this.GroupName);
			htw.AddAttribute(HtmlTextWriterAttribute.Value, this.Value);
			if (this.Checked)
			{
				htw.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
			}
			if (!this.Enabled)
			{
				htw.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			string text = base.Attributes["onclick"];
			if (this.AutoPostBack)
			{
				if (text != null)
				{
					text = string.Empty;
				}
				text += this.Page.ClientScript.GetPostBackEventReference(this, string.Empty);
				htw.AddAttribute(HtmlTextWriterAttribute.Onclick, text);
				htw.AddAttribute("language", "javascript");
			}
			else if (text != null)
			{
				htw.AddAttribute(HtmlTextWriterAttribute.Onclick, text);
			}
			if (this.AccessKey.Length > 0)
			{
				htw.AddAttribute(HtmlTextWriterAttribute.Accesskey, this.AccessKey);
			}
			if (this.TabIndex != 0)
			{
				htw.AddAttribute(HtmlTextWriterAttribute.Tabindex, this.TabIndex.ToString(NumberFormatInfo.InvariantInfo));
			}
			htw.RenderBeginTag(HtmlTextWriterTag.Input);
			htw.RenderEndTag();
		}

		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.OnCheckedChanged(EventArgs.Empty);
		}

		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			bool result = false;
			string text = postCollection[this.GroupName];
			if (text != null && text == this.Value)
			{
				if (!this.Checked)
				{
					this.Checked = true;
					result = true;
				}
			}
			else if (this.Checked)
			{
				this.Checked = false;
			}
			return result;
		}
	}
}
