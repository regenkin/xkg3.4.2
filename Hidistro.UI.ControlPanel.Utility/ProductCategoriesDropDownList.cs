using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class ProductCategoriesDropDownList : DropDownList
	{
		private string m_NullToDisplay = "";

		private bool m_AutoDataBind = false;

		private string strDepth = "\u3000";

		private bool isTopCategory = false;

		public string NullToDisplay
		{
			get
			{
				return this.m_NullToDisplay;
			}
			set
			{
				this.m_NullToDisplay = value;
			}
		}

		public bool AutoDataBind
		{
			get
			{
				return this.m_AutoDataBind;
			}
			set
			{
				this.m_AutoDataBind = value;
			}
		}

		public bool IsTopCategory
		{
			get
			{
				return this.isTopCategory;
			}
			set
			{
				this.isTopCategory = value;
			}
		}

		public new int? SelectedValue
		{
			get
			{
				int? result;
				if (!string.IsNullOrEmpty(base.SelectedValue))
				{
					result = new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
				}
				else
				{
					result = null;
				}
				return result;
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
				else
				{
					base.SelectedIndex = -1;
				}
			}
		}

		public bool IsUnclassified
		{
			get;
			set;
		}

		protected override void OnLoad(EventArgs e)
		{
			if (this.AutoDataBind && !this.Page.IsPostBack)
			{
				this.DataBind();
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			if (this.IsUnclassified)
			{
				this.Items.Add(new ListItem("未分类商品", "0"));
			}
			if (this.IsTopCategory)
			{
				IList<CategoryInfo> list = CatalogHelper.GetMainCategories();
				foreach (CategoryInfo current in list)
				{
					this.Items.Add(new ListItem(Globals.HtmlDecode(current.Name), current.CategoryId.ToString()));
				}
			}
			else
			{
				IList<CategoryInfo> list = CatalogHelper.GetSequenceCategories();
				for (int i = 0; i < list.Count; i++)
				{
					this.Items.Add(new ListItem(this.FormatDepth(list[i].Depth, Globals.HtmlDecode(list[i].Name)), list[i].CategoryId.ToString(CultureInfo.InvariantCulture)));
				}
			}
		}

		private string FormatDepth(int depth, string categoryName)
		{
			for (int i = 1; i < depth; i++)
			{
				categoryName = this.strDepth + categoryName;
			}
			return categoryName;
		}
	}
}
