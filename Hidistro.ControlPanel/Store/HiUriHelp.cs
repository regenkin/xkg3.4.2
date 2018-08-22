using Hidistro.Core;
using System;
using System.Collections.Specialized;

namespace Hidistro.ControlPanel.Store
{
	public class HiUriHelp
	{
		private NameValueCollection queryStrings;

		public NameValueCollection QueryStrings
		{
			get
			{
				return this.queryStrings;
			}
			set
			{
				this.queryStrings = value;
			}
		}

		public string NewUrl
		{
			get;
			set;
		}

		public HiUriHelp(string query)
		{
			this.CreateQueryString(query);
		}

		public HiUriHelp(NameValueCollection query)
		{
			this.QueryStrings = new NameValueCollection(query);
		}

		public void AddQueryString(string key, string value)
		{
			if (this.QueryStrings == null)
			{
				this.QueryStrings = new NameValueCollection();
			}
			this.QueryStrings.Add(key, value);
		}

		public string GetQueryString(string key)
		{
			string result;
			if (!string.IsNullOrEmpty(key) && this.queryStrings != null)
			{
				result = this.QueryStrings[key];
			}
			else
			{
				result = "";
			}
			return result;
		}

		public void SetQueryString(string key, string value)
		{
			if (!string.IsNullOrEmpty(key) && this.queryStrings != null)
			{
				this.QueryStrings[key] = value;
			}
		}

		public void RemoveQueryString(string key)
		{
			if (this.QueryStrings != null || this.QueryStrings.Count > 1)
			{
				this.QueryStrings.Remove(key);
			}
		}

		public string GetNewQuery()
		{
			string text = "?";
			string[] allKeys = this.QueryStrings.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				string text2 = allKeys[i];
				Globals.Debuglog(text2 + "：" + this.QueryStrings[text2] + "\r\n", "_Debuglog.txt");
				string text3 = text;
				text = string.Concat(new string[]
				{
					text3,
					text2,
					"=",
					this.QueryStrings[text2],
					"&"
				});
			}
			return text.TrimEnd(new char[]
			{
				'&'
			});
		}

		public void CreateQueryString(string queryString)
		{
			queryString = queryString.Replace("?", "");
			NameValueCollection nameValueCollection = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
			if (!string.IsNullOrEmpty(queryString))
			{
				int length = queryString.Length;
				for (int i = 0; i < length; i++)
				{
					int num = i;
					int num2 = -1;
					while (i < length)
					{
						char c = queryString[i];
						if (c == '=')
						{
							if (num2 < 0)
							{
								num2 = i;
							}
						}
						else if (c == '&')
						{
							break;
						}
						i++;
					}
					string value = null;
					string key;
					if (num2 >= 0)
					{
						key = queryString.Substring(num, num2 - num);
						value = queryString.Substring(num2 + 1, i - num2 - 1);
					}
					else
					{
						key = queryString.Substring(num, i - num);
					}
					this.AddQueryString(key, value);
					if (i == length - 1 && queryString[i] == '&')
					{
						this.AddQueryString(key, string.Empty);
					}
				}
			}
		}
	}
}
