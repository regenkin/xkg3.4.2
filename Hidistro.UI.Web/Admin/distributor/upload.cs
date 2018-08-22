using Hidistro.Core;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.distributor
{
	public class upload : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (base.Request.QueryString["delimg"] != null)
			{
				string path = base.Server.HtmlEncode(base.Request.QueryString["delimg"]);
				path = base.Server.MapPath(path);
				if (System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);
				}
				base.Response.Write("0");
				base.Response.End();
			}
			System.Drawing.Image image = null;
			System.Drawing.Image image2 = null;
			System.Drawing.Bitmap bitmap = null;
			System.Drawing.Graphics graphics = null;
			System.IO.MemoryStream memoryStream = null;
			int num = int.Parse(base.Request.QueryString["imgurl"]);
			try
			{
				if (num < 9)
				{
					System.Web.HttpPostedFile httpPostedFile = base.Request.Files["Filedata"];
					string str = System.DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					string str2 = "/Storage/data/FriendExtension/";
					string text = str + System.IO.Path.GetExtension(httpPostedFile.FileName);
					httpPostedFile.SaveAs(Globals.MapPath(str2 + text));
					base.Response.StatusCode = 200;
					base.Response.Write(str + "|/Storage/data/FriendExtension/" + text);
				}
				else
				{
					base.Response.Write("0");
				}
			}
			catch (System.Exception)
			{
				base.Response.StatusCode = 500;
				base.Response.Write("服务器错误");
				base.Response.End();
			}
			finally
			{
				if (bitmap != null)
				{
					bitmap.Dispose();
				}
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (image2 != null)
				{
					image2.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
				base.Response.End();
			}
		}
	}
}
