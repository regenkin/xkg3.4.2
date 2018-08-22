using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using HiTemplate.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GoodsListGroup : System.Web.IHttpHandler
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
			string arg_25_0 = context.Request.Form["id"];
			context.Response.Write(this.GoodGroupJson(context));
		}

		public string GoodGroupJson(System.Web.HttpContext context)
		{
			Hi_Json_GoodGourpContent hi_Json_GoodGourpContent = new Hi_Json_GoodGourpContent();
			hi_Json_GoodGourpContent.showPrice = (context.Request.Form["ShowPrice"] == null || System.Convert.ToBoolean(context.Request.Form["ShowPrice"]));
			hi_Json_GoodGourpContent.layout = ((context.Request.Form["Layout"] != null) ? System.Convert.ToInt32(context.Request.Form["Layout"]) : 1);
			hi_Json_GoodGourpContent.showName = (context.Request.Form["showName"] == null || System.Convert.ToBoolean(context.Request.Form["showName"]));
			hi_Json_GoodGourpContent.showIco = (context.Request.Form["ShowIco"] == null || System.Convert.ToBoolean(context.Request.Form["ShowIco"]));
			hi_Json_GoodGourpContent.goodsize = ((context.Request.Form["GoodListSize"] != null) ? System.Convert.ToInt32(context.Request.Form["GoodListSize"]) : 6);
			System.Collections.Generic.List<HiShop_Model_Good> list = new System.Collections.Generic.List<HiShop_Model_Good>();
			System.Data.DataTable goods = this.GetGoods(context);
			for (int i = 0; i < goods.Rows.Count; i++)
			{
				list.Add(new HiShop_Model_Good
				{
					item_id = goods.Rows[i]["ProductId"].ToString(),
					title = goods.Rows[i]["ProductName"].ToString(),
					price = System.Convert.ToDouble(goods.Rows[i]["MinShowPrice"]).ToString("f2"),
					original_price = System.Convert.ToDouble(goods.Rows[i]["MarketPrice"]).ToString("f2"),
					link = Globals.GetWebUrlStart() + "/ProductDetails.aspx?productId=" + goods.Rows[i]["ProductId"].ToString(),
					pic = goods.Rows[i]["ThumbnailUrl310"].ToString()
				});
			}
			hi_Json_GoodGourpContent.goodslist = list;
			return JsonConvert.SerializeObject(hi_Json_GoodGourpContent);
		}

		public System.Data.DataTable GetGoods(System.Web.HttpContext context)
		{
			int top = (context.Request.Form["GoodListSize"] != null) ? System.Convert.ToInt32(context.Request.Form["GoodListSize"]) : 6;
			ProductShowOrderPriority show = ProductShowOrderPriority.NULLORDER;
			ProductShowOrderPriority show2 = (ProductShowOrderPriority)((!string.IsNullOrEmpty(context.Request.Form["SecondPriority"])) ? System.Convert.ToInt32(context.Request.Form["SecondPriority"]) : 0);
			string text = ProductTempSQLADD.ReturnShowOrder(show);
			if (!string.IsNullOrEmpty(text))
			{
				text += ",";
			}
			if (!string.IsNullOrEmpty(ProductTempSQLADD.ReturnShowOrder(show2)))
			{
				text += ProductTempSQLADD.ReturnShowOrder(show2);
			}
			return ProductHelper.GetTopProductOrder(top, text);
		}
	}
}
