using Ajax;
using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.Members)]
	public class MembersIntegralQuery : AdminPage
	{
		private string searchKey;

		private string realName;

		private int? rankId;

		private int? vipCard;

		private bool? approved;

		public string clientType;

		private string phone;

		public string ValidSmsNum = "0";

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txtSearchText;

		protected System.Web.UI.WebControls.TextBox txtPhone;

		protected System.Web.UI.WebControls.TextBox txtRealName;

		protected MemberGradeDropDownList rankList;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected PageSize hrefPageSize;

		protected Pager pager;

		protected Grid grdMemberList;

		protected Pager pager1;

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.grdMemberList.ReBindData += new Grid.ReBindDataEventHandler(this.grdMemberList_ReBindData);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
		}

		public MembersIntegralQuery() : base("m04", "hyp10")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Utility.RegisterTypeForAjax(typeof(ManageMembers));
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.ViewState["ClientType"] = ((base.Request.QueryString["clientType"] != null) ? base.Request.QueryString["clientType"] : null);
				this.BindDDL();
				this.BindData();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}

		public void BindDDL()
		{
			this.rankList.DataBind();
			this.rankList.SelectedValue = this.rankId;
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
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
				{
					this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
				{
					this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["realName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Approved"]))
				{
					this.approved = new bool?(System.Convert.ToBoolean(this.Page.Request.QueryString["Approved"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["phone"]))
				{
					this.phone = this.Page.Request.QueryString["phone"];
				}
				this.rankList.SelectedValue = this.rankId;
				this.txtSearchText.Text = this.searchKey;
				this.txtRealName.Text = this.realName;
				this.txtPhone.Text = this.phone;
				return;
			}
			this.rankId = this.rankList.SelectedValue;
			this.searchKey = this.txtSearchText.Text;
			this.realName = this.txtRealName.Text.Trim();
			this.phone = this.txtPhone.Text.Trim();
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			if (this.rankList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("rankId", this.rankList.SelectedValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("searchKey", this.txtSearchText.Text);
			nameValueCollection.Add("realName", this.txtRealName.Text);
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
			memberQuery.Realname = this.realName;
			memberQuery.GradeId = this.rankId;
			memberQuery.PageIndex = this.pager.PageIndex;
			memberQuery.IsApproved = this.approved;
			memberQuery.SortBy = this.grdMemberList.SortOrderBy;
			memberQuery.PageSize = this.pager.PageSize;
			memberQuery.Stutas = new UserStatus?(UserStatus.Normal);
			memberQuery.EndTime = new System.DateTime?(System.DateTime.Now);
			memberQuery.StartTime = new System.DateTime?(System.DateTime.Now.AddDays((double)(-(double)this.GetSiteSetting().ActiveDay)));
			memberQuery.CellPhone = ((base.Request.QueryString["phone"] != null) ? base.Request.QueryString["phone"] : "");
			memberQuery.ClientType = ((base.Request.QueryString["clientType"] != null) ? base.Request.QueryString["clientType"] : "");
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

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}

		private void ddlApproved_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.ReBind(false);
		}
	}
}
