using Hidistro.Entities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	[ParseChildren(true)]
	public class RegionArea : TemplatedWebControl
	{
		private HtmlGenericControl contents;

		private HtmlGenericControl contentRegion;

		protected override void AttachChildControls()
		{
			this.contents = (HtmlGenericControl)this.FindControl("contents");
			this.contentRegion = (HtmlGenericControl)this.FindControl("contentRegion");
			if (!this.Page.IsPostBack)
			{
				this.BindAreasHtml();
				this.BindRegionHtml();
			}
		}

		private void BindAreasHtml()
		{
			Dictionary<int, string> regions = RegionHelper.GetRegions();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<ul>");
			foreach (int current in regions.Keys)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				string arg = string.Empty;
				Dictionary<int, string> provinces = RegionHelper.GetProvinces(current);
				foreach (int current2 in provinces.Keys)
				{
					stringBuilder2.Append(current2.ToString() + ",");
				}
				if (!string.IsNullOrEmpty(stringBuilder2.ToString()))
				{
					arg = stringBuilder2.ToString().Substring(0, stringBuilder2.ToString().Length - 1);
				}
				stringBuilder.AppendFormat("<li> <input id=\"areas_{0}\" onclick=\"checkRansack(this.value,this.checked)\" type=\"checkbox\" value=\"{1}\" /><label for=\"areas_{0}\">{2}</label> </li>", current, arg, regions[current]);
			}
			stringBuilder.Append("</ul>");
			this.contents.InnerHtml = stringBuilder.ToString();
		}

		private void BindRegionHtml()
		{
			Dictionary<int, string> regions = RegionHelper.GetRegions();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<ul>");
			foreach (int current in regions.Keys)
			{
				Dictionary<int, string> provinces = RegionHelper.GetProvinces(current);
				foreach (int current2 in provinces.Keys)
				{
					stringBuilder.AppendFormat("<li> <input id=\"{0}\" type=\"checkbox\"  value=\"{1}\" /><label for=\"{0}\">{1}</label></li> ", current2, provinces[current2]);
				}
			}
			stringBuilder.Append("</ul>");
			this.contentRegion.InnerHtml = stringBuilder.ToString();
		}
	}
}
