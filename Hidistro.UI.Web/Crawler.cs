using System;
using System.IO;
using System.Net;
using System.Web;

public class Crawler
{
	public string SourceUrl
	{
		get;
		set;
	}

	public string ServerUrl
	{
		get;
		set;
	}

	public string State
	{
		get;
		set;
	}

	private System.Web.HttpServerUtility Server
	{
		get;
		set;
	}

	public Crawler(string sourceUrl, System.Web.HttpServerUtility server)
	{
		this.SourceUrl = sourceUrl;
		this.Server = server;
	}

	public Crawler Fetch()
	{
		System.Net.HttpWebRequest httpWebRequest = System.Net.WebRequest.Create(this.SourceUrl) as System.Net.HttpWebRequest;
		Crawler result;
		using (System.Net.HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as System.Net.HttpWebResponse)
		{
			if (httpWebResponse.StatusCode != System.Net.HttpStatusCode.OK)
			{
				this.State = string.Concat(new object[]
				{
					"Url returns ",
					httpWebResponse.StatusCode,
					", ",
					httpWebResponse.StatusDescription
				});
				result = this;
			}
			else if (httpWebResponse.ContentType.IndexOf("image") == -1)
			{
				this.State = "Url is not an image";
				result = this;
			}
			else
			{
				this.ServerUrl = PathFormatter.Format(System.IO.Path.GetFileName(this.SourceUrl), Config.GetString("catcherPathFormat"));
				string path = this.Server.MapPath(this.ServerUrl);
				if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(path)))
				{
					System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
				}
				try
				{
					System.IO.Stream responseStream = httpWebResponse.GetResponseStream();
					System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(responseStream);
					byte[] bytes;
					using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
					{
						byte[] array = new byte[4096];
						int count;
						while ((count = binaryReader.Read(array, 0, array.Length)) != 0)
						{
							memoryStream.Write(array, 0, count);
						}
						bytes = memoryStream.ToArray();
					}
					System.IO.File.WriteAllBytes(path, bytes);
					this.State = "SUCCESS";
				}
				catch (System.Exception ex)
				{
					this.State = "抓取错误：" + ex.Message;
				}
				result = this;
			}
		}
		return result;
	}
}
