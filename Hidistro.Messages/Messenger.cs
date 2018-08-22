using Aop.Api.Response;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Orders;
using Hishop.AlipayFuwu.Api.Model;
using Hishop.AlipayFuwu.Api.Util;
using Hishop.Plugins;
using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace Hidistro.Messages
{
	public static class Messenger
	{
		internal static bool SendMail(System.Net.Mail.MailMessage email, EmailSender sender)
		{
			string text;
			return Messenger.SendMail(email, sender, out text);
		}

		internal static bool SendMail(System.Net.Mail.MailMessage email, EmailSender sender, out string msg)
		{
			bool result;
			try
			{
				msg = "";
				result = sender.Send(email, System.Text.Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding));
			}
			catch (System.Exception ex)
			{
				msg = ex.Message;
				result = false;
			}
			return result;
		}

		internal static EmailSender CreateEmailSender(SiteSettings settings)
		{
			string text;
			return Messenger.CreateEmailSender(settings, out text);
		}

		internal static EmailSender CreateEmailSender(SiteSettings settings, out string msg)
		{
			EmailSender result;
			try
			{
				msg = "";
				if (!settings.EmailEnabled)
				{
					result = null;
				}
				else
				{
					result = EmailSender.CreateInstance(settings.EmailSender, HiCryptographer.Decrypt(settings.EmailSettings));
				}
			}
			catch (System.Exception ex)
			{
				msg = ex.Message;
				result = null;
			}
			return result;
		}

		private static TemplateMessage GenerateWeixinMessage_OrderMsg(string templateId, SiteSettings settings, OrderInfo order, string FirstData = "")
		{
			string firstProductName = new OrderDao().GetFirstProductName(order.OrderId);
			string weixinToken = settings.WeixinToken;
			return new TemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new TemplateMessage.MessagePart[]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? ("订单号：" + order.OrderId) : FirstData
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = firstProductName
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = order.GetTotal().ToString("F2")
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword3",
						Value = OrderInfo.GetOrderStatusName(order.OrderStatus)
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = ""
					}
				}
			};
		}

		private static AliTemplateMessage GenerateFuwuMessage_OrderMsg(string templateId, SiteSettings settings, OrderInfo order, string FirstData = "")
		{
			string firstProductName = new OrderDao().GetFirstProductName(order.OrderId);
			string weixinToken = settings.WeixinToken;
			return new AliTemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new AliTemplateMessage.MessagePart[]
				{
					new AliTemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? firstProductName : FirstData
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = order.OrderId
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = OrderInfo.GetOrderStatusName(order.OrderStatus)
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "remark",
						Value = "订单总金额￥" + order.GetTotal().ToString("F2")
					}
				}
			};
		}

		private static TemplateMessage GenerateWeixinMessage_RefundSuccessMsg(string templateId, SiteSettings settings, OrderInfo order, RefundInfo refundInfo, string FirstData = "")
		{
			ProductInfo productDetails = new ProductDao().GetProductDetails(refundInfo.ProductId);
			string productName = productDetails.ProductName;
			string weixinToken = settings.WeixinToken;
			TemplateMessage result;
			try
			{
				if (string.IsNullOrEmpty(refundInfo.RefundRemark))
				{
					refundInfo.RefundRemark = "";
				}
				TemplateMessage templateMessage = new TemplateMessage
				{
					Url = "",
					TemplateId = templateId,
					Touser = "",
					Data = new TemplateMessage.MessagePart[]
					{
						new TemplateMessage.MessagePart
						{
							Name = "first",
							Value = string.IsNullOrEmpty(FirstData) ? ("订单号：" + order.OrderId) : FirstData
						},
						new TemplateMessage.MessagePart
						{
							Name = "keynote1",
							Value = refundInfo.RefundMoney.ToString("f2")
						},
						new TemplateMessage.MessagePart
						{
							Name = "keynote2",
							Value = "请联系商家"
						},
						new TemplateMessage.MessagePart
						{
							Name = "keynote3",
							Value = "请联系商家"
						},
						new TemplateMessage.MessagePart
						{
							Name = "keynote4",
							Value = productName
						},
						new TemplateMessage.MessagePart
						{
							Name = "keynote5",
							Value = order.OrderId
						},
						new TemplateMessage.MessagePart
						{
							Name = "keynote6",
							Value = refundInfo.RefundRemark.Replace("\r", "").Replace("\n", "")
						}
					}
				};
				result = templateMessage;
			}
			catch (System.Exception var_12_1DC)
			{
				result = null;
			}
			return result;
		}

		private static AliTemplateMessage GenerateFuwuMessage_RefundSuccessMsg(string templateId, SiteSettings settings, OrderInfo order, RefundInfo refundInfo, string FirstData = "")
		{
			ProductInfo productDetails = new ProductDao().GetProductDetails(refundInfo.ProductId);
			string productName = productDetails.ProductName;
			string weixinToken = settings.WeixinToken;
			if (string.IsNullOrEmpty(refundInfo.RefundRemark))
			{
				refundInfo.RefundRemark = "";
			}
			return new AliTemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new AliTemplateMessage.MessagePart[]
				{
					new AliTemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? ("订单号：" + order.OrderId) : FirstData
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = refundInfo.RefundMoney.ToString("F2")
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = productName
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword3",
						Value = order.OrderId
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "remark",
						Value = refundInfo.RefundRemark.Replace("\r", "").Replace("\n", "")
					}
				}
			};
		}

		private static TemplateMessage GenerateWeixinMessage_DistributorCreateMsg(string templateId, SiteSettings settings, DistributorsInfo distributor, MemberInfo member, string FirstData = "")
		{
			return new TemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new TemplateMessage.MessagePart[]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? "您好，有一位新分销商申请了店铺" : FirstData
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = string.IsNullOrEmpty(member.UserBindName) ? member.UserName : member.UserBindName
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = member.CellPhone
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword3",
						Value = System.DateTime.Now.ToString("yyyy-MM-dd")
					}
				}
			};
		}

		private static AliTemplateMessage GenerateFuwuMessage_DistributorCreateMsg(string templateId, SiteSettings settings, DistributorsInfo distributor, MemberInfo member, string FirstData = "")
		{
			return new AliTemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new AliTemplateMessage.MessagePart[]
				{
					new AliTemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? "您好，有一位新分销商申请了店铺" : FirstData
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = member.UserName + "，" + member.CellPhone
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "remark",
						Value = string.IsNullOrEmpty(member.UserBindName) ? member.UserName : member.UserBindName
					}
				}
			};
		}

		private static TemplateMessage GenerateWeixinMessage_ServiceMsg(string templateId, SiteSettings settings, string FirstData = "", string TitleStr = "", string RemarkData = "")
		{
			string weixinToken = settings.WeixinToken;
			return new TemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new TemplateMessage.MessagePart[]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = FirstData
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = TitleStr
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = System.DateTime.Now.ToString("yyyy-MM-dd")
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = RemarkData
					}
				}
			};
		}

		private static AliTemplateMessage GenerateFuwuMessage_ServiceMsg(string templateId, SiteSettings settings, string FirstData = "", string TitleStr = "", string RemarkData = "")
		{
			return new AliTemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new AliTemplateMessage.MessagePart[]
				{
					new AliTemplateMessage.MessagePart
					{
						Name = "first",
						Value = FirstData
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = TitleStr
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "remark",
						Value = RemarkData
					}
				}
			};
		}

		private static TemplateMessage GenerateWeixinMessage_DrawCashResultMsg(string templateId, SiteSettings settings, BalanceDrawRequestInfo balance, string FirstData = "", string IsCheckDesc = "")
		{
			string weixinToken = settings.WeixinToken;
			return new TemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new TemplateMessage.MessagePart[]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? "分销商提现" : FirstData
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = balance.StoreName
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = balance.Amount.ToString("F2")
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword3",
						Value = balance.StoreName + "[" + balance.MerchantCode + "]"
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword4",
						Value = balance.RequestTime.ToString("yyyy-MM-dd")
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword5",
						Value = IsCheckDesc
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = balance.Remark
					}
				}
			};
		}

		private static AliTemplateMessage GenerateFuwuMessage_DrawCashResultMsg(string templateId, SiteSettings settings, BalanceDrawRequestInfo balance, string FirstData = "", string IsCheckDesc = "")
		{
			string weixinToken = settings.WeixinToken;
			return new AliTemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new AliTemplateMessage.MessagePart[]
				{
					new AliTemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? "分销商提现" : FirstData
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = balance.StoreName + "申请金额：￥" + balance.Amount.ToString("F2")
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "remark",
						Value = string.Concat(new string[]
						{
							"提现帐号[",
							balance.MerchantCode,
							"],当前状态：[",
							IsCheckDesc,
							"]，备注：",
							balance.Remark
						})
					}
				}
			};
		}

		private static TemplateMessage GenerateWeixinMessage_CouponWillExpiredMsg(string templateId, SiteSettings settings, CouponInfo_MemberWeiXin couponInfo, string FirstData = "")
		{
			string weixinToken = settings.WeixinToken;
			return new TemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new TemplateMessage.MessagePart[]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? "您有一张优惠券即将过期" : FirstData
					},
					new TemplateMessage.MessagePart
					{
						Name = "orderTicketStore",
						Value = couponInfo.IsAllProduct ? "全部商品" : "部分商品"
					},
					new TemplateMessage.MessagePart
					{
						Name = "orderTicketRule",
						Value = (couponInfo.ConditionValue > 0m) ? string.Format("订单满{0}元可用", couponInfo.ConditionValue.ToString("F2")) : "不限制"
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = "优惠券可以订单支付时抵扣等额现金。"
					}
				}
			};
		}

		private static AliTemplateMessage GenerateFuwuMessage_CouponWillExpiredMsg(string templateId, SiteSettings settings, CouponInfo_MemberWeiXin couponInfo, string FirstData = "")
		{
			return new AliTemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new AliTemplateMessage.MessagePart[]
				{
					new AliTemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? "您有一张优惠券即将过期" : FirstData
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = (couponInfo.ConditionValue > 0m) ? string.Format("订单满{0}元可用", couponInfo.ConditionValue.ToString("F2")) : "不限制"
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "remark",
						Value = couponInfo.IsAllProduct ? "适用全部商品" : "适用部分商品，优惠券可以订单支付时抵扣等额现金。"
					}
				}
			};
		}

		private static TemplateMessage GenerateWeixinMessage_AccountChangeMsg(string templateId, SiteSettings settings, MemberInfo member, string FirstData = "", string ChangeTypeDesc = "", string RemarkData = "")
		{
			string weixinToken = settings.WeixinToken;
			return new TemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new TemplateMessage.MessagePart[]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? "帐号更新提醒" : FirstData
					},
					new TemplateMessage.MessagePart
					{
						Name = "account",
						Value = string.IsNullOrEmpty(member.UserBindName) ? member.UserName : member.UserBindName
					},
					new TemplateMessage.MessagePart
					{
						Name = "time",
						Value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
					},
					new TemplateMessage.MessagePart
					{
						Name = "type",
						Value = ChangeTypeDesc
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = RemarkData
					}
				}
			};
		}

		private static AliTemplateMessage GenerateFuwuMessage_AccountChangeMsg(string templateId, SiteSettings settings, MemberInfo member, string FirstData = "", string ChangeTypeDesc = "", string RemarkData = "")
		{
			return new AliTemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new AliTemplateMessage.MessagePart[]
				{
					new AliTemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? "帐号更新提醒" : FirstData
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = string.IsNullOrEmpty(member.UserBindName) ? member.UserName : member.UserBindName
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "remark",
						Value = "消息类型[" + ChangeTypeDesc + "]，" + RemarkData
					}
				}
			};
		}

		private static TemplateMessage GenerateWeixinMessage_PersonalMsg(string templateId, SiteSettings settings, MemberInfo member, string FirstData = "", string ContentData = "", string title = "")
		{
			string weixinToken = settings.WeixinToken;
			return new TemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new TemplateMessage.MessagePart[]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? "个人消息通知" : FirstData
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = string.IsNullOrEmpty(title) ? "标题" : title
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = System.DateTime.Now.ToString("yyyy-MM-dd")
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword3",
						Value = string.IsNullOrEmpty(ContentData) ? "获得了奖品" : ContentData
					}
				}
			};
		}

		private static AliTemplateMessage GenerateFuwuMessage_PersonalMsg(string templateId, SiteSettings settings, MemberInfo member, string FirstData = "", string ContentData = "")
		{
			string weixinToken = settings.WeixinToken;
			return new AliTemplateMessage
			{
				Url = "",
				TemplateId = templateId,
				Touser = "",
				Data = new AliTemplateMessage.MessagePart[]
				{
					new AliTemplateMessage.MessagePart
					{
						Name = "first",
						Value = string.IsNullOrEmpty(FirstData) ? "个人消息通知" : FirstData
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = string.IsNullOrEmpty(ContentData) ? "获得了奖品" : ContentData
					},
					new AliTemplateMessage.MessagePart
					{
						Name = "remark",
						Value = ""
					}
				}
			};
		}

		public static string SendWeiXinMsg_OrderCreate(OrderInfo order)
		{
			new System.Threading.Thread(()=>
			{
				Messenger.SendFuwuMsg_OrderCreate(order);
			}).Start();
			string buyerWXOpenId;
			string salerWXOpenId;
			new OrderDao().GetOrderUserOpenId(order.OrderId, out buyerWXOpenId, out salerWXOpenId);
			string text = "OrderCreate";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			string result;
			if (messageTemplateByDetailType == null)
			{
				result = "消息模板不存在。模板：" + text;
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_OrderMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, order, "您好，订单【" + order.OrderId + "】已经提交成功。");
				Messenger.Send_WeiXin_ToMoreUser(text, buyerWXOpenId, salerWXOpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
				result = "OK";
			}
			return result;
		}

		public static string SendFuwuMsg_OrderCreate(OrderInfo order)
		{
			string buyerWXOpenId;
			string salerWXOpenId;
			new OrderDao().GetOrderUserAliOpenId(order.OrderId, out buyerWXOpenId, out salerWXOpenId);
			string text = "OrderCreate";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			string result;
			if (fuwuMessageTemplateByDetailType == null)
			{
				result = "消息模板不存在。模板：" + text;
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_OrderMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, order, "您好，订单【" + order.OrderId + "】已经提交成功。");
				Messenger.Send_Fuwu_ToMoreUser(text, buyerWXOpenId, salerWXOpenId, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
				result = "OK";
			}
			return result;
		}

		public static void SendWeiXinMsg_OrderPay(OrderInfo order)
		{
            new System.Threading.Thread(() =>
            {
                Messenger.SendFuwuMsg_OrderPay(order);
            }).Start();
			string buyerWXOpenId;
			string salerWXOpenId;
			new OrderDao().GetOrderUserOpenId(order.OrderId, out buyerWXOpenId, out salerWXOpenId);
			string text = "OrderPay";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_OrderMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, order, "您好，订单【" + order.OrderId + "】已付款成功。请等待卖家发货！");
				Messenger.Send_WeiXin_ToMoreUser(text, buyerWXOpenId, salerWXOpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_OrderPay(OrderInfo order)
		{
			string buyerWXOpenId;
			string salerWXOpenId;
			new OrderDao().GetOrderUserAliOpenId(order.OrderId, out buyerWXOpenId, out salerWXOpenId);
			string text = "OrderPay";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_OrderMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, order, "您好，订单【" + order.OrderId + "】已付款成功。请等待卖家发货！");
				Messenger.Send_Fuwu_ToMoreUser(text, buyerWXOpenId, salerWXOpenId, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_ServiceRequest(OrderInfo order, int refundType)
		{
			new System.Threading.Thread(()=>
			{
				Messenger.SendFuwuMsg_ServiceRequest(order, refundType);
			}).Start();
			string buyerWXOpenId;
			string salerWXOpenId;
			new OrderDao().GetOrderUserOpenId(order.OrderId, out buyerWXOpenId, out salerWXOpenId);
			string text = "退款";
			if (refundType == 1)
			{
				text = "退货";
			}
			string text2 = "ServiceRequest";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text2);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_ServiceMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, "买家申请" + text + "，请注意处理", "买家" + text + "申请", string.Concat(new string[]
				{
					"买家由于：",
					order.RefundRemark,
					",对订单",
					order.OrderId,
					" 申请",
					text,
					"，请及时处理。"
				}));
				Messenger.Send_WeiXin_ToMoreUser(text2, buyerWXOpenId, salerWXOpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_ServiceRequest(OrderInfo order, int refundType)
		{
			string buyerWXOpenId;
			string salerWXOpenId;
			new OrderDao().GetOrderUserAliOpenId(order.OrderId, out buyerWXOpenId, out salerWXOpenId);
			string text = "退款";
			if (refundType == 1)
			{
				text = "退货";
			}
			string text2 = "ServiceRequest";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text2);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_ServiceMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, "买家申请" + text + "，请注意处理", "买家" + text + "申请", string.Concat(new string[]
				{
					"买家由于：",
					order.RefundRemark,
					",对订单",
					order.OrderId,
					" 申请",
					text,
					"，请及时处理。"
				}));
				Messenger.Send_Fuwu_ToMoreUser(text2, buyerWXOpenId, salerWXOpenId, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_DrawCashRequest(BalanceDrawRequestInfo balance)
		{
			new System.Threading.Thread(()=>
			{
				Messenger.SendFuwuMsg_DrawCashRequest(balance);
			}).Start();
			string str = "其它";
			if (balance.RequesType == 0)
			{
				str = "微信钱包";
			}
			else if (balance.RequesType == 1)
			{
				str = "(支付宝)" + balance.MerchantCode;
			}
			else if (balance.RequesType == 2)
			{
				str = "(" + balance.BankName + ")" + balance.MerchantCode;
			}
			else if (balance.RequesType == 3)
			{
				str = "微信红包";
			}
			string text = "DrawCashRequest";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_ServiceMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, string.Concat(new string[]
				{
					"分销商",
					balance.UserName,
					"（",
					balance.StoreName,
					"）申请提现"
				}), "分销商提现申请", "申请金额：￥" + balance.Amount.ToString("F2") + "   提现账户：" + str);
				Messenger.Send_WeiXin_ToMoreUser(text, balance.UserOpenId, balance.UserOpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_DrawCashRequest(BalanceDrawRequestInfo balance)
		{
			string str = "其它";
			if (balance.RequesType == 0)
			{
				str = "微信钱包";
			}
			else if (balance.RequesType == 1)
			{
				str = "(支付宝)" + balance.MerchantCode;
			}
			else if (balance.RequesType == 2)
			{
				str = "(" + balance.BankName + ")" + balance.MerchantCode;
			}
			else if (balance.RequesType == 3)
			{
				str = "微信红包";
			}
			string text = "DrawCashRequest";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_ServiceMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, string.Concat(new string[]
				{
					"分销商",
					balance.UserName,
					"（",
					balance.StoreName,
					"）申请提现"
				}), "分销商提现申请", "申请金额：￥" + balance.Amount.ToString("F2") + "   提现账户：" + str);
				string aliOpenIDByUserId = new MemberDao().GetAliOpenIDByUserId(balance.UserId);
				Messenger.Send_Fuwu_ToMoreUser(text, aliOpenIDByUserId, aliOpenIDByUserId, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_ProductAsk(string ProductName, string SalerOpenId, string AskContent)
		{
			new System.Threading.Thread(()=>
			{
				Messenger.SendFuwuMsg_ProductAsk(ProductName, SalerOpenId, AskContent);
			}).Start();
			string text = "ProductAsk";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				AskContent = AskContent.Replace("\r", "").Replace("\n", "");
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_ServiceMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, "您有最新的商品咨询待处理", "商品咨询", "商品名称：" + ProductName + "  咨询内容：" + AskContent);
				Messenger.Send_WeiXin_ToMoreUser(text, SalerOpenId, "", messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_ProductAsk(string ProductName, string SalerOpenId, string AskContent)
		{
			string text = "ProductAsk";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AskContent = AskContent.Replace("\r", "").Replace("\n", "");
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_ServiceMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, "您有最新的商品咨询待处理", "商品咨询", "商品名称：" + ProductName + "  咨询内容：" + AskContent);
				Messenger.Send_Fuwu_ToMoreUser(text, SalerOpenId, "", fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_DistributorCreate(DistributorsInfo distributor, MemberInfo member)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_DistributorCreate(distributor, member);
			}).Start();
			string text = "DistributorCreate";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_DistributorCreateMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, distributor, member, "您好，有一位新分销商申请了店铺");
				Messenger.Send_WeiXin_ToMoreUser(text, member.OpenId, "", messageTemplateByDetailType, masterSettings, true, templateMessage);
				int num = 0;
				int distributorNumOfTotal = new MemberDao().GetDistributorNumOfTotal(member.ReferralUserId, out num);
				System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
				if (!string.IsNullOrEmpty(member.OpenId))
				{
					TemplateMessage templateMessage2 = Messenger.GenerateWeixinMessage_DistributorCreateMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, distributor, member, string.Format("恭喜您，成为{0}的第{1}位分销商", masterSettings.SiteName, distributorNumOfTotal));
					list.Add(member.OpenId);
					Messenger.Send_WeiXin_ToListUser(list, templateMessage2, masterSettings, "");
				}
				if (member.ReferralUserId > 0 && member.ReferralUserId != member.UserId)
				{
					string openIDByUserId = new MemberDao().GetOpenIDByUserId(member.ReferralUserId);
					if (!string.IsNullOrEmpty(openIDByUserId))
					{
						list.Clear();
						list.Add(openIDByUserId);
						TemplateMessage templateMessage3 = Messenger.GenerateWeixinMessage_DistributorCreateMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, distributor, member, string.Format("恭喜您，{0}成功开店，成为您的第{1}位下级分销商", member.UserName, num));
						Messenger.Send_WeiXin_ToListUser(list, templateMessage3, masterSettings, "");
					}
				}
			}
		}

		public static void SendFuwuMsg_DistributorCreate(DistributorsInfo distributor, MemberInfo member)
		{
			string buyerWXOpenId = "";
			if (member.AlipayOpenid != null)
			{
				buyerWXOpenId = member.AlipayOpenid;
			}
			else
			{
				member.AlipayOpenid = "";
			}
			string text = "DistributorCreate";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_DistributorCreateMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, distributor, member, "您好，有一位新分销商申请了店铺");
				Messenger.Send_Fuwu_ToMoreUser(text, buyerWXOpenId, "", fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
				int num = 0;
				int distributorNumOfTotal = new MemberDao().GetDistributorNumOfTotal(member.ReferralUserId, out num);
				System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
				if (!string.IsNullOrEmpty(member.AlipayOpenid))
				{
					AliTemplateMessage templateMessage2 = Messenger.GenerateFuwuMessage_DistributorCreateMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, distributor, member, string.Format("恭喜您，成为{0}的第{1}位分销商", masterSettings.SiteName, distributorNumOfTotal));
					list.Add(member.AlipayOpenid);
					Messenger.Send_Fuwu_ToListUser(list, templateMessage2);
				}
				if (member.ReferralUserId > 0 && member.ReferralUserId != member.UserId)
				{
					string aliOpenIDByUserId = new MemberDao().GetAliOpenIDByUserId(member.ReferralUserId);
					if (!string.IsNullOrEmpty(aliOpenIDByUserId))
					{
						list.Clear();
						list.Add(aliOpenIDByUserId);
						AliTemplateMessage templateMessage3 = Messenger.GenerateFuwuMessage_DistributorCreateMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, distributor, member, string.Format("恭喜您，{0}成功开店，成为您的第{1}位下级分销商", member.UserName, num));
						Messenger.Send_Fuwu_ToListUser(list, templateMessage3);
					}
				}
			}
		}

		public static void SendWeiXinMsg_OrderGetCommission(OrderInfo order, string WxOpenId, string AliOpneid, decimal CommissionAmount)
		{
			AliOHHelper.log("AliOpneid:" + AliOpneid);
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_OrderGetCommission(order, AliOpneid, CommissionAmount);
			}).Start();
			string text = "OrderGetCommission";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_OrderMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, order, "您好，分销订单" + order.OrderId + "已经完成，您获得佣金￥" + CommissionAmount.ToString("F2"));
				Messenger.Send_WeiXin_ToMoreUser(text, "", WxOpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_OrderGetCommission(OrderInfo order, string AliOpneid, decimal CommissionAmount)
		{
			string text = "OrderGetCommission";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_OrderMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, order, "您好，分销订单" + order.OrderId + "已经完成，您获得佣金￥" + CommissionAmount.ToString("F2"));
				Messenger.Send_Fuwu_ToMoreUser(text, "", AliOpneid, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_ProductCreate(string ProductName)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_ProductCreate(ProductName);
			}).Start();
			string text = "ProductCreate";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_ServiceMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, "有新品上线了。", "新品上架提醒", "商品名称：" + ProductName);
				Messenger.Send_WeiXin_ToMoreUser(text, "", "*", messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_ProductCreate(string ProductName)
		{
			string text = "ProductCreate";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_ServiceMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, "有新品上线了。", "新品上架提醒", "商品名称：" + ProductName);
				Messenger.Send_Fuwu_ToMoreUser(text, "", "*", fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_PasswordReset(MemberInfo member)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_PasswordReset(member);
			}).Start();
			string text = "PasswordReset";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_AccountChangeMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您重置了商城账户的登录密码", "密码重置", "您成功修改了账户的登录密码，请牢记。如有问题，请联系客服。");
				Messenger.Send_WeiXin_ToMoreUser(text, member.OpenId, "", messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_PasswordReset(MemberInfo member)
		{
			string text = "PasswordReset";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_AccountChangeMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您重置了商城账户的登录密码", "密码重置", "您成功修改了账户的登录密码，请牢记。如有问题，请联系客服。");
				Messenger.Send_Fuwu_ToMoreUser(text, member.AlipayOpenid, "", fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_PasswordReset(MemberInfo member, string password)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_PasswordReset(member, password);
			}).Start();
			string text = "PasswordReset";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_AccountChangeMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您重置了商城账户的登录密码 ", "密码重置", "您账户修改登录密码:" + password + "，请牢记。如有问题，请联系客服。");
				Messenger.Send_WeiXin_ToMoreUser(text, member.OpenId, "", messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_SysBindUserName(MemberInfo member, string password)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_SysBindUserName(member, password);
			}).Start();
			string text = "PasswordReset";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_AccountChangeMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "管理员为您绑定了商城账户及密码", "系统帐号绑定", "您的系统账户登录密码:" + password + "，请牢记。如有问题，请联系客服。");
				Messenger.Send_WeiXin_ToMoreUser(text, member.OpenId, "", messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_SysBindUserName(MemberInfo member, string password)
		{
			string text = "PasswordReset";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_AccountChangeMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "管理员为您绑定了商城账户及密码 ", "系统帐号绑定", "您的系统账户登录密码:" + password + "，请牢记。如有问题，请联系客服。");
				Messenger.Send_Fuwu_ToMoreUser(text, member.AlipayOpenid, "", fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_PasswordReset(MemberInfo member, string password)
		{
			string text = "PasswordReset";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_AccountChangeMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您重置了商城账户的登录密码 ", "密码重置", "您账户修改登录密码:" + password + "，请牢记。如有问题，请联系客服。");
				Messenger.Send_Fuwu_ToMoreUser(text, member.AlipayOpenid, "", fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_DistributorGradeChange(MemberInfo member, string gradeName)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_DistributorGradeChange(member, gradeName);
			}).Start();
			if (!string.IsNullOrEmpty(member.OpenId))
			{
				string text = "DistributorGradeChange";
				MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
				if (messageTemplateByDetailType != null)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_AccountChangeMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "恭喜您成功升级！", "分销商账户升级", "恭喜您成功升级为[" + gradeName + "]，您将享受到更多的分销商特权！");
					Messenger.Send_WeiXin_ToMoreUser(text, "", member.OpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
				}
			}
		}

		public static void SendFuwuMsg_DistributorGradeChange(MemberInfo member, string gradeName)
		{
			if (!string.IsNullOrEmpty(member.AlipayOpenid))
			{
				string text = "DistributorGradeChange";
				MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
				if (fuwuMessageTemplateByDetailType != null)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
					AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_AccountChangeMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "恭喜您成功升级！", "分销商账户升级", "恭喜您成功升级为[" + gradeName + "]，您将享受到更多的分销商特权！");
					Messenger.Send_Fuwu_ToMoreUser(text, "", member.AlipayOpenid, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
				}
			}
		}

		public static void SendWeiXinMsg_DrawCashRelease(BalanceDrawRequestInfo balance)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_DrawCashRelease(balance);
			}).Start();
			string text = "DrawCashRelease";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_DrawCashResultMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, balance, "分销商提现已通过", "通过");
				Messenger.Send_WeiXin_ToMoreUser(text, balance.UserOpenId, balance.UserOpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_DrawCashRelease(BalanceDrawRequestInfo balance)
		{
			string text = "DrawCashRelease";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_DrawCashResultMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, balance, "分销商提现已通过", "通过");
				string aliOpenIDByUserId = new MemberDao().GetAliOpenIDByUserId(balance.UserId);
				Messenger.Send_Fuwu_ToMoreUser(text, aliOpenIDByUserId, aliOpenIDByUserId, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_DrawCashReject(BalanceDrawRequestInfo balance)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_DrawCashReject(balance);
			}).Start();
			string text = "DrawCashReject";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				balance.Remark = "驳回原因：" + balance.Remark;
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_DrawCashResultMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, balance, "分销商提现结果被驳回", "被驳回");
				Messenger.Send_WeiXin_ToMoreUser(text, balance.UserOpenId, balance.UserOpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_DrawCashReject(BalanceDrawRequestInfo balance)
		{
			string text = "DrawCashReject";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_DrawCashResultMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, balance, "分销商提现结果被驳回", "驳回");
				string aliOpenIDByUserId = new MemberDao().GetAliOpenIDByUserId(balance.UserId);
				Messenger.Send_Fuwu_ToMoreUser(text, aliOpenIDByUserId, aliOpenIDByUserId, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_AccountLockOrUnLock(MemberInfo member, bool IsLock)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_AccountLockOrUnLock(member, IsLock);
			}).Start();
			string text = "AccountLock";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage;
				if (IsLock)
				{
					templateMessage = Messenger.GenerateWeixinMessage_AccountChangeMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您的分销商资格已被冻结", "账户冻结", "您的分销商资格已被冻结，如有疑问，请联系客服！");
				}
				else
				{
					templateMessage = Messenger.GenerateWeixinMessage_AccountChangeMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您的分销商资格已解冻", "账户解冻", "您的分销商资格账户已解冻，如有疑问，请联系客服！");
				}
				Messenger.Send_WeiXin_ToMoreUser(text, member.OpenId, member.OpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_AccountLockOrUnLock(MemberInfo member, bool IsLock)
		{
			string text = "AccountLock";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage;
				if (IsLock)
				{
					templateMessage = Messenger.GenerateFuwuMessage_AccountChangeMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您的分销商资格已被冻结", "账户冻结", "您的分销商资格已被冻结，如有疑问，请联系客服！");
				}
				else
				{
					templateMessage = Messenger.GenerateFuwuMessage_AccountChangeMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您的分销商资格已解冻", "账户解冻", "您的分销商资格账户已解冻，如有疑问，请联系客服！");
				}
				Messenger.Send_Fuwu_ToMoreUser(text, member.AlipayOpenid, member.AlipayOpenid, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_AccountLock(MemberInfo member)
		{
			Messenger.SendWeiXinMsg_AccountLockOrUnLock(member, true);
		}

		public static void SendWeiXinMsg_AccountUnLock(MemberInfo member)
		{
			Messenger.SendWeiXinMsg_AccountLockOrUnLock(member, false);
		}

		public static void SendWeiXinMsg_DistributorCancel(MemberInfo member)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_DistributorCancel(member);
			}).Start();
			string text = "DistributorCancel";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_AccountChangeMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您已经被取消分销资质", "账户被取消分销商资质", "您的分销商资格已被取消，如有疑问，请联系客服！");
				Messenger.Send_WeiXin_ToMoreUser(text, member.OpenId, member.OpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_DistributorCancel(MemberInfo member)
		{
			string text = "DistributorCancel";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_AccountChangeMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您已经被取消分销资质", "账户被取消分销商资质", "您的分销商资格已被取消，如有疑问，请联系客服！");
				Messenger.Send_Fuwu_ToMoreUser(text, member.AlipayOpenid, member.AlipayOpenid, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_OrderDeliver(OrderInfo order)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_OrderDeliver(order);
			}).Start();
			string buyerWXOpenId;
			string salerWXOpenId;
			new OrderDao().GetOrderUserOpenId(order.OrderId, out buyerWXOpenId, out salerWXOpenId);
			string text = "OrderDeliver";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_OrderMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, order, "您好，您的订单" + order.OrderId + "已经发货！");
				Messenger.Send_WeiXin_ToMoreUser(text, buyerWXOpenId, salerWXOpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_OrderDeliver(OrderInfo order)
		{
			string buyerWXOpenId;
			string salerWXOpenId;
			new OrderDao().GetOrderUserAliOpenId(order.OrderId, out buyerWXOpenId, out salerWXOpenId);
			string text = "OrderDeliver";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_OrderMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, order, "您好，您的订单" + order.OrderId + "已经发货！");
				Messenger.Send_Fuwu_ToMoreUser(text, buyerWXOpenId, salerWXOpenId, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_MemberGradeChange(MemberInfo member)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_MemberGradeChange(member);
			}).Start();
			string text = "MemberGradeChange";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_AccountChangeMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "恭喜您的会员等级升级", "账户升级", "恭喜您成功升级，您将享受到更多的会员特权！");
				Messenger.Send_WeiXin_ToMoreUser(text, member.OpenId, member.OpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_MemberGradeChange(MemberInfo member)
		{
			string text = "MemberGradeChange";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_AccountChangeMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "恭喜您的会员等级升级", "账户升级", "恭喜您成功升级，您将享受到更多的会员特权！");
				Messenger.Send_Fuwu_ToMoreUser(text, member.AlipayOpenid, member.AlipayOpenid, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static bool SendWeiXinMsg_CouponWillExpired(CouponInfo_MemberWeiXin coupon)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_CouponWillExpired(coupon);
			}).Start();
			string text = "CouponWillExpired";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			bool result;
			if (messageTemplateByDetailType == null)
			{
				result = false;
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_CouponWillExpiredMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, coupon, string.Format("您有一张优惠券将于{0}到期", coupon.EndDate.ToString("yyyy-MM-dd HH:mm:ss")));
				string token_Message = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
				bool flag = Messenger.Send_WeiXin_ToOneUser(text, token_Message, coupon.OpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
				result = flag;
			}
			return result;
		}

		public static bool SendFuwuMsg_CouponWillExpired(CouponInfo_MemberWeiXin coupon)
		{
			string detailType = "CouponWillExpired";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(detailType);
			bool result;
			if (fuwuMessageTemplateByDetailType == null)
			{
				result = false;
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_CouponWillExpiredMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, coupon, string.Format("您有一张优惠券将于{0}到期", coupon.EndDate.ToString("yyyy-MM-dd HH:mm:ss")));
				Messenger.FuwuSend(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, templateMessage);
				result = true;
			}
			return result;
		}

		public static void SendWeiXinMsg_OrderGetPoint(OrderInfo order, int integral)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_OrderGetPoint(order, integral);
			}).Start();
			string buyerWXOpenId;
			string text;
			new OrderDao().GetOrderUserOpenId(order.OrderId, out buyerWXOpenId, out text);
			string text2 = "OrderGetPoint";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text2);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_OrderMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, order, "您好，订单" + order.OrderId + "已经完成，您获得积分" + integral.ToString());
				Messenger.Send_WeiXin_ToMoreUser(text2, buyerWXOpenId, "", messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_OrderGetPoint(OrderInfo order, int integral)
		{
			string buyerWXOpenId;
			string text;
			new OrderDao().GetOrderUserAliOpenId(order.OrderId, out buyerWXOpenId, out text);
			string text2 = "OrderGetPoint";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text2);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_OrderMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, order, "您好，订单" + order.OrderId + "已经完成，您获得积分" + integral.ToString());
				Messenger.Send_Fuwu_ToMoreUser(text2, buyerWXOpenId, "", fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_OrderGetCoupon(OrderInfo order)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_OrderGetCoupon(order);
			}).Start();
			string buyerWXOpenId;
			string text;
			new OrderDao().GetOrderUserOpenId(order.OrderId, out buyerWXOpenId, out text);
			string text2 = "OrderGetCoupon";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text2);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_OrderMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, order, "您好，订单" + order.OrderId + "已经完成，您获得优惠券一张");
				Messenger.Send_WeiXin_ToMoreUser(text2, buyerWXOpenId, "", messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_OrderGetCoupon(OrderInfo order)
		{
			string buyerWXOpenId;
			string text;
			new OrderDao().GetOrderUserAliOpenId(order.OrderId, out buyerWXOpenId, out text);
			string text2 = "OrderGetCoupon";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text2);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_OrderMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, order, "您好，订单" + order.OrderId + "已经完成，您获得优惠券一张");
				Messenger.Send_Fuwu_ToMoreUser(text2, buyerWXOpenId, "", fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_RefundSuccess(RefundInfo refundInfo)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_RefundSuccess(refundInfo);
			}).Start();
			OrderInfo orderInfo = new OrderDao().GetOrderInfo(refundInfo.OrderId);
			string buyerWXOpenId;
			string text;
			new OrderDao().GetOrderUserOpenId(refundInfo.OrderId, out buyerWXOpenId, out text);
			string text2 = "RefundSuccess";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text2);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_RefundSuccessMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, orderInfo, refundInfo, "您的退款申请已经发放，敬请关注！");
				if (templateMessage != null)
				{
					Messenger.Send_WeiXin_ToMoreUser(text2, buyerWXOpenId, "", messageTemplateByDetailType, masterSettings, true, templateMessage);
				}
			}
		}

		public static void SendFuwuMsg_RefundSuccess(RefundInfo refundInfo)
		{
			OrderInfo orderInfo = new OrderDao().GetOrderInfo(refundInfo.OrderId);
			string buyerWXOpenId;
			string text;
			new OrderDao().GetOrderUserAliOpenId(refundInfo.OrderId, out buyerWXOpenId, out text);
			string text2 = "RefundSuccess";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text2);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_RefundSuccessMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, orderInfo, refundInfo, "您的退款申请已经发放，敬请关注！");
				Messenger.Send_Fuwu_ToMoreUser(text2, buyerWXOpenId, "", fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_PrizeRelease(MemberInfo member, string GameTitle)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_PrizeRelease(member, GameTitle);
			}).Start();
			if (GameTitle == null)
			{
				GameTitle = "";
			}
			string text = "PrizeRelease";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(text);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_PersonalMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您好，您的奖品已经发货！", "您参与的抽奖活动【" + GameTitle + "】所获得奖品已经发货，请注意收货", "");
				Messenger.Send_WeiXin_ToMoreUser(text, member.OpenId, member.OpenId, messageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendFuwuMsg_PrizeRelease(MemberInfo member, string GameTitle)
		{
			if (GameTitle == null)
			{
				GameTitle = "";
			}
			string text = "PrizeRelease";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(text);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_PersonalMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您好，您的奖品已经发货！", "您参与的抽奖活动【" + GameTitle + "】所获得奖品已经发货，请注意收货");
				Messenger.Send_Fuwu_ToMoreUser(text, member.AlipayOpenid, member.AlipayOpenid, fuwuMessageTemplateByDetailType, masterSettings, true, templateMessage);
			}
		}

		public static void SendWeiXinMsg_MemberRegister(MemberInfo member)
		{
			new System.Threading.Thread(() =>
            {
				Messenger.SendFuwuMsg_MemberRegister(member);
			}).Start();
			string detailType = "PrizeRelease";
			MessageTemplate messageTemplateByDetailType = MessageTemplateHelper.GetMessageTemplateByDetailType(detailType);
			if (messageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				int num = 0;
				int memberNumOfTotal = new MemberDao().GetMemberNumOfTotal(member.ReferralUserId, out num);
				System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
				if (!string.IsNullOrEmpty(member.OpenId))
				{
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_PersonalMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您好，您有新的消息！", string.Format("恭喜您，成为{0}的第{1}位会员", masterSettings.SiteName, memberNumOfTotal), "会员注册成功");
					list.Add(member.OpenId);
					Messenger.Send_WeiXin_ToListUser(list, templateMessage, masterSettings, "");
				}
				if (member.ReferralUserId > 0 && member.ReferralUserId != member.UserId)
				{
					string openIDByUserId = new MemberDao().GetOpenIDByUserId(member.ReferralUserId);
					if (!string.IsNullOrEmpty(openIDByUserId))
					{
						TemplateMessage templateMessage = Messenger.GenerateWeixinMessage_PersonalMsg(messageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "您好，您有新的消息！", string.Format("恭喜您！您成功邀请到{0}成为您店铺的第{1}位下级会员！", member.UserName, num), "下级会员注册通知");
						list.Clear();
						list.Add(openIDByUserId);
						Messenger.Send_WeiXin_ToListUser(list, templateMessage, masterSettings, "");
					}
				}
			}
		}

		public static void SendFuwuMsg_MemberRegister(MemberInfo member)
		{
			string detailType = "PrizeRelease";
			MessageTemplate fuwuMessageTemplateByDetailType = MessageTemplateHelper.GetFuwuMessageTemplateByDetailType(detailType);
			if (fuwuMessageTemplateByDetailType != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				int num = 0;
				int memberNumOfTotal = new MemberDao().GetMemberNumOfTotal(member.ReferralUserId, out num);
				System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
				if (!string.IsNullOrEmpty(member.AlipayOpenid))
				{
					AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_PersonalMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "会员注册通知", string.Format("恭喜您，成为{0}的第{1}位会员", masterSettings.SiteName, memberNumOfTotal));
					list.Add(member.AlipayOpenid);
					Messenger.Send_Fuwu_ToListUser(list, templateMessage);
				}
				if (member.ReferralUserId > 0 && member.ReferralUserId != member.UserId)
				{
					string aliOpenIDByUserId = new MemberDao().GetAliOpenIDByUserId(member.ReferralUserId);
					if (!string.IsNullOrEmpty(aliOpenIDByUserId))
					{
						AliTemplateMessage templateMessage = Messenger.GenerateFuwuMessage_PersonalMsg(fuwuMessageTemplateByDetailType.WeixinTemplateId, masterSettings, member, "下级会员注册通知", string.Format("恭喜您！您成功邀请到{0}成为您店铺的第{1}位下级会员！", member.UserName, num));
						list.Clear();
						list.Add(aliOpenIDByUserId);
						Messenger.Send_Fuwu_ToListUser(list, templateMessage);
					}
				}
			}
		}

		private static void FuwuSend(string FuwuTemplateId, SiteSettings settings, AliTemplateMessage templateMessage)
		{
			if (!string.IsNullOrWhiteSpace(FuwuTemplateId) && settings.AlipayAppid.Length > 15 && templateMessage != null)
			{
				AliOHHelper.TemplateSend(templateMessage);
			}
		}

		private static void Send(MessageTemplate template, SiteSettings settings, TemplateMessage templateMessage)
		{
			if (template.SendWeixin && !string.IsNullOrWhiteSpace(template.WeixinTemplateId) && templateMessage != null)
			{
				string token_Message = TokenApi.GetToken_Message(settings.WeixinAppId, settings.WeixinAppSecret);
				TemplateApi.SendMessage(token_Message, templateMessage);
			}
		}

		private static void Send_WeiXin_ToMoreUser(string TemplateDetailType, string BuyerWXOpenId, string SalerWXOpenId, MessageTemplate template, SiteSettings settings, bool sendFirst, TemplateMessage templateMessage)
		{
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			AliOHHelper.log(string.Concat(new string[]
			{
				"当前微信模板类型：",
				TemplateDetailType,
				",会员",
				BuyerWXOpenId,
				"|分销商",
				SalerWXOpenId,
				"|",
				templateMessage.TemplateId
			}));
			if (template.SendWeixin && !string.IsNullOrWhiteSpace(template.WeixinTemplateId) && templateMessage != null)
			{
				string token_Message = TokenApi.GetToken_Message(settings.WeixinAppId, settings.WeixinAppSecret);
				string text = "";
				if (TemplateDetailType == "OrderCreate")
				{
					text = "Msg1";
				}
				else if (TemplateDetailType == "OrderPay")
				{
					text = "Msg2";
				}
				else if (TemplateDetailType == "ServiceRequest")
				{
					text = "Msg3";
				}
				else if (TemplateDetailType == "DrawCashRequest")
				{
					text = "Msg4";
				}
				else if (TemplateDetailType == "ProductAsk")
				{
					text = "Msg5";
				}
				else if (TemplateDetailType == "DistributorCreate")
				{
					text = "Msg6";
				}
				if (text != "")
				{
					list = MessageTemplateHelper.GetAdminUserMsgList(text);
				}
				if (!string.IsNullOrEmpty(BuyerWXOpenId) && template.IsSendWeixin_ToMember)
				{
					list.Add(BuyerWXOpenId);
				}
				if (!string.IsNullOrEmpty(SalerWXOpenId) && template.IsSendWeixin_ToDistributor)
				{
					if (SalerWXOpenId == "*")
					{
						new DistributorsDao().SelectDistributorsOpenId(ref list);
					}
					else
					{
						list.Add(SalerWXOpenId);
					}
				}
				list = list.Distinct<string>().ToList<string>();
				Messenger.Send_WeiXin_ToListUser(list, templateMessage, settings, token_Message);
			}
		}

		private static void Send_WeiXin_ToListUser(System.Collections.Generic.List<string> SendToUserList, TemplateMessage templateMessage, SiteSettings settings, string accessToken = "")
		{
			if (templateMessage != null)
			{
				if (string.IsNullOrEmpty(accessToken))
				{
					accessToken = TokenApi.GetToken_Message(settings.WeixinAppId, settings.WeixinAppSecret);
				}
				foreach (string current in SendToUserList)
				{
					templateMessage.Touser = current;
					try
					{
						string text = TemplateApi.SendMessageDebug(accessToken, templateMessage);
						if (text.Contains("invalid credential"))
						{
							accessToken = TokenApi.GetToken_Message(settings.WeixinAppId, settings.WeixinAppSecret);
							text = TemplateApi.SendMessageDebug(accessToken, templateMessage);
						}
					}
					catch (System.Exception var_2_84)
					{
					}
				}
			}
		}

		private static void Send_Fuwu_ToListUser(System.Collections.Generic.List<string> SendToUserList, AliTemplateMessage templateMessage)
		{
			if (templateMessage != null)
			{
				foreach (string current in SendToUserList)
				{
					templateMessage.Touser = current;
					AliOHHelper.log(AliOHHelper.templateSendMessage(templateMessage, "查看详情"));
					try
					{
						AlipayMobilePublicMessageSingleSendResponse alipayMobilePublicMessageSingleSendResponse = AliOHHelper.TemplateSend(templateMessage);
						AliOHHelper.log(alipayMobilePublicMessageSingleSendResponse.Body);
					}
					catch (System.Exception ex)
					{
						AliOHHelper.log(ex.Message.ToString());
					}
				}
			}
		}

		private static void Send_Fuwu_ToMoreUser(string TemplateDetailType, string BuyerWXOpenId, string SalerWXOpenId, MessageTemplate template, SiteSettings settings, bool sendFirst, AliTemplateMessage templateMessage)
		{
			if (string.IsNullOrEmpty(templateMessage.TemplateId))
			{
				AliOHHelper.log("模板ID为空值,当前模板类型" + TemplateDetailType);
			}
			else
			{
				System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
				AliOHHelper.log(string.Concat(new string[]
				{
					"当前模板类型：",
					TemplateDetailType,
					",会员",
					BuyerWXOpenId,
					"|分销商",
					SalerWXOpenId,
					"|",
					templateMessage.TemplateId
				}));
				if (settings.AlipayAppid.Length > 15 && !string.IsNullOrWhiteSpace(template.WeixinTemplateId) && templateMessage != null)
				{
					string text = "";
					if (TemplateDetailType == "OrderCreate")
					{
						text = "Msg1";
					}
					else if (TemplateDetailType == "OrderPay")
					{
						text = "Msg2";
					}
					else if (TemplateDetailType == "ServiceRequest")
					{
						text = "Msg3";
					}
					else if (TemplateDetailType == "DrawCashRequest")
					{
						text = "Msg4";
					}
					else if (TemplateDetailType == "ProductAsk")
					{
						text = "Msg5";
					}
					else if (TemplateDetailType == "DistributorCreate")
					{
						text = "Msg6";
					}
					if (text != "")
					{
						list = MessageTemplateHelper.GetFuwuAdminUserMsgList(text);
					}
					if (!string.IsNullOrEmpty(BuyerWXOpenId) && template.IsSendWeixin_ToMember)
					{
						list.Add(BuyerWXOpenId);
					}
					if (!string.IsNullOrEmpty(SalerWXOpenId) && template.IsSendWeixin_ToDistributor)
					{
						if (SalerWXOpenId == "*")
						{
							new DistributorsDao().SelectDistributorsAliOpenId(ref list);
						}
						else
						{
							list.Add(SalerWXOpenId);
						}
					}
					list = list.Distinct<string>().ToList<string>();
					Messenger.Send_Fuwu_ToListUser(list, templateMessage);
				}
			}
		}

		private static bool Send_WeiXin_ToOneUser(string TemplateDetailType, string accessToken, string OpenId, MessageTemplate template, SiteSettings settings, bool sendFirst, TemplateMessage templateMessage)
		{
			bool result = false;
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			if (template.SendWeixin && !string.IsNullOrWhiteSpace(template.WeixinTemplateId) && templateMessage != null)
			{
				if (string.IsNullOrEmpty(accessToken))
				{
					accessToken = TokenApi.GetToken_Message(settings.WeixinAppId, settings.WeixinAppSecret);
				}
				list.Add(OpenId);
				foreach (string current in list)
				{
					templateMessage.Touser = current;
					try
					{
						string text = TemplateApi.SendMessageDebug(accessToken, templateMessage);
						if (text.Contains("invalid credential"))
						{
							accessToken = TokenApi.GetToken_Message(settings.WeixinAppId, settings.WeixinAppSecret);
							text = TemplateApi.SendMessageDebug(accessToken, templateMessage);
						}
						result = true;
					}
					catch (System.Exception var_4_C0)
					{
						result = false;
					}
				}
			}
			return result;
		}

		private static string GetUserCellPhone(MemberInfo user)
		{
			string result;
			if (user == null)
			{
				result = null;
			}
			else
			{
				result = user.CellPhone;
			}
			return result;
		}

		private static void GenericUserMessages(SiteSettings settings, string UserName, string userEmail, string password, string dealPassword, MessageTemplate template, out System.Net.Mail.MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
		{
			email = null;
			smsMessage = null;
			string text;
			innerMessage = (text = null);
			innerSubject = text;
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled)
				{
					email = Messenger.GenericUserEmail(template, settings, UserName, userEmail, password, dealPassword);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericUserMessageFormatter(settings, template.SMSBody, UserName, userEmail, password, dealPassword);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericUserMessageFormatter(settings, template.InnerMessageSubject, UserName, userEmail, password, dealPassword);
					innerMessage = Messenger.GenericUserMessageFormatter(settings, template.InnerMessageBody, UserName, userEmail, password, dealPassword);
				}
			}
		}

		private static System.Net.Mail.MailMessage GenericUserEmail(MessageTemplate template, SiteSettings settings, string UserName, string userEmail, string password, string dealPassword)
		{
			System.Net.Mail.MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			System.Net.Mail.MailMessage result;
			if (emailTemplate == null)
			{
				result = null;
			}
			else
			{
				emailTemplate.Subject = Messenger.GenericUserMessageFormatter(settings, emailTemplate.Subject, UserName, userEmail, password, dealPassword);
				emailTemplate.Body = Messenger.GenericUserMessageFormatter(settings, emailTemplate.Body, UserName, userEmail, password, dealPassword);
				result = emailTemplate;
			}
			return result;
		}

		private static string GenericUserMessageFormatter(SiteSettings settings, string stringToFormat, string UserName, string userEmail, string password, string dealPassword)
		{
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$UserName$", UserName.Trim());
			stringToFormat = stringToFormat.Replace("$Email$", userEmail.Trim());
			stringToFormat = stringToFormat.Replace("$Password$", password);
			stringToFormat = stringToFormat.Replace("$DealPassword$", dealPassword);
			return stringToFormat;
		}

		private static void GenericOrderMessages(SiteSettings settings, string UserName, string userEmail, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason, MessageTemplate template, out System.Net.Mail.MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
		{
			email = null;
			smsMessage = null;
			string text;
			innerMessage = (text = null);
			innerSubject = text;
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled)
				{
					email = Messenger.GenericOrderEmail(template, settings, UserName, userEmail, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericOrderMessageFormatter(settings, UserName, template.SMSBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericOrderMessageFormatter(settings, UserName, template.InnerMessageSubject, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
					innerMessage = Messenger.GenericOrderMessageFormatter(settings, UserName, template.InnerMessageBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				}
			}
		}

		private static System.Net.Mail.MailMessage GenericOrderEmail(MessageTemplate template, SiteSettings settings, string UserName, string userEmail, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason)
		{
			System.Net.Mail.MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			System.Net.Mail.MailMessage result;
			if (emailTemplate == null)
			{
				result = null;
			}
			else
			{
				emailTemplate.Subject = Messenger.GenericOrderMessageFormatter(settings, UserName, emailTemplate.Subject, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				emailTemplate.Body = Messenger.GenericOrderMessageFormatter(settings, UserName, emailTemplate.Body, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				result = emailTemplate;
			}
			return result;
		}

		private static string GenericOrderMessageFormatter(SiteSettings settings, string UserName, string stringToFormat, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason)
		{
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$UserName$", UserName);
			stringToFormat = stringToFormat.Replace("$OrderId$", orderId);
			stringToFormat = stringToFormat.Replace("$Total$", total.ToString("F"));
			stringToFormat = stringToFormat.Replace("$Memo$", memo);
			stringToFormat = stringToFormat.Replace("$Shipping_Type$", shippingType);
			stringToFormat = stringToFormat.Replace("$Shipping_Name$", shippingName);
			stringToFormat = stringToFormat.Replace("$Shipping_Addr$", shippingAddress);
			stringToFormat = stringToFormat.Replace("$Shipping_Zip$", shippingZip);
			stringToFormat = stringToFormat.Replace("$Shipping_Phone$", shippingPhone);
			stringToFormat = stringToFormat.Replace("$Shipping_Cell$", shippingCell);
			stringToFormat = stringToFormat.Replace("$Shipping_Email$", shippingEmail);
			stringToFormat = stringToFormat.Replace("$Shipping_Billno$", shippingBillno);
			stringToFormat = stringToFormat.Replace("$RefundMoney$", refundMoney.ToString("F"));
			stringToFormat = stringToFormat.Replace("$CloseReason$", closeReason);
			return stringToFormat;
		}
	}
}
