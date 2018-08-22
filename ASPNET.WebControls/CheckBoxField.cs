using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNET.WebControls
{
	public class CheckBoxField : BoundField
	{
		public const string DataCellCheckBoxID = "checkboxCol";

		public const string HeaderCheckBoxID = "checkboxHead";

		private string text = "选择";

		private int headWidth = 30;

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
			{
				Label label = new Label();
				label.Text = this.text;
				label.ID = "label";
				cell.Controls.Add(label);
				cell.Width = Unit.Pixel(this.HeadWidth);
				break;
			}
			case DataControlCellType.Footer:
				break;
			case DataControlCellType.DataCell:
				checkBox.ID = "checkboxCol";
				cell.Controls.Add(checkBox);
				cell.Width = Unit.Pixel(5);
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
			string checkHeadReverseScript = CheckBoxField.GetCheckHeadReverseScript();
			string clickCheckScript = CheckBoxField.GetClickCheckScript();
			if (!clientScript.IsClientScriptBlockRegistered("clientCheckHeadReverse"))
			{
				clientScript.RegisterClientScriptBlock(pg.GetType(), "clientCheckHeadReverse", checkHeadReverseScript.Replace("[frmID]", formID));
			}
			if (!clientScript.IsClientScriptBlockRegistered("clickCheckScript"))
			{
				clientScript.RegisterClientScriptBlock(pg.GetType(), "clickCheckScript", clickCheckScript.Replace("[frmID]", formID));
			}
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
			str += "  }";
			str += " }";
			return str + "  </script>";
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
