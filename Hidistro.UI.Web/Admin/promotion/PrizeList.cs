using ASPNET.WebControls;
using ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using Hidistro.Vshop;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class PrizeList : AdminPage
	{
		private string Receiver = "";

		private string StartDate = "";

		private string EndDate = "";

		private string ProductName = "";

		private string AddrReggion = "";

		private string ActitivyTitle = "";

		protected string ShowTabNum = "0";

		protected System.Web.UI.WebControls.HiddenField txtLogID;

		protected System.Web.UI.WebControls.HiddenField txtLogPid;

		protected System.Web.UI.WebControls.HiddenField txtDeliverID;

		protected System.Web.UI.WebControls.HiddenField txtDeliverStaus;

		protected System.Web.UI.WebControls.Button ConfirmSend;

		protected System.Web.UI.WebControls.Button batchDel;

		protected System.Web.UI.WebControls.TextBox txtPrizeReceiver;

		protected System.Web.UI.WebControls.TextBox txtPrizeTel;

		protected System.Web.UI.WebControls.HiddenField HideRegion;

		protected System.Web.UI.WebControls.TextBox txtPrizeAddress;

		protected System.Web.UI.WebControls.Button AddrBtn;

		protected System.Web.UI.WebControls.DropDownList txtPrizeExpressName;

		protected System.Web.UI.WebControls.TextBox txtPrizeCourierNumber;

		protected System.Web.UI.WebControls.HiddenField txtDeliveryTime;

		protected System.Web.UI.WebControls.Button SendBtn;

		protected System.Web.UI.WebControls.LinkButton ListAll;

		protected System.Web.UI.WebControls.LinkButton ListWaitAddr;

		protected System.Web.UI.WebControls.LinkButton ListWaitSend;

		protected System.Web.UI.WebControls.LinkButton ListHasSend;

		protected System.Web.UI.WebControls.LinkButton ListHasReceive;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txtTitle;

		protected System.Web.UI.WebControls.TextBox txtReceiver;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.TextBox txtProductName;

		protected RegionSelector SelReggion;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected System.Web.UI.WebControls.Button btnExportButton;

		protected System.Web.UI.WebControls.Repeater reDistributor;

		protected Pager pager;

		protected PrizeList() : base("m08", "yxp16")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.AddrBtn.Click += new System.EventHandler(this.AddrBtn_Click);
			this.SendBtn.Click += new System.EventHandler(this.SendBtn_Click);
			this.ConfirmSend.Click += new System.EventHandler(this.ConfirmSend_Click);
			this.batchDel.Click += new System.EventHandler(this.batchDel_Click);
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		protected void batchDel_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选择要删除的数据项", false);
				return;
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			int[] ids = System.Array.ConvertAll<string, int>(array, (string s) => int.Parse(s));
			if (GameHelper.DeletePrizesDelivery(ids))
			{
				this.ShowMsg("删除成功！", true);
				this.BindData();
				return;
			}
			this.ShowMsg("删除失败", false);
		}

		protected void ConfirmSend_Click(object sender, System.EventArgs e)
		{
			if (this.txtLogID.Value == "")
			{
				this.ShowMsg("LogID为空，请检查！", false);
				return;
			}
			int id = 0;
			if (!int.TryParse(this.txtDeliverID.Value.Trim(), out id) || this.txtDeliverStaus.Value != "2")
			{
				this.ShowMsg("当前状态下不允许操作！", false);
				return;
			}
			if (GameHelper.UpdatePrizesDelivery(new PrizesDeliveQuery
			{
				Status = 3,
				ReceiveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
				LogId = this.txtLogID.Value,
				Id = id
			}))
			{
				this.ShowMsg("收货在确认成功！", true);
				this.BindData();
				return;
			}
			this.ShowMsg("收货在确认成功失败", false);
		}

		protected void SendBtn_Click(object sender, System.EventArgs e)
		{
			if (this.txtLogID.Value == "")
			{
				this.ShowMsg("LogID为空，请检查！", false);
				return;
			}
			int id = 0;
			if (!int.TryParse(this.txtDeliverID.Value.Trim(), out id) || this.txtDeliverStaus.Value == "" || this.txtDeliverStaus.Value == "3" || this.txtDeliverStaus.Value == "0")
			{
				this.ShowMsg("当前状态下不允许操作！", false);
				return;
			}
			string a = this.txtDeliveryTime.Value.Trim();
			string text = this.txtPrizeExpressName.Text.Trim();
			string text2 = this.txtPrizeCourierNumber.Text.Trim();
			if (text.Length < 2)
			{
				this.ShowMsg("快递公司名称有误！", false);
				return;
			}
			if (text2.Length < 6)
			{
				this.ShowMsg("快递单号不正确！", false);
				return;
			}
			PrizesDeliveQuery prizesDeliveQuery = new PrizesDeliveQuery();
			prizesDeliveQuery.Status = 2;
			prizesDeliveQuery.ExpressName = text;
			prizesDeliveQuery.CourierNumber = text2;
			if (a == "")
			{
				prizesDeliveQuery.DeliveryTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			}
			prizesDeliveQuery.LogId = this.txtLogID.Value;
			prizesDeliveQuery.Id = id;
			if (GameHelper.UpdatePrizesDelivery(prizesDeliveQuery))
			{
				this.ShowMsg("奖品发货信息保存成功！", true);
				this.BindData();
				return;
			}
			this.ShowMsg("奖品发货信息保存失败", false);
		}

		protected void AddrBtn_Click(object sender, System.EventArgs e)
		{
			if (this.txtLogID.Value == "")
			{
				this.ShowMsg("LogID为空，请检查！", false);
				return;
			}
			string text = this.txtPrizeReceiver.Text.Trim();
			string text2 = this.txtPrizeTel.Text.Trim();
			string text3 = this.txtPrizeAddress.Text.Trim();
			string text4 = this.HideRegion.Value.Trim();
			if (text.Length < 2)
			{
				this.ShowMsg("收货人不能为空", false);
				return;
			}
			if (text2.Length < 8)
			{
				this.ShowMsg("联系电话不正确", false);
				return;
			}
			if (text3.Length < 6)
			{
				this.ShowMsg("地址不够详细", false);
				return;
			}
			int currentRegionId = 0;
			if (!int.TryParse(text4, out currentRegionId))
			{
				this.ShowMsg("省市区未选择", false);
				return;
			}
			text4 = RegionHelper.GetFullPath(currentRegionId);
			PrizesDeliveQuery prizesDeliveQuery = new PrizesDeliveQuery();
			prizesDeliveQuery.Status = 1;
			prizesDeliveQuery.ReggionPath = text4;
			prizesDeliveQuery.Address = text3;
			prizesDeliveQuery.Tel = text2;
			prizesDeliveQuery.Receiver = text;
			prizesDeliveQuery.LogId = this.txtLogID.Value;
			prizesDeliveQuery.Pid = this.txtLogPid.Value;
			int id = 0;
			int.TryParse(this.txtDeliverID.Value.Trim(), out id);
			prizesDeliveQuery.Id = id;
			if (prizesDeliveQuery.LogId == "0")
			{
				if (GameHelper.UpdateOneyuanDelivery(prizesDeliveQuery))
				{
					this.ShowMsg("保存收货人信息成功！", true);
					this.BindData();
					return;
				}
				this.ShowMsg("保存信息失败", false);
				return;
			}
			else
			{
				if (GameHelper.UpdatePrizesDelivery(prizesDeliveQuery))
				{
					this.ShowMsg("保存收货人信息成功！", true);
					this.BindData();
					return;
				}
				this.ShowMsg("保存信息失败", false);
				return;
			}
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				this.AddrReggion = "";
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ActitivyTitle"]))
				{
					this.ActitivyTitle = this.Page.Request.QueryString["ActitivyTitle"];
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["AddrReggion"]))
				{
					this.AddrReggion = base.Server.UrlDecode(this.Page.Request.QueryString["AddrReggion"]);
					this.SelReggion.SetSelectedRegionId(new int?(int.Parse(this.AddrReggion)));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShowTabNum"]))
				{
					this.ShowTabNum = base.Server.UrlDecode(this.Page.Request.QueryString["ShowTabNum"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Receiver"]))
				{
					this.Receiver = base.Server.UrlDecode(this.Page.Request.QueryString["Receiver"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
				{
					this.ProductName = base.Server.UrlDecode(this.Page.Request.QueryString["ProductName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
				{
					this.StartDate = base.Server.UrlDecode(this.Page.Request.QueryString["StartDate"]);
					this.calendarStartDate.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.StartDate));
				}
				else
				{
					this.StartDate = "";
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
				{
					this.EndDate = base.Server.UrlDecode(this.Page.Request.QueryString["EndDate"]);
					this.calendarEndDate.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.EndDate));
				}
				else
				{
					this.EndDate = "";
				}
				this.txtReceiver.Text = this.Receiver;
				this.txtProductName.Text = this.ProductName;
				this.txtTitle.Text = this.ActitivyTitle;
				return;
			}
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				this.StartDate = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				this.EndDate = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			this.Receiver = this.txtReceiver.Text;
			this.ProductName = this.txtProductName.Text;
			this.AddrReggion = "";
			this.ActitivyTitle = this.txtTitle.Text;
			if (this.SelReggion.GetSelectedRegionId().HasValue)
			{
				this.AddrReggion = this.SelReggion.GetSelectedRegionId().Value.ToString();
			}
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("Receiver", this.txtReceiver.Text);
			nameValueCollection.Add("ProductName", this.txtProductName.Text);
			nameValueCollection.Add("StartDate", this.StartDate);
			nameValueCollection.Add("EndDate", this.EndDate);
			nameValueCollection.Add("AddrReggion", this.AddrReggion);
			nameValueCollection.Add("ShowTabNum", this.ShowTabNum);
			nameValueCollection.Add("ActitivyTitle", this.ActitivyTitle);
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}

		private void BindData()
		{
			PrizesDeliveQuery prizesDeliveQuery = new PrizesDeliveQuery();
			prizesDeliveQuery.Status = int.Parse(this.ShowTabNum) - 1;
			prizesDeliveQuery.StartDate = this.StartDate;
			prizesDeliveQuery.EndDate = this.EndDate;
			prizesDeliveQuery.SortBy = "WinTime";
			prizesDeliveQuery.PrizeType = 2;
			prizesDeliveQuery.SortOrder = SortAction.Desc;
			prizesDeliveQuery.PageIndex = this.pager.PageIndex;
			prizesDeliveQuery.PageSize = this.pager.PageSize;
			prizesDeliveQuery.ProductName = this.ProductName;
			prizesDeliveQuery.Receiver = this.Receiver;
			prizesDeliveQuery.ReggionId = this.AddrReggion;
			prizesDeliveQuery.ActivityTitle = this.ActitivyTitle;
			Globals.EntityCoding(prizesDeliveQuery, true);
			DbQueryResult allPrizesDeliveryList = GameHelper.GetAllPrizesDeliveryList(prizesDeliveQuery, "", "*");
			this.reDistributor.DataSource = allPrizesDeliveryList.Data;
			this.reDistributor.DataBind();
			this.pager.TotalRecords = allPrizesDeliveryList.TotalRecords;
			this.bidExpress();
			System.Data.DataTable prizesDeliveryNum = GameHelper.GetPrizesDeliveryNum();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			if (prizesDeliveryNum != null && prizesDeliveryNum.Rows.Count > 0)
			{
				num = int.Parse(prizesDeliveryNum.Rows[0]["st0"].ToString());
				num2 = int.Parse(prizesDeliveryNum.Rows[0]["st1"].ToString());
				num3 = int.Parse(prizesDeliveryNum.Rows[0]["st2"].ToString());
				num4 = int.Parse(prizesDeliveryNum.Rows[0]["st3"].ToString());
				num5 = num + num2 + num3 + num4;
			}
			this.ListAll.Text = "所有奖品(" + num5 + ")";
			this.ListWaitAddr.Text = "待填写收货地址(" + num + ")";
			this.ListWaitSend.Text = "待发货(" + num2 + ")";
			this.ListHasSend.Text = "已发货(" + num3 + ")";
			this.ListHasReceive.Text = "已收货(" + num4 + ")";
		}

		private void bidExpress()
		{
			System.Data.DataTable expressTable = ExpressHelper.GetExpressTable();
			this.txtPrizeExpressName.Items.Clear();
			this.txtPrizeExpressName.Items.Add(new System.Web.UI.WebControls.ListItem("----请选择快递公司----", ""));
			foreach (System.Data.DataRow dataRow in expressTable.Rows)
			{
				this.txtPrizeExpressName.Items.Add(new System.Web.UI.WebControls.ListItem((string)dataRow["Name"], (string)dataRow["Name"]));
			}
		}

		protected void tabClick(object sender, System.EventArgs e)
		{
			string commandName = ((System.Web.UI.WebControls.LinkButton)sender).CommandName;
			base.Response.Write(commandName);
			this.txtReceiver.Text = "";
			this.txtProductName.Text = "";
			this.ShowTabNum = commandName;
			this.AddrReggion = "";
			this.StartDate = "";
			this.EndDate = "";
			this.ReBind(true);
		}

		protected void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				this.EndDate = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				this.StartDate = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			if (this.SelReggion.GetSelectedRegionId().HasValue)
			{
				this.AddrReggion = this.SelReggion.GetSelectedRegionId().Value.ToString();
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShowTabNum"]))
			{
				this.ShowTabNum = base.Server.UrlDecode(this.Page.Request.QueryString["ShowTabNum"]);
			}
			this.ReBind(true);
		}

		protected void btnExportButton_Click(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ActitivyTitle"]))
			{
				this.ActitivyTitle = this.Page.Request.QueryString["ActitivyTitle"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["AddrReggion"]))
			{
				this.AddrReggion = base.Server.UrlDecode(this.Page.Request.QueryString["AddrReggion"]);
				this.SelReggion.SetSelectedRegionId(new int?(int.Parse(this.AddrReggion)));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShowTabNum"]))
			{
				this.ShowTabNum = base.Server.UrlDecode(this.Page.Request.QueryString["ShowTabNum"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Receiver"]))
			{
				this.Receiver = base.Server.UrlDecode(this.Page.Request.QueryString["Receiver"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
			{
				this.ProductName = base.Server.UrlDecode(this.Page.Request.QueryString["ProductName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
			{
				this.StartDate = base.Server.UrlDecode(this.Page.Request.QueryString["StartDate"]);
				this.calendarStartDate.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.StartDate));
			}
			else
			{
				this.StartDate = "";
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
			{
				this.EndDate = base.Server.UrlDecode(this.Page.Request.QueryString["EndDate"]);
				this.calendarEndDate.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.EndDate));
			}
			else
			{
				this.EndDate = "";
			}
			PrizesDeliveQuery prizesDeliveQuery = new PrizesDeliveQuery();
			prizesDeliveQuery.Status = int.Parse(this.ShowTabNum) - 1;
			prizesDeliveQuery.StartDate = this.StartDate;
			prizesDeliveQuery.EndDate = this.EndDate;
			prizesDeliveQuery.SortBy = "WinTime";
			prizesDeliveQuery.PrizeType = 2;
			prizesDeliveQuery.SortOrder = SortAction.Desc;
			prizesDeliveQuery.PageIndex = this.pager.PageIndex;
			prizesDeliveQuery.PageSize = this.pager.PageSize;
			prizesDeliveQuery.ProductName = this.ProductName;
			prizesDeliveQuery.Receiver = this.Receiver;
			prizesDeliveQuery.ReggionId = this.AddrReggion;
			prizesDeliveQuery.ActivityTitle = this.ActitivyTitle;
			Globals.EntityCoding(prizesDeliveQuery, true);
			DbQueryResult allPrizesDeliveryList = GameHelper.GetAllPrizesDeliveryList(prizesDeliveQuery, "", "*");
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >活动名称</td>");
			stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >商品名称</td>");
			stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >奖品等级</td>");
			stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >中奖时间</td>");
			stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >收货人</td>");
			stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >联系电话</td>");
			stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >详细地址</td>");
			stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >快递公司</td>");
			stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >快递编号</td>");
			stringBuilder.AppendLine("</tr>");
			System.Data.DataTable dataTable = (System.Data.DataTable)allPrizesDeliveryList.Data;
			string text = string.Empty;
			new System.Collections.Generic.List<int>();
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
				string[] collection = text.Split(new char[]
				{
					','
				});
				System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>(collection);
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					string item = dataTable.Rows[i]["LogId"].ToString();
					if (list.Contains(item))
					{
						stringBuilder.AppendLine("<tr>");
						stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[i]["Title"] + "</td>");
						stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[i]["ProductName"] + "</td>");
						stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + GameHelper.GetPrizeGradeName(dataTable.Rows[i]["PrizeGrade"].ToString()) + "</td>");
						stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[i]["WinTime"] + "</td>");
						stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[i]["Receiver"] + "</td>");
						stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[i]["Tel"] + "</td>");
						stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[i]["Address"] + "</td>");
						stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[i]["ExpressName"] + "</td>");
						stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[i]["CourierNumber"] + "</td>");
						stringBuilder.AppendLine("</tr>");
					}
				}
			}
			else
			{
				for (int j = 0; j < dataTable.Rows.Count; j++)
				{
					dataTable.Rows[j]["LogId"].ToString();
					stringBuilder.AppendLine("<tr>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[j]["Title"] + "</td>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[j]["ProductName"] + "</td>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + GameHelper.GetPrizeGradeName(dataTable.Rows[j]["PrizeGrade"].ToString()) + "</td>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[j]["WinTime"] + "</td>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[j]["Receiver"] + "</td>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[j]["Tel"] + "</td>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[j]["Address"] + "</td>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[j]["ExpressName"] + "</td>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + dataTable.Rows[j]["CourierNumber"] + "</td>");
					stringBuilder.AppendLine("</tr>");
				}
			}
			if (dataTable.Rows.Count == 0)
			{
				stringBuilder.AppendLine("<tr><td></td></tr>");
			}
			stringBuilder.AppendLine("</table>");
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "UTF-8";
			this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=PrizeLists_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
			this.Page.Response.ContentType = "application/ms-excel";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(stringBuilder.ToString());
			this.Page.Response.End();
		}
	}
}
