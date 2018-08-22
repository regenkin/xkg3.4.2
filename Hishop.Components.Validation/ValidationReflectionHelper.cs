using Hishop.Components.Validation.Properties;
using Hishop.Components.Validation.Validators;
using System;
using System.Globalization;
using System.Reflection;

namespace Hishop.Components.Validation
{
	internal static class ValidationReflectionHelper
	{
		public static PropertyInfo GetProperty(Type type, string propertyName, bool throwIfInvalid)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentNullException("propertyName");
			}
			PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
			if (ValidationReflectionHelper.IsValidProperty(property))
			{
				return property;
			}
			if (throwIfInvalid)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionInvalidProperty, new object[]
				{
					propertyName,
					type.FullName
				}));
			}
			return null;
		}

		public static bool IsValidProperty(PropertyInfo propertyInfo)
		{
			return propertyInfo != null && propertyInfo.CanRead && propertyInfo.GetIndexParameters().Length == 0;
		}

		public static FieldInfo GetField(Type type, string fieldName, bool throwIfInvalid)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				throw new ArgumentNullException("fieldName");
			}
			FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
			if (ValidationReflectionHelper.IsValidField(field))
			{
				return field;
			}
			if (throwIfInvalid)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionInvalidField, new object[]
				{
					fieldName,
					type.FullName
				}));
			}
			return null;
		}

		public static bool IsValidField(FieldInfo fieldInfo)
		{
			return null != fieldInfo;
		}

		public static MethodInfo GetMethod(Type type, string methodName, bool throwIfInvalid)
		{
			if (string.IsNullOrEmpty(methodName))
			{
				throw new ArgumentNullException("methodName");
			}
			MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);
			if (ValidationReflectionHelper.IsValidMethod(method))
			{
				return method;
			}
			if (throwIfInvalid)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionInvalidMethod, new object[]
				{
					methodName,
					type.FullName
				}));
			}
			return null;
		}

		public static bool IsValidMethod(MethodInfo methodInfo)
		{
			return methodInfo != null && typeof(void) != methodInfo.ReturnType && methodInfo.GetParameters().Length == 0;
		}

		public static T ExtractValidationAttribute<T>(ICustomAttributeProvider attributeProvider, string ruleset) where T : BaseValidationAttribute
		{
			if (attributeProvider != null)
			{
				object[] customAttributes = attributeProvider.GetCustomAttributes(typeof(T), false);
				for (int i = 0; i < customAttributes.Length; i++)
				{
					T result = (T)((object)customAttributes[i]);
					if (ruleset.Equals(result.Ruleset))
					{
						return result;
					}
				}
			}
			return default(T);
		}
	}
}
