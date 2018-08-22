using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;
using Hishop.Weixin.Pay.Notify;
using Hishop.Weixin.Pay.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace Hishop.Weixin.Pay
{
	public class NotifyClient
	{
		public static readonly string Update_Feedback_Url = "https://api.weixin.qq.com/payfeedback/update";

		private PayAccount _payAccount;

		public NotifyClient(string appId, string appSecret, string partnerId, string partnerKey, bool enableSP, string sub_appid, string sub_mch_id)
		{
			this._payAccount = new PayAccount(appId, appSecret, partnerId, partnerKey, enableSP, sub_appid, sub_mch_id);
		}

		public NotifyClient(PayAccount account) : this(account.AppId, account.AppSecret, account.PartnerId, account.PartnerKey, account.EnableSP, account.Sub_appid, account.Sub_mch_id)
		{
		}

		private string ReadString(Stream inStream)
		{
			string result;
			if (inStream == null)
			{
				result = null;
			}
			else
			{
				byte[] array = new byte[inStream.Length];
				inStream.Read(array, 0, array.Length);
				result = Encoding.UTF8.GetString(array);
			}
			return result;
		}

		private bool ValidPaySign(PayNotify notify, out string servicesign)
		{
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appid", notify.appid);
			payDictionary.Add("bank_type", notify.bank_type);
			payDictionary.Add("cash_fee", notify.cash_fee);
			payDictionary.Add("fee_type", notify.fee_type);
			payDictionary.Add("is_subscribe", notify.is_subscribe);
			payDictionary.Add("mch_id", notify.mch_id);
			payDictionary.Add("nonce_str", notify.nonce_str);
			payDictionary.Add("openid", notify.openid);
			payDictionary.Add("out_trade_no", notify.out_trade_no);
			payDictionary.Add("result_code", notify.result_code);
			payDictionary.Add("return_code", notify.return_code);
			payDictionary.Add("sub_mch_id", notify.sub_mch_id);
			payDictionary.Add("time_end", notify.time_end);
			payDictionary.Add("total_fee", notify.total_fee);
			payDictionary.Add("trade_type", notify.trade_type);
			payDictionary.Add("transaction_id", notify.transaction_id);
			servicesign = SignHelper.SignPay(payDictionary, this._payAccount.PartnerKey);
			bool result = notify.sign == servicesign;
			servicesign = servicesign + "-" + SignHelper.BuildQuery(payDictionary, false);
			return result;
		}

		private bool ValidAlarmSign(AlarmNotify notify)
		{
			return true;
		}

		private bool ValidFeedbackSign(FeedBackNotify notify)
		{
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appid", this._payAccount.AppId);
			payDictionary.Add("timestamp", notify.TimeStamp);
			payDictionary.Add("openid", notify.OpenId);
			return notify.AppSignature == SignHelper.SignPay(payDictionary, "");
		}

		public PayNotify GetPayNotify(Stream inStream)
		{
			string xml = this.ReadString(inStream);
			return this.GetPayNotify(xml);
		}

		public DataTable ErrorTable(string tabName = "Notify")
		{
			return new DataTable
			{
				Columns = 
				{
					new DataColumn("OperTime"),
					new DataColumn("Error"),
					new DataColumn("Param"),
					new DataColumn("PayInfo")
				},
				TableName = tabName
			};
		}

		public void WriteLog(DataTable dt)
		{
			dt.WriteXml(HttpContext.Current.Request.MapPath("/NotifyError.xml"));
		}

		public PayNotify GetPayNotify(string xml)
		{
			DataTable dataTable = this.ErrorTable("Notify");
			DataRow dataRow = dataTable.NewRow();
			dataRow["OperTime"] = DateTime.Now;
			PayNotify result;
			try
			{
				if (string.IsNullOrEmpty(xml))
				{
					dataRow["Error"] = "InStream Null";
					dataRow["Param"] = "null";
					dataTable.Rows.Add(dataRow);
					this.WriteLog(dataTable);
					result = null;
				}
				else
				{
					PayNotify notifyObject = Utils.GetNotifyObject<PayNotify>(xml);
					WxPayData wxPayData = new WxPayData();
					try
					{
						wxPayData.FromXml(xml, this._payAccount.PartnerKey);
					}
					catch (WxPayException ex)
					{
						Utils.WxPaylog("支付出错了：" + ex.Message, "_WxPaylog.txt");
					}
					SortedDictionary<string, object> values = wxPayData.GetValues();
					if (notifyObject == null || values == null || values.Keys.Count == 0 || (values.ContainsKey("return_code") && values["return_code"].ToString() == "FAIL") || (values.ContainsKey("result_code") && values["result_code"].ToString() == "FAIL"))
					{
						Utils.WxPaylog(xml, "_WxPaylog.txt");
						result = null;
					}
					else
					{
						notifyObject.PayInfo = new PayInfo
						{
							SignType = "MD5",
							Sign = notifyObject.sign,
							TradeMode = 0,
							BankType = notifyObject.bank_type,
							BankBillNo = "",
							TotalFee = notifyObject.total_fee / 100m,
							FeeType = (notifyObject.fee_type == "CNY") ? 1 : 0,
							NotifyId = "",
							TransactionId = notifyObject.transaction_id,
							OutTradeNo = notifyObject.out_trade_no,
							TransportFee = 0m,
							ProductFee = 0m,
							Discount = 1m,
							BuyerAlias = ""
						};
						result = notifyObject;
					}
				}
			}
			catch (Exception ex2)
			{
				dataRow["Error"] = ex2.Message;
				dataRow["Param"] = xml;
				dataTable.Rows.Add(dataRow);
				this.WriteLog(dataTable);
				result = null;
			}
			return result;
		}

		public AlarmNotify GetAlarmNotify(Stream inStream)
		{
			string xml = this.ReadString(inStream);
			return this.GetAlarmNotify(xml);
		}

		public AlarmNotify GetAlarmNotify(string xml)
		{
			AlarmNotify result;
			if (string.IsNullOrEmpty(xml))
			{
				result = null;
			}
			else
			{
				AlarmNotify notifyObject = Utils.GetNotifyObject<AlarmNotify>(xml);
				if (notifyObject == null || !this.ValidAlarmSign(notifyObject))
				{
					result = null;
				}
				else
				{
					result = notifyObject;
				}
			}
			return result;
		}

		public FeedBackNotify GetFeedBackNotify(Stream inStream)
		{
			string xml = this.ReadString(inStream);
			return this.GetFeedBackNotify(xml);
		}

		public FeedBackNotify GetFeedBackNotify(string xml)
		{
			FeedBackNotify result;
			if (string.IsNullOrEmpty(xml))
			{
				result = null;
			}
			else
			{
				FeedBackNotify notifyObject = Utils.GetNotifyObject<FeedBackNotify>(xml);
				if (notifyObject == null || !this.ValidFeedbackSign(notifyObject))
				{
					result = null;
				}
				else
				{
					result = notifyObject;
				}
			}
			return result;
		}
	}
}
