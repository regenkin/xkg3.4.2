using Hidistro.ControlPanel.Commodities;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class BrandCategoriesCheckBoxList : CheckBoxList
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
			DataTable brandCategories = CatalogHelper.GetBrandCategories();
			foreach (DataRow dataRow in brandCategories.Rows)
			{
				this.Items.Add(new ListItem((string)dataRow["BrandName"], ((int)dataRow["BrandId"]).ToString(CultureInfo.InvariantCulture)));
			}
		}
	}
}
