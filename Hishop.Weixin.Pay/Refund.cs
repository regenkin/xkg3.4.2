using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Hishop.Weixin.Pay
{
	public class Refund
	{
		public static string SendRequest(RefundInfo info, PayConfig config, out string WxRefundNum)
		{
			WxPayData wxPayData = new WxPayData();
			if (!string.IsNullOrEmpty(info.transaction_id))
			{
				wxPayData.SetValue("transaction_id", info.transaction_id);
			}
			else
			{
				wxPayData.SetValue("out_trade_no", info.out_trade_no);
			}
			wxPayData.SetValue("total_fee", (int)info.TotalFee.Value);
			wxPayData.SetValue("refund_fee", (int)info.RefundFee.Value);
			wxPayData.SetValue("out_refund_no", info.out_refund_no);
			wxPayData.SetValue("op_user_id", config.MchID);
			if (!string.IsNullOrEmpty(config.sub_appid))
			{
				wxPayData.SetValue("sub_appid", config.sub_appid);
				wxPayData.SetValue("sub_mch_id", config.sub_mch_id);
			}
			WxPayData wxPayData2 = WxPayApi.Refund(wxPayData, config, 60);
			SortedDictionary<string, object> values = wxPayData2.GetValues();
			string result;
			if (values.ContainsKey("return_code") && values["return_code"].ToString() == "SUCCESS" && values.ContainsKey("result_code") && values["result_code"].ToString() == "SUCCESS")
			{
				WxRefundNum = "";
				result = "SUCCESS";
			}
			else
			{
				HttpService.WxDebuglog(JsonConvert.SerializeObject(values), "_wxpay.txt");
				string str = "";
				if (values.ContainsKey("err_code_des"))
				{
					str = values["err_code_des"].ToString();
				}
				if (values.ContainsKey("refund_id"))
				{
					WxRefundNum = values["refund_id"].ToString();
				}
				else
				{
					WxRefundNum = "";
				}
				result = str + values["return_msg"].ToString();
			}
			return result;
		}
	}
}
