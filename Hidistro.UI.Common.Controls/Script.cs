using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class Script : Literal
	{
		private const string srcFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";

		private string src;

		public virtual string Src
		{
			get
			{
				if (string.IsNullOrEmpty(this.src))
				{
					return null;
				}
				if (this.src.StartsWith("/"))
				{
					return Globals.ApplicationPath + this.src;
				}
				return Globals.ApplicationPath + "/" + this.src;
			}
			set
			{
				this.src = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.Src))
			{
				writer.Write("<script src=\"{0}\" type=\"text/javascript\"></script>", this.Src);
			}
		}
	}
}
