using System;

namespace Hishop.TransferManager
{
	public class Target
	{
		public string Name
		{
			get;
			private set;
		}

		public Version Version
		{
			get;
			private set;
		}

		public Target(string name, int major, int minor, int build) : this(name, new Version(major, minor, build))
		{
		}

		public Target(string name, string versionString) : this(name, new Version(versionString))
		{
		}

		public Target(string name, Version version)
		{
			this.Name = name;
			this.Version = version;
		}

		public override string ToString()
		{
			return this.Name + this.Version.ToString();
		}
	}
}
