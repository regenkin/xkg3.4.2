using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hishop.Components.Validation
{
	internal class ValidatorBuilderBase
	{
		protected delegate CompositeValidatorBuilder CompositeValidatorBuilderCreator(IValidatedElement validatedElement);

		private MemberAccessValidatorBuilderFactory memberAccessValidatorFactory;

		public ValidatorBuilderBase() : this(new MemberAccessValidatorBuilderFactory())
		{
		}

		public ValidatorBuilderBase(MemberAccessValidatorBuilderFactory memberAccessValidatorFactory)
		{
			this.memberAccessValidatorFactory = memberAccessValidatorFactory;
		}

		public Validator CreateValidator(IValidatedType validatedType)
		{
			List<Validator> list = new List<Validator>();
			this.CollectValidatorsForType(validatedType, list);
			this.CollectValidatorsForProperties(validatedType.GetValidatedProperties(), list, validatedType.TargetType);
			this.CollectValidatorsForFields(validatedType.GetValidatedFields(), list, validatedType.TargetType);
			this.CollectValidatorsForMethods(validatedType.GetValidatedMethods(), list, validatedType.TargetType);
			this.CollectValidatorsForSelfValidationMethods(validatedType.GetSelfValidationMethods(), list);
			if (list.Count == 1)
			{
				return list[0];
			}
			return new AndCompositeValidator(list.ToArray());
		}

		private void CollectValidatorsForType(IValidatedType validatedType, List<Validator> validators)
		{
			Validator validator = this.CreateValidatorForValidatedElement(validatedType, new ValidatorBuilderBase.CompositeValidatorBuilderCreator(this.GetCompositeValidatorBuilderForType));
			if (validator != null)
			{
				validators.Add(validator);
			}
		}

		private void CollectValidatorsForProperties(IEnumerable<IValidatedElement> validatedElements, List<Validator> validators, Type ownerType)
		{
			foreach (IValidatedElement current in validatedElements)
			{
				Validator validator = this.CreateValidatorForValidatedElement(current, new ValidatorBuilderBase.CompositeValidatorBuilderCreator(this.GetCompositeValidatorBuilderForProperty));
				if (validator != null)
				{
					validators.Add(validator);
				}
			}
		}

		private void CollectValidatorsForFields(IEnumerable<IValidatedElement> validatedElements, List<Validator> validators, Type ownerType)
		{
			foreach (IValidatedElement current in validatedElements)
			{
				Validator validator = this.CreateValidatorForValidatedElement(current, new ValidatorBuilderBase.CompositeValidatorBuilderCreator(this.GetCompositeValidatorBuilderForField));
				if (validator != null)
				{
					validators.Add(validator);
				}
			}
		}

		private void CollectValidatorsForMethods(IEnumerable<IValidatedElement> validatedElements, List<Validator> validators, Type ownerType)
		{
			foreach (IValidatedElement current in validatedElements)
			{
				Validator validator = this.CreateValidatorForValidatedElement(current, new ValidatorBuilderBase.CompositeValidatorBuilderCreator(this.GetCompositeValidatorBuilderForMethod));
				if (validator != null)
				{
					validators.Add(validator);
				}
			}
		}

		private void CollectValidatorsForSelfValidationMethods(IEnumerable<MethodInfo> selfValidationMethods, List<Validator> validators)
		{
			foreach (MethodInfo current in selfValidationMethods)
			{
				validators.Add(new SelfValidationValidator(current));
			}
		}

		protected Validator CreateValidatorForValidatedElement(IValidatedElement validatedElement, ValidatorBuilderBase.CompositeValidatorBuilderCreator validatorBuilderCreator)
		{
			IEnumerator<IValidatorDescriptor> enumerator = validatedElement.GetValidatorDescriptors().GetEnumerator();
			if (!enumerator.MoveNext())
			{
				return null;
			}
			CompositeValidatorBuilder compositeValidatorBuilder = validatorBuilderCreator(validatedElement);
			do
			{
				Validator valueValidator = enumerator.Current.CreateValidator(validatedElement.TargetType, validatedElement.MemberInfo.ReflectedType, this.memberAccessValidatorFactory.MemberValueAccessBuilder);
				compositeValidatorBuilder.AddValueValidator(valueValidator);
			}
			while (enumerator.MoveNext());
			return compositeValidatorBuilder.GetValidator();
		}

		protected CompositeValidatorBuilder GetCompositeValidatorBuilderForProperty(IValidatedElement validatedElement)
		{
			return this.memberAccessValidatorFactory.GetPropertyValueAccessValidatorBuilder(validatedElement.MemberInfo as PropertyInfo, validatedElement);
		}

		protected CompositeValidatorBuilder GetValueCompositeValidatorBuilderForProperty(IValidatedElement validatedElement)
		{
			return new CompositeValidatorBuilder(validatedElement);
		}

		protected CompositeValidatorBuilder GetCompositeValidatorBuilderForField(IValidatedElement validatedElement)
		{
			return this.memberAccessValidatorFactory.GetFieldValueAccessValidatorBuilder(validatedElement.MemberInfo as FieldInfo, validatedElement);
		}

		protected CompositeValidatorBuilder GetCompositeValidatorBuilderForMethod(IValidatedElement validatedElement)
		{
			return this.memberAccessValidatorFactory.GetMethodValueAccessValidatorBuilder(validatedElement.MemberInfo as MethodInfo, validatedElement);
		}

		protected CompositeValidatorBuilder GetCompositeValidatorBuilderForType(IValidatedElement validatedElement)
		{
			return this.memberAccessValidatorFactory.GetTypeValidatorBuilder(validatedElement.MemberInfo as Type, validatedElement);
		}
	}
}
