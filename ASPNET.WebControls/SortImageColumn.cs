using System;
using System.Web.UI.WebControls;

namespace ASPNET.WebControls
{
	public class SortImageColumn : ImageField
	{
		public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
		{
			base.InitializeCell(cell, cellType, rowState, rowIndex);
			if (cell == null)
			{
				throw new ArgumentNullException("cell");
			}
			string webResourceUrl = base.Control.Page.ClientScript.GetWebResourceUrl(base.GetType(), "ASPNET.WebControls.Grid.Images.up.gif");
			string webResourceUrl2 = base.Control.Page.ClientScript.GetWebResourceUrl(base.GetType(), "ASPNET.WebControls.Grid.Images.down.gif");
			ImageButton imageButton = new ImageButton();
			imageButton.ID = "rise";
			imageButton.ImageUrl = webResourceUrl;
			imageButton.CommandName = "Rise";
			ImageButton imageButton2 = new ImageButton();
			imageButton2.ID = "fall";
			imageButton2.ImageUrl = webResourceUrl2;
			imageButton2.CommandName = "Fall";
			if (cellType != DataControlCellType.DataCell)
			{
				return;
			}
			cell.Controls.Add(imageButton2);
			cell.Controls.Add(imageButton);
		}
	}
}
