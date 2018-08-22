using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	[PrivilegeCheck(Privilege.EditMember)]
	public class EditMember : AdminPage
	{
		private int currentUserId;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txtBindName;

		protected System.Web.UI.WebControls.TextBox txtUserPassword;

		protected System.Web.UI.HtmlControls.HtmlInputHidden PSWUserIds;

		protected System.Web.UI.WebControls.Button BindCheck;

		protected System.Web.UI.WebControls.Literal lblLoginNameValue;

		protected System.Web.UI.WebControls.Literal LitUserBindName;

		protected MemberGradeDropDownList drpMemberRankList;

		protected System.Web.UI.WebControls.TextBox txtRealName;

		protected System.Web.UI.WebControls.TextBox txtprivateEmail;

		protected RegionSelector rsddlRegion;

		protected System.Web.UI.WebControls.TextBox txtAddress;

		protected System.Web.UI.WebControls.TextBox txtQQ;

		protected System.Web.UI.WebControls.TextBox txtCellPhone;

		protected System.Web.UI.WebControls.TextBox txtCardID;

		protected FormatedTimeLabel lblRegsTimeValue;

		protected System.Web.UI.WebControls.Literal lblTotalAmountValue;

		protected System.Web.UI.WebControls.Button btnEditUser;

		protected EditMember() : base("m04", "hyp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.currentUserId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
			this.BindCheck.Click += new System.EventHandler(this.BindCheck_Click);
			if (!this.Page.IsPostBack)
			{
				this.drpMemberRankList.AllowNull = false;
				this.drpMemberRankList.DataBind();
				this.LoadMemberInfo();
			}
		}

		private void LoadMemberInfo()
		{
			MemberInfo member = MemberHelper.GetMember(this.currentUserId);
			if (member == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.drpMemberRankList.SelectedValue = new int?(member.GradeId);
			this.lblLoginNameValue.Text = member.UserName;
			this.lblRegsTimeValue.Time = member.CreateDate;
			this.lblTotalAmountValue.Text = Globals.FormatMoney(member.Expenditure);
			this.txtRealName.Text = member.RealName;
			this.txtAddress.Text = Globals.HtmlDecode(member.Address);
			this.rsddlRegion.SetSelectedRegionId(new int?(member.RegionId));
			this.txtQQ.Text = member.QQ;
			this.txtCellPhone.Text = member.CellPhone;
			this.txtprivateEmail.Text = member.Email;
			this.txtCardID.Text = member.CardID;
			this.LitUserBindName.Text = member.UserBindName;
			this.BindCheck.CommandName = member.UserId.ToString();
			if (string.IsNullOrEmpty(member.UserBindName))
			{
				this.LitUserBindName.Text = "<a id='bindSysUser'>点击绑定系统用户名</a>";
			}
		}

		protected void BindCheck_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			if (!int.TryParse(this.BindCheck.CommandName, out num))
			{
				this.ShowMsg("用户不存在！", false);
				return;
			}
			string text = this.txtBindName.Text;
			string text2 = this.txtUserPassword.Text;
			MemberInfo bindusernameMember = MemberProcessor.GetBindusernameMember(text);
			if (bindusernameMember != null && bindusernameMember.UserId != num)
			{
				this.ShowMsg("该用户名已经被绑定", false);
				return;
			}
			if (bindusernameMember != null)
			{
				this.ShowMsg("该用户已经绑定系统帐号", false);
				this.LoadMemberInfo();
				return;
			}
			if (MemberProcessor.BindUserName(num, text, HiCryptographer.Md5Encrypt(text2)))
			{
				this.ShowMsg("用户绑定成功!", true);
				MemberInfo member = MemberProcessor.GetMember(num, false);
				Messenger.SendWeiXinMsg_SysBindUserName(member, text2);
				this.LoadMemberInfo();
				return;
			}
			this.ShowMsg("用户绑定失败!", false);
		}

		protected void btnEditUser_Click(object sender, System.EventArgs e)
		{
			MemberInfo member = MemberHelper.GetMember(this.currentUserId);
			int gradeId = member.GradeId;
			member.GradeId = this.drpMemberRankList.SelectedValue.Value;
			member.RealName = this.txtRealName.Text.Trim();
			if (this.rsddlRegion.GetSelectedRegionId().HasValue)
			{
				member.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
				member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
			}
			member.Address = Globals.HtmlEncode(this.txtAddress.Text);
			member.QQ = this.txtQQ.Text;
			member.Email = member.QQ + "@qq.com";
			member.CellPhone = this.txtCellPhone.Text;
			member.Email = this.txtprivateEmail.Text;
			member.CardID = this.txtCardID.Text;
			if (!this.ValidationMember(member))
			{
				return;
			}
			if (gradeId != this.drpMemberRankList.SelectedValue.Value)
			{
				Messenger.SendWeiXinMsg_MemberGradeChange(member);
			}
			if (MemberHelper.Update(member))
			{
				this.ShowMsgAndReUrl("成功修改了当前会员的个人资料", true, "/Admin/member/managemembers.aspx");
				return;
			}
			this.ShowMsg("当前会员的个人信息修改失败", false);
		}

		private bool ValidationMember(MemberInfo member)
		{
			ValidationResults validationResults = Validation.Validate<MemberInfo>(member, new string[]
			{
				"ValMember"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
