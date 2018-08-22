using System;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	internal class FormFixerHtmlTextWriter : HtmlTextWriter
	{
		private string _url;

		internal FormFixerHtmlTextWriter(TextWriter writer) : base(writer)
		{
			this._url = HttpContext.Current.Request.RawUrl;
		}

		public override void WriteAttribute(string name, string value, bool encode)
		{
			if (this._url != null && string.Compare(name, "action", true) == 0)
			{
				value = this._url;
			}
			base.WriteAttribute(name, value, encode);
		}
	}
}
