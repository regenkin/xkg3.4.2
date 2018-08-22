using System;

namespace Hishop.TransferManager
{
	public class YfxTarget : Target
	{
		public const string TargetName = "分销商城";

		public YfxTarget(Version version) : base("分销商城", version)
		{
		}

		public YfxTarget(string versionString) : base("分销商城", versionString)
		{
		}

		public YfxTarget(int major, int minor, int build) : base("分销商城", major, minor, build)
		{
		}
	}
}
