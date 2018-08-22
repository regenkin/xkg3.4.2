using System;

namespace Hidistro.Entities.Store
{
	public class RolePermissionInfo
	{
		public string PermissionId
		{
			get;
			set;
		}

		public int RoleId
		{
			get;
			set;
		}

		public static string GetPermissionId(string itemId, string pageLinkId)
		{
			return string.Format("{0}_{1}", itemId, pageLinkId);
		}
	}
}
