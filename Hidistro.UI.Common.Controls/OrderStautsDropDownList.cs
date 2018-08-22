using Hidistro.Entities.Orders;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderStautsDropDownList : DropDownList
	{
		private bool allowNull = true;

		public new OrderStatus SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return OrderStatus.All;
				}
				return (OrderStatus)int.Parse(base.SelectedValue);
			}
			set
			{
				ListItemCollection arg_20_0 = base.Items;
				ListItemCollection arg_1B_0 = base.Items;
				int num = (int)value;
				base.SelectedIndex = arg_20_0.IndexOf(arg_1B_0.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
			}
		}

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

		public OrderStautsDropDownList()
		{
			base.Items.Clear();
			base.Items.Add(new ListItem("所有订单", 0.ToString(CultureInfo.InvariantCulture)));
			base.Items.Add(new ListItem("等待买家付款", 1.ToString(CultureInfo.InvariantCulture)));
			base.Items.Add(new ListItem("等待发货", 2.ToString(CultureInfo.InvariantCulture)));
			base.Items.Add(new ListItem("已发货", 3.ToString(CultureInfo.InvariantCulture)));
			base.Items.Add(new ListItem("已关闭", 4.ToString(CultureInfo.InvariantCulture)));
			base.Items.Add(new ListItem("成功订单", 5.ToString(CultureInfo.InvariantCulture)));
			base.Items.Add(new ListItem("历史订单", 99.ToString(CultureInfo.InvariantCulture)));
		}
	}
}
