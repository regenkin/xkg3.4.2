using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Hishop.Weixin.Pay
{
	public class RedPackClient
	{
		public static readonly string SendRedPack_Url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";

		private static object LockLog = new object();

		public string SendRedpack(string appId, string mch_id, string sub_mch_id, string nick_name, string send_name, string re_openid, string wishing, string client_ip, string act_name, string remark, int amount, string partnerkey, string weixincertpath, string weixincertpassword, int sendredpackrecordid, bool enablesp, string main_appId, string main_mch_id, string main_paykey)
		{
			return this.SendRedpack(new SendRedPackInfo
			{
				WXAppid = appId,
				Mch_Id = mch_id,
				Sub_Mch_Id = mch_id,
				Main_AppId = main_appId,
				Main_Mch_ID = main_mch_id,
				Main_PayKey = main_paykey,
				EnableSP = enablesp,
				Nick_Name = nick_name,
				Send_Name = send_name,
				Re_Openid = re_openid,
				Wishing = wishing,
				Client_IP = client_ip,
				Act_Name = act_name,
				Remark = remark,
				Total_Amount = amount,
				PartnerKey = partnerkey,
				WeixinCertPath = weixincertpath,
				WeixinCertPassword = weixincertpassword,
				SendRedpackRecordID = sendredpackrecordid
			});
		}

		public string SendRedpack(SendRedPackInfo sendredpack)
		{
			string result = string.Empty;
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("nonce_str", Utils.CreateNoncestr());
			if (sendredpack.EnableSP)
			{
				if (sendredpack.SendRedpackRecordID > 0)
				{
					payDictionary.Add("mch_billno", sendredpack.Main_Mch_ID + DateTime.Now.ToString("yyyymmdd") + sendredpack.SendRedpackRecordID.ToString().PadLeft(10, '0'));
				}
				else
				{
					payDictionary.Add("mch_billno", sendredpack.Main_Mch_ID + DateTime.Now.ToString("yyyymmdd") + DateTime.Now.ToString("MMddHHmmss"));
				}
				payDictionary.Add("mch_id", sendredpack.Main_Mch_ID);
				payDictionary.Add("sub_mch_id", sendredpack.Sub_Mch_Id);
				payDictionary.Add("wxappid", sendredpack.Main_AppId);
				payDictionary.Add("msgappid", sendredpack.Main_AppId);
			}
			else
			{
				if (sendredpack.SendRedpackRecordID > 0)
				{
					payDictionary.Add("mch_billno", sendredpack.Mch_Id + DateTime.Now.ToString("yyyymmdd") + sendredpack.SendRedpackRecordID.ToString().PadLeft(10, '0'));
				}
				else
				{
					payDictionary.Add("mch_billno", sendredpack.Mch_Id + DateTime.Now.ToString("yyyymmdd") + DateTime.Now.ToString("MMddHHmmss"));
				}
				payDictionary.Add("mch_id", sendredpack.Mch_Id);
				payDictionary.Add("wxappid", sendredpack.WXAppid);
				payDictionary.Add("nick_name", sendredpack.Nick_Name);
				payDictionary.Add("min_value", sendredpack.Total_Amount);
				payDictionary.Add("max_value", sendredpack.Total_Amount);
			}
			payDictionary.Add("send_name", sendredpack.Send_Name);
			payDictionary.Add("re_openid", sendredpack.Re_Openid);
			payDictionary.Add("total_amount", sendredpack.Total_Amount);
			payDictionary.Add("total_num", sendredpack.Total_Num);
			payDictionary.Add("wishing", sendredpack.Wishing);
			payDictionary.Add("client_ip", sendredpack.Client_IP);
			payDictionary.Add("act_name", sendredpack.Act_Name);
			payDictionary.Add("remark", sendredpack.Remark);
			string text = SignHelper.SignPackage(payDictionary, sendredpack.PartnerKey);
			payDictionary.Add("sign", text);
			string text2 = SignHelper.BuildXml(payDictionary, false);
			RedPackClient.Debuglog(text2, "_DebugRedPacklog.txt");
			string text3 = RedPackClient.Send(sendredpack.WeixinCertPath, sendredpack.WeixinCertPassword, text2, RedPackClient.SendRedPack_Url);
			RedPackClient.writeLog(payDictionary, text, RedPackClient.SendRedPack_Url, text3);
			if (!string.IsNullOrEmpty(text3) && text3.Contains("SUCCESS") && !text3.Contains("<![CDATA[FAIL]]></result_code>"))
			{
				result = "1";
			}
			else
			{
				Regex regex = new Regex("<return_msg><!\\[CDATA\\[(?<code>(.*))\\]\\]></return_msg>");
				Match match = regex.Match(text3);
				string empty = string.Empty;
				if (match.Success)
				{
					result = match.Groups["code"].Value;
				}
			}
			return result;
		}

		public static void Debuglog(string log, string logname = "_DebugRedPacklog.txt")
		{
			lock (RedPackClient.LockLog)
			{
				try
				{
					string str = DateTime.Now.ToString("yyyyMMdd") + logname;
					string path = HttpRuntime.AppDomainAppPath.ToString() + "log/" + str;
					StreamWriter streamWriter = File.AppendText(path);
					streamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + log);
					streamWriter.WriteLine("---------------");
					streamWriter.Close();
				}
				catch (Exception var_3_88)
				{
				}
			}
		}

		public static string Send(string cert, string password, string data, string url)
		{
			return RedPackClient.Send(cert, password, Encoding.GetEncoding("UTF-8").GetBytes(data), url);
		}

		public static string Send(string cert, string password, byte[] data, string url)
		{
			ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RedPackClient.CheckValidationResult);
			X509Certificate2 value = new X509Certificate2(cert, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
			X509Certificate2 certificate = new X509Certificate2(cert, password);
			X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			x509Store.Open(OpenFlags.ReadWrite);
			x509Store.Remove(certificate);
			x509Store.Add(certificate);
			x509Store.Close();
			HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
			if (httpWebRequest == null)
			{
				throw new ApplicationException(string.Format("Invalid url string: {0}", url));
			}
			httpWebRequest.UserAgent = "Hishop";
			httpWebRequest.ContentType = "text/xml";
			httpWebRequest.ClientCertificates.Add(value);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentLength = (long)data.Length;
			Stream requestStream = httpWebRequest.GetRequestStream();
			requestStream.Write(data, 0, data.Length);
			requestStream.Close();
			Stream responseStream;
			try
			{
				responseStream = httpWebRequest.GetResponse().GetResponseStream();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			string result = string.Empty;
			using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
			{
				result = streamReader.ReadToEnd();
			}
			responseStream.Close();
			return result;
		}

		private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return errors == SslPolicyErrors.None;
		}

		public static void writeLog(IDictionary<string, string> param, string sign, string url, string msg)
		{
			DataTable dataTable = new DataTable();
			dataTable.TableName = "log";
			dataTable.Columns.Add(new DataColumn("OperTime"));
			foreach (KeyValuePair<string, string> current in param)
			{
				dataTable.Columns.Add(new DataColumn(current.Key));
			}
			dataTable.Columns.Add(new DataColumn("Msg"));
			dataTable.Columns.Add(new DataColumn("Sign"));
			dataTable.Columns.Add(new DataColumn("Url"));
			DataRow dataRow = dataTable.NewRow();
			dataRow["OperTime"] = DateTime.Now;
			foreach (KeyValuePair<string, string> current in param)
			{
				dataRow[current.Key] = current.Value;
			}
			dataRow["Msg"] = msg;
			dataRow["Sign"] = sign;
			dataRow["Url"] = url;
			dataTable.Rows.Add(dataRow);
			dataTable.WriteXml(HttpContext.Current.Server.MapPath("~/wxpay.xml"));
		}

		public static string PostData(string url, string postData)
		{
			string text = string.Empty;
			string result;
			try
			{
				Uri requestUri = new Uri(url);
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
				Encoding uTF = Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "text/xml";
				httpWebRequest.ContentLength = (long)postData.Length;
				using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
				{
					streamWriter.Write(postData);
				}
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream responseStream = httpWebResponse.GetResponseStream())
					{
						Encoding uTF2 = Encoding.UTF8;
						StreamReader streamReader = new StreamReader(responseStream, uTF2);
						text = streamReader.ReadToEnd();
						XmlDocument xmlDocument = new XmlDocument();
						try
						{
							xmlDocument.LoadXml(text);
						}
						catch (Exception ex)
						{
							text = string.Format("获取信息错误doc.load：{0}", ex.Message) + text;
						}
						try
						{
							if (xmlDocument == null)
							{
								result = text;
								return result;
							}
							XmlNode xmlNode = xmlDocument.SelectSingleNode("xml/return_code");
							if (xmlNode == null)
							{
								result = text;
								return result;
							}
							if (!(xmlNode.InnerText == "SUCCESS"))
							{
								result = xmlDocument.InnerXml;
								return result;
							}
							XmlNode xmlNode2 = xmlDocument.SelectSingleNode("xml/prepay_id");
							if (xmlNode2 != null)
							{
								result = xmlNode2.InnerText;
								return result;
							}
						}
						catch (Exception ex)
						{
							text = string.Format("获取信息错误node.load：{0}", ex.Message) + text;
						}
					}
				}
			}
			catch (Exception ex)
			{
				text = string.Format("获取信息错误post error：{0}", ex.Message) + text;
			}
			result = text;
			return result;
		}
	}
}
