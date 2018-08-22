using Hidistro.Entities.Sales;
using Hidistro.Vshop;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class ExpressCheckBoxList : CheckBoxList
	{
		private IList<string> expressCompany;

		public IList<string> ExpressCompany
		{
			get
			{
				IList<string> result;
				if (this.expressCompany == null)
				{
					result = new List<string>();
				}
				else
				{
					result = this.expressCompany;
				}
				return result;
			}
			set
			{
				this.expressCompany = value;
			}
		}

		public void BindExpressCheckBoxList()
		{
			base.Items.Clear();
			IList<ExpressCompanyInfo> allExpress = ExpressHelper.GetAllExpress();
			foreach (ExpressCompanyInfo current in allExpress)
			{
				ListItem listItem = new ListItem(current.Name, current.Name);
				if (this.ExpressCompany != null)
				{
					foreach (string current2 in this.ExpressCompany)
					{
						if (string.Compare(listItem.Value, current2, false) == 0)
						{
							listItem.Selected = true;
						}
					}
				}
				base.Items.Add(listItem);
			}
		}

		public override void DataBind()
		{
			this.BindExpressCheckBoxList();
			base.DataBind();
		}
	}
}
