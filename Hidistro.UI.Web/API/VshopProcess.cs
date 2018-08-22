using Aop.Api.Response;
using ControlPanel.Promotions;
using ControlPanel.WeiXin;
using Hidistro.ControlPanel.Bargain;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Settings;
using Hidistro.ControlPanel.Store;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Bargain;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.FenXiao;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Settings;
using Hidistro.Entities.StatisticsReport;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.Vshop;
using Hishop.AlipayFuwu.Api.Model;
using Hishop.AlipayFuwu.Api.Util;
using Hishop.Weixin.MP.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.API
{
	public class VshopProcess : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
	{
		private StatisticNotifier myNotifier = new StatisticNotifier();

		private UpdateStatistics myEvent = new UpdateStatistics();

		private int buyAmount;

		private string productSku;

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			string text = context.Request["action"];
			string key;
			switch (key = text)
			{
			case "GetOrderItemStatus":
				this.GetOrderItemStatus(context);
				return;
			case "AddParticipant":
				this.AddParticipant(context);
				return;
			case "GetWinXinInfo":
				this.GetWinXinInfo(context);
				return;
			case "followCheck":
				this.followCheck(context);
				return;
			case "OperateAllDistributorProducts":
				this.OperateAllDistributorProducts(context);
				return;
			case "GetDrawStatus":
				this.GetDrawRemarks(context);
				return;
			case "CheckCoupon":
				this.CheckCoupon(context);
				return;
			case "SignToday":
				this.SignToday(context);
				return;
			case "ConfirmPrizeArriver":
				this.ConfirmPrizeArriver(context);
				return;
			case "ConfirmOneyuangPrizeAddr":
				this.ConfirmOneyuangPrizeAddr(context);
				return;
			case "ConfirmPrizeAddr":
				this.ConfirmPrizeAddr(context);
				return;
			case "AddToCartBySkus":
				this.ProcessAddToCartBySkus(context);
				return;
			case "GetSkuByOptions":
				this.ProcessGetSkuByOptions(context);
				return;
			case "DeleteCartProduct":
				this.ProcessDeleteCartProduct(context);
				return;
			case "ChageQuantity":
				this.ProcessChageQuantity(context);
				return;
			case "SubmitMemberCard":
				this.ProcessSubmitMemberCard(context);
				return;
			case "AddShippingAddress":
				this.AddShippingAddress(context);
				return;
			case "DelShippingAddress":
				this.DelShippingAddress(context);
				return;
			case "SetDefaultShippingAddress":
				this.SetDefaultShippingAddress(context);
				return;
			case "UpdateShippingAddress":
				this.UpdateShippingAddress(context);
				return;
			case "Vote":
				this.Vote(context);
				return;
			case "SetUserName":
				this.SetUserName(context);
				return;
			case "Submmitorder":
				this.ProcessSubmmitorder(context);
				return;
			case "CancelOrder":
				this.CancelOrder(context);
				return;
			case "FinishOrder":
				this.FinishOrder(context);
				return;
			case "RequestReturn":
				this.RequestReturn(context);
				return;
			case "AddProductConsultations":
				this.AddProductConsultations(context);
				return;
			case "AddProductReview":
				this.AddProductReview(context);
				return;
			case "AddFavorite":
				this.AddFavorite(context);
				return;
			case "DelFavorite":
				this.DelFavorite(context);
				return;
			case "CheckFavorite":
				this.CheckFavorite(context);
				return;
			case "ProcessAddToCartByExchange":
				this.ProcessAddToCartByExchange(context);
				return;
			case "Logistic":
				this.SearchExpressData(context);
				return;
			case "GetShippingTypes":
				this.GetShippingTypes(context);
				return;
			case "UserLogin":
				this.UserLogin(context);
				return;
			case "RegisterUser":
				this.RegisterUser(context);
				return;
			case "BindUserName":
				this.BindUserName(context);
				return;
			case "BindOldUserName":
				this.BindOldUserName(context);
				return;
			case "AddDistributor":
				this.AddDistributor(context);
				return;
			case "SetDistributorMsg":
				this.SetDistributorMsg(context);
				return;
			case "DeleteProducts":
				this.DeleteDistributorProducts(context);
				return;
			case "AddDistributorProducts":
				this.AddDistributorProducts(context);
				return;
			case "UpdateDistributor":
				this.UpdateDistributor(context);
				return;
			case "AddCommissions":
				this.AddCommissions(context);
				return;
			case "AdjustCommissions":
				this.AdjustCommissions(context);
				return;
			case "EditPassword":
				this.EditPassword(context);
				return;
			case "GetOrderRedPager":
				this.GetOrderRedPager(context);
				return;
			case "countfreight":
				this.countfreight(context);
				return;
			case "checkdistribution":
				this.checkdistribution(context);
				return;
			case "countfreighttype":
				this.countfreighttype(context);
				return;
			case "getqrcodescaninfo":
				this.GetQRCodeScanInfo(context);
				return;
			case "getalifuwuqrcodescaninfo":
				this.GetAliFuWuQRCodeScanInfo(context);
				return;
			case "clearqrcodescaninfo":
				this.ClearQRCodeScanInfo(context);
				return;
			case "CombineOrders":
				this.CombineOrders(context);
				return;
			case "AddCustomDistributorStatistic":
				this.AddCustomDistributorStatistic(context);
				return;
			case "GetCustomDistributorStatistic":
				this.GetCustomDistributorStatistic(context);
				return;
			case "GetMyMember":
				this.GetMyMember(context);
				return;
			case "GetNotice":
				this.GetNotice(context);
				return;
			case "GetMyDistributors":
				this.GetMyDistributors(context);
				return;
			case "GetSecondDistributors":
				this.GetSecondDistributors(context);
				return;
			case "GetBargainCount":
				this.GetBargainCount(context);
				return;
			case "GetBargainList":
				this.GetBargainList(context);
				return;
			case "OpenBargain":
				this.OpenBargain(context);
				return;
			case "HelpBargain":
				this.HelpBargain(context);
				return;
			case "LoadHelpBargainDetial":
				this.LoadHelpBargainDetial(context);
				return;
			case "LoadMoreHelpBargainDetial":
				this.LoadMoreHelpBargainDetial(context);
				return;
			case "ExistsBargainDetial":
				this.ExistsBargainDetial(context);
				return;
			case "GetBargain":
				this.GetBargain(context);
				return;
			case "HelpBargainDetial":
				this.HelpBargainDetial(context);
				return;
			case "ExistsHelpBargainDetial":
				this.ExistsHelpBargainDetial(context);
				return;
			case "GetStatisticalData":
				this.GetStatisticalData(context);
				return;
			case "UpdateBargainEndDate":
				this.UpdateBargainEndDate(context);
				break;

				return;
			}
		}

		private void UpdateBargainEndDate(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int bargainId = System.Convert.ToInt32(context.Request["BargainId"]);
			System.DateTime endDate = System.DateTime.Parse(context.Request["EndDate"]);
			string s;
			if (BargainHelper.UpdateBargain(bargainId, endDate))
			{
				s = "{\"success\":1, \"msg\":\"修改成功\"}";
			}
			else
			{
				s = "{\"success\":0, \"msg\":\"修改失败\"}";
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetStatisticalData(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int bargainId = System.Convert.ToInt32(context.Request["BargainId"]);
			BargainStatisticalData bargainStatisticalDataInfo = BargainHelper.GetBargainStatisticalDataInfo(bargainId);
			string s = "";
			if (bargainStatisticalDataInfo != null)
			{
				int num = bargainStatisticalDataInfo.ActivityStock - bargainStatisticalDataInfo.ActivitySales;
				string text = "0";
				if (bargainStatisticalDataInfo.ActivitySales > 0)
				{
					text = (bargainStatisticalDataInfo.AverageTransactionPrice / bargainStatisticalDataInfo.ActivitySales).ToString("f2");
				}
				s = string.Concat(new object[]
				{
					"{\"success\":1, \"NumberOfParticipants\":\"",
					bargainStatisticalDataInfo.NumberOfParticipants,
					"\", \"SingleMember\":\"",
					bargainStatisticalDataInfo.SingleMember,
					"\", \"ActivitySales\":\"",
					bargainStatisticalDataInfo.ActivitySales,
					"\", \"SurplusInventory\":\"",
					num,
					"\", \"AverageTransactionPrice\":\"",
					text,
					"\", \"ActiveState\":\"",
					bargainStatisticalDataInfo.ActiveState,
					"\"}"
				});
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private void ExistsHelpBargainDetial(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int bargainDetialId = System.Convert.ToInt32(context.Request["BargainDetialId"]);
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			string s = "";
			if (currentMember != null)
			{
				HelpBargainDetialInfo helpBargainDetialInfo = BargainHelper.GeHelpBargainDetialInfo(bargainDetialId, currentMember.UserId);
				if (helpBargainDetialInfo != null)
				{
					s = "{\"success\":1, \"msg\":\"已经存在\"}";
				}
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetBargain(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int id = System.Convert.ToInt32(context.Request["BargainId"]);
			string s = "";
			BargainInfo bargainInfo = BargainHelper.GetBargainInfo(id);
			if (bargainInfo != null)
			{
				s = string.Concat(new string[]
				{
					"{\"success\":1, \"Title\":\"",
					bargainInfo.Title,
					"\", \"ActivityCover\":\"",
					bargainInfo.ActivityCover,
					"\", \"Remarks\":\"",
					bargainInfo.Remarks,
					"\"}"
				});
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private void ExistsBargainDetial(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = System.Convert.ToInt32(context.Request["BargainId"]);
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			string s = "";
			if (currentMember != null)
			{
				int userId = currentMember.UserId;
				BargainDetialInfo bargainDetialInfo = BargainHelper.GetBargainDetialInfo(num, userId);
				string text = "img";
				if (bargainDetialInfo != null)
				{
					BargainInfo bargainInfo = BargainHelper.GetBargainInfo(num);
					decimal floorPrice = bargainInfo.FloorPrice;
					decimal num2 = bargainInfo.InitialPrice - floorPrice;
					decimal d;
					if (num2 > 0m)
					{
						d = (bargainInfo.InitialPrice - bargainDetialInfo.Price) / num2;
					}
					else
					{
						d = 1m;
					}
					ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(bargainInfo.ProductId);
					int number = bargainDetialInfo.Number;
					string sku = bargainDetialInfo.Sku;
					string text2 = this.LoadHelpBargainDetial(bargainDetialInfo.Id);
					if (BargainHelper.ActionIsEnd(bargainDetialInfo.Id))
					{
						text = "order";
					}
					if (bargainInfo.ActivityStock <= bargainInfo.TranNumber || bargainInfo.EndDate <= System.DateTime.Now)
					{
						text = "end";
					}
					s = string.Concat(new object[]
					{
						"{\"success\":1,\"status\":\"",
						text,
						"\",\"SaleStatus\":\"",
						productBaseInfo.SaleStatus,
						"\",\"ProductId\":\"",
						bargainInfo.ProductId,
						"\", \"Price\":\"",
						bargainDetialInfo.Price.ToString("f2"),
						"\", \"progress\":\"",
						(int)(d * 100m),
						"\", \"BargainDetialId\":\"",
						bargainDetialInfo.Id,
						"\", \"Number\":\"",
						number,
						"\",\"Sku\":\"",
						sku,
						"\", \"BargainDetialHtml\":\"",
						text2,
						"\"}"
					});
				}
				else
				{
					s = "{\"success\":0, \"Price\":\"0\"}";
				}
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private void HelpBargainDetial(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int id = System.Convert.ToInt32(context.Request["BargainDetialId"]);
			int id2 = System.Convert.ToInt32(context.Request["BargainId"]);
			BargainDetialInfo bargainDetialInfo = BargainHelper.GetBargainDetialInfo(id);
			string s;
			if (bargainDetialInfo != null)
			{
				BargainInfo bargainInfo = BargainHelper.GetBargainInfo(id2);
				ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(bargainInfo.ProductId);
				string text = productBaseInfo.SaleStatus.ToString();
				decimal floorPrice = bargainInfo.FloorPrice;
				decimal d = (bargainInfo.InitialPrice - bargainDetialInfo.Price) / (bargainInfo.InitialPrice - floorPrice);
				int number = bargainDetialInfo.Number;
				string sku = bargainDetialInfo.Sku;
				string text2 = this.LoadHelpBargainDetial(bargainDetialInfo.Id);
				if (BargainHelper.ActionIsEnd(bargainDetialInfo.Id))
				{
					text = "order";
				}
				s = string.Concat(new object[]
				{
					"{\"success\":1,\"SaleStatus\":\"",
					text,
					"\",\"ProductId\":\"",
					bargainInfo.ProductId,
					"\", \"Price\":\"",
					bargainDetialInfo.Price.ToString("f2"),
					"\", \"progress\":\"",
					(int)(d * 100m),
					"\", \"BargainDetialId\":\"",
					bargainDetialInfo.Id,
					"\", \"Number\":\"",
					number,
					"\",\"Sku\":\"",
					sku,
					"\", \"BargainDetialHtml\":\"",
					text2,
					"\"}"
				});
			}
			else
			{
				BargainInfo bargainInfo2 = BargainHelper.GetBargainInfo(id2);
				ProductInfo productInfo = null;
				if (bargainInfo2 != null)
				{
					productInfo = ProductHelper.GetProductBaseInfo(bargainInfo2.ProductId);
				}
				s = "{\"success\":0, \"Price\":\"0\", \"SaleStatus\":\"" + productInfo.SaleStatus + "\"}";
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private string LoadHelpBargainDetial(int bargainDetialId)
		{
			System.Data.DataTable helpBargainDetials = BargainHelper.GetHelpBargainDetials(bargainDetialId);
			int helpBargainDetialCount = BargainHelper.GetHelpBargainDetialCount(bargainDetialId);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("<h6>已有" + helpBargainDetialCount + "位好友帮忙砍价</h6>");
			stringBuilder.Append("<ul>");
			if (helpBargainDetials.Rows.Count > 0)
			{
				int num = 0;
				while (num < helpBargainDetials.Rows.Count && num != 2)
				{
					stringBuilder.Append("<li class='clearfix'>");
					stringBuilder.Append(string.Concat(new string[]
					{
						"<p class='fl'><span class='colorl'>",
						helpBargainDetials.Rows[num]["UserName"].ToString(),
						"</span>, 价格砍掉￥",
						string.IsNullOrEmpty(helpBargainDetials.Rows[num]["BargainPrice"].ToString()) ? "0.00" : string.Format("{0:F2}", helpBargainDetials.Rows[num]["BargainPrice"]),
						"</p>"
					}));
					stringBuilder.Append("<p class='fr colorc'>" + System.DateTime.Parse(helpBargainDetials.Rows[num]["CreateDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
					stringBuilder.Append("</li>");
					num++;
				}
				stringBuilder.Append("</ul>");
				stringBuilder.Append("<p class='look-all'>");
				if (helpBargainDetials.Rows.Count > 2)
				{
					stringBuilder.Append("<span class='fr colorl' onclick='LoadMore()'>显示更多&#62;&#62;</span>");
				}
				stringBuilder.Append("</p>");
			}
			return stringBuilder.ToString();
		}

		private void LoadMoreHelpBargainDetial(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int bargainDetialId = System.Convert.ToInt32(context.Request["BargainDetialId"]);
			System.Data.DataTable helpBargainDetials = BargainHelper.GetHelpBargainDetials(bargainDetialId);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("<h6>已有" + helpBargainDetials.Rows.Count + "位好友帮忙砍价</h6>");
			stringBuilder.Append("<ul>");
			string s;
			if (helpBargainDetials.Rows.Count > 0)
			{
				for (int i = 0; i < helpBargainDetials.Rows.Count; i++)
				{
					stringBuilder.Append("<li class='clearfix'>");
					stringBuilder.Append(string.Concat(new string[]
					{
						"<p class='fl'><span class='colorl'>",
						helpBargainDetials.Rows[i]["UserName"].ToString(),
						"</span>, 价格砍掉￥",
						string.IsNullOrEmpty(helpBargainDetials.Rows[i]["BargainPrice"].ToString()) ? "0.00" : string.Format("{0:F2}", helpBargainDetials.Rows[i]["BargainPrice"]),
						"</p>"
					}));
					stringBuilder.Append("<p class='fr colorc'>" + System.DateTime.Parse(helpBargainDetials.Rows[i]["CreateDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
					stringBuilder.Append("</li>");
				}
				stringBuilder.Append("</ul>");
				stringBuilder.Append("<p class='look-all'><span class='fr colorl'></span></p>");
				s = "{\"success\":1, \"msg\":\"" + stringBuilder.ToString() + "\"}";
			}
			else
			{
				s = "{\"success\":0, \"msg\":\"暂无数据\"}";
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private void LoadHelpBargainDetial(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int bargainDetialId = System.Convert.ToInt32(context.Request["BargainDetialId"]);
			System.Convert.ToInt32(context.Request["Size"]);
			System.Data.DataTable helpBargainDetials = BargainHelper.GetHelpBargainDetials(bargainDetialId);
			BargainHelper.GetHelpBargainDetialCount(bargainDetialId);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("<h6>已有" + helpBargainDetials.Rows.Count + "位好友帮忙砍价</h6>");
			stringBuilder.Append("<ul>");
			string s;
			if (helpBargainDetials.Rows.Count > 0)
			{
				for (int i = 0; i < helpBargainDetials.Rows.Count; i++)
				{
					stringBuilder.Append("<li class='clearfix'>");
					stringBuilder.Append(string.Concat(new string[]
					{
						"<p class='fl'><span class='colorl'>",
						helpBargainDetials.Rows[i]["UserName"].ToString(),
						"</span>价格砍掉￥",
						string.IsNullOrEmpty(helpBargainDetials.Rows[i]["BargainPrice"].ToString()) ? "0.00" : string.Format("{0:f2}", helpBargainDetials.Rows[i]["BargainPrice"].ToString()),
						"</p>"
					}));
					stringBuilder.Append("<p class='fr colorc'>" + System.DateTime.Parse(helpBargainDetials.Rows[i]["CreateDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
					stringBuilder.Append("</li>");
				}
				stringBuilder.Append("</ul>");
				if (helpBargainDetials.Rows.Count > 2)
				{
					stringBuilder.Append("<p class='look-all'><span class='fr colorl'>显示更多&#62;&#62;</span></p>");
				}
				s = "{\"success\":1, \"msg\":\"" + stringBuilder.ToString() + "\"}";
			}
			else
			{
				s = "{\"success\":0, \"msg\":\"暂无数据\"}";
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private void HelpBargain(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string userAgent = context.Request.UserAgent;
			if (!userAgent.ToLower().Contains("micromessenger") && !userAgent.ToLower().Contains("alipay"))
			{
				context.Response.Write("{\"success\":3, \"msg\":\"砍价只能在微信端商城或支付宝服务窗进行\"}");
				return;
			}
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				context.Response.Write("{\"success\":0, \"msg\":\"请先登录\"}");
				return;
			}
			int bargainId = System.Convert.ToInt32(context.Request["BargainId"]);
			int bargainDetialId = System.Convert.ToInt32(context.Request["BargainDetialId"]);
			HelpBargainDetialInfo helpBargainDetialInfo = new HelpBargainDetialInfo();
			helpBargainDetialInfo.BargainDetialId = bargainDetialId;
			helpBargainDetialInfo.BargainId = bargainId;
			helpBargainDetialInfo.UserId = currentMember.UserId;
			helpBargainDetialInfo.CreateDate = System.DateTime.Now;
			if (BargainHelper.ActionIsEnd(bargainDetialId))
			{
				context.Response.Write("{\"success\":6, \"msg\":\"商品已经下单了，谢谢您的参与\"}");
				return;
			}
			decimal bargainPrice = this.GetBargainPrice(bargainId, bargainDetialId);
			if (bargainPrice == 0m)
			{
				context.Response.Write("{\"success\":4, \"msg\":\"商品已经砍到底价了，谢谢您的参与\"}");
				return;
			}
			helpBargainDetialInfo.BargainPrice = bargainPrice;
			if (BargainHelper.ExistsHelpBargainDetial(helpBargainDetialInfo))
			{
				context.Response.Write("{\"success\":1, \"msg\":\"你已经帮忙砍过了\"}");
				return;
			}
			string text = BargainHelper.InsertHelpBargainDetial(helpBargainDetialInfo);
			if (text == "1")
			{
				string s = "{\"success\":2, \"msg\":\"您帮忙砍了" + helpBargainDetialInfo.BargainPrice + "元.\"}";
				context.Response.Write(s);
				context.Response.End();
				return;
			}
			context.Response.Write("{\"success\":5, \"msg\":\"" + text + "\"}");
		}

		private decimal GetBargainPrice(int bargainId, int bargainDetialId)
		{
			decimal num = 0m;
			if (bargainId > 0)
			{
				BargainInfo bargainInfo = BargainHelper.GetBargainInfo(bargainId);
				BargainDetialInfo bargainDetialInfo = BargainHelper.GetBargainDetialInfo(bargainDetialId);
				if (bargainInfo != null)
				{
					if (bargainInfo.BargainType == 0)
					{
						num = (decimal)bargainInfo.BargainTypeMinVlue;
					}
					else
					{
						int maxValue = (int)(bargainInfo.BargainTypeMaxVlue * 100f);
						int minValue = (int)(bargainInfo.BargainTypeMinVlue * 100f);
						System.Random random = new System.Random();
						float num2 = (float)random.Next(minValue, maxValue);
						decimal num3 = (decimal)num2 / 100m;
						num = num3;
					}
				}
				if (bargainDetialInfo.Price - num < bargainInfo.FloorPrice)
				{
					num = bargainDetialInfo.Price - bargainInfo.FloorPrice;
				}
			}
			return num;
		}

		private void OpenBargain(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				context.Response.Write("{\"success\":0, \"msg\":\"请先登录\"}");
				return;
			}
			int num = System.Convert.ToInt32(context.Request["BargainId"]);
			int number = System.Convert.ToInt32(context.Request["number"]);
			string sku = context.Request["sku"];
			string s = "";
			int num2 = 0;
			BargainInfo bargainInfo = BargainHelper.GetBargainInfo(num);
			if (bargainInfo != null)
			{
				string text = BargainHelper.IsCanBuyByBarginId(num);
				if (text == "1")
				{
					bool flag = BargainHelper.InsertBargainDetial(new BargainDetialInfo
					{
						BargainId = num,
						UserId = currentMember.UserId,
						Number = number,
						Sku = sku,
						Price = bargainInfo.InitialPrice,
						CreateDate = System.DateTime.Now
					}, out num2);
					HelpBargainDetialInfo helpBargainDetialInfo = new HelpBargainDetialInfo();
					helpBargainDetialInfo.BargainDetialId = num2;
					helpBargainDetialInfo.BargainId = num;
					helpBargainDetialInfo.UserId = currentMember.UserId;
					helpBargainDetialInfo.CreateDate = System.DateTime.Now;
					helpBargainDetialInfo.BargainPrice = this.GetBargainPrice(num, num2);
					if (flag)
					{
						string a = BargainHelper.InsertHelpBargainDetial(helpBargainDetialInfo);
						if (a == "1")
						{
							s = string.Concat(new object[]
							{
								"{\"success\":\"1\",\"bargainDetialId\":\"",
								num2,
								"\",\"msg\":\"发起成功,自己砍掉",
								helpBargainDetialInfo.BargainPrice.ToString("f2"),
								"元,请邀请好友砍价.\"}"
							});
						}
						else
						{
							s = "{\"success\":\"2\",\"msg\":\"添加失败\"}";
						}
					}
					else
					{
						s = "{\"success\":\"2\",\"msg\":\"添加失败\"}";
					}
				}
				else
				{
					s = "{\"success\":\"2\",\"msg\":\"发起砍价失败，" + text + "！\"}";
				}
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetBargainList(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Data.DataTable dataTable = new System.Data.DataTable();
			string type = context.Request["status"];
			int pageIndex = int.Parse(context.Request["pageIndex"]) + 1;
			BargainQuery bargainQuery = new BargainQuery();
			bargainQuery.Type = type;
			bargainQuery.PageIndex = pageIndex;
			int total = BargainHelper.GetTotal(bargainQuery);
			dataTable = (System.Data.DataTable)BargainHelper.GetBargainList(bargainQuery).Data;
			string liHtml = this.GetLiHtml(dataTable);
			string s = string.Concat(new object[]
			{
				"{\"success\":\"true\",\"rowtotal\":\"",
				dataTable.Rows.Count,
				"\",\"total\":\"",
				total,
				"\",\"lihtml\":\"",
				liHtml,
				"\"}"
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private string GetLiHtml(System.Data.DataTable dt)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (dt != null && dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					stringBuilder.Append("<li>");
					stringBuilder.Append("<div class='shopimg'>");
					stringBuilder.Append("<img src='" + (string.IsNullOrEmpty(dt.Rows[i]["ThumbnailUrl60"].ToString()) ? "/utility/pics/none.gif" : dt.Rows[i]["ThumbnailUrl60"].ToString()) + "'  style='width:600px;height:200px'>");
					stringBuilder.Append("<p class='mask'>");
					stringBuilder.Append("<span class='fl'>" + dt.Rows[i]["ProductName"].ToString() + "</span>");
					stringBuilder.Append("<span class='fr'>原价：￥" + (string.IsNullOrEmpty(dt.Rows[i]["SalePrice"].ToString()) ? "0.00" : string.Format("{0:f2}", dt.Rows[i]["SalePrice"].ToString())) + "</span>");
					stringBuilder.Append("</p>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("<div class='bargain-info'>");
					stringBuilder.Append("<p>砍至底价：<strong class='colorr'>￥" + (string.IsNullOrEmpty(dt.Rows[i]["FloorPrice"].ToString()) ? "0.00" : string.Format("{0:F2}", dt.Rows[i]["FloorPrice"].ToString())) + "</strong></p>");
					stringBuilder.Append("<p>结束时间：" + System.DateTime.Parse(dt.Rows[i]["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
					stringBuilder.Append(BargainHelper.GetLinkHtml(dt.Rows[i]["id"].ToString(), dt.Rows[i]["bargainstatus"].ToString(), "0", "0"));
					stringBuilder.Append("</div>");
					stringBuilder.Append("</li>");
				}
			}
			return stringBuilder.ToString();
		}

		private void GetBargainCount(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Data.DataTable allBargain = BargainHelper.GetAllBargain();
			int count = allBargain.Rows.Count;
			int num = allBargain.Select(string.Concat(new object[]
			{
				" BeginDate< '",
				System.DateTime.Now,
				"' AND EndDate> '",
				System.DateTime.Now,
				"'"
			})).Length;
			int num2 = allBargain.Select("BeginDate>'" + System.DateTime.Now + "'").Length;
			int num3 = allBargain.Select(" EndDate< '" + System.DateTime.Now + "'").Length;
			string s = string.Concat(new object[]
			{
				"{\"type\":\"1\",\"allCount\":",
				count,
				",\"ingCount\":",
				num,
				",\"unbegunCount\":",
				num2,
				",\"endCount\":",
				num3,
				"}"
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetSecondDistributors(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int gradeId = int.Parse(context.Request["GradeId"]);
			int pageIndex = int.Parse(context.Request["PageIndex"]) + 1;
			int num = int.Parse(context.Request["ReferralId"]);
			DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId(), true);
			int pageSize = 10;
			DistributorsQuery distributorsQuery = new DistributorsQuery();
			distributorsQuery.GradeId = gradeId;
			distributorsQuery.PageIndex = pageIndex;
			distributorsQuery.UserId = currentDistributors.UserId;
			distributorsQuery.ReferralPath = num.ToString();
			distributorsQuery.PageSize = pageSize;
			int num2 = 0;
			System.Data.DataTable downDistributors = DistributorsBrower.GetDownDistributors(distributorsQuery, out num2);
			string secondDistributorsHtml = this.GetSecondDistributorsHtml(downDistributors);
			string s = string.Empty;
			if (downDistributors.Rows.Count > 0)
			{
				s = string.Concat(new object[]
				{
					"{\"success\":\"true\",\"rowtotal\":\"",
					downDistributors.Rows.Count,
					"\",\"total\":\"",
					num2,
					"\",\"lihtml\":\"",
					secondDistributorsHtml,
					"\"}"
				});
			}
			else
			{
				s = "{\"success\":\"false\"}";
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetMyDistributors(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int gradeId = int.Parse(context.Request["GradeId"]);
			int pageIndex = int.Parse(context.Request["PageIndex"]) + 1;
			int num = int.Parse(context.Request["ReferralId"]);
			DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId(), true);
			int pageSize = 10;
			DistributorsQuery distributorsQuery = new DistributorsQuery();
			distributorsQuery.GradeId = gradeId;
			distributorsQuery.PageIndex = pageIndex;
			distributorsQuery.UserId = currentDistributors.UserId;
			distributorsQuery.ReferralPath = num.ToString();
			distributorsQuery.PageSize = pageSize;
			int num2 = 0;
			System.Data.DataTable downDistributors = DistributorsBrower.GetDownDistributors(distributorsQuery, out num2);
			string myDistributorsHtml = this.GetMyDistributorsHtml(downDistributors);
			string s = string.Empty;
			if (downDistributors.Rows.Count > 0)
			{
				s = string.Concat(new object[]
				{
					"{\"success\":\"true\",\"rowtotal\":\"",
					downDistributors.Rows.Count,
					"\",\"total\":\"",
					num2,
					"\",\"lihtml\":\"",
					myDistributorsHtml,
					"\"}"
				});
			}
			else
			{
				s = "{\"success\":\"false\"}";
			}
			context.Response.Write(s);
			context.Response.End();
		}

		public string GetSecondDistributorsHtml(System.Data.DataTable dt)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					stringBuilder.Append("<li>");
					stringBuilder.Append(string.Concat(new string[]
					{
						" <h3> ",
						dt.Rows[i]["StoreName"].ToString(),
						"【",
						dt.Rows[i]["GradeName"].ToString(),
						"】</h3>"
					}));
					stringBuilder.Append("<div class='userinfobox'>");
					stringBuilder.Append("<div class='userimg'>");
					stringBuilder.Append("<img src='" + (string.IsNullOrEmpty(dt.Rows[i]["Logo"].ToString()) ? "/templates/common/images/user.png" : dt.Rows[i]["Logo"].ToString()) + "'>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("<div class='usertextinfo clearfix'>");
					stringBuilder.Append("<div class='left'>");
					stringBuilder.Append("<p><span class='colorc'>用户呢称：</span>" + dt.Rows[i]["UserName"].ToString() + "</p>");
					stringBuilder.Append("<p><span class='colorc'>申请时间：</span>" + (string.IsNullOrEmpty(dt.Rows[i]["CreateTime"].ToString()) ? "" : System.DateTime.Parse(dt.Rows[i]["CreateTime"].ToString()).ToString("yyyy-MM-dd")) + "</p>");
					stringBuilder.Append(string.Concat(new string[]
					{
						"<p><a href='ChirldrenDistributorDetials.aspx?distributorId=",
						dt.Rows[i]["UserId"].ToString(),
						"'><span class='colorc'>销售总额：</span><span class='colorg'>",
						string.IsNullOrEmpty(dt.Rows[i]["OrderTotal"].ToString()) ? "0.00" : string.Format("{0:F2}", dt.Rows[i]["OrderTotal"].ToString()),
						"</span></a></p>"
					}));
					stringBuilder.Append("</div>");
					stringBuilder.Append("<div class='right'>");
					stringBuilder.Append("<p><span class='colorc'>上级店铺：</span><span >" + dt.Rows[i]["LStoreName"].ToString() + "</span></p>");
					stringBuilder.Append("<p><span class='colorc'>下级会员：</span>" + dt.Rows[i]["MemberTotal"].ToString() + " 位</p>");
					stringBuilder.Append(string.Concat(new string[]
					{
						"<p><a href='ChirldrenDistributorDetials.aspx?distributorId=",
						dt.Rows[i]["UserId"].ToString(),
						"'><span class='colorc'>贡献佣金：</span><span class='colorg'>￥",
						string.IsNullOrEmpty(dt.Rows[i]["CommTotal"].ToString()) ? "0.00" : string.Format("{0:F2}", dt.Rows[i]["CommTotal"].ToString()),
						"</span></a></p>"
					}));
					stringBuilder.Append("</div>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("<span class='left radius'></span>");
					stringBuilder.Append("<span class='right radius'></span>");
					stringBuilder.Append("</li>");
				}
			}
			return stringBuilder.ToString();
		}

		public string GetMyDistributorsHtml(System.Data.DataTable dt)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					stringBuilder.Append("<li>");
					stringBuilder.Append(string.Concat(new string[]
					{
						" <h3> ",
						dt.Rows[i]["StoreName"].ToString(),
						"【",
						dt.Rows[i]["GradeName"].ToString(),
						"】</h3>"
					}));
					stringBuilder.Append("<div class='userinfobox'>");
					stringBuilder.Append("<div class='userimg'>");
					stringBuilder.Append("<img src='" + (string.IsNullOrEmpty(dt.Rows[i]["Logo"].ToString()) ? "/templates/common/images/user.png" : dt.Rows[i]["Logo"].ToString()) + "'>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("<div class='usertextinfo clearfix'>");
					stringBuilder.Append("<div class='left'>");
					stringBuilder.Append("<p><span class='colorc'>用户呢称：</span>" + dt.Rows[i]["UserName"].ToString() + "</p>");
					stringBuilder.Append("<p><span class='colorc'>申请时间：</span>" + (string.IsNullOrEmpty(dt.Rows[i]["CreateTime"].ToString()) ? "" : System.DateTime.Parse(dt.Rows[i]["CreateTime"].ToString()).ToString("yyyy-MM-dd")) + "</p>");
					stringBuilder.Append(string.Concat(new string[]
					{
						"<p><a href='ChirldrenDistributorDetials.aspx?distributorId=",
						dt.Rows[i]["UserId"].ToString(),
						"'><span class='colorc'>销售总额：</span><span class='colorg'>",
						string.IsNullOrEmpty(dt.Rows[i]["OrderTotal"].ToString()) ? "0.00" : string.Format("{0:F2}", dt.Rows[i]["OrderTotal"].ToString()),
						"</span></a></p>"
					}));
					stringBuilder.Append("</div>");
					stringBuilder.Append("<div class='right'>");
					stringBuilder.Append(string.Concat(new string[]
					{
						"<p><a href='ChirldrenDistributorStores.aspx?UserId=",
						dt.Rows[i]["UserId"].ToString(),
						"'><span class='colorc'>下级分店：</span><span class='colorg'>",
						dt.Rows[i]["disTotal"].ToString(),
						" 家</span></a></p>"
					}));
					stringBuilder.Append("<p><span class='colorc'>下级会员：</span>" + dt.Rows[i]["MemberTotal"].ToString() + " 位</p>");
					stringBuilder.Append(string.Concat(new string[]
					{
						"<p><a href='ChirldrenDistributorDetials.aspx?distributorId=",
						dt.Rows[i]["UserId"].ToString(),
						"'><span class='colorc'>贡献佣金：</span><span class='colorg'>￥",
						string.IsNullOrEmpty(dt.Rows[i]["CommTotal"].ToString()) ? "0.00" : string.Format("{0:F2}", dt.Rows[i]["CommTotal"].ToString()),
						"</span></a></p>"
					}));
					stringBuilder.Append("</div>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("<span class='left radius'></span>");
					stringBuilder.Append("<span class='right radius'></span>");
					stringBuilder.Append("</li>");
				}
			}
			return stringBuilder.ToString();
		}

		private void GetMyMember(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int referralUserId = int.Parse(context.Request["UserId"]);
			int pageIndex = int.Parse(context.Request["PageIndex"]) + 1;
			int pageSize = 10;
			int num = 0;
			System.Data.DataTable membersByUserId = MemberProcessor.GetMembersByUserId(referralUserId, pageIndex, pageSize, out num);
			string myMemberHtml = this.GetMyMemberHtml(membersByUserId);
			string s = string.Empty;
			if (membersByUserId.Rows.Count > 0)
			{
				s = string.Concat(new object[]
				{
					"{\"success\":\"true\",\"rowtotal\":\"",
					membersByUserId.Rows.Count,
					"\",\"total\":\"",
					num,
					"\",\"lihtml\":\"",
					myMemberHtml,
					"\"}"
				});
			}
			else
			{
				s = "{\"success\":\"false\"}";
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetNotice(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string s = "{\"success\":\"false\"}";
			int sendType = Globals.RequestFormNum("type");
			int num = Globals.RequestFormNum("readtype");
			int num2 = Globals.RequestFormNum("page");
			int num3 = Globals.RequestFormNum("pagesize");
			if (num3 < 5)
			{
				num3 = 10;
			}
			if (num2 < 1)
			{
				num2 = 1;
			}
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember != null)
			{
				int userId = currentMember.UserId;
				DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(userId);
				NoticeQuery noticeQuery = new NoticeQuery();
				noticeQuery.SortBy = "ID";
				noticeQuery.SortOrder = SortAction.Desc;
				Globals.EntityCoding(noticeQuery, true);
				noticeQuery.PageIndex = num2;
				noticeQuery.PageSize = num3;
				noticeQuery.UserId = new int?(userId);
				noticeQuery.SendType = sendType;
				noticeQuery.IsDistributor = new bool?(distributorInfo != null);
				noticeQuery.IsPub = new int?(1);
				noticeQuery.IsDel = new int?(0);
				if (num == 1)
				{
					noticeQuery.IsNotShowRead = new int?(1);
				}
				DbQueryResult noticeRequest = NoticeBrowser.GetNoticeRequest(noticeQuery);
				object data = noticeRequest.Data;
				if (data != null)
				{
					System.Data.DataTable dataTable = (System.Data.DataTable)data;
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					int count = dataTable.Rows.Count;
					if (count > 0)
					{
						int i = 0;
						stringBuilder.Append(string.Concat(new string[]
						{
							"{\"id\":",
							dataTable.Rows[i]["ID"].ToString(),
							",\"isread\":",
							NoticeBrowser.IsView(currentMember.UserId, Globals.ToNum(dataTable.Rows[i]["ID"])) ? "1" : "0",
							",\"title\":\"",
							Globals.String2Json(dataTable.Rows[i]["title"].ToString()),
							"\",\"pubtime\":\"",
							System.DateTime.Parse(dataTable.Rows[i]["PubTime"].ToString()).ToString("yyyy-MM-dd"),
							"\"}"
						}));
						for (i = 1; i < count; i++)
						{
							stringBuilder.Append(string.Concat(new string[]
							{
								",{\"id\":",
								dataTable.Rows[i]["ID"].ToString(),
								",\"isread\":",
								NoticeBrowser.IsView(currentMember.UserId, Globals.ToNum(dataTable.Rows[i]["ID"])) ? "1" : "0",
								",\"title\":\"",
								Globals.String2Json(dataTable.Rows[i]["title"].ToString()),
								"\",\"pubtime\":\"",
								System.DateTime.Parse(dataTable.Rows[i]["PubTime"].ToString()).ToString("yyyy-MM-dd"),
								"\"}"
							}));
						}
					}
					s = string.Concat(new object[]
					{
						"{\"success\":\"true\",\"rowtotal\":\"",
						dataTable.Rows.Count,
						"\",\"total\":\"",
						noticeRequest.TotalRecords,
						"\",\"lihtml\":[",
						stringBuilder.ToString(),
						"]}"
					});
				}
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private string GetMyMemberHtml(System.Data.DataTable dt)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					stringBuilder.Append("<li>");
					stringBuilder.Append(string.Concat(new string[]
					{
						" <h3> ",
						dt.Rows[i]["username"].ToString(),
						"【",
						dt.Rows[i]["GradeName"].ToString(),
						"】</h3>"
					}));
					stringBuilder.Append("<div class='userinfobox'>");
					stringBuilder.Append("<div class='userimg'>");
					stringBuilder.Append("<img src='" + (string.IsNullOrEmpty(dt.Rows[i]["UserHead"].ToString()) ? "/templates/common/images/user.png" : dt.Rows[i]["UserHead"].ToString()) + "'>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("<div class='usertextinfo clearfix'>");
					stringBuilder.Append("<div class='left'>");
					stringBuilder.Append("<p><span class='colorc'>注册时间：</span>" + (string.IsNullOrEmpty(dt.Rows[i]["CreateDate"].ToString()) ? "" : System.DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("yyyy-MM-dd")) + "</p>");
					stringBuilder.Append("<p><span class='colorc'>订单数量：</span><span class='colorg'>" + (string.IsNullOrEmpty(dt.Rows[i]["OrderMumber"].ToString()) ? "0" : dt.Rows[i]["OrderMumber"].ToString()) + "</span> 单</p>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("<div class='right'>");
					stringBuilder.Append("<p><span class='colorc'>最近下单：</span>" + (string.IsNullOrEmpty(dt.Rows[i]["OrderDate"].ToString()) ? "" : System.DateTime.Parse(dt.Rows[i]["OrderDate"].ToString()).ToString("yyyy-MM-dd")) + "</p>");
					stringBuilder.Append("<p><span class='colorc'>消费金额：</span><span class='colorg'>￥" + (string.IsNullOrEmpty(dt.Rows[i]["OrdersTotal"].ToString()) ? "0" : string.Format("{0:F}", dt.Rows[i]["OrdersTotal"].ToString())) + "</span></p>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("<span class='left radius'></span>");
					stringBuilder.Append("<span class='right radius'></span>");
					stringBuilder.Append("</li>");
				}
			}
			return stringBuilder.ToString();
		}

		private void GetWinXinInfo(System.Web.HttpContext context)
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			context.Response.ContentType = "application/json";
			string text = string.Empty;
			if (currentMember == null || string.IsNullOrEmpty(currentMember.OpenId))
			{
				text = "{\"success\":\"false\",\"message\":\"非微信注册会员，获取失败！\"}";
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				string token_Message = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
				if (token_Message.Contains("errmsg") && token_Message.Contains("errcode"))
				{
					text = "{\"success\":\"false\",\"message\":\"获取微信令牌失败！\"}";
				}
				else
				{
					text = BarCodeApi.GetUserInfosByOpenID(token_Message, currentMember.OpenId);
					Globals.Debuglog(text, "_DebuglogGetPic.txt");
				}
			}
			context.Response.Write(text);
			context.Response.End();
		}

		private void AddCustomDistributorStatistic(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string logo = context.Request["logo"];
			string storeName = context.Request["storeName"];
			string text = context.Request["orderNums"];
			string s = context.Request["commTotalSum"];
			bool flag = VShopHelper.InsertCustomDistributorStatistic(new CustomDistributorStatistic
			{
				OrderNums = string.IsNullOrEmpty(text) ? 0 : int.Parse(text),
				Logo = logo,
				StoreName = storeName,
				CommTotalSum = string.IsNullOrEmpty(text) ? 0f : float.Parse(s)
			});
			string s2 = string.Empty;
			if (flag)
			{
				s2 = "{\"success\":\"true\",\"message\":\"添加成功！\"}";
			}
			context.Response.Write(s2);
			context.Response.End();
		}

		private void GetCustomDistributorStatistic(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = context.Request["orderId"];
			int id = 0;
			if (!string.IsNullOrEmpty(text))
			{
				id = int.Parse(text);
			}
			System.Data.DataTable customDistributorStatistic = VShopHelper.GetCustomDistributorStatistic(id);
			string s = "";
			if (customDistributorStatistic != null && customDistributorStatistic.Rows.Count > 0)
			{
				s = string.Concat(new string[]
				{
					"{\"success\":\"true\",\"logo\":\"",
					customDistributorStatistic.Rows[0]["logo"].ToString(),
					"\",\"storename\":\"",
					customDistributorStatistic.Rows[0]["storename"].ToString(),
					"\",\"ordernums\":\"",
					customDistributorStatistic.Rows[0]["ordernums"].ToString(),
					"\",\"commtotalsum\":\"",
					customDistributorStatistic.Rows[0]["commtotalsum"].ToString(),
					"\"}"
				});
			}
			context.Response.Write(s);
			context.Response.End();
		}

		public void followCheck(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string s = string.Empty;
			int currentMemberUserId = Globals.GetCurrentMemberUserId();
			string text = context.Request["followtype"];
			if (currentMemberUserId == 0)
			{
				s = "{\"type\":\"1\",\"tips\":\"当前用户未登入！\"}";
			}
			else if (string.IsNullOrEmpty(text))
			{
				s = "{\"type\":\"0\",\"tips\":\"非法访问！\"}";
			}
			else
			{
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				if (currentMember == null)
				{
					s = "{\"type\":\"0\",\"tips\":\"用户不存在！\"}";
				}
				else
				{
					int num;
					if (text == "wx" && !string.IsNullOrEmpty(currentMember.OpenId))
					{
						num = currentMember.IsFollowWeixin;
						if (num == 0)
						{
							SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
							string token_Message = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
							if (token_Message.Contains("errmsg") && token_Message.Contains("errcode"))
							{
								num = 2;
							}
							else
							{
								string userInfosByOpenID = BarCodeApi.GetUserInfosByOpenID(token_Message, currentMember.OpenId);
								var type = new
								{
									subscribe = "",
									nickname = "",
									headimgurl = ""
								};
								type = JsonConvert.DeserializeAnonymousType(userInfosByOpenID, type);
								if (type.subscribe.Trim() != "1")
								{
									num = 0;
								}
								else
								{
									num = 1;
									MemberProcessor.UpdateUserFollowStateByUserId(currentMemberUserId, 1, "wx");
								}
							}
						}
					}
					else if (text == "fw" && !string.IsNullOrEmpty(currentMember.AlipayOpenid))
					{
						num = currentMember.IsFollowAlipay;
						if (num == 0)
						{
							if (MemberProcessor.IsFuwuFollowUser(currentMember.AlipayOpenid))
							{
								num = 1;
								MemberProcessor.UpdateUserFollowStateByUserId(currentMemberUserId, 1, "fw");
							}
							else
							{
								Articles articles = new Articles
								{
									msgType = "text",
									toUserId = currentMember.AlipayOpenid,
									text = new MessageText
									{
										content = "系统正在检查用户是否关注服务窗，如果已经关注，请忽略此条信息！"
									}
								};
								AlipayMobilePublicMessageCustomSendResponse alipayMobilePublicMessageCustomSendResponse = AliOHHelper.CustomSend(articles);
								if (alipayMobilePublicMessageCustomSendResponse != null && alipayMobilePublicMessageCustomSendResponse.Code == "200")
								{
									num = 1;
									MemberProcessor.UpdateUserFollowStateByUserId(currentMemberUserId, 1, "fw");
								}
							}
						}
					}
					else
					{
						num = 2;
					}
					if (num == 0)
					{
						s = "{\"type\":\"2\",\"tips\":\"当前用户未关注！\"}";
					}
					else if (num == 1)
					{
						s = "{\"type\":\"3\",\"tips\":\"当前用已关注！\"}";
					}
					else
					{
						s = "{\"type\":\"4\",\"tips\":\"非正常访问！！\"}";
					}
				}
			}
			context.Response.Write(s);
			context.Response.End();
		}

		public void CombineOrders(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = string.Empty;
			string text2 = Globals.RequestFormStr("orderidlist").Trim(new char[]
			{
				','
			});
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				text = "{\"type\":\"0\",\"tips\":\"请重新登录！\"}";
			}
			else
			{
				int userId = currentMember.UserId;
				string[] array = text2.Split(new char[]
				{
					','
				});
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				string text3 = string.Empty;
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text4 = array2[i];
					if (!string.IsNullOrEmpty(text4))
					{
						OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(text4);
						if (orderInfo == null)
						{
							text = "{\"type\":\"0\",\"tips\":\"参数错误！\"}";
							break;
						}
						if (orderInfo.UserId != userId)
						{
							text = "{\"type\":\"0\",\"tips\":\"当前订单不是本人的！\"}";
							break;
						}
						if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay || orderInfo.PaymentTypeId == 99 || orderInfo.PaymentTypeId == 0)
						{
							text = "{\"type\":\"0\",\"tips\":\"存在非线上支付的订单或非待付款的订单！\"}";
							break;
						}
						if (string.IsNullOrEmpty(text3))
						{
							text3 = orderInfo.Gateway;
							stringBuilder.Append(orderInfo.OrderId + ",");
						}
						else
						{
							if (orderInfo.Gateway != text3)
							{
								text = "{\"type\":\"0\",\"tips\":\"所选订单的支付方式不一致！\"}";
								break;
							}
							stringBuilder.Append(orderInfo.OrderId + ",");
						}
					}
				}
				string text5 = stringBuilder.ToString().Trim(new char[]
				{
					','
				});
				if (!string.IsNullOrEmpty(text5))
				{
					string text6 = this.GenerateOrderId();
					ShoppingProcessor.CombineOrderToPay(text5, text6);
					text = "{\"type\":\"1\",\"tips\":\"" + text6 + "\"}";
				}
				else if (string.IsNullOrEmpty(text))
				{
					text = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
				}
			}
			context.Response.Write(text);
			context.Response.End();
		}

		public void GetDrawRemarks(System.Web.HttpContext context)
		{
			int num = 0;
			if (!int.TryParse(context.Request["SerialID"], out num))
			{
				context.Response.Write("N");
				return;
			}
			BalanceDrawRequestInfo balanceDrawRequestById = DistributorsBrower.GetBalanceDrawRequestById(num.ToString());
			if (balanceDrawRequestById != null)
			{
				context.Response.Write(balanceDrawRequestById.Remark);
				return;
			}
			context.Response.Write("N");
		}

		public void ConfirmPrizeArriver(System.Web.HttpContext context)
		{
			int id = 0;
			if (int.TryParse(context.Request["Tabid"], out id))
			{
				if (GameHelper.UpdateOneyuanDelivery(new PrizesDeliveQuery
				{
					Status = 3,
					ReceiveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					Id = id,
					LogId = "0"
				}))
				{
					context.Response.Write("success");
					return;
				}
				context.Response.Write("收货确认失败");
				return;
			}
			else
			{
				string text = context.Request["pid"];
				string text2 = context.Request["LogId"];
				if (text == "")
				{
					context.Response.Write("PID为空，请检查！");
					return;
				}
				if (text2 == "")
				{
					context.Response.Write("logID为空，请检查！");
					return;
				}
				int id2 = 0;
				if (!int.TryParse(text, out id2))
				{
					context.Response.Write("当前状态下不允许操作！");
					return;
				}
				if (GameHelper.UpdatePrizesDelivery(new PrizesDeliveQuery
				{
					Status = 3,
					ReceiveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					Id = id2,
					LogId = text2
				}))
				{
					context.Response.Write("success");
					return;
				}
				context.Response.Write("收货确认失败");
				return;
			}
		}

		public void CheckCoupon(System.Web.HttpContext context)
		{
			string text = context.Request["CouponId"];
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"status\":\"N\",\"tips\":\"参数错误1\"}");
				return;
			}
			int couponId = 0;
			if (int.TryParse(text, out couponId))
			{
				context.Response.Write("{\"status\":\"Y\",\"tips\":\"" + CouponHelper.GetCouponsProductIds(couponId) + "\"}");
				return;
			}
			context.Response.Write("{\"status\":\"N\",\"tips\":\"参数错误2\"}");
		}

		public void ConfirmOneyuangPrizeAddr(System.Web.HttpContext context)
		{
			string s = context.Request["shippingId"];
			string str = context.Request["ShipToDate"];
			string pid = context.Request["pid"];
			string s2 = context.Request["Editid"];
			int shippingId = 0;
			if (!int.TryParse(s, out shippingId))
			{
				context.Response.Write("收货人不能为空");
				return;
			}
			ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(shippingId);
			if (shippingAddress == null)
			{
				context.Response.Write("地址信息不完整！");
				return;
			}
			string shipTo = shippingAddress.ShipTo;
			string cellPhone = shippingAddress.CellPhone;
			string address = shippingAddress.Address + "(送货时间：" + str + ")";
			string text = shippingAddress.RegionId.ToString();
			int currentRegionId = 0;
			int.TryParse(text, out currentRegionId);
			text = RegionHelper.GetFullPath(currentRegionId);
			PrizesDeliveQuery prizesDeliveQuery = new PrizesDeliveQuery();
			prizesDeliveQuery.Status = 1;
			prizesDeliveQuery.ReggionPath = text;
			prizesDeliveQuery.Address = address;
			prizesDeliveQuery.Tel = cellPhone;
			prizesDeliveQuery.Receiver = shipTo;
			prizesDeliveQuery.LogId = "0";
			int id = 0;
			int.TryParse(s2, out id);
			prizesDeliveQuery.Id = id;
			prizesDeliveQuery.Pid = pid;
			prizesDeliveQuery.RecordType = 1;
			if (GameHelper.UpdatePrizesDelivery(prizesDeliveQuery))
			{
				context.Response.Write("success");
				return;
			}
			context.Response.Write("保存信息失败");
		}

		public void ConfirmPrizeAddr(System.Web.HttpContext context)
		{
			string str = context.Request["ShipToDate"];
			string logId = context.Request["LogId"];
			string text = context.Request["id"];
			string s = context.Request["shippingId"];
			int shippingId = 0;
			if (!int.TryParse(s, out shippingId))
			{
				context.Response.Write("收货人不能为空");
				return;
			}
			ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(shippingId);
			if (shippingAddress == null)
			{
				context.Response.Write("地址信息不完整！");
				return;
			}
			string shipTo = shippingAddress.ShipTo;
			string cellPhone = shippingAddress.CellPhone;
			string address = shippingAddress.Address + "(送货时间：" + str + ")";
			string text2 = shippingAddress.RegionId.ToString();
			int currentRegionId = 0;
			int.TryParse(text2, out currentRegionId);
			text2 = RegionHelper.GetFullPath(currentRegionId);
			PrizesDeliveQuery prizesDeliveQuery = new PrizesDeliveQuery();
			prizesDeliveQuery.Status = 1;
			prizesDeliveQuery.ReggionPath = text2;
			prizesDeliveQuery.Address = address;
			prizesDeliveQuery.Tel = cellPhone;
			prizesDeliveQuery.Receiver = shipTo;
			prizesDeliveQuery.LogId = logId;
			int id = 0;
			int.TryParse(text.Trim(), out id);
			prizesDeliveQuery.Id = id;
			if (GameHelper.UpdatePrizesDelivery(prizesDeliveQuery))
			{
				context.Response.Write("success");
				return;
			}
			context.Response.Write("保存信息失败");
		}

		public void SignToday(System.Web.HttpContext context)
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				context.Response.Write("未找到会员信息");
				return;
			}
			if (UserSignHelper.IsSign(currentMember.UserId))
			{
				int num = UserSignHelper.USign(currentMember.UserId);
				context.Response.Write("suss" + num.ToString());
				return;
			}
			context.Response.Write("已签到");
		}

		public void countfreighttype(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			ShoppingCartInfo shoppingCartInfo = null;
			if (int.TryParse(context.Request["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(context.Request["from"]) && context.Request["from"] == "signBuy")
			{
				this.productSku = context.Request["productSku"];
				shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
			}
			else
			{
				int templateid = 0;
				if (!string.IsNullOrEmpty(context.Request["TemplateId"]) && int.TryParse(context.Request["TemplateId"], out templateid))
				{
					shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(templateid);
				}
			}
			System.Data.DataView dataView = new System.Data.DataView();
			if (shoppingCartInfo != null)
			{
				dataView = SettingsHelper.GetAllFreightItems().DefaultView;
			}
			float num = 0f;
			if (dataView.Count > 0)
			{
				System.Collections.Generic.Dictionary<int, ShoppingCartItemInfo> dictionary = new System.Collections.Generic.Dictionary<int, ShoppingCartItemInfo>();
				foreach (ShoppingCartItemInfo current in shoppingCartInfo.LineItems)
				{
					if (!dictionary.ContainsKey(current.FreightTemplateId))
					{
						current.SumSubTotal = current.SubTotal;
						current.CubicMeter *= current.Quantity;
						current.FreightWeight *= current.Quantity;
						dictionary.Add(current.FreightTemplateId, current);
					}
					else
					{
						dictionary[current.FreightTemplateId].SumSubTotal += current.SubTotal;
						dictionary[current.FreightTemplateId].FreightWeight += current.FreightWeight * current.Quantity;
						dictionary[current.FreightTemplateId].CubicMeter += current.CubicMeter * current.Quantity;
						dictionary[current.FreightTemplateId].Quantity += current.Quantity;
					}
				}
				shoppingCartInfo.LineItems.Clear();
				foreach (System.Collections.Generic.KeyValuePair<int, ShoppingCartItemInfo> current2 in dictionary)
				{
					shoppingCartInfo.LineItems.Add(current2.Value);
				}
				foreach (ShoppingCartItemInfo current3 in shoppingCartInfo.LineItems)
				{
					if (!current3.IsfreeShipping)
					{
						bool flag = false;
						FreightTemplate templateMessage = SettingsHelper.GetTemplateMessage(current3.FreightTemplateId);
						if (templateMessage != null && current3.FreightTemplateId > 0 && !templateMessage.FreeShip)
						{
							if (templateMessage.HasFree)
							{
								flag = this.IsFreeTemplateShipping(context.Request["RegionId"], current3.FreightTemplateId, int.Parse(context.Request["ModeId"]), current3);
							}
							if (!flag)
							{
								dataView.RowFilter = string.Concat(new object[]
								{
									" RegionId=",
									context.Request["RegionId"],
									" and ModeId=",
									context.Request["ModeId"],
									" and TemplateId=",
									current3.FreightTemplateId,
									" and IsDefault=0"
								});
								if (dataView.Count == 1)
								{
									string a;
									if ((a = dataView[0]["MUnit"].ToString()) != null)
									{
										if (!(a == "1"))
										{
											if (!(a == "2"))
											{
												if (a == "3")
												{
													if (current3.CubicMeter > 0m)
													{
														num += this.setferight(float.Parse(current3.CubicMeter.ToString()), float.Parse(dataView[0]["FristNumber"].ToString()), float.Parse(dataView[0]["FristPrice"].ToString()), float.Parse(dataView[0]["AddNumber"].ToString()), float.Parse(dataView[0]["AddPrice"].ToString()));
													}
												}
											}
											else if (current3.FreightWeight > 0m)
											{
												num += this.setferight(float.Parse(current3.FreightWeight.ToString()), float.Parse(dataView[0]["FristNumber"].ToString()), float.Parse(dataView[0]["FristPrice"].ToString()), float.Parse(dataView[0]["AddNumber"].ToString()), float.Parse(dataView[0]["AddPrice"].ToString()));
											}
										}
										else
										{
											num += this.setferight(float.Parse(current3.Quantity.ToString()), float.Parse(dataView[0]["FristNumber"].ToString()), float.Parse(dataView[0]["FristPrice"].ToString()), float.Parse(dataView[0]["AddNumber"].ToString()), float.Parse(dataView[0]["AddPrice"].ToString()));
										}
									}
								}
								else
								{
									dataView.RowFilter = string.Concat(new object[]
									{
										"  ModeId=",
										context.Request["ModeId"],
										" and TemplateId=",
										current3.FreightTemplateId,
										" and  IsDefault=1"
									});
									string a2;
									if (dataView.Count == 1 && (a2 = dataView[0]["MUnit"].ToString()) != null)
									{
										if (!(a2 == "1"))
										{
											if (!(a2 == "2"))
											{
												if (a2 == "3")
												{
													if (current3.CubicMeter > 0m)
													{
														num += this.setferight(float.Parse(current3.CubicMeter.ToString()), float.Parse(dataView[0]["FristNumber"].ToString()), float.Parse(dataView[0]["FristPrice"].ToString()), float.Parse(dataView[0]["AddNumber"].ToString()), float.Parse(dataView[0]["AddPrice"].ToString()));
													}
												}
											}
											else if (current3.FreightWeight > 0m)
											{
												num += this.setferight(float.Parse(current3.FreightWeight.ToString()), float.Parse(dataView[0]["FristNumber"].ToString()), float.Parse(dataView[0]["FristPrice"].ToString()), float.Parse(dataView[0]["AddNumber"].ToString()), float.Parse(dataView[0]["AddPrice"].ToString()));
											}
										}
										else
										{
											num += this.setferight(float.Parse(current3.Quantity.ToString()), float.Parse(dataView[0]["FristNumber"].ToString()), float.Parse(dataView[0]["FristPrice"].ToString()), float.Parse(dataView[0]["AddNumber"].ToString()), float.Parse(dataView[0]["AddPrice"].ToString()));
										}
									}
								}
							}
						}
					}
				}
			}
			stringBuilder.Append("\"Status\":\"OK\",\"CountFeright\":\"" + num.ToString("F2") + "\"");
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public void GetAliFuWuQRCodeScanInfo(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string key = context.Request["sceneId"];
			stringBuilder.Append("{");
			if (AlipayFuwuConfig.BindAdmin.ContainsKey(key))
			{
				string text = AlipayFuwuConfig.BindAdmin[key];
				if (!string.IsNullOrEmpty(text))
				{
					stringBuilder.Append("\"Status\":\"1\",");
					stringBuilder.Append("\"SceneId\":\"" + text + "\"");
				}
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public void GetQRCodeScanInfo(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			string text = "";
			string text2 = "";
			string text3 = "";
			string arg_44_0 = context.Request["AppID"];
			string text4 = context.Request["Ticket"];
			bool flag = false;
			if (!string.IsNullOrEmpty(text4) && WeiXinHelper.BindAdminOpenId.ContainsKey(text4.Trim()))
			{
				text = WeiXinHelper.BindAdminOpenId[text4.Trim()];
				flag = true;
				WeiXinHelper.BindAdminOpenId.Remove(text4.Trim());
			}
			string text5 = "";
			string text6 = "";
			if (flag)
			{
				try
				{
					text5 = "#1";
					SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
					text5 = "#1OK";
					text5 = string.Concat(new string[]
					{
						text5,
						"WeixinAppId=",
						masterSettings.WeixinAppId,
						"--WeixinAppSecret=",
						masterSettings.WeixinAppSecret
					});
					string token_Message = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
					text5 = text5 + "  accessToken=" + token_Message;
					text5 += "#2OK";
					BarCodeApi.GetHeadImageUrlByOpenID(token_Message, text, out text6, out text2, out text3);
					goto IL_16B;
				}
				catch (System.Exception ex)
				{
					text6 = text5 + "从腾讯服务器获取头像信息出错：" + ex.Message;
					goto IL_16B;
				}
			}
			text6 = "无扫描用户。";
			IL_16B:
			stringBuilder.Append(string.Concat(new string[]
			{
				"\"Status\":\"",
				flag ? "1" : "-1",
				"\",\"OpenID\":\"",
				text,
				"\",\"NickName\":\"",
				text2,
				"\",\"UserHead\":\"",
				text3,
				"\",\"RetInfo\":\"",
				text6,
				"\""
			}));
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public void ClearQRCodeScanInfo(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			string text = "";
			string appID = context.Request["AppID"];
			bool flag = WeiXinHelper.DeleteOldQRCode(appID);
			stringBuilder.Append(string.Concat(new string[]
			{
				"\"Status\":\"",
				flag ? "1" : "-1",
				"\",\"RetInfo\":\"",
				text,
				"\""
			}));
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public bool IsFreeTemplateShipping(string RegionId, int FreightTemplateId, int ModeId, ShoppingCartItemInfo info)
		{
			bool result = false;
			System.Data.DataTable freeTemplateShipping = SettingsHelper.GetFreeTemplateShipping(RegionId, FreightTemplateId, ModeId);
			if (freeTemplateShipping.Rows.Count > 0)
			{
				string text = freeTemplateShipping.Rows[0]["ConditionType"].ToString();
				string a;
				if ((a = text) != null)
				{
					if (!(a == "1"))
					{
						if (!(a == "2"))
						{
							if (a == "3")
							{
								if (info.Quantity >= int.Parse(freeTemplateShipping.Rows[0]["ConditionNumber"].ToString().Split(new char[]
								{
									'$'
								})[0]) && info.SumSubTotal >= decimal.Parse(freeTemplateShipping.Rows[0]["ConditionNumber"].ToString().Split(new char[]
								{
									'$'
								})[1]))
								{
									result = true;
									return result;
								}
								return result;
							}
						}
						else
						{
							if (info.SumSubTotal >= decimal.Parse(freeTemplateShipping.Rows[0]["ConditionNumber"].ToString()))
							{
								result = true;
								return result;
							}
							return result;
						}
					}
					else
					{
						if (info.Quantity >= int.Parse(freeTemplateShipping.Rows[0]["ConditionNumber"].ToString()))
						{
							result = true;
							return result;
						}
						return result;
					}
				}
				result = false;
			}
			return result;
		}

		public float setferight(float counttype, float FristNumber, float FristPrice, float AddNumber, float AddPrice)
		{
			double num = 0.0;
			double num2 = System.Math.Round((double)(counttype - FristNumber), 2);
			if (num2 <= 0.0)
			{
				num += (double)FristPrice;
			}
			else
			{
				num += (double)FristPrice;
				int num3 = (int)System.Math.Ceiling(num2 / (double)AddNumber);
				num += (double)((float)num3 * AddPrice);
			}
			return (float)num;
		}

		public void checkdistribution(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			ShoppingCartInfo shoppingCart;
			if (int.TryParse(context.Request["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(context.Request["from"]) && context.Request["from"] == "signBuy")
			{
				this.productSku = context.Request["productSku"];
				shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
			}
			else
			{
				shoppingCart = ShoppingCartProcessor.GetShoppingCart();
			}
			string regionId = context.Request["city"];
			string text = "";
			foreach (ShoppingCartItemInfo current in shoppingCart.LineItems)
			{
				if (current.FreightTemplateId > 0)
				{
					text = text + current.FreightTemplateId + ",";
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Substring(0, text.Length - 1);
				System.Data.DataTable specifyRegionGroupsModeId = SettingsHelper.GetSpecifyRegionGroupsModeId(text, regionId);
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				stringBuilder2.AppendLine(" <button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择配送方式<span class=\"caret\"></span></button>");
				stringBuilder2.AppendLine("<ul id=\"shippingTypeUl\" class=\"dropdown-menu\" role=\"menu\">");
				if (specifyRegionGroupsModeId.Rows.Count > 0)
				{
					for (int i = 0; i < specifyRegionGroupsModeId.Rows.Count; i++)
					{
						string modelType = this.getModelType(int.Parse(specifyRegionGroupsModeId.Rows[i]["ModeId"].ToString()));
						stringBuilder2.AppendFormat(string.Concat(new object[]
						{
							"<li><a href=\"#\" name=\"",
							specifyRegionGroupsModeId.Rows[i]["ModeId"],
							"\" value=\"",
							specifyRegionGroupsModeId.Rows[i]["ModeId"],
							"\">",
							modelType,
							"</a></li>"
						}), new object[0]);
					}
				}
				else
				{
					stringBuilder2.AppendFormat("<li><a href=\"#\" name=\"0\" value=\"0\">包邮</a></li>", new object[0]);
				}
				stringBuilder2.AppendLine("</ul>");
				stringBuilder.Append(stringBuilder2.ToString() ?? "");
			}
			else
			{
				System.Text.StringBuilder stringBuilder3 = new System.Text.StringBuilder();
				stringBuilder3.AppendLine(" <button type='button' class='btn btn-default dropdown-toggle' data-toggle='dropdown'>请选择配送方式<span class='caret'></span></button>");
				stringBuilder3.AppendLine("<ul id='shippingTypeUl' class='dropdown-menu' role='menu'>");
				stringBuilder3.AppendFormat("<li><a href='#' name='0' value='0'>包邮</a></li>", new object[0]);
				stringBuilder3.AppendLine("</ul>");
				stringBuilder.Append(stringBuilder3.ToString() ?? "");
			}
			context.Response.Write(stringBuilder.ToString());
		}

		public string getModelType(int m)
		{
			string result = string.Empty;
			switch (m)
			{
			case 1:
				result = "快递";
				break;
			case 2:
				result = "EMS";
				break;
			case 3:
				result = "顺丰";
				break;
			case 4:
				result = "平邮";
				break;
			default:
				result = "包邮";
				break;
			}
			return result;
		}

		public void countfreight(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string s = context.Request["id"];
			int currentRegionId = 0;
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			if (int.TryParse(s, out currentRegionId))
			{
				string city = RegionHelper.GetCity(currentRegionId);
				if (city != "0")
				{
					stringBuilder.Append("\"Status\":\"OK\",\"Msg\":\"" + city + "\"");
				}
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public void GetOrderRedPager(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = 0;
			int.TryParse(context.Request["id"], out num);
			int shareUser = 0;
			int.TryParse(context.Request["userid"], out shareUser);
			int currentMemberUserId = Globals.GetCurrentMemberUserId();
			if (currentMemberUserId <= 0)
			{
				context.Response.Write("{\"status\":\"-1\",\"tips\":\"用户未登录！\"}");
				return;
			}
			ShareActivityInfo act = ShareActHelper.GetAct(num);
			if (act == null)
			{
				context.Response.Write("{\"status\":\"-2\",\"tips\":\"活动不存在！\"}");
				return;
			}
			if (act.BeginDate > System.DateTime.Now)
			{
				context.Response.Write("{\"status\":\"-2\",\"tips\":\"活动未开始！\"}");
				return;
			}
			if (act.EndDate < System.DateTime.Now)
			{
				context.Response.Write("{\"status\":\"-2\",\"tips\":\"活动已结束！\"}");
				return;
			}
			int attendCount = ShareActHelper.GeTAttendCount(num, shareUser);
			if (attendCount > act.CouponNumber)
			{
				context.Response.Write("{\"status\":\"-3\",\"tips\":\"您来晚了，领取机会已用完！\"}");
				return;
			}
			bool flag = ShareActHelper.HasAttend(num, currentMemberUserId);
			if (flag)
			{
				context.Response.Write("{\"status\":\"-5\",\"tips\":\"" + currentMemberUserId.ToString() + "\"}");
				return;
			}
			int couponId = act.CouponId;
			if (CouponHelper.GetCoupon(couponId) == null)
			{
				context.Response.Write("{\"status\":\"-2\",\"tips\":\"优惠券不存在！\"}");
				return;
			}
			SendCouponResult sendCouponResult = CouponHelper.IsCanSendCouponToMember(couponId, currentMemberUserId);
			if (sendCouponResult.GetHashCode() == 1)
			{
				context.Response.Write("{\"status\":\"-2\",\"tips\":\"优惠券已结束！\"}");
				return;
			}
			if (sendCouponResult.GetHashCode() == 2)
			{
				context.Response.Write("{\"status\":\"-2\",\"tips\":\"会员不在此活动范围内！\"}");
				return;
			}
			if (sendCouponResult.GetHashCode() == 3)
			{
				context.Response.Write("{\"status\":\"-3\",\"tips\":\"优惠券已领完！\"}");
				return;
			}
			if (sendCouponResult.GetHashCode() == 4)
			{
				context.Response.Write("{\"status\":\"-2\",\"tips\":\"已到领取上限！\"}");
				return;
			}
			if (sendCouponResult.GetHashCode() == 5)
			{
				context.Response.Write("{\"status\":\"-4\",\"tips\":\"领取优惠券失败！\"}");
				return;
			}
			bool flag2 = ShareActHelper.AddRecord(new ShareActivityRecordInfo
			{
				shareId = num,
				shareUser = shareUser,
				attendUser = currentMemberUserId
			});
			if (!flag2)
			{
				context.Response.Write("{\"status\":\"-4\",\"tips\":\"领取优惠券失败！\"}");
				return;
			}
			SendCouponResult sendCouponResult2 = CouponHelper.SendCouponToMember(couponId, currentMemberUserId);
			if (sendCouponResult2.GetHashCode() == 0)
			{
				context.Response.Write("{\"status\":\"0\",\"tips\":\"" + currentMemberUserId.ToString() + "\"}");
				return;
			}
			context.Response.Write("{\"status\":\"-4\",\"tips\":\"领取优惠券失败！\"}");
		}

		public void EditPassword(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string sourceData = context.Request["oldPwd"];
			string text = context.Request["password"];
			string b = context.Request["passagain"];
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (string.IsNullOrEmpty(currentMember.UserBindName))
			{
				context.Response.Write("{\"Status\":\"-5\"}");
				return;
			}
			MemberInfo memberInfo = new MemberInfo();
			int currentMemberUserId = Globals.GetCurrentMemberUserId();
			if (currentMemberUserId <= 0)
			{
				context.Response.Write("{\"Status\":\"-1\"}");
				return;
			}
			memberInfo = MemberProcessor.GetMember(currentMemberUserId, false);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			if (memberInfo.Password == HiCryptographer.Md5Encrypt(sourceData))
			{
				if (text == b)
				{
					if (MemberProcessor.SetPwd(currentMemberUserId.ToString(), HiCryptographer.Md5Encrypt(text)))
					{
						stringBuilder.Append("\"Status\":\"OK\"");
						try
						{
							MemberInfo memberInfo2 = memberInfo;
							if (memberInfo2 != null)
							{
								Messenger.SendWeiXinMsg_PasswordReset(memberInfo2);
							}
							goto IL_126;
						}
						catch (System.Exception)
						{
							goto IL_126;
						}
					}
					stringBuilder.Append("\"Status\":\"-3\"");
				}
				else
				{
					stringBuilder.Append("\"Status\":\"-2\"");
				}
			}
			else
			{
				stringBuilder.Append("\"Status\":\"-4\"");
			}
			IL_126:
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public void BindUserName(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string userName = context.Request["userName"];
			string password = context.Request["password"];
			string passagain = context.Request["passagain"];
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			string value = this.BindUserNameRegist(userName, password, passagain, context);
			stringBuilder.Append(value);
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public void BindOldUserName(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo memberInfo = new MemberInfo();
			string text = context.Request["userName"];
			string sourceData = context.Request["password"];
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			if (!string.IsNullOrEmpty(text))
			{
				memberInfo = MemberProcessor.GetusernameMember(text);
				if (memberInfo == null)
				{
					stringBuilder.Append("\"Status\":\"-1\",\"Msg\":\"帐号不存在！\"");
					stringBuilder.Append("}");
					context.Response.Write(stringBuilder.ToString());
					return;
				}
				if (memberInfo.Status != System.Convert.ToInt32(UserStatus.DEL))
				{
					if (memberInfo.Password == HiCryptographer.Md5Encrypt(sourceData))
					{
						MemberInfo currentMember = MemberProcessor.GetCurrentMember();
						if (currentMember.UserId != memberInfo.UserId)
						{
							if (currentMember.ReferralUserId == memberInfo.ReferralUserId || currentMember.ReferralUserId == memberInfo.UserId)
							{
								DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(currentMember.UserId);
								DistributorsInfo distributorInfo2 = DistributorsBrower.GetDistributorInfo(memberInfo.UserId);
								int userOrders = ShoppingProcessor.GetUserOrders(currentMember.UserId);
								ShoppingProcessor.GetUserOrders(memberInfo.UserId);
								if (distributorInfo2 == null && distributorInfo == null)
								{
									if (userOrders == 0)
									{
										if (MemberProcessor.DelUserMessage(currentMember, memberInfo.UserId))
										{
											HiCache.Remove(string.Format("DataCache-Member-{0}", currentMember.UserId));
											stringBuilder.Append(this.resultstring(memberInfo.UserId, context));
										}
										else
										{
											stringBuilder.Append("\"Status\":\"-1\",\"Msg\":\"删除会员信息失败！\"");
										}
									}
									else
									{
										stringBuilder.Append("\"Status\":\"-1\",\"Msg\":\"当前的登录帐号已产生订单，不能合并！\"");
									}
								}
								else if (distributorInfo2 != null && distributorInfo == null)
								{
									if (userOrders == 0)
									{
										if (MemberProcessor.DelUserMessage(currentMember, memberInfo.UserId))
										{
											HiCache.Remove(string.Format("DataCache-Member-{0}", currentMember.UserId));
											stringBuilder.Append(this.resultstring(memberInfo.UserId, context));
										}
										else
										{
											stringBuilder.Append("\"Status\":\"-1\",\"Msg\":\"删除会员信息失败！\"");
										}
									}
									else
									{
										stringBuilder.Append("\"Status\":\"-1\",\"Msg\":\"会员帐号已产生订单，帐号不能合并！\"");
									}
								}
								else
								{
									stringBuilder.Append("\"Status\":\"-1\",\"Msg\":\"当前分销商不能合并！\"");
								}
							}
							else
							{
								stringBuilder.Append("\"Status\":\"-1\",\"Msg\":\"两个帐号不属于同一上级！\"");
							}
						}
						else
						{
							stringBuilder.Append("\"Status\":\"-1\",\"Msg\":\"不能绑定相同帐号！\"");
						}
					}
					else
					{
						stringBuilder.Append("\"Status\":\"-1\",\"Msg\":\"密码错误！\"");
					}
				}
				else
				{
					stringBuilder.Append("\"Status\":\"-1\",\"Msg\":\"您的帐号在系统中已删除，不能绑定！\"");
				}
			}
			else
			{
				stringBuilder.Append("\"Status\":\"-1\",\"Msg\":\"帐号不能为空！\"");
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public string resultstring(int userid, System.Web.HttpContext context)
		{
			this.setLogin(userid);
			return "\"Status\":\"0\"";
		}

		public string BindUserNameRegist(string userName, string password, string passagain, System.Web.HttpContext context)
		{
			if (!(password == passagain))
			{
				return "\"Status\":\"-2\"";
			}
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			MemberInfo memberInfo = new MemberInfo();
			if (string.IsNullOrEmpty(userName))
			{
				return "\"Status\":\"-1\"";
			}
			if (MemberProcessor.GetBindusernameMember(userName) != null)
			{
				return "\"Status\":\"-1\"";
			}
			if (MemberProcessor.BindUserName(currentMember.UserId, userName, HiCryptographer.Md5Encrypt(password)))
			{
				return "\"Status\":\"OK\"";
			}
			return "\"Status\":\"-3\"";
		}

		public void RegisterUser(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string userName = context.Request["userName"];
			string password = context.Request["password"];
			string passagain = context.Request["passagain"];
			string text = context.Request["openId"];
			string headimgurl = context.Request["headimgurl"];
			int num = 0;
			System.Web.HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
			if (httpCookie != null)
			{
				num = Globals.ToNum(httpCookie.Value);
			}
			if (num > 0 && DistributorsBrower.GetDistributorInfo(num) == null)
			{
				num = 0;
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			if (!string.IsNullOrEmpty(text))
			{
				if (MemberProcessor.GetOpenIdMember(text, "wx") == null)
				{
					string value = this.regist(userName, password, passagain, text, headimgurl, num.ToString(), context);
					stringBuilder.Append(value);
				}
				else
				{
					stringBuilder.Append("\"Status\":\"-3\"");
				}
			}
			else
			{
				string value2 = this.regist(userName, password, passagain, text, headimgurl, num.ToString(), context);
				stringBuilder.Append(value2);
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public string regist(string userName, string password, string passagain, string openId, string headimgurl, string referralUserId, System.Web.HttpContext context)
		{
			if (!(password == passagain))
			{
				return "\"Status\":\"-2\"";
			}
			MemberInfo memberInfo = new MemberInfo();
			if (MemberProcessor.GetusernameMember(userName) == null)
			{
				MemberInfo memberInfo2 = new MemberInfo();
				string generateId = Globals.GetGenerateId();
				memberInfo2.GradeId = MemberProcessor.GetDefaultMemberGrade();
				memberInfo2.OpenId = openId;
				memberInfo2.UserHead = headimgurl;
				memberInfo2.UserName = userName;
				memberInfo2.ReferralUserId = (string.IsNullOrEmpty(referralUserId) ? 0 : System.Convert.ToInt32(referralUserId));
				memberInfo2.CreateDate = System.DateTime.Now;
				memberInfo2.SessionId = generateId;
				memberInfo2.SessionEndTime = System.DateTime.Now.AddYears(10);
				memberInfo2.Password = HiCryptographer.Md5Encrypt(password);
				memberInfo2.UserBindName = userName;
				bool flag = MemberProcessor.CreateMember(memberInfo2);
				if (flag)
				{
					this.myNotifier.updateAction = UpdateAction.MemberUpdate;
					this.myNotifier.actionDesc = "会员注册";
					this.myNotifier.RecDateUpdate = System.DateTime.Today;
					this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
					this.myNotifier.UpdateDB();
				}
				MemberInfo member = MemberProcessor.GetMember(generateId);
				this.setLogin(member.UserId);
				return "\"Status\":\"OK\",\"referralUserId\":" + memberInfo2.ReferralUserId;
			}
			return "\"Status\":\"-1\"";
		}

		public void UserLogin(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo memberInfo = new MemberInfo();
			string text = context.Request["userName"];
			string sourceData = context.Request["password"];
			string text2 = context.Request["openId"];
			string userHead = context.Request["headimgurl"];
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			if (!string.IsNullOrEmpty(text))
			{
				memberInfo = MemberProcessor.GetusernameMember(text);
				if (memberInfo == null)
				{
					stringBuilder.Append("\"Status\":\"-1\"");
					stringBuilder.Append("}");
					context.Response.Write(stringBuilder.ToString());
					return;
				}
				if (memberInfo.Status == System.Convert.ToInt32(UserStatus.DEL))
				{
					stringBuilder.Append("\"Status\":\"-4\"");
					stringBuilder.Append("}");
					context.Response.Write(stringBuilder.ToString());
					return;
				}
				if (memberInfo.Password == HiCryptographer.Md5Encrypt(sourceData))
				{
					if (!string.IsNullOrEmpty(text2))
					{
						memberInfo.OpenId = text2;
						memberInfo.UserHead = userHead;
						MemberProcessor.UpdateMember(memberInfo);
					}
					this.setLogin(memberInfo.UserId);
					stringBuilder.Append("\"Status\":\"OK\"");
				}
				else
				{
					stringBuilder.Append("\"Status\":\"-2\"");
				}
			}
			else
			{
				stringBuilder.Append("\"Status\":\"-3\"");
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		private void CheckFavorite(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				context.Response.Write("{\"success\":false}");
				return;
			}
			int productId = System.Convert.ToInt32(context.Request["ProductId"]);
			if (ProductBrowser.ExistsProduct(productId, currentMember.UserId))
			{
				context.Response.Write("{\"success\":true}");
				return;
			}
			context.Response.Write("{\"success\":false}");
		}

		private void DelFavorite(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int favoriteId = System.Convert.ToInt32(context.Request["favoriteId"]);
			if (ProductBrowser.DeleteFavorite(favoriteId) == 1)
			{
				context.Response.Write("{\"success\":true}");
				return;
			}
			context.Response.Write("{\"success\":false, \"msg\":\"取消失败\"}");
		}

		private void AddFavorite(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"请先登录才可以收藏商品\"}");
				return;
			}
			int productId = System.Convert.ToInt32(context.Request["ProductId"]);
			if (ProductBrowser.AddProductToFavorite(productId, currentMember.UserId))
			{
				context.Response.Write("{\"success\":true}");
				return;
			}
			context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
		}

		private void AddProductReview(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"请先登录\"}");
				return;
			}
			bool flag = false;
			int num = Globals.RequestFormNum("ProductId");
			string text = Globals.RequestFormStr("orderid");
			string text2 = Globals.RequestFormStr("skuid");
			int num2 = Globals.RequestFormNum("itemid");
			int userId = currentMember.UserId;
			LineItemInfo lineItemInfo = null;
			if (string.IsNullOrEmpty(text2))
			{
				flag = true;
				lineItemInfo = ProductBrowser.GetLatestOrderItemByProductIDAndUserID(num, userId);
				if (lineItemInfo == null)
				{
					context.Response.Write("{\"success\":false, \"msg\":\"完成订单后才能评价该商品\"}");
					return;
				}
				text2 = lineItemInfo.SkuId;
				text = lineItemInfo.OrderID;
				num2 = lineItemInfo.ID;
			}
			if (lineItemInfo != null || ProductBrowser.IsReview(text, text2, num, userId))
			{
				string str = "该商品您已经评价过";
				if (flag)
				{
					str = "完成订单后才能评价该商品";
				}
				context.Response.Write("{\"success\":false, \"msg\":\"" + str + "\"}");
				return;
			}
			LineItemInfo returnMoneyByOrderIDAndProductID = ProductBrowser.GetReturnMoneyByOrderIDAndProductID(text, text2, num2);
			if (returnMoneyByOrderIDAndProductID == null)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"您未购买该商品，不能评价\"}");
				return;
			}
			if (returnMoneyByOrderIDAndProductID.OrderItemsStatus != OrderStatus.Finished)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"订单完成后，才能对商品进行评价\"}");
				return;
			}
			if (ProductBrowser.InsertProductReview(new ProductReviewInfo
			{
				ReviewDate = System.DateTime.Now,
				ReviewText = context.Request["ReviewText"],
				ProductId = num,
				UserEmail = currentMember.Email,
				UserId = currentMember.UserId,
				UserName = currentMember.UserName,
				OrderId = text,
				SkuId = text2,
				OrderItemID = num2
			}))
			{
				context.Response.Write("{\"success\":true}");
				return;
			}
			context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
		}

		private void AddProductConsultations(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			ProductConsultationInfo productConsultationInfo = new ProductConsultationInfo();
			productConsultationInfo.ConsultationDate = System.DateTime.Now;
			productConsultationInfo.ConsultationText = context.Request["ConsultationText"];
			productConsultationInfo.ProductId = System.Convert.ToInt32(context.Request["ProductId"]);
			productConsultationInfo.UserEmail = currentMember.Email;
			productConsultationInfo.UserId = currentMember.UserId;
			productConsultationInfo.UserName = currentMember.UserName;
			if (ProductBrowser.InsertProductConsultation(productConsultationInfo))
			{
				context.Response.Write("{\"success\":true}");
				try
				{
					ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(productConsultationInfo.ProductId);
					if (productBaseInfo != null)
					{
						Messenger.SendWeiXinMsg_ProductAsk(productBaseInfo.ProductName, "", productConsultationInfo.ConsultationText);
					}
					return;
				}
				catch (System.Exception)
				{
					return;
				}
			}
			context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
		}

		private void CancelOrder(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string orderId = string.Empty;
			string text = System.Convert.ToString(context.Request["orderId"]);
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (text.Contains(','))
			{
				string[] array = text.Trim().Trim(new char[]
				{
					','
				}).Split(new char[]
				{
					','
				});
				int num = 0;
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string orderId2 = array2[i];
					OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId2);
					orderInfo.CloseReason = "买家关闭";
					if (currentMember.UserId == orderInfo.UserId && MemberProcessor.CancelOrder(orderInfo))
					{
						orderInfo.OnClosed();
						num++;
					}
				}
				if (num > 0)
				{
					context.Response.Write("{\"success\":true,\"icount\":" + num.ToString() + "}");
					return;
				}
				context.Response.Write("{\"success\":false, \"msg\":\"取消订单失败\"}");
				return;
			}
			else
			{
				orderId = text;
				OrderInfo orderInfo2 = ShoppingProcessor.GetOrderInfo(orderId);
				orderInfo2.CloseReason = "买家关闭";
				if (currentMember.UserId != orderInfo2.UserId)
				{
					context.Response.Write("{\"success\":false, \"msg\":\"只能取消自己的订单\"}");
					return;
				}
				if (MemberProcessor.CancelOrder(orderInfo2))
				{
					orderInfo2.OnClosed();
					context.Response.Write("{\"success\":true,\"icount\":1}");
					return;
				}
				context.Response.Write("{\"success\":false, \"msg\":\"取消订单失败\"}");
				return;
			}
		}

		private void FinishOrder(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			bool flag = false;
			string orderId = System.Convert.ToString(context.Request["orderId"]);
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
			System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
			LineItemInfo lineItemInfo = new LineItemInfo();
			foreach (System.Collections.Generic.KeyValuePair<string, LineItemInfo> current in lineItems)
			{
				lineItemInfo = current.Value;
				if (lineItemInfo.OrderItemsStatus == OrderStatus.ApplyForRefund || lineItemInfo.OrderItemsStatus == OrderStatus.ApplyForReturns)
				{
					flag = true;
				}
			}
			if (flag)
			{
				context.Response.Write("{\"success\":false, \"msg\":\"订单中商品有退货(款)不允许完成\"}");
				return;
			}
			if (orderInfo != null && MemberProcessor.ConfirmOrderFinish(orderInfo))
			{
				DistributorsBrower.UpdateCalculationCommission(orderInfo);
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				int num = 0;
				if (masterSettings.IsRequestDistributor && !string.IsNullOrEmpty(masterSettings.FinishedOrderMoney.ToString()) && currentMember.Expenditure >= masterSettings.FinishedOrderMoney)
				{
					num = 1;
				}
				foreach (LineItemInfo current2 in orderInfo.LineItems.Values)
				{
					if (current2.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
					{
						ShoppingProcessor.UpdateOrderGoodStatu(orderInfo.OrderId, current2.SkuId, 5, current2.ID);
					}
				}
				DistributorsInfo distributorsInfo = new DistributorsInfo();
				distributorsInfo = DistributorsBrower.GetUserIdDistributors(orderInfo.UserId);
				if (distributorsInfo != null && distributorsInfo.UserId > 0)
				{
					num = 0;
				}
				context.Response.Write("{\"success\":true,\"isapply\":" + num + "}");
				return;
			}
			context.Response.Write("{\"success\":false, \"msg\":\"订单当前状态不允许完成\"}");
		}

		private void SearchExpressData(System.Web.HttpContext context)
		{
			string s = string.Empty;
			if (!string.IsNullOrEmpty(context.Request["OrderId"]))
			{
				string orderId = context.Request["OrderId"];
				OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
				if (orderInfo != null && (orderInfo.OrderStatus == OrderStatus.SellerAlreadySent || orderInfo.OrderStatus == OrderStatus.Finished || orderInfo.OrderStatus == OrderStatus.Deleted) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
				{
					s = ExpressHelper.GetExpressData(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
				}
			}
			context.Response.ContentType = "application/json";
			context.Response.Write(s);
			context.Response.End();
		}

		private void DelShippingAddress(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				context.Response.Write("{\"success\":false}");
				return;
			}
			int userId = currentMember.UserId;
			int shippingid = System.Convert.ToInt32(context.Request.Form["shippingid"]);
			if (MemberProcessor.DelShippingAddress(shippingid, userId))
			{
				context.Response.Write("{\"success\":true}");
				return;
			}
			context.Response.Write("{\"success\":false}");
		}

		private void SetDefaultShippingAddress(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				context.Response.Write("{\"success\":false}");
				return;
			}
			int userId = currentMember.UserId;
			int shippingId = System.Convert.ToInt32(context.Request.Form["shippingid"]);
			if (MemberProcessor.SetDefaultShippingAddress(shippingId, userId))
			{
				context.Response.Write("{\"success\":true}");
				return;
			}
			context.Response.Write("{\"success\":false}");
		}

		private void AddShippingAddress(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				context.Response.Write("{\"success\":false}");
				return;
			}
			if (MemberProcessor.AddShippingAddress(new ShippingAddressInfo
			{
				Address = context.Request.Form["address"],
				CellPhone = context.Request.Form["cellphone"],
				ShipTo = context.Request.Form["shipTo"],
				Zipcode = "",
				IsDefault = true,
				UserId = currentMember.UserId,
				RegionId = System.Convert.ToInt32(context.Request.Form["regionSelectorValue"])
			}) > 0)
			{
				context.Response.Write("{\"success\":true}");
				return;
			}
			context.Response.Write("{\"success\":false}");
		}

		private void UpdateShippingAddress(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				context.Response.Write("{\"success\":false}");
				return;
			}
			if (MemberProcessor.UpdateShippingAddress(new ShippingAddressInfo
			{
				Address = context.Request.Form["address"],
				CellPhone = context.Request.Form["cellphone"],
				ShipTo = context.Request.Form["shipTo"],
				Zipcode = "",
				UserId = currentMember.UserId,
				ShippingId = System.Convert.ToInt32(context.Request.Form["shippingid"]),
				RegionId = System.Convert.ToInt32(context.Request.Form["regionSelectorValue"])
			}))
			{
				context.Response.Write("{\"success\":true}");
				return;
			}
			context.Response.Write("{\"success\":false}");
		}

		private void ProcessSubmitMemberCard(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				context.Response.Write("{\"success\":false}");
				return;
			}
			currentMember.Address = context.Request.Form.Get("address");
			currentMember.RealName = context.Request.Form.Get("name");
			currentMember.CellPhone = context.Request.Form.Get("phone");
			currentMember.QQ = context.Request.Form.Get("qq");
			if (!string.IsNullOrEmpty(currentMember.QQ))
			{
				currentMember.Email = currentMember.QQ + "@qq.com";
			}
			currentMember.VipCardNumber = SettingsManager.GetMasterSettings(true).VipCardPrefix + currentMember.UserId.ToString();
			currentMember.VipCardDate = new System.DateTime?(System.DateTime.Now);
			string s = MemberProcessor.UpdateMember(currentMember) ? "{\"success\":true}" : "{\"success\":false}";
			context.Response.Write(s);
		}

		private bool ExistsProduct(int productId, int exchangeId, int number)
		{
			System.Collections.Generic.List<ShoppingCartInfo> shoppingCartAviti = ShoppingCartProcessor.GetShoppingCartAviti(1);
			int num = 0;
			if (shoppingCartAviti != null)
			{
				num = shoppingCartAviti.Count<ShoppingCartInfo>();
			}
			PointExchangeProductInfo productInfo = PointExChangeHelper.GetProductInfo(exchangeId, productId);
			if (productInfo != null)
			{
				int eachMaxNumber = productInfo.EachMaxNumber;
				if (eachMaxNumber == 0)
				{
					return true;
				}
				if (eachMaxNumber > 0)
				{
					return num + number <= eachMaxNumber;
				}
			}
			return false;
		}

		private void ProcessAddToCartByExchange(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = int.Parse(context.Request["quantity"], System.Globalization.NumberStyles.None);
			string skuId = context.Request["productSkuId"];
			int categoryid = int.Parse(context.Request["categoryid"], System.Globalization.NumberStyles.None);
			int templateid = int.Parse(context.Request["Templateid"], System.Globalization.NumberStyles.None);
			int productId = int.Parse(context.Request["ProductId"], System.Globalization.NumberStyles.None);
			int type = 0;
			int.TryParse(context.Request["type"], out type);
			int exchangeId = 0;
			int.TryParse(context.Request["exchangeId"], out exchangeId);
			if (MemberProcessor.GetCurrentMember() == null)
			{
				context.Response.Write("{\"Status\":\"2\"}");
				return;
			}
			if (!this.ExistsProduct(productId, int.Parse(context.Request["exchangeId"]), num))
			{
				context.Response.Write("{\"Status\":\"10\"}");
				return;
			}
			ShoppingCartProcessor.AddLineItem(skuId, num, categoryid, templateid, type, exchangeId, 0);
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
			context.Response.Write(string.Concat(new string[]
			{
				"{\"Status\":\"OK\",\"TotalMoney\":\"",
				shoppingCart.GetTotal().ToString(".00"),
				"\",\"Quantity\":\"",
				shoppingCart.GetQuantity().ToString(),
				"\"}"
			}));
		}

		private void ProcessAddToCartBySkus(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int quantity = int.Parse(context.Request["quantity"], System.Globalization.NumberStyles.None);
			string skuId = context.Request["productSkuId"];
			int categoryid = int.Parse(context.Request["categoryid"], System.Globalization.NumberStyles.None);
			int templateid = int.Parse(context.Request["Templateid"], System.Globalization.NumberStyles.None);
			int limitedTimeDiscountId = 0;
			int type = 0;
			int.TryParse(context.Request["type"], out type);
			int exchangeId = 0;
			int.TryParse(context.Request["exchangeId"], out exchangeId);
			if (MemberProcessor.GetCurrentMember() == null)
			{
				Globals.Debuglog("检验到用户未登录", "_LoginDebuglog.txt");
				context.Response.Write("{\"Status\":\"2\"}");
				return;
			}
			ShoppingCartProcessor.AddLineItem(skuId, quantity, categoryid, templateid, type, exchangeId, limitedTimeDiscountId);
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
			context.Response.Write(string.Concat(new string[]
			{
				"{\"Status\":\"OK\",\"TotalMoney\":\"",
				shoppingCart.GetTotal().ToString(".00"),
				"\",\"Quantity\":\"",
				shoppingCart.GetQuantity().ToString(),
				"\"}"
			}));
		}

		private void ProcessGetSkuByOptions(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int productId = int.Parse(context.Request["productId"], System.Globalization.NumberStyles.None);
			string text = context.Request["options"];
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"0\"}");
				return;
			}
			if (text.EndsWith(","))
			{
				text = text.Substring(0, text.Length - 1);
			}
			SKUItem productAndSku = ShoppingProcessor.GetProductAndSku(MemberProcessor.GetCurrentMember(), productId, text);
			if (productAndSku == null)
			{
				context.Response.Write("{\"Status\":\"1\"}");
				return;
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"Status\":\"OK\",");
			stringBuilder.AppendFormat("\"SkuId\":\"{0}\",", productAndSku.SkuId);
			stringBuilder.AppendFormat("\"SKU\":\"{0}\",", productAndSku.SKU);
			stringBuilder.AppendFormat("\"Weight\":\"{0}\",", productAndSku.Weight);
			stringBuilder.AppendFormat("\"Stock\":\"{0}\",", productAndSku.Stock);
			stringBuilder.AppendFormat("\"SalePrice\":\"{0}\"", productAndSku.SalePrice.ToString("F2"));
			stringBuilder.Append("}");
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder.ToString());
		}

		private void ProcessDeleteCartProduct(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string skuId = context.Request["skuId"];
			int type = 0;
			int.TryParse(context.Request["type"], out type);
			int limitedTimeDiscountId = Globals.RequestFormNum("limitedTimeDiscountId");
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			ShoppingCartProcessor.RemoveLineItem(skuId, type, limitedTimeDiscountId);
			stringBuilder.Append("{");
			stringBuilder.Append("\"Status\":\"OK\"");
			stringBuilder.Append("}");
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder.ToString());
		}

		private void ProcessChageQuantity(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string skuId = context.Request["skuId"];
			int num = 1;
			int.TryParse(context.Request["quantity"], out num);
			int type = 0;
			int.TryParse(context.Request["type"], out type);
			int exchangeId = 0;
			int.TryParse(context.Request["exchangeId"], out exchangeId);
			int num2 = Globals.RequestFormNum("limitedTimeDiscountId");
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			int num3 = ShoppingCartProcessor.GetSkuStock(skuId, type, exchangeId);
			if (num > num3)
			{
				stringBuilder.AppendFormat("\"Status\":\"{0}\"", num3);
				num = num3;
			}
			else
			{
				bool flag = true;
				if (num2 > 0)
				{
					LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(num2);
					if (discountInfo != null)
					{
						int limitNumber = discountInfo.LimitNumber;
						if (limitNumber > 0)
						{
							int limitedTimeDiscountUsedNum = ShoppingCartProcessor.GetLimitedTimeDiscountUsedNum(num2, skuId, 0, -1, true);
							if (num + limitedTimeDiscountUsedNum > limitNumber)
							{
								int num4 = limitNumber - limitedTimeDiscountUsedNum;
								num3 = ((num4 > 0) ? num4 : 0);
								int limitedTimeDiscountUsedNum2 = ShoppingCartProcessor.GetLimitedTimeDiscountUsedNum(num2, skuId, 0, -1, false);
								if (num3 == 0 && num > limitNumber - limitedTimeDiscountUsedNum2)
								{
									flag = false;
									num3 = limitNumber - limitedTimeDiscountUsedNum2;
								}
							}
						}
					}
				}
				if (!flag)
				{
					stringBuilder.AppendFormat("\"Status\":\"{0}\"", num3);
				}
				else
				{
					stringBuilder.Append("\"Status\":\"OK\",");
					ShoppingCartProcessor.UpdateLineItemQuantity(skuId, (num > 0) ? num : 1, type, num2);
					ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
					stringBuilder.AppendFormat("\"TotalPrice\":\"{0}\"", shoppingCart.GetAmount());
				}
			}
			stringBuilder.Append("}");
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder.ToString());
		}

		private void GetOrderItemStatus(System.Web.HttpContext context)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			string orderId = Globals.RequestFormStr("orderid");
			string skuid = Globals.RequestFormStr("skuid");
			int orderitemid = Globals.RequestFormNum("orderitemid");
			RefundInfo byOrderIdAndProductID = RefundHelper.GetByOrderIdAndProductID(orderId, 0, skuid, orderitemid);
			if (byOrderIdAndProductID != null)
			{
				stringBuilder.Append(string.Concat(new object[]
				{
					"\"Status\":1,\"HandleStatus\":",
					(int)byOrderIdAndProductID.HandleStatus,
					",\"Reason\":\"",
					Globals.String2Json(byOrderIdAndProductID.AdminRemark),
					"\""
				}));
			}
			else
			{
				stringBuilder.Append("\"Status\":0,\"Tips\":\"未找到项目\"");
			}
			stringBuilder.Append("}");
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder.ToString());
		}

		private void AddParticipant(System.Web.HttpContext context)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			string text = context.Request.Form["ActivityId"];
			string text2 = context.Request.Form["buyNum"];
			string text3 = context.Request.Form["SkuId"];
			string text4 = context.Request.Form["Options"];
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3))
			{
				stringBuilder.Append("\"Status\":\"false\",\"Msg\":\"参数错误，不能提交1\"");
			}
			else
			{
				int num = 0;
				OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(text);
				if (int.TryParse(text2, out num) && num > 0 && oneyuanTaoInfoById != null)
				{
					OneyuanTaoParticipantInfo oneyuanTaoParticipantInfo = new OneyuanTaoParticipantInfo();
					OneTaoState oneTaoState = OneyuanTaoHelp.getOneTaoState(oneyuanTaoInfoById);
					if (oneTaoState != OneTaoState.进行中)
					{
						stringBuilder.Append("\"Status\":\"false\",\"Msg\":\"当前活动" + oneTaoState.ToString() + "，不能参与\"");
					}
					else
					{
						MemberProcessor.GetCurrentMember();
						if (!MemberProcessor.CheckCurrentMemberIsInRange(oneyuanTaoInfoById.FitMember, oneyuanTaoInfoById.DefualtGroup, oneyuanTaoInfoById.CustomGroup))
						{
							stringBuilder.Append("\"Status\":false,\"Msg\":\"您当前不能参与该活动！！\"");
						}
						else
						{
							oneyuanTaoParticipantInfo.Pid = OneyuanTaoHelp.GetOrderNumber(false);
							oneyuanTaoParticipantInfo.ActivityId = text;
							oneyuanTaoParticipantInfo.UserId = Globals.GetCurrentMemberUserId();
							oneyuanTaoParticipantInfo.SkuId = text3;
							oneyuanTaoParticipantInfo.SkuIdStr = OneyuanTaoHelp.GetSkuStrBySkuId(text3, true);
							oneyuanTaoParticipantInfo.BuyNum = num;
							oneyuanTaoParticipantInfo.TotalPrice = oneyuanTaoInfoById.EachPrice * num;
							oneyuanTaoParticipantInfo.ProductPrice = oneyuanTaoInfoById.ProductPrice;
							if (!string.IsNullOrEmpty(text4))
							{
								SKUItem productAndSku = ShoppingProcessor.GetProductAndSku(MemberProcessor.GetCurrentMember(), oneyuanTaoInfoById.ProductId, text4);
								if (productAndSku != null)
								{
									oneyuanTaoParticipantInfo.ProductPrice = productAndSku.SalePrice;
								}
							}
							if (OneyuanTaoHelp.AddParticipant(oneyuanTaoParticipantInfo))
							{
								stringBuilder.Append("\"Status\":true,\"Msg\":\"参与活动成功！\",\"Aid\":\"" + oneyuanTaoParticipantInfo.Pid + "\"");
							}
							else
							{
								stringBuilder.Append("\"Status\":false,\"Msg\":\"参与活动失败\"");
							}
						}
					}
				}
				else
				{
					stringBuilder.Append("\"Status\":\"false\",\"Msg\":\"参数错误，不能提交2\"");
				}
			}
			stringBuilder.Append("}");
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder.ToString());
		}

		public string ordersummit(ShoppingCartInfo cart, System.Web.HttpContext context, string remark, int shippingId, string couponCode, string selectCouponValue, string shippingTypeinfo, bool summittype, string OrderMarking, System.Collections.Generic.IList<ShoppingCartItemInfo> ItemInfo, int PointExchange, int bargainDetialId, out string ActivitiesIds)
		{
			ActivitiesIds = "";
			int num = 0;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(cart, false, false);
			if (orderInfo != null)
			{
				orderInfo.OrderId = this.GenerateOrderId();
				orderInfo.OrderDate = System.DateTime.Now;
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				orderInfo.UserId = currentMember.UserId;
				orderInfo.Username = currentMember.UserName;
				orderInfo.EmailAddress = currentMember.Email;
				orderInfo.RealName = currentMember.RealName;
				orderInfo.QQ = currentMember.QQ;
				orderInfo.Remark = remark;
				string text = "";
				string activitiesName = "";
				string empty = string.Empty;
				if (bargainDetialId > 0)
				{
					orderInfo.BargainDetialId = bargainDetialId;
					orderInfo.DiscountAmount = 0m;
				}
				else
				{
					orderInfo.DiscountAmount = this.DiscountMoney(ItemInfo, out text, out activitiesName, currentMember, out num, out empty);
				}
				ActivitiesIds = text;
				if (num > 0 && CouponHelper.IsCanSendCouponToMember(num, currentMember.UserId) != SendCouponResult.正常领取)
				{
					num = 0;
				}
				orderInfo.OrderMarking = OrderMarking;
				if (orderInfo.DiscountAmount > 0m || num > 0)
				{
					orderInfo.ActivitiesId = text;
					orderInfo.ActivitiesName = activitiesName;
				}
				orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
				int num2 = 0;
				System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
				foreach (System.Collections.Generic.KeyValuePair<string, LineItemInfo> current in lineItems)
				{
					LineItemInfo lineItemInfo = new LineItemInfo();
					lineItemInfo = current.Value;
					if (lineItemInfo.Type == 1)
					{
						num2++;
					}
				}
				if (orderInfo.LineItems.Count == num2)
				{
					orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
				}
				orderInfo.RefundStatus = RefundStatus.None;
				orderInfo.ShipToDate = context.Request["shiptoDate"];
				int num3 = Globals.GetCurrentDistributorId();
				if (num3 > 0)
				{
					DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(num3);
					if (distributorInfo != null)
					{
						switch (Globals.GetCommitionType())
						{
						case 0:
							orderInfo.ReferralPath = distributorInfo.ReferralPath;
							break;
						case 1:
							if (num3 == orderInfo.UserId)
							{
								num3 = distributorInfo.UserId;
								DistributorsInfo distributorInfo2 = DistributorsBrower.GetDistributorInfo(num3);
								if (distributorInfo2 != null)
								{
									orderInfo.ReferralPath = distributorInfo2.ReferralPath;
								}
							}
							else
							{
								orderInfo.ReferralPath = distributorInfo.ReferralPath;
							}
							break;
						}
					}
					else
					{
						num3 = 0;
					}
				}
				orderInfo.ReferralUserId = num3;
				int num4 = 0;
				int num5 = 0;
				ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(shippingId);
				if (shippingAddress != null)
				{
					orderInfo.ShippingRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, "，");
					orderInfo.RegionId = shippingAddress.RegionId;
					orderInfo.Address = shippingAddress.Address;
					orderInfo.ZipCode = shippingAddress.Zipcode;
					orderInfo.ShipTo = shippingAddress.ShipTo;
					orderInfo.TelPhone = shippingAddress.TelPhone;
					orderInfo.CellPhone = shippingAddress.CellPhone;
					MemberProcessor.SetDefaultShippingAddress(shippingId, MemberProcessor.GetCurrentMember().UserId);
				}
				if (int.TryParse(shippingTypeinfo, out num4))
				{
					orderInfo.ShippingModeId = num4;
					orderInfo.ModeName = this.getModelType(num4);
					orderInfo.AdjustedFreight = 0m;
					if (num4 > 0)
					{
						System.Data.DataView dataView = new System.Data.DataView();
						if (cart != null)
						{
							dataView = SettingsHelper.GetAllFreightItems().DefaultView;
						}
						float num6 = 0f;
						if (dataView.Count > 0)
						{
							System.Collections.Generic.Dictionary<int, ShoppingCartItemInfo> dictionary = new System.Collections.Generic.Dictionary<int, ShoppingCartItemInfo>();
							foreach (ShoppingCartItemInfo current2 in cart.LineItems)
							{
								if (!dictionary.ContainsKey(current2.FreightTemplateId))
								{
									current2.SumSubTotal = current2.SubTotal;
									current2.CubicMeter *= current2.Quantity;
									current2.FreightWeight *= current2.Quantity;
									dictionary.Add(current2.FreightTemplateId, current2);
								}
								else
								{
									dictionary[current2.FreightTemplateId].SumSubTotal += current2.SubTotal;
									dictionary[current2.FreightTemplateId].FreightWeight += current2.FreightWeight * current2.Quantity;
									dictionary[current2.FreightTemplateId].CubicMeter += current2.CubicMeter * current2.Quantity;
									dictionary[current2.FreightTemplateId].Quantity += current2.Quantity;
								}
							}
							cart.LineItems.Clear();
							foreach (System.Collections.Generic.KeyValuePair<int, ShoppingCartItemInfo> current3 in dictionary)
							{
								cart.LineItems.Add(current3.Value);
							}
							foreach (ShoppingCartItemInfo current4 in cart.LineItems)
							{
								if (!current4.IsfreeShipping)
								{
									bool flag = false;
									FreightTemplate templateMessage = SettingsHelper.GetTemplateMessage(current4.FreightTemplateId);
									if (templateMessage != null && current4.FreightTemplateId > 0 && !templateMessage.FreeShip)
									{
										if (templateMessage.HasFree)
										{
											flag = this.IsFreeTemplateShipping(context.Request["Shippingcity"], current4.FreightTemplateId, num4, current4);
										}
										if (!flag)
										{
											dataView.RowFilter = string.Concat(new object[]
											{
												" RegionId=",
												context.Request["Shippingcity"],
												" and ModeId=",
												num4,
												" and TemplateId=",
												current4.FreightTemplateId,
												" and IsDefault=0"
											});
											if (dataView.Count == 1)
											{
												string a;
												if ((a = dataView[0]["MUnit"].ToString()) != null)
												{
													if (!(a == "1"))
													{
														if (!(a == "2"))
														{
															if (a == "3")
															{
																if (current4.CubicMeter > 0m)
																{
																	num6 += this.setferight(float.Parse(current4.CubicMeter.ToString()), float.Parse(dataView[0]["FristNumber"].ToString()), float.Parse(dataView[0]["FristPrice"].ToString()), float.Parse(dataView[0]["AddNumber"].ToString()), float.Parse(dataView[0]["AddPrice"].ToString()));
																}
															}
														}
														else if (current4.FreightWeight > 0m)
														{
															num6 += this.setferight(float.Parse(current4.FreightWeight.ToString()), float.Parse(dataView[0]["FristNumber"].ToString()), float.Parse(dataView[0]["FristPrice"].ToString()), float.Parse(dataView[0]["AddNumber"].ToString()), float.Parse(dataView[0]["AddPrice"].ToString()));
														}
													}
													else
													{
														num6 += this.setferight(float.Parse(current4.Quantity.ToString()), float.Parse(dataView[0]["FristNumber"].ToString()), float.Parse(dataView[0]["FristPrice"].ToString()), float.Parse(dataView[0]["AddNumber"].ToString()), float.Parse(dataView[0]["AddPrice"].ToString()));
													}
												}
											}
											else
											{
												dataView.RowFilter = string.Concat(new object[]
												{
													"  ModeId=",
													num4,
													" and TemplateId=",
													current4.FreightTemplateId,
													" and  IsDefault=1"
												});
												string a2;
												if (dataView.Count == 1 && (a2 = dataView[0]["MUnit"].ToString()) != null)
												{
													if (!(a2 == "1"))
													{
														if (!(a2 == "2"))
														{
															if (a2 == "3")
															{
																if (current4.CubicMeter > 0m)
																{
																	num6 += this.setferight(float.Parse(current4.CubicMeter.ToString()), float.Parse(dataView[0]["FristNumber"].ToString()), float.Parse(dataView[0]["FristPrice"].ToString()), float.Parse(dataView[0]["AddNumber"].ToString()), float.Parse(dataView[0]["AddPrice"].ToString()));
																}
															}
														}
														else if (current4.FreightWeight > 0m)
														{
															num6 += this.setferight(float.Parse(current4.FreightWeight.ToString()), float.Parse(dataView[0]["FristNumber"].ToString()), float.Parse(dataView[0]["FristPrice"].ToString()), float.Parse(dataView[0]["AddNumber"].ToString()), float.Parse(dataView[0]["AddPrice"].ToString()));
														}
													}
													else
													{
														num6 += this.setferight(float.Parse(current4.Quantity.ToString()), float.Parse(dataView[0]["FristNumber"].ToString()), float.Parse(dataView[0]["FristPrice"].ToString()), float.Parse(dataView[0]["AddNumber"].ToString()), float.Parse(dataView[0]["AddPrice"].ToString()));
													}
												}
											}
										}
									}
								}
							}
						}
						string s = num6.ToString("F2");
						orderInfo.AdjustedFreight = decimal.Parse(s);
					}
				}
				if (int.TryParse(context.Request["paymentType"], out num5))
				{
					orderInfo.PaymentTypeId = num5;
					if (num5 == 0 || num5 == -1)
					{
						orderInfo.PaymentType = "货到付款";
						orderInfo.Gateway = "hishop.plugins.payment.podrequest";
					}
					else if (num5 == 88)
					{
						orderInfo.PaymentType = "微信支付";
						orderInfo.Gateway = "hishop.plugins.payment.weixinrequest";
					}
					else if (num5 == 99)
					{
						orderInfo.PaymentType = "线下付款";
						orderInfo.Gateway = "hishop.plugins.payment.offlinerequest";
					}
					else
					{
						PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(num5);
						if (paymentMode != null)
						{
							orderInfo.PaymentTypeId = paymentMode.ModeId;
							orderInfo.PaymentType = paymentMode.Name;
							orderInfo.Gateway = paymentMode.Gateway;
						}
					}
				}
				if (!string.IsNullOrEmpty(couponCode) && bargainDetialId == 0)
				{
					CouponInfo couponInfo = ShoppingProcessor.UseCoupon(cart.GetTotal(), couponCode);
					orderInfo.CouponName = couponInfo.CouponName;
					if (couponInfo.ConditionValue > 0m)
					{
						orderInfo.CouponAmount = couponInfo.ConditionValue;
					}
					orderInfo.CouponCode = couponCode;
					orderInfo.CouponValue = couponInfo.CouponValue;
				}
				if (!string.IsNullOrEmpty(selectCouponValue) && selectCouponValue != "0" && bargainDetialId == 0)
				{
					orderInfo.RedPagerActivityName = selectCouponValue.Split(new char[]
					{
						'|'
					})[0];
					orderInfo.RedPagerID = new int?(int.Parse(selectCouponValue.Split(new char[]
					{
						'|'
					})[1]));
					orderInfo.RedPagerOrderAmountCanUse = decimal.Parse(selectCouponValue.Split(new char[]
					{
						'|'
					})[2]);
					orderInfo.RedPagerAmount = decimal.Parse(selectCouponValue.Split(new char[]
					{
						'|'
					})[3]);
					if (CouponHelper.CheckCouponsIsUsed(orderInfo.RedPagerID.Value))
					{
						stringBuilder.Append("\"Status\":\"Error\"");
						stringBuilder.AppendFormat(",\"ErrorMsg\":\"优惠券已被使用，请重新提交订单！\"", new object[0]);
						return stringBuilder.ToString();
					}
				}
				else
				{
					selectCouponValue = "";
				}
				orderInfo.PointToCash = 0m;
				orderInfo.PointExchange = 0;
				if (masterSettings.PonitToCash_Enable && bargainDetialId == 0)
				{
					if (PointExchange > 0)
					{
						int pointToCashRate = masterSettings.PointToCashRate;
						decimal ponitToCash_MaxAmount = masterSettings.PonitToCash_MaxAmount;
						float num7 = float.Parse(PointExchange.ToString()) / float.Parse(pointToCashRate.ToString());
						decimal total = orderInfo.GetTotal();
						if (decimal.Parse(num7.ToString()) > total || ponitToCash_MaxAmount < decimal.Parse(num7.ToString()))
						{
							stringBuilder.Append("\"Status\":\"Error\"");
							stringBuilder.AppendFormat(string.Concat(new object[]
							{
								",\"ErrorMsg\":\"抵现金额不能大于应付总额",
								total,
								"元,最高抵现金额",
								ponitToCash_MaxAmount,
								"元！\""
							}), new object[0]);
							return stringBuilder.ToString();
						}
						orderInfo.PointToCash = decimal.Parse(num7.ToString());
						orderInfo.PointExchange = PointExchange;
					}
				}
				else
				{
					PointExchange = 0;
				}
				try
				{
					//orderInfo.RedPagerAmount + orderInfo.PointToCash + orderInfo.DiscountAmount;
					this.SetOrderItemStatus(orderInfo, orderInfo.RedPagerAmount, orderInfo.PointToCash, orderInfo.DiscountAmount, empty);
					BargainInfo bargainInfoByDetialId = BargainHelper.GetBargainInfoByDetialId(orderInfo.BargainDetialId);
					if (bargainInfoByDetialId == null || (bargainInfoByDetialId != null && bargainInfoByDetialId.IsCommission))
					{
						if (orderInfo.BargainDetialId > 0)
						{
							BargainDetialInfo bargainDetialInfo = BargainHelper.GetBargainDetialInfo(orderInfo.BargainDetialId);
							if (bargainDetialInfo != null)
							{
								System.Collections.Generic.Dictionary<string, LineItemInfo>.ValueCollection values = orderInfo.LineItems.Values;
								foreach (LineItemInfo current5 in values)
								{
									current5.ItemAdjustedPrice = bargainDetialInfo.Price;
									current5.ItemListPrice = bargainDetialInfo.Price;
								}
							}
						}
						orderInfo = ShoppingProcessor.GetCalculadtionCommission(orderInfo);
					}
					else
					{
						orderInfo.ThirdCommission = 0m;
						orderInfo.SecondCommission = 0m;
						orderInfo.FirstCommission = 0m;
					}
					if (ShoppingProcessor.CreatOrder(orderInfo))
					{
						MemberHelper.SetOrderDate(orderInfo.UserId, 0);
						if (summittype)
						{
							ShoppingCartProcessor.ClearShoppingCart();
						}
						try
						{
							OrderInfo orderInfo2 = orderInfo;
							if (orderInfo2 != null)
							{
								Messenger.SendWeiXinMsg_OrderCreate(orderInfo2);
							}
						}
						catch (System.Exception ex)
						{
							string arg_F85_0 = ex.Message;
						}
						if (!string.IsNullOrEmpty(masterSettings.ManageOpenID))
						{
							MemberProcessor.GetOpenIdMember(masterSettings.ManageOpenID, "wx");
						}
						stringBuilder.Append("\"Status\":\"OK\",\"OrderMarkingStatus\":\"" + orderInfo.OrderMarking + "\",");
						stringBuilder.AppendFormat("\"OrderId\":\"{0}\"", orderInfo.OrderMarking);
					}
					else
					{
						stringBuilder.Append("\"Status\":\"Error\"");
						stringBuilder.AppendFormat(",\"ErrorMsg\":\"提交订单失败！\"", new object[0]);
					}
					goto IL_1027;
				}
				catch (OrderException ex2)
				{
					stringBuilder.Append("\"Status\":\"Error\"");
					stringBuilder.AppendFormat(",\"ErrorMsg\":\"{0}\"", ex2.Message);
					goto IL_1027;
				}
			}
			stringBuilder.Append("\"Status\":\"None\"");
			IL_1027:
			return stringBuilder.ToString();
		}

		private void ProcessSubmmitorder(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			if (currentMember == null)
			{
				stringBuilder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"请先登录！\"");
				stringBuilder.Append("}");
				context.Response.ContentType = "application/json";
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			int shippingId = 0;
			string couponCode = context.Request["couponCode"];
			int num = Globals.RequestFormNum("bargainDetialId");
			string[] array = context.Request["selectCouponValue"].Split(new char[]
			{
				','
			});
			string[] array2 = context.Request["PointNumber"].Split(new char[]
			{
				','
			});
			if (array2.Length > 0)
			{
				int num2 = 0;
				int num3 = 0;
				string[] array3 = array2;
				for (int i = 0; i < array3.Length; i++)
				{
					string s = array3[i];
					if (!int.TryParse(s, out num3))
					{
						stringBuilder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"输入参数不正确！\"");
						stringBuilder.Append("}");
						context.Response.ContentType = "application/json";
						context.Response.Write(stringBuilder.ToString());
						return;
					}
					num2 += num3;
				}
				if (num2 != 0 && num2 > currentMember.Points)
				{
					stringBuilder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"您当前积分不足！\"");
					stringBuilder.Append("}");
					context.Response.ContentType = "application/json";
					context.Response.Write(stringBuilder.ToString());
					return;
				}
			}
			if (num > 0)
			{
				int userId = currentMember.UserId;
				bool flag = OrderHelper.ExistsOrderByBargainDetialId(userId, num);
				if (flag)
				{
					stringBuilder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"您已经参加该活动，不能重复下单！\"");
					stringBuilder.Append("}");
					context.Response.ContentType = "application/json";
					context.Response.Write(stringBuilder.ToString());
					return;
				}
				string text = BargainHelper.IsCanBuyByBarginDetailId(num);
				if (text != "1")
				{
					stringBuilder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"" + text + ",不能提交订单！\"");
					stringBuilder.Append("}");
					context.Response.ContentType = "application/json";
					context.Response.Write(stringBuilder.ToString());
					return;
				}
			}
			string text2 = "";
			string text3 = "";
			shippingId = int.Parse(context.Request["shippingId"]);
			int num4;
			int.TryParse(context.Request["groupbuyId"], out num4);
			int num5 = 0;
			string remark = context.Request["remark"];
			string text4 = this.GenerateOrderId();
			int num6;
			if (int.TryParse(context.Request["buyAmount"], out num6) && !string.IsNullOrEmpty(context.Request["productSku"]) && !string.IsNullOrEmpty(context.Request["from"]) && (context.Request["from"] == "signBuy" || context.Request["from"] == "groupBuy"))
			{
				string text5 = context.Request["productSku"];
				if (!(context.Request["from"] == "signBuy"))
				{
					goto IL_64D;
				}
				System.Collections.Generic.List<ShoppingCartInfo> list = null;
				if (num > 0)
				{
					bool flag2 = BargainHelper.UpdateNumberById(num, num6, out num5);
					if (flag2)
					{
						list = ShoppingCartProcessor.GetListShoppingCart(text5, num5, num, 0);
					}
				}
				else
				{
					int num7 = Globals.RequestFormNum("limitedTimeDiscountId");
					int num8 = num6;
					if (num7 > 0)
					{
						bool flag3 = true;
						LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(num7);
						if (discountInfo == null)
						{
							flag3 = false;
						}
						if (flag3)
						{
							int limitedTimeDiscountUsedNum = ShoppingCartProcessor.GetLimitedTimeDiscountUsedNum(num7, text5, 0, currentMember.UserId, false);
							if (discountInfo.LimitNumber > 0 && num6 > discountInfo.LimitNumber - limitedTimeDiscountUsedNum)
							{
								if (MemberHelper.CheckCurrentMemberIsInRange(discountInfo.ApplyMembers, discountInfo.DefualtGroup, discountInfo.CustomGroup, currentMember.UserId))
								{
									num8 = discountInfo.LimitNumber - limitedTimeDiscountUsedNum;
								}
								else
								{
									num8 = 0;
								}
							}
						}
						if (num8 <= 0 || !flag3)
						{
							num7 = 0;
							num8 = num6;
						}
					}
					list = ShoppingCartProcessor.GetListShoppingCart(text5, num8, 0, num7);
				}
				if (list == null)
				{
					stringBuilder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"您选择的商品库存不足！\"");
					goto IL_64D;
				}
				using (System.Collections.Generic.List<ShoppingCartInfo>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ShoppingCartInfo current = enumerator.Current;
						string value = this.ordersummit(current, context, remark, shippingId, couponCode, array[0], context.Request["shippingType"], false, text4, current.LineItems, int.Parse(array2[0]), num, out text3);
						stringBuilder.Append(value);
						text2 = text3 + ",";
					}
					goto IL_64D;
				}
			}
			System.Collections.Generic.List<ShoppingCartInfo> list2 = null;
			list2 = ShoppingCartProcessor.GetOrderSummitCart();
			string[] array4 = context.Request["shippingType"].Split(new char[]
			{
				','
			});
			string[] array5 = context.Request["remark"].Split(new char[]
			{
				','
			});
			int num9 = 0;
			int num10 = 0;
			foreach (ShoppingCartInfo current2 in list2)
			{
				foreach (ShoppingCartItemInfo current3 in current2.LineItems)
				{
					if (current3.Type == 1)
					{
						num10 += current3.PointNumber;
					}
				}
			}
			if (num10 > currentMember.Points)
			{
				stringBuilder.Append("\"Status\":\"Eror\",\"ErrorMsg\":\"您当前积分不足！\"");
			}
			else
			{
				foreach (ShoppingCartInfo current4 in list2)
				{
					this.ordersummit(current4, context, array5[num9], shippingId, couponCode, array[num9], array4[num9], true, text4, current4.LineItems, int.Parse(array2[num9]), 0, out text3);
					text2 = text2 + text3 + ",";
					num9++;
				}
				stringBuilder.Append("\"Status\":\"OK\",\"OrderMarkingStatus\":\"1\",");
				stringBuilder.AppendFormat("\"OrderId\":\"{0}\"", text4);
			}
			IL_64D:
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = text2.Substring(0, text2.Length - 1);
				string[] array6 = text2.Split(new char[]
				{
					','
				});
				string[] array7 = array6;
				for (int j = 0; j < array7.Length; j++)
				{
					string text6 = array7[j];
					if (!string.IsNullOrEmpty(text6) && int.Parse(text6) > 0)
					{
						int hishop_Activities = ActivityHelper.GetHishop_Activities(int.Parse(text6));
						int userId2 = currentMember.UserId;
						ActivityHelper.AddActivitiesMember(hishop_Activities, userId2);
					}
				}
			}
			stringBuilder.Append("}");
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder.ToString());
		}

		public decimal DiscountMoney(System.Collections.Generic.IList<ShoppingCartItemInfo> LineItems, out string ActivitiesId, out string ActivitiesName, MemberInfo member, out int CouponId, out string vItemList)
		{
			decimal d = 0m;
			decimal num = 0m;
			decimal num2 = 0m;
			CouponId = 0;
			ActivitiesId = "";
			ActivitiesName = "";
			vItemList = "";
			decimal d2 = 0m;
			int num3 = 0;
			foreach (ShoppingCartItemInfo current in LineItems)
			{
				if (current.Type == 0)
				{
					d2 += current.SubTotal;
					num3 += current.Quantity;
				}
			}
			System.Data.DataTable activities = ActivityHelper.GetActivities();
			for (int i = 0; i < activities.Rows.Count; i++)
			{
				if (int.Parse(activities.Rows[i]["attendTime"].ToString()) == 0 || int.Parse(activities.Rows[i]["attendTime"].ToString()) > ActivityHelper.GetActivitiesMember(member.UserId, int.Parse(activities.Rows[i]["ActivitiesId"].ToString())))
				{
					decimal num4 = 0m;
					int num5 = 0;
					System.Data.DataTable activities_Detail = ActivityHelper.GetActivities_Detail(int.Parse(activities.Rows[i]["ActivitiesId"].ToString()));
					foreach (ShoppingCartItemInfo current2 in LineItems)
					{
						if (current2.Type == 0)
						{
							System.Data.DataTable activitiesProducts = ActivityHelper.GetActivitiesProducts(int.Parse(activities.Rows[i]["ActivitiesId"].ToString()), current2.ProductId);
							if (activitiesProducts.Rows.Count > 0)
							{
								num4 += current2.SubTotal;
								num5 += current2.Quantity;
								vItemList = vItemList + "," + current2.ProductId;
							}
						}
					}
					bool flag = false;
					if (activities_Detail.Rows.Count > 0)
					{
						for (int j = 0; j < activities_Detail.Rows.Count; j++)
						{
							string grades = activities_Detail.Rows[j]["MemberGrades"].ToString();
							string defualtGroup = activities_Detail.Rows[j]["DefualtGroup"].ToString();
							string customGroup = activities_Detail.Rows[j]["CustomGroup"].ToString();
							if (MemberProcessor.CheckCurrentMemberIsInRange(grades, defualtGroup, customGroup))
							{
								if (bool.Parse(activities.Rows[i]["isAllProduct"].ToString()))
								{
									if (decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString()) > 0m)
									{
										if (d2 != 0m && d2 >= decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString()))
										{
											num = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString());
											d = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ReductionMoney"].ToString());
											ActivitiesId = activities_Detail.Rows[activities_Detail.Rows.Count - 1]["id"].ToString();
											ActivitiesName = activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ActivitiesName"].ToString();
											CouponId = int.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["CouponId"].ToString());
											break;
										}
										if (d2 != 0m && d2 <= decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString()))
										{
											num = decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString());
											d = decimal.Parse(activities_Detail.Rows[0]["ReductionMoney"].ToString());
											ActivitiesId = activities_Detail.Rows[0]["id"].ToString();
											ActivitiesName = activities_Detail.Rows[0]["ActivitiesName"].ToString();
											CouponId = 0;
											break;
										}
										if (d2 != 0m && d2 >= decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString()))
										{
											num = decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString());
											d = decimal.Parse(activities_Detail.Rows[j]["ReductionMoney"].ToString());
											ActivitiesId = activities_Detail.Rows[j]["id"].ToString();
											ActivitiesName = activities_Detail.Rows[j]["ActivitiesName"].ToString();
											CouponId = int.Parse(activities_Detail.Rows[j]["CouponId"].ToString());
										}
									}
									else
									{
										if (num3 != 0 && num3 >= int.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetNumber"].ToString()))
										{
											num = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString());
											num2 = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ReductionMoney"].ToString());
											ActivitiesId = activities_Detail.Rows[activities_Detail.Rows.Count - 1]["id"].ToString();
											ActivitiesName = activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ActivitiesName"].ToString();
											CouponId = int.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["CouponId"].ToString());
											flag = true;
											break;
										}
										if (num3 != 0 && num3 <= int.Parse(activities_Detail.Rows[0]["MeetNumber"].ToString()))
										{
											num = decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString());
											num2 = decimal.Parse(activities_Detail.Rows[0]["ReductionMoney"].ToString());
											ActivitiesId = activities_Detail.Rows[0]["id"].ToString();
											ActivitiesName = activities_Detail.Rows[0]["ActivitiesName"].ToString();
											CouponId = 0;
											flag = true;
											break;
										}
										if (num3 != 0 && num3 >= int.Parse(activities_Detail.Rows[j]["MeetNumber"].ToString()))
										{
											num = decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString());
											num2 = decimal.Parse(activities_Detail.Rows[j]["ReductionMoney"].ToString());
											ActivitiesId = activities_Detail.Rows[j]["id"].ToString();
											ActivitiesName = activities_Detail.Rows[j]["ActivitiesName"].ToString();
											CouponId = int.Parse(activities_Detail.Rows[j]["CouponId"].ToString());
											flag = true;
										}
									}
								}
								else
								{
									d2 = num4;
									num3 = num5;
									if (decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString()) > 0m)
									{
										if (d2 != 0m && d2 >= decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString()))
										{
											num = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString());
											d = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ReductionMoney"].ToString());
											ActivitiesId = activities_Detail.Rows[activities_Detail.Rows.Count - 1]["id"].ToString();
											ActivitiesName = activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ActivitiesName"].ToString();
											CouponId = int.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["CouponId"].ToString());
											break;
										}
										if (d2 != 0m && d2 <= decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString()))
										{
											num = decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString());
											d = decimal.Parse(activities_Detail.Rows[0]["ReductionMoney"].ToString());
											ActivitiesId = activities_Detail.Rows[0]["id"].ToString();
											ActivitiesName = activities_Detail.Rows[0]["ActivitiesName"].ToString();
											CouponId = 0;
											break;
										}
										if (d2 != 0m && d2 >= decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString()))
										{
											num = decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString());
											d = decimal.Parse(activities_Detail.Rows[j]["ReductionMoney"].ToString());
											ActivitiesId = activities_Detail.Rows[j]["id"].ToString();
											ActivitiesName = activities_Detail.Rows[j]["ActivitiesName"].ToString();
											CouponId = int.Parse(activities_Detail.Rows[j]["CouponId"].ToString());
										}
									}
									else
									{
										if (num3 != 0 && num3 >= int.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetNumber"].ToString()))
										{
											num = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["MeetMoney"].ToString());
											d = decimal.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ReductionMoney"].ToString());
											ActivitiesId = activities_Detail.Rows[activities_Detail.Rows.Count - 1]["id"].ToString();
											ActivitiesName = activities_Detail.Rows[activities_Detail.Rows.Count - 1]["ActivitiesName"].ToString();
											CouponId = int.Parse(activities_Detail.Rows[activities_Detail.Rows.Count - 1]["CouponId"].ToString());
											flag = true;
											break;
										}
										if (num3 != 0 && num3 <= int.Parse(activities_Detail.Rows[0]["MeetNumber"].ToString()))
										{
											num = decimal.Parse(activities_Detail.Rows[0]["MeetMoney"].ToString());
											d = decimal.Parse(activities_Detail.Rows[0]["ReductionMoney"].ToString());
											ActivitiesId = activities_Detail.Rows[0]["id"].ToString();
											ActivitiesName = activities_Detail.Rows[0]["ActivitiesName"].ToString();
											CouponId = 0;
											flag = true;
											break;
										}
										if (num3 != 0 && num3 >= int.Parse(activities_Detail.Rows[j]["MeetNumber"].ToString()))
										{
											num = decimal.Parse(activities_Detail.Rows[j]["MeetMoney"].ToString());
											d = decimal.Parse(activities_Detail.Rows[j]["ReductionMoney"].ToString());
											ActivitiesId = activities_Detail.Rows[j]["id"].ToString();
											ActivitiesName = activities_Detail.Rows[j]["ActivitiesName"].ToString();
											CouponId = int.Parse(activities_Detail.Rows[j]["CouponId"].ToString());
											flag = true;
										}
									}
								}
							}
						}
						if (flag)
						{
							if (num3 > 0)
							{
								num2 += d;
							}
						}
						else if (d2 != 0m && num != 0m && d2 >= num)
						{
							num2 += d;
						}
					}
				}
			}
			return num2;
		}

		public void SetOrderItemStatus(OrderInfo order, decimal redPagerAmount, decimal pointDiscountAverage, decimal DiscountAverage, string productItemList)
		{
			productItemList = productItemList.Trim(new char[]
			{
				','
			});
			if (!string.IsNullOrEmpty(productItemList))
			{
				productItemList = "," + productItemList + ",";
			}
			decimal d = 0m;
			decimal d2 = 0m;
			decimal d3 = 0m;
			System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = order.LineItems;
			LineItemInfo lineItemInfo = new LineItemInfo();
			string text = string.Empty;
			if (order.RedPagerID.HasValue)
			{
				text = CouponHelper.GetCouponsProductIdsByMemberCouponIDByRedPagerId(order.RedPagerID.Value);
				if (!string.IsNullOrEmpty(text))
				{
					text = "_" + text.Trim(new char[]
					{
						'_'
					}) + "_";
				}
			}
			foreach (System.Collections.Generic.KeyValuePair<string, LineItemInfo> current in lineItems)
			{
				lineItemInfo = current.Value;
				lineItemInfo.OrderItemsStatus = OrderStatus.WaitBuyerPay;
				if (lineItemInfo.Type == 0)
				{
					decimal subTotal = lineItemInfo.GetSubTotal();
					d += subTotal;
					if (string.IsNullOrEmpty(productItemList) || productItemList.Contains("," + lineItemInfo.ProductId.ToString() + ","))
					{
						d2 += subTotal;
					}
					if (string.IsNullOrEmpty(text) || text.Contains("_" + lineItemInfo.ProductId.ToString() + "_"))
					{
						d3 += subTotal;
					}
				}
			}
			if (lineItems.Count > 1)
			{
				if (!(redPagerAmount > 0m) && !(DiscountAverage > 0m) && !(pointDiscountAverage > 0m))
				{
					return;
				}
				using (System.Collections.Generic.Dictionary<string, LineItemInfo>.Enumerator enumerator2 = lineItems.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						System.Collections.Generic.KeyValuePair<string, LineItemInfo> current2 = enumerator2.Current;
						lineItemInfo = current2.Value;
						if (lineItemInfo.Type == 0)
						{
							float num = float.Parse(lineItemInfo.GetSubTotal().ToString()) * float.Parse(pointDiscountAverage.ToString()) / float.Parse(d.ToString());
							if (string.IsNullOrEmpty(productItemList) || productItemList.Contains("," + lineItemInfo.ProductId.ToString() + ","))
							{
								num += float.Parse(lineItemInfo.GetSubTotal().ToString()) * float.Parse(DiscountAverage.ToString()) / float.Parse(d2.ToString());
							}
							if (string.IsNullOrEmpty(text) || text.Contains("_" + lineItemInfo.ProductId.ToString() + "_"))
							{
								num += float.Parse(lineItemInfo.GetSubTotal().ToString()) * float.Parse(redPagerAmount.ToString()) / float.Parse(d3.ToString());
							}
							lineItemInfo.DiscountAverage = System.Convert.ToDecimal(num);
						}
						else
						{
							lineItemInfo.DiscountAverage = 0m;
						}
					}
					return;
				}
			}
			if (lineItems.Count == 1)
			{
				foreach (System.Collections.Generic.KeyValuePair<string, LineItemInfo> arg_346_0 in lineItems)
				{
					if (lineItemInfo.Type == 0)
					{
						lineItemInfo.DiscountAverage = redPagerAmount + pointDiscountAverage + DiscountAverage;
					}
					else
					{
						lineItemInfo.DiscountAverage = 0m;
					}
				}
			}
		}

		private void Vote(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int voteId = 1;
			int.TryParse(context.Request["voteId"], out voteId);
			string text = context.Request["itemIds"];
			text = text.Remove(text.Length - 1);
			if (MemberProcessor.GetCurrentMember() == null)
			{
				MemberInfo memberInfo = new MemberInfo();
				string generateId = Globals.GetGenerateId();
				memberInfo.ReferralUserId = Globals.GetCurrentDistributorId();
				memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
				memberInfo.UserName = "";
				memberInfo.OpenId = "";
				memberInfo.CreateDate = System.DateTime.Now;
				memberInfo.SessionId = generateId;
				memberInfo.SessionEndTime = System.DateTime.Now;
				MemberProcessor.CreateMember(memberInfo);
				memberInfo = MemberProcessor.GetMember(generateId);
				this.setLogin(memberInfo.UserId);
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			if (VshopBrowser.Vote(voteId, text))
			{
				stringBuilder.Append("\"Status\":\"OK\"");
			}
			else
			{
				stringBuilder.Append("\"Status\":\"Error\"");
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public void RequestReturn(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			decimal refundMoney = decimal.Parse(context.Request["Money"]);
			RefundInfo refundInfo = new RefundInfo();
			refundInfo.OrderId = context.Request["orderid"];
			refundInfo.ApplyForTime = System.DateTime.Now;
			refundInfo.Comments = context.Request["Reason"];
			refundInfo.HandleStatus = RefundInfo.Handlestatus.NoneAudit;
			refundInfo.Account = context.Request["Account"];
			refundInfo.RefundMoney = refundMoney;
			refundInfo.SkuId = context.Request["skuid"];
			refundInfo.ProductId = int.Parse(context.Request["productid"]);
			refundInfo.OrderItemID = Globals.RequestFormNum("orderitemid");
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			refundInfo.UserId = currentMember.UserId;
			int orderItemsStatus = 7;
			refundInfo.RefundType = 1;
			if (int.Parse(context.Request["OrderStatus"].ToString()) == 2)
			{
				orderItemsStatus = 6;
				refundInfo.HandleStatus = RefundInfo.Handlestatus.NoRefund;
				refundInfo.RefundType = 2;
				refundInfo.AuditTime = System.DateTime.Now.ToString();
			}
			stringBuilder.Append("{");
			if (!string.IsNullOrEmpty(refundInfo.Account.Trim()))
			{
				if (!ShoppingProcessor.GetReturnInfo(refundInfo.UserId, refundInfo.OrderId, refundInfo.ProductId, refundInfo.SkuId))
				{
					if (ShoppingProcessor.InsertOrderRefund(refundInfo))
					{
						if (ShoppingProcessor.UpdateOrderGoodStatu(refundInfo.OrderId, refundInfo.SkuId, orderItemsStatus, refundInfo.OrderItemID))
						{
							try
							{
								this.myNotifier.updateAction = UpdateAction.OrderUpdate;
								this.myNotifier.actionDesc = "申请退货或退款";
								this.myNotifier.RecDateUpdate = System.DateTime.Today;
								this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
								this.myNotifier.UpdateDB();
							}
							catch (System.Exception)
							{
							}
							if (refundInfo.RefundType != 1)
							{
								if (refundInfo.RefundType != 2)
								{
									goto IL_257;
								}
							}
							try
							{
								OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(refundInfo.OrderId);
								if (orderInfo != null)
								{
									orderInfo.RefundRemark = refundInfo.Comments.Replace("\r", "").Replace("\n", "");
									Messenger.SendWeiXinMsg_ServiceRequest(orderInfo, refundInfo.RefundType);
								}
							}
							catch (System.Exception)
							{
							}
							IL_257:
							stringBuilder.Append("\"Status\":\"OK\"");
						}
						else
						{
							stringBuilder.Append("\"Status\":\"Error\"");
						}
					}
					else
					{
						stringBuilder.Append("\"Status\":\"Error\"");
					}
				}
				else
				{
					stringBuilder.Append("\"Status\":\"Repeat\"");
				}
			}
			else
			{
				stringBuilder.Append("\"Status\":\"Mesg\"");
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public void SetUserName(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			currentMember.UserName = context.Request["userName"];
			currentMember.VipCardDate = new System.DateTime?(System.DateTime.Now);
			currentMember.CellPhone = context.Request["CellPhone"];
			currentMember.QQ = context.Request["QQ"];
			if (!string.IsNullOrEmpty(currentMember.QQ))
			{
				currentMember.Email = currentMember.QQ + "@qq.com";
			}
			currentMember.RealName = context.Request["RealName"];
			currentMember.CardID = context.Request["CardID"];
			if (!string.IsNullOrEmpty(context.Request["userHead"]))
			{
				currentMember.UserHead = context.Request["userHead"];
			}
			new DistributorsInfo();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			if (MemberProcessor.UpdateMember(currentMember))
			{
				stringBuilder.Append("\"Status\":\"OK\"");
			}
			else
			{
				stringBuilder.Append("\"Status\":\"Error\"");
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public void SetDistributorMsg(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			currentMember.VipCardDate = new System.DateTime?(System.DateTime.Now);
			currentMember.CellPhone = context.Request["CellPhone"];
			currentMember.MicroSignal = context.Request["MicroSignal"];
			currentMember.RealName = context.Request["RealName"];
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			if (MemberProcessor.UpdateMember(currentMember))
			{
				stringBuilder.Append("\"Status\":\"OK\"");
			}
			else
			{
				stringBuilder.Append("\"Status\":\"Error\"");
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		public string GenerateOrderId()
		{
			return Globals.GetGenerateId();
		}

		public void GetShippingTypes(System.Web.HttpContext context)
		{
			ShoppingCartInfo shoppingCartInfo = null;
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (int.TryParse(context.Request["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(context.Request["from"]) && context.Request["from"] == "signBuy")
			{
				this.productSku = context.Request["productSku"];
				shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
			}
			else
			{
				int templateid = 0;
				if (!string.IsNullOrEmpty(context.Request["TemplateId"]) && int.TryParse(context.Request["TemplateId"], out templateid))
				{
					shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(templateid);
				}
			}
			System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
			string regionId = context.Request["city"];
			context.Response.ContentType = "application/json";
			string text = "";
			foreach (ShoppingCartItemInfo current in shoppingCartInfo.LineItems)
			{
				if (current.FreightTemplateId > 0)
				{
					text = text + current.FreightTemplateId + ",";
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Substring(0, text.Length - 1);
				System.Data.DataTable specifyRegionGroupsModeId = SettingsHelper.GetSpecifyRegionGroupsModeId(text, regionId);
				if (specifyRegionGroupsModeId.Rows.Count > 0)
				{
					for (int i = 0; i < specifyRegionGroupsModeId.Rows.Count; i++)
					{
						string modelType = this.getModelType(int.Parse(specifyRegionGroupsModeId.Rows[i]["ModeId"].ToString()));
						stringBuilder2.Append(string.Concat(new object[]
						{
							",{\"modelId\":\"",
							specifyRegionGroupsModeId.Rows[i]["ModeId"],
							"\",\"text\":\"",
							modelType,
							"\"}"
						}));
					}
				}
				else
				{
					stringBuilder2.Append(",{\"modelId\":\"0\",\"text\":\"包邮\"}");
				}
				stringBuilder.Append(stringBuilder2.ToString() ?? "");
			}
			else
			{
				stringBuilder2.Append(",{\"modelId\":\"0\",\"text\":\"包邮\"}");
			}
			if (stringBuilder2.Length > 0)
			{
				stringBuilder2.Remove(0, 1);
			}
			stringBuilder2.Insert(0, "{\"data\":[").Append("]}");
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder2.ToString());
		}

		public void AddDistributor(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (this.CheckRequestDistributors(context, stringBuilder))
			{
				if (!SystemAuthorizationHelper.CheckDistributorIsCanAuthorization())
				{
					context.Response.Write("{\"success\":false,\"msg\":\"平台分销商数已达上限，请联系商家客服！\"}");
					return;
				}
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				if (currentMember == null)
				{
					context.Response.Write("{\"success\":false,\"msg\":\"请先登录再申请！\"}");
					return;
				}
				bool flag = false;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				if (!masterSettings.DistributorApplicationCondition)
				{
					flag = true;
				}
				else
				{
					int finishedOrderMoney = masterSettings.FinishedOrderMoney;
					if (finishedOrderMoney > 0)
					{
						System.Data.DataTable userOrderPaidWaitFinish = OrderHelper.GetUserOrderPaidWaitFinish(currentMember.UserId);
						decimal num = 0m;
						for (int i = 0; i < userOrderPaidWaitFinish.Rows.Count; i++)
						{
							OrderInfo orderInfo = OrderHelper.GetOrderInfo(userOrderPaidWaitFinish.Rows[i]["orderid"].ToString());
							if (orderInfo != null)
							{
								decimal total = orderInfo.GetTotal();
								if (total > 0m)
								{
									num += total;
								}
							}
						}
						if (currentMember.Expenditure + num >= finishedOrderMoney)
						{
							flag = true;
						}
					}
					if (!flag && masterSettings.EnableDistributorApplicationCondition && !string.IsNullOrEmpty(masterSettings.DistributorProductsDate) && !string.IsNullOrEmpty(masterSettings.DistributorProducts) && masterSettings.DistributorProductsDate.Contains("|"))
					{
						System.DateTime value = default(System.DateTime);
						System.DateTime value2 = default(System.DateTime);
						bool flag2 = System.DateTime.TryParse(masterSettings.DistributorProductsDate.Split(new char[]
						{
							'|'
						})[0].ToString(), out value);
						bool flag3 = System.DateTime.TryParse(masterSettings.DistributorProductsDate.Split(new char[]
						{
							'|'
						})[1].ToString(), out value2);
						if (flag2 && flag3 && System.DateTime.Now.CompareTo(value) >= 0 && System.DateTime.Now.CompareTo(value2) < 0 && MemberProcessor.CheckMemberIsBuyProds(currentMember.UserId, masterSettings.DistributorProducts, new System.DateTime?(value), new System.DateTime?(value2)))
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					context.Response.Write("{\"success\":false,\"msg\":\"您未达到申请分销商的条件！\"}");
					return;
				}
				DistributorsInfo distributorsInfo = new DistributorsInfo();
				distributorsInfo.RequestAccount = context.Request["acctount"].Trim();
				distributorsInfo.StoreName = context.Request["stroename"].Trim();
				distributorsInfo.StoreDescription = context.Request["descriptions"].Trim();
				distributorsInfo.Logo = context.Request["logo"].Trim();
				distributorsInfo.BackImage = "";
				distributorsInfo.CellPhone = context.Request["CellPhone"].Trim();
				DistributorGradeInfo isDefaultDistributorGradeInfo = DistributorGradeBrower.GetIsDefaultDistributorGradeInfo();
				if (isDefaultDistributorGradeInfo == null)
				{
					context.Response.Write("{\"success\":false,\"msg\":\"默认分销商等级未设置，请联系商家客服！\"}");
					return;
				}
				distributorsInfo.DistriGradeId = isDefaultDistributorGradeInfo.GradeId;
				if (DistributorsBrower.AddDistributors(distributorsInfo))
				{
					DistributorsBrower.DistributorGradeChange(distributorsInfo, "", distributorsInfo.DistriGradeId);
					System.Web.HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies["Vshop-Member"];
					if (httpCookie != null)
					{
						string name = "Vshop-ReferralId";
						System.Web.HttpContext.Current.Response.Cookies[name].Expires = System.DateTime.Now.AddDays(-1.0);
						System.Web.HttpCookie httpCookie2 = new System.Web.HttpCookie(name);
						httpCookie2.Value = Globals.GetCurrentMemberUserId().ToString();
						httpCookie2.Expires = System.DateTime.Now.AddYears(10);
						System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie2);
					}
					this.myNotifier.updateAction = UpdateAction.MemberUpdate;
					this.myNotifier.actionDesc = "会员申请成为分销商";
					this.myNotifier.RecDateUpdate = System.DateTime.Today;
					this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
					this.myNotifier.UpdateDB();
					context.Response.Write("{\"success\":true}");
					try
					{
						DistributorsInfo distributorsInfo2 = distributorsInfo;
						if (distributorsInfo2 != null)
						{
							Messenger.SendWeiXinMsg_DistributorCreate(distributorsInfo2, MemberProcessor.GetCurrentMember());
						}
						return;
					}
					catch (System.Exception)
					{
						return;
					}
				}
				context.Response.Write("{\"success\":false,\"msg\":\"店铺名称已存在，请重新输入店铺名称\"}");
				return;
			}
			else
			{
				context.Response.Write("{\"success\":false,\"msg\":\"" + stringBuilder.ToString() + "\"}");
			}
		}

		private bool CheckRequestDistributors(System.Web.HttpContext context, System.Text.StringBuilder sb)
		{
			if (string.IsNullOrEmpty(context.Request["stroename"]))
			{
				sb.AppendFormat("请输入店铺名称", new object[0]);
				return false;
			}
			if (context.Request["stroename"].Length > 20)
			{
				sb.AppendFormat("请输入店铺名称字符不多于20个字符", new object[0]);
				return false;
			}
			if (!string.IsNullOrEmpty(context.Request["descriptions"]) && context.Request["descriptions"].Trim().Length > 30)
			{
				sb.AppendFormat("店铺描述字不能多于30个字", new object[0]);
				return false;
			}
			return true;
		}

		private bool CheckUpdateDistributors(System.Web.HttpContext context, System.Text.StringBuilder sb)
		{
			if (string.IsNullOrEmpty(context.Request["stroename"]))
			{
				sb.AppendFormat("请输入店铺名称", new object[0]);
				return false;
			}
			if (context.Request["stroename"].Length > 20)
			{
				sb.AppendFormat("请输入店铺名称字符不多于20个字符", new object[0]);
				return false;
			}
			if (!string.IsNullOrEmpty(context.Request["descriptions"]) && context.Request["descriptions"].Trim().Length > 30)
			{
				sb.AppendFormat("店铺描述字不能多于30个字", new object[0]);
				return false;
			}
			return true;
		}

		private void DeleteDistributorProducts(System.Web.HttpContext context)
		{
			if (!string.IsNullOrEmpty(context.Request["Params"]))
			{
				string json = context.Request["Params"];
				JObject jObject = JObject.Parse(json);
				if (jObject.Count > 0)
				{
					System.Collections.Generic.List<int> productList = (from s in jObject.Values()
					select System.Convert.ToInt32(s)).ToList<int>();
					DistributorsBrower.DeleteDistributorProductIds(productList);
				}
			}
			context.Response.Write("{\"success\":\"true\",\"msg\":\"保存成功\"}");
			context.Response.End();
		}

		private void AddDistributorProducts(System.Web.HttpContext context)
		{
			if (!string.IsNullOrEmpty(context.Request["Params"]))
			{
				string json = context.Request["Params"];
				JObject jObject = JObject.Parse(json);
				if (jObject.Count > 0)
				{
					System.Collections.Generic.List<int> productList = (from s in jObject.Values()
					select System.Convert.ToInt32(s)).ToList<int>();
					DistributorsBrower.AddDistributorProductId(productList);
				}
			}
			context.Response.Write("{\"success\":\"true\",\"msg\":\"保存成功\"}");
			context.Response.End();
		}

		private void OperateAllDistributorProducts(System.Web.HttpContext context)
		{
			string text = context.Request["deleteAll"];
			if (string.IsNullOrEmpty(text))
			{
				text = "false";
			}
			if (text == "true")
			{
				this.AutoDeleteDistributorProducts();
			}
			else
			{
				this.AutoAddDistributorProducts();
			}
			context.Response.Write("{\"success\":\"true\",\"msg\":\"保存成功\"}");
			context.Response.End();
		}

		private bool AutoDeleteDistributorProducts()
		{
			int num = 0;
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			System.Data.DataTable products = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, new int?(0), "", 1, 100000, out num, "DisplaySequence", "desc", true);
			foreach (System.Data.DataRow dataRow in products.Rows)
			{
				int item = (int)dataRow["ProductId"];
				list.Add(item);
			}
			if (list.Count > 0)
			{
				DistributorsBrower.DeleteDistributorProductIds(list);
				return true;
			}
			return false;
		}

		private bool AutoAddDistributorProducts()
		{
			int num = 0;
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			System.Data.DataTable products = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, new int?(0), "", 1, 10000, out num, "DisplaySequence", "desc", false);
			foreach (System.Data.DataRow dataRow in products.Rows)
			{
				int item = (int)dataRow["ProductId"];
				list.Add(item);
			}
			if (list.Count > 0)
			{
				DistributorsBrower.AddDistributorProductId(list);
				return true;
			}
			return false;
		}

		private void UpdateDistributor(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (!this.CheckUpdateDistributors(context, stringBuilder))
			{
				context.Response.Write("{\"success\":false,\"msg\":\"" + stringBuilder.ToString() + "\"}");
				return;
			}
			DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId(), true);
			currentDistributors.StoreName = context.Request["stroename"].Trim();
			currentDistributors.StoreDescription = context.Request["descriptions"].Trim();
			currentDistributors.RequestAccount = context.Request["accountname"].Trim();
			currentDistributors.Logo = context.Request["logo"].Trim();
			currentDistributors.CellPhone = context.Request["CellPhone"].Trim();
			if (DistributorsBrower.UpdateDistributorMessage(currentDistributors))
			{
				DistributorsBrower.UpdateStoreCard(currentDistributors.UserId, "");
				context.Response.Write("{\"success\":true}");
				return;
			}
			context.Response.Write("{\"success\":false,\"msg\":\"店铺名称已存在，请重新命名!\"}");
		}

		private void AddCommissions(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/json";
			string s = "";
			if (!DistributorsBrower.IsExitsCommionsRequest())
			{
				if (this.CheckAddCommissions(context, ref s))
				{
					string merchantCode = context.Request["account"].Trim();
					decimal amount = decimal.Parse(context.Request["commissionmoney"].Trim());
					int num = 0;
					int.TryParse(context.Request["requesttype"].Trim(), out num);
					string text = context.Request["realname"].Trim();
					string bankName = context.Request["bankname"].Trim();
					BalanceDrawRequestInfo balanceDrawRequestInfo = new BalanceDrawRequestInfo();
					balanceDrawRequestInfo.MerchantCode = merchantCode;
					balanceDrawRequestInfo.Amount = amount;
					balanceDrawRequestInfo.RequesType = num;
					MemberInfo currentMember = MemberProcessor.GetCurrentMember();
					if (text == "")
					{
						text = currentMember.RealName;
					}
					else
					{
						currentMember.RealName = text;
					}
					DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(currentMember.UserId);
					if (distributorInfo != null)
					{
						balanceDrawRequestInfo.StoreName = distributorInfo.StoreName;
					}
					balanceDrawRequestInfo.AccountName = text;
					balanceDrawRequestInfo.BankName = bankName;
					if ((string.IsNullOrEmpty(currentMember.OpenId) || currentMember.OpenId.Length < 28) && (num == 3 || num == 0))
					{
						s = "{\"success\":false,\"msg\":\"您的帐号未绑定，无法通过微信支付佣金！\"}";
					}
					else if (DistributorsBrower.AddBalanceDrawRequest(balanceDrawRequestInfo, currentMember))
					{
						try
						{
							this.myNotifier.updateAction = UpdateAction.OrderUpdate;
							this.myNotifier.actionDesc = "申请提现";
							this.myNotifier.RecDateUpdate = System.DateTime.Today;
							this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
							this.myNotifier.UpdateDB();
						}
						catch (System.Exception)
						{
						}
						try
						{
							BalanceDrawRequestInfo balanceDrawRequestInfo2 = balanceDrawRequestInfo;
							if (balanceDrawRequestInfo2 != null)
							{
								Messenger.SendWeiXinMsg_DrawCashRequest(balanceDrawRequestInfo2);
							}
						}
						catch (System.Exception)
						{
						}
						s = "{\"success\":true,\"msg\":\"申请成功！\"}";
					}
					else
					{
						s = "{\"success\":false,\"msg\":\"真实姓名或手机号未填写！\"}";
					}
				}
			}
			else
			{
				s = "{\"success\":false,\"msg\":\"您有申请正在审核中！\"}";
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private bool CheckAddCommissions(System.Web.HttpContext context, ref string msg)
		{
			int num = 0;
			if (!int.TryParse(context.Request["requesttype"], out num))
			{
				num = 1;
			}
			string text = context.Request["bankname"].Trim();
			string text2 = context.Request["account"];
			if (num == 1 && !Globals.CheckReg(text2, "^1\\d{10}$") && !Globals.CheckReg(text2, "^(\\w-*\\.*)+@(\\w-?)+(\\.\\w{2,})+$"))
			{
				msg = "{\"success\":false,\"msg\":\"支付宝账号格式不正确！\"}";
				return false;
			}
			if (num == 2 && text2.Length < 4)
			{
				msg = "{\"success\":false,\"msg\":\"收款帐号不能为空，请准确填写！\"}";
				return false;
			}
			if (num == 2 && text.Length < 2)
			{
				msg = "{\"success\":false,\"msg\":\"帐号说明不能为空！\"}";
				return false;
			}
			if (string.IsNullOrEmpty(context.Request["commissionmoney"].Trim()))
			{
				msg = "{\"success\":false,\"msg\":\"提现金额不允许为空！\"}";
				return false;
			}
			if (decimal.Parse(context.Request["commissionmoney"].Trim()) <= 0m)
			{
				msg = "{\"success\":false,\"msg\":\"提现金额必须大于0的纯数字！\"}";
				return false;
			}
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[0-9]*[1-9][0-9]*$");
			if (!regex.IsMatch(context.Request["commissionmoney"].Trim()))
			{
				msg = "{\"success\":false,\"msg\":\"请输入正整数！\"}";
				return false;
			}
			decimal d = 0m;
			decimal.TryParse(SettingsManager.GetMasterSettings(false).MentionNowMoney, out d);
			if (d > 0m && decimal.Parse(context.Request["commissionmoney"].Trim()) < 0m)
			{
				msg = "{\"success\":false,\"msg\":\"提现金额必须大于等于" + d.ToString() + "元！\"}";
				return false;
			}
			DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(false);
			if (decimal.Parse(context.Request["commissionmoney"].Trim()) > currentDistributors.ReferralBlance)
			{
				msg = "{\"success\":false,\"msg\":\"提现金额必须为小于现有佣金余额！\"}";
				return false;
			}
			return true;
		}

		private void AdjustCommissions(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/json";
			string s = "";
			if (this.CheckAjustCommissions(context, ref s))
			{
				decimal adjustcommssion = 0m;
				decimal commssionmoney = 0m;
				decimal.TryParse(context.Request["adjustprice"], out adjustcommssion);
				decimal.TryParse(context.Request["commssionprice"], out commssionmoney);
				string text = ShoppingProcessor.UpdateAdjustCommssions(context.Request["orderId"], context.Request["skuId"], commssionmoney, adjustcommssion);
				if (text == "1")
				{
					s = "{\"success\":true,\"msg\":\"修改金额成功！\"}";
				}
				else
				{
					s = "{\"success\":false,\"msg\":\"优惠金额修改失败！原因是：" + text + "\"}";
				}
			}
			context.Response.Write(s);
			context.Response.End();
		}

		private bool CheckAjustCommissions(System.Web.HttpContext context, ref string msg)
		{
			if (string.IsNullOrEmpty(context.Request["orderId"]))
			{
				msg = "{\"success\":false,\"msg\":\"订单号不允许为空！\"}";
				return false;
			}
			if (string.IsNullOrEmpty(context.Request["skuId"]))
			{
				msg = "{\"success\":false,\"msg\":\"商品规格不允许为空！\"}";
				return false;
			}
			if (string.IsNullOrEmpty(context.Request["adjustprice"]))
			{
				msg = "{\"success\":false,\"msg\":\"请输入要调整的价格！\"}";
				return false;
			}
			if (string.IsNullOrEmpty(context.Request["commssionprice"]))
			{
				msg = "{\"success\":false,\"msg\":\"佣金金额值不对！\"}";
				return false;
			}
			if (System.Convert.ToDecimal(context.Request["adjustprice"]) < 0m || System.Convert.ToDecimal(context.Request["ajustprice"]) > System.Convert.ToDecimal(context.Request["commssionprice"]))
			{
				msg = "{\"success\":false,\"msg\":\"输入金额必须在0~" + context.Request["commssionprice"].ToString() + "之间！\"}";
				return false;
			}
			return true;
		}

		private void setLogin(int UserId)
		{
			System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Vshop-Member");
			httpCookie.Value = UserId.ToString();
			httpCookie.Expires = System.DateTime.Now.AddYears(1);
			System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
			System.Web.HttpCookie httpCookie2 = new System.Web.HttpCookie("Vshop-Member-Verify");
			httpCookie2.Value = Globals.EncryptStr(UserId.ToString());
			httpCookie2.Expires = System.DateTime.Now.AddYears(1);
			System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie2);
			System.Web.HttpContext.Current.Session["userid"] = UserId.ToString();
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(UserId);
			if (userIdDistributors != null && userIdDistributors.UserId > 0)
			{
				Globals.SetDistributorCookie(userIdDistributors.UserId);
			}
		}
	}
}
