using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	[ParseChildren(true)]
	public class MemberSearch : TemplatedWebControl
	{
		public delegate void ReSearchEventHandler(object sender, EventArgs e);

		private TextBox txtSearchText;

		private PageSizeDropDownList dropPageSize;

		private IButton btnSearchButton;

		private MemberGradeDropDownList dropMemberRank;

		public event MemberSearch.ReSearchEventHandler ReSearch;

		public MemberQuery Item
		{
			get
			{
				MemberQuery memberQuery = new MemberQuery();
				if (this.txtSearchText != null)
				{
					memberQuery.Username = this.txtSearchText.Text.Trim();
				}
				if (this.dropPageSize != null)
				{
					memberQuery.PageSize = this.dropPageSize.SelectedValue;
				}
				if (this.dropMemberRank != null)
				{
					memberQuery.GradeId = new int?(this.dropMemberRank.SelectedValue.Value);
				}
				return memberQuery;
			}
		}

		public void OnReSearch(object sender, EventArgs e)
		{
			if (this.ReSearch != null)
			{
				this.ReSearch(sender, e);
			}
		}

		protected override void AttachChildControls()
		{
			this.txtSearchText = (TextBox)this.FindControl("txtSearchText");
			this.dropPageSize = (PageSizeDropDownList)this.FindControl("dropPageSize");
			this.btnSearchButton = ButtonManager.Create(this.FindControl("btnSearchButton"));
			this.dropMemberRank = (MemberGradeDropDownList)this.FindControl("rankList");
			this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
			if (!this.Page.IsPostBack)
			{
				this.InitializeControls();
			}
		}

		private void btnSearchButton_Click(object sender, EventArgs e)
		{
			this.OnReSearch(sender, e);
		}

		protected void InitializeControls()
		{
			MemberQuery memberQuery = new MemberQuery();
			if (this.dropPageSize != null)
			{
				this.dropPageSize.SelectedValue = memberQuery.PageSize;
			}
			if (this.dropMemberRank != null)
			{
				this.dropMemberRank.DataBind();
			}
			if (this.Page.Request.QueryString["rankId"] != null)
			{
				int value;
				if (int.TryParse(this.Page.Request.QueryString["rankId"], out value))
				{
					this.dropMemberRank.SelectedValue = new int?(value);
				}
			}
		}
	}
}
