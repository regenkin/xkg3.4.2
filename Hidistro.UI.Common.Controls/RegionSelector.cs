using Hidistro.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Common.Controls
{
	public class RegionSelector : WebControl
	{
		private int? currentRegionId;

		private bool dataLoaded;

		private WebControl ddlProvinces;

		private WebControl ddlCitys;

		private WebControl ddlCountys;

		private int? provinceId;

		private int? cityId;

		private int? countyId;

		public string ProvinceTitle
		{
			get;
			set;
		}

		public string CityTitle
		{
			get;
			set;
		}

		public string CountyTitle
		{
			get;
			set;
		}

		public string Separator
		{
			get;
			set;
		}

		public string SelectedRegions
		{
			get
			{
				int? selectedRegionId = this.GetSelectedRegionId();
				if (!selectedRegionId.HasValue)
				{
					return "";
				}
				return RegionHelper.GetFullRegion(selectedRegionId.Value, this.Separator);
			}
			set
			{
				string[] array = value.Split(new char[]
				{
					','
				});
				if (array.Length >= 3)
				{
					int? selectedRegionId = new int?(RegionHelper.GetRegionId(array[2], array[1], array[0]));
					this.SetSelectedRegionId(selectedRegionId);
				}
			}
		}

		public string NullToDisplay
		{
			get;
			set;
		}

		public override ControlCollection Controls
		{
			get
			{
				base.EnsureChildControls();
				return base.Controls;
			}
		}

		public RegionSelector()
		{
			this.ProvinceTitle = "省：";
			this.CityTitle = "市：";
			this.CountyTitle = "区/县：";
			this.NullToDisplay = "-请选择-";
			this.Separator = "，";
		}

		public int? GetSelectedRegionId()
		{
			if (!string.IsNullOrEmpty(this.Context.Request.Form["regionSelectorValue"]))
			{
				return new int?(int.Parse(this.Context.Request.Form["regionSelectorValue"]));
			}
			return null;
		}

		public void SetSelectedRegionId(int? selectedRegionId)
		{
			this.currentRegionId = selectedRegionId;
			this.dataLoaded = true;
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (!this.dataLoaded)
			{
				if (!string.IsNullOrEmpty(this.Context.Request.Form["regionSelectorValue"]))
				{
					this.currentRegionId = new int?(int.Parse(this.Context.Request.Form["regionSelectorValue"]));
				}
				this.dataLoaded = true;
			}
			if (this.currentRegionId.HasValue)
			{
				XmlNode region = RegionHelper.GetRegion(this.currentRegionId.Value);
				if (region != null)
				{
					if (region.Name == "county")
					{
						this.countyId = new int?(this.currentRegionId.Value);
						this.cityId = new int?(int.Parse(region.ParentNode.Attributes["id"].Value));
						this.provinceId = new int?(int.Parse(region.ParentNode.ParentNode.Attributes["id"].Value));
					}
					else if (region.Name == "city")
					{
						this.cityId = new int?(this.currentRegionId.Value);
						this.provinceId = new int?(int.Parse(region.ParentNode.Attributes["id"].Value));
					}
					else if (region.Name == "province")
					{
						this.provinceId = new int?(this.currentRegionId.Value);
					}
				}
			}
			this.ddlProvinces = this.CreateDropDownList("ddlRegions1", "-请选择省-");
			RegionSelector.FillDropDownList(this.ddlProvinces, RegionHelper.GetAllProvinces(), this.provinceId);
			this.Controls.Add(RegionSelector.CreateTag("<span>"));
			this.Controls.Add(this.ddlProvinces);
			this.Controls.Add(RegionSelector.CreateTag("</span>"));
			this.ddlCitys = this.CreateDropDownList("ddlRegions2", "-请选择市-");
			if (this.provinceId.HasValue)
			{
				RegionSelector.FillDropDownList(this.ddlCitys, RegionHelper.GetCitys(this.provinceId.Value), this.cityId);
			}
			this.Controls.Add(RegionSelector.CreateTag("<span>"));
			this.Controls.Add(this.ddlCitys);
			this.Controls.Add(RegionSelector.CreateTag("</span>"));
			this.ddlCountys = this.CreateDropDownList("ddlRegions3", "-请选择区-");
			if (this.cityId.HasValue)
			{
				RegionSelector.FillDropDownList(this.ddlCountys, RegionHelper.GetCountys(this.cityId.Value), this.countyId);
			}
			this.Controls.Add(RegionSelector.CreateTag("<span>"));
			this.Controls.Add(this.ddlCountys);
			this.Controls.Add(RegionSelector.CreateTag("</span>"));
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
			writer.AddAttribute("id", "regionSelectorValue");
			writer.AddAttribute("name", "regionSelectorValue");
			writer.AddAttribute("value", this.currentRegionId.HasValue ? this.currentRegionId.Value.ToString(CultureInfo.InvariantCulture) : "");
			writer.AddAttribute("type", "hidden");
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
			if (!this.Page.ClientScript.IsStartupScriptRegistered(base.GetType(), "RegionSelectScript"))
			{
				string script = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.region.helper.js"));
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "RegionSelectScript", script, false);
			}
		}

		private static void FillDropDownList(WebControl ddlRegions, Dictionary<int, string> regions, int? selectedId)
		{
			foreach (int current in regions.Keys)
			{
				WebControl webControl = RegionSelector.CreateOption(current.ToString(CultureInfo.InvariantCulture), regions[current]);
				if (selectedId.HasValue && current == selectedId.Value)
				{
					webControl.Attributes.Add("selected", "true");
				}
				ddlRegions.Controls.Add(webControl);
			}
		}

		private WebControl CreateDropDownList(string controlId, string nullText)
		{
			WebControl webControl = new WebControl(HtmlTextWriterTag.Select);
			webControl.Attributes.Add("id", controlId);
			webControl.Attributes.Add("name", controlId);
			webControl.Attributes.Add("selectset", "regions");
			WebControl webControl2 = new WebControl(HtmlTextWriterTag.Option);
			webControl2.Controls.Add(new LiteralControl(nullText));
			webControl2.Attributes.Add("value", "");
			webControl.Controls.Add(webControl2);
			return webControl;
		}

		private static WebControl CreateOption(string val, string text)
		{
			WebControl webControl = new WebControl(HtmlTextWriterTag.Option);
			webControl.Attributes.Add("value", val);
			webControl.Controls.Add(new LiteralControl(text.Trim()));
			return webControl;
		}

		private static Literal CreateTag(string tagName)
		{
			return new Literal
			{
				Text = tagName
			};
		}
	}
}
