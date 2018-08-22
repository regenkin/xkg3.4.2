using System;

namespace Hidistro.Entities.Store
{
	public enum SystemAuthorizationState
	{
		正常权限 = 1,
		未经官方授权 = 0,
		已过授权有效期 = -1
	}
}
