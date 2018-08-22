using ControlPanel.Promotions;
using Hidistro.ControlPanel.Bargain;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

namespace Hidistro.UI.Web.Pay
{
	public class wx_Submit : System.Web.UI.Page
	{
		public string pay_json = string.Empty;

		public string CheckValue = "";

		public int shareid;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.QueryString.Get("orderId");
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			System.Collections.Generic.List<OrderInfo> orderMarkingOrderInfo = ShoppingProcessor.GetOrderMarkingOrderInfo(text);
			if (orderMarkingOrderInfo.Count == 0)
			{
				return;
			}
			decimal d = 0m;
			foreach (OrderInfo current in orderMarkingOrderInfo)
			{
				if (current.BargainDetialId > 0)
				{
					string text2 = BargainHelper.IsCanBuyByBarginDetailId(current.BargainDetialId);
					if (text2 != "1")
					{
						current.OrderStatus = OrderStatus.Closed;
						current.CloseReason = text2;
						OrderHelper.UpdateOrder(current);
						base.Response.Write("<script>alert('" + text2 + "，订单自动关闭！');location.href='/Vshop/MemberOrders.aspx'</script>");
						base.Response.End();
						return;
					}
				}
				else
				{
					foreach (LineItemInfo current2 in current.LineItems.Values)
					{
						if (!ProductHelper.GetProductHasSku(current2.SkuId, current2.Quantity))
						{
							current.OrderStatus = OrderStatus.Closed;
							current.CloseReason = "库存不足";
							OrderHelper.UpdateOrder(current);
							base.Response.Write("<script>alert('库存不足，订单自动关闭！');location.href='/Vshop/MemberOrders.aspx'</script>");
							base.Response.End();
							return;
						}
					}
				}
				d += current.GetTotal();
			}
			PackageInfo packageInfo = new PackageInfo();
			packageInfo.Body = text;
			packageInfo.NotifyUrl = string.Format("http://{0}/pay/wx_Pay.aspx", base.Request.Url.Host);
			packageInfo.OutTradeNo = text;
			packageInfo.TotalFee = (int)(d * 100m);
			if (packageInfo.TotalFee < 1m)
			{
				packageInfo.TotalFee = 1m;
			}
			string openId = "";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember != null)
			{
				openId = currentMember.OpenId;
			}
			packageInfo.OpenId = openId;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			PayClient payClient;
			if (masterSettings.EnableSP)
			{
				payClient = new PayClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.Main_PayKey, true, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID);
			}
			else
			{
				payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, false, "", "");
			}
			if (payClient.checkSetParams(out this.CheckValue))
			{
				if (!payClient.checkPackage(packageInfo, out this.CheckValue))
				{
					return;
				}
				PayRequestInfo payRequestInfo = payClient.BuildPayRequest(packageInfo);
				this.pay_json = this.ConvertPayJson(payRequestInfo);
				if (!payRequestInfo.package.ToLower().StartsWith("prepay_id=wx"))
				{
					this.CheckValue = payRequestInfo.package;
				}
				System.Data.DataTable shareActivity = ShareActHelper.GetShareActivity();
				int num = 0;
				decimal d2 = 0m;
				if (shareActivity.Rows.Count > 0)
				{
					for (int i = 0; i < shareActivity.Rows.Count; i++)
					{
						if (d != 0m && d >= decimal.Parse(shareActivity.Rows[shareActivity.Rows.Count - 1]["MeetValue"].ToString()))
						{
							num = int.Parse(shareActivity.Rows[shareActivity.Rows.Count - 1]["Id"].ToString());
							d2 = decimal.Parse(shareActivity.Rows[shareActivity.Rows.Count - 1]["MeetValue"].ToString());
							break;
						}
						if (d != 0m && d <= decimal.Parse(shareActivity.Rows[0]["MeetValue"].ToString()))
						{
							num = int.Parse(shareActivity.Rows[0]["Id"].ToString());
							d2 = decimal.Parse(shareActivity.Rows[0]["MeetValue"].ToString());
							break;
						}
						if (d != 0m && d >= decimal.Parse(shareActivity.Rows[i]["MeetValue"].ToString()))
						{
							num = int.Parse(shareActivity.Rows[i]["Id"].ToString());
							d2 = decimal.Parse(shareActivity.Rows[i]["MeetValue"].ToString());
						}
					}
					if (d >= d2)
					{
						this.shareid = num;
					}
				}
				return;
			}
		}

		public string ConvertPayJson(PayRequestInfo req)
		{
			string str = "{";
			str = str + "\"appId\":\"" + req.appId + "\",";
			str = str + "\"timeStamp\":\"" + req.timeStamp + "\",";
			str = str + "\"nonceStr\":\"" + req.nonceStr + "\",";
			str = str + "\"package\":\"" + req.package + "\",";
			str = str + "\"signType\":\"" + req.signType + "\",";
			str = str + "\"paySign\":\"" + req.paySign + "\"";
			return str + "}";
		}
	}
}
