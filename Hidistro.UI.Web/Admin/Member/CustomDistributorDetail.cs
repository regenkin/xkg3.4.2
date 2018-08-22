using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	public class CustomDistributorDetail : AdminPage
	{
		protected string clientType = Globals.RequestQueryStr("ClientType").ToLower();

		protected int currentGroupId;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Literal GroupName;

		protected TrimTextBox txtUserNames;

		protected System.Web.UI.WebControls.Button BtnAddMembers;

		protected System.Web.UI.WebControls.Literal litAll;

		protected System.Web.UI.WebControls.Literal litNew;

		protected System.Web.UI.WebControls.Literal litActivy;

		protected System.Web.UI.WebControls.Literal litSleep;

		protected PageSize hrefPageSize;

		protected Pager pager;

		protected System.Web.UI.WebControls.Button lkbDelectCheck;

		protected Grid grdMemberList;

		protected Pager pager1;

		protected CustomDistributorDetail() : base("m04", "hyp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["GroupId"], out this.currentGroupId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.BtnAddMembers.Click += new System.EventHandler(this.BtnAddMembers_Click);
			this.lkbDelectCheck.Click += new System.EventHandler(this.lkbDelectCheck_Click);
			this.grdMemberList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdMemberList_RowDeleting);
			if (!base.IsPostBack)
			{
				CustomGroupingInfo groupInfoById = CustomGroupingHelper.GetGroupInfoById(this.currentGroupId);
				this.GroupName.Text = groupInfoById.GroupName;
				this.BindData();
			}
		}

		protected void BtnAddMembers_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtUserNames.Text.Trim()))
			{
				this.ShowMsg("请输入用户名", false);
				return;
			}
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			string text = this.txtUserNames.Text.Trim().Replace("\r\n", "\n").Replace("\n", ",");
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string username = array[i];
				int memberIdByUserNameOrNiChen = MemberHelper.GetMemberIdByUserNameOrNiChen(username, "");
				if (memberIdByUserNameOrNiChen > 0)
				{
					list.Add(memberIdByUserNameOrNiChen);
				}
			}
			this.txtUserNames.Text = "";
			if (list.Count > 0)
			{
				string text2 = CustomGroupingHelper.AddCustomGroupingUser(list, this.currentGroupId);
				if (string.IsNullOrEmpty(text2))
				{
					this.ShowMsg("添加成功！", true);
				}
				else
				{
					this.ShowMsg(text2, false);
				}
				this.BindData();
				return;
			}
			this.ShowMsg("未找到会员", false);
		}

		protected void lkbDelectCheck_Click(object sender, System.EventArgs e)
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
				this.ShowMsg("请先选择要移除的会员账号！", false);
				return;
			}
			if (CustomGroupingHelper.DelGroupUser(text, this.currentGroupId))
			{
				this.BindData();
			}
		}

		protected void grdMemberList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			string userId = this.grdMemberList.DataKeys[e.RowIndex].Value.ToString();
			if (CustomGroupingHelper.DelGroupUser(userId, this.currentGroupId))
			{
				this.BindData();
			}
		}

		protected void BindData()
		{
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.PageIndex = this.pager.PageIndex;
			memberQuery.SortBy = this.grdMemberList.SortOrderBy;
			memberQuery.PageSize = this.pager.PageSize;
			memberQuery.ClientType = this.clientType;
			memberQuery.GroupId = new int?(this.currentGroupId);
			memberQuery.Stutas = new UserStatus?(UserStatus.Normal);
			if (this.grdMemberList.SortOrder.ToLower() == "desc")
			{
				memberQuery.SortOrder = SortAction.Desc;
			}
			DbQueryResult members = MemberHelper.GetMembers(memberQuery, false);
			this.grdMemberList.DataSource = members.Data;
			this.grdMemberList.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = members.TotalRecords);
		}
	}
}
