using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class HiControls : WebControl
	{
		private string linkURL = "";

		private int height = 0;

		public string LinkURL
		{
			get
			{
				return this.linkURL;
			}
			set
			{
				this.linkURL = value;
			}
		}

		public new int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.LinkURL))
			{
				try
				{
					WebRequest webRequest = WebRequest.Create(this.LinkURL);
					webRequest.Timeout = 100000;
					WebResponse response = webRequest.GetResponse();
					using (Stream responseStream = response.GetResponseStream())
					{
						using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("GB2312")))
						{
							writer.Write(streamReader.ReadToEnd());
						}
					}
				}
				catch
				{
				}
			}
		}
	}
}
