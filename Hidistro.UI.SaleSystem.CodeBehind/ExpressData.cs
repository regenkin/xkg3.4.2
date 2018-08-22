using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Web;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ExpressData : System.Web.IHttpHandler
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
			try
			{
				this.SearchExpressData(context);
			}
			catch
			{
			}
		}

		private void SearchExpressData(System.Web.HttpContext context)
		{
			string text = "{";
			if (!string.IsNullOrEmpty(context.Request["OrderId"]))
			{
				string orderId = context.Request["OrderId"];
				OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
				if (orderInfo != null)
				{
					if ((orderInfo.OrderStatus == OrderStatus.SellerAlreadySent || orderInfo.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
					{
						string expressData = Express.GetExpressData(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber, 2);
						text = text + "\"Express\":\"" + expressData + "\"";
					}
				}
			}
			text += "}";
			context.Response.ContentType = "text/plain";
			context.Response.Write(text);
			context.Response.End();
		}
	}
}
