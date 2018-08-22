using System;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GetTemplateByID : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			string dataName = context.Request.QueryString["id"];
			context.Response.Write(this.GetTemplateJson(context, dataName));
		}

		public string GetTemplateJson(System.Web.HttpContext context, string dataName)
		{
			System.IO.StreamReader streamReader = new System.IO.StreamReader(context.Server.MapPath("/Templates/vshop/" + dataName + "/data/default.json"), System.Text.Encoding.UTF8);
			string result;
			try
			{
				string text = streamReader.ReadToEnd();
				streamReader.Close();
				text = text.Replace("\r\n", "").Replace("\n", "");
				result = text;
			}
			catch
			{
				result = "";
			}
			return result;
		}
	}
}
