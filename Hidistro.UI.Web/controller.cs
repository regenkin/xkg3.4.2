using System;
using System.Web;

namespace WebSite.Public.plugins.ueditor
{
	public class controller : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			string key;
			Handler handler;
			switch (key = context.Request["action"])
			{
			case "config":
				handler = new ConfigHandler(context);
				goto IL_291;
			case "uploadimage":
				handler = new UploadHandler(context, new UploadConfig
				{
					AllowExtensions = Config.GetStringList("imageAllowFiles"),
					PathFormat = Config.GetString("imagePathFormat"),
					SizeLimit = Config.GetInt("imageMaxSize"),
					UploadFieldName = Config.GetString("imageFieldName")
				});
				goto IL_291;
			case "uploadscrawl":
				handler = new UploadHandler(context, new UploadConfig
				{
					AllowExtensions = new string[]
					{
						".png"
					},
					PathFormat = Config.GetString("scrawlPathFormat"),
					SizeLimit = Config.GetInt("scrawlMaxSize"),
					UploadFieldName = Config.GetString("scrawlFieldName"),
					Base64 = true,
					Base64Filename = "scrawl.png"
				});
				goto IL_291;
			case "uploadvideo":
				handler = new UploadHandler(context, new UploadConfig
				{
					AllowExtensions = Config.GetStringList("videoAllowFiles"),
					PathFormat = Config.GetString("videoPathFormat"),
					SizeLimit = Config.GetInt("videoMaxSize"),
					UploadFieldName = Config.GetString("videoFieldName")
				});
				goto IL_291;
			case "uploadfile":
				handler = new UploadHandler(context, new UploadConfig
				{
					AllowExtensions = Config.GetStringList("fileAllowFiles"),
					PathFormat = Config.GetString("filePathFormat"),
					SizeLimit = Config.GetInt("fileMaxSize"),
					UploadFieldName = Config.GetString("fileFieldName")
				});
				goto IL_291;
			case "listimage":
				handler = new ListFileManager(context, Config.GetString("imageManagerListPath"), Config.GetStringList("imageManagerAllowFiles"));
				goto IL_291;
			case "listfile":
				handler = new ListFileManager(context, Config.GetString("fileManagerListPath"), Config.GetStringList("fileManagerAllowFiles"));
				goto IL_291;
			case "catchimage":
				handler = new CrawlerHandler(context);
				goto IL_291;
			}
			handler = new NotSupportedHandler(context);
			IL_291:
			handler.Process();
		}
	}
}
