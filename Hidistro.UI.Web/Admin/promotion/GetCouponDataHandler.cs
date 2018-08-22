using Hidistro.ControlPanel.Promotions;
using Hidistro.Entities.Promotions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class GetCouponDataHandler : System.Web.IHttpHandler
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
				CouponInfo coupon = CouponHelper.GetCoupon(couponId);
				var value = new
				{
					type = "success",
					time = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					data = coupon
				};
				string s = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonConverter[]
				{
					new IsoDateTimeConverter
					{
						DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
					}
				});
				context.Response.Write(s);
			}
			catch (System.Exception ex)
			{
				context.Response.Write("{\"type\":\"error\",\"data\":\"" + ex.Message + "\"}");
			}
		}
	}
}
