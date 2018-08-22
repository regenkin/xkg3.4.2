using System;
using System.IO.Compression;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class GzipExtention
	{
		public static void Gzip(System.Web.HttpContext context)
		{
			string text = context.Request.Headers["Accept-Encoding"].ToString().ToUpperInvariant();
			if (text.Length > 0)
			{
				if (text.Contains("GZIP"))
				{
					context.Response.AppendHeader("Content-encoding", "gzip");
					context.Response.Filter = new System.IO.Compression.GZipStream(context.Response.Filter, System.IO.Compression.CompressionMode.Compress);
					return;
				}
				if (text.Contains("DEFLATE"))
				{
					context.Response.AppendHeader("Content-encoding", "deflate");
					context.Response.Filter = new System.IO.Compression.DeflateStream(context.Response.Filter, System.IO.Compression.CompressionMode.Compress);
				}
			}
		}
	}
}
