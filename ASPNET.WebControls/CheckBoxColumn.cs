using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNET.WebControls
{
	public class CheckBoxColumn : BoundField
	{
		public const string DataCellCheckBoxID = "checkboxCol";

		public const string HeaderCheckBoxID = "checkboxHead";

		private bool visible;

		private string text = "选择";

		private int headWidth = 30;

		private int cellWidth = 5;

		public int HeadWidth
		{
			get
			{
				return this.headWidth;
			}
			set
			{
				this.headWidth = value;
			}
		}

		public int CellWidth
		{
			get
			{
				return this.cellWidth;
			}
			set
			{
				this.cellWidth = value;
			}
		}

		public bool ShowHead
		{
			get
			{
				return this.visible;
			}
			set
			{
				this.visible = value;
			}
		}

		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
		{
			base.InitializeCell(cell, cellType, rowState, rowIndex);
			if (cell == null)
			{
				throw new ArgumentNullException("cell");
			}
			CheckBox checkBox = new CheckBox();
			switch (cellType)
			{
			case DataControlCellType.Header:
				if (this.ShowHead)
				{
					checkBox.Text = this.Text;
					checkBox.ID = "checkboxHead";
					cell.Controls.Add(checkBox);
				}
				else
				{
					Label label = new Label();
					label.Text = this.Text;
					label.ID = "label";
					cell.Controls.Add(label);
				}
				cell.Width = Unit.Pixel(this.HeadWidth);
				break;
			case DataControlCellType.Footer:
				break;
			case DataControlCellType.DataCell:
				checkBox.ID = "checkboxCol";
				cell.Controls.Add(checkBox);
				cell.Width = Unit.Pixel(this.CellWidth);
				return;
			default:
				return;
			}
		}

		public static void RegisterClientCheckEvents(Page pg, string formID)
		{
			if (pg == null)
			{
				throw new ArgumentNullException("pg");
			}
			ClientScriptManager clientScript = pg.ClientScript;
			string checkHeadScript = CheckBoxColumn.GetCheckHeadScript();
			string checkHeadReverseScript = CheckBoxColumn.GetCheckHeadReverseScript();
			string clickCheckScript = CheckBoxColumn.GetClickCheckScript();
			if (!clientScript.IsClientScriptBlockRegistered("clientScriptCheckAll"))
			{
				clientScript.RegisterClientScriptBlock(pg.GetType(), "clientScriptCheckAll", checkHeadScript.Replace("[frmID]", formID));
			}
			if (!clientScript.IsClientScriptBlockRegistered("clientReverseScript"))
			{
				clientScript.RegisterClientScriptBlock(pg.GetType(), "clientScriptCheckReverse", checkHeadReverseScript.Replace("[frmID]", formID));
			}
			if (!clientScript.IsClientScriptBlockRegistered("clientCheckClickScript"))
			{
				clientScript.RegisterClientScriptBlock(pg.GetType(), "clientCheckClickScript", clickCheckScript.Replace("[frmID]", formID));
			}
			CheckBoxColumn.RegisterAttributes(pg);
		}

		private static void RegisterAttributes(Control ctrl)
		{
			foreach (Control control in ctrl.Controls)
			{
				try
				{
					if (control.HasControls())
					{
						CheckBoxColumn.RegisterAttributes(control);
					}
					CheckBox checkBox = (CheckBox)control;
					if (checkBox != null && checkBox.ID == "checkboxCol")
					{
						checkBox.Attributes.Add("onclick", "CheckChanged()");
					}
					else if (checkBox != null && checkBox.ID == "checkboxHead")
					{
						checkBox.Attributes.Add("onclick", "CheckAll(this)");
					}
				}
				catch (InvalidCastException)
				{
				}
			}
		}

		private static string GetCheckColScript()
		{
			string str = " <script language=JavaScript>";
			str += " function CheckAll( checkAllBox )";
			str += " {";
			str += "   var frm = document.[frmID];";
			str += "   var ChkState=checkAllBox.checked;";
			str += "   for(i=0;i< frm.length;i++)";
			str += "    {";
			str += "         e=frm.elements[i];";
			str += "        if(e.type=='checkbox' && e.name.indexOf('checkboxCol') != -1)";
			str += "         {";
			str += "            e.checked= ChkState ;";
			str += "          if( ChkState == true)";
			str += "           {";
			str += "             b = e.parentNode.parentNode;";
			str += "             b.style.background = '#FBFBF4';";
			str += "           }";
			str += "          else ";
			str += "           {";
			str += "             b = e.parentNode.parentNode;";
			str += "             b.style.background = '#ffffff';";
			str += "           }";
			str += "        }";
			str += "    }";
			str += " }";
			return str + "  </script>";
		}

		private static string GetClickCheckScript()
		{
			string str = " <script language=JavaScript>";
			str += " function CheckClickAll()";
			str += " {";
			str += " var frm = document.[frmID];";
			str += "  for(i=0;i< frm.length;i++)";
			str += "  {";
			str += "         e=frm.elements[i];";
			str += "        if(e.type=='checkbox' && e.name.indexOf('checkboxCol') != -1)";
			str += "           {";
			str += "            e.checked= true ;";
			str += "             b = e.parentNode.parentNode;";
			str += "             b.style.background = '#FBFBF4';";
			str += "            }";
			str += "        if(e.type=='checkbox' && e.name.indexOf('checkboxHead') != -1)";
			str += "            e.checked= true ;";
			str += "  }";
			str += " }";
			return str + "  </script>";
		}

		private static string GetCheckHeadScript()
		{
			string str = "<script language=JavaScript>";
			str += "function CheckChanged()";
			str += "{";
			str += "  var frm = document.[frmID];";
			str += "  var boolAllChecked;";
			str += "  boolAllChecked=true;";
			str += "  for(i=0;i< frm.length;i++)";
			str += "  {";
			str += "    e=frm.elements[i];";
			str += "  if ( e.type=='checkbox' && e.name.indexOf('checkboxCol') != -1 )";
			str += "         if( e.checked == true)";
			str += "           {";
			str += "             b = e.parentNode.parentNode;";
			str += "             b.style.background = '#FBFBF4';";
			str += "           }";
			str += "          else ";
			str += "           {";
			str += "             b = e.parentNode.parentNode;";
			str += "             b.style.background = '#ffffff';";
			str += "           }";
			str += "  }";
			str += " }";
			return str + " </script>";
		}

		private static string GetCheckHeadReverseScript()
		{
			string str = " <script language=JavaScript>";
			str += " function CheckReverse()";
			str += " {";
			str += " var frm = document.[frmID];";
			str += "  var boolAllChecked;";
			str += "  boolAllChecked=true;";
			str += "  for(i=0;i< frm.length;i++)";
			str += "  {";
			str += "         e=frm.elements[i];";
			str += "     if(e.type=='checkbox' && e.name.indexOf('checkboxCol') != -1)";
			str += "        {";
			str += "         if( e.checked== false)";
			str += "           {";
			str += "              e.checked = true;";
			str += "              b = e.parentNode.parentNode;";
			str += "              b.style.background = '#FBFBF4';";
			str += "           }";
			str += "          else ";
			str += "           {";
			str += "             e.checked = false;";
			str += "             b = e.parentNode.parentNode;";
			str += "             b.style.background = '#ffffff';";
			str += "            }";
			str += "          }";
			str += "    }";
			str += " }";
			return str + "  </script>";
		}
	}
}
