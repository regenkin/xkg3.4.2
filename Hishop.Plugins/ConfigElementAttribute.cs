using System;

namespace Hishop.Plugins
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public sealed class ConfigElementAttribute : Attribute
	{
		public string Name
		{
			get;
			private set;
		}

		public string Description
		{
			get;
			set;
		}

		public InputType InputType
		{
			get;
			set;
		}

		public string[] Options
		{
			get;
			set;
		}

		public bool Nullable
		{
			get;
			set;
		}

		public ConfigElementAttribute(string name)
		{
			this.InputType = InputType.TextBox;
			this.Name = name;
			this.Nullable = true;
		}
	}
}
