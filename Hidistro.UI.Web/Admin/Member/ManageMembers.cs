using Ajax;
using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.StatisticsReport;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.Members)]
	public class ManageMembers : AdminPage
	{
		private string searchKey;

		private string realName;

		private int? rankId;

		private int? vipCard;

		private bool? approved;

		public string clientType;

		private string mstatus = Globals.RequestQueryStr("MemberStatus");

		private string storeName;

		private string phone;

		public string ValidSmsNum = "0";

		protected string addHideCss = string.Empty;

		private StatisticNotifier myNotifier = new StatisticNotifier();

		private UpdateStatistics myEvent = new UpdateStatistics();

		protected string reUrl = string.Empty;

		protected Script Script5;

		protected Script Script6;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Literal ListActive;

		protected System.Web.UI.WebControls.Literal Listfrozen;

		protected System.Web.UI.WebControls.Literal Literal1;

		protected System.Web.UI.WebControls.Literal Literal2;

		protected System.Web.UI.WebControls.TextBox txtSearchText;

		protected System.Web.UI.WebControls.TextBox txtPhone;

		protected System.Web.UI.WebControls.TextBox txtStoreName;

		protected System.Web.UI.WebControls.TextBox txtRealName;

		protected MemberGradeDropDownList rankList;

		protected System.Web.UI.WebControls.DropDownList MemberStatus;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected ExportFieldsCheckBoxList exportFieldsCheckBoxList;

		protected ExportFormatRadioButtonList exportFormatRadioButtonList;

		protected System.Web.UI.WebControls.Button btnExport;

		protected PageSize hrefPageSize;

		protected Pager pager;

		protected System.Web.UI.WebControls.Button lkbDelectCheck1;

		protected Grid grdMemberList;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hdUserId;

		protected System.Web.UI.WebControls.Button huifuUser;

		protected System.Web.UI.WebControls.Button BatchHuifu;

		protected System.Web.UI.WebControls.Button BatchCreatDist;

		protected Pager pager1;

		protected MemberGradeDropDownList GradeCheckList;

		protected System.Web.UI.WebControls.Button GradeCheck;

		protected System.Web.UI.WebControls.Button GroupCheck;

		protected System.Web.UI.WebControls.HiddenField hdCustomGroup;

		protected System.Web.UI.WebControls.Button btnModelGroupCheck;

		protected System.Web.UI.WebControls.HiddenField hdModelGroupCheckUserId;

		protected System.Web.UI.WebControls.HiddenField hdModelCustomGroup;

		protected System.Web.UI.WebControls.DropDownList DDL_ReferralUser;

		protected MemberGradeDropDownList DDL_User;

		protected System.Web.UI.WebControls.TextBox txtContent;

		protected System.Web.UI.HtmlControls.HtmlTextArea txtmsgcontent;

		protected System.Web.UI.WebControls.Button btnSendMessage;

		protected System.Web.UI.WebControls.TextBox txtPassword;

		protected System.Web.UI.WebControls.TextBox txtConformPassword;

		protected System.Web.UI.HtmlControls.HtmlInputHidden PSWUserIds;

		protected System.Web.UI.WebControls.Button PassCheck;

		protected void grdMemberList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				return;
			}
			e.Row.Visible = false;
		}

		protected string GetOpenID(string openId)
		{
			if (string.IsNullOrEmpty(openId))
			{
				return "未绑定 ";
			}
			return "<a href='javascript:void(0)'>" + openId.Substring(0, 10) + "....</a>";
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			string a;
			if ((a = this.mstatus) != null)
			{
				if (a == "1" || a == "0")
				{
					goto IL_5C;
				}
				if (a == "7")
				{
					this.addHideCss = "hide";
					this.lkbDelectCheck1.Visible = false;
					goto IL_5C;
				}
			}
			this.mstatus = "1";
			IL_5C:
			this.grdMemberList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdMemberList_RowDeleting);
			this.grdMemberList.ReBindData += new Grid.ReBindDataEventHandler(this.grdMemberList_ReBindData);
			this.grdMemberList.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdMemberList_RowDataBound);
			this.lkbDelectCheck1.Click += new System.EventHandler(this.lkbDelectCheck_Click);
			this.huifuUser.Click += new System.EventHandler(this.huifu);
			this.BatchHuifu.Click += new System.EventHandler(this.BatchHuifu_click);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			this.GradeCheck.Click += new System.EventHandler(this.GradeCheck_Click);
			this.GroupCheck.Click += new System.EventHandler(this.GroupCheck_Click);
			this.btnModelGroupCheck.Click += new System.EventHandler(this.btnModelGroupCheck_Click);
			this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
			this.PassCheck.Click += new System.EventHandler(this.PassCheck_Click);
			this.BatchCreatDist.Click += new System.EventHandler(this.BatchCreatDist_click);
			if (!this.Page.IsPostBack)
			{
				this.txtPassword.Attributes.Add("value", "888888");
				this.txtConformPassword.Attributes.Add("value", "888888");
			}
		}

		public ManageMembers() : base("m04", "hyp02")
		{
		}

		private void PassCheck_Click(object sender, System.EventArgs e)
		{
			string value = this.PSWUserIds.Value;
			if (value.Length <= 0)
			{
				this.ShowMsg("请先选择要修改密码的会员！", false);
				return;
			}
			if (this.txtPassword.Text.Trim().Length < 6 || this.txtPassword.Text.Trim().Length > 20)
			{
				this.ShowMsg("密码长度在6-20位之间！", false);
				return;
			}
			if (this.txtPassword.Text != this.txtConformPassword.Text)
			{
				this.ShowMsg("两次输入密码不一致！", false);
				return;
			}
			int num = MemberProcessor.SetMultiplePwd(value, HiCryptographer.Md5Encrypt(this.txtPassword.Text.Trim()));
			this.EditPasswordSendWeiXinMessage(value, this.txtPassword.Text.Trim());
			this.ShowMsg(string.Format("成功修改了{0}个会员的密码", num), true);
		}

		private void EditPasswordSendWeiXinMessage(string userIds, string password)
		{
			try
			{
				System.Collections.Generic.List<MemberInfo> memberInfoList = MemberProcessor.GetMemberInfoList(userIds);
				if (memberInfoList != null && memberInfoList.Count > 0)
				{
					foreach (MemberInfo current in memberInfoList)
					{
						if (current != null)
						{
							Messenger.SendWeiXinMsg_PasswordReset(current, password);
						}
					}
				}
			}
			catch (System.Exception)
			{
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			this.Page.Title = masterSettings.SiteName;
			Utility.RegisterTypeForAjax(typeof(ManageMembers));
			this.LoadParameters();
			this.reUrl = base.Request.Url.ToString();
			if (!this.Page.IsPostBack)
			{
				this.ViewState["ClientType"] = ((base.Request.QueryString["clientType"] != null) ? base.Request.QueryString["clientType"] : null);
				this.BindDDL();
				this.BindData();
				this.ValidSmsNum = this.GetSmsValidCount().ToString();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}

		public void BindDDL()
		{
			this.rankList.DataBind();
			this.rankList.SelectedValue = this.rankId;
			this.GradeCheckList.DataBind();
			this.GradeCheckList.SelectedValue = this.rankId;
			this.DDL_User.DataBind();
			this.DDL_User.SelectedValue = this.rankId;
			this.BindDDLDistributors();
		}

		public void BindDDLDistributors()
		{
			DistributorsQuery query = new DistributorsQuery();
			this.DDL_ReferralUser.DataSource = DistributorsBrower.SelectDistributors(query);
			this.DDL_ReferralUser.DataTextField = "StoreName";
			this.DDL_ReferralUser.DataValueField = "UserId";
			this.DDL_ReferralUser.DataBind();
		}

		private SiteSettings GetSiteSetting()
		{
			return SettingsManager.GetMasterSettings(false);
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["rankId"], out value))
				{
					this.rankId = new int?(value);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Username"]))
				{
					this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["Username"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
				{
					this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["realName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Approved"]))
				{
					this.approved = new bool?(System.Convert.ToBoolean(this.Page.Request.QueryString["Approved"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
				{
					this.storeName = this.Page.Request.QueryString["StoreName"];
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["phone"]))
				{
					this.phone = this.Page.Request.QueryString["phone"];
				}
				this.rankList.SelectedValue = this.rankId;
				this.txtSearchText.Text = this.searchKey;
				this.txtRealName.Text = this.realName;
				this.txtStoreName.Text = this.storeName;
				this.MemberStatus.SelectedValue = this.mstatus;
				this.txtPhone.Text = this.phone;
				return;
			}
			this.rankId = this.rankList.SelectedValue;
			this.searchKey = this.txtSearchText.Text;
			this.realName = this.txtRealName.Text.Trim();
			this.storeName = this.txtStoreName.Text;
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			if (this.rankList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("rankId", this.rankList.SelectedValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("Username", this.txtSearchText.Text);
			nameValueCollection.Add("realName", this.txtRealName.Text);
			nameValueCollection.Add("StoreName", this.txtStoreName.Text);
			nameValueCollection.Add("MemberStatus", this.MemberStatus.SelectedItem.Value);
			nameValueCollection.Add("clientType", (this.ViewState["ClientType"] != null) ? this.ViewState["ClientType"].ToString() : "");
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			nameValueCollection.Add("phone", this.txtPhone.Text);
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}

		protected void BindData()
		{
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.Username = this.searchKey;
			memberQuery.UserBindName = this.realName;
			memberQuery.GradeId = this.rankId;
			memberQuery.PageIndex = this.pager.PageIndex;
			memberQuery.IsApproved = this.approved;
			memberQuery.SortBy = this.grdMemberList.SortOrderBy;
			memberQuery.PageSize = this.pager.PageSize;
			memberQuery.Stutas = new UserStatus?((UserStatus)System.Enum.Parse(typeof(UserStatus), this.mstatus));
			memberQuery.EndTime = new System.DateTime?(System.DateTime.Now);
			memberQuery.StartTime = new System.DateTime?(System.DateTime.Now.AddDays((double)(-(double)this.GetSiteSetting().ActiveDay)));
			memberQuery.CellPhone = ((base.Request.QueryString["phone"] != null) ? base.Request.QueryString["phone"] : "");
			memberQuery.ClientType = ((base.Request.QueryString["clientType"] != null) ? base.Request.QueryString["clientType"] : "");
			memberQuery.StoreName = this.storeName;
			if (this.grdMemberList.SortOrder.ToLower() == "desc")
			{
				memberQuery.SortOrder = SortAction.Desc;
			}
			if (this.vipCard.HasValue && this.vipCard.Value != 0)
			{
				memberQuery.HasVipCard = new bool?(this.vipCard.Value == 1);
			}
			DbQueryResult members = MemberHelper.GetMembers(memberQuery, false);
			this.grdMemberList.DataSource = members.Data;
			this.grdMemberList.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = members.TotalRecords);
		}

		private void grdMemberList_ReBindData(object sender)
		{
			this.ReBind(false);
		}

		private void huifu(object sender, System.EventArgs e)
		{
			int userId = 0;
			if (!int.TryParse(this.hdUserId.Value, out userId))
			{
				this.ShowMsg("用户ID不存在！", false);
				return;
			}
			if (!MemberHelper.huifu(userId))
			{
				this.ShowMsg("未知错误", false);
				return;
			}
			this.ShowMsgAndReUrl("恢复成功！", true, this.reUrl);
		}

		private void grdMemberList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int num = (int)this.grdMemberList.DataKeys[e.RowIndex].Value;
			base.Response.Write(num);
			base.Response.End();
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			if (!MemberHelper.Delete(num))
			{
				this.ShowMsg("未知错误", false);
				return;
			}
			try
			{
				this.myNotifier.updateAction = UpdateAction.MemberUpdate;
				this.myNotifier.actionDesc = "删除会员";
				this.myNotifier.RecDateUpdate = System.DateTime.Today;
				this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
				this.myNotifier.UpdateDB();
			}
			catch (System.Exception)
			{
			}
			this.BindData();
			this.ShowMsg("成功删除了选择的会员", true);
		}

		private void BatchHuifu_click(object sender, System.EventArgs e)
		{
			string text = "";
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdMemberList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					text = text + this.grdMemberList.DataKeys[gridViewRow.RowIndex].Value.ToString() + ",";
				}
			}
			text = text.TrimEnd(new char[]
			{
				','
			});
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要恢复的会员账号！", false);
				return;
			}
			if (MemberHelper.BacthHuifu(text))
			{
				this.ShowMsg("成功恢复了选择的会员！", true);
				this.BindData();
			}
		}

		private void BatchCreatDist_click(object sender, System.EventArgs e)
		{
			string text = "";
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdMemberList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					text = text + this.grdMemberList.DataKeys[gridViewRow.RowIndex].Value.ToString() + ",";
				}
			}
			text = text.TrimEnd(new char[]
			{
				','
			});
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要设置成分销商的会员账号！", false);
				return;
			}
			string msg = "";
			if (MemberHelper.CreateDistributorByUserIds(text, ref msg))
			{
				this.ShowMsg(msg, true);
				this.BindData();
				return;
			}
			this.ShowMsg(msg, false);
		}

		private void lkbDelectCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdMemberList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					text = text + this.grdMemberList.DataKeys[gridViewRow.RowIndex].Value.ToString() + ",";
				}
			}
			text = text.TrimEnd(new char[]
			{
				','
			});
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要删除的会员账号！", false);
				return;
			}
			if (VShopHelper.IsExistUsers(text) > 0)
			{
				this.ShowMsg("选中会员中有分销商，请取消分销商资质后再删除！", false);
				return;
			}
			if (MemberHelper.Deletes(text))
			{
				this.ShowMsg("成功删除了选择的会员！", true);
				this.BindData();
			}
		}

		private void btnExport_Click(object sender, System.EventArgs e)
		{
			if (this.exportFieldsCheckBoxList.SelectedItem == null)
			{
				this.ShowMsg("请选择需要导出的会员信息", false);
				return;
			}
			System.Collections.Generic.IList<string> list = new System.Collections.Generic.List<string>();
			System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
			foreach (System.Web.UI.WebControls.ListItem listItem in this.exportFieldsCheckBoxList.Items)
			{
				if (listItem.Selected)
				{
					list.Add(listItem.Value);
					list2.Add(listItem.Text);
				}
			}
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.Username = this.searchKey;
			memberQuery.Realname = this.realName;
			memberQuery.GradeId = this.rankId;
			if (this.vipCard.HasValue && this.vipCard.Value != 0)
			{
				memberQuery.HasVipCard = new bool?(this.vipCard.Value == 1);
			}
			System.Data.DataTable membersNopage = MemberHelper.GetMembersNopage(memberQuery, list);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (string current in list2)
			{
				stringBuilder.Append(current + ",");
				if (current == list2[list2.Count - 1])
				{
					stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
					stringBuilder.Append("\r\n");
				}
			}
			foreach (System.Data.DataRow dataRow in membersNopage.Rows)
			{
				foreach (string current2 in list)
				{
					stringBuilder.Append(dataRow[current2]).Append(",");
					if (current2 == list[list2.Count - 1])
					{
						stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
						stringBuilder.Append("\r\n");
					}
				}
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			if (this.exportFormatRadioButtonList.SelectedValue == "csv")
			{
				this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.csv");
				this.Page.Response.ContentType = "application/octet-stream";
			}
			else
			{
				this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.txt");
				this.Page.Response.ContentType = "application/vnd.ms-word";
			}
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.EnableViewState = false;
			this.Page.Response.Write(stringBuilder.ToString());
			this.Page.Response.End();
		}

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}

		private void ddlApproved_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.ReBind(false);
		}

		private void GradeCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdMemberList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					text = text + this.grdMemberList.DataKeys[gridViewRow.RowIndex].Value.ToString() + ",";
				}
			}
			text = text.TrimEnd(new char[]
			{
				','
			});
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要修改等级的用户", false);
				return;
			}
			int gradeId = System.Convert.ToInt32(this.GradeCheckList.SelectedValue);
			int num = MemberHelper.SetUsersGradeId(text, gradeId);
			if (num > 0)
			{
				string[] array = text.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					MemberInfo member = MemberHelper.GetMember(int.Parse(array[i]));
					if (member != null)
					{
						Messenger.SendWeiXinMsg_MemberGradeChange(member);
					}
				}
			}
			this.ShowMsg(string.Format("成功修改了{0}个用户的等级", num), true);
			this.BindData();
		}

		private void GroupCheck_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdMemberList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					int item = 0;
					if (int.TryParse(this.grdMemberList.DataKeys[gridViewRow.RowIndex].Value.ToString(), out item))
					{
						list.Add(item);
					}
				}
			}
			if (list.Count == 0)
			{
				this.ShowMsg("请先选择需要设置分组的用户", false);
				return;
			}
			if (!string.IsNullOrEmpty(this.hdCustomGroup.Value))
			{
				System.Collections.Generic.IList<int> list2 = new System.Collections.Generic.List<int>();
				if (this.hdCustomGroup.Value.Contains(","))
				{
					string[] array = this.hdCustomGroup.Value.Split(new char[]
					{
						','
					});
					for (int i = 0; i < array.Length; i++)
					{
						string s = array[i];
						int item2 = 0;
						if (int.TryParse(s, out item2))
						{
							list2.Add(item2);
						}
					}
				}
				else if (!this.hdCustomGroup.Value.Equals("-1"))
				{
					int item3 = 0;
					if (int.TryParse(this.hdCustomGroup.Value, out item3))
					{
						list2.Add(item3);
					}
				}
				foreach (int current in list)
				{
					CustomGroupingHelper.SetUserCustomGroup(current, list2);
				}
				this.ShowMsg("设置成功！", true);
				this.BindData();
				return;
			}
			this.ShowMsg("请先选择分组", false);
		}

		private void btnModelGroupCheck_Click(object sender, System.EventArgs e)
		{
			int userId = 0;
			if (int.TryParse(this.hdModelGroupCheckUserId.Value, out userId))
			{
				if (!string.IsNullOrEmpty(this.hdModelCustomGroup.Value))
				{
					System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
					if (this.hdModelCustomGroup.Value.Contains(","))
					{
						string[] array = this.hdModelCustomGroup.Value.Split(new char[]
						{
							','
						});
						for (int i = 0; i < array.Length; i++)
						{
							string s = array[i];
							int item = 0;
							if (int.TryParse(s, out item))
							{
								list.Add(item);
							}
						}
					}
					else if (!this.hdModelCustomGroup.Value.Equals("-1"))
					{
						int item2 = 0;
						if (int.TryParse(this.hdModelCustomGroup.Value, out item2))
						{
							list.Add(item2);
						}
					}
					CustomGroupingHelper.SetUserCustomGroup(userId, list);
					this.ShowMsg("设置成功！", true);
					return;
				}
				this.ShowMsg("请先选择分组", false);
			}
		}

		private void btnSendMessage_Click(object sender, System.EventArgs e)
		{
			SiteSettings siteSetting = this.GetSiteSetting();
			string sMSSender = siteSetting.SMSSender;
			if (string.IsNullOrEmpty(sMSSender))
			{
				this.ShowMsg("请先选择发送方式", false);
				return;
			}
			ConfigData configData = null;
			if (siteSetting.SMSEnabled)
			{
				configData = new ConfigData(HiCryptographer.Decrypt(siteSetting.SMSSettings));
			}
			if (configData == null)
			{
				this.ShowMsg("请先选择发送方式并填写配置信息", false);
				return;
			}
			if (!configData.IsValid)
			{
				string text = "";
				foreach (string current in configData.ErrorMsgs)
				{
					text += Formatter.FormatErrorMessage(current);
				}
				this.ShowMsg(text, false);
				return;
			}
			string text2 = this.txtmsgcontent.Value.Trim();
			if (string.IsNullOrEmpty(text2))
			{
				this.ShowMsg("请先填写发送的内容信息", false);
				return;
			}
			int smsValidCount = this.GetSmsValidCount();
			string text3 = null;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdMemberList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					string text4 = ((System.Web.UI.DataBoundLiteralControl)gridViewRow.Controls[4].Controls[0]).Text.Trim().Replace("<div></div>", "").Replace("&nbsp;", "");
					System.Web.UI.WebControls.HiddenField hiddenField = (System.Web.UI.WebControls.HiddenField)gridViewRow.FindControl("hidCellPhone");
					text4 = hiddenField.Value;
					if (!string.IsNullOrEmpty(text4) && System.Text.RegularExpressions.Regex.IsMatch(text4, "^(13|14|15|18)\\d{9}$"))
					{
						text3 = text3 + text4 + ",";
					}
				}
			}
			if (text3 == null)
			{
				this.ShowMsg("请先选择要发送的会员或检测所选手机号格式是否正确", false);
				return;
			}
			text3 = text3.Substring(0, text3.Length - 1);
			string[] array;
			if (text3.Contains(","))
			{
				array = text3.Split(new char[]
				{
					','
				});
			}
			else
			{
				array = new string[]
				{
					text3
				};
			}
			if (smsValidCount < array.Length)
			{
				this.ShowMsg("发送失败，您的剩余短信条数不足。可用条数：" + smsValidCount.ToString(), false);
				return;
			}
			SMSSender sMSSender2 = SMSSender.CreateInstance(sMSSender, configData.SettingsXml);
			string msg;
			bool success = sMSSender2.Send(array, text2, out msg);
			this.ShowMsg(msg, success);
			this.ValidSmsNum = this.GetSmsValidCount().ToString();
		}

		private int GetSmsValidCount()
		{
			SiteSettings siteSetting = this.GetSiteSetting();
			if (siteSetting.SMSEnabled)
			{
				return int.Parse(this.GetAmount(siteSetting).ToString());
			}
			return 0;
		}

		[AjaxMethod]
		public string SetDistributors(string ids, int rid)
		{
			ids = ids.TrimEnd(new char[]
			{
				','
			});
			if (string.IsNullOrEmpty(ids))
			{
				return "请先选择要修改分销商的用户！";
			}
			if (VShopHelper.IsExistUsers(ids) > 0)
			{
				return "选中会员中有分销商，分销商上下级调整请到栏目“分销->分销商列表”中设置！";
			}
			int num = MemberHelper.SetRegions(ids, rid);
			if (num > 0)
			{
				return string.Format("success", num);
			}
			return "设置失败";
		}

		protected int GetAmount(SiteSettings settings)
		{
			int result = 0;
			if (!string.IsNullOrEmpty(settings.SMSSettings))
			{
				string xml = HiCryptographer.Decrypt(settings.SMSSettings);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				string innerText = xmlDocument.SelectSingleNode("xml/Appkey").InnerText;
				string postData = "method=getAmount&Appkey=" + innerText;
				string text = this.PostData("http://sms.kuaidiantong.cn/getAmount.aspx", postData);
				int num;
				if (int.TryParse(text, out num))
				{
					result = System.Convert.ToInt32(text);
				}
			}
			return result;
		}

		public string PostData(string url, string postData)
		{
			string result = string.Empty;
			try
			{
				System.Uri requestUri = new System.Uri(url);
				System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUri);
				System.Text.Encoding uTF = System.Text.Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = (long)bytes.Length;
				using (System.IO.Stream requestStream = httpWebRequest.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
				using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (System.IO.Stream responseStream = httpWebResponse.GetResponseStream())
					{
						System.Text.Encoding uTF2 = System.Text.Encoding.UTF8;
						System.IO.Stream stream = responseStream;
						if (httpWebResponse.ContentEncoding.ToLower() == "gzip")
						{
							stream = new GZipStream(responseStream, CompressionMode.Decompress);
						}
						else if (httpWebResponse.ContentEncoding.ToLower() == "deflate")
						{
							stream = new DeflateStream(responseStream, CompressionMode.Decompress);
						}
						using (System.IO.StreamReader streamReader = new System.IO.StreamReader(stream, uTF2))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				result = string.Format("获取信息错误：{0}", ex.Message);
			}
			return result;
		}

		[AjaxMethod]
		public string DelUser(int userID)
		{
			if (VShopHelper.IsExistUsers(userID.ToString()) > 0)
			{
				return "isExistVShop";
			}
			if (MemberHelper.Delete2(userID))
			{
				return "success";
			}
			return "fail";
		}

		[AjaxMethod]
		public string GradeCheckUser(string userID, int gradeID)
		{
			MemberInfo member = MemberHelper.GetMember(int.Parse(userID));
			if (MemberHelper.SetUsersGradeId(userID, gradeID) > 0)
			{
				if (gradeID != member.GradeId)
				{
					Messenger.SendWeiXinMsg_MemberGradeChange(member);
				}
				return "success";
			}
			return "fail";
		}

		protected string GetMemberCustomGroup()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Collections.Generic.IList<CustomGroupingInfo> customGroupingList = CustomGroupingHelper.GetCustomGroupingList();
			if (customGroupingList != null && customGroupingList.Count > 0)
			{
				using (System.Collections.Generic.IEnumerator<CustomGroupingInfo> enumerator = customGroupingList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CustomGroupingInfo current = enumerator.Current;
						stringBuilder.Append(" <label class=\"middle mr20\">");
						stringBuilder.AppendFormat("<input type=\"checkbox\" class=\"CustomGroup\" value=\"{0}\">{1}", current.Id, current.GroupName);
						stringBuilder.Append("  </label>");
					}
					goto IL_7D;
				}
			}
			stringBuilder.Append("暂无分组信息，去<a href='../member/CustomDistributorList.aspx'>创建新分组</a>");
			IL_7D:
			return stringBuilder.ToString();
		}

		protected string GetModelMemberCustomGroup()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Collections.Generic.IList<CustomGroupingInfo> customGroupingList = CustomGroupingHelper.GetCustomGroupingList();
			if (customGroupingList != null && customGroupingList.Count > 0)
			{
				using (System.Collections.Generic.IEnumerator<CustomGroupingInfo> enumerator = customGroupingList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CustomGroupingInfo current = enumerator.Current;
						stringBuilder.Append(" <label class=\"middle mr20\">");
						stringBuilder.AppendFormat("<input type=\"checkbox\" class=\"ModelCustomGroup\" value=\"{0}\">{1}", current.Id, current.GroupName);
						stringBuilder.Append("  </label>");
					}
					goto IL_7D;
				}
			}
			stringBuilder.Append("暂无分组信息，去<a href='../member/CustomDistributorList.aspx'>创建新分组</a>");
			IL_7D:
			return stringBuilder.ToString();
		}

		[AjaxMethod]
		public string SetUserPoint(int userID, int points, string remark)
		{
			ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
			MemberHelper.GetMember(userID);
			if (IntegralDetailHelp.AddIntegralDetail(new IntegralDetailInfo
			{
				IntegralSourceType = (points > 0) ? 1 : 2,
				IntegralSource = "(管理员)" + currentManager.UserName + ":手动调整积分",
				Userid = userID,
				IntegralChange = points,
				IntegralStatus = System.Convert.ToInt32(IntegralDetailStatus.OrderToIntegral),
				Remark = remark
			}, null))
			{
				return "success";
			}
			return "fail";
		}
	}
}
