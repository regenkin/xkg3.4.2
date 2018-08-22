using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class SKUImage : WebControl
	{
		public string ImageUrl
		{
			get;
			set;
		}

		public string ValueStr
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (string.IsNullOrEmpty(this.ImageUrl))
			{
				writer.Write(string.Format("<a href=\"javascript:void(0)\">{0}</a>", this.ValueStr));
			}
			else
			{
				writer.Write(string.Format("<a  class=\"{0}\" href=\"javascript:void(0)\"><img src=\"{1}\" width=\"23\" height=\"20\" alt=\"{2}\" /></a>", this.CssClass, Globals.ApplicationPath + this.ImageUrl, this.ValueStr));
			}
		}
	}
}
