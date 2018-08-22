using Hidistro.ControlPanel.Settings;
using Hidistro.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class FreightTemplateDownList : DropDownList
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

		public new int SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return 0;
				}
				return int.Parse(base.SelectedValue);
			}
			set
			{
				if (value > 0)
				{
					base.SelectedValue = value.ToString();
					return;
				}
				base.SelectedValue = string.Empty;
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			IList<FreightTemplate> freightTemplates = SettingsHelper.GetFreightTemplates();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, "0"));
			}
			foreach (FreightTemplate current in freightTemplates)
			{
				base.Items.Add(new ListItem(current.Name, current.TemplateId.ToString()));
			}
		}
	}
}
