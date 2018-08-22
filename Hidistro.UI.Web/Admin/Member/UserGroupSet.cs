using Hidistro.ControlPanel.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	public class UserGroupSet : AdminPage
	{
		protected Script Script5;

		protected Script Script6;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.HtmlControls.HtmlInputText txt_time;

		protected System.Web.UI.WebControls.Button btnSaveClientSettings;

		protected UserGroupSet() : base("m04", "hyp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSaveClientSettings.Click += new System.EventHandler(this.btnSaveClientSettings_Click);
			if (!base.IsPostBack)
			{
				this.DataBind();
			}
		}

		public new void DataBind()
		{
			this.txt_time.Value = MemberHelper.SelectUserGroupSet().ToString();
		}

		protected void btnSaveClientSettings_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txt_time.Value))
			{
				this.ShowMsg("请输出时间间隔！", false);
				return;
			}
			if (System.Convert.ToInt32(this.txt_time.Value) < 1)
			{
				this.ShowMsg("请输入大于1的整数！", false);
				return;
			}
			if (this.txt_time.Value.Length > 3)
			{
				this.ShowMsg("间隔时间不能超过999天！", false);
				return;
			}
			if (System.Convert.ToInt32(this.txt_time.Value) > 999)
			{
				this.ShowMsg("间隔时间不能超过999天！", false);
				return;
			}
			if (MemberHelper.SetUserGroup(System.Convert.ToInt32(this.txt_time.Value)) > 0)
			{
				this.ShowMsg("设置成功", true);
				return;
			}
			this.ShowMsg("设置失败", false);
		}
	}
}
