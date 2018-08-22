using ASPNET.WebControls;
using Hidistro.ControlPanel.OutPay;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Oneyuan
{
	public class RefoundList : AdminPage
	{
		private static object payLock = new object();

		private string atitle;

		private string PayWay = "";

		private string UserName;

		private string phone;

		private int state;

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtEditInfo;

		protected System.Web.UI.WebControls.Literal ListWait;

		protected System.Web.UI.WebControls.Literal ListEnd;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txtTitle;

		protected System.Web.UI.WebControls.TextBox txtUserName;

		protected System.Web.UI.WebControls.TextBox txtPhone;

		protected System.Web.UI.WebControls.DropDownList txtPayWay;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected System.Web.UI.HtmlControls.HtmlTableCell actionTd;

		protected System.Web.UI.WebControls.Repeater Datalist;

		protected Pager pager;

		protected RefoundList() : base("m08", "yxp22")
		{
		}

		private void AjaxAction(string action)
		{
			string s = "{\"state\":false,\"msg\":\"未定义操作\"}";
			System.Collections.Specialized.NameValueCollection form = base.Request.Form;
			if (action != null)
			{
				if (!(action == "WeiXinRefund"))
				{
					if (!(action == "WeiXinBacthRefund"))
					{
						if (action == "BatchRefund")
						{
							string text = base.Request.Form["vaid"];
							if (string.IsNullOrEmpty(text))
							{
								s = "{\"state\":false,\"msg\":\"参数错误！\"}";
							}
							else
							{
								System.Collections.Generic.List<string> participantPids = OneyuanTaoHelp.GetParticipantPids(text, true, false, "weixin");
								if (participantPids.Count > 0)
								{
									string WxPids = string.Join(",", participantPids);
									new System.Threading.Thread(() =>
                                    {
										Globals.Debuglog(this.BatchWxRefund(WxPids), "_wxrefund.txt");
									}).Start();
								}
								System.Collections.Generic.List<string> participantPids2 = OneyuanTaoHelp.GetParticipantPids(text, true, false, "alipay");
								if (participantPids2.Count > 0)
								{
									s = "{\"state\":true,\"msg\":\"转向支付宝退款中...！\",\"alipay\":true}";
								}
								else if (participantPids.Count > 0)
								{
									s = "{\"state\":true,\"msg\":\"正在后台处理微信退款，请稍后刷新！\",\"alipay\":false}";
								}
								else
								{
									s = "{\"state\":false,\"msg\":\"没有退款数据可以操作！\",\"alipay\":false}";
									OneyuanTaoHelp.SetIsAllRefund(new System.Collections.Generic.List<string>
									{
										text
									});
								}
							}
						}
					}
					else
					{
						string pids = base.Request.Form["Pids"];
						s = this.BatchWxRefund(pids);
					}
				}
				else
				{
					string text2 = form["Pid"];
					if (!string.IsNullOrEmpty(text2))
					{
						OneyuanTaoParticipantInfo addParticipant = OneyuanTaoHelp.GetAddParticipant(0, text2, "");
						if (addParticipant != null && addParticipant.IsPay)
						{
							if (!addParticipant.IsRefund)
							{
								string text3 = addParticipant.out_refund_no;
								if (string.IsNullOrEmpty(text3))
								{
									text3 = RefundHelper.GenerateRefundOrderId();
									OneyuanTaoHelp.Setout_refund_no(text2, text3);
								}
								string refundNum = "";
								string text4 = RefundHelper.SendWxRefundRequest(text2, addParticipant.TotalPrice, addParticipant.TotalPrice, text3, out refundNum);
								if (text4 == "")
								{
									s = "{\"state\":true,\"msg\":\"退款成功\"}";
									addParticipant.Remark = "退款成功";
									addParticipant.RefundNum = refundNum;
									OneyuanTaoHelp.SetRefundinfo(addParticipant);
									OneyuanTaoHelp.SetIsAllRefund(new System.Collections.Generic.List<string>
									{
										addParticipant.ActivityId
									});
								}
								else
								{
									addParticipant.Remark = "退款失败:" + text4.Replace("OK", "");
									OneyuanTaoHelp.SetRefundinfoErr(addParticipant);
									s = "{\"state\":false,\"msg\":\"" + addParticipant.Remark + "\"}";
								}
							}
							else
							{
								s = "{\"state\":false,\"msg\":\"该订单已退款！\"}";
							}
						}
						else
						{
							s = "{\"state\":false,\"msg\":\"用户记录不存在或者用户未支付！\"}";
						}
					}
					else
					{
						s = "{\"state\":false,\"msg\":\"参数错误！\"}";
					}
				}
			}
			base.Response.ClearContent();
			base.Response.ContentType = "application/json";
			base.Response.Write(s);
			base.Response.End();
		}

		private string BatchWxRefund(string Pids)
		{
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			string result;
			if (!string.IsNullOrEmpty(Pids))
			{
				string text = "";
				string[] pIds = Pids.Replace(" ", "").Replace("\u3000", "").Split(new char[]
				{
					','
				});
				System.Collections.Generic.IList<OneyuanTaoParticipantInfo> refundParticipantList = OneyuanTaoHelp.GetRefundParticipantList(pIds);
				int num = 0;
				foreach (OneyuanTaoParticipantInfo current in refundParticipantList)
				{
					try
					{
						if (current != null && current.IsPay && current.PayWay == "weixin")
						{
							list.Add(current.ActivityId);
							if (!current.IsRefund)
							{
								string text2 = current.out_refund_no;
								if (string.IsNullOrEmpty(text2))
								{
									text2 = RefundHelper.GenerateRefundOrderId();
									OneyuanTaoHelp.Setout_refund_no(current.Pid, text2);
								}
								string refundNum = "";
								string text3 = RefundHelper.SendWxRefundRequest(current.Pid, current.TotalPrice, current.TotalPrice, text2, out refundNum);
								if (text3 == "")
								{
									current.Remark = "退款成功";
									current.RefundNum = refundNum;
									OneyuanTaoHelp.SetRefundinfo(current);
									num++;
								}
								else
								{
									current.Remark = "退款失败:" + text3.Replace("OK", "");
									OneyuanTaoHelp.SetRefundinfoErr(current);
									if (text3.Contains("金额不足") || text3.Contains("mch_id") || text3.Contains("mch_id") || text3.ToLower().Contains("appid") || text3.Contains("密码") || text3.Contains("证书") || text3.Contains("签名") || text3.ToLower().Contains("mchid"))
									{
										text = text3;
										break;
									}
								}
							}
							else
							{
								num++;
							}
						}
					}
					catch (System.Exception ex)
					{
						text = ex.Message;
						Globals.Debuglog("微信退款异常信息：" + ex.Message, "_wxrefund.txt");
					}
				}
				if (num == 0)
				{
					if (text == "")
					{
						result = "{\"state\":false,\"msg\":\"微信批量退款失败！\"}";
					}
					else
					{
						text = text.Replace(",", "，").Replace("\"", " ").Replace("'", " ").Replace(":", " ");
						result = "{\"state\":false,\"msg\":\"退款中断->" + text + "\"}";
					}
				}
				else
				{
					result = string.Concat(new string[]
					{
						"{\"state\":true,\"msg\":\"成功退款",
						num.ToString(),
						"笔，失败",
						(refundParticipantList.Count - num).ToString(),
						"笔\"}"
					});
				}
			}
			else
			{
				result = "{\"state\":false,\"msg\":\"参数错误！\"}";
			}
			list = list.Distinct<string>().ToList<string>();
			OneyuanTaoHelp.SetIsAllRefund(list);
			return result;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["action"];
			if (!string.IsNullOrEmpty(text))
			{
				this.AjaxAction(text);
				base.Response.End();
			}
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
		}

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["atitle"]))
				{
					this.atitle = base.Server.UrlDecode(this.Page.Request.QueryString["atitle"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PayWay"]))
				{
					this.PayWay = base.Server.UrlDecode(this.Page.Request.QueryString["PayWay"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
				{
					this.UserName = base.Server.UrlDecode(this.Page.Request.QueryString["UserName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["phone"]))
				{
					this.phone = base.Server.UrlDecode(this.Page.Request.QueryString["phone"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
				{
					int.TryParse(this.Page.Request.QueryString["state"], out this.state);
				}
				this.txtTitle.Text = this.atitle;
				this.txtPayWay.SelectedValue = this.PayWay;
				this.txtUserName.Text = this.UserName;
				this.txtPhone.Text = this.phone;
				return;
			}
			this.atitle = this.txtTitle.Text;
			int.TryParse(this.Page.Request.QueryString["state"], out this.state);
			this.PayWay = this.txtPayWay.SelectedValue;
			this.UserName = this.txtUserName.Text;
			this.phone = this.txtPhone.Text;
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("atitle", this.atitle);
			nameValueCollection.Add("UserName", this.UserName);
			nameValueCollection.Add("PayWay", this.PayWay);
			nameValueCollection.Add("phone", this.phone);
			nameValueCollection.Add("state", this.state.ToString());
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}

		private void BindData()
		{
			OneyuanTaoPartInQuery oneyuanTaoPartInQuery = new OneyuanTaoPartInQuery();
			oneyuanTaoPartInQuery.PageIndex = this.pager.PageIndex;
			oneyuanTaoPartInQuery.PageSize = this.pager.PageSize;
			oneyuanTaoPartInQuery.IsPay = 1;
			oneyuanTaoPartInQuery.SortBy = "Pid";
			oneyuanTaoPartInQuery.UserName = this.UserName;
			oneyuanTaoPartInQuery.PayWay = this.PayWay;
			oneyuanTaoPartInQuery.CellPhone = this.phone;
			oneyuanTaoPartInQuery.Atitle = this.atitle;
			if (this.state == 0)
			{
				oneyuanTaoPartInQuery.state = 5;
			}
			else
			{
				oneyuanTaoPartInQuery.state = 4;
			}
			DbQueryResult oneyuanPartInDataTable = OneyuanTaoHelp.GetOneyuanPartInDataTable(oneyuanTaoPartInQuery);
			if (oneyuanPartInDataTable.Data != null)
			{
				System.Data.DataTable dataTable = (System.Data.DataTable)oneyuanPartInDataTable.Data;
				dataTable.Columns.Add("ActionBtn");
				dataTable.Columns.Add("ASate");
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					if ((bool)dataRow["IsRefund"])
					{
						dataRow["ASate"] = "<span class='green'>已退款</span>";
					}
					else if ((bool)dataRow["RefundErr"])
					{
						dataRow["ASate"] = "<span class='red'>退款错误</span>";
					}
					else
					{
						dataRow["ASate"] = "<span class='red'>待退款</span>";
					}
					string text = string.Concat(new object[]
					{
						"<a class=\"btn btn-xs btn-danger\"  onclick=\"DoRefund('",
						dataRow["Pid"],
						"','",
						dataRow["PayWay"],
						"')\" >退款</a> "
					});
					if ((bool)dataRow["RefundErr"])
					{
						text = "<a class=\"btn btn-xs btn-primary\"  Remark='" + Globals.HtmlEncode(dataRow["Remark"].ToString()) + "' onclick=\"AView(this)\"  >原因</a> " + text;
					}
					if ((bool)dataRow["IsRefund"])
					{
						text = "";
						if (dataRow["RefundTime"] != System.DBNull.Value)
						{
							text = "<span>" + ((System.DateTime)dataRow["RefundTime"]).ToString("yyyy-MM-dd") + "</span> ";
						}
						this.actionTd.InnerText = "退款时间";
					}
					dataRow["ActionBtn"] = text;
				}
				this.Datalist.DataSource = dataTable;
				this.Datalist.DataBind();
				this.pager.TotalRecords = oneyuanPartInDataTable.TotalRecords;
				int num = 0;
				int refundTotalNum = OneyuanTaoHelp.GetRefundTotalNum(out num);
				this.ListWait.Text = "待退款(" + refundTotalNum.ToString() + ")";
				this.ListEnd.Text = "已退款(" + num.ToString() + ")";
			}
		}
	}
}
