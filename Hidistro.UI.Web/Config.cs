using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Web;

public static class Config
{
	private static bool noCache = true;

	private static JObject _Items;

	public static JObject Items
	{
		get
		{
			if (Config.noCache || Config._Items == null)
			{
				Config._Items = Config.BuildItems();
			}
			return Config._Items;
		}
	}

	private static JObject BuildItems()
	{
		string json = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("/hieditor/ueditor/net/config.json"));
		return JObject.Parse(json);
	}

	public static T GetValue<T>(string key)
	{
		return Config.Items[key].Value<T>();
	}

	public static string[] GetStringList(string key)
	{
		return (from x in Config.Items[key]
		select x.Value<string>()).ToArray<string>();
	}

	public static string GetString(string key)
	{
		return Config.GetValue<string>(key);
	}

	public static int GetInt(string key)
	{
		return Config.GetValue<int>(key);
	}
}
