using Hidistro.UI.ControlPanel.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class StoreCardSet : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlImage idImg;

		protected StoreCardSet() : base("m05", "fxp13")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string a = base.Request.Form["action"];
			if (a == "Edit")
			{
				base.Response.ContentType = "application/json";
				string s = "{\"success\":\"false\",\"Desciption\":\"保存设置失败!\"}";
				string path = base.Server.MapPath("~/Storage/Utility/StoreCardSet.js");
				string value = base.Request.Form["SotreCardJson"];
				if (!string.IsNullOrEmpty(value))
				{
					JObject jObject = JsonConvert.DeserializeObject(value) as JObject;
					if (jObject["posList"] != null && jObject["DefaultHead"] != null && jObject["myusername"] != null && jObject["shopname"] != null)
					{
						try
						{
							jObject["writeDate"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
							System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(jObject));
							s = "{\"success\":\"true\",\"Desciption\":\"保存设置成功!\"}";
						}
						catch (System.Exception ex)
						{
							s = "{\"success\":\"false\",\"Desciption\":\"保存设置失败!" + ex.Message + "\"}";
						}
					}
				}
				base.Response.Write(s);
				base.Response.End();
			}
		}
	}
}
