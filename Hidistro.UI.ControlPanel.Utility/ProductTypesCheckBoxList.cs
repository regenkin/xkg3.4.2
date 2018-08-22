using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class ProductTypesCheckBoxList : CheckBoxList
	{
		private RepeatDirection repeatDirection = RepeatDirection.Horizontal;

		private int repeatColumns = 7;

		public override RepeatDirection RepeatDirection
		{
			get
			{
				return this.repeatDirection;
			}
			set
			{
				this.repeatDirection = value;
			}
		}

		public override int RepeatColumns
		{
			get
			{
				return this.repeatColumns;
			}
			set
			{
				this.repeatColumns = value;
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			IList<ProductTypeInfo> productTypes = ProductTypeHelper.GetProductTypes();
			foreach (ProductTypeInfo current in productTypes)
			{
				base.Items.Add(new ListItem(current.TypeName, current.TypeId.ToString()));
			}
		}
	}
}
