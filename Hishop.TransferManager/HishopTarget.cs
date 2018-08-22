using System;

namespace Hishop.TransferManager
{
	public class HishopTarget : Target
	{
		public const string TargetName = "Hishop";

		public HishopTarget(Version version) : base("Hishop", version)
		{
		}

		public HishopTarget(string versionString) : base("Hishop", versionString)
		{
		}

		public HishopTarget(int major, int minor, int build) : base("Hishop", major, minor, build)
		{
		}
	}
}
