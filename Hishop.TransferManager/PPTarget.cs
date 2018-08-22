using System;

namespace Hishop.TransferManager
{
	public class PPTarget : Target
	{
		public const string TargetName = "拍拍助理";

		public PPTarget(Version version) : base("拍拍助理", version)
		{
		}

		public PPTarget(string versionString) : base("拍拍助理", versionString)
		{
		}

		public PPTarget(int major, int minor, int build) : base("拍拍助理", major, minor, build)
		{
		}
	}
}
