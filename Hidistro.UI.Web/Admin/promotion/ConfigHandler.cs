using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class ConfigHandler : System.Web.IHttpHandler
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
			try
			{
				string a = context.Request["type"].ToString();
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				if (a == "0")
				{
					bool ponitToCash_Enable = bool.Parse(context.Request["enable"].ToString());
					masterSettings.PonitToCash_Enable = ponitToCash_Enable;
				}
				if (a == "1")
				{
					bool shareAct_Enable = bool.Parse(context.Request["enable"].ToString());
					masterSettings.ShareAct_Enable = shareAct_Enable;
				}
				SettingsManager.Save(masterSettings);
				context.Response.Write("保存成功");
			}
			catch (System.Exception ex)
			{
				context.Response.Write("保存失败！（" + ex.Message + ")");
			}
		}
	}
}
