using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ClassShowOnDataLitl : Literal
	{
		public string Style
		{
			get
			{
				if (this.ViewState["Style"] == null)
				{
					return string.Empty;
				}
				return (string)this.ViewState["Style"];
			}
			set
			{
				this.ViewState["Style"] = value;
			}
		}

		public string Class
		{
			get
			{
				if (this.ViewState["Class"] == null)
				{
					return string.Empty;
				}
				return (string)this.ViewState["Class"];
			}
			set
			{
				this.ViewState["Class"] = value;
			}
		}

		public string DefaultText
		{
			get
			{
				if (this.ViewState["DefaultText"] == null)
				{
					return string.Empty;
				}
				return (string)this.ViewState["DefaultText"];
			}
			set
			{
				this.ViewState["DefaultText"] = value;
			}
		}

		public bool IsShowLink
		{
			get
			{
				return this.ViewState["IsShowLink"] != null && (bool)this.ViewState["IsShowLink"];
			}
			set
			{
				this.ViewState["IsShowLink"] = value;
			}
		}

		public string Link
		{
			get
			{
				if (this.ViewState["Link"] == null)
				{
					return string.Empty;
				}
				return Globals.GetSiteUrls().UrlData.FormatUrl((string)this.ViewState["Link"]) + this.LinkQuery;
			}
			set
			{
				this.ViewState["Link"] = value;
			}
		}

		public string LinkQuery
		{
			get
			{
				if (this.ViewState["LinkQuery"] == null)
				{
					return string.Empty;
				}
				return (string)this.ViewState["LinkQuery"];
			}
			set
			{
				this.ViewState["LinkQuery"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string arg_05_0 = string.Empty;
			if (string.IsNullOrEmpty(base.Text))
			{
				base.Text = string.Format("<span>{0}</span>", this.DefaultText);
			}
			else
			{
				base.Text = string.Format("<span style=\"{0}\" class=\"{1}\">{2}</span>", this.Style, this.Class, base.Text);
				if (this.IsShowLink)
				{
					base.Text = string.Format("<a href=\"{0}\">{1}</a>", this.Link, base.Text);
				}
			}
			base.Render(writer);
		}
	}
}
