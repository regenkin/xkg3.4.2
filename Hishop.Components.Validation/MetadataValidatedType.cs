using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hishop.Components.Validation
{
	internal class MetadataValidatedType : MetadataValidatedElement, IValidatedType, IValidatedElement
	{
		public MetadataValidatedType(Type targetType, string ruleset) : base(targetType, ruleset)
		{
		}

		IEnumerable<IValidatedElement> IValidatedType.GetValidatedProperties()
		{
			MetadataValidatedElement metadataValidatedElement = new MetadataValidatedElement(base.Ruleset);
			try
			{
				PropertyInfo[] properties = base.TargetType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
				for (int i = 0; i < properties.Length; i++)
				{
					PropertyInfo propertyInfo = properties[i];
					if (ValidationReflectionHelper.IsValidProperty(propertyInfo))
					{
						metadataValidatedElement.UpdateFlyweight(propertyInfo);
						yield return metadataValidatedElement;
					}
				}
			}
			finally
			{
			}
			yield break;
		}

		IEnumerable<IValidatedElement> IValidatedType.GetValidatedFields()
		{
			MetadataValidatedElement metadataValidatedElement = new MetadataValidatedElement(base.Ruleset);
			try
			{
				FieldInfo[] fields = base.TargetType.GetFields(BindingFlags.Instance | BindingFlags.Public);
				for (int i = 0; i < fields.Length; i++)
				{
					FieldInfo fieldInfo = fields[i];
					metadataValidatedElement.UpdateFlyweight(fieldInfo);
					yield return metadataValidatedElement;
				}
			}
			finally
			{
			}
			yield break;
		}

		IEnumerable<IValidatedElement> IValidatedType.GetValidatedMethods()
		{
			MetadataValidatedElement metadataValidatedElement = new MetadataValidatedElement(base.Ruleset);
			try
			{
				MethodInfo[] methods = base.TargetType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
				for (int i = 0; i < methods.Length; i++)
				{
					MethodInfo methodInfo = methods[i];
					methodInfo.GetParameters();
					if (ValidationReflectionHelper.IsValidMethod(methodInfo))
					{
						metadataValidatedElement.UpdateFlyweight(methodInfo);
						yield return metadataValidatedElement;
					}
				}
			}
			finally
			{
			}
			yield break;
		}

		IEnumerable<MethodInfo> IValidatedType.GetSelfValidationMethods()
		{
			Type targetType = base.TargetType;
			if (targetType.GetCustomAttributes(typeof(HasSelfValidationAttribute), false).Length != 0)
			{
				try
				{
					MethodInfo[] methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					for (int i = 0; i < methods.Length; i++)
					{
						MethodInfo methodInfo = methods[i];
						bool flag = methodInfo.ReturnType != typeof(void);
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (!flag && parameters.Length == 1 && parameters[0].ParameterType == typeof(ValidationResults))
						{
							try
							{
								object[] customAttributes = methodInfo.GetCustomAttributes(typeof(SelfValidationAttribute), false);
								for (int j = 0; j < customAttributes.Length; j++)
								{
									SelfValidationAttribute selfValidationAttribute = (SelfValidationAttribute)customAttributes[j];
									if (base.Ruleset.Equals(selfValidationAttribute.Ruleset))
									{
										yield return methodInfo;
									}
								}
							}
							finally
							{
							}
						}
					}
				}
				finally
				{
				}
			}
			yield break;
		}
	}
}
