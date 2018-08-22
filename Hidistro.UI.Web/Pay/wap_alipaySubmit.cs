using Hidistro.ControlPanel.Bargain;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.OutPay.App;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Pay
{
	public class wap_alipaySubmit : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl infos;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.QueryString.Get("orderId");
			string text2 = base.Request.QueryString.Get("Ptype");
			if (string.IsNullOrEmpty(text))
			{
				this.infos.InnerText = "订单号为空，请返回";
				return;
			}
			if (!string.IsNullOrEmpty(text2) && text2 == "OneTao")
			{
				this.DoOneTaoPay(text);
				base.Response.End();
			}
			System.Collections.Generic.List<OrderInfo> orderMarkingOrderInfo = ShoppingProcessor.GetOrderMarkingOrderInfo(text);
			if (orderMarkingOrderInfo.Count == 0)
			{
				this.infos.InnerText = "订单信息未找到！";
				return;
			}
			decimal num = 0m;
			foreach (OrderInfo current in orderMarkingOrderInfo)
			{
				if (current.BargainDetialId > 0)
				{
					string text3 = BargainHelper.IsCanBuyByBarginDetailId(current.BargainDetialId);
					if (text3 != "1")
					{
						current.OrderStatus = OrderStatus.Closed;
						current.CloseReason = text3;
						OrderHelper.UpdateOrder(current);
						base.Response.Write("<script>alert('" + text3 + "，订单自动关闭！');location.href='/Vshop/MemberOrders.aspx'</script>");
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
				num += current.GetTotal();
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			string alipay_Pid = masterSettings.Alipay_Pid;
			string alipay_Key = masterSettings.Alipay_Key;
			string text4 = "utf-8";
            Hidistro.ControlPanel.OutPay.App.Core.setConfig(alipay_Pid, "MD5", alipay_Key, text4);
			string value = "1";
			string value2 = Globals.FullPath("/Pay/wap_alipay_return_url.aspx");
			string value3 = Globals.FullPath("/Pay/wap_alipay_notify_url.aspx");
			string value4 = text;
			string value5 = "订单支付";
			decimal num2 = num;
			string value6 = Globals.FullPath("/Vshop/MemberOrderDetails.aspx?orderId=") + orderMarkingOrderInfo[0].OrderId;
			string value7 = "订单号-" + orderMarkingOrderInfo[0].OrderId + " ...";
			string value8 = "1m";
			string value9 = "";
			string s = Hidistro.ControlPanel.OutPay.App.Core.BuildRequest(new System.Collections.Generic.SortedDictionary<string, string>
			{
				{
					"partner",
					alipay_Pid
				},
				{
					"seller_id",
					alipay_Pid
				},
				{
					"_input_charset",
					text4
				},
				{
					"service",
					"alipay.wap.create.direct.pay.by.user"
				},
				{
					"payment_type",
					value
				},
				{
					"notify_url",
					value3
				},
				{
					"return_url",
					value2
				},
				{
					"out_trade_no",
					value4
				},
				{
					"subject",
					value5
				},
				{
					"total_fee",
					num2.ToString("F")
				},
				{
					"show_url",
					value6
				},
				{
					"body",
					value7
				},
				{
					"it_b_pay",
					value8
				},
				{
					"extern_token",
					value9
				}
			}, "get", "确认");
			base.Response.Write(s);
		}

		private void DoOneTaoPay(string Pid)
		{
			if (string.IsNullOrEmpty(Pid))
			{
				base.Response.Write("支付参数不正确！<a href='javascript:history.go(-1);'>返回上一页</a>");
				base.Response.End();
			}
			OneyuanTaoParticipantInfo addParticipant = OneyuanTaoHelp.GetAddParticipant(0, Pid, "");
			if (addParticipant == null)
			{
				base.Response.Write("您的夺宝信息已被删除！<a href='javascript:history.go(-1);'>返回上一页</a>");
				base.Response.End();
			}
			string activityId = addParticipant.ActivityId;
			OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(activityId);
			if (oneyuanTaoInfoById == null)
			{
				base.Response.Write("夺宝活动已被删除，你无法完成支付！<a href='javascript:history.go(-1);'>返回上一页</a>");
				base.Response.End();
			}
			OneTaoState oneTaoState = OneyuanTaoHelp.getOneTaoState(oneyuanTaoInfoById);
			if (oneTaoState != OneTaoState.进行中)
			{
				base.Response.Write("当前活动" + oneTaoState.ToString() + "，无法支付！<a href='javascript:history.go(-1);'>返回上一页</a>");
				base.Response.End();
			}
			if (oneyuanTaoInfoById.FinishedNum + addParticipant.BuyNum > oneyuanTaoInfoById.ReachNum)
			{
				base.Response.Write("活动已满员，您可以试下其它活动！<a href='/Vshop/OneyuanList.aspx'>夺宝活动中心</a>");
				base.Response.End();
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			string alipay_Pid = masterSettings.Alipay_Pid;
			string alipay_Key = masterSettings.Alipay_Key;
			string text = "utf-8";
            Hidistro.ControlPanel.OutPay.App.Core.setConfig(alipay_Pid, "MD5", alipay_Key, text);
			string value = "1";
			string value2 = Globals.FullPath("/Vshop/OneTaoPaySuccess.aspx");
			string value3 = Globals.FullPath("/Pay/wap_alipay_notify_url.aspx");
			string value4 = "订单支付";
			decimal totalPrice = addParticipant.TotalPrice;
			string value5 = Globals.FullPath("/Vshop/OneTaoPayView.aspx?Pid=") + Pid;
			string value6 = "一元夺宝支付，当前支付编号-" + Pid + " ...";
			string value7 = "1m";
			string value8 = "";
			string s = Hidistro.ControlPanel.OutPay.App.Core.BuildRequest(new System.Collections.Generic.SortedDictionary<string, string>
			{
				{
					"partner",
					alipay_Pid
				},
				{
					"seller_id",
					alipay_Pid
				},
				{
					"_input_charset",
					text
				},
				{
					"service",
					"alipay.wap.create.direct.pay.by.user"
				},
				{
					"payment_type",
					value
				},
				{
					"notify_url",
					value3
				},
				{
					"return_url",
					value2
				},
				{
					"out_trade_no",
					Pid
				},
				{
					"subject",
					value4
				},
				{
					"total_fee",
					totalPrice.ToString("F")
				},
				{
					"show_url",
					value5
				},
				{
					"body",
					value6
				},
				{
					"it_b_pay",
					value7
				},
				{
					"extern_token",
					value8
				}
			}, "get", "确认");
			base.Response.Write(s);
		}
	}
}
