using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;

namespace Hishop.Weixin.Pay.Lib
{
	public class HttpService
	{
		private static object LockLog = new object();

		public static void WxDebuglog(string log, string logname = "_wxpay.txt")
		{
			lock (HttpService.LockLog)
			{
				try
				{
					string str = DateTime.Now.ToString("yyyyMMdd") + logname;
					string path = HttpRuntime.AppDomainAppPath.ToString() + "log/" + str;
					StreamWriter streamWriter = File.AppendText(path);
					streamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + log);
					streamWriter.WriteLine("---------------");
					streamWriter.Close();
				}
				catch (Exception var_3_88)
				{
				}
			}
		}

		public static string Post(string xml, string url, bool isUseCert, PayConfig config, int timeout)
		{
			GC.Collect();
			string text = "";
			HttpWebRequest httpWebRequest = null;
			HttpWebResponse httpWebResponse = null;
			string result;
			try
			{
				ServicePointManager.DefaultConnectionLimit = 200;
				if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
				{
					ServicePointManager.ServerCertificateValidationCallback = ((object s, X509Certificate ce, X509Chain ch, SslPolicyErrors e) => true);
				}
				httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "POST";
				httpWebRequest.Timeout = timeout * 1000;
				httpWebRequest.ContentType = "text/xml";
				byte[] bytes = Encoding.UTF8.GetBytes(xml);
				httpWebRequest.ContentLength = (long)bytes.Length;
				if (isUseCert)
				{
					X509Certificate2 value = new X509Certificate2(config.SSLCERT_PATH, config.SSLCERT_PASSWORD);
					httpWebRequest.ClientCertificates.Add(value);
				}
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
				text = streamReader.ReadToEnd().Trim();
				streamReader.Close();
			}
			catch (Exception ex)
			{
				HttpService.WxDebuglog(ex.Message, "_wxpay.txt");
				result = "POSTERROR:" + ex.Message;
				return result;
			}
			finally
			{
				if (httpWebResponse != null)
				{
					httpWebResponse.Close();
				}
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
			result = text;
			return result;
		}

		public static string Get(string url, string PROXY_URL = "")
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("PROXY_URL", PROXY_URL);
			GC.Collect();
			string text = "";
			HttpWebRequest httpWebRequest = null;
			HttpWebResponse httpWebResponse = null;
			string result;
			try
			{
				ServicePointManager.DefaultConnectionLimit = 200;
				if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
				{
					ServicePointManager.ServerCertificateValidationCallback = ((object s, X509Certificate ce, X509Chain ch, SslPolicyErrors e) => true);
				}
				httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "GET";
				if (!string.IsNullOrEmpty(PROXY_URL))
				{
					httpWebRequest.Proxy = new WebProxy
					{
						Address = new Uri(PROXY_URL)
					};
				}
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
				text = streamReader.ReadToEnd().Trim();
				streamReader.Close();
			}
			catch (ThreadAbortException var_6_E9)
			{
				Thread.ResetAbort();
			}
			catch (WebException ex)
			{
				dictionary.Add("HttpService", ex.ToString());
				if (ex.Status == WebExceptionStatus.ProtocolError)
				{
					dictionary.Add("HttpService", "StatusCode : " + ((HttpWebResponse)ex.Response).StatusCode);
					dictionary.Add("HttpService", "StatusDescription : " + ((HttpWebResponse)ex.Response).StatusDescription);
				}
				result = "";
				return result;
			}
			catch (Exception var_8_17F)
			{
				result = "";
				return result;
			}
			finally
			{
				if (httpWebResponse != null)
				{
					httpWebResponse.Close();
				}
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
			result = text;
			return result;
		}
	}
}
