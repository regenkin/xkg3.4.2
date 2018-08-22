using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class SaveRolePermissionData : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			this.SaveRolePermission(context);
		}

		private void SaveRolePermission(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			int roleId = 0;
			try
			{
				roleId = int.Parse(context.Request.Form["roleId"]);
			}
			catch (System.Exception)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参与错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			string text = context.Request.Form["rolePermissions"];
			System.Collections.Generic.List<RolePermissionInfo> list = new System.Collections.Generic.List<RolePermissionInfo>();
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[]
				{
					','
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string permissionId = array2[i];
					list.Add(new RolePermissionInfo
					{
						RoleId = roleId,
						PermissionId = permissionId
					});
				}
			}
			bool flag = ManagerHelper.AddRolePermission(list, roleId);
			if (flag)
			{
				stringBuilder.Append("\"status\":\"1\",\"Desciption\":\"操作成功!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			stringBuilder.Append("\"status\":\"1\",\"Desciption\":\"操作成功,该部门未设置任何权限!\"}");
			context.Response.Write(stringBuilder.ToString());
		}
	}
}
