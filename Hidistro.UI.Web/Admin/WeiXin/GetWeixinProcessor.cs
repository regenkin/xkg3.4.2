using Hidistro.ControlPanel.Members;
using Newtonsoft.Json;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	public class GetWeixinProcessor : System.Web.IHttpHandler
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
			string text = context.Request["action"];
			string a;
			if ((a = text.ToLower()) != null)
			{
				if (!(a == "getcanchangebind"))
				{
					return;
				}
				this.GetCanChangeBind(context);
			}
		}

		private void GetCanChangeBind(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			try
			{
				int status = MemberHelper.CanChangeBindWeixin();
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status
				}));
			}
			catch (System.Exception)
			{
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status = 4,
					msg = "程序出错了"
				}));
			}
		}
	}
}
