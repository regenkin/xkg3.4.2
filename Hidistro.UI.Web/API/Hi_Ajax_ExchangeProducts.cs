using ControlPanel.Promotions;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Vshop;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class Hi_Ajax_ExchangeProducts : System.Web.IHttpHandler
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
			int num = 0;
			int.TryParse(context.Request.Params["id"], out num);
			if (num > 0)
			{
				PointExChangeInfo pointExChangeInfo = PointExChangeHelper.Get(num);
				string text = context.Request.Params["sort"];
				if (string.IsNullOrWhiteSpace(text))
				{
					text = "ProductId";
				}
				string text2 = context.Request.Params["order"];
				if (string.IsNullOrWhiteSpace(text2))
				{
					text2 = "asc";
				}
				int pageNumber;
				if (!int.TryParse(context.Request.Params["page"], out pageNumber))
				{
					pageNumber = 1;
				}
				int maxNum;
				if (!int.TryParse(context.Request.Params["size"], out maxNum))
				{
					maxNum = 10;
				}
				if (pointExChangeInfo.BeginDate <= System.DateTime.Now && pointExChangeInfo.EndDate >= System.DateTime.Now)
				{
					int num2;
					System.Data.DataTable products = PointExChangeHelper.GetProducts(num, pageNumber, maxNum, out num2, text, text2);
					foreach (System.Data.DataRow dataRow in products.Rows)
					{
						if (dataRow["ProductNumber"].ToString() == "0")
						{
							int productId = 0;
							int.TryParse(dataRow["ProductId"].ToString(), out productId);
							ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), productId);
							if (product != null && product.SaleStatus == ProductSaleStatus.OnSale)
							{
								dataRow["ProductNumber"] = product.Stock.ToString();
							}
						}
						else
						{
							int productId2 = 0;
							int.TryParse(dataRow["ProductId"].ToString(), out productId2);
							int num3 = 0;
							int.TryParse(dataRow["ProductNumber"].ToString(), out num3);
							int productExchangedCount = PointExChangeHelper.GetProductExchangedCount(num, productId2);
							int num4 = (num3 - productExchangedCount >= 0) ? (num3 - productExchangedCount) : 0;
							dataRow["ProductNumber"] = num4;
						}
					}
					string s = JsonConvert.SerializeObject(products, Formatting.Indented);
					context.Response.Write(s);
				}
			}
		}
	}
}
