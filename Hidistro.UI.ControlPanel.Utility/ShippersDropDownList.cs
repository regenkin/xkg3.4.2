using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class ShippersDropDownList : DropDownList
	{
		private bool includeDistributor = false;

		public new int SelectedValue
		{
			get
			{
				int result;
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					result = 0;
				}
				else
				{
					result = int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
				}
				return result;
			}
			set
			{
				base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString(CultureInfo.InvariantCulture)));
			}
		}

		public bool IncludeDistributor
		{
			get
			{
				return this.includeDistributor;
			}
			set
			{
				this.includeDistributor = value;
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			IList<ShippersInfo> shippers = SalesHelper.GetShippers(this.IncludeDistributor);
			foreach (ShippersInfo current in shippers)
			{
				this.Items.Add(new ListItem(current.ShipperName, current.ShipperId.ToString()));
			}
		}
	}
}
