using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Oneyuan
{
	public class OneyuanList : AdminPage
	{
		private string atitle = "";

		private int ReachType;

		private int state;

		protected System.Web.UI.WebControls.Literal ListTotal;

		protected System.Web.UI.WebControls.Literal ListStart;

		protected System.Web.UI.WebControls.Literal ListWait;

		protected System.Web.UI.WebControls.Literal Listend;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txtTitle;

		protected System.Web.UI.WebControls.DropDownList txtReachType;

		protected System.Web.UI.WebControls.DropDownList txtState;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected System.Web.UI.WebControls.Repeater Datalist;

		protected Pager pager;

		protected OneyuanList() : base("m08", "yxp20")
		{
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

		private OneTaoState getOneTaoState(OneyuanTaoInfo selitem)
		{
			return OneyuanTaoHelp.getOneTaoState(selitem);
		}

		private void AjaxAction(string action)
		{
			string s = "{\"state\":false,\"msg\":\"未定义操作\"}";
			System.Collections.Specialized.NameValueCollection form = base.Request.Form;
			switch (action)
			{
			case "Del":
			{
				s = "{\"state\":false,\"msg\":\"活动信息未找到失败\"}";
				string text = form["Aid"];
				if (!string.IsNullOrEmpty(text))
				{
					OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(text);
					if (oneyuanTaoInfoById != null)
					{
						if (OneyuanTaoHelp.DeleteOneyuanTao(text))
						{
							s = "{\"state\":true,\"msg\":\"删除成功\"}";
							OneyuanTaoHelp.DelParticipantMember(text, true);
						}
						else
						{
							s = "{\"state\":false,\"msg\":\"该活动已有人参与，不能删除！\"}";
						}
					}
				}
				break;
			}
			case "BatchDel":
			{
				s = "{\"state\":false,\"msg\":\"批量删除失败\"}";
				string text = form["Aids"];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(new char[]
					{
						','
					});
					int num2 = OneyuanTaoHelp.BatchDeleteOneyuanTao(array);
					if (num2 > 0)
					{
						s = string.Concat(new string[]
						{
							"{\"state\":true,\"msg\":\"成功删除",
							num2.ToString(),
							"条数据，失败",
							(array.Length - num2).ToString(),
							"条！\"}"
						});
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string activityId = array2[i];
							OneyuanTaoHelp.DelParticipantMember(activityId, true);
						}
					}
					else
					{
						s = "{\"state\":false,\"msg\":\"没有找到可删除的数据！\"}";
					}
				}
				break;
			}
			case "EndII":
			{
				string text = form["Aid"];
				string text2 = form["CanDraw"];
				if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
				{
					s = "{\"state\":false,\"msg\":\"参数错误\"}";
				}
				else if (text2.Trim() == "1")
				{
					string text3 = OneyuanTaoHelp.CalculateWinner(text);
					if (text3 == "success")
					{
						s = "{\"state\":true,\"msg\":\"手动开奖成功！\"}";
					}
					else
					{
						s = "{\"state\":false,\"msg\":\"" + text3 + "\"}";
					}
				}
				else if (OneyuanTaoHelp.SetOneyuanTaoIsOn(text, false))
				{
					s = "{\"state\":true,\"msg\":\"提前终止活动成功！！\"}";
					OneyuanTaoHelp.DelParticipantMember(text, true);
				}
				else
				{
					s = "{\"state\":false,\"msg\":\"提前终止活动失败！\"}";
				}
				break;
			}
			case "End":
			{
				s = "{\"state\":false,\"msg\":\"结束失败\"}";
				string text = form["Aid"];
				if (!string.IsNullOrEmpty(text))
				{
					OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(text);
					if (oneyuanTaoInfoById != null)
					{
						OneTaoState oneTaoState = this.getOneTaoState(oneyuanTaoInfoById);
						if (oneTaoState == OneTaoState.进行中)
						{
							if (OneyuanTaoHelp.SetOneyuanTaoIsOn(text, false))
							{
								s = "{\"state\":true,\"msg\":\"提前终止活动成功！！\"}";
								OneyuanTaoHelp.DelParticipantMember(text, true);
							}
							else
							{
								s = "{\"state\":false,\"msg\":\"提前终止活动失败！\"}";
							}
						}
						else
						{
							s = "{\"state\":false,\"msg\":\"提前终止活动失败！\"}";
						}
					}
				}
				break;
			}
			case "Start":
			{
				s = "{\"state\":false,\"msg\":\"操作开始失败\"}";
				string text = form["Aid"];
				if (!string.IsNullOrEmpty(text))
				{
					OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(text);
					if (oneyuanTaoInfoById != null)
					{
						if (this.getOneTaoState(oneyuanTaoInfoById) == OneTaoState.未开始)
						{
							if (OneyuanTaoHelp.SetOneyuanTaoIsOn(text, true))
							{
								s = "{\"state\":true,\"msg\":\"提前开启活动成功！！\"}";
							}
							else
							{
								s = "{\"state\":false,\"msg\":\"当前状态不能结束！\"}";
							}
						}
						else
						{
							s = "{\"state\":false,\"msg\":\"当前状态开启活动！\"}";
						}
					}
				}
				break;
			}
			case "BatchStart":
			{
				s = "{\"state\":false,\"msg\":\"批量操作开始失败\"}";
				string text = form["Aids"];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(new char[]
					{
						','
					});
					int num3 = OneyuanTaoHelp.BatchSetOneyuanTaoIsOn(array, true);
					if (num3 > 0)
					{
						s = string.Concat(new string[]
						{
							"{\"state\":true,\"msg\":\"成功开启",
							num3.ToString(),
							"条活动，失败",
							(array.Length - num3).ToString(),
							"条！\"}"
						});
					}
					else
					{
						s = "{\"state\":false,\"msg\":\"没有找到可开启的活动数据！\"}";
					}
				}
				break;
			}
			}
			base.Response.ClearContent();
			base.Response.ContentType = "application/json";
			base.Response.Write(s);
			base.Response.End();
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
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ReachType"]))
				{
					int.TryParse(this.Page.Request.QueryString["ReachType"], out this.ReachType);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
				{
					int.TryParse(this.Page.Request.QueryString["state"], out this.state);
				}
				this.txtTitle.Text = this.atitle;
				this.txtReachType.SelectedValue = this.ReachType.ToString();
				this.txtState.SelectedValue = this.state.ToString();
				return;
			}
			this.atitle = this.txtTitle.Text;
			int.TryParse(this.txtReachType.SelectedItem.Value, out this.ReachType);
			int.TryParse(this.txtState.SelectedItem.Value, out this.state);
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("atitle", this.atitle);
			nameValueCollection.Add("ReachType", this.ReachType.ToString());
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
			DbQueryResult oneyuanTao = OneyuanTaoHelp.GetOneyuanTao(new OneyuanTaoQuery
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				title = this.atitle,
				state = this.state,
				ReachType = this.ReachType
			});
			if (oneyuanTao.Data != null)
			{
				System.Data.DataTable dataTable = (System.Data.DataTable)oneyuanTao.Data;
				dataTable.Columns.Add("ActionBtn");
				dataTable.Columns.Add("ASate");
				dataTable.Columns.Add("PrizeState");
				dataTable.Columns.Add("CanDel");
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					OneyuanTaoInfo info = OneyuanTaoHelp.DataRowToOneyuanTaoInfo(dataRow);
					OneTaoState oneTaoState = OneyuanTaoHelp.getOneTaoState(info);
					OneTaoPrizeState prizeState = OneyuanTaoHelp.getPrizeState(info);
					dataRow["ASate"] = oneTaoState;
					dataRow["PrizeState"] = prizeState;
					dataRow["CanDel"] = 0;
					if (prizeState == OneTaoPrizeState.成功开奖)
					{
						dataRow["PrizeState"] = "<span class='success'>" + prizeState + "<span>";
					}
					else if (prizeState == OneTaoPrizeState.已关闭)
					{
						dataRow["PrizeState"] = "<span class='normal'>" + prizeState + "<span>";
					}
					else if (prizeState == OneTaoPrizeState.待退款)
					{
						dataRow["PrizeState"] = "<span class='green'>" + prizeState + "<span>";
					}
					else
					{
						dataRow["PrizeState"] = "<span class='errcss'>" + prizeState + "<span>";
					}
					string text = "<a class=\"btn btn-xs btn-primary\" onclick=\"AView('" + dataRow["ActivityId"] + "')\" >查看</a> ";
					if (oneTaoState == OneTaoState.进行中 || oneTaoState == OneTaoState.未开始)
					{
						object obj = text;
						text = string.Concat(new object[]
						{
							obj,
							"<a class=\"btn btn-xs btn-primary\" onclick=\"AEdit('",
							dataRow["ActivityId"],
							"')\"  >编辑</a> "
						});
					}
					if (oneTaoState == OneTaoState.未开始)
					{
						object obj2 = text;
						text = string.Concat(new object[]
						{
							obj2,
							"<a class=\"btn btn-xs btn-success\" onclick=\"AStart('",
							dataRow["ActivityId"],
							"')\"  >开启</a> "
						});
					}
					if (oneTaoState == OneTaoState.进行中)
					{
						object obj3 = text;
						text = string.Concat(new object[]
						{
							obj3,
							"<a class=\"btn btn-xs btn-danger\" onclick=\"AEnd('",
							dataRow["ActivityId"],
							"','",
							dataRow["FinishedNum"],
							"','",
							dataRow["ReachType"],
							"','",
							dataRow["ReachNum"],
							"')\"  >结束</a> "
						});
					}
					if ((oneTaoState == OneTaoState.已结束 && (int)dataRow["FinishedNum"] == 0) || oneTaoState == OneTaoState.未开始)
					{
						object obj4 = text;
						text = string.Concat(new object[]
						{
							obj4,
							"<a class=\"btn btn-xs btn-danger\" onclick=\"ADel('",
							dataRow["ActivityId"],
							"')\" >删除</a> "
						});
						dataRow["CanDel"] = 1;
					}
					if (oneTaoState == OneTaoState.开奖失败)
					{
						object obj5 = text;
						text = string.Concat(new object[]
						{
							obj5,
							"<a class=\"btn btn-xs btn-danger\" onclick=\"BatchRefund('",
							dataRow["ActivityId"],
							"')\" >批量退款</a> "
						});
					}
					dataRow["ActionBtn"] = text;
				}
				this.Datalist.DataSource = dataTable;
				this.Datalist.DataBind();
				this.pager.TotalRecords = oneyuanTao.TotalRecords;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int oneyuanTaoTotalNum = OneyuanTaoHelp.GetOneyuanTaoTotalNum(out num, out num2, out num3);
				this.ListTotal.Text = "所有夺宝(" + oneyuanTaoTotalNum.ToString() + ")";
				this.ListStart.Text = "进行中(" + num.ToString() + ")";
				this.ListWait.Text = "未开始(" + num2.ToString() + ")";
				this.Listend.Text = "已结束(" + num3.ToString() + ")";
			}
			this.pager.TotalRecords = oneyuanTao.TotalRecords;
		}
	}
}
