using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class EditManager : AdminPage
	{
		private int userId;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Literal lblLoginNameValue;

		protected RoleDropDownList dropRole;

		protected System.Web.UI.WebControls.TextBox txtprivateEmail;

		protected FormatedTimeLabel lblRegsTimeValue;

		protected System.Web.UI.WebControls.Button btnEditProfile;

		protected EditManager() : base("m09", "szp11")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEditProfile.Click += new System.EventHandler(this.btnEditProfile_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropRole.DataBind();
				ManagerInfo manager = ManagerHelper.GetManager(this.userId);
				if (manager == null)
				{
					this.ShowMsg("匿名用户或非管理员用户不能编辑", false);
					return;
				}
				this.GetAccountInfo(manager);
				this.GetPersonaInfo(manager);
			}
		}

		private void GetPersonaInfo(ManagerInfo user)
		{
			this.txtprivateEmail.Text = user.Email;
		}

		private void GetAccountInfo(ManagerInfo user)
		{
			this.lblLoginNameValue.Text = user.UserName;
			this.lblRegsTimeValue.Time = user.CreateDate;
			this.dropRole.SelectedValue = user.RoleId;
			if (Globals.GetCurrentManagerUserId() == this.userId)
			{
				this.dropRole.Enabled = false;
			}
		}

		private void btnEditProfile_Click(object sender, System.EventArgs e)
		{
			if (!this.Page.IsValid)
			{
				return;
			}
			ManagerInfo manager = ManagerHelper.GetManager(this.userId);
			manager.Email = this.txtprivateEmail.Text;
			if (!this.ValidationManageEamilr(manager))
			{
				return;
			}
			manager.RoleId = this.dropRole.SelectedValue;
			if (ManagerHelper.Update(manager))
			{
				this.ShowMsg("成功修改了当前管理员的个人资料", true);
				return;
			}
			this.ShowMsg("当前管理员的个人信息修改失败", false);
		}

		private bool ValidationManageEamilr(ManagerInfo siteManager)
		{
			ValidationResults validationResults = Validation.Validate<ManagerInfo>(siteManager, new string[]
			{
				"ValManagerEmail"
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
