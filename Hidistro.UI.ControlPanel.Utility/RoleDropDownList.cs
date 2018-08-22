using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class RoleDropDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "";

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

		public new int SelectedValue
		{
			get
			{
				int result = 0;
				int.TryParse(base.SelectedValue, out result);
				return result;
			}
			set
			{
				base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString()));
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			IList<RoleInfo> roles = ManagerHelper.GetRoles();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			if (roles != null && roles.Count > 0)
			{
				foreach (RoleInfo current in roles)
				{
					base.Items.Add(new ListItem(current.RoleName, current.RoleId.ToString()));
				}
			}
		}
	}
}
