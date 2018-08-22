using Hidistro.Core;
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class Style : Literal
	{
		private const string linkFormat = "<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" media=\"{1}\" />";

		private string href;

		[DefaultValue("screen")]
		public string Media
		{
			get;
			set;
		}

		public virtual string Href
		{
			get
			{
				if (string.IsNullOrEmpty(this.href))
				{
					return null;
				}
				if (this.href.StartsWith("/"))
				{
					return Globals.ApplicationPath + this.href;
				}
				return Globals.ApplicationPath + "/" + this.href;
			}
			set
			{
				this.href = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.Href))
			{
				writer.Write("<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" media=\"{1}\" />", this.Href, this.Media);
			}
		}
	}
}
