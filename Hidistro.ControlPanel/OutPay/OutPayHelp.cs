using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.OutPay;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Hidistro.ControlPanel.OutPay
{
	public class OutPayHelp
	{
		public static char[] Chars = new char[]
		{
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'R',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z',
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z'
		};

		private static string WeiXinAppid = "";

		private static string WeiXinMchid = "";

		private static string WeiXinKey = "";

		public static string BatchWeixinPayCheckRealName = "";

		private static string WeiXinCertPath = "";

		private static string WeixinCertPassword = "";

		private static bool IsReadSeting = false;

		private static string WeiPayUrl = string.Format("https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers", new object[0]);

		private static string GATEWAY_NEW = "https://mapi.alipay.com/gateway.do?";

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

		public static string GetRandomString(int length)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Random random = new Random();
			string value = DateTime.Now.ToString("yyyyMMdd");
			stringBuilder.Append(value);
			for (int i = 0; i < length; i++)
			{
				stringBuilder.Append(OutPayHelp.Chars[random.Next(0, OutPayHelp.Chars.Length)]);
			}
			return stringBuilder.ToString();
		}

		public static List<WeiPayResult> BatchWeiPay(List<OutPayWeiInfo> BatchUserList)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			OutPayHelp.WeiXinMchid = masterSettings.WeixinPartnerID;
			OutPayHelp.WeiXinAppid = masterSettings.WeixinAppId;
			OutPayHelp.WeiXinKey = masterSettings.WeixinPartnerKey;
			OutPayHelp.BatchWeixinPayCheckRealName = masterSettings.BatchWeixinPayCheckRealName.ToString();
			OutPayHelp.WeiXinCertPath = masterSettings.WeixinCertPath;
			OutPayHelp.WeixinCertPassword = masterSettings.WeixinCertPassword;
			string batchWeixinPayCheckRealName = OutPayHelp.BatchWeixinPayCheckRealName;
			if (batchWeixinPayCheckRealName != null)
			{
				if (!(batchWeixinPayCheckRealName == "0"))
				{
					if (!(batchWeixinPayCheckRealName == "1"))
					{
						if (batchWeixinPayCheckRealName == "2")
						{
							OutPayHelp.BatchWeixinPayCheckRealName = "OPTION_CHECK";
						}
					}
					else
					{
						OutPayHelp.BatchWeixinPayCheckRealName = "FORCE_CHECK";
					}
				}
				else
				{
					OutPayHelp.BatchWeixinPayCheckRealName = "NO_CHECK";
				}
			}
			List<WeiPayResult> list = new List<WeiPayResult>();
			WeiPayResult weiPayResult = new WeiPayResult();
			weiPayResult.return_code = "SUCCESS";
			weiPayResult.err_code = "";
			weiPayResult.return_msg = "微信企业付款参数配置错误";
			if (OutPayHelp.WeiXinMchid == "")
			{
				weiPayResult.return_code = "FAIL";
				weiPayResult.return_msg = "商户号未配置！";
			}
			else if (OutPayHelp.WeiXinAppid == "")
			{
				weiPayResult.return_code = "FAIL";
				weiPayResult.return_msg = "公众号APPID未配置！";
			}
			else if (OutPayHelp.WeiXinKey == "")
			{
				weiPayResult.return_code = "FAIL";
				weiPayResult.return_msg = "商户密钥未配置！";
			}
			List<WeiPayResult> result;
			if (weiPayResult.return_code == "FAIL")
			{
				weiPayResult.return_code = "INITFAIL";
				list.Add(weiPayResult);
				result = list;
			}
			else
			{
				foreach (OutPayWeiInfo current in BatchUserList)
				{
					WeiPayResult weiPayResult2 = OutPayHelp.WeiXinPayOut(current, OutPayHelp.WeiXinAppid, OutPayHelp.WeiXinMchid, OutPayHelp.BatchWeixinPayCheckRealName, OutPayHelp.WeiXinKey);
					list.Add(weiPayResult2);
					if (weiPayResult2.return_code == "SUCCESS" && (weiPayResult2.err_code == "NOAUTH" || weiPayResult2.err_code == "NOTENOUGH" || weiPayResult2.err_code == "CA_ERROR" || weiPayResult2.err_code == "SIGN_ERROR" || weiPayResult2.err_code == "XML_ERROR"))
					{
						list.Add(weiPayResult2);
						break;
					}
				}
				result = list;
			}
			return result;
		}

		public static WeiPayResult SingleWeiPay(int amount, string desc, string useropenid, string realname, string tradeno, int UserId)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			OutPayHelp.WeiXinMchid = masterSettings.WeixinPartnerID;
			OutPayHelp.WeiXinAppid = masterSettings.WeixinAppId;
			OutPayHelp.WeiXinKey = masterSettings.WeixinPartnerKey;
			OutPayHelp.BatchWeixinPayCheckRealName = masterSettings.BatchWeixinPayCheckRealName.ToString();
			OutPayHelp.WeiXinCertPath = masterSettings.WeixinCertPath;
			OutPayHelp.WeixinCertPassword = masterSettings.WeixinCertPassword;
			string batchWeixinPayCheckRealName = OutPayHelp.BatchWeixinPayCheckRealName;
			if (batchWeixinPayCheckRealName != null)
			{
				if (!(batchWeixinPayCheckRealName == "0"))
				{
					if (!(batchWeixinPayCheckRealName == "1"))
					{
						if (batchWeixinPayCheckRealName == "2")
						{
							OutPayHelp.BatchWeixinPayCheckRealName = "OPTION_CHECK";
						}
					}
					else
					{
						OutPayHelp.BatchWeixinPayCheckRealName = "FORCE_CHECK";
					}
				}
				else
				{
					OutPayHelp.BatchWeixinPayCheckRealName = "NO_CHECK";
				}
			}
			WeiPayResult weiPayResult = new WeiPayResult();
			weiPayResult.return_code = "SUCCESS";
			weiPayResult.err_code = "";
			weiPayResult.return_msg = "微信企业付款参数配置错误";
			if (OutPayHelp.WeiXinMchid == "")
			{
				weiPayResult.return_code = "FAIL";
				weiPayResult.return_msg = "商户号未配置！";
			}
			else if (OutPayHelp.WeiXinAppid == "")
			{
				weiPayResult.return_code = "FAIL";
				weiPayResult.return_msg = "公众号APPID未配置！";
			}
			else if (OutPayHelp.WeiXinKey == "")
			{
				weiPayResult.return_code = "FAIL";
				weiPayResult.return_msg = "商户密钥未配置！";
			}
			WeiPayResult result;
			if (weiPayResult.return_code == "FAIL")
			{
				result = weiPayResult;
			}
			else
			{
				weiPayResult.return_code = "FAIL";
				weiPayResult.return_msg = "用户参数出错了！";
				result = OutPayHelp.WeiXinPayOut(new OutPayWeiInfo
				{
					Amount = amount,
					Partner_Trade_No = tradeno,
					Openid = useropenid,
					Re_User_Name = realname,
					Desc = desc,
					UserId = UserId,
					device_info = "",
					Nonce_Str = OutPayHelp.GetRandomString(20)
				}, OutPayHelp.WeiXinAppid, OutPayHelp.WeiXinMchid, OutPayHelp.BatchWeixinPayCheckRealName, OutPayHelp.WeiXinKey);
			}
			return result;
		}

		public static WeiPayResult WeiXinPayOut(OutPayWeiInfo payinfos, string Mch_appid, string Mchid, string Check_Name, string _key)
		{
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			sortedDictionary.Add("mch_appid", Mch_appid);
			sortedDictionary.Add("mchid", Mchid);
			sortedDictionary.Add("nonce_str", payinfos.Nonce_Str);
			sortedDictionary.Add("partner_trade_no", payinfos.Partner_Trade_No);
			sortedDictionary.Add("openid", payinfos.Openid);
			sortedDictionary.Add("check_name", Check_Name);
			sortedDictionary.Add("amount", payinfos.Amount.ToString());
			sortedDictionary.Add("desc", payinfos.Desc);
			sortedDictionary.Add("spbill_create_ip", Globals.ServerIP());
			sortedDictionary.Add("re_user_name", payinfos.Re_User_Name);
			sortedDictionary.Add("device_info", "");
			string text = "";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<xml>");
			foreach (string current in sortedDictionary.Keys)
			{
				if (sortedDictionary[current] != "")
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"&",
						current,
						"=",
						sortedDictionary[current]
					});
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"<",
						current,
						">",
						sortedDictionary[current],
						"</",
						current,
						">"
					}));
				}
			}
			text = text.Remove(0, 1);
			text = text + "&key=" + _key;
			text = OutPayHelp.GetMD5(text, "utf-8");
			text = text.ToUpper();
			stringBuilder.AppendLine("<sign>" + text + "</sign>");
			stringBuilder.AppendLine("</xml>");
			HttpHelp httpHelp = new HttpHelp();
			string text3 = httpHelp.DoPost(OutPayHelp.WeiPayUrl, stringBuilder.ToString(), OutPayHelp.WeixinCertPassword, OutPayHelp.WeiXinCertPath);
			WeiPayResult weiPayResult = new WeiPayResult();
			weiPayResult.return_code = "FAIL";
			weiPayResult.return_msg = "访问服务器出错了！";
			weiPayResult.err_code = "SERVERERR";
			weiPayResult.UserId = payinfos.UserId;
			weiPayResult.Amount = payinfos.Amount;
			weiPayResult.partner_trade_no = payinfos.Partner_Trade_No;
			WeiPayResult result;
			if (httpHelp.errstr != "")
			{
				weiPayResult.return_msg = httpHelp.errstr;
				result = weiPayResult;
			}
			else
			{
				try
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(text3);
					weiPayResult.return_code = xmlDocument.SelectSingleNode("/xml/return_code").InnerText;
					weiPayResult.return_msg = xmlDocument.SelectSingleNode("/xml/return_msg").InnerText;
					if (weiPayResult.return_code.ToUpper() == "SUCCESS")
					{
						weiPayResult.result_code = xmlDocument.SelectSingleNode("/xml/result_code").InnerText;
						if (weiPayResult.result_code.ToUpper() == "SUCCESS")
						{
							weiPayResult.mch_appid = xmlDocument.SelectSingleNode("/xml/mch_appid").InnerText;
							weiPayResult.mchid = xmlDocument.SelectSingleNode("/xml/mchid").InnerText;
							weiPayResult.device_info = xmlDocument.SelectSingleNode("/xml/device_info").InnerText;
							weiPayResult.nonce_str = xmlDocument.SelectSingleNode("/xml/nonce_str").InnerText;
							weiPayResult.result_code = xmlDocument.SelectSingleNode("/xml/result_code").InnerText;
							weiPayResult.partner_trade_no = xmlDocument.SelectSingleNode("/xml/partner_trade_no").InnerText;
							weiPayResult.payment_no = xmlDocument.SelectSingleNode("/xml/payment_no").InnerText;
							weiPayResult.payment_time = xmlDocument.SelectSingleNode("/xml/payment_time").InnerText;
						}
						else
						{
							weiPayResult.err_code = xmlDocument.SelectSingleNode("/xml/err_code").InnerText;
						}
					}
					else
					{
						weiPayResult.err_code = "FAIL";
					}
				}
				catch (Exception ex)
				{
					Globals.Debuglog(text3, "_DebuglogBatchPayment.txt");
					weiPayResult.return_code = "FAIL";
					weiPayResult.return_msg = ex.Message.ToString();
				}
				result = weiPayResult;
			}
			return result;
		}

		public static string Sign(string prestr, string key, string _input_charset)
		{
			return OutPayHelp.GetMD5(prestr + key, "utf-8");
		}

		public static bool Verify(string prestr, string sign, string key, string _input_charset)
		{
			string a = OutPayHelp.Sign(prestr, key, _input_charset);
			return a == sign;
		}

		public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue, string _key, string _input_charset)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary = OutPayHelp.BuildRequestPara(sParaTemp, _key, _input_charset);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(new string[]
			{
				"<form id='alipaysubmit' name='alipaysubmit' action='",
				OutPayHelp.GATEWAY_NEW,
				"_input_charset=",
				_input_charset,
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

		private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp, string _key, string _input_charset)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary = OutPayHelp.FilterPara(sParaTemp);
			string value = OutPayHelp.BuildRequestMysign(dictionary, _key, _input_charset);
			dictionary.Add("sign", value);
			dictionary.Add("sign_type", "MD5");
			return dictionary;
		}

		private static string BuildRequestMysign(Dictionary<string, string> sPara, string _key, string _input_charset)
		{
			string prestr = OutPayHelp.CreateLinkString(sPara);
			return OutPayHelp.Sign(prestr, _key, _input_charset);
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

		public static bool VerifyNotify(SortedDictionary<string, string> inputPara, string notify_id, string sign, string _key, string _input_charset, string _partner)
		{
			Dictionary<string, string> dicArray = new Dictionary<string, string>();
			dicArray = OutPayHelp.FilterPara(inputPara);
			string prestr = OutPayHelp.CreateLinkString(dicArray);
			bool flag = OutPayHelp.Verify(prestr, sign, _key, _input_charset);
			string a = "true";
			if (notify_id != null && notify_id != "")
			{
				string text = "https://mapi.alipay.com/gateway.do?service=notify_verify&";
				a = new HttpHelp().DoGet(string.Concat(new string[]
				{
					text,
					"partner=",
					_partner,
					"&notify_id=",
					notify_id
				}), null);
			}
			return a == "true" && flag;
		}
	}
}
