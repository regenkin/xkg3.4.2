using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNET.WebControls
{
	public class YesNoImageColumn : DataControlField
	{
		private string dataField;

		public string CommandName
		{
			get;
			set;
		}

		public string DataField
		{
			get
			{
				if (string.IsNullOrEmpty(this.dataField))
				{
					object obj = base.ViewState["DataField"];
					if (obj != null)
					{
						this.dataField = (string)obj;
					}
					else
					{
						this.dataField = string.Empty;
					}
				}
				return this.dataField;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["DataField"]))
				{
					base.ViewState["DataField"] = value;
					this.dataField = value;
					this.OnFieldChanged();
				}
			}
		}

		public YesNoImageColumn()
		{
			this.CommandName = "SetYesOrNo";
		}

		public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
		{
			base.InitializeCell(cell, cellType, rowState, rowIndex);
			if (cell == null)
			{
				throw new ArgumentNullException("cell");
			}
			if (cellType == DataControlCellType.DataCell)
			{
				cell.DataBinding += new EventHandler(this.cell_DataBinding);
				ImageButton imageButton = new ImageButton();
				imageButton.BorderWidth = Unit.Pixel(0);
				imageButton.CommandName = this.CommandName;
				cell.Controls.Add(imageButton);
			}
		}

		private void cell_DataBinding(object sender, EventArgs e)
		{
			TableCell tableCell = (TableCell)sender;
			if (tableCell.Controls.Count == 0)
			{
				return;
			}
			string webResourceUrl = base.Control.Page.ClientScript.GetWebResourceUrl(base.GetType(), "ASPNET.WebControls.Grid.Images.false.gif");
			string webResourceUrl2 = base.Control.Page.ClientScript.GetWebResourceUrl(base.GetType(), "ASPNET.WebControls.Grid.Images.true.gif");
			GridViewRow gridViewRow = (GridViewRow)tableCell.NamingContainer;
			ImageButton imageButton = (ImageButton)tableCell.Controls[0];
			try
			{
				imageButton.ImageUrl = (Convert.ToBoolean(DataBinder.Eval(gridViewRow.DataItem, this.DataField)) ? webResourceUrl2 : webResourceUrl);
			}
			catch
			{
				throw new Exception("Specified DataField was not found.");
			}
		}

		protected override DataControlField CreateField()
		{
			return new YesNoImageColumn();
		}
	}
}
