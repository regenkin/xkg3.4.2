using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Member
{
	public class ScoreConfigHandler : System.Web.IHttpHandler
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
				bool flag = bool.Parse(context.Request["enable"].ToString());
				if (a == "0")
				{
					masterSettings.sign_score_Enable = flag;
				}
				else if (a == "1")
				{
					masterSettings.shopping_score_Enable = flag;
				}
				else if (a == "2")
				{
					masterSettings.share_score_Enable = flag;
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
