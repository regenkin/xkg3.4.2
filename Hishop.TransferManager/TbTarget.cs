using System;

namespace Hishop.TransferManager
{
	public class TbTarget : Target
	{
		public const string TargetName = "淘宝助理";

		public TbTarget(Version version) : base("淘宝助理", version)
		{
		}

		public TbTarget(string versionString) : base("淘宝助理", versionString)
		{
		}

		public TbTarget(int major, int minor, int build) : base("淘宝助理", major, minor, build)
		{
		}
	}
}
