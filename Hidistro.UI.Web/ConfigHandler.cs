using System;
using System.Web;

public class ConfigHandler : Handler
{
	public ConfigHandler(System.Web.HttpContext context) : base(context)
	{
	}

	public override void Process()
	{
		base.WriteJson(Config.Items);
	}
}
