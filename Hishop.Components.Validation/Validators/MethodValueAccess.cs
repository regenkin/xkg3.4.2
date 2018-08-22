using Hishop.Components.Validation.Properties;
using System;
using System.Globalization;
using System.Reflection;

namespace Hishop.Components.Validation.Validators
{
	internal sealed class MethodValueAccess : ValueAccess
	{
		private MethodInfo methodInfo;

		public override string Key
		{
			get
			{
				return this.methodInfo.Name;
			}
		}

		public MethodValueAccess(MethodInfo methodInfo)
		{
			this.methodInfo = methodInfo;
		}

		public override bool GetValue(object source, out object value, out string valueAccessFailureMessage)
		{
			value = null;
			valueAccessFailureMessage = null;
			if (source == null)
			{
				valueAccessFailureMessage = string.Format(CultureInfo.CurrentCulture, Resources.ErrorValueAccessNull, new object[]
				{
					this.Key
				});
				return false;
			}
			if (!this.methodInfo.DeclaringType.IsAssignableFrom(source.GetType()))
			{
				valueAccessFailureMessage = string.Format(CultureInfo.CurrentCulture, Resources.ErrorValueAccessInvalidType, new object[]
				{
					this.Key,
					source.GetType().FullName
				});
				return false;
			}
			value = this.methodInfo.Invoke(source, null);
			return true;
		}
	}
}
