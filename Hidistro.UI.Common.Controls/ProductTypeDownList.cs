using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ProductTypeDownList : DropDownList
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
				if (value.HasValue && value > 0)
				{
					base.SelectedValue = value.Value.ToString();
					return;
				}
				base.SelectedValue = string.Empty;
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			IList<ProductTypeInfo> productTypes = ProductTypeHelper.GetProductTypes();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			foreach (ProductTypeInfo current in productTypes)
			{
				base.Items.Add(new ListItem(current.TypeName, current.TypeId.ToString()));
			}
		}
	}
}
