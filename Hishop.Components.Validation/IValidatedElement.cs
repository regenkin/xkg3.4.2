using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hishop.Components.Validation
{
	internal interface IValidatedElement
	{
		CompositionType CompositionType
		{
			get;
		}

		string CompositionMessageTemplate
		{
			get;
		}

		string CompositionTag
		{
			get;
		}

		bool IgnoreNulls
		{
			get;
		}

		string IgnoreNullsMessageTemplate
		{
			get;
		}

		string IgnoreNullsTag
		{
			get;
		}

		MemberInfo MemberInfo
		{
			get;
		}

		Type TargetType
		{
			get;
		}

		IEnumerable<IValidatorDescriptor> GetValidatorDescriptors();
	}
}
