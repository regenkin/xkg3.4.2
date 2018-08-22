using ControlPanel.Settings;
using Hidistro.Entities.Settings;
using Newtonsoft.Json;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class GetCustomerService : System.Web.IHttpHandler
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
			int.TryParse(context.Request["id"].ToString(), out num);
			if (num > 0)
			{
				CustomerServiceInfo customer = CustomerServiceHelper.GetCustomer(num);
				var value = new
				{
					type = "success",
					userver = customer.userver,
					password = customer.password,
					nickname = customer.nickname
				};
				string s = JsonConvert.SerializeObject(value);
				context.Response.Write(s);
				return;
			}
			var value2 = new
			{
				type = "success",
				userver = string.Empty,
				password = string.Empty,
				nickname = string.Empty
			};
			string s2 = JsonConvert.SerializeObject(value2);
			context.Response.Write(s2);
		}
	}
}
