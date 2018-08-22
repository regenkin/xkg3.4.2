using Hidistro.Core;
using System;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(false), PersistChildren(true)]
	public class HeadContainer : Control
	{
		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write("<script language=\"javascript\" type=\"text/javascript\"> \r\n            var applicationPath = \"{0}\";\r\n        </script>", Globals.ApplicationPath);
			writer.WriteLine();
			this.RenderMetaLanguage(writer);
			this.RenderFavicon(writer);
			this.RenderMetaAuthor(writer);
			this.RenderMetaGenerator(writer);
		}

		private void RenderMetaGenerator(HtmlTextWriter writer)
		{
			writer.WriteLine("<meta name=\"GENERATOR\" content=\"普方分销 3.4kb34001\" />");
		}

		private void RenderFavicon(HtmlTextWriter writer)
		{
			string arg = Globals.FullPath(Globals.GetSiteUrls().Favicon);
			writer.WriteLine("<link rel=\"icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", arg);
			writer.WriteLine("<link rel=\"shortcut icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", arg);
		}

		private void RenderMetaAuthor(HtmlTextWriter writer)
		{
			writer.WriteLine("<meta name=\"author\" content=\"Hishop development team\" />");
		}

		private void RenderMetaLanguage(HtmlTextWriter writer)
		{
			writer.WriteLine("<meta http-equiv=\"content-language\" content=\"zh-CN\" />");
		}
	}
}
