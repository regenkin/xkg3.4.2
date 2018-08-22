using System;

namespace Hidistro.Entities.Sales
{
	public enum PaymentModeActionStatus
	{
		Success,
		DuplicateName,
		OutofNumber,
		DuplicateGateway,
		UnknowError = 99
	}
}
