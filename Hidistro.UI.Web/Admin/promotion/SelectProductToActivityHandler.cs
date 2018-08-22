using ControlPanel.Promotions;
using Hidistro.ControlPanel.Commodities;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class SelectProductToActivityHandler : System.Web.IHttpHandler
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
				int couponId = int.Parse(context.Request["id"].ToString());
				string text = context.Request["products"].ToString();
				bool flag = bool.Parse(context.Request["bsingle"].ToString());
				bool flag2 = bool.Parse(context.Request["setSale"].ToString());
				bool flag3;
				if (!flag)
				{
					bool isAllProduct = bool.Parse(context.Request["all"].ToString());
					System.Collections.Generic.IList<string> productIDs = text.Split(new char[]
					{
						'|'
					});
					flag3 = ActivityHelper.AddProducts(couponId, isAllProduct, productIDs);
					if (flag2)
					{
						ProductHelper.UpShelf(text.Replace('|', ','));
					}
				}
				else
				{
					int productID = int.Parse(text);
					flag3 = ActivityHelper.AddProducts(couponId, productID);
					if (flag2)
					{
						ProductHelper.UpShelf(productID.ToString());
					}
				}
				if (flag3)
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
