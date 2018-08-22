using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class EditManagerPassword : AdminPage
	{
		private int userId;

		protected Script Script4;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Literal lblLoginNameValue;

		protected System.Web.UI.HtmlControls.HtmlGenericControl panelOld;

		protected System.Web.UI.WebControls.TextBox txtOldPassWord;

		protected System.Web.UI.WebControls.TextBox txtNewPassWord;

		protected System.Web.UI.WebControls.TextBox txtPassWordCompare;

		protected System.Web.UI.WebControls.Button btnEditPassWordOK;

		protected EditManagerPassword() : base("m09", "szp11")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEditPassWordOK.Click += new System.EventHandler(this.btnEditPassWordOK_Click);
			if (!this.Page.IsPostBack)
			{
				ManagerInfo manager = ManagerHelper.GetManager(this.userId);
				if (manager == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.lblLoginNameValue.Text = manager.UserName;
				this.GetSecurity();
			}
		}

		private void btnEditPassWordOK_Click(object sender, System.EventArgs e)
		{
			ManagerInfo manager = ManagerHelper.GetManager(this.userId);
			if (Globals.GetCurrentManagerUserId() == this.userId && manager.Password != HiCryptographer.Md5Encrypt(this.txtOldPassWord.Text))
			{
				this.ShowMsg("旧密码输入不正确", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtNewPassWord.Text) || this.txtNewPassWord.Text.Length > 20 || this.txtNewPassWord.Text.Length < 6)
			{
				this.ShowMsg("密码不能为空，长度限制在6-20个字符之间", false);
				return;
			}
			if (string.Compare(this.txtNewPassWord.Text, this.txtPassWordCompare.Text) != 0)
			{
				this.ShowMsg("两次输入的密码不一样", false);
				return;
			}
			HiConfiguration config = HiConfiguration.GetConfig();
			if (string.IsNullOrEmpty(this.txtNewPassWord.Text) || this.txtNewPassWord.Text.Length < 6 || this.txtNewPassWord.Text.Length > config.PasswordMaxLength)
			{
				this.ShowMsg(string.Format("管理员登录密码的长度只能在{0}和{1}个字符之间", 6, config.PasswordMaxLength), false);
				return;
			}
			manager.Password = HiCryptographer.Md5Encrypt(this.txtNewPassWord.Text);
			if (ManagerHelper.Update(manager))
			{
				this.ShowMsg("成功修改了管理员登录密码", true);
			}
		}

		private void GetSecurity()
		{
			if (Globals.GetCurrentManagerUserId() != this.userId)
			{
				this.panelOld.Visible = false;
				return;
			}
			this.panelOld.Visible = true;
		}
	}
}
