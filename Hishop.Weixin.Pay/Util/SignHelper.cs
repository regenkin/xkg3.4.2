using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Hishop.Weixin.Pay.Util
{
	internal class SignHelper
	{
		public static string BuildQuery(IDictionary<string, string> dict, bool encode)
		{
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(dict);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			IEnumerator<KeyValuePair<string, string>> enumerator = sortedDictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> current = enumerator.Current;
				string key = current.Key;
				current = enumerator.Current;
				string value = current.Value;
				if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
				{
					if (flag)
					{
						stringBuilder.Append("&");
					}
					stringBuilder.Append(key);
					stringBuilder.Append("=");
					if (encode)
					{
						stringBuilder.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
					}
					else
					{
						stringBuilder.Append(value);
					}
					flag = true;
				}
			}
			return stringBuilder.ToString();
		}

		public static string BuildXml(IDictionary<string, string> dict, bool encode)
		{
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(dict);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<xml>");
			IEnumerator<KeyValuePair<string, string>> enumerator = sortedDictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> current = enumerator.Current;
				string key = current.Key;
				current = enumerator.Current;
				string value = current.Value;
				if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
				{
					decimal num = 0m;
					bool flag = false;
					if (!decimal.TryParse(value, out num))
					{
						flag = true;
					}
					if (encode)
					{
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							"<",
							key,
							">",
							flag ? "<![CDATA[" : "",
							HttpUtility.UrlEncode(value, Encoding.UTF8),
							flag ? "]]>" : "",
							"</",
							key,
							">"
						}));
					}
					else
					{
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							"<",
							key,
							">",
							flag ? "<![CDATA[" : "",
							value,
							flag ? "]]>" : "",
							"</",
							key,
							">"
						}));
					}
				}
			}
			stringBuilder.AppendLine("</xml>");
			return stringBuilder.ToString();
		}

		public static string SignPackage(IDictionary<string, string> parameters, string partnerKey)
		{
			string text = SignHelper.BuildQuery(parameters, false);
			text += string.Format("&key={0}", partnerKey);
			return FormsAuthentication.HashPasswordForStoringInConfigFile(text, "MD5").ToUpper();
		}

		public static string SignPay(IDictionary<string, string> parameters, string key = "")
		{
			string text = SignHelper.BuildQuery(parameters, false);
			text += string.Format("&key={0}", key);
			return FormsAuthentication.HashPasswordForStoringInConfigFile(text, "MD5").ToUpper();
		}
	}
}
