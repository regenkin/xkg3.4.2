using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class NoticeEdit : AdminPage
	{
		protected string adminName = string.Empty;

		protected string reUrl = string.Empty;

		protected string htmlMenuTitleAdd = string.Empty;

		protected string ArticleTitle = string.Empty;

		protected int recordcount;

		protected int sendType;

		protected string menuTitle = "公告";

		protected int Id;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txtTitle;

		protected ucUeditor txtMemo;

		protected System.Web.UI.WebControls.RadioButtonList rbSendTolist;

		protected NoticeEdit() : base("m01", "dpp11")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = Globals.RequestFormStr("posttype");
			string a;
			if ((a = text) != null)
			{
				if (a == "save")
				{
					base.Response.ContentType = "application/json";
					string s = "{\"success\":\"0\",\"tips\":\"操作失败！\"}";
					string title = Globals.RequestFormStr("title");
					this.sendType = Globals.RequestFormNum("type");
					if (this.sendType != 1)
					{
						this.sendType = 0;
					}
					int num = Globals.RequestFormNum("sendto");
					switch (num)
					{
					case 0:
					case 1:
					case 2:
						break;
					default:
						num = 0;
						break;
					}
					this.Id = Globals.RequestFormNum("Id");
					string memo = Globals.RequestFormStr("memo");
					NoticeInfo noticeInfo = new NoticeInfo();
					noticeInfo.Id = this.Id;
					noticeInfo.Title = title;
					noticeInfo.Memo = memo;
					noticeInfo.AddTime = System.DateTime.Now;
					noticeInfo.SendType = this.sendType;
					noticeInfo.SendTo = num;
					ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
					this.adminName = currentManager.UserName;
					noticeInfo.Author = this.adminName;
					if (num == 2)
					{
						System.Data.DataTable dataTable = NoticeHelper.GetTempSelectedUser(this.adminName).Tables[0];
						int count = dataTable.Rows.Count;
						if (count == 0)
						{
							s = "{\"success\":\"0\",\"tips\":\"请先选择用户！\"}";
							base.Response.Write(s);
							base.Response.End();
						}
						else
						{
							System.Collections.Generic.List<NoticeUserInfo> list = new System.Collections.Generic.List<NoticeUserInfo>();
							for (int i = 0; i < count; i++)
							{
								list.Add(new NoticeUserInfo
								{
									UserId = Globals.ToNum(dataTable.Rows[i]["UserID"]),
									NoticeId = 0
								});
							}
							noticeInfo.NoticeUserInfo = list;
						}
					}
					int num2 = NoticeHelper.SaveNotice(noticeInfo);
					if (num2 > 0)
					{
						s = "{\"success\":\"1\",\"id\":" + num2 + "}";
					}
					base.Response.Write(s);
					base.Response.End();
					return;
				}
				if (a == "getselecteduser")
				{
					base.Response.ContentType = "application/json";
					ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
					this.adminName = currentManager.UserName;
					System.Data.DataTable dataTable2 = NoticeHelper.GetSelectedUser(this.adminName).Tables[0];
					int count2 = dataTable2.Rows.Count;
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					if (count2 > 0)
					{
						int j = 0;
						stringBuilder.Append(string.Concat(new string[]
						{
							"{\"name\":\"",
							Globals.String2Json(dataTable2.Rows[j]["username"].ToString()),
							"\",\"tel\":\"",
							Globals.String2Json(dataTable2.Rows[j]["CellPhone"].ToString()),
							"\",\"bindname\":\"",
							Globals.String2Json(dataTable2.Rows[j]["UserBindName"].ToString()),
							"\"}"
						}));
						for (j = 1; j < count2; j++)
						{
							stringBuilder.Append(string.Concat(new string[]
							{
								",{\"name\":\"",
								Globals.String2Json(dataTable2.Rows[j]["username"].ToString()),
								"\",\"tel\":\"",
								Globals.String2Json(dataTable2.Rows[j]["CellPhone"].ToString()),
								"\",\"bindname\":\"",
								Globals.String2Json(dataTable2.Rows[j]["UserBindName"].ToString()),
								"\"}"
							}));
						}
					}
					string s = string.Concat(new object[]
					{
						"{\"success\":\"1\",\"icount\":",
						count2,
						",\"userlist\":[",
						stringBuilder.ToString(),
						"]}"
					});
					base.Response.Write(s);
					base.Response.End();
					return;
				}
			}
			if (!base.IsPostBack)
			{
				this.Id = Globals.RequestQueryNum("Id");
				if (this.Id > 0)
				{
					NoticeInfo noticeInfo2 = NoticeHelper.GetNoticeInfo(this.Id);
					if (noticeInfo2 != null)
					{
						this.txtTitle.Text = noticeInfo2.Title;
						this.txtMemo.Text = noticeInfo2.Memo;
						this.rbSendTolist.SelectedValue = noticeInfo2.SendTo.ToString();
					}
				}
				this.reUrl = Globals.RequestQueryStr("reurl");
				if (string.IsNullOrEmpty(this.reUrl))
				{
					this.reUrl = "noticelist.aspx";
				}
				this.sendType = Globals.RequestQueryNum("type");
				this.rbSendTolist.Items[0].Attributes.Add("onclick", "CancelShowUserList()");
				this.rbSendTolist.Items[1].Attributes.Add("onclick", "CancelShowUserList()");
				int num3 = this.sendType;
				if (num3 == 1)
				{
					this.menuTitle = "消息";
					this.rbSendTolist.Items[2].Attributes.Add("onclick", "ShowUserList()");
				}
				else
				{
					this.rbSendTolist.Items[2].Attributes.Add("class", "hide");
					this.rbSendTolist.Width = 175;
					this.sendType = 0;
				}
				ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
				this.adminName = currentManager.UserName;
			}
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
		}
	}
}
