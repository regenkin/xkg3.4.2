using System;

namespace Hidistro.Entities.Commodities
{
	public enum CategoryActionStatus
	{
		Success,
		DuplicateName,
		DeleteForbid,
		DeleteForbidProducts,
		UpdateParentError,
		UnknowError = 99
	}
}
