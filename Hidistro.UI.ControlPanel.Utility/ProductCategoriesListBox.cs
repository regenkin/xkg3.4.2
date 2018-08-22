using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class ProductCategoriesListBox : ListBox
	{
		private string strDepth = "\u3000\u3000";

		public int SelectedCategoryId
		{
			get
			{
				int result = 0;
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Selected)
					{
						result = int.Parse(this.Items[i].Value);
					}
				}
				return result;
			}
			set
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Value == value.ToString())
					{
						this.Items[i].Selected = true;
					}
					else
					{
						this.Items[i].Selected = false;
					}
				}
			}
		}

		public new IList<int> SelectedValue
		{
			get
			{
				IList<int> list = new List<int>();
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Selected)
					{
						list.Add(int.Parse(this.Items[i].Value));
					}
				}
				return list;
			}
			set
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					this.Items[i].Selected = false;
				}
				foreach (int current in value)
				{
					for (int i = 0; i < this.Items.Count; i++)
					{
						if (this.Items[i].Value == current.ToString())
						{
							this.Items[i].Selected = true;
						}
					}
				}
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			IList<CategoryInfo> sequenceCategories = CatalogHelper.GetSequenceCategories();
			for (int i = 0; i < sequenceCategories.Count; i++)
			{
				this.Items.Add(new ListItem(this.FormatDepth(sequenceCategories[i].Depth, Globals.HtmlDecode(sequenceCategories[i].Name)), sequenceCategories[i].CategoryId.ToString(CultureInfo.InvariantCulture)));
			}
			ListItem item = new ListItem("--所有--", "0");
			this.Items.Insert(0, item);
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
