using Newtonsoft.Json;
using System;
using System.Web;

public abstract class Handler
{
	public System.Web.HttpRequest Request
	{
		get;
		private set;
	}

	public System.Web.HttpResponse Response
	{
		get;
		private set;
	}

	public System.Web.HttpContext Context
	{
		get;
		private set;
	}

	public System.Web.HttpServerUtility Server
	{
		get;
		private set;
	}

	public Handler(System.Web.HttpContext context)
	{
		this.Request = context.Request;
		this.Response = context.Response;
		this.Context = context;
		this.Server = context.Server;
	}

	public abstract void Process();

	protected void WriteJson(object response)
	{
		string text = this.Request["callback"];
		string text2 = JsonConvert.SerializeObject(response);
		if (string.IsNullOrWhiteSpace(text))
		{
			this.Response.AddHeader("Content-Type", "text/plain");
			this.Response.Write(text2);
		}
		else
		{
			this.Response.AddHeader("Content-Type", "application/javascript");
			this.Response.Write(string.Format("{0}({1});", text, text2));
		}
		this.Response.End();
	}
}
