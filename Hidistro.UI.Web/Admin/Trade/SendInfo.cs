using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Trade
{
	public class SendInfo : AdminPage
	{
		protected string Reurl = string.Empty;

		protected System.Web.UI.WebControls.Label lblStatus;

		protected System.Web.UI.WebControls.HyperLink hlinkAllOrder;

		protected System.Web.UI.WebControls.HyperLink hlinkNotPay;

		protected System.Web.UI.WebControls.HyperLink hlinkYetPay;

		protected System.Web.UI.WebControls.Button btnDeleteCheck;

		protected PageSize hrefPageSize;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected Pager pager;

		protected FormatedMoneyLabel lblOrderTotalForRemark;

		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;

		protected System.Web.UI.WebControls.TextBox txtRemark;

		protected System.Web.UI.HtmlControls.HtmlInputText txtcategoryId;

		protected System.Web.UI.WebControls.Button btnRemark;

		protected SendInfo() : base("m03", "ddp08")
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
				int count = allOrderID.Rows.Count;
				int num2 = allOrderID.Select("OrderStatus=" + 1).Length;
				int num3 = allOrderID.Select("OrderStatus=" + 2).Length;
				int num4 = allOrderID.Select("OrderStatus=" + 3).Length;
				int num5 = allOrderID.Select("OrderStatus=" + 5).Length;
				int num6 = allOrderID.Select("OrderStatus=" + 7).Length;
				int num7 = allOrderID.Select("OrderStatus=" + 4).Length;
				string s = string.Concat(new object[]
				{
					"{\"type\":\"1\",\"allcount\":",
					count,
					",\"waibuyerpaycount\":",
					num2,
					",\"buyalreadypaidcount\":",
					num3,
					",\"sellalreadysentcount\":",
					num4,
					",\"finishedcount\":",
					num5,
					",\"applyforreturnscount\":",
					num6,
					",\"closedcount\":",
					num7,
					"}"
				});
				base.Response.Write(s);
				base.Response.End();
			}
			this.Reurl = base.Request.Url.ToString();
			if (!this.Reurl.Contains("?"))
			{
				this.Reurl += "?pageindex=1";
			}
			this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
			this.btnDeleteCheck.Click += new System.EventHandler(this.btnDeleteCheck_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindOrders();
			}
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
				if (!(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway") is System.DBNull))
				{
					string arg_CE_0 = (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway");
				}
				int num = (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyId") != System.DBNull.Value) ? ((int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyId")) : 0;
				System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("lkbtnEditPrice");
				if (orderStatus == OrderStatus.WaitBuyerPay)
				{
					hyperLink.Visible = true;
				}
				if (num > 0)
				{
					GroupBuyStatus arg_14A_0 = (GroupBuyStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyStatus");
				}
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
					int num = 0;
					int num2 = 0;
					int arg_49_0 = orderInfo.GroupBuyId;
					if (OrderHelper.ConfirmPay(orderInfo))
					{
						DebitNoteInfo debitNoteInfo = new DebitNoteInfo();
						debitNoteInfo.NoteId = Globals.GetGenerateId();
						debitNoteInfo.OrderId = e.CommandArgument.ToString();
						debitNoteInfo.Operator = ManagerHelper.GetCurrentManager().UserName;
						debitNoteInfo.Remark = "后台" + debitNoteInfo.Operator + "收款成功";
						OrderHelper.SaveDebitNote(debitNoteInfo);
						if (orderInfo.GroupBuyId > 0)
						{
							int arg_BE_0 = num + num2;
						}
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

		private void BindOrders()
		{
			OrderQuery orderQuery = this.GetOrderQuery();
			DbQueryResult orders = OrderHelper.GetOrders(orderQuery);
			this.rptList.DataSource = orders.Data;
			this.rptList.DataBind();
			this.pager.TotalRecords = orders.TotalRecords;
			this.lblStatus.Text = ((int)orderQuery.Status).ToString();
		}

		private OrderQuery GetOrderQuery()
		{
			OrderQuery orderQuery = new OrderQuery();
			orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
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
	}
}
