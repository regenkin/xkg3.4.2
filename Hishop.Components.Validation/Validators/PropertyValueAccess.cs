using Hishop.Components.Validation.Properties;
using System;
using System.Globalization;
using System.Reflection;

namespace Hishop.Components.Validation.Validators
{
	internal sealed class PropertyValueAccess : ValueAccess
	{
		private PropertyInfo propertyInfo;

		public override string Key
		{
			get
			{
				return this.propertyInfo.Name;
			}
		}

		public PropertyValueAccess(PropertyInfo propertyInfo)
		{
			this.propertyInfo = propertyInfo;
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
			if (!this.propertyInfo.DeclaringType.IsAssignableFrom(source.GetType()))
			{
				valueAccessFailureMessage = string.Format(CultureInfo.CurrentCulture, Resources.ErrorValueAccessInvalidType, new object[]
				{
					this.Key,
					source.GetType().FullName
				});
				return false;
			}
			value = this.propertyInfo.GetValue(source, null);
			return true;
		}
	}
}
