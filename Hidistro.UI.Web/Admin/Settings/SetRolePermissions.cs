using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class SetRolePermissions : AdminPage
	{
		private int roleId;

		private System.Collections.Generic.IList<RolePermissionInfo> oldPermissions;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Literal ltRoleName;

		protected System.Web.UI.WebControls.Literal ltHtml;

		protected SetRolePermissions() : base("m09", "szp10")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				this.roleId = int.Parse(base.Request.QueryString["roleId"]);
				this.oldPermissions = ManagerHelper.GetRolePremissonsByRoleId(this.roleId);
			}
			catch (System.Exception)
			{
				base.GotoResourceNotFound();
			}
			if (!this.Page.IsPostBack)
			{
				this.BindRoleInfo();
				this.BindDate();
			}
		}

		private void BindRoleInfo()
		{
			RoleInfo role = ManagerHelper.GetRole(this.roleId);
			if (role != null)
			{
				this.ltRoleName.Text = role.RoleName;
			}
		}

		private void BindDate()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendFormat("<input type=\"hidden\" id=\"txtRoleId\" value=\"{0}\" />", this.roleId);
			System.Collections.Generic.Dictionary<string, NavModule> moduleList = Navigation.GetNavigation(true).ModuleList;
			bool isCheck = true;
			foreach (System.Collections.Generic.KeyValuePair<string, NavModule> current in moduleList)
			{
				bool flag = true;
				string empty = string.Empty;
				string firstNoHeaderNode = this.GetFirstNoHeaderNode(current.Value.ItemList, current.Value.ID, out flag, out empty);
				bool flag2 = true;
				string pageLink = this.GetPageLink(current.Value.ItemList, current.Value.ID, empty, out flag2);
				if (!flag2)
				{
					isCheck = false;
				}
				if (!flag)
				{
					isCheck = false;
				}
				stringBuilder.Append(" <div class=\"checkboxlist\">");
				stringBuilder.Append(" <div class=\"boxrow clearfix\">");
				stringBuilder.Append(" <div class=\"classa fl\">");
				stringBuilder.Append(" <label class=\"setalign\">");
				stringBuilder.AppendFormat(" <input type=\"checkbox\" {1}><strong>{0}</strong>", current.Value.Title, this.GetInputCheck(isCheck));
				stringBuilder.Append(" </label>");
				stringBuilder.Append(" </div>");
				stringBuilder.Append(firstNoHeaderNode);
				stringBuilder.Append(" </div>");
				stringBuilder.Append(pageLink);
				stringBuilder.Append(" </div>");
			}
			this.ltHtml.Text = stringBuilder.ToString();
		}

		private string GetFirstNoHeaderNode(System.Collections.Generic.Dictionary<string, NavItem> navItems, string id, out bool isAllCheck, out string nodeKey)
		{
			nodeKey = string.Empty;
			isAllCheck = true;
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("<div class=\"two fl clearfix\">");
			if (navItems.Values.Count > 0)
			{
				System.Collections.Generic.KeyValuePair<string, NavItem> keyValuePair = navItems.First<System.Collections.Generic.KeyValuePair<string, NavItem>>();
				if (string.IsNullOrEmpty(keyValuePair.Value.SpanName))
				{
					nodeKey = keyValuePair.Key;
					int num = 0;
					using (System.Collections.Generic.Dictionary<string, NavPageLink>.Enumerator enumerator = keyValuePair.Value.PageLinks.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							System.Collections.Generic.KeyValuePair<string, NavPageLink> current = enumerator.Current;
							string permissionId = RolePermissionInfo.GetPermissionId(id, current.Value.ID);
							bool flag = this.oldPermissions.FirstOrDefault((RolePermissionInfo p) => p.PermissionId == permissionId) != null;
							if (!flag)
							{
								isAllCheck = false;
							}
							if (num == 0)
							{
								stringBuilder.Append(" <div class=\"titlecheck fl\">");
								stringBuilder.AppendFormat(" <label class=\"setalign\"> <input type=\"checkbox\" name=\"permissions\" value=\"{0}\" {1}>{2}</label>", permissionId, this.GetInputCheck(flag), current.Value.Title);
								stringBuilder.Append("</div>");
							}
							else
							{
								stringBuilder.Append("  <div class=\"twoinerlist fl\">");
								stringBuilder.AppendFormat(" <label class=\"setalign\"> <input type=\"checkbox\" name=\"permissions\" value=\"{0}\" {1}>{2}</label>", permissionId, this.GetInputCheck(flag), current.Value.Title);
								stringBuilder.Append("</div>");
							}
							num++;
						}
						goto IL_185;
					}
				}
				stringBuilder.Append(" <div class=\"titlecheck fl\">");
				stringBuilder.Append("  &nbsp;");
				stringBuilder.Append("</div>");
			}
			IL_185:
			stringBuilder.Append("</div>");
			return stringBuilder.ToString();
		}

		private string GetPageLink(System.Collections.Generic.Dictionary<string, NavItem> navItems, string id, string nodeKey, out bool isAllCheck)
		{
			isAllCheck = true;
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string arg_0F_0 = string.Empty;
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>(navItems.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != nodeKey)
				{
					string nodeString = this.GetNodeString(navItems[list[i]].PageLinks, id, out isAllCheck);
					if (!string.IsNullOrEmpty(navItems[list[i]].SpanName))
					{
						stringBuilder.Append(this.TwoHeaderBegin(navItems[list[i]].SpanName, isAllCheck));
					}
					stringBuilder.Append(nodeString);
					if (i < list.Count - 1)
					{
						if (!string.IsNullOrEmpty(navItems[list[i + 1]].SpanName))
						{
							stringBuilder.Append(this.TwoHeaderEnd());
						}
					}
					else
					{
						stringBuilder.Append(this.TwoHeaderEnd());
					}
				}
			}
			return stringBuilder.ToString();
		}

		private string TwoHeaderBegin(string titleName, bool isCheck)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append(" <div class=\"boxrow clearfix\">");
			stringBuilder.Append(" <div class=\"classa fl\">&nbsp;</div>");
			stringBuilder.Append("<div class=\"two fl clearfix\">");
			stringBuilder.Append("<div class=\"titlecheck fl\">");
			stringBuilder.Append(" <label class=\"setalign\">");
			stringBuilder.AppendFormat(" <input type=\"checkbox\" {1}><strong>{0}</strong>", titleName, this.GetInputCheck(isCheck));
			stringBuilder.Append(" </label>");
			stringBuilder.Append(" </div>");
			stringBuilder.Append(" <div class=\"twoinerlist fl\">");
			return stringBuilder.ToString();
		}

		private string TwoHeaderEnd()
		{
			return " </div> </div> </div>  ";
		}

		private string GetInputCheck(bool isCheck)
		{
			if (!isCheck)
			{
				return "";
			}
			return "checked=\"checked\"";
		}

		private string GetNodeString(System.Collections.Generic.Dictionary<string, NavPageLink> pageLinks, string id, out bool isAllCheck)
		{
			isAllCheck = true;
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (System.Collections.Generic.KeyValuePair<string, NavPageLink> current in pageLinks)
			{
				string permissionId = RolePermissionInfo.GetPermissionId(id, current.Value.ID);
				bool flag = this.oldPermissions.FirstOrDefault((RolePermissionInfo p) => p.PermissionId == permissionId) != null;
				if (!flag)
				{
					isAllCheck = false;
				}
				stringBuilder.Append(" <label class=\"setalign\">");
				stringBuilder.AppendFormat(" <input name=\"permissions\" value=\"{0}\" type=\"checkbox\" {1}>{2}", permissionId, this.GetInputCheck(flag), current.Value.Title);
				stringBuilder.Append(" </label>");
			}
			return stringBuilder.ToString();
		}
	}
}
