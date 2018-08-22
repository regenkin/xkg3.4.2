using System;
using System.Linq;
using System.Web;

public class CrawlerHandler : Handler
{
	private string[] Sources;

	private Crawler[] Crawlers;

	public CrawlerHandler(System.Web.HttpContext context) : base(context)
	{
	}

	public override void Process()
	{
		this.Sources = base.Request.Form.GetValues("source[]");
		if (this.Sources == null || this.Sources.Length == 0)
		{
			base.WriteJson(new
			{
				state = "参数错误：没有指定抓取源"
			});
			return;
		}
		this.Crawlers = (from x in this.Sources
		select new Crawler(x, base.Server).Fetch()).ToArray<Crawler>();
		base.WriteJson(new
		{
			state = "SUCCESS",
			list = from x in this.Crawlers
			select new
			{
				state = x.State,
				source = x.SourceUrl,
				url = x.ServerUrl
			}
		});
	}
}
