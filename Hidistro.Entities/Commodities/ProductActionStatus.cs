using System;

namespace Hidistro.Entities.Commodities
{
	public enum ProductActionStatus
	{
		Success,
		DuplicateName,
		DuplicateSKU,
		SKUError,
		AttributeError,
		OffShelfError,
		ProductTagEroor,
		UnknowError = 99
	}
}
