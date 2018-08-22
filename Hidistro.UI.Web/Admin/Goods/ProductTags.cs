using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	public class ProductTags : AdminPage
	{
		protected System.Web.UI.WebControls.Repeater rp_prducttag;

		protected System.Web.UI.WebControls.TextBox txttagname;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hdtagId;

		protected System.Web.UI.WebControls.Button btnupdatetag;

		protected System.Web.UI.WebControls.TextBox txtaddtagname;

		protected System.Web.UI.WebControls.Button btnaddtag;

		protected ProductTags() : base("m02", "spp14")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request["isAjax"]) && base.Request["isAjax"] == "true")
			{
				string text = base.Request["Mode"].ToString();
				string text2 = "false";
				string a;
				if ((a = text) != null && a == "Add")
				{
					string text3 = "标签名称不允许为空";
					if (!string.IsNullOrEmpty(base.Request["TagValue"].Trim()))
					{
						text3 = "添加标签名称失败，请确认标签名是否已存在";
						string tagName = Globals.HtmlEncode(base.Request["TagValue"].ToString());
						int num = CatalogHelper.AddTags(tagName);
						if (num > 0)
						{
							text2 = "true";
							text3 = num.ToString();
						}
					}
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					base.Response.Write(string.Concat(new string[]
					{
						"{\"Status\":\"",
						text2,
						"\",\"msg\":\"",
						text3,
						"\"}"
					}));
					base.Response.End();
				}
			}
			this.btnaddtag.Click += new System.EventHandler(this.btnaddtag_Click);
			this.btnupdatetag.Click += new System.EventHandler(this.btnupdatetag_Click);
			this.rp_prducttag.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rp_prducttag_ItemCommand);
			if (!base.IsPostBack)
			{
				this.ProductTagsBind();
			}
		}

		protected void ProductTagsBind()
		{
			this.rp_prducttag.DataSource = CatalogHelper.GetTags();
			this.rp_prducttag.DataBind();
		}

		protected void btnaddtag_Click(object sender, System.EventArgs e)
		{
			string text = Globals.HtmlEncode(this.txtaddtagname.Text.Trim());
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("标签名称不允许为空！", false);
				return;
			}
			if (CatalogHelper.AddTags(text) > 0)
			{
				this.ShowMsg("添加商品标签成功！", true);
				this.ProductTagsBind();
				return;
			}
			this.ShowMsg("添加商品标签失败，请确认是否存在重名标签名称", false);
		}

		protected void btnupdatetag_Click(object sender, System.EventArgs e)
		{
			string value = this.hdtagId.Value.Trim();
			string text = Globals.HtmlEncode(this.txttagname.Text.Trim());
			if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请选择要修改的商品标签或输入商品标签名称", false);
				return;
			}
			if (System.Convert.ToInt32(value) <= 0)
			{
				this.ShowMsg("选择的商品标签有误", false);
				return;
			}
			if (CatalogHelper.UpdateTags(System.Convert.ToInt32(value), text))
			{
				this.ShowMsg("修改商品标签成功", true);
				this.ProductTagsBind();
				return;
			}
			this.ShowMsg("修改商品标签失败，请确认输入的商品标签名称是否存在同名", false);
		}

		protected void rp_prducttag_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName.Equals("delete"))
			{
				string value = e.CommandArgument.ToString();
				if (!string.IsNullOrEmpty(value) && System.Convert.ToInt32(value) > 0)
				{
					if (CatalogHelper.DeleteTags(System.Convert.ToInt32(value)))
					{
						this.ShowMsg("删除商品标签成功", true);
						this.ProductTagsBind();
						return;
					}
					this.ShowMsg("删除商品标签失败", false);
				}
			}
		}
	}
}
