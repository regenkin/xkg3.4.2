using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using HiTemplate.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class Hi_Ajax_GoodsList : System.Web.IHttpHandler
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
			context.Response.Write(this.GoodGroupJson(context));
		}

		public string GoodGroupJson(System.Web.HttpContext context)
		{
			Hi_Json_GoodGourpContent hi_Json_GoodGourpContent = new Hi_Json_GoodGourpContent();
			hi_Json_GoodGourpContent.showPrice = (context.Request.Form["ShowPrice"] == null || System.Convert.ToBoolean(context.Request.Form["ShowPrice"]));
			hi_Json_GoodGourpContent.layout = ((context.Request.Form["Layout"] != null) ? System.Convert.ToInt32(context.Request.Form["Layout"]) : 1);
			hi_Json_GoodGourpContent.showName = (context.Request.Form["showName"] == null || System.Convert.ToBoolean(System.Convert.ToInt32(context.Request.Form["showName"])));
			hi_Json_GoodGourpContent.showIco = (context.Request.Form["ShowIco"] == null || System.Convert.ToBoolean(context.Request.Form["ShowIco"]));
			string text = (context.Request.Form["IDs"] != null) ? context.Request.Form["IDs"] : "";
			System.Collections.Generic.List<HiShop_Model_Good> list = new System.Collections.Generic.List<HiShop_Model_Good>();
			if (!string.IsNullOrEmpty(text))
			{
				System.Collections.Generic.IList<ProductInfo> goods = this.GetGoods(context, text);
				foreach (ProductInfo current in goods)
				{
					list.Add(new HiShop_Model_Good
					{
						item_id = current.ProductId.ToString(),
						title = current.ProductName.ToString(),
						price = System.Convert.ToDouble(current.MinShowPrice).ToString("f2"),
						original_price = System.Convert.ToDouble(current.MarketPrice).ToString("f2"),
						link = Globals.GetWebUrlStart() + "/ProductDetails.aspx?productId=" + current.ProductId.ToString(),
						pic = current.ThumbnailUrl310.ToString()
					});
				}
			}
			hi_Json_GoodGourpContent.goodslist = list;
			return JsonConvert.SerializeObject(hi_Json_GoodGourpContent);
		}

		public System.Collections.Generic.IList<ProductInfo> GetGoods(System.Web.HttpContext context, string ids)
		{
			System.Collections.Generic.List<int> productIds = (from s in ids.Split(new char[]
			{
				','
			})
			select int.Parse(s)).ToList<int>();
			return ProductHelper.GetProducts(productIds, true);
		}
	}
}
