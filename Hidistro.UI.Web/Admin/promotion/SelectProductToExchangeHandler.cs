using ControlPanel.Promotions;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class SelectProductToExchangeHandler : System.Web.IHttpHandler
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
				int exchangeId = int.Parse(context.Request["id"].ToString());
				string productIds = context.Request["products"].ToString();
				string pNumbers = context.Request["pNumbers"];
				string points = context.Request["points"];
				string eachNumbers = context.Request["eachNumbers"];
				bool flag = PointExChangeHelper.AddProducts(exchangeId, productIds, pNumbers, points, eachNumbers);
				if (flag)
				{
					context.Response.Write("{\"type\":\"success\",\"data\":\"添加商品成功\"}");
				}
				else
				{
					context.Response.Write("{\"type\":\"success\",\"data\":\"写数据库失败\"}");
				}
			}
			catch (System.Exception ex)
			{
				context.Response.Write("{\"type\":\"error\",\"data\":\"" + ex.Message + "\"}");
			}
		}
	}
}
