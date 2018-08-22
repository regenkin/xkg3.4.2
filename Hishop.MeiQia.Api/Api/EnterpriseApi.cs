using Hishop.MeiQia.Api.Util;
using System;
using System.Collections.Generic;

namespace Hishop.MeiQia.Api.Api
{
	public class EnterpriseApi
	{
		public static string CreateEnterprise(string accessToken, IDictionary<string, string> parameters)
		{
			string url = string.Format("http://open.meiqia.com/cgi-bin/create/unit?access_token={0}", accessToken);
			return new WebUtils().DoPost(url, parameters);
		}

		public static string UpdateEnterprise(string accessToken, IDictionary<string, string> parameters)
		{
			string url = string.Format("http://open.meiqia.com/cgi-bin/update/unit?access_token={0}", accessToken);
			return new WebUtils().DoPost(url, parameters);
		}

		public static string ActivateEnterprise(string accessToken, string unit)
		{
			string url = string.Format("http://open.meiqia.com/cgi-bin/activate/userver?access_token={0}", accessToken);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("unit", unit);
			return new WebUtils().DoPost(url, dictionary);
		}

		public static string GetUnitId(string accessToken, string unit)
		{
			string url = string.Format("http://open.meiqia.com/cgi-bin/get/unitid?access_token={0}&unit={1}", accessToken, unit);
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
