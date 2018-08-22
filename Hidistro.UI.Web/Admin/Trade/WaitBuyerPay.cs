using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.StatisticsReport;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Trade
{
	public class WaitBuyerPay : AdminPage
	{
		private StatisticNotifier myNotifier = new StatisticNotifier();

		private UpdateStatistics myEvent = new UpdateStatistics();

		protected string Reurl = string.Empty;

		protected System.Web.UI.WebControls.HyperLink hlinkAllOrder;

		protected System.Web.UI.WebControls.TextBox txtOrderId;

		protected System.Web.UI.WebControls.Label lblStatus;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.TextBox txtShopTo;

		protected RegionSelector dropRegion;

		protected System.Web.UI.WebControls.TextBox txtProductName;

		protected System.Web.UI.WebControls.DropDownList OrderFromList;

		protected System.Web.UI.WebControls.TextBox txtShopName;

		protected System.Web.UI.WebControls.TextBox txtUserName;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected System.Web.UI.WebControls.Button btnConfirmSelPayment;

		protected System.Web.UI.WebControls.Button btnDeleteCheck;

		protected System.Web.UI.WebControls.Button btnExport;

		protected PageSize hrefPageSize;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected Pager pager;

		protected CloseTranReasonDropDownList ddlCloseReason;

		protected System.Web.UI.WebControls.Button btnCloseOrder;

		protected FormatedMoneyLabel lblOrderTotalForRemark;

		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;

		protected System.Web.UI.WebControls.TextBox txtRemark;

		protected System.Web.UI.HtmlControls.HtmlInputText txtcategoryId;

		protected System.Web.UI.WebControls.Button btnRemark;

		protected System.Web.UI.WebControls.Label lblOrderId;

		protected System.Web.UI.WebControls.Label lblOrderTotal;

		protected System.Web.UI.WebControls.Label lblRefundType;

		protected System.Web.UI.WebControls.Label lblRefundRemark;

		protected System.Web.UI.WebControls.Label lblContacts;

		protected System.Web.UI.WebControls.Label lblEmail;

		protected System.Web.UI.WebControls.Label lblTelephone;

		protected System.Web.UI.WebControls.Label lblAddress;

		protected System.Web.UI.WebControls.TextBox txtAdminRemark;

		protected System.Web.UI.WebControls.Label return_lblOrderId;

		protected System.Web.UI.WebControls.Label return_lblOrderTotal;

		protected System.Web.UI.WebControls.Label return_lblRefundType;

		protected System.Web.UI.WebControls.Label return_lblReturnRemark;

		protected System.Web.UI.WebControls.Label return_lblContacts;

		protected System.Web.UI.WebControls.Label return_lblEmail;

		protected System.Web.UI.WebControls.Label return_lblTelephone;

		protected System.Web.UI.WebControls.Label return_lblAddress;

		protected System.Web.UI.WebControls.TextBox return_txtRefundMoney;

		protected System.Web.UI.WebControls.TextBox return_txtAdminRemark;

		protected System.Web.UI.WebControls.Label replace_lblOrderId;

		protected System.Web.UI.WebControls.Label replace_lblOrderTotal;

		protected System.Web.UI.WebControls.Label replace_lblComments;

		protected System.Web.UI.WebControls.Label replace_lblContacts;

		protected System.Web.UI.WebControls.Label replace_lblEmail;

		protected System.Web.UI.WebControls.Label replace_lblTelephone;

		protected System.Web.UI.WebControls.Label replace_lblAddress;

		protected System.Web.UI.WebControls.Label replace_lblPostCode;

		protected System.Web.UI.WebControls.TextBox replace_txtAdminRemark;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderTotal;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundType;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundMoney;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidAdminRemark;

		protected System.Web.UI.WebControls.Button btnAcceptRefund;

		protected System.Web.UI.WebControls.Button btnRefuseRefund;

		protected System.Web.UI.WebControls.Button btnAcceptReturn;

		protected System.Web.UI.WebControls.Button btnRefuseReturn;

		protected System.Web.UI.WebControls.Button btnAcceptReplace;

		protected System.Web.UI.WebControls.Button btnRefuseReplace;

		protected System.Web.UI.WebControls.Button btnOrderGoods;

		protected System.Web.UI.WebControls.Button btnProductGoods;

		protected WaitBuyerPay() : base("m03", "ddp14")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string a = Globals.RequestFormStr("isCallback");
			if (a == "true")
			{
				if (string.IsNullOrEmpty(base.Request["ReturnsId"]))
				{
					base.Response.Write("{\"Status\":\"0\"}");
					base.Response.End();
					return;
				}
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(base.Request["orderId"]);
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				int num;
				string text;
				if (base.Request["type"] == "refund")
				{
					RefundHelper.GetRefundType(base.Request["orderId"], out num, out text);
				}
				else
				{
					num = 0;
					text = "";
				}
				string arg;
				if (num == 1)
				{
					arg = "退到预存款";
				}
				else
				{
					arg = "银行转帐";
				}
				stringBuilder.AppendFormat(",\"OrderTotal\":\"{0}\"", Globals.FormatMoney(orderInfo.GetTotal()));
				if (!(base.Request["type"] == "replace"))
				{
					stringBuilder.AppendFormat(",\"RefundType\":\"{0}\"", num);
					stringBuilder.AppendFormat(",\"RefundTypeStr\":\"{0}\"", arg);
				}
				stringBuilder.AppendFormat(",\"Contacts\":\"{0}\"", orderInfo.ShipTo);
				stringBuilder.AppendFormat(",\"Email\":\"{0}\"", orderInfo.EmailAddress);
				stringBuilder.AppendFormat(",\"Telephone\":\"{0}\"", (orderInfo.TelPhone + " " + orderInfo.CellPhone).Trim());
				stringBuilder.AppendFormat(",\"Address\":\"{0}\"", orderInfo.Address);
				stringBuilder.AppendFormat(",\"Remark\":\"{0}\"", text.Replace("\r\n", ""));
				stringBuilder.AppendFormat(",\"PostCode\":\"{0}\"", orderInfo.ZipCode);
				stringBuilder.AppendFormat(",\"GroupBuyId\":\"{0}\"", (orderInfo.GroupBuyId > 0) ? orderInfo.GroupBuyId : 0);
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{\"Status\":\"1\"" + stringBuilder.ToString() + "}");
				base.Response.End();
			}
			else if (a == "GetOrdersStates")
			{
				base.Response.ContentType = "application/json";
				System.Data.DataTable allOrderID = OrderHelper.GetAllOrderID();
				int arg_238_0 = allOrderID.Rows.Count;
				int num2 = allOrderID.Select("OrderStatus=" + 1).Length;
				string s = "{\"type\":\"1\",\"waibuyerpaycount\":" + num2 + "}";
				base.Response.Write(s);
				base.Response.End();
			}
			this.Reurl = base.Request.Url.ToString();
			if (!this.Reurl.Contains("?"))
			{
				this.Reurl += "?pageindex=1";
			}
			this.Reurl = System.Text.RegularExpressions.Regex.Replace(this.Reurl, "&t=(\\d+)", "");
			this.Reurl = System.Text.RegularExpressions.Regex.Replace(this.Reurl, "(\\?)t=(\\d+)", "?");
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
			this.btnCloseOrder.Click += new System.EventHandler(this.btnCloseOrder_Click);
			this.btnDeleteCheck.Click += new System.EventHandler(this.btnDeleteCheck_Click);
			this.btnConfirmSelPayment.Click += new System.EventHandler(this.btnConfirmSelPayment_Click);
			this.btnProductGoods.Click += new System.EventHandler(this.btnProductGoods_Click);
			this.btnOrderGoods.Click += new System.EventHandler(this.btnOrderGoods_Click);
			if (!this.Page.IsPostBack)
			{
				this.SetOrderStatusLink();
				this.bindOrderType();
				this.BindOrders();
			}
		}

		private void bindOrderType()
		{
			int selectedIndex = 0;
			int.TryParse(base.Request.QueryString["orderType"], out selectedIndex);
			this.OrderFromList.SelectedIndex = selectedIndex;
		}

		private void btnProductGoods_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要下载配货表的订单", false);
				return;
			}
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string str = array[i];
				list.Add("'" + str + "'");
			}
			System.Data.DataSet productGoods = OrderHelper.GetProductGoods(string.Join(",", list.ToArray()));
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>商品名称</td>");
			stringBuilder.AppendLine("<td>货号</td>");
			stringBuilder.AppendLine("<td>规格</td>");
			stringBuilder.AppendLine("<td>拣货数量</td>");
			stringBuilder.AppendLine("<td>现库存数</td>");
			stringBuilder.AppendLine("</tr>");
			foreach (System.Data.DataRow dataRow in productGoods.Tables[0].Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td>" + dataRow["ProductName"] + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SKU"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["SKUContent"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ShipmentQuantity"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Stock"] + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("</table>");
			stringBuilder.AppendLine("</body></html>");
			base.Response.Clear();
			base.Response.Buffer = false;
			base.Response.Charset = "GB2312";
			base.Response.AppendHeader("Content-Disposition", "attachment;filename=productgoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
			base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			base.Response.ContentType = "application/ms-excel";
			this.EnableViewState = false;
			base.Response.Write(stringBuilder.ToString());
			base.Response.End();
		}

		private void btnOrderGoods_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要下载配货表的订单", false);
				return;
			}
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string str = array[i];
				list.Add("'" + str + "'");
			}
			System.Data.DataSet orderGoods = OrderHelper.GetOrderGoods(string.Join(",", list.ToArray()));
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>订单单号</td>");
			stringBuilder.AppendLine("<td>商品名称</td>");
			stringBuilder.AppendLine("<td>货号</td>");
			stringBuilder.AppendLine("<td>规格</td>");
			stringBuilder.AppendLine("<td>拣货数量</td>");
			stringBuilder.AppendLine("<td>现库存数</td>");
			stringBuilder.AppendLine("<td>备注</td>");
			stringBuilder.AppendLine("</tr>");
			foreach (System.Data.DataRow dataRow in orderGoods.Tables[0].Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ProductName"] + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SKU"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["SKUContent"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ShipmentQuantity"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Stock"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Remark"] + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("</table>");
			stringBuilder.AppendLine("</body></html>");
			base.Response.Clear();
			base.Response.Buffer = false;
			base.Response.Charset = "GB2312";
			base.Response.AppendHeader("Content-Disposition", "attachment;filename=ordergoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
			base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			base.Response.ContentType = "application/ms-excel";
			this.EnableViewState = false;
			base.Response.Write(stringBuilder.ToString());
			base.Response.End();
		}

		protected void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadOrders(true);
		}

		protected void rptList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rptSubList");
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID").ToString());
				if (orderInfo != null && orderInfo.LineItems.Count > 0)
				{
					repeater.DataSource = orderInfo.LineItems.Values;
					repeater.DataBind();
				}
				OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderStatus");
				string a = "";
				if (!(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway") is System.DBNull))
				{
					a = (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway");
				}
				int num = (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyId") != System.DBNull.Value) ? ((int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyId")) : 0;
				System.Web.UI.HtmlControls.HtmlInputButton htmlInputButton = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnModifyPrice");
				System.Web.UI.HtmlControls.HtmlInputButton arg_13C_0 = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnSendGoods");
				System.Web.UI.WebControls.Button button = (System.Web.UI.WebControls.Button)e.Item.FindControl("btnPayOrder");
				System.Web.UI.WebControls.Button button2 = (System.Web.UI.WebControls.Button)e.Item.FindControl("btnConfirmOrder");
				System.Web.UI.HtmlControls.HtmlInputButton htmlInputButton2 = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnCloseOrderClient");
				System.Web.UI.HtmlControls.HtmlAnchor arg_197_0 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckRefund");
				System.Web.UI.HtmlControls.HtmlAnchor arg_1AD_0 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReturn");
				System.Web.UI.HtmlControls.HtmlAnchor arg_1C3_0 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReplace");
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litOtherInfo");
				System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Item.FindControl("WeiXinNickName");
				int totalPointNumber = orderInfo.GetTotalPointNumber();
				MemberInfo member = MemberProcessor.GetMember(orderInfo.UserId, true);
				if (member != null)
				{
					literal2.Text = "买家：" + member.UserName;
				}
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				decimal total = orderInfo.GetTotal();
				if (total > 0m)
				{
					stringBuilder.Append("<strong>￥" + total.ToString("F2") + "</strong>");
					stringBuilder.Append("<small>(含运费￥" + orderInfo.AdjustedFreight.ToString("F2") + ")</small>");
				}
				if (totalPointNumber > 0)
				{
					stringBuilder.Append("<small>" + totalPointNumber.ToString() + "积分</small>");
				}
				if (orderInfo.PaymentType == "货到付款")
				{
					stringBuilder.Append("<span class=\"setColor bl\"><strong>货到付款</strong></span>");
				}
				if (string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Append("<strong>￥" + total.ToString("F2") + "</strong>");
				}
				literal.Text = stringBuilder.ToString();
				if (orderStatus == OrderStatus.WaitBuyerPay)
				{
					htmlInputButton.Visible = true;
					htmlInputButton.Attributes.Add("onclick", "DialogFrame('../trade/EditOrder.aspx?OrderId=" + System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID") + "&reurl='+ encodeURIComponent(goUrl),'修改订单价格',900,450)");
					htmlInputButton2.Attributes.Add("onclick", "CloseOrderFun('" + System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID") + "')");
					htmlInputButton2.Visible = true;
					if (a != "hishop.plugins.payment.podrequest")
					{
						button.Visible = true;
					}
				}
				if (num > 0)
				{
					GroupBuyStatus arg_3C6_0 = (GroupBuyStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyStatus");
				}
				button2.Visible = (orderStatus == OrderStatus.SellerAlreadySent);
			}
		}

		protected void rptList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			bool flag = false;
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(e.CommandArgument.ToString());
			if (orderInfo != null)
			{
				if (e.CommandName == "CONFIRM_PAY" && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
				{
					if (OrderHelper.ConfirmPay(orderInfo))
					{
						DebitNoteInfo debitNoteInfo = new DebitNoteInfo();
						debitNoteInfo.NoteId = Globals.GetGenerateId();
						debitNoteInfo.OrderId = e.CommandArgument.ToString();
						debitNoteInfo.Operator = ManagerHelper.GetCurrentManager().UserName;
						debitNoteInfo.Remark = "后台" + debitNoteInfo.Operator + "收款成功";
						OrderHelper.SaveDebitNote(debitNoteInfo);
						this.BindOrders();
						orderInfo.OnPayment();
						this.ShowMsg("成功的确认了订单收款", true);
						return;
					}
					this.ShowMsg("确认订单收款失败", false);
					return;
				}
				else if (e.CommandName == "FINISH_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
				{
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
					if (!flag)
					{
						if (OrderHelper.ConfirmOrderFinish(orderInfo))
						{
							this.BindOrders();
							DistributorsBrower.UpdateCalculationCommission(orderInfo);
							foreach (LineItemInfo current2 in orderInfo.LineItems.Values)
							{
								if (current2.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
								{
									RefundHelper.UpdateOrderGoodStatu(orderInfo.OrderId, current2.SkuId, 5, current2.ID);
								}
							}
							this.ShowMsg("成功的完成了该订单", true);
							this.myNotifier.updateAction = UpdateAction.OrderUpdate;
							this.myNotifier.actionDesc = "订单已完成";
							if (orderInfo.PayDate.HasValue)
							{
								this.myNotifier.RecDateUpdate = orderInfo.PayDate.Value;
							}
							else
							{
								this.myNotifier.RecDateUpdate = System.DateTime.Today;
							}
							this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
							this.myNotifier.UpdateDB();
							return;
						}
						this.ShowMsg("完成订单失败", false);
						return;
					}
					else
					{
						this.ShowMsg("订单中商品有退货(款)不允许完成!", false);
					}
				}
			}
		}

		protected void btnConfirmSelPayment_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要批量确认付款的订单", false);
				return;
			}
			string[] array = text.Trim(new char[]
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
				string text2 = array2[i];
				if (!string.IsNullOrEmpty(text2))
				{
					OrderInfo orderInfo = OrderHelper.GetOrderInfo(text2);
					if (orderInfo != null && OrderHelper.ConfirmPay(orderInfo))
					{
						num++;
						DebitNoteInfo debitNoteInfo = new DebitNoteInfo();
						debitNoteInfo.NoteId = Globals.GetGenerateId();
						debitNoteInfo.OrderId = text2;
						debitNoteInfo.Operator = ManagerHelper.GetCurrentManager().UserName;
						debitNoteInfo.Remark = "后台" + debitNoteInfo.Operator + "收款成功";
						OrderHelper.SaveDebitNote(debitNoteInfo);
						orderInfo.OnPayment();
					}
				}
			}
			if (num > 0)
			{
				this.BindOrders();
				this.ShowMsg("成功的确认了" + num.ToString() + "个订单收款", true);
			}
			else
			{
				this.ShowMsg("确认订单收款失败", false);
			}
			this.BindOrders();
		}

		protected void btnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要删除的订单", false);
				return;
			}
			text = "'" + text.Replace(",", "','") + "'";
			int num = OrderHelper.DeleteOrders(text);
			this.BindOrders();
			this.ShowMsg(string.Format("成功删除了{0}个订单", num), true);
		}

		private void btnSendGoods_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要发货的订单", false);
				return;
			}
			this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/Sales/BatchSendOrderGoods.aspx?OrderIds=" + text));
		}

		protected void rptSubList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				OrderCombineStatus orderCombineStatus = (OrderCombineStatus)e.Item.FindControl("lbOrderCombineStatus");
				orderCombineStatus.OrderItemID = Globals.ToNum(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "ID").ToString());
				orderCombineStatus.OrderID = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID").ToString();
				orderCombineStatus.SkuID = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "SkuID").ToString();
				orderCombineStatus.Type = Globals.ToNum(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Type").ToString());
				orderCombineStatus.DetailUrl = "OrderDetails.aspx?OrderId=" + System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID").ToString() + "#returnInfo";
			}
		}

		private void BindOrders()
		{
			OrderQuery orderQuery = this.GetOrderQuery();
			DbQueryResult orders = OrderHelper.GetOrders(orderQuery);
			this.rptList.DataSource = orders.Data;
			this.rptList.DataBind();
			this.pager.TotalRecords = orders.TotalRecords;
			this.txtUserName.Text = orderQuery.UserName;
			this.txtOrderId.Text = orderQuery.OrderId;
			this.txtProductName.Text = orderQuery.ProductName;
			this.txtShopTo.Text = orderQuery.ShipTo;
			this.calendarStartDate.SelectedDate = orderQuery.StartDate;
			this.calendarEndDate.SelectedDate = orderQuery.EndDate;
			this.lblStatus.Text = ((int)orderQuery.Status).ToString();
			if (orderQuery.RegionId.HasValue)
			{
				this.dropRegion.SetSelectedRegionId(orderQuery.RegionId);
			}
			this.txtShopName.Text = orderQuery.StoreName;
		}

		private OrderQuery GetOrderQuery()
		{
			OrderQuery orderQuery = new OrderQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				orderQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
			{
				orderQuery.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipTo"]))
			{
				orderQuery.ShipTo = Globals.UrlDecode(this.Page.Request.QueryString["ShipTo"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
			{
				orderQuery.UserName = Globals.UrlDecode(this.Page.Request.QueryString["UserName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
			{
				orderQuery.StartDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["StartDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				orderQuery.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
			{
				orderQuery.EndDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["EndDate"]).AddMilliseconds(86399.0));
			}
			orderQuery.Status = OrderStatus.WaitBuyerPay;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsPrinted"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["IsPrinted"], out value))
				{
					orderQuery.IsPrinted = new int?(value);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ModeId"]))
			{
				int value2 = 0;
				if (int.TryParse(this.Page.Request.QueryString["ModeId"], out value2))
				{
					orderQuery.ShippingModeId = new int?(value2);
				}
			}
			int value3;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["region"]) && int.TryParse(this.Page.Request.QueryString["region"], out value3))
			{
				orderQuery.RegionId = new int?(value3);
			}
			int value4;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserId"]) && int.TryParse(this.Page.Request.QueryString["UserId"], out value4))
			{
				orderQuery.UserId = new int?(value4);
			}
			int num = 0;
			if (int.TryParse(base.Request.QueryString["orderType"], out num) && num > 0)
			{
				orderQuery.Type = new OrderQuery.OrderType?((OrderQuery.OrderType)num);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
			{
				orderQuery.StoreName = Globals.UrlDecode(this.Page.Request.QueryString["StoreName"]);
			}
			orderQuery.PageIndex = this.pager.PageIndex;
			orderQuery.PageSize = this.pager.PageSize;
			orderQuery.SortBy = "OrderDate";
			orderQuery.SortOrder = SortAction.Desc;
			return orderQuery;
		}

		private void ReloadOrders(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("UserName", this.txtUserName.Text);
			nameValueCollection.Add("OrderId", this.txtOrderId.Text);
			nameValueCollection.Add("ProductName", this.txtProductName.Text);
			nameValueCollection.Add("ShipTo", this.txtShopTo.Text);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			nameValueCollection.Add("OrderType", this.OrderFromList.SelectedValue);
			nameValueCollection.Add("OrderStatus", this.lblStatus.Text);
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				nameValueCollection.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
			}
			if (this.dropRegion.GetSelectedRegionId().HasValue)
			{
				nameValueCollection.Add("region", this.dropRegion.GetSelectedRegionId().Value.ToString());
			}
			nameValueCollection.Add("StoreName", this.txtShopName.Text.Trim());
			base.ReloadPage(nameValueCollection);
		}

		private void SetOrderStatusLink()
		{
			this.hlinkAllOrder.NavigateUrl = "WaitBuyerPay.aspx";
		}

		private void btnRemark_Click(object sender, System.EventArgs e)
		{
			if (this.hidOrderId.Value == "0")
			{
				string text = Globals.RequestFormStr("CheckBoxGroup");
				if (text.Length <= 0)
				{
					this.ShowMsg("请先选择要批量备注的订单", false);
					return;
				}
				if (this.txtRemark.Text.Length > 300)
				{
					this.ShowMsg("备注长度限制在300个字符以内", false);
					return;
				}
				string[] array = text.Split(new char[]
				{
					','
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					if (!string.IsNullOrEmpty(text2))
					{
						OrderInfo orderInfo = OrderHelper.GetOrderInfo(text2);
						orderInfo.OrderId = text2;
						if (this.orderRemarkImageForRemark.SelectedItem != null)
						{
							orderInfo.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
						}
						orderInfo.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
						OrderHelper.SaveRemark(orderInfo);
					}
				}
				this.ShowMsg("批量保存备注成功", true);
				this.BindOrders();
				return;
			}
			else
			{
				if (this.txtRemark.Text.Length > 300)
				{
					this.ShowMsg("备注长度限制在300个字符以内", false);
					return;
				}
				OrderInfo orderInfo2 = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
				orderInfo2.OrderId = this.hidOrderId.Value;
				if (this.orderRemarkImageForRemark.SelectedItem != null)
				{
					orderInfo2.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
				}
				orderInfo2.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
				if (OrderHelper.SaveRemark(orderInfo2))
				{
					this.BindOrders();
					this.ShowMsg("保存备注成功", true);
					return;
				}
				this.ShowMsg("保存失败", false);
				return;
			}
		}

		private void btnCloseOrder_Click(object sender, System.EventArgs e)
		{
			string value = this.hidOrderId.Value;
			if (value == "sel")
			{
				string selectedValue = this.ddlCloseReason.SelectedValue;
				if ("请选择关闭的理由" == selectedValue)
				{
					this.ShowMsg("请选择关闭的理由", false);
					return;
				}
				string text = Globals.RequestFormStr("CheckBoxGroup");
				if (text.Length <= 0)
				{
					this.ShowMsg("请先选择要批量关闭的订单", false);
					return;
				}
				string[] array = text.Split(new char[]
				{
					','
				});
				int num = 0;
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					if (!string.IsNullOrEmpty(text2))
					{
						OrderInfo orderInfo = OrderHelper.GetOrderInfo(text2);
						orderInfo.CloseReason = selectedValue;
						if (OrderHelper.CloseTransaction(orderInfo))
						{
							orderInfo.OnClosed();
							num++;
						}
					}
				}
				if (num > 0)
				{
					this.ShowMsg("成功关闭" + num.ToString() + "个订单", true);
				}
				else
				{
					this.ShowMsg("关闭订单失败", false);
				}
				this.BindOrders();
				return;
			}
			else
			{
				OrderInfo orderInfo2 = OrderHelper.GetOrderInfo(value);
				orderInfo2.CloseReason = this.ddlCloseReason.SelectedValue;
				if ("请选择关闭的理由" == orderInfo2.CloseReason)
				{
					this.ShowMsg("请选择关闭的理由", false);
					return;
				}
				if (OrderHelper.CloseTransaction(orderInfo2))
				{
					orderInfo2.OnClosed();
					this.BindOrders();
					this.ShowMsg("关闭订单成功", true);
					return;
				}
				this.ShowMsg("关闭订单失败", false);
				return;
			}
		}

		protected string GetSpitLink(object oSplitState, object oOrderStatus, object oOrderID)
		{
			string result = string.Empty;
			int num = Globals.ToNum(oSplitState);
			if (num < 1)
			{
				string text = oOrderID.ToString();
				OrderStatus orderStatus = (OrderStatus)oOrderStatus;
				if ((orderStatus == OrderStatus.BuyerAlreadyPaid || orderStatus == OrderStatus.WaitBuyerPay) && OrderHelper.GetItemNumByOrderID(text) > 1)
				{
					result = string.Concat(new string[]
					{
						"<a href='OrderSplit.aspx?OrderId=",
						text,
						"&reurl=",
						base.Server.UrlEncode(base.Request.Url.ToString()),
						"' target='_blank' class='btn btn-default resetSize inputw100 bl mb5'>订单拆分</a>"
					});
				}
			}
			return result;
		}

		protected void btnExport_Click(object sender, System.EventArgs e)
		{
			string text = Globals.RequestFormStr("CheckBoxGroup");
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请选择需要导出的记录", false);
				return;
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("订单编号,买家会员名,买家支付宝账号,买家应付货款,买家应付邮费,买家支付积分,总金额,返点积分,买家实际支付金额,买家实际支付积分,订单状态,买家留言,收货人姓名,收货地址,运送方式,联系电话,联系手机,订单创建时间,订单付款时间,宝贝标题,宝贝种类,物流单号,物流公司,订单备注,宝贝总数量,分销商Id,分销商店铺名称,订单关闭原因,卖家服务费,买家服务费,发票抬头,定金排名,修改后的sku,修改后的收货地址,异常信息,卡券抵扣,积分抵扣,是否是O2O交易");
			stringBuilder.Append("\r\n");
			string[] array = text.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string orderId = array2[i];
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
				if (orderInfo != null)
				{
					stringBuilder2.Append(this.FormatOrderStr(orderInfo.OrderId));
					stringBuilder2.Append("," + this.FormatOrderStr(orderInfo.Username));
					stringBuilder2.Append(",");
					decimal total = orderInfo.GetTotal();
					decimal adjustedFreight = orderInfo.AdjustedFreight;
					stringBuilder2.Append("," + this.FormatOrderStr((total - adjustedFreight).ToString("F2")));
					stringBuilder2.Append("," + this.FormatOrderStr(orderInfo.AdjustedFreight.ToString("F2")));
					stringBuilder2.Append(",{买家支付积分}");
					stringBuilder2.Append("," + this.FormatOrderStr(total.ToString("F2")));
					stringBuilder2.Append("," + this.FormatOrderStr(""));
					stringBuilder2.Append("," + this.FormatOrderStr((total - adjustedFreight).ToString("F2")));
					stringBuilder2.Append("," + this.FormatOrderStr(orderInfo.PointExchange.ToString()));
					string str = string.Empty;
					switch (orderInfo.OrderStatus)
					{
					case OrderStatus.BuyerAlreadyPaid:
						str = "已付款";
						break;
					case OrderStatus.SellerAlreadySent:
						str = "已发货";
						break;
					}
					stringBuilder2.Append("," + this.FormatOrderStr(str));
					stringBuilder2.Append("," + this.FormatOrderStr(orderInfo.Remark.ToString()));
					string text2 = string.Empty;
					string oldAddress = orderInfo.OldAddress;
					if (!string.IsNullOrEmpty(orderInfo.ShippingRegion))
					{
						text2 = orderInfo.ShippingRegion.Replace('，', ' ');
					}
					if (!string.IsNullOrEmpty(orderInfo.Address))
					{
						text2 += orderInfo.Address;
					}
					stringBuilder2.Append("," + this.FormatOrderStr(orderInfo.ShipTo.ToString()));
					if (!string.IsNullOrEmpty(orderInfo.ZipCode))
					{
						text2 = text2 + " " + orderInfo.ZipCode;
					}
					if (!string.IsNullOrEmpty(orderInfo.TelPhone))
					{
						text2 = text2 + " " + orderInfo.TelPhone;
					}
					if (!string.IsNullOrEmpty(orderInfo.CellPhone))
					{
						text2 = text2 + " " + orderInfo.CellPhone;
					}
					string str2 = string.Empty;
					if (string.IsNullOrEmpty(oldAddress))
					{
						stringBuilder2.Append("," + this.FormatOrderStr(text2));
					}
					else
					{
						str2 = text2;
						stringBuilder2.Append("," + this.FormatOrderStr(oldAddress));
					}
					string text3 = string.Empty;
					if (orderInfo.OrderStatus == OrderStatus.Finished || orderInfo.OrderStatus == OrderStatus.SellerAlreadySent)
					{
						text3 = orderInfo.RealModeName;
						if (string.IsNullOrEmpty(text3))
						{
							text3 = orderInfo.ModeName;
						}
					}
					else
					{
						text3 = orderInfo.ModeName;
					}
					stringBuilder2.Append("," + this.FormatOrderStr(text3));
					stringBuilder2.Append("," + this.FormatOrderStr(orderInfo.TelPhone));
					stringBuilder2.Append("," + this.FormatOrderStr(orderInfo.CellPhone));
					stringBuilder2.Append("," + this.FormatOrderStr(orderInfo.OrderDate.ToString()));
					string str3 = string.Empty;
					if (orderInfo.PayDate.HasValue)
					{
						str3 = orderInfo.PayDate.ToString();
					}
					stringBuilder2.Append("," + this.FormatOrderStr(str3));
					stringBuilder2.Append("," + this.FormatOrderStr("{宝贝标题}"));
					stringBuilder2.Append("," + this.FormatOrderStr(""));
					stringBuilder2.Append("," + this.FormatOrderStr(orderInfo.ShipOrderNumber));
					stringBuilder2.Append("," + this.FormatOrderStr(orderInfo.ExpressCompanyName));
					stringBuilder2.Append("," + this.FormatOrderStr((orderInfo.ManagerRemark == null) ? "" : orderInfo.ManagerRemark.ToString()));
					stringBuilder2.Append("," + this.FormatOrderStr("{宝贝总数量}"));
					stringBuilder2.Append("," + this.FormatOrderStr((orderInfo.ReferralUserId > 0) ? orderInfo.ReferralUserId.ToString() : ""));
					if (orderInfo.ReferralUserId > 0)
					{
						DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(orderInfo.ReferralUserId);
						if (distributorInfo != null)
						{
							stringBuilder2.Append("," + this.FormatOrderStr(""));
						}
						else
						{
							stringBuilder2.Append("," + this.FormatOrderStr(distributorInfo.StoreName));
						}
					}
					else
					{
						stringBuilder2.Append("," + this.FormatOrderStr(""));
					}
					stringBuilder2.Append(",");
					stringBuilder2.Append(",0");
					stringBuilder2.Append(",0元");
					stringBuilder2.Append(",");
					stringBuilder2.Append(",");
					stringBuilder2.Append(",");
					stringBuilder2.Append(",");
					stringBuilder2.Append("," + str2);
					stringBuilder2.Append(",");
					stringBuilder2.Append("," + ((orderInfo.CouponValue == 0m) ? "" : orderInfo.CouponValue.ToString()));
					stringBuilder2.Append("," + ((orderInfo.PointToCash == 0m) ? "" : orderInfo.PointToCash.ToString()));
					stringBuilder2.Append(",");
					string text4 = stringBuilder2.ToString();
					foreach (LineItemInfo current in orderInfo.LineItems.Values)
					{
						stringBuilder.Append(text4.Replace("{宝贝标题}", current.ItemDescription).Replace("{宝贝总数量}", current.Quantity.ToString())).Replace("{买家支付积分}", current.PointNumber.ToString());
						stringBuilder.Append("\r\n");
					}
				}
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DataExport.csv");
			this.Page.Response.ContentType = "application/octet-stream";
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.EnableViewState = false;
			this.Page.Response.Write(stringBuilder.ToString());
			this.Page.Response.End();
		}

		private string FormatOrderStr(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return "";
			}
			return str.Replace(",", "，").Replace("\n", "");
		}
	}
}
