using ASPNET.WebControls;
using ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class VoteList : AdminPage
	{
		protected VoteStatus status = VoteStatus.In;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Label lblIn;

		protected System.Web.UI.WebControls.Label lblEnd;

		protected System.Web.UI.WebControls.Label lblUnBegin;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.Button btnSeach;

		protected System.Web.UI.WebControls.Button DelBtn;

		protected System.Web.UI.WebControls.TextBox txt_Ids;

		protected System.Web.UI.WebControls.Repeater grdDate;

		protected Pager pager;

		protected VoteList() : base("m08", "yxp06")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdDate.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.grdDate_ItemCommand);
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("status"))
			{
				int num = 0;
				if (base.Request["status"].ToString().bInt(ref num))
				{
					this.status = (VoteStatus)num;
				}
			}
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void grdDate_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				if (string.IsNullOrEmpty(e.CommandArgument.ToString()))
				{
					return;
				}
				if (VoteHelper.Delete((long)int.Parse(e.CommandArgument.ToString())))
				{
					this.ShowMsg("删除成功！", true);
					this.BindData();
					return;
				}
				this.ShowMsg("删除失败！", false);
			}
		}

		private void BindData()
		{
			string name = this.txt_name.Text.Trim();
			DbQueryResult dbQueryResult = VoteHelper.Query(new VoteSearch
			{
				status = this.status,
				IsCount = true,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "VoteId",
				SortOrder = SortAction.Desc,
				Name = name
			});
			System.Data.DataTable dataTable = (System.Data.DataTable)dbQueryResult.Data;
			if (dataTable != null)
			{
				dataTable.Columns.Add("sStatus");
				dataTable.Columns.Add("sType");
				dataTable.Columns.Add("sAttend");
				string text = "";
				if (dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						System.DateTime t = System.DateTime.Parse(dataRow["startDate"].ToString());
						System.DateTime t2 = System.DateTime.Parse(dataRow["endDate"].ToString());
						bool flag = bool.Parse(dataRow["IsMultiCheck"].ToString());
						int num = int.Parse(dataRow["voteId"].ToString());
						if (t > System.DateTime.Now)
						{
							dataRow["sStatus"] = "未开始";
						}
						else if (t2 < System.DateTime.Now)
						{
							dataRow["sStatus"] = "已结束";
						}
						else
						{
							dataRow["sStatus"] = "进行中";
						}
						if (flag)
						{
							dataRow["sType"] = "多选";
						}
						else
						{
							dataRow["sType"] = "单选";
						}
						dataRow["sAttend"] = VoteHelper.GetVoteAttends((long)num);
						text = text + ", " + num.ToString();
					}
					if (text.Length > 1)
					{
						text = text.Substring(1);
					}
				}
			}
			this.grdDate.DataSource = dataTable;
			this.grdDate.DataBind();
			this.pager.TotalRecords = dbQueryResult.TotalRecords;
			this.CountTotal();
		}

		private void CountTotal()
		{
			VoteSearch voteSearch = new VoteSearch();
			voteSearch.status = this.status;
			voteSearch.IsCount = true;
			voteSearch.PageIndex = this.pager.PageIndex;
			voteSearch.PageSize = this.pager.PageSize;
			voteSearch.SortBy = "VoteId";
			voteSearch.SortOrder = SortAction.Desc;
			voteSearch.status = VoteStatus.In;
			DbQueryResult dbQueryResult = VoteHelper.Query(voteSearch);
			this.lblIn.Text = ((dbQueryResult.Data != null) ? dbQueryResult.TotalRecords.ToString() : "0");
			voteSearch.status = VoteStatus.End;
			dbQueryResult = VoteHelper.Query(voteSearch);
			this.lblEnd.Text = ((dbQueryResult.Data != null) ? dbQueryResult.TotalRecords.ToString() : "0");
			voteSearch.status = VoteStatus.unBegin;
			dbQueryResult = VoteHelper.Query(voteSearch);
			this.lblUnBegin.Text = ((dbQueryResult.Data != null) ? dbQueryResult.TotalRecords.ToString() : "0");
		}

		public string GetUrl(object voteId)
		{
			return string.Concat(new object[]
			{
				"http://",
				Globals.DomainName,
				Globals.ApplicationPath,
				"/Vshop/Vote.aspx?voteId=",
				voteId
			});
		}

		protected void lkStart_Click(object sender, System.EventArgs e)
		{
			string text = this.txt_Ids.Text;
			this.txt_Ids.Text = "";
			int num = 0;
			if (text.bInt(ref num))
			{
				VoteInfo vote = VoteHelper.GetVote((long)num);
				vote.StartDate = System.DateTime.Now;
				if (VoteHelper.Update(vote, false))
				{
					this.ShowMsg("开启成功！", true);
					this.BindData();
					return;
				}
				this.ShowMsg("开启失败！", false);
			}
		}

		protected void lkStop_Click(object sender, System.EventArgs e)
		{
			string text = this.txt_Ids.Text;
			this.txt_Ids.Text = "";
			int num = 0;
			if (text.bInt(ref num))
			{
				VoteInfo vote = VoteHelper.GetVote((long)num);
				vote.EndDate = System.DateTime.Now;
				if (VoteHelper.Update(vote, false))
				{
					this.ShowMsg("结束成功！", true);
					this.BindData();
					return;
				}
				this.ShowMsg("结束失败！", false);
			}
		}

		protected void lkDelete_Click(object sender, System.EventArgs e)
		{
			string text = this.txt_Ids.Text;
			this.txt_Ids.Text = "";
			int num = 0;
			if (text.bInt(ref num))
			{
				if (VoteHelper.Delete((long)num))
				{
					this.ShowMsg("删除成功！", true);
					this.BindData();
					return;
				}
				this.ShowMsg("删除失败！", false);
			}
		}

		protected void DelBtn_Click(object sender, System.EventArgs e)
		{
			string text = this.txt_Ids.Text;
			if (text.Length > 1)
			{
				text = text.Substring(1);
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string s = array2[i];
				VoteHelper.Delete((long)int.Parse(s));
			}
			this.BindData();
			this.ShowMsg("批量删除成功！", true);
		}

		protected void btnSeach_Click(object sender, System.EventArgs e)
		{
			this.BindData();
		}
	}
}
