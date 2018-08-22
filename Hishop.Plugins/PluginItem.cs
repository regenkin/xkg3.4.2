using System;

namespace Hishop.Plugins
{
	public class PluginItem
	{
		public virtual string FullName
		{
			get;
			set;
		}

		public virtual string DisplayName
		{
			get;
			set;
		}

		public virtual string Logo
		{
			get;
			set;
		}

		public virtual string ShortDescription
		{
			get;
			set;
		}

		public virtual string Description
		{
			get;
			set;
		}

		public virtual string ToJsonString()
		{
			return string.Concat(new string[]
			{
				"{\"FullName\":\"",
				this.FullName,
				"\",\"DisplayName\":\"",
				this.DisplayName,
				"\",\"Logo\":\"",
				this.Logo,
				"\",\"ShortDescription\":\"",
				this.ShortDescription,
				"\",\"Description\":\"",
				this.Description,
				"\"}"
			});
		}

		public virtual string ToXmlString()
		{
			return string.Concat(new string[]
			{
				"<xml><FullName>",
				this.FullName,
				"</FullName><DisplayName>",
				this.DisplayName,
				"</DisplayName><Logo>",
				this.Logo,
				"</Logo><ShortDescription>",
				this.ShortDescription,
				"</ShortDescription><Description>",
				this.Description,
				"</Description></xml>"
			});
		}
	}
}
