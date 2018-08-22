using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_SaveTemplate : System.Web.IHttpHandler
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
			context.Response.ContentType = "text/plain";
			string text = context.Request.Form["content"];
			string text2 = context.Request.Form["id"];
			JObject jo = (JObject)JsonConvert.DeserializeObject(text);
			string text3 = "保存成功";
			string text4 = "1";
			try
			{
				System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(context.Server.MapPath("/Templates/vshop/" + text2.Trim() + "/data/default.json"), false, System.Text.Encoding.UTF8);
				string text5 = text;
				for (int i = 0; i < text5.Length; i++)
				{
					char value = text5[i];
					streamWriter.Write(value);
				}
				streamWriter.Close();
				string text6 = "<%@ Control Language=\"C#\" %>\n<%@ Register TagPrefix=\"Hi\" Namespace=\"HiTemplate\" Assembly=\"HiTemplate\" %>";
				text6 += this.GetPModulesHtml(context, jo);
				string lModulesHtml = this.GetLModulesHtml(context, jo);
				text6 += lModulesHtml;
				string text7 = "/Templates/vshop/" + text2;
				if (!System.IO.Directory.Exists(context.Server.MapPath(text7)))
				{
					System.IO.Directory.CreateDirectory(context.Server.MapPath(text7));
				}
				System.IO.StreamWriter streamWriter2 = new System.IO.StreamWriter(context.Server.MapPath(text7 + "/Skin-HomePage.html"), false, System.Text.Encoding.UTF8);
				string text8 = text6;
				for (int j = 0; j < text8.Length; j++)
				{
					char value2 = text8[j];
					streamWriter2.Write(value2);
				}
				streamWriter2.Close();
			}
			catch (System.Exception ex)
			{
				text3 = ex.Message;
				text4 = "0";
			}
			if (context.Request.Form["is_preview"] == "1")
			{
				context.Response.Write(string.Concat(new string[]
				{
					"{\"status\":",
					text4,
					",\"msg\":\"",
					text3,
					"\",\"link\":\"default.aspx\"}"
				}));
				return;
			}
			context.Response.Write(string.Concat(new string[]
			{
				"{\"status\":",
				text4,
				",\"msg\":\"",
				text3,
				"\"}"
			}));
		}

		public string GetPModulesHtml(System.Web.HttpContext context, JObject jo)
		{
			string text = "";
			foreach (JToken current in ((System.Collections.Generic.IEnumerable<JToken>)jo["PModules"]))
			{
				text += this.Base64Decode(current["dom_conitem"].ToString());
			}
			return text;
		}

		public string GetLModulesHtml(System.Web.HttpContext context, JObject jo)
		{
			string text = "";
			foreach (JToken current in ((System.Collections.Generic.IEnumerable<JToken>)jo["LModules"]))
			{
				if (current["type"].ToString() == "5")
				{
					text += this.GetGoodGroupTag(context, this.Base64Decode(current["dom_conitem"].ToString()), current);
				}
				else if (current["type"].ToString() == "4")
				{
					text += this.GetGoodTag(context, current);
				}
				else
				{
					text += this.Base64Decode(current["dom_conitem"].ToString());
				}
			}
			return text;
		}

		public string GetGoodTag(System.Web.HttpContext context, JToken data)
		{
			string result;
			try
			{
				string text = "";
				foreach (JToken current in ((System.Collections.Generic.IEnumerable<JToken>)data["content"]["goodslist"]))
				{
					text = text + current["item_id"] + ",";
				}
				text = text.TrimEnd(new char[]
				{
					','
				});
				string text2 = "";
				if (!string.IsNullOrEmpty(text))
				{
					string text3 = "/admin/shop/Modules/GoodGroup" + data["content"]["layout"] + ".cshtml";
					text2 = string.Concat(new object[]
					{
						"<Hi:GoodsMobule runat=\"server\" Layout=\"",
						data["content"]["layout"],
						"\" ShowName=\"",
						data["content"]["showName"],
						"\" IDs=\"",
						text,
						"\" ShowIco=\"",
						data["content"]["showIco"],
						"\" ShowPrice=\"",
						data["content"]["showPrice"],
						"\" DataUrl=\"",
						context.Request.Form["getGoodUrl"],
						"\" ID=\"goods_",
						System.Guid.NewGuid().ToString("N"),
						"\" TemplateFile=\"",
						text3,
						"\"    />"
					});
				}
				else
				{
					text2 += this.Base64Decode(data["dom_conitem"].ToString());
				}
				result = text2;
			}
			catch
			{
				result = "";
			}
			return result;
		}

		public string GetGoodGroupTag(System.Web.HttpContext context, string path, JToken data)
		{
			string result;
			try
			{
				string text = string.Concat(new object[]
				{
					"<Hi:GoodsListModule runat=\"server\"  Type=\"goodGroup\" Layout=\"",
					data["content"]["layout"],
					"\" ShowName=\"",
					data["content"]["showName"],
					"\" ShowIco=\"",
					data["content"]["showIco"],
					"\" ShowPrice=\"",
					data["content"]["showPrice"],
					"\" DataUrl=\"",
					context.Request.Form["getGoodGroupUrl"],
					"\" ID=\"group_",
					System.Guid.NewGuid().ToString("N"),
					"\" TemplateFile=\"",
					path,
					"\"  GoodListSize=\"",
					data["content"]["goodsize"],
					"\" FirstPriority=\"",
					data["content"]["firstPriority"],
					"\"  SecondPriority=\"",
					data["content"]["secondPriority"],
					"\"    />"
				});
				result = text;
			}
			catch
			{
				result = "";
			}
			return result;
		}

		public string Base64Code(string message)
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);
			return System.Convert.ToBase64String(bytes);
		}

		public string Base64Decode(string message)
		{
			byte[] bytes = System.Convert.FromBase64String(message);
			return System.Text.Encoding.UTF8.GetString(bytes);
		}
	}
}
