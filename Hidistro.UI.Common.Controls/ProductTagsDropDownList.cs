using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ProductTagsDropDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "全部";

		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}

		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
			}
		}

		public new int? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return new int?(int.Parse(base.SelectedValue));
			}
			set
			{
				if (!value.HasValue)
				{
					base.SelectedValue = string.Empty;
					return;
				}
				base.SelectedValue = value.ToString();
			}
		}

		public override void DataBind()
		{
			base.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			DataTable tags = CatalogHelper.GetTags();
			foreach (DataRow dataRow in tags.Rows)
			{
				ListItem item = new ListItem(Globals.HtmlDecode(dataRow["TagName"].ToString()), dataRow["TagID"].ToString());
				base.Items.Add(item);
			}
		}
	}
}
