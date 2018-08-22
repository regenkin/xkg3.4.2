using Hidistro.ControlPanel.Store;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class LogsUserNameDropDownList : DropDownList
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

		public override void DataBind()
		{
			this.Items.Clear();
			IList<string> list = new List<string>();
			list = EventLogs.GetOperationUseNames();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			foreach (string current in list)
			{
				base.Items.Add(new ListItem(current, current));
			}
		}
	}
}
