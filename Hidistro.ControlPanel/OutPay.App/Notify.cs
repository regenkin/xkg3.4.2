using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Hidistro.ControlPanel.OutPay.App
{
	public class Notify
	{
		private string Https_veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&";

		private string _public_key = "";

		public Notify()
		{
			this._public_key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";
		}

		public bool Verify(SortedDictionary<string, string> inputPara, string notify_id, string sign)
		{
			bool signVeryfy = this.GetSignVeryfy(inputPara, sign, "");
			string a = "true";
			if (notify_id != null && notify_id != "")
			{
				a = this.GetResponseTxt(notify_id);
			}
			return a == "true" && signVeryfy;
		}

		private string GetPreSignStr(SortedDictionary<string, string> inputPara)
		{
			Dictionary<string, string> dicArray = new Dictionary<string, string>();
			dicArray = Core.FilterPara(inputPara);
			return Core.CreateLinkString(dicArray);
		}

		private bool GetSignVeryfy(SortedDictionary<string, string> inputPara, string sign, string _private_key = "")
		{
			Dictionary<string, string> dicArray = new Dictionary<string, string>();
			dicArray = Core.FilterPara(inputPara);
			string text = Core.CreateLinkString(dicArray);
			bool result = false;
			if (sign != null && sign != "")
			{
				string sign_type = Core._sign_type;
				if (sign_type != null)
				{
					if (!(sign_type == "RSA"))
					{
						if (sign_type == "MD5")
						{
							result = (Core.GetMD5(text + Core._private_key, Core._input_charset) == sign);
						}
					}
					else
					{
						result = RSAFromPkcs8.verify(text, sign, this._public_key, Core._input_charset);
					}
				}
			}
			return result;
		}

		private string GetResponseTxt(string notify_id)
		{
			string strUrl = string.Concat(new string[]
			{
				this.Https_veryfy_url,
				"partner=",
				Core._partner,
				"&notify_id=",
				notify_id
			});
			return this.Get_Http(strUrl, 120000);
		}

		private string Get_Http(string strUrl, int timeout)
		{
			string result;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(strUrl);
				httpWebRequest.Timeout = timeout;
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, Encoding.Default);
				StringBuilder stringBuilder = new StringBuilder();
				while (-1 != streamReader.Peek())
				{
					stringBuilder.Append(streamReader.ReadLine());
				}
				result = stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				result = "错误：" + ex.Message;
			}
			return result;
		}
	}
}
