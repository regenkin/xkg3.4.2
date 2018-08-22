using Hidistro.ControlPanel;
using Hidistro.Entities.Sales;
using Newtonsoft.Json.Linq;
using System;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class ExpressReturn : System.Web.IHttpHandler
	{
		private System.Web.HttpContext context;

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
				string text = context.Request["action"];
				if (string.IsNullOrEmpty(text))
				{
					context.Response.Write("参数错误");
				}
				else
				{
					this.context = context;
					string a;
					if ((a = text) != null && a == "SaveExpressData")
					{
						this.SaveExpressData();
					}
				}
			}
			catch (System.Exception ex)
			{
				context.Response.Write(ex.Message.ToString());
			}
		}

		private void SaveExpressData()
		{
			string text = this.context.Request["param"];
			if (string.IsNullOrEmpty(text))
			{
				this.context.Response.Write("{\"result\":\"false\",\"returnCode\":\"500\",\"message\":\"服务器错误\"}");
				return;
			}
			try
			{
				JObject jObject = JObject.Parse(text);
				ExpressDataInfo expressDataInfo = new ExpressDataInfo();
				expressDataInfo.ExpressNumber = jObject["lastResult"]["nu"].ToString();
				expressDataInfo.CompanyCode = jObject["lastResult"]["com"].ToString();
				expressDataInfo.DataContent = text;
				if (new ExpressDataHelper().AddExpressData(expressDataInfo))
				{
					this.context.Response.Write("{\"result\":\"true\",\"returnCode\":\"200\",\"message\":\"成功\"}");
				}
				else
				{
					this.context.Response.Write("{\"result\":\"false\",\"returnCode\":\"500\",\"message\":\"服务器错误\"}");
				}
			}
			catch (System.Exception)
			{
				this.context.Response.Write("{\"result\":\"false\",\"returnCode\":\"500\",\"message\":\"服务器错误\"}");
			}
		}
	}
}
