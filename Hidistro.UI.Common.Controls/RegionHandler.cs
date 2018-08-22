using Hidistro.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Common.Controls
{
	public class RegionHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			try
			{
				string text = context.Request["action"];
				string a;
				if ((a = text) != null)
				{
					if (!(a == "getregions"))
					{
						if (a == "getregioninfo")
						{
							this.GetRegionInfo(context);
						}
					}
					else
					{
						RegionHandler.GetRegions(context);
					}
				}
			}
			catch
			{
			}
		}

		private void GetRegionInfo(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = 0;
			int.TryParse(context.Request["regionId"], out num);
			if (num <= 0)
			{
				context.Response.Write("{\"Status\":\"0\"}");
				return;
			}
			XmlNode region = RegionHelper.GetRegion(num);
			if (region == null)
			{
				context.Response.Write("{\"Status\":\"0\"}");
				return;
			}
			int num2 = 1;
			if (region.Name.Equals("city"))
			{
				num2 = 2;
			}
			else if (region.Name.Equals("county"))
			{
				num2 = 3;
			}
			string str = (num2 > 1) ? RegionHelper.GetFullPath(num) : "";
			string str2 = "";
			if (!region.Name.Equals("province"))
			{
				str2 = region.ParentNode.Attributes["id"].Value;
			}
			string text = "{";
			text += "\"Status\":\"OK\",";
			text = text + "\"RegionId\":\"" + num.ToString(CultureInfo.InvariantCulture) + "\",";
			text = text + "\"Depth\":\"" + num2.ToString(CultureInfo.InvariantCulture) + "\",";
			text = text + "\"Path\":\"" + str + "\",";
			text = text + "\"ParentId\":\"" + str2 + "\"";
			text += "}";
			context.Response.Write(text);
		}

		private static void GetRegions(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = 0;
			int.TryParse(context.Request["parentId"], out num);
			Dictionary<int, string> dictionary;
			if (num > 0)
			{
				XmlNode region = RegionHelper.GetRegion(num);
				if (region == null)
				{
					context.Response.Write("{\"Status\":\"0\"}");
					return;
				}
				if (region.Name.Equals("province"))
				{
					dictionary = RegionHelper.GetCitys(num);
				}
				else
				{
					dictionary = RegionHelper.GetCountys(num);
				}
			}
			else
			{
				dictionary = RegionHelper.GetAllProvinces();
			}
			if (dictionary.Count == 0)
			{
				context.Response.Write("{\"Status\":\"0\"}");
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"Status\":\"OK\",");
			stringBuilder.Append("\"Regions\":[");
			foreach (int current in dictionary.Keys)
			{
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"RegionId\":\"{0}\",", current.ToString(CultureInfo.InvariantCulture));
				stringBuilder.AppendFormat("\"RegionName\":\"{0}\"", dictionary[current]);
				stringBuilder.Append("},");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("]}");
			dictionary.Clear();
			context.Response.Write(stringBuilder.ToString());
		}
	}
}
