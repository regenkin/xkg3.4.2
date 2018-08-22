using ControlPanel.WeiBo;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Articles : AdminPage
	{
		protected string ArticleTitle = string.Empty;

		protected int articleid;

		protected int articletype;

		protected Articles() : base("m01", "dpp06")
		{
		}

		[PrivilegeCheck(Privilege.Summary)]
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string stype = base.Request.QueryString["articletype"];
			string a = base.Request.Form["posttype"];
			string s = base.Request.Form["id"];
			int.TryParse(s, out this.articleid);
			if (a == "del" && this.articleid > 0)
			{
				base.Response.ContentType = "application/json";
				string s2 = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
				System.Data.DataSet dataSet = ArticleHelper.ArticleIsInWeiXinReply(this.articleid);
				System.Data.DataTable dataTable = dataSet.Tables[0];
				System.Data.DataTable dataTable2 = dataSet.Tables[1];
				System.Data.DataTable dataTable3 = dataSet.Tables[2];
				if (Globals.ToNum(dataTable.Rows[0][0]) > 0)
				{
					s2 = "{\"type\":\"0\",\"tips\":\"删除失败，该素材已在栏目“微信->自动回复”中使用。\"}";
				}
				else if (Globals.ToNum(dataTable2.Rows[0][0]) > 0)
				{
					s2 = "{\"type\":\"0\",\"tips\":\"删除失败，该素材已在栏目“微博->自动回复”中使用。\"}";
				}
				else if (Globals.ToNum(dataTable3.Rows[0][0]) > 0)
				{
					s2 = "{\"type\":\"0\",\"tips\":\"删除失败，该素材已在栏目“服务窗->自动回复”中使用。\"}";
				}
				else if (ArticleHelper.DeleteArticle(this.articleid))
				{
					s2 = "{\"type\":\"1\",\"tips\":\"删除成功\"}";
				}
				base.Response.Write(s2);
				base.Response.End();
				return;
			}
			this.LoadParameters(stype);
		}

		private void LoadParameters(string _stype)
		{
			int.TryParse(_stype, out this.articletype);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["key"]))
			{
				this.ArticleTitle = base.Server.UrlDecode(this.Page.Request.QueryString["key"]);
			}
		}
	}
}
