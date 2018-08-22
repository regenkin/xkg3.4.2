using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Entities.Members;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class MemberGradeDropDownList : DropDownList
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

		public new int? SelectedValue
		{
			get
			{
				int? result;
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					result = null;
				}
				else
				{
					result = new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
				}
				return result;
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
			foreach (MemberGradeInfo current in memberGrades)
			{
				this.Items.Add(new ListItem(Globals.HtmlDecode(current.Name), current.GradeId.ToString()));
			}
		}
	}
}
