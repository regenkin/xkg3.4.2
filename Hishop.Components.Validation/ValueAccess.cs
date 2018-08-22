using System;

namespace Hishop.Components.Validation
{
	public abstract class ValueAccess
	{
		public abstract string Key
		{
			get;
		}

		public abstract bool GetValue(object source, out object value, out string valueAccessFailureMessage);
	}
}
