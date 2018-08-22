using System;

namespace Hishop.Components.Validation
{
	internal interface IValidatorDescriptor
	{
		Validator CreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder);
	}
}
