using Hidistro.Core.Enums;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Hidistro.Core.Configuration
{
	internal class HiApplication
	{
		private string _name;

		private ApplicationType _appType = ApplicationType.Common;

		private Regex _regex;

		public string Name
		{
			get
			{
				return this._name;
			}
		}

		public ApplicationType ApplicationType
		{
			get
			{
				return this._appType;
			}
		}

		internal HiApplication(string pattern, string name, ApplicationType appType)
		{
			this._name = name.ToLower(CultureInfo.InvariantCulture);
			this._appType = appType;
			this._regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}

		public bool IsMatch(string url)
		{
			return this._regex.IsMatch(url);
		}
	}
}
