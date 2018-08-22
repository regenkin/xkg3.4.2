using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class MoneyColumnForAdmin : BoundField
	{
		private string remarkText = string.Empty;

		public string NullToDisplay
		{
			get
			{
				if (base.ViewState["NullToDisplay"] == null)
				{
					return "-";
				}
				return (string)base.ViewState["NullToDisplay"];
			}
			set
			{
				base.ViewState["NullToDisplay"] = value;
			}
		}

		public string EditTextBoxId
		{
			get
			{
				if (base.ViewState["EditTextBoxId"] == null)
				{
					return null;
				}
				return (string)base.ViewState["EditTextBoxId"];
			}
			set
			{
				base.ViewState["EditTextBoxId"] = value;
			}
		}

		public bool AllowEdit
		{
			get
			{
				return base.ViewState["AllowEdit"] == null || (bool)base.ViewState["AllowEdit"];
			}
			set
			{
				base.ViewState["AllowEdit"] = value;
			}
		}

		public string RemarkText
		{
			get
			{
				return this.remarkText;
			}
			set
			{
				this.remarkText = value;
			}
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
				if (rowState == DataControlRowState.Edit && this.AllowEdit)
				{
					cell.DataBinding += new EventHandler(this.EditDataBinding);
					return;
				}
				cell.DataBinding += new EventHandler(this.cell_DataBinding);
			}
		}

		private void cell_DataBinding(object sender, EventArgs e)
		{
			TableCell tableCell = (TableCell)sender;
			GridViewRow gridViewRow = (GridViewRow)tableCell.NamingContainer;
			try
			{
				tableCell.Controls.Clear();
				object obj = DataBinder.Eval(gridViewRow.DataItem, this.DataField);
				tableCell.Text = ((obj == null || obj == DBNull.Value) ? this.NullDisplayText : Convert.ToDecimal(obj).ToString("F", CultureInfo.InvariantCulture));
				if (tableCell.Text == "")
				{
					tableCell.Text = "-";
				}
				if (!string.IsNullOrEmpty(this.RemarkText))
				{
					tableCell.Text = this.RemarkText + tableCell.Text;
				}
			}
			catch
			{
				throw new Exception("Specified DataField was not found.");
			}
		}

		private void EditDataBinding(object sender, EventArgs e)
		{
			TableCell tableCell = (TableCell)sender;
			GridViewRow gridViewRow = (GridViewRow)tableCell.NamingContainer;
			try
			{
				tableCell.Controls.Clear();
				object obj = DataBinder.Eval(gridViewRow.DataItem, this.DataField);
				TextBox textBox = new TextBox();
				textBox.ID = this.EditTextBoxId;
				textBox.Width = Unit.Percentage(100.0);
				textBox.Text = ((obj == null || obj == DBNull.Value) ? "" : string.Format("{0:F}", obj));
				tableCell.Controls.Add(textBox);
			}
			catch
			{
				throw new Exception("Specified DataField was not found.");
			}
		}
	}
}
