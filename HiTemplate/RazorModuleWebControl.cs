using RazorEngine;
using RazorEngine.Templating;
using System;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HiTemplate
{
	public class RazorModuleWebControl : WebControl
	{
		[Bindable(true)]
		public string TemplateFile
		{
			get;
			set;
		}

		[Bindable(true)]
		public string ShowPrice
		{
			get;
			set;
		}

		[Bindable(true)]
		public string ShowIco
		{
			get;
			set;
		}

		[Bindable(true)]
		public string ShowName
		{
			get;
			set;
		}

		[Bindable(true)]
		public string DataUrl
		{
			get;
			set;
		}

		[Bindable(true)]
		public string Layout
		{
			get;
			set;
		}

		protected virtual void RenderModule(HtmlTextWriter writer, object jsonData)
		{
			string name = "TemplateCacheKey-" + this.ID;
			string key = "TemplateFileCacheKey-" + this.ID;
			string text = HttpContext.Current.Cache[key] as string;
			if (string.IsNullOrEmpty(text) || text.Length == 0)
			{
				string text2 = HttpContext.Current.Request.MapPath(this.TemplateFile);
				text = File.ReadAllText(text2);
				string value = Engine.Razor.RunCompile(new LoadedTemplateSource(text, null), name, null, jsonData, null);
				HttpContext.Current.Cache.Insert(key, text, new CacheDependency(text2), DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
				writer.Write(value);
			}
			else
			{
				string value = Engine.Razor.IsTemplateCached(name, null) ? Engine.Razor.Run(name, null, jsonData, null) : Engine.Razor.RunCompile(new LoadedTemplateSource(text, null), name, null, jsonData, null);
				writer.Write(value);
			}
		}
	}
}
