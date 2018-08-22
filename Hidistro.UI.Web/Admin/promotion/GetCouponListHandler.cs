using Hidistro.ControlPanel.Promotions;
using Hidistro.Entities.Promotions;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class GetCouponListHandler : System.Web.IHttpHandler
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
				string[] allKeys = context.Request.Params.AllKeys;
				System.DateTime end = System.DateTime.Now;
				if (allKeys.Contains("time"))
				{
					end = System.DateTime.Parse(context.Request["time"].ToString());
				}
				System.Data.DataTable unFinishedCoupon = CouponHelper.GetUnFinishedCoupon(end, new CouponType?(CouponType.活动赠送));
				var value = new
				{
					type = "success",
					data = unFinishedCoupon
				};
				string s = JsonConvert.SerializeObject(value);
				context.Response.Write(s);
			}
			catch (System.Exception ex)
			{
				context.Response.Write("{\"type\":\"error\",\"data\":\"" + ex.Message + "\"}");
			}
		}
	}
}
