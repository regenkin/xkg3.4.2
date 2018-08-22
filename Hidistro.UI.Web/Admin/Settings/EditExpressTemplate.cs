using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.Vshop;
using System;
using System.Data;
using System.Text;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class EditExpressTemplate : AdminPage
	{
		protected string ems = "";

		protected string width = "";

		protected string height = "";

		protected EditExpressTemplate() : base("m09", "szp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			int num = 0;
			string text = this.Page.Request.QueryString["ExpressName"];
			string text2 = this.Page.Request.QueryString["XmlFile"];
			if (!int.TryParse(this.Page.Request.QueryString["ExpressId"], out num))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || !text2.EndsWith(".xml"))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!base.IsPostBack)
			{
				System.Data.DataTable expressTable = ExpressHelper.GetExpressTable();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", text2)));
				XmlNode xmlNode = xmlDocument.SelectSingleNode("/printer/size");
				string innerText = xmlNode.InnerText;
				this.width = innerText.Split(new char[]
				{
					':'
				})[0];
				this.height = innerText.Split(new char[]
				{
					':'
				})[1];
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				foreach (System.Data.DataRow dataRow in expressTable.Rows)
				{
					stringBuilder.AppendFormat("<option value='{0}' {1}>{0}</option>", dataRow["Name"], dataRow["Name"].Equals(text) ? "selected" : "");
				}
				this.ems = stringBuilder.ToString();
			}
		}
	}
}
