using Hidistro.Vshop;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ExpressRadioButtonList : RadioButtonList
	{
		public string Name
		{
			get;
			set;
		}

		public IList<string> ExpressCompanies
		{
			get;
			set;
		}

		public override void DataBind()
		{
			IList<string> list = this.ExpressCompanies;
			if (list == null || list.Count == 0)
			{
				list = ExpressHelper.GetAllExpressName();
			}
			base.Items.Clear();
			foreach (string current in list)
			{
				ListItem listItem = new ListItem(current, current);
				if (string.Compare(listItem.Value, this.Name, false) == 0)
				{
					listItem.Selected = true;
				}
				base.Items.Add(listItem);
			}
		}
	}
}
