using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Goods.ascx;
using System;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.AddProductType)]
	public class AddAttribute : AdminPage
	{
		protected AttributeView attributeView;

		protected AddAttribute() : base("m02", "spp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
	}
}
