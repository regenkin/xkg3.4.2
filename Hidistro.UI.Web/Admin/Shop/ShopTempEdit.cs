using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class ShopTempEdit : AdminPage
	{
		public string tempName;

		public string scriptSrc = "/Templates/vshop/";

		public string cssSrc = "/Templates/vshop/";

		public bool isModuleEdit;

		protected System.Web.UI.HtmlControls.HtmlInputHidden j_pageID;

		protected System.Web.UI.WebControls.Literal La_script;

		public ShopTempEdit() : base("m01", "dpp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.tempName = base.Request.QueryString["tempName"];
				if (string.IsNullOrEmpty(this.tempName))
				{
					this.tempName = "t1";
				}
				this.j_pageID.Value = this.tempName;
				this.La_script.Text = this.GetTemplatescript(this.tempName);
				this.isModuleEdit = this.GetIsModuleEdit(this.tempName);
				this.cssSrc = this.cssSrc + this.tempName + "/css/head.css";
				this.scriptSrc = this.scriptSrc + this.tempName + "/script/head.js";
			}
		}

		public bool GetIsModuleEdit(string tempName)
		{
			string filename = base.Server.MapPath(Globals.ApplicationPath + "/Templates/vshop/" + tempName + "/template.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			return System.Convert.ToBoolean(xmlDocument.SelectSingleNode("root/IsModuleEdit").InnerText);
		}

		public string GetTemplatescript(string tempName)
		{
			string path = base.Server.MapPath("/Templates/vshop/ti/data/default.json");
			if (!string.IsNullOrEmpty(tempName))
			{
				path = base.Server.MapPath("/Templates/vshop/" + tempName + "/script/default.json");
			}
			System.IO.StreamReader streamReader = new System.IO.StreamReader(path, System.Text.Encoding.UTF8);
			string result;
			try
			{
				string text = streamReader.ReadToEnd();
				streamReader.Close();
				result = text;
			}
			catch
			{
				result = "";
			}
			return result;
		}
	}
}
