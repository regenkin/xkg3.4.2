using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.Vshop;
using Hishop.Plugins;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Trade
{
	[PrivilegeCheck(Privilege.EditOrders)]
	public class SendOrderGoods : AdminPage
	{
		protected string orderIds = string.Empty;

		protected string ReUrl = Globals.RequestQueryStr("reurl");

		protected string type = "sendorders";

		protected System.Web.UI.WebControls.Repeater rptItemList;

		protected System.Web.UI.WebControls.Literal litOrdersCount;

		protected SendOrderGoods() : base("m03", "ddp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Globals.RequestQueryStr("type") == "saveorders")
			{
				this.type = "saveorders";
			}
			string text = Globals.RequestFormStr("posttype");
			this.orderIds = Globals.RequestQueryStr("OrderId").Trim(new char[]
			{
				','
			});
			if (string.IsNullOrEmpty(this.ReUrl))
			{
				this.ReUrl = "manageorder.aspx";
			}
			string a;
			if ((a = text) != null)
			{
				if (a == "saveorders")
				{
					string text2 = Globals.RequestFormStr("data");
					base.Response.ContentType = "application/json";
					string s = "{\"type\":\"0\",\"tips\":\"指定物流失败！\"}";
					JArray jArray = (JArray)JsonConvert.DeserializeObject(text2);
					string text3 = string.Empty;
					if (jArray != null)
					{
						if (jArray.Count > 1)
						{
							text3 = "批量";
						}
						bool flag = true;
						using (System.Collections.Generic.IEnumerator<JToken> enumerator = jArray.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								JObject jObject = (JObject)enumerator.Current;
								if (!this.CheckOrderCompany(jObject["orderid"].ToString(), jObject["companycode"].ToString(), jObject["compname"].ToString(), jObject["shipordernumber"].ToString()))
								{
									flag = false;
								}
							}
						}
						if (flag)
						{
							using (System.Collections.Generic.IEnumerator<JToken> enumerator2 = jArray.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									JObject jObject2 = (JObject)enumerator2.Current;
									OrderHelper.UpdateOrderCompany(jObject2["orderid"].ToString(), jObject2["companycode"].ToString(), jObject2["compname"].ToString(), jObject2["shipordernumber"].ToString());
								}
							}
							s = "{\"type\":\"1\",\"tips\":\"" + text3 + "指定物流成功！\"}";
						}
						else
						{
							s = "{\"type\":\"0\",\"tips\":\"" + text3 + "指定物流失败，请检测数据的正确性！\"}";
						}
					}
					base.Response.Write(s);
					base.Response.End();
					return;
				}
				if (a == "saveoneorders")
				{
					string text2 = Globals.RequestFormStr("data");
					base.Response.ContentType = "application/json";
					string s = "{\"type\":\"0\",\"tips\":\"指定物流失败！\"}";
					JArray jArray = (JArray)JsonConvert.DeserializeObject(text2);
					string text3 = string.Empty;
					if (jArray != null)
					{
						bool flag2 = true;
						string shipNumber = "1111111111";
						using (System.Collections.Generic.IEnumerator<JToken> enumerator3 = jArray.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								JObject jObject3 = (JObject)enumerator3.Current;
								if (!this.CheckOrderCompany(jObject3["orderid"].ToString(), jObject3["companycode"].ToString(), jObject3["compname"].ToString(), shipNumber))
								{
									flag2 = false;
								}
							}
						}
						if (flag2)
						{
							using (System.Collections.Generic.IEnumerator<JToken> enumerator4 = jArray.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									JObject jObject4 = (JObject)enumerator4.Current;
									OrderHelper.UpdateOrderCompany(jObject4["orderid"].ToString(), jObject4["companycode"].ToString(), jObject4["compname"].ToString(), "");
								}
							}
							s = "{\"type\":\"1\",\"tips\":\"" + text3 + "指定物流成功！\"}";
						}
						else
						{
							s = "{\"type\":\"0\",\"tips\":\"" + text3 + "指定物流失败，请检测数据的正确性！\"}";
						}
					}
					base.Response.Write(s);
					base.Response.End();
					return;
				}
				if (a == "sendorders")
				{
					string text2 = Globals.RequestFormStr("data");
					base.Response.ContentType = "application/json";
					string s = "{\"type\":\"0\",\"tips\":\"发货失败！\"}";
					JArray jArray = (JArray)JsonConvert.DeserializeObject(text2);
					string text3 = string.Empty;
					if (jArray != null)
					{
						if (jArray.Count > 1)
						{
							text3 = "批量";
						}
						bool flag3 = true;
						using (System.Collections.Generic.IEnumerator<JToken> enumerator5 = jArray.GetEnumerator())
						{
							while (enumerator5.MoveNext())
							{
								JObject jObject5 = (JObject)enumerator5.Current;
								if (!this.CheckOrderCompany(jObject5["orderid"].ToString(), jObject5["companycode"].ToString(), jObject5["compname"].ToString(), jObject5["shipordernumber"].ToString()))
								{
									flag3 = false;
								}
							}
						}
						if (flag3)
						{
							int num = 0;
							using (System.Collections.Generic.IEnumerator<JToken> enumerator6 = jArray.GetEnumerator())
							{
								while (enumerator6.MoveNext())
								{
									JObject jObject6 = (JObject)enumerator6.Current;
									OrderInfo orderInfo = OrderHelper.GetOrderInfo(jObject6["orderid"].ToString());
									if ((orderInfo.GroupBuyId <= 0 || orderInfo.GroupBuyStatus == GroupBuyStatus.Success) && ((orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && orderInfo.Gateway == "hishop.plugins.payment.podrequest") || orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid) && !string.IsNullOrEmpty(jObject6["shipordernumber"].ToString().Trim()) && jObject6["shipordernumber"].ToString().Trim().Length <= 30)
									{
										orderInfo.ExpressCompanyAbb = jObject6["companycode"].ToString();
										orderInfo.ExpressCompanyName = jObject6["compname"].ToString();
										orderInfo.ShipOrderNumber = jObject6["shipordernumber"].ToString();
										if (OrderHelper.SendGoods(orderInfo))
										{
											Express.SubscribeExpress100(jObject6["companycode"].ToString(), jObject6["shipordernumber"].ToString());
											SendNoteInfo sendNoteInfo = new SendNoteInfo();
											sendNoteInfo.NoteId = Globals.GetGenerateId() + num;
											sendNoteInfo.OrderId = jObject6["orderid"].ToString();
											sendNoteInfo.Operator = ManagerHelper.GetCurrentManager().UserName;
											sendNoteInfo.Remark = "后台" + sendNoteInfo.Operator + "发货成功";
											OrderHelper.SaveSendNote(sendNoteInfo);
											if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
											{
												if (orderInfo.Gateway == "hishop.plugins.payment.ws_wappay.wswappayrequest")
												{
													PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
													if (paymentMode != null)
													{
														PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
														{
															paymentMode.Gateway
														})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
														{
															paymentMode.Gateway
														})), "");
														paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
													}
												}
												if (orderInfo.Gateway == "hishop.plugins.payment.weixinrequest")
												{
													SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
													PayClient payClient;
													if (masterSettings.EnableSP)
													{
														payClient = new PayClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.Main_PayKey, true, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID);
													}
													else
													{
														payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, false, "", "");
													}
													payClient.DeliverNotify(new DeliverInfo
													{
														TransId = orderInfo.GatewayOrderId,
														OutTradeNo = orderInfo.OrderId,
														OpenId = MemberHelper.GetMember(orderInfo.UserId).OpenId
													});
												}
											}
											orderInfo.OnDeliver();
											num++;
										}
									}
								}
							}
							if (num == 0)
							{
								s = "{\"type\":\"0\",\"tips\":\"" + text3 + "发货失败！\"}";
							}
							else
							{
								s = string.Concat(new object[]
								{
									"{\"type\":\"1\",\"tips\":\"",
									text3,
									"发货成功！发货数量",
									num,
									"个\"}"
								});
							}
						}
						else
						{
							s = "{\"type\":\"0\",\"tips\":\"" + text3 + "发货失败，请检测数据的正确性！\"}";
						}
					}
					base.Response.Write(s);
					base.Response.End();
					return;
				}
				if (a == "getcompany")
				{
					base.Response.ContentType = "application/json";
					string text2 = "[{\"type\":\"0\",\"data\":[]}]";
					System.Collections.Generic.IList<ExpressCompanyInfo> allExpress = ExpressHelper.GetAllExpress();
					int num2 = 0;
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					foreach (ExpressCompanyInfo current in allExpress)
					{
						if (num2 == 0)
						{
							stringBuilder.Append(string.Concat(new string[]
							{
								"{\"code\":\"",
								SendOrderGoods.String2Json(current.Kuaidi100Code),
								"\",\"name\":\"",
								SendOrderGoods.String2Json(current.Name),
								"\"}"
							}));
						}
						else
						{
							stringBuilder.Append(string.Concat(new string[]
							{
								",{\"code\":\"",
								SendOrderGoods.String2Json(current.Kuaidi100Code),
								"\",\"name\":\"",
								SendOrderGoods.String2Json(current.Name),
								"\"}"
							}));
						}
						num2++;
					}
					if (!string.IsNullOrEmpty(stringBuilder.ToString()))
					{
						text2 = "[{\"type\":\"1\",\"data\":[" + stringBuilder.ToString() + "]}]";
					}
					base.Response.Write(text2);
					base.Response.End();
					return;
				}
			}
			if (string.IsNullOrEmpty(this.orderIds))
			{
				base.GotoResourceNotFound();
				return;
			}
			string[] array = this.orderIds.Split(new char[]
			{
				','
			});
			bool flag4 = true;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string lstr = array2[i];
				if (!Globals.IsOrdersID(lstr))
				{
					flag4 = false;
					break;
				}
			}
			if (flag4)
			{
				System.Data.DataSet ordersByOrderIDList = OrderHelper.GetOrdersByOrderIDList(this.orderIds);
				this.rptItemList.DataSource = ordersByOrderIDList;
				this.rptItemList.DataBind();
				this.litOrdersCount.Text = ordersByOrderIDList.Tables[0].Rows.Count.ToString();
				return;
			}
			base.Response.Write("非法参数请求！");
			base.Response.End();
		}

		public bool CheckOrderCompany(string oid, string companycode, string companyname, string shipNumber)
		{
			bool result = true;
			if (string.IsNullOrEmpty(oid) || string.IsNullOrEmpty(companycode) || string.IsNullOrEmpty(companyname) || string.IsNullOrEmpty(shipNumber))
			{
				result = false;
			}
			return result;
		}

		private static string String2Json(string s)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			int i = 0;
			while (i < s.Length)
			{
				char c = s.ToCharArray()[i];
				char c2 = c;
				if (c2 <= '"')
				{
					switch (c2)
					{
					case '\b':
						stringBuilder.Append("\\b");
						break;
					case '\t':
						stringBuilder.Append("\\t");
						break;
					case '\n':
						stringBuilder.Append("\\n");
						break;
					case '\v':
						goto IL_C0;
					case '\f':
						stringBuilder.Append("\\f");
						break;
					case '\r':
						stringBuilder.Append("\\r");
						break;
					default:
						if (c2 != '"')
						{
							goto IL_C0;
						}
						stringBuilder.Append("\\\"");
						break;
					}
				}
				else if (c2 != '/')
				{
					if (c2 != '\\')
					{
						goto IL_C0;
					}
					stringBuilder.Append("\\\\");
				}
				else
				{
					stringBuilder.Append("\\/");
				}
				IL_C8:
				i++;
				continue;
				IL_C0:
				stringBuilder.Append(c);
				goto IL_C8;
			}
			return stringBuilder.ToString();
		}
	}
}
