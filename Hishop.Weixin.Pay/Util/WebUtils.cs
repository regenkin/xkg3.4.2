using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Hishop.Weixin.Pay.Util
{
	internal sealed class WebUtils
	{
		private int _timeout = 100000;

		public int Timeout
		{
			get
			{
				return this._timeout;
			}
			set
			{
				this._timeout = value;
			}
		}

		public string DoPost(string url, string data)
		{
			HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
			webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			Stream requestStream = webRequest.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
			Encoding encoding = Encoding.GetEncoding(httpWebResponse.CharacterSet);
			return this.GetResponseAsString(httpWebResponse, encoding);
		}

		public string DoGet(string url)
		{
			HttpWebRequest webRequest = this.GetWebRequest(url, "GET");
			webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
			HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
			Encoding uTF = Encoding.UTF8;
			return this.GetResponseAsString(rsp, uTF);
		}

		public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		public HttpWebRequest GetWebRequest(string url, string method)
		{
			HttpWebRequest httpWebRequest;
			if (url.Contains("https"))
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
				httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
			}
			else
			{
				httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			}
			httpWebRequest.ServicePoint.Expect100Continue = false;
			httpWebRequest.Method = method;
			httpWebRequest.KeepAlive = true;
			httpWebRequest.UserAgent = "Hishop";
			httpWebRequest.Timeout = this._timeout;
			return httpWebRequest;
		}

		public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
		{
			Stream stream = null;
			StreamReader streamReader = null;
			string result;
			try
			{
				stream = rsp.GetResponseStream();
				streamReader = new StreamReader(stream, encoding);
				result = streamReader.ReadToEnd();
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
				if (stream != null)
				{
					stream.Close();
				}
				if (rsp != null)
				{
					rsp.Close();
				}
			}
			return result;
		}
	}
}
