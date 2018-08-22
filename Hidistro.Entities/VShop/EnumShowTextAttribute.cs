using System;

namespace Hidistro.Entities.VShop
{
	[System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
	public sealed class EnumShowTextAttribute : System.Attribute
	{
		public string ShowText
		{
			get;
			private set;
		}

		public EnumShowTextAttribute(string showTest)
		{
			this.ShowText = showTest;
		}
	}
}
