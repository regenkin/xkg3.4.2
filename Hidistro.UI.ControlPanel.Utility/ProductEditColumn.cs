using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class ProductEditColumn : DataControlField
	{
		public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
		{
			base.InitializeCell(cell, cellType, rowState, rowIndex);
			if (null == cell)
			{
				throw new ArgumentNullException("cell");
			}
			if (cellType == DataControlCellType.DataCell)
			{
				cell.DataBinding += new EventHandler(this.cell_DataBinding);
			}
		}

		private void cell_DataBinding(object sender, EventArgs e)
		{
			TableCell tableCell = (TableCell)sender;
			GridViewRow gridViewRow = (GridViewRow)tableCell.NamingContainer;
			try
			{
				int num = Convert.ToInt32(DataBinder.Eval(gridViewRow.DataItem, "ProductId"));
				string adminAbsolutePath = Globals.GetAdminAbsolutePath(string.Format("/product/EditProduct.aspx?productId={0}", num));
				tableCell.Text = string.Format("<a href=\"{0}\"><img border=\"0\" src=\"{1}/admin/images/inout.gif\" alt=\"{2}\" /></a>", adminAbsolutePath, Globals.ApplicationPath, "编辑");
			}
			catch
			{
				throw new Exception("Specified DataField was not found.");
			}
		}

		protected override DataControlField CreateField()
		{
			return new ProductEditColumn();
		}
	}
}
