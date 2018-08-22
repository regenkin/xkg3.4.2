using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VVote : VMemberTemplatedWebControl
	{
		private int voteId;

		private int voteNum = 0;

		private System.Web.UI.WebControls.Literal litVoteName;

		private System.Web.UI.WebControls.Literal litVoteNum;

		private VshopTemplatedRepeater rptVoteItems;

		private System.Web.UI.HtmlControls.HtmlInputHidden hidCheckNum;

		private System.Web.UI.HtmlControls.HtmlGenericControl divVoteOk;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VVote.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["voteId"], out this.voteId))
			{
				base.GotoResourceNotFound("");
			}
			this.litVoteName = (System.Web.UI.WebControls.Literal)this.FindControl("litVoteName");
			this.litVoteNum = (System.Web.UI.WebControls.Literal)this.FindControl("litVoteNum");
			this.rptVoteItems = (VshopTemplatedRepeater)this.FindControl("rptVoteItems");
			this.hidCheckNum = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidCheckNum");
			this.divVoteOk = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("divVoteOk");
			string empty = string.Empty;
			int num = 1;
			DataTable vote = VshopBrowser.GetVote(this.voteId, out empty, out num, out this.voteNum);
			if (vote == null)
			{
				base.GotoResourceNotFound("");
			}
			this.LoadVoteItemTable(vote);
			this.rptVoteItems.DataSource = vote;
			this.rptVoteItems.DataBind();
			this.litVoteName.Text = empty;
			this.hidCheckNum.Value = num.ToString();
			this.litVoteNum.Text = string.Format("共有{0}人参与投票", this.voteNum);
			if (VshopBrowser.IsVote(this.voteId))
			{
				System.Web.UI.WebControls.Literal expr_158 = this.litVoteNum;
				expr_158.Text += "(您已投票)";
				this.divVoteOk.Visible = false;
			}
			PageTitle.AddSiteNameTitle("投票调查");
		}

		private void LoadVoteItemTable(DataTable table)
		{
			table.Columns.Add("Lenth");
			table.Columns.Add("Percentage");
			foreach (DataRow dataRow in table.Rows)
			{
				dataRow["Lenth"] = this.GetVoteItemCountString((int)dataRow["ItemCount"]);
				if (this.voteNum != 0)
				{
					dataRow["Percentage"] = (decimal.Parse(dataRow["ItemCount"].ToString()) * 100m / decimal.Parse(this.voteNum.ToString())).ToString("F2");
				}
				else
				{
					dataRow["Percentage"] = 0.0;
				}
			}
		}

		private string GetVoteItemCountString(int num)
		{
			string text = string.Empty;
			if (this.voteNum != 0)
			{
				int num2 = num * 30 / this.voteNum;
				for (int i = 0; i < num2; i++)
				{
					text += "&nbsp;";
				}
			}
			return text;
		}
	}
}
