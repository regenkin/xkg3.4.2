using ASPNET.WebControls;
using ControlPanel.WeiBo;
using Hidistro.Core.Entities;
using Hidistro.Entities.Weibo;
using Hidistro.UI.ControlPanel.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Message : AdminPage
	{
		private string status = "-1";

		private string Reply = "2";

		public string alldata = "class=\"active\"";

		public string nodata = "";

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidstatus;

		protected System.Web.UI.WebControls.CheckBox replycheck;

		protected System.Web.UI.WebControls.Repeater RepMessage;

		protected Pager pager;

		protected Message() : base("m07", "wbp04")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.RepMessage.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.RepMessage_ItemDataBound);
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.bind();
			}
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Status"]))
				{
					this.status = base.Server.UrlDecode(this.Page.Request.QueryString["Status"]);
				}
				if (this.status == "-1")
				{
					this.alldata = "class=\"active\"";
					this.nodata = "";
				}
				else
				{
					this.alldata = "";
					this.nodata = "class=\"active\"";
				}
				this.hidstatus.Value = this.status;
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Reply"]))
				{
					this.Reply = base.Server.UrlDecode(this.Page.Request.QueryString["Reply"]);
					if (this.Reply == "2")
					{
						this.replycheck.Checked = true;
						return;
					}
				}
			}
			else
			{
				this.status = this.hidstatus.Value;
				this.Reply = (this.replycheck.Checked ? "2" : "");
			}
		}

		private void RepMessage_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				string id = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Sender_id").ToString();
				WeiBo weiBo = new WeiBo();
				string json = weiBo.userinfo(id);
				JObject jObject = JObject.Parse(json);
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("LitUserName");
				if (jObject["id"] != null)
				{
					System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)e.Item.FindControl("Pic");
					image.ImageUrl = jObject["profile_image_url"].ToString();
					literal.Text = jObject["screen_name"].ToString();
				}
				if (string.IsNullOrEmpty(literal.Text))
				{
					literal.Text = "您访问太过频繁，微博接口禁止调用。";
				}
			}
		}

		private void ReBind(bool isSearch)
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{
				{
					"Status",
					this.hidstatus.Value
				},
				{
					"Reply",
					this.replycheck.Checked ? "2" : ""
				}
			});
		}

		public void bind()
		{
			MessageQuery messageQuery = new MessageQuery();
			if (!this.replycheck.Checked)
			{
				messageQuery.Status = int.Parse(this.status);
			}
			else
			{
				messageQuery.Status = 2;
			}
			messageQuery.PageSize = this.pager.PageSize;
			messageQuery.PageIndex = this.pager.PageIndex;
			DbQueryResult messages = WeiboHelper.GetMessages(messageQuery);
			this.RepMessage.DataSource = messages.Data;
			this.RepMessage.DataBind();
			this.pager.TotalRecords = messages.TotalRecords;
		}

		protected void CheckBox1_CheckedChanged(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
	}
}
