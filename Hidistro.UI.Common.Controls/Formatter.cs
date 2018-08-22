using System;
using System.Globalization;

namespace Hidistro.UI.Common.Controls
{
	public sealed class Formatter
	{
		private Formatter()
		{
		}

		public static string FormatErrorMessage(string msg)
		{
			return string.Format(CultureInfo.InvariantCulture, "<li>{0}</li>", new object[]
			{
				msg
			});
		}
	}
}
