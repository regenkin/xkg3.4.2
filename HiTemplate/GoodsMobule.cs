using HiTemplate.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;

namespace HiTemplate
{
	public class GoodsMobule : RazorModuleWebControl
	{
		[Bindable(true)]
		public string IDs
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			try
			{
				Hi_Json_GoodGourpContent jsonData = ((JObject)JsonConvert.DeserializeObject(this.GetDataJson())).ToObject<Hi_Json_GoodGourpContent>();
				base.RenderModule(writer, jsonData);
			}
			catch (Exception var_1_24)
			{
				Hi_Json_GoodGourpContent jsonData = ((JObject)JsonConvert.DeserializeObject(this.GetDataJson())).ToObject<Hi_Json_GoodGourpContent>();
				base.RenderModule(writer, new Hi_Json_GoodGourpContent());
			}
		}

		public string GetDataJson()
		{
			string result = "";
			try
			{
				string s = string.Format("ShowPrice={0}&Layout={1}&ShowIco={2}&ShowName={3}&IDs={4}", new object[]
				{
					base.ShowPrice,
					base.Layout,
					base.ShowIco,
					base.ShowName,
					this.IDs
				});
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				string text = Urls.ApplicationPath;
				if (string.IsNullOrEmpty(base.DataUrl))
				{
					text += "/api/Hi_Ajax_GoodsList.ashx";
				}
				else
				{
					text += base.DataUrl;
				}
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = (long)bytes.Length;
				try
				{
					Stream requestStream = httpWebRequest.GetRequestStream();
					requestStream.Write(bytes, 0, bytes.Length);
					HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					Stream responseStream = httpWebResponse.GetResponseStream();
					StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
					StringBuilder stringBuilder = new StringBuilder();
					while (-1 != streamReader.Peek())
					{
						stringBuilder.Append(streamReader.ReadLine());
					}
					result = stringBuilder.ToString();
				}
				catch (Exception ex)
				{
					result = "错误：" + ex.Message;
				}
			}
			catch (Exception ex)
			{
				result = "错误：" + ex.Message;
			}
			return result;
		}
	}
}
