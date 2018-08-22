using Hidistro.Core;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class ImgUpload : System.Web.UI.Page
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
			int num = int.Parse(base.Request.QueryString["imgurl"]);
			string text = base.Request.QueryString["oldurl"].ToString();
			try
			{
				if (num < 1)
				{
					System.Web.HttpPostedFile httpPostedFile = base.Request.Files["Filedata"];
					string str = System.DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					string text2 = "/Storage/master/topic/";
					if (!System.IO.Directory.Exists(base.Server.MapPath(text2)))
					{
						System.IO.Directory.CreateDirectory(base.Server.MapPath(text2));
					}
					string text3 = str + System.IO.Path.GetExtension(httpPostedFile.FileName);
					httpPostedFile.SaveAs(Globals.MapPath(text2 + text3));
					if (!string.IsNullOrEmpty(text))
					{
						string path2 = base.Server.MapPath(text);
						if (System.IO.File.Exists(path2))
						{
							System.IO.File.Delete(path2);
						}
					}
					base.Response.StatusCode = 200;
					base.Response.Write(str + "|/Storage/master/topic/" + text3);
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
				base.Response.End();
			}
		}
	}
}
