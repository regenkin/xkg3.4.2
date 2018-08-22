using Hidistro.Entities;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class RegionAllName : Literal
	{
		public string RegionId
		{
			get
			{
				if (this.ViewState["RegionId"] == null)
				{
					return null;
				}
				return (string)this.ViewState["RegionId"];
			}
			set
			{
				this.ViewState["RegionId"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			int num = int.Parse(this.RegionId);
			string text = string.Empty;
			if (num > 0)
			{
				text = RegionHelper.GetFullRegion(num, "  ");
			}
			base.Text = text;
			base.Render(writer);
		}
	}
}
