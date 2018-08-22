using Hidistro.Core;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin.Settings.flex
{
	public class UploadFile : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Web.HttpFileCollection files = base.Request.Files;
			if (files.Count > 0)
			{
				string str = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/master/flex");
				System.Web.HttpPostedFile httpPostedFile = files[0];
				string text = System.IO.Path.GetExtension(httpPostedFile.FileName).ToLower();
				if (text != ".jpg" && text != ".gif" && text != ".jpeg" && text != ".png" && text != ".bmp")
				{
					base.Response.Write("1");
					return;
				}
				string text2 = System.DateTime.Now.ToString("yyyyMMdd") + new System.Random().Next(10000, 99999).ToString(System.Globalization.CultureInfo.InvariantCulture);
				text2 += text;
				string filename = str + "/" + text2;
				try
				{
					httpPostedFile.SaveAs(filename);
					base.Response.Write(text2);
					return;
				}
				catch
				{
					base.Response.Write("0");
					return;
				}
			}
			base.Response.Write("2");
		}
	}
}
