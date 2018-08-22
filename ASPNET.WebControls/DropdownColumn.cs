using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNET.WebControls
{
	public class DropdownColumn : BoundField
	{
		private string dataField;

		private string dataKey;

		private string dataTextField;

		private string dataValueField;

		private bool allowNull = true;

		private string nullToDisplay = "";

		private bool forEditItem;

		private string clientOnChangeEventScript;

		private string id = "DropdownlistCol";

		private bool enabledSelect = true;

		public object DataSource;

		public string ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		public bool EnabledSelect
		{
			get
			{
				return this.enabledSelect;
			}
			set
			{
				this.enabledSelect = value;
			}
		}

		public string ClientOnChangeEventScript
		{
			get
			{
				return this.clientOnChangeEventScript;
			}
			set
			{
				this.clientOnChangeEventScript = value;
			}
		}

		public string DataKey
		{
			get
			{
				return this.dataKey;
			}
			set
			{
				this.dataKey = value;
			}
		}

		public bool JustForEditItem
		{
			get
			{
				return this.forEditItem;
			}
			set
			{
				this.forEditItem = value;
			}
		}

		public new string DataField
		{
			get
			{
				return this.dataField;
			}
			set
			{
				this.dataField = value;
			}
		}

		public string DataTextField
		{
			get
			{
				return this.dataTextField;
			}
			set
			{
				this.dataTextField = value;
			}
		}

		public string DataValueField
		{
			get
			{
				return this.dataValueField;
			}
			set
			{
				this.dataValueField = value;
			}
		}

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

		public string[] SelectedValues
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				if (this.Owner != null)
				{
					foreach (GridViewRow gridViewRow in this.Owner.Rows)
					{
						DropDownList dropDownList = (DropDownList)gridViewRow.FindControl(this.ID);
						if (dropDownList != null && dropDownList.Items.Count > 0)
						{
							arrayList.Add(dropDownList.SelectedValue);
						}
					}
				}
				return (string[])arrayList.ToArray(typeof(string));
			}
		}

		public GridView Owner
		{
			get
			{
				return (GridView)base.Control;
			}
		}

		public ListItemCollection SelectedItems
		{
			get
			{
				ListItemCollection listItemCollection = new ListItemCollection();
				foreach (GridViewRow gridViewRow in this.Owner.Rows)
				{
					DropDownList dropDownList = (DropDownList)gridViewRow.FindControl(this.ID);
					if (dropDownList != null && dropDownList.Items.Count > 0)
					{
						listItemCollection.Add(dropDownList.SelectedItem);
					}
				}
				return listItemCollection;
			}
		}

		public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
		{
			base.InitializeCell(cell, cellType, rowState, rowIndex);
			if (cellType == DataControlCellType.DataCell)
			{
				if (this.JustForEditItem)
				{
					switch (rowState)
					{
					case DataControlRowState.Normal:
					case DataControlRowState.Alternate:
					case DataControlRowState.Selected:
						cell.DataBinding += new EventHandler(this.ItemDataBinding);
						return;
					case DataControlRowState.Alternate | DataControlRowState.Selected:
						break;
					case DataControlRowState.Edit:
						cell.DataBinding += new EventHandler(this.EditItemDataBinding);
						return;
					default:
						return;
					}
				}
				else
				{
					cell.DataBinding += new EventHandler(this.EditItemDataBinding);
					DropDownList dropDownList = new DropDownList();
					dropDownList.ID = this.ID;
					if (!this.EnabledSelect)
					{
						dropDownList.Enabled = false;
					}
					cell.Controls.Add(dropDownList);
				}
			}
		}

		private void EditItemDataBinding(object sender, EventArgs e)
		{
			DataControlFieldCell dataControlFieldCell = (DataControlFieldCell)sender;
			DropDownList dropDownList = (DropDownList)dataControlFieldCell.Controls[0];
			ListItem listItem = null;
			try
			{
				IEnumerator enumerator = ((IEnumerable)this.DataSource).GetEnumerator();
				while (enumerator.MoveNext())
				{
					string value = Convert.ToString(DataBinder.Eval(enumerator.Current, this.DataValueField));
					string text = Convert.ToString(DataBinder.Eval(enumerator.Current, this.DataTextField));
					dropDownList.Items.Add(new ListItem(text, value));
				}
			}
			catch
			{
				throw new Exception("Specified Field was not found.");
			}
			if (this.AllowNull)
			{
				dropDownList.Items.Insert(0, new ListItem(this.NullToDisplay, string.Empty));
			}
			try
			{
				GridViewRow gridViewRow = (GridViewRow)dataControlFieldCell.NamingContainer;
				listItem = dropDownList.Items.FindByValue(Convert.ToString(DataBinder.Eval(gridViewRow.DataItem, this.DataKey)));
			}
			catch
			{
				throw new Exception("Specified DataField was not found.");
			}
			if (listItem != null)
			{
				listItem.Selected = true;
			}
			if (this.ClientOnChangeEventScript != null)
			{
				dropDownList.Attributes.Add("onchange", this.ClientOnChangeEventScript);
			}
		}

		private void ItemDataBinding(object sender, EventArgs e)
		{
			DataControlFieldCell dataControlFieldCell = (DataControlFieldCell)sender;
			GridViewRow gridViewRow = (GridViewRow)dataControlFieldCell.NamingContainer;
			try
			{
				dataControlFieldCell.Text = Convert.ToString(DataBinder.Eval(gridViewRow.DataItem, this.DataField));
			}
			catch
			{
				throw new Exception("Specified DataField was not found.");
			}
		}
	}
}
