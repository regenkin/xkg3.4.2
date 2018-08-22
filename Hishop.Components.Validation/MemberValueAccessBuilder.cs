using Hishop.Components.Validation.Properties;
using System;
using System.Reflection;

namespace Hishop.Components.Validation
{
	public abstract class MemberValueAccessBuilder
	{
		public ValueAccess GetFieldValueAccess(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			return this.DoGetFieldValueAccess(fieldInfo);
		}

		protected abstract ValueAccess DoGetFieldValueAccess(FieldInfo fieldInfo);

		public ValueAccess GetMethodValueAccess(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			if (typeof(void) == methodInfo.ReturnType)
			{
				throw new ArgumentException(Resources.ExceptionMethodHasNoReturnValue, "methodInfo");
			}
			if (0 < methodInfo.GetParameters().Length)
			{
				throw new ArgumentException(Resources.ExceptionMethodHasParameters, "methodInfo");
			}
			return this.DoGetMethodValueAccess(methodInfo);
		}

		protected abstract ValueAccess DoGetMethodValueAccess(MethodInfo methodInfo);

		public ValueAccess GetPropertyValueAccess(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("propertyInfo");
			}
			return this.DoGetPropertyValueAccess(propertyInfo);
		}

		protected abstract ValueAccess DoGetPropertyValueAccess(PropertyInfo propertyInfo);
	}
}
