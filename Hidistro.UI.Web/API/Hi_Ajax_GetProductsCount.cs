using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class Hi_Ajax_GetProductsCount : System.Web.IHttpHandler
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
			context.Response.Write(this.GetCountJson());
		}

		public string GetCountJson()
		{
			string text = "";
			string text2 = "";
			System.Web.HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
			if (httpCookie == null || httpCookie.Value == "0")
			{
				text = SettingsManager.GetMasterSettings(true).SiteName;
				text2 = SettingsManager.GetMasterSettings(true).DistributorLogoPic;
				return string.Concat(new object[]
				{
					"{\"count\":",
					ProductHelper.GetProductsCount(),
					",\"storeName\":\"",
					text,
					"\",\"logoUrl\":\"\"}"
				});
			}
			DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(System.Convert.ToInt32(httpCookie.Value), true);
			if (currentDistributors != null)
			{
				text = currentDistributors.StoreName;
				text2 = currentDistributors.Logo;
			}
			return string.Concat(new object[]
			{
				"{\"count\":",
				ProductHelper.GetProductsCount(),
				",\"storeName\":\"",
				text,
				"\",\"logoUrl\":\"",
				text2,
				"\"}"
			});
		}
	}
}
