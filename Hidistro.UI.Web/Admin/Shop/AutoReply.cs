using ASPNET.WebControls;
using ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Weibo;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class AutoReply : AdminPage
	{
		protected bool _enable;

		protected System.Web.UI.WebControls.Repeater repreplykey;

		protected Pager pager;

		protected System.Web.UI.HtmlControls.HtmlInputText txtkey;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidid;

		protected System.Web.UI.WebControls.Button BtnKey;

		protected System.Web.UI.HtmlControls.HtmlInputRadioButton Matching1;

		protected System.Web.UI.HtmlControls.HtmlInputRadioButton Matching2;

		protected AutoReply() : base("m07", "wbp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.BtnKey.Click += new System.EventHandler(this.BtnKey_Click);
			this.repreplykey.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.repreplykey_ItemDataBound);
			this.repreplykey.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.repreplykey_ItemCommand);
			if (!base.IsPostBack)
			{
				this.bind();
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this._enable = masterSettings.CustomReply;
			}
		}

		private void repreplykey_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("repreply");
				int replyKeyId = (int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "id");
				repeater.DataSource = WeiboHelper.GetReplyInfo(replyKeyId);
				repeater.DataBind();
			}
		}

		private void repreplykey_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "del")
			{
				System.Collections.Generic.IList<ReplyInfo> replyInfo = WeiboHelper.GetReplyInfo(int.Parse(e.CommandArgument.ToString()));
				if (replyInfo.Count > 0)
				{
					this.ShowMsg("关键词中有回复信息，请先删除回复信息！", false);
					return;
				}
				if (WeiboHelper.DeleteReplyKeyInfo(int.Parse(e.CommandArgument.ToString())))
				{
					this.ShowMsg("关键词删除成功！", true);
					this.bind();
					return;
				}
				this.ShowMsg("关键词删除失败！", false);
			}
		}

		private void BtnKey_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtkey.Value.Trim()))
			{
				this.ShowMsg("关键词不能为空！", false);
				return;
			}
			ReplyKeyInfo replyKeyInfo = new ReplyKeyInfo();
			replyKeyInfo.Keys = this.txtkey.Value;
			replyKeyInfo.Type = 1;
			if (!string.IsNullOrEmpty(this.hidid.Value))
			{
				replyKeyInfo.Id = int.Parse(this.hidid.Value);
				if (WeiboHelper.UpdateReplyKeyInfo(replyKeyInfo))
				{
					this.ShowMsg("关键词修改成功！", true);
					this.bind();
					return;
				}
				this.ShowMsg("关键词修改失败！", false);
				return;
			}
			else
			{
				if (WeiboHelper.SaveReplyKeyInfo(replyKeyInfo))
				{
					this.ShowMsg("关键词添加成功！", true);
					this.bind();
					return;
				}
				this.ShowMsg("关键词添加失败！", false);
				return;
			}
		}

		public void bind()
		{
			this.repreplykey.DataSource = WeiboHelper.GetTopReplyInfos(1);
			this.repreplykey.DataBind();
		}
	}
}
