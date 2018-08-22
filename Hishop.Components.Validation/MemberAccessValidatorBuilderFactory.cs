using System;
using System.Reflection;

namespace Hishop.Components.Validation
{
	internal class MemberAccessValidatorBuilderFactory
	{
		private MemberValueAccessBuilder valueAccessBuilder;

		public MemberValueAccessBuilder MemberValueAccessBuilder
		{
			get
			{
				return this.valueAccessBuilder;
			}
		}

		public MemberAccessValidatorBuilderFactory() : this(new ReflectionMemberValueAccessBuilder())
		{
		}

		public MemberAccessValidatorBuilderFactory(MemberValueAccessBuilder valueAccessBuilder)
		{
			this.valueAccessBuilder = valueAccessBuilder;
		}

		public virtual ValueAccessValidatorBuilder GetPropertyValueAccessValidatorBuilder(PropertyInfo propertyInfo, IValidatedElement validatedElement)
		{
			return new ValueAccessValidatorBuilder(this.valueAccessBuilder.GetPropertyValueAccess(propertyInfo), validatedElement);
		}

		public virtual ValueAccessValidatorBuilder GetFieldValueAccessValidatorBuilder(FieldInfo fieldInfo, IValidatedElement validatedElement)
		{
			return new ValueAccessValidatorBuilder(this.valueAccessBuilder.GetFieldValueAccess(fieldInfo), validatedElement);
		}

		public virtual ValueAccessValidatorBuilder GetMethodValueAccessValidatorBuilder(MethodInfo methodInfo, IValidatedElement validatedElement)
		{
			return new ValueAccessValidatorBuilder(this.valueAccessBuilder.GetMethodValueAccess(methodInfo), validatedElement);
		}

		public virtual CompositeValidatorBuilder GetTypeValidatorBuilder(Type type, IValidatedElement validatedElement)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return new CompositeValidatorBuilder(validatedElement);
		}
	}
}
