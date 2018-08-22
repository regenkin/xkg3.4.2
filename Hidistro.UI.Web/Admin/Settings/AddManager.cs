using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.settings
{
	public class AddManager : AdminPage
	{
		protected Script Script5;

		protected Script Script7;

		protected Script Script6;

		protected Script Script4;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.HiddenField hidpic;

		protected System.Web.UI.WebControls.HiddenField hidpicdel;

		protected System.Web.UI.WebControls.TextBox txtUserName;

		protected System.Web.UI.WebControls.TextBox txtPassword;

		protected System.Web.UI.WebControls.TextBox txtPasswordagain;

		protected System.Web.UI.WebControls.TextBox txtEmail;

		protected RoleDropDownList dropRole;

		protected System.Web.UI.WebControls.Button btnSave;

		protected AddManager() : base("m09", "szp11")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.dropRole.DataBind();
			}
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			string text = this.txtUserName.Text.Trim();
			if (text.Length > 20 || text.Length < 3)
			{
				this.ShowMsg("3-20个字符，支持汉字、字母、数字等组合", false);
				return;
			}
			if (this.txtPassword.Text.Length > 20 || this.txtPassword.Text.Length < 6)
			{
				this.ShowMsg("密码为6-20个字符，可由英文‘数字及符号组成", false);
				return;
			}
			if (string.Compare(this.txtPassword.Text, this.txtPasswordagain.Text) != 0)
			{
				this.ShowMsg("请确保两次输入的密码相同", false);
				return;
			}
			if (string.Compare(this.txtPassword.Text, this.txtPasswordagain.Text) != 0)
			{
				this.ShowMsg("请确保两次输入的密码相同", false);
				return;
			}
			string text2 = this.txtEmail.Text.Trim();
			if (!System.Text.RegularExpressions.Regex.IsMatch(text2, "^(\\w)+(\\.\\w+)*@(\\w)+((\\.\\w+)+)$"))
			{
				this.ShowMsg("请输入有效的邮箱地址，长度在256个字符以内", false);
				return;
			}
			int num = 0;
			int.TryParse(this.dropRole.SelectedValue.ToString(), out num);
			if (num == 0)
			{
				this.ShowMsg("所属部门没有选择，请选择", false);
				return;
			}
			ManagerInfo manager = ManagerHelper.GetManager(this.txtUserName.Text.Trim());
			if (manager != null)
			{
				this.ShowMsg("用户名已存在", false);
				return;
			}
			if (ManagerHelper.Create(new ManagerInfo
			{
				RoleId = num,
				UserName = text,
				Email = text2,
				Password = HiCryptographer.Md5Encrypt(this.txtPassword.Text.Trim())
			}))
			{
				this.txtEmail.Text = string.Empty;
				this.txtUserName.Text = string.Empty;
				this.ShowMsg("成功添加了一个管理员", true);
			}
		}
	}
}
