using System;

namespace Hishop.Plugins
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class PluginAttribute : Attribute
	{
		public string Name
		{
			get;
			private set;
		}

		public int Sequence
		{
			get;
			set;
		}

		public PluginAttribute(string name)
		{
			this.Name = name;
		}
	}
}
