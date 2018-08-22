using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VLoginOut : VshopTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VLogout.html";
			}
			base.OnInit(e);
		}

		private string GetResponseResult(string url)
		{
			System.Net.WebRequest webRequest = System.Net.WebRequest.Create(url);
			string result;
			using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webRequest.GetResponse())
			{
				using (System.IO.Stream responseStream = httpWebResponse.GetResponseStream())
				{
					using (System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}
			return result;
		}

		protected override void AttachChildControls()
		{
			Globals.ClearUserCookie();
			this.Page.Response.Redirect(Globals.ApplicationPath + "/Default.aspx");
		}
	}
}
