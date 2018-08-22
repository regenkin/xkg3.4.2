using System;
using System.Reflection;

namespace Hishop.Components.Validation
{
	internal class MetadataValidatorBuilder : ValidatorBuilderBase
	{
		public MetadataValidatorBuilder()
		{
		}

		public MetadataValidatorBuilder(MemberAccessValidatorBuilderFactory memberAccessValidatorFactory) : base(memberAccessValidatorFactory)
		{
		}

		public Validator CreateValidator(Type type, string ruleset)
		{
			return base.CreateValidator(new MetadataValidatedType(type, ruleset));
		}

		internal Validator CreateValidatorForType(Type type, string ruleset)
		{
			return base.CreateValidatorForValidatedElement(new MetadataValidatedType(type, ruleset), new ValidatorBuilderBase.CompositeValidatorBuilderCreator(base.GetCompositeValidatorBuilderForType));
		}

		internal Validator CreateValidatorForProperty(PropertyInfo propertyInfo, string ruleset)
		{
			return base.CreateValidatorForValidatedElement(new MetadataValidatedElement(propertyInfo, ruleset), new ValidatorBuilderBase.CompositeValidatorBuilderCreator(base.GetCompositeValidatorBuilderForProperty));
		}

		internal Validator CreateValidatorForField(FieldInfo fieldInfo, string ruleset)
		{
			return base.CreateValidatorForValidatedElement(new MetadataValidatedElement(fieldInfo, ruleset), new ValidatorBuilderBase.CompositeValidatorBuilderCreator(base.GetCompositeValidatorBuilderForField));
		}

		internal Validator CreateValidatorForMethod(MethodInfo methodInfo, string ruleset)
		{
			return base.CreateValidatorForValidatedElement(new MetadataValidatedElement(methodInfo, ruleset), new ValidatorBuilderBase.CompositeValidatorBuilderCreator(base.GetCompositeValidatorBuilderForMethod));
		}
	}
}
