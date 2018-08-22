using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Hidistro.ControlPanel.OutPay.App
{
	public class Core
	{
		public static string _sign_type = "";

		public static string _private_key = "";

		public static string _input_charset = "";

		public static string GATEWAY_NEW = "https://mapi.alipay.com/gateway.do?";

		public static string _partner = "";

		public static void setConfig(string partner, string sing_type, string private_key, string input_charset)
		{
			Core._partner = partner;
			Core._sign_type = sing_type;
			Core._input_charset = input_charset;
			Core._private_key = private_key;
		}

		public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> current in dicArrayPre)
			{
				if (current.Key.ToLower() != "sign" && current.Key.ToLower() != "sign_type" && current.Value != "" && current.Value != null)
				{
					dictionary.Add(current.Key, current.Value);
				}
			}
			return dictionary;
		}

		public static string CreateLinkString(Dictionary<string, string> dicArray)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> current in dicArray)
			{
				stringBuilder.Append(current.Key + "=" + current.Value + "&");
			}
			int length = stringBuilder.Length;
			stringBuilder.Remove(length - 1, 1);
			return stringBuilder.ToString();
		}

		public static string CreateLinkStringUrlencode(Dictionary<string, string> dicArray, Encoding code)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> current in dicArray)
			{
				stringBuilder.Append(current.Key + "=" + HttpUtility.UrlEncode(current.Value, code) + "&");
			}
			int length = stringBuilder.Length;
			stringBuilder.Remove(length - 1, 1);
			return stringBuilder.ToString();
		}

		public static void LogResult(string sWord)
		{
			string text = HttpContext.Current.Server.MapPath("log");
			text = text + "\\" + DateTime.Now.ToString().Replace(":", "") + ".txt";
			StreamWriter streamWriter = new StreamWriter(text, false, Encoding.Default);
			streamWriter.Write(sWord);
			streamWriter.Close();
		}

		public static string GetAbstractToMD5(Stream sFile)
		{
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(sFile);
			StringBuilder stringBuilder = new StringBuilder(32);
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
			}
			return stringBuilder.ToString();
		}

		public static string GetAbstractToMD5(byte[] dataFile)
		{
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(dataFile);
			StringBuilder stringBuilder = new StringBuilder(32);
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
			}
			return stringBuilder.ToString();
		}

		private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary = Core.FilterPara(sParaTemp);
			string value = Core.BuildRequestMysign(dictionary);
			dictionary.Add("sign", value);
			dictionary.Add("sign_type", Core._sign_type);
			return dictionary;
		}

		private static string BuildRequestMysign(Dictionary<string, string> sPara)
		{
			string text = Core.CreateLinkString(sPara);
			string sign_type = Core._sign_type;
			string result;
			if (sign_type != null)
			{
				if (sign_type == "RSA")
				{
					result = RSAFromPkcs8.sign(text, Core._private_key, Core._input_charset);
					return result;
				}
				if (sign_type == "MD5")
				{
					result = Core.GetMD5(text + Core._private_key, Core._input_charset);
					return result;
				}
			}
			result = "";
			return result;
		}

		public static string GetMD5(string myString, string _input_charset = "utf-8")
		{
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.GetEncoding(_input_charset).GetBytes(myString);
			byte[] array = mD.ComputeHash(bytes);
			string text = null;
			for (int i = 0; i < array.Length; i++)
			{
				text += array[i].ToString("x").PadLeft(2, '0');
			}
			return text;
		}

		public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary = Core.BuildRequestPara(sParaTemp);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(new string[]
			{
				"<form id='alipaysubmit' name='alipaysubmit' action='",
				Core.GATEWAY_NEW,
				"_input_charset=",
				Core._input_charset,
				"' method='",
				strMethod.ToLower().Trim(),
				"'>"
			}));
			foreach (KeyValuePair<string, string> current in dictionary)
			{
				stringBuilder.Append(string.Concat(new string[]
				{
					"<input type='hidden' name='",
					current.Key,
					"' value='",
					current.Value,
					"'/>"
				}));
			}
			stringBuilder.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");
			stringBuilder.Append("<script>document.forms['alipaysubmit'].submit();</script>");
			return stringBuilder.ToString();
		}
	}
}
