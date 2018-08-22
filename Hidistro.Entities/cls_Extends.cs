using System;

namespace Hidistro.Entities
{
	public static class cls_Extends
	{
		public static string ReplaceSingleQuoteMark(this string target)
		{
			return target.Replace("'", "''");
		}

		public static bool bInt(this string val, ref int i)
		{
			return !string.IsNullOrEmpty(val) && !val.Contains(".") && !val.Contains("-") && int.TryParse(val, out i);
		}

		public static bool bDecimal(this string val, ref decimal i)
		{
			return !string.IsNullOrEmpty(val) && decimal.TryParse(val, out i);
		}

		public static bool bDate(this string val, ref System.DateTime i)
		{
			return !string.IsNullOrEmpty(val) && System.DateTime.TryParse(val, out i);
		}

		public static bool bBool(this string val, ref bool i)
		{
			return !string.IsNullOrEmpty(val) && bool.TryParse(val, out i);
		}
	}
}
