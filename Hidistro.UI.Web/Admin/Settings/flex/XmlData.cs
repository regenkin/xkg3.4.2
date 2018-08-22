using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin.Settings.flex
{
	public class XmlData : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["xmlname"];
			string s = base.Request.Form["xmldata"];
			string text2 = base.Request.Form["expressname"];
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2) && !SalesHelper.IsExistExpress(text2) && SalesHelper.AddExpressTemplate(text2, text + ".xml"))
			{
				string path = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}.xml", text));
				System.IO.FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create);
				byte[] bytes = new System.Text.UTF8Encoding().GetBytes(s);
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Flush();
				fileStream.Close();
			}
		}
	}
}
