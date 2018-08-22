using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Hidistro.ControlPanel.OutPay
{
	public class HttpHelp
	{
		public string errstr = "";

		public string DoPost(string url, IDictionary<string, string> parameters)
		{
			HttpWebRequest webRequest = this.GetWebRequest(url, "POST", null, null);
			webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
			byte[] bytes = Encoding.UTF8.GetBytes(HttpHelp.BuildQuery(parameters));
			Stream requestStream = webRequest.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
			return this.GetResponseAsString(rsp, Encoding.UTF8);
		}

		public string DoPost(string url, string value, string cerPassword = null, string cerPath = null)
		{
			HttpWebRequest webRequest = this.GetWebRequest(url, "POST", cerPassword, cerPath);
			webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			Stream requestStream = webRequest.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
			return this.GetResponseAsString(rsp, Encoding.UTF8);
		}

		public string DoGet(string url, IDictionary<string, string> parameters)
		{
			if (parameters != null && parameters.Count > 0)
			{
				if (url.Contains("?"))
				{
					url = url + "&" + HttpHelp.BuildQuery(parameters);
				}
				else
				{
					url = url + "?" + HttpHelp.BuildQuery(parameters);
				}
			}
			HttpWebRequest webRequest = this.GetWebRequest(url, "GET", null, null);
			webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
			HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
			return this.GetResponseAsString(rsp, Encoding.UTF8);
		}

		public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		public HttpWebRequest GetWebRequest(string url, string method, string cerPassword = null, string cerPath = null)
		{
			HttpWebRequest httpWebRequest = null;
			if (url.Contains("https"))
			{
				this.errstr = "";
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
				httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
				try
				{
					if (cerPassword != null)
					{
						X509Certificate2 value = new X509Certificate2(cerPath, cerPassword);
						httpWebRequest.ClientCertificates.Add(value);
					}
				}
				catch (Exception ex)
				{
					this.errstr = ex.Message.ToString().Trim();
				}
			}
			else
			{
				httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			}
			httpWebRequest.ServicePoint.Expect100Continue = false;
			httpWebRequest.Method = method;
			httpWebRequest.KeepAlive = true;
			httpWebRequest.UserAgent = "Hishop";
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

		public string BuildGetUrl(string url, IDictionary<string, string> parameters)
		{
			if (parameters != null && parameters.Count > 0)
			{
				if (url.Contains("?"))
				{
					url = url + "&" + HttpHelp.BuildQuery(parameters);
				}
				else
				{
					url = url + "?" + HttpHelp.BuildQuery(parameters);
				}
			}
			return url;
		}

		public static string BuildQuery(IDictionary<string, string> parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			IEnumerator<KeyValuePair<string, string>> enumerator = parameters.GetEnumerator();
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
					stringBuilder.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
					flag = true;
				}
			}
			return stringBuilder.ToString();
		}
	}
}
