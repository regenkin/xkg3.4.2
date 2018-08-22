using ControlPanel.Promotions;
using Hidistro.ControlPanel.Promotions;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class EditProductHandler : System.Web.IHttpHandler
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
			context.Response.ContentType = "text/plain";
			try
			{
				int num = int.Parse(context.Request["actType"].ToString());
				int num2 = int.Parse(context.Request["id"].ToString());
				string productIds = context.Request["products"];
				int num3 = int.Parse(context.Request["type"].ToString());
				bool flag = false;
				if (num == 0)
				{
					if (num3 == 0)
					{
						flag = CouponHelper.SetProductsStatus(num2, 1, productIds);
					}
					else if (num3 == 1)
					{
						flag = CouponHelper.SetProductsStatus(num2, 0, productIds);
					}
					else if (num3 == 2)
					{
						flag = CouponHelper.DeleteProducts(num2, productIds);
					}
				}
				else if (num == 1)
				{
					if (num3 == 0)
					{
						flag = ActivityHelper.SetProductsStatus(num2, 1, productIds);
					}
					else if (num3 == 1)
					{
						flag = ActivityHelper.SetProductsStatus(num2, 0, productIds);
					}
					else if (num3 == 2)
					{
						flag = ActivityHelper.DeleteProducts(num2, productIds);
					}
				}
				else if (num == 2)
				{
					if (num3 == 0)
					{
						flag = PointExChangeHelper.SetProductsStatus(num2, 1, productIds);
					}
					else if (num3 == 1)
					{
						flag = PointExChangeHelper.SetProductsStatus(num2, 0, productIds);
					}
					else if (num3 == 2)
					{
						flag = PointExChangeHelper.DeleteProducts(num2, productIds);
					}
				}
				if (flag)
				{
					context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
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
