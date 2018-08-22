using System;
using System.Collections.Generic;

namespace Hishop.Weixin.Pay.Util
{
	internal class PayDictionary : Dictionary<string, string>
	{
		public void Add(string key, object value)
		{
			string value2;
			if (value == null)
			{
				value2 = null;
			}
			else if (value is string)
			{
				value2 = (string)value;
			}
			else if (value is DateTime)
			{
				value2 = ((DateTime)value).ToString("yyyyMMddHHmmss");
			}
			else if (value is DateTime?)
			{
				value2 = (value as DateTime?).Value.ToString("yyyyMMddHHmmss");
			}
			else if (value is decimal)
			{
				value2 = string.Format("{0:F2}", value);
			}
			else if (value is decimal?)
			{
				value2 = string.Format("{0:F0}", (value as decimal?).Value);
			}
			else
			{
				value2 = value.ToString();
			}
			this.Add(key, value2);
		}

		public new void Add(string key, string value)
		{
			if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
			{
				base.Add(key, value);
			}
		}
	}
}
