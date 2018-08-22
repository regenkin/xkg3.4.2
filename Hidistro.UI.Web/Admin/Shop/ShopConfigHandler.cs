using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class ShopConfigHandler : System.Web.IHttpHandler
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
					bool enableSaleService = bool.Parse(context.Request["enable"].ToString());
					masterSettings.EnableSaleService = enableSaleService;
				}
				SettingsManager.Save(masterSettings);
				context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
			}
			catch (System.Exception ex)
			{
				context.Response.Write("{\"type\":\"error\",\"data\":\"" + ex.Message + "\"}");
			}
		}
	}
}
