using System;

namespace Hishop.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class HasSelfValidationAttribute : Attribute
	{
	}
}
