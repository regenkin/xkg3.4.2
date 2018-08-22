using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Settings;
using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.Trade
{
	public class ExpressTemplates : AdminPage
	{
		protected Pager pager;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected Grid grdExpressTemplates;

		protected System.Web.UI.WebControls.Button lkbDeleteCheck;

		protected Pager pager1;

		protected ExpressTemplates() : base("m03", "ddp11")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdExpressTemplates.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdExpressTemplates_RowCommand);
			this.lkbDeleteCheck.Click += new System.EventHandler(this.lkbDeleteCheck_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindExpressTemplates();
			}
		}

		private void lkbDeleteCheck_Click(object sender, System.EventArgs e)
		{
			this.DeleteCheck();
		}

		private void DeleteCheck()
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要删除的快递单模板", false);
				return;
			}
			int num = SettingsHelper.DeleteExpressTemplates(text);
			if (num > 0)
			{
				string[] array = text.Split(new char[]
				{
					','
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					for (int j = 0; j < this.grdExpressTemplates.Rows.Count; j++)
					{
						if (this.grdExpressTemplates.DataKeys[j].Value.ToString() == text2.ToString().Trim())
						{
							System.Web.UI.WebControls.Literal literal = this.grdExpressTemplates.Rows[j].FindControl("litXmlFile") as System.Web.UI.WebControls.Literal;
							this.DeleteXmlFile(literal.Text);
							break;
						}
					}
				}
			}
			this.BindExpressTemplates();
			this.ShowMsg(string.Format("成功删除了{0}个快递单模板", num), true);
		}

		private void grdExpressTemplates_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			System.Web.UI.WebControls.GridViewRow gridViewRow = (System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
			int num = (int)this.grdExpressTemplates.DataKeys[gridViewRow.RowIndex].Value;
			if (e.CommandName == "SetYesOrNo")
			{
				SalesHelper.SetExpressIsUse(num);
				this.BindExpressTemplates();
				return;
			}
			if (!(e.CommandName == "DeleteRow"))
			{
				if (e.CommandName == "IsDefault")
				{
					SettingsHelper.SetExpressIsDefault(num);
					this.BindExpressTemplates();
				}
				return;
			}
			if (SalesHelper.DeleteExpressTemplate(num))
			{
				System.Web.UI.WebControls.Literal literal = this.grdExpressTemplates.Rows[gridViewRow.RowIndex].FindControl("litXmlFile") as System.Web.UI.WebControls.Literal;
				this.DeleteXmlFile(literal.Text);
				this.BindExpressTemplates();
				this.ShowMsg("已经成功删除选择的快递单模板", true);
				return;
			}
			this.ShowMsg("删除快递单模板失败", false);
		}

		private void DeleteXmlFile(string xmlfile)
		{
			string text = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", xmlfile));
			if (System.IO.File.Exists(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("printer/pic");
				string path = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", xmlNode.InnerText));
				if (System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);
				}
				System.IO.File.Delete(text);
			}
		}

		private void BindExpressTemplates()
		{
			this.grdExpressTemplates.DataSource = SalesHelper.GetExpressTemplates();
			this.grdExpressTemplates.DataBind();
		}
	}
}
