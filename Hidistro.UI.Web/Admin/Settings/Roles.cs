using ASPNET.WebControls;
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

namespace Hidistro.UI.Web.Admin.settings
{
	public class Roles : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected Grid grdGroupList;

		protected System.Web.UI.HtmlControls.HtmlInputHidden htxtRoleId;

		protected System.Web.UI.WebControls.TextBox txtAddRoleName;

		protected System.Web.UI.WebControls.TextBox txtRoleDesc;

		protected System.Web.UI.WebControls.Button btnSubmitRoles;

		protected Roles() : base("m09", "szp10")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSubmitRoles.Click += new System.EventHandler(this.btnSubmitRoles_Click);
			this.grdGroupList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdGroupList_RowDeleting);
			if (!this.Page.IsPostBack)
			{
				this.BindUserGroup();
			}
		}

		public void BindUserGroup()
		{
			this.grdGroupList.DataSource = ManagerHelper.GetRoles();
			this.grdGroupList.DataBind();
		}

		private void btnEditRoles_Click()
		{
			RoleInfo roleInfo = new RoleInfo();
			if (string.IsNullOrEmpty(this.txtAddRoleName.Text.Trim()))
			{
				this.ShowMsg("部门名称不能为空，长度限制在60个字符以内", false);
				return;
			}
			roleInfo.RoleId = int.Parse(this.htxtRoleId.Value);
			RoleInfo role = ManagerHelper.GetRole(roleInfo.RoleId);
			if (role == null)
			{
				this.ShowMsg("已经存在相同的部门名称", false);
				return;
			}
			if (ManagerHelper.RoleExists(roleInfo.RoleName) && roleInfo.RoleName != role.RoleName)
			{
				this.ShowMsg("已经存在相同的部门名称", false);
				return;
			}
			roleInfo.RoleName = Globals.HtmlEncode(this.txtAddRoleName.Text.Trim()).Replace(",", "");
			roleInfo.Description = Globals.HtmlEncode(this.txtRoleDesc.Text.Trim());
			roleInfo.IsDefault = bool.Parse(base.Request["rdIsDefault"]);
			ValidationResults validationResults = Validation.Validate<RoleInfo>(roleInfo, new string[]
			{
				"ValRoleInfo"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
				return;
			}
			ManagerHelper.UpdateRole(roleInfo);
			this.BindUserGroup();
		}

		private void grdGroupList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (ManagerHelper.DeleteRole((int)this.grdGroupList.DataKeys[e.RowIndex].Value))
			{
				this.BindUserGroup();
				this.ShowMsg("成功删除了选择的部门", true);
				return;
			}
			this.ShowMsg("删除失败，该部门下已有管理员", false);
		}

		protected void btnSubmitRoles_Click(object sender, System.EventArgs e)
		{
			if (this.htxtRoleId.Value != "")
			{
				this.btnEditRoles_Click();
				return;
			}
			string text = Globals.HtmlEncode(this.txtAddRoleName.Text.Trim()).Replace(",", "");
			string description = Globals.HtmlEncode(this.txtRoleDesc.Text.Trim());
			if (string.IsNullOrEmpty(text) || text.Length > 60)
			{
				this.ShowMsg("部门名称不能为空，长度限制在60个字符以内", false);
				return;
			}
			if (!ManagerHelper.RoleExists(text))
			{
				ManagerHelper.AddRole(new RoleInfo
				{
					RoleName = text,
					Description = description,
					IsDefault = bool.Parse(base.Request["rdIsDefault"])
				});
				this.BindUserGroup();
				this.ShowMsg("成功添加了一个部门", true);
				return;
			}
			this.ShowMsg("已经存在相同的部门名称", false);
		}

		private void Reset()
		{
			this.txtAddRoleName.Text = string.Empty;
			this.txtRoleDesc.Text = string.Empty;
		}
	}
}
