using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	public class MemberDetails : AdminPage
	{
		private int currentUserId;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Literal litUserName;

		protected System.Web.UI.WebControls.TextBox lblUserLink;

		protected System.Web.UI.WebControls.TextBox litGrade;

		protected System.Web.UI.WebControls.TextBox litRealName;

		protected System.Web.UI.WebControls.TextBox litUserBindName;

		protected System.Web.UI.WebControls.TextBox litEmail;

		protected System.Web.UI.WebControls.TextBox litAddress;

		protected System.Web.UI.WebControls.TextBox litQQ;

		protected System.Web.UI.WebControls.TextBox litOpenId;

		protected System.Web.UI.WebControls.TextBox litAlipayOpenid;

		protected System.Web.UI.WebControls.TextBox litCellPhone;

		protected System.Web.UI.WebControls.TextBox txtCardID;

		protected System.Web.UI.WebControls.TextBox litCreateDate;

		protected System.Web.UI.WebControls.Button btnEdit;

		protected MemberDetails() : base("m04", "hyp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.currentUserId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			if (!this.Page.IsPostBack)
			{
				this.LoadMemberInfo();
			}
		}

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			base.Response.Redirect(Globals.GetAdminAbsolutePath("/member/EditMember.aspx?userId=" + this.Page.Request.QueryString["userId"]), true);
		}

		private void LoadMemberInfo()
		{
			MemberInfo member = MemberHelper.GetMember(this.currentUserId);
			if (member == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			System.Uri arg_25_0 = System.Web.HttpContext.Current.Request.Url;
			this.litUserName.Text = member.UserName;
			MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(member.GradeId);
			if (memberGrade != null)
			{
				this.litGrade.Text = memberGrade.Name;
			}
			this.litCreateDate.Text = member.CreateDate.ToString();
			this.litRealName.Text = member.RealName;
			this.litAddress.Text = RegionHelper.GetFullRegion(member.RegionId, "") + member.Address;
			this.litQQ.Text = member.QQ;
			this.litCellPhone.Text = member.CellPhone;
			this.litEmail.Text = member.Email;
			this.litOpenId.Text = member.OpenId;
			this.litAlipayOpenid.Text = member.AlipayOpenid;
			this.txtCardID.Text = member.CardID;
			this.litUserBindName.Text = member.UserBindName;
		}
	}
}
