using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;

namespace Hishop.Weixin.Pay
{
	public class PayClient
	{
		public static readonly string Deliver_Notify_Url = "https://api.weixin.qq.com/pay/delivernotify";

		public static readonly string prepay_id_Url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

		private PayAccount _payAccount;

		public PayClient(string appId, string appSecret, string partnerId, string partnerKey, bool enableSP, string sub_appid, string sub_mch_id)
		{
			this._payAccount = new PayAccount(appId, appSecret, partnerId, partnerKey, enableSP, sub_appid, sub_mch_id);
		}

		public PayClient(PayAccount account) : this(account.AppId, account.AppSecret, account.PartnerId, account.PartnerKey, account.EnableSP, account.Sub_appid, account.Sub_mch_id)
		{
		}

		public bool checkSetParams(out string errmsg)
		{
			errmsg = "";
			bool flag = true;
			bool result;
			if (this._payAccount == null)
			{
				flag = false;
				errmsg = "微信支付参数未初始化！";
			}
			else if (!this._payAccount.EnableSP)
			{
				if (string.IsNullOrEmpty(this._payAccount.AppId) || this._payAccount.AppId.Length < 15)
				{
					errmsg = "商户公众号未正确配置！";
					result = false;
					return result;
				}
				if (string.IsNullOrEmpty(this._payAccount.PartnerId) || this._payAccount.PartnerId.Length < 8)
				{
					errmsg = "商户号未正确配置！";
					result = false;
					return result;
				}
				if (string.IsNullOrEmpty(this._payAccount.PartnerKey) || this._payAccount.PartnerKey.Length < 8)
				{
					errmsg = "商户KEY未正确配置！";
					result = false;
					return result;
				}
				if (string.IsNullOrEmpty(this._payAccount.AppSecret) || this._payAccount.AppSecret.Length < 8)
				{
					errmsg = "公众号AppSecret未正确配置！";
					result = false;
					return result;
				}
			}
			else
			{
				if (string.IsNullOrEmpty(this._payAccount.AppId) || this._payAccount.AppId.Length < 15)
				{
					errmsg = "服务商公众号未正确配置！";
					result = false;
					return result;
				}
				if (string.IsNullOrEmpty(this._payAccount.PartnerId) || this._payAccount.PartnerId.Length < 8)
				{
					errmsg = "服务商商户号未正确配置！";
					result = false;
					return result;
				}
				if (string.IsNullOrEmpty(this._payAccount.PartnerKey) || this._payAccount.PartnerKey.Length < 8)
				{
					errmsg = "服务商KEY未正确配置！";
					result = false;
					return result;
				}
				if (string.IsNullOrEmpty(this._payAccount.AppSecret) || this._payAccount.AppSecret.Length < 8)
				{
					errmsg = "公众号AppSecret未正确配置！";
					result = false;
					return result;
				}
				if (string.IsNullOrEmpty(this._payAccount.Sub_appid) || this._payAccount.Sub_appid.Length < 8)
				{
					errmsg = "商户公众号未正确配置！";
					result = false;
					return result;
				}
				if (string.IsNullOrEmpty(this._payAccount.Sub_mch_id) || this._payAccount.Sub_mch_id.Length < 8)
				{
					errmsg = "子商户号未正确配置！";
					result = false;
					return result;
				}
			}
			result = flag;
			return result;
		}

		public bool checkPackage(PackageInfo package, out string errmsg)
		{
			bool flag = true;
			errmsg = "";
			bool result;
			if (string.IsNullOrEmpty(package.NotifyUrl) || package.NotifyUrl.Length < 5)
			{
				errmsg = "返回地址NotifyUrl未配置！";
				result = false;
			}
			else if (string.IsNullOrEmpty(package.OpenId) || package.OpenId.Length < 8)
			{
				errmsg = "用户OPENID不正确！";
				result = false;
			}
			else if (string.IsNullOrEmpty(package.OutTradeNo))
			{
				errmsg = "交易订单号不能为空";
				result = false;
			}
			else if (package.TotalFee == 0m)
			{
				errmsg = "支付金额不能为零";
				result = false;
			}
			else
			{
				result = flag;
			}
			return result;
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
			dataTable.WriteXml(HttpContext.Current.Server.MapPath("/wxpay.xml"));
		}

		internal string BuildPackage(PackageInfo package)
		{
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appid", this._payAccount.AppId);
			payDictionary.Add("mch_id", this._payAccount.PartnerId);
			if (this._payAccount.EnableSP)
			{
				payDictionary.Add("sub_appid", this._payAccount.Sub_appid);
				payDictionary.Add("sub_mch_id", this._payAccount.Sub_mch_id);
				payDictionary.Add("sub_openid", package.OpenId);
			}
			else
			{
				payDictionary.Add("openid", package.OpenId);
			}
			payDictionary.Add("device_info", "");
			payDictionary.Add("nonce_str", Utils.CreateNoncestr());
			payDictionary.Add("body", package.Body);
			payDictionary.Add("attach", "");
			payDictionary.Add("out_trade_no", package.OutTradeNo);
			payDictionary.Add("total_fee", (int)package.TotalFee);
			payDictionary.Add("spbill_create_ip", package.SpbillCreateIp);
			payDictionary.Add("time_start", package.TimeExpire);
			payDictionary.Add("time_expire", "");
			payDictionary.Add("goods_tag", package.GoodsTag);
			payDictionary.Add("notify_url", package.NotifyUrl);
			payDictionary.Add("trade_type", "JSAPI");
			payDictionary.Add("product_id", "");
			string sign = SignHelper.SignPackage(payDictionary, this._payAccount.PartnerKey);
			PayClient.writeLog(payDictionary, sign, "", "");
			string text = this.GetPrepay_id(payDictionary, sign);
			if (text.Length > 64)
			{
				text = "";
			}
			return string.Format("prepay_id=" + text, new object[0]);
		}

		public PayRequestInfo BuildPayRequest(PackageInfo package)
		{
			PayRequestInfo payRequestInfo = new PayRequestInfo();
			payRequestInfo.appId = this._payAccount.AppId;
			payRequestInfo.package = this.BuildPackage(package);
			payRequestInfo.timeStamp = Utils.GetCurrentTimeSeconds().ToString();
			payRequestInfo.nonceStr = Utils.CreateNoncestr();
			payRequestInfo.paySign = SignHelper.SignPay(new PayDictionary
			{
				{
					"appId",
					this._payAccount.AppId
				},
				{
					"timeStamp",
					payRequestInfo.timeStamp
				},
				{
					"package",
					payRequestInfo.package
				},
				{
					"nonceStr",
					payRequestInfo.nonceStr
				},
				{
					"signType",
					"MD5"
				}
			}, this._payAccount.PartnerKey);
			return payRequestInfo;
		}

		public bool DeliverNotify(DeliverInfo deliver)
		{
			string token = Utils.GetToken(this._payAccount.AppId, this._payAccount.AppSecret);
			return this.DeliverNotify(deliver, token);
		}

		public bool DeliverNotify(DeliverInfo deliver, string token)
		{
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appid", this._payAccount.AppId);
			payDictionary.Add("openid", deliver.OpenId);
			payDictionary.Add("transid", deliver.TransId);
			payDictionary.Add("out_trade_no", deliver.OutTradeNo);
			payDictionary.Add("deliver_timestamp", Utils.GetTimeSeconds(deliver.TimeStamp));
			payDictionary.Add("deliver_status", deliver.Status ? 1 : 0);
			payDictionary.Add("deliver_msg", deliver.Message);
			deliver.AppId = this._payAccount.AppId;
			deliver.AppSignature = SignHelper.SignPay(payDictionary, "");
			payDictionary.Add("app_signature", deliver.AppSignature);
			payDictionary.Add("sign_method", deliver.SignMethod);
			string data = JsonConvert.SerializeObject(payDictionary);
			string url = string.Format("{0}?access_token={1}", PayClient.Deliver_Notify_Url, token);
			string text = new WebUtils().DoPost(url, data);
			return !string.IsNullOrEmpty(text) && text.Contains("ok");
		}

		internal string GetPrepay_id(PayDictionary dict, string sign)
		{
			dict.Add("sign", sign);
			string value = SignHelper.BuildQuery(dict, false);
			string text = SignHelper.BuildXml(dict, false);
			string text2 = "";
			text2 = PayClient.PostData(PayClient.prepay_id_Url, text);
			try
			{
				DataTable dataTable = new DataTable();
				dataTable.TableName = "log";
				dataTable.Columns.Add(new DataColumn("OperTime"));
				dataTable.Columns.Add(new DataColumn("Info"));
				dataTable.Columns.Add(new DataColumn("param"));
				dataTable.Columns.Add(new DataColumn("query"));
				DataRow dataRow = dataTable.NewRow();
				dataRow["OperTime"] = DateTime.Now.ToString();
				dataRow["Info"] = text2;
				dataRow["param"] = text;
				dataRow["query"] = value;
				dataTable.Rows.Add(dataRow);
				dataTable.WriteXml(HttpContext.Current.Request.MapPath("/PrepayID.xml"));
			}
			catch (Exception ex)
			{
				PayClient.writeLog(dict, sign, "", ex.Message + "-PrepayId获取错误");
			}
			return text2;
		}

		public static string PostData(string url, string postData)
		{
			string text = string.Empty;
			string result;
			try
			{
				Uri requestUri = new Uri(url);
				HttpWebRequest httpWebRequest;
				if (url.ToLower().StartsWith("https"))
				{
					ServicePointManager.ServerCertificateValidationCallback = ((object s, X509Certificate c, X509Chain ch, SslPolicyErrors e) => true);
					httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(requestUri);
				}
				else
				{
					httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
				}
				Encoding uTF = Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.KeepAlive = true;
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				requestStream.Dispose();
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
							if (xmlNode.InnerText == "SUCCESS")
							{
								XmlNode xmlNode2 = xmlDocument.SelectSingleNode("xml/prepay_id");
								if (xmlNode2 != null)
								{
									result = xmlNode2.InnerText;
									return result;
								}
							}
							else
							{
								XmlNode xmlNode3 = xmlDocument.SelectSingleNode("xml/return_msg");
								if (xmlNode3 != null)
								{
									result = xmlNode3.InnerText;
									return result;
								}
								result = xmlDocument.InnerXml;
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
