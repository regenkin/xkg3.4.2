using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ASPNET.WebControls
{
	[ToolboxData("<{0}:GroupRadioButton runat=server></{0}:GroupRadioButton>")]
	public class GroupRadioButton : CompositeControl
	{
		private HtmlInputHidden txtChecked;

		private Control container;

		public bool Checked
		{
			get
			{
				return !string.IsNullOrEmpty(this.txtChecked.Value) && bool.Parse(this.txtChecked.Value);
			}
			set
			{
				this.ViewState["Checked"] = value;
				this.RecreateChildControls();
			}
		}

		public string GroupName
		{
			get
			{
				if (this.ViewState["GroupName"] == null)
				{
					return "GroupRadioButton";
				}
				return (string)this.ViewState["GroupName"];
			}
			set
			{
				this.ViewState["GroupName"] = value;
			}
		}

		private static string Script
		{
			get
			{
				return "\r\n<script type=\"text/javascript\">\r\nfunction ResetGroup(root, current)\r\n{\r\n    var groups = document.getElementById(root).getElementsByTagName(\"input\");\r\n\r\n    for(i=0;i<groups.length;i++)\r\n    {\r\n        var inputElement = groups[i];\r\n        if (inputElement.type.toLowerCase() != \"hidden\" || inputElement.id.indexOf(\"_txtChecked\") < 0)\r\n            continue;\r\n\r\n        inputElement.value = \"false\";\r\n    }\r\n    document.getElementById(current).value =\"true\";\r\n}\r\n</script>\r\n";
			}
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			this.container = this.NamingContainer;
			while (this.container != null && !this.IsDataBindControl(this.container))
			{
				this.container = this.container.Parent;
			}
			this.txtChecked = new HtmlInputHidden();
			this.txtChecked.ID = this.NamingContainer.ClientID + "_txtChecked";
			if (this.ViewState["Checked"] != null && (bool)this.ViewState["Checked"])
			{
				this.txtChecked.Value = "true";
			}
			else
			{
				this.txtChecked.Value = "false";
			}
			this.Controls.Add(this.txtChecked);
		}

		private bool IsDataBindControl(Control control)
		{
			return control is GridView || control is Repeater || control is DataList || control is DataGrid;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			this.txtChecked.RenderControl(writer);
			this.RenderInputTag(writer);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			ClientScriptManager clientScript = this.Page.ClientScript;
			if (!clientScript.IsClientScriptBlockRegistered("ResetGroupScript"))
			{
				clientScript.RegisterClientScriptBlock(this.Page.GetType(), "ResetGroupScript", GroupRadioButton.Script);
			}
		}

		private void RenderInputTag(HtmlTextWriter htw)
		{
			htw.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
			htw.AddAttribute(HtmlTextWriterAttribute.Name, this.container.ClientID + "_" + this.GroupName);
			if (this.ViewState["Checked"] != null && (bool)this.ViewState["Checked"])
			{
				htw.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
			}
			htw.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Concat(new string[]
			{
				"ResetGroup('",
				this.container.ClientID,
				"', '",
				this.txtChecked.ClientID,
				"');"
			}));
			htw.RenderBeginTag(HtmlTextWriterTag.Input);
			htw.RenderEndTag();
		}
	}
}
