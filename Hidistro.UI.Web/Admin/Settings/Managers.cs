using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.settings
{
	public class Managers : AdminPage
	{
		protected Script Script5;

		protected Script Script7;

		protected Script Script6;

		protected Script Script4;

		protected Script Script9;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txtSearchText;

		protected RoleDropDownList dropRolesList;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected Pager pager;

		protected Grid grdManager;

		protected Managers() : base("m09", "szp11")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdManager.ReBindData += new Grid.ReBindDataEventHandler(this.grdManager_ReBindData);
			this.grdManager.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdManager_RowDeleting);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropRolesList.DataBind();
				this.BindData();
			}
		}

		private void grdManager_ReBindData(object sender)
		{
			this.ReloadManagerLogs(false);
		}

		private void grdManager_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int num = (int)this.grdManager.DataKeys[e.RowIndex].Value;
			if (Globals.GetCurrentManagerUserId() == num)
			{
				this.ShowMsg("不能删除自己", false);
				return;
			}
			ManagerInfo manager = ManagerHelper.GetManager(num);
			if (!ManagerHelper.Delete(manager.UserId))
			{
				this.ShowMsg("未知错误", false);
				return;
			}
			this.BindData();
			this.ShowMsg("成功删除了一个管理员", true);
		}

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadManagerLogs(true);
		}

		private void ReloadManagerLogs(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("Username", this.txtSearchText.Text);
			nameValueCollection.Add("RoleId", System.Convert.ToString(this.dropRolesList.SelectedValue));
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("SortBy", this.grdManager.SortOrderBy);
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			base.ReloadPage(nameValueCollection);
		}

		private void BindData()
		{
			ManagerQuery managerQuery = this.GetManagerQuery();
			DbQueryResult managers = ManagerHelper.GetManagers(managerQuery);
			this.grdManager.DataSource = managers.Data;
			this.grdManager.DataBind();
			this.txtSearchText.Text = managerQuery.Username;
			this.dropRolesList.SelectedValue = managerQuery.RoleId;
			this.pager.TotalRecords = managers.TotalRecords;
		}

		private ManagerQuery GetManagerQuery()
		{
			ManagerQuery managerQuery = new ManagerQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Username"]))
			{
				managerQuery.Username = base.Server.UrlDecode(this.Page.Request.QueryString["Username"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RoleId"]))
			{
				managerQuery.RoleId = int.Parse(this.Page.Request.QueryString["RoleId"]);
			}
			managerQuery.PageSize = this.pager.PageSize;
			managerQuery.PageIndex = this.pager.PageIndex;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortBy"]))
			{
				managerQuery.SortBy = this.Page.Request.QueryString["SortBy"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
			{
				managerQuery.SortOrder = SortAction.Desc;
			}
			return managerQuery;
		}
	}
}
