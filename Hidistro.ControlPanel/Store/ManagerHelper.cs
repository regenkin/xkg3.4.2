using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Hidistro.ControlPanel.Store
{
	public static class ManagerHelper
	{
		public static bool AddRole(RoleInfo role)
		{
			return new RoleDao().AddRole(role);
		}

		public static bool UpdateRole(RoleInfo role)
		{
			return new RoleDao().UpdateRole(role);
		}

		public static bool DeleteRole(int roleId)
		{
			return new RoleDao().DeleteRole(roleId);
		}

		public static bool RoleExists(string roleName)
		{
			return new RoleDao().RoleExists(roleName);
		}

		public static RoleInfo GetRole(int roleId)
		{
			return new RoleDao().GetRole(roleId);
		}

		public static IList<RoleInfo> GetRoles()
		{
			return new RoleDao().GetRoles();
		}

		public static IList<int> GetPrivilegeByRoles(int roleId)
		{
			return new RoleDao().GetPrivilegeByRoles(roleId);
		}

		public static void AddPrivilegeInRoles(int roleId, string strPermissions)
		{
			new RoleDao().AddPrivilegeInRoles(roleId, strPermissions);
		}

		public static void ClearRolePrivilege(int roleId)
		{
			new RoleDao().ClearRolePrivilege(roleId);
		}

		public static ManagerInfo GetManager(int userId)
		{
			ManagerInfo managerInfo = HiCache.Get(string.Format("DataCache-Manager-{0}", userId)) as ManagerInfo;
			if (managerInfo == null)
			{
				managerInfo = new MessageDao().GetManager(userId);
				HiCache.Insert(string.Format("DataCache-Manager-{0}", userId), managerInfo, 360, CacheItemPriority.Normal);
			}
			return managerInfo;
		}

		public static ManagerInfo GetManager(string userName)
		{
			return new MessageDao().GetManager(userName);
		}

		public static ManagerInfo GetCurrentManager()
		{
			return ManagerHelper.GetManager(Globals.GetCurrentManagerUserId());
		}

		public static DbQueryResult GetManagers(ManagerQuery query)
		{
			return new MessageDao().GetManagers(query);
		}

		public static bool Create(ManagerInfo manager)
		{
			return new MessageDao().Create(manager);
		}

		public static bool Delete(int userId)
		{
			ManagerInfo manager = ManagerHelper.GetManager(userId);
			bool result;
			if (manager.UserId == Globals.GetCurrentManagerUserId())
			{
				result = false;
			}
			else
			{
				HiCache.Remove(string.Format("DataCache-Manager-{0}", userId));
				result = new MessageDao().DeleteManager(userId);
			}
			return result;
		}

		public static bool Update(ManagerInfo manager)
		{
			HiCache.Remove(string.Format("DataCache-Manager-{0}", manager.UserId));
			bool result;
			if (new MessageDao().Update(manager))
			{
				HiCache.Insert(string.Format("DataCache-Manager-{0}", manager.UserId), manager, 360, CacheItemPriority.Normal);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static void CheckPrivilege(Privilege privilege)
		{
			ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
			if (currentManager == null)
			{
				HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + privilege.ToString()));
			}
		}

		public static bool AddRolePermission(IList<RolePermissionInfo> models, int roleId = 0)
		{
			if (models.Count > 0)
			{
				roleId = models[0].RoleId;
			}
			HiCache.Remove(string.Format("DataCache-RolePermissions-{0}", roleId));
			return new RolePermissionDao().AddRolePermission(models, roleId);
		}

		public static bool AddRolePermission(RolePermissionInfo model)
		{
			return ManagerHelper.AddRolePermission(new List<RolePermissionInfo>
			{
				model
			}, 0);
		}

		public static bool IsHavePermission(RolePermissionInfo model)
		{
			string text = HiCache.Get(string.Format("DataCache-RolePermissions-{0}", model.RoleId)) as string;
			List<RolePermissionInfo> list = new List<RolePermissionInfo>();
			if (!string.IsNullOrEmpty(text))
			{
				string value = HiCryptographer.Decrypt(text);
				list = JsonConvert.DeserializeObject<List<RolePermissionInfo>>(value);
			}
			if (list == null || list.Count == 0)
			{
				list = new RolePermissionDao().GetPermissionsByRoleId(model.RoleId);
				string obj = HiCryptographer.Encrypt(JsonConvert.SerializeObject(list));
				HiCache.Insert(string.Format("DataCache-RolePermissions-{0}", model.RoleId), obj, 360, CacheItemPriority.Normal);
			}
			bool result;
			if (list == null || list.Count == 0)
			{
				result = false;
			}
			else
			{
				RolePermissionInfo rolePermissionInfo = list.FirstOrDefault((RolePermissionInfo p) => p.PermissionId == model.PermissionId);
				result = (rolePermissionInfo != null);
			}
			return result;
		}

		public static bool IsHavePermission(string itemId, string pageLinkId, int roleId)
		{
			return pageLinkId == "dpp01" || pageLinkId == "00000" || ManagerHelper.IsHavePermission(new RolePermissionInfo
			{
				PermissionId = RolePermissionInfo.GetPermissionId(itemId, pageLinkId),
				RoleId = roleId
			});
		}

		public static IList<RolePermissionInfo> GetRolePremissonsByRoleId(int roleId)
		{
			return new RolePermissionDao().GetPermissionsByRoleId(roleId);
		}
	}
}
