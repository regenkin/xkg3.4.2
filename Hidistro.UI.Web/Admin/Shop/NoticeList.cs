using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class NoticeList : AdminPage
	{
		protected string localUrl = string.Empty;

		protected string htmlMenuTitleAdd = string.Empty;

		protected string ArticleTitle = string.Empty;

		private int pageno;

		protected int recordcount;

		protected int sendType;

		private string title = string.Empty;

		protected System.Web.UI.WebControls.TextBox txtTitle;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.TextBox txtUserName;

		protected System.Web.UI.WebControls.DropDownList ddlState;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Button btnDel;

		protected System.Web.UI.WebControls.Button btnPub;

		protected System.Web.UI.WebControls.HyperLink hlinkAdd;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected Pager pager;

		protected NoticeList() : base("m01", "dpp11")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.sendType = Globals.RequestQueryNum("type");
			this.pageno = Globals.RequestQueryNum("pageindex");
			if (this.pageno < 1)
			{
				this.pageno = 1;
			}
			this.localUrl = base.Request.Url.ToString();
			if (this.sendType == 1)
			{
				this.btnDel.OnClientClick = "return HiConform('<strong>消息删除后将不可恢复，并且用户也将看不到内容！</strong><p>确定要批量删除所选择的消息吗？</p>', this);";
				this.hlinkAdd.NavigateUrl = "noticeedit.aspx?type=1&reurl=" + base.Server.UrlEncode(this.localUrl);
				this.hlinkAdd.Text = "发布新消息";
			}
			else
			{
				this.btnDel.OnClientClick = "return HiConform('<strong>公告删除后将不可恢复，并且用户也将看不到内容！</strong><p>确定要批量删除所选择的公告吗？</p>', this);";
				this.hlinkAdd.NavigateUrl = "noticeedit.aspx?reurl=" + base.Server.UrlEncode(this.localUrl);
				this.hlinkAdd.Text = "创建新的公告";
			}
			if (!base.IsPostBack)
			{
				this.BindData(this.pageno, this.sendType);
			}
		}

		private void BindData(int pageno, int sendtype)
		{
			NoticeQuery noticeQuery = new NoticeQuery();
			noticeQuery.SortBy = "ID";
			noticeQuery.SortOrder = SortAction.Desc;
			Globals.EntityCoding(noticeQuery, true);
			noticeQuery.PageIndex = pageno;
			noticeQuery.SendType = sendtype;
			noticeQuery.PageSize = this.pager.PageSize;
			noticeQuery.IsDistributor = new bool?(true);
			this.title = Globals.RequestQueryStr("title");
			string text = Globals.RequestQueryStr("starttime");
			string text2 = Globals.RequestQueryStr("endtime");
			string text3 = Globals.RequestQueryStr("username");
			string text4 = Globals.RequestQueryStr("state");
			if (!string.IsNullOrEmpty(this.title))
			{
				noticeQuery.Title = this.title;
				this.txtTitle.Text = this.title;
			}
			try
			{
				if (!string.IsNullOrEmpty(text))
				{
					noticeQuery.StartTime = new System.DateTime?(System.DateTime.Parse(text));
					this.calendarStartDate.Text = noticeQuery.StartTime.Value.ToString("yyyy-MM-dd");
				}
				if (!string.IsNullOrEmpty(text2))
				{
					noticeQuery.EndTime = new System.DateTime?(System.DateTime.Parse(text2));
					this.calendarEndDate.Text = noticeQuery.EndTime.Value.ToString("yyyy-MM-dd");
				}
			}
			catch
			{
			}
			if (!string.IsNullOrEmpty(text3))
			{
				noticeQuery.Author = text3;
				this.txtUserName.Text = text3;
			}
			string a;
			if ((a = text4) != null && (a == "0" || a == "1"))
			{
				noticeQuery.IsPub = new int?(Globals.ToNum(text4));
				this.ddlState.SelectedValue = text4;
			}
			noticeQuery.SortBy = "IsPub asc,AddTime";
			DbQueryResult noticeRequest = NoticeHelper.GetNoticeRequest(noticeQuery);
			this.rptList.DataSource = noticeRequest.Data;
			this.rptList.DataBind();
			int totalRecords = noticeRequest.TotalRecords;
			this.pager.TotalRecords = totalRecords;
			this.recordcount = totalRecords;
			if (this.pager.TotalRecords <= this.pager.PageSize)
			{
				this.pager.Visible = false;
			}
		}

		protected string FormatSendTo(object sendto, object id)
		{
			string text = string.Empty;
			switch (Globals.ToNum(sendto))
			{
			case 0:
				text = "所有用户";
				break;
			case 1:
				text = "分销商";
				break;
			case 2:
			{
				text = "指定用户";
				int selectedUser = NoticeHelper.GetSelectedUser(Globals.ToNum(id));
				if (selectedUser > 0)
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						"(<span style='color:green'>",
						selectedUser,
						"</span>)"
					});
				}
				break;
			}
			}
			return text;
		}

		protected string FormatIsPub(object ispub)
		{
			string result = string.Empty;
			switch (Globals.ToNum(ispub))
			{
			case 0:
				result = "<span class='red'>待发布</span>";
				break;
			case 1:
				result = "已发布";
				break;
			}
			return result;
		}

		protected void rptList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			string commandName;
			if ((commandName = e.CommandName) != null)
			{
				int noticeid;
				if (commandName == "delete")
				{
					noticeid = Globals.ToNum(e.CommandArgument.ToString());
					NoticeHelper.DelNotice(noticeid);
					this.ShowMsg("成功删除了指定的" + ((this.sendType == 1) ? "消息" : "公告"), true);
					this.BindData(this.pageno, this.sendType);
					return;
				}
				if (!(commandName == "pub"))
				{
					return;
				}
				noticeid = Globals.ToNum(e.CommandArgument.ToString());
				NoticeHelper.NoticePub(noticeid);
				this.ShowMsg("成功发布了指定的" + ((this.sendType == 1) ? "消息" : "公告"), true);
				this.BindData(this.pageno, this.sendType);
			}
		}

		protected void rptList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				int num = (int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "IsPub");
				int num2 = (int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Id");
				System.Web.UI.WebControls.Button button = e.Item.FindControl("btnPub") as System.Web.UI.WebControls.Button;
				System.Web.UI.WebControls.HyperLink hyperLink = e.Item.FindControl("hpLinkEdit") as System.Web.UI.WebControls.HyperLink;
				button.Visible = (num == 0);
				if (num == 0)
				{
					hyperLink.NavigateUrl = string.Concat(new object[]
					{
						"noticeedit.aspx?id=",
						num2,
						"&reurl=",
						base.Server.UrlEncode(this.localUrl)
					});
					hyperLink.Visible = true;
				}
			}
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			string s = this.txtTitle.Text.Trim();
			string text = Globals.RequestFormStr("ctl00$ContentPlaceHolder1$calendarStartDate$txtDateTimePicker");
			string text2 = Globals.RequestFormStr("ctl00$ContentPlaceHolder1$calendarEndDate$txtDateTimePicker");
			string text3 = this.txtUserName.Text.Trim();
			string text4 = this.ddlState.SelectedValue.Trim();
			string url = string.Concat(new object[]
			{
				"NoticeList.aspx?type=",
				this.sendType,
				"&title=",
				base.Server.UrlEncode(s),
				"&starttime=",
				text,
				"&endtime=",
				text2,
				"&username=",
				text3,
				"&state=",
				text4
			});
			base.Response.Redirect(url);
			base.Response.End();
		}

		protected void btnDel_Click(object sender, System.EventArgs e)
		{
			string text = Globals.RequestFormStr("cbNoticeGroup").Trim();
			string[] array = text.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string s = array2[i];
				int num = Globals.ToNum(s);
				if (num > 0)
				{
					NoticeHelper.DelNotice(num);
				}
			}
			this.ShowMsg("成功删除了指定的" + ((this.sendType == 1) ? "消息" : "公告"), true);
			this.BindData(this.pageno, this.sendType);
		}
	}
}
