using Newtonsoft.Json.Linq;
using System;

namespace Hishop.MeiQia.Api.Util
{
	public class Common
	{
		public static string GetJsonValue(string msg, string Field)
		{
			string result = "";
			JObject jObject = JObject.Parse(msg);
			if (jObject[Field] != null)
			{
				result = jObject[Field].ToString();
			}
			return result;
		}
	}
}
