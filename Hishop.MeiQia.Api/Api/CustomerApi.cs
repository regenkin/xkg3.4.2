using Hishop.MeiQia.Api.Util;
using System;
using System.Collections.Generic;

namespace Hishop.MeiQia.Api.Api
{
	public class CustomerApi
	{
		public static string CreateCustomer(string accessToken, IDictionary<string, string> parameters)
		{
			string url = string.Format("http://open.meiqia.com/cgi-bin/create/userver?access_token={0}", accessToken);
			return new WebUtils().DoPost(url, parameters);
		}

		public static string UpdateCustomer(string accessToken, IDictionary<string, string> parameters)
		{
			string url = string.Format("http://open.meiqia.com/cgi-bin/update/userver?access_token={0}", accessToken);
			return new WebUtils().DoPost(url, parameters);
		}

		public static string DeleteCustomer(string accessToken, string unit, string userver)
		{
			string url = string.Format("http://open.meiqia.com/cgi-bin/delete/userver?access_token={0}", accessToken);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("unit", unit);
			dictionary.Add("userver", userver);
			return new WebUtils().DoPost(url, dictionary);
		}

		public static string GetUserverId(string accessToken, string userver)
		{
			string url = string.Format("http://open.meiqia.com/cgi-bin/get/userverid?access_token={0}&userver={1}", accessToken, userver);
			string text = new WebUtils().DoGet(url, null);
			string result;
			if (text.Contains("id"))
			{
				result = Common.GetJsonValue(text, "id");
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
	}
}
