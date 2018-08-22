using Hidistro.Entities.Commodities;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SaleStatusDropDownList : DropDownList
	{
		private bool allowNull = true;

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

		public new ProductSaleStatus SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return ProductSaleStatus.All;
				}
				return (ProductSaleStatus)int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
			}
			set
			{
				ListItemCollection arg_20_0 = base.Items;
				ListItemCollection arg_1B_0 = base.Items;
				int num = (int)value;
				base.SelectedIndex = arg_20_0.IndexOf(arg_1B_0.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
			}
		}

		public SaleStatusDropDownList()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(string.Empty, string.Empty));
			}
			this.Items.Add(new ListItem("出售中", "1"));
			this.Items.Add(new ListItem("仓库中", "3"));
		}
	}
}
