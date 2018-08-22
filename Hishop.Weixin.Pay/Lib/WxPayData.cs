using LitJson;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Hishop.Weixin.Pay.Lib
{
	public class WxPayData
	{
		private SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();

		public void SetValue(string key, object value)
		{
			this.m_values[key] = value;
		}

		public object GetValue(string key)
		{
			object result = null;
			this.m_values.TryGetValue(key, out result);
			return result;
		}

		public bool IsSet(string key)
		{
			object obj = null;
			this.m_values.TryGetValue(key, out obj);
			return null != obj;
		}

		public string ToXml()
		{
			StringBuilder stringBuilder = new StringBuilder("<?xml version=\"1.0\" standalone=\"true\"?>");
			stringBuilder.AppendLine("<xml>");
			string result;
			foreach (KeyValuePair<string, object> current in this.m_values)
			{
				if (current.Value == null)
				{
					result = "";
					return result;
				}
				if (current.Value.GetType() == typeof(int))
				{
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"<",
						current.Key,
						">",
						current.Value,
						"</",
						current.Key,
						">"
					}));
				}
				else if (current.Value.GetType() == typeof(string))
				{
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"<",
						current.Key,
						"><![CDATA[",
						current.Value,
						"]]></",
						current.Key,
						">"
					}));
				}
			}
			stringBuilder.AppendLine("</xml>");
			result = stringBuilder.ToString();
			return result;
		}

		public SortedDictionary<string, object> FromXml(string xml, string key)
		{
			SortedDictionary<string, object> result;
			if (string.IsNullOrEmpty(xml))
			{
				result = null;
			}
			else
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				XmlNode firstChild = xmlDocument.FirstChild;
				XmlNodeList childNodes = firstChild.ChildNodes;
				foreach (XmlNode xmlNode in childNodes)
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					this.m_values[xmlElement.Name] = xmlElement.InnerText;
				}
				try
				{
					if (!this.CheckSign(key))
					{
						this.m_values.Clear();
						this.m_values["return_code"] = "FAIL";
						this.m_values["return_msg"] = "签名验证异常";
					}
				}
				catch (WxPayException ex)
				{
					this.m_values.Clear();
					this.m_values["return_code"] = "FAIL";
					this.m_values["return_msg"] = "签名异常" + ex.Message;
				}
				result = this.m_values;
			}
			return result;
		}

		public string ToUrl()
		{
			string text = "";
			foreach (KeyValuePair<string, object> current in this.m_values)
			{
				if (current.Key != "sign" && current.Value.ToString() != "")
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						current.Key,
						"=",
						current.Value,
						"&"
					});
				}
			}
			text = text.Trim(new char[]
			{
				'&'
			});
			return text;
		}

		public string ToJson()
		{
			return JsonMapper.ToJson(this.m_values);
		}

		public string ToPrintStr()
		{
			string text = "";
			foreach (KeyValuePair<string, object> current in this.m_values)
			{
				text += string.Format("{0}={1}<br>", current.Key, current.Value.ToString());
			}
			return text;
		}

		public string MakeSign(string key)
		{
			string text = this.ToUrl();
			text = text + "&key=" + key;
			MD5 mD = MD5.Create();
			byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(text));
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString().ToUpper();
		}

		public bool CheckSign(string key)
		{
			bool result;
			if (!this.IsSet("sign"))
			{
				result = true;
			}
			else
			{
				if (this.GetValue("sign") == null || this.GetValue("sign").ToString() == "")
				{
					this.SetValue("sign", "");
				}
				string b = this.GetValue("sign").ToString();
				string a = this.MakeSign(key);
				result = (a == b);
			}
			return result;
		}

		public SortedDictionary<string, object> GetValues()
		{
			return this.m_values;
		}

		public IDictionary<string, string> GetParam()
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (string current in this.m_values.Keys)
			{
				dictionary.Add(current, this.m_values[current].ToString());
			}
			return dictionary;
		}
	}
}
