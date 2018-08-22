using ASPNET.WebControls;
using ControlPanel.Promotions;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class UCGameLists : BaseUserControl
	{
		protected string isFinished = "0";

		private int pageSize = 10;

		private int pageIndex = 1;

		protected PageSize hrefPageSize;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button btnSeach;

		protected System.Web.UI.WebControls.Button btnDel;

		protected Grid grdGameLists;

		protected Pager pager1;

		public GameType? PGameType
		{
			get;
			set;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.isFinished = base.Request.QueryString["isFinished"];
			if (string.IsNullOrEmpty(this.isFinished))
			{
				this.isFinished = "0";
			}
			try
			{
				this.pageIndex = int.Parse(base.Request["pageindex"]);
			}
			catch (System.Exception)
			{
				this.pageIndex = 1;
			}
			try
			{
				this.pageSize = int.Parse(base.Request.QueryString["pagesize"]);
			}
			catch (System.Exception)
			{
				this.pageSize = 10;
			}
			this.pager1.DefaultPageSize = this.pageSize;
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void BindData()
		{
			GameSearch gameSearch = new GameSearch
			{
				SortBy = "GameId",
				PageIndex = this.pageIndex,
				PageSize = this.pageSize,
				GameType = new int?((int)this.PGameType.Value),
				BeginTime = this.calendarStartDate.SelectedDate,
				EndTime = this.calendarEndDate.SelectedDate
			};
			string text = this.isFinished;
			if (!string.IsNullOrEmpty(text))
			{
				gameSearch.Status = text;
			}
			DbQueryResult gameListByView = GameHelper.GetGameListByView(gameSearch);
			System.Data.DataTable dataSource = (System.Data.DataTable)gameListByView.Data;
			this.grdGameLists.DataSource = dataSource;
			this.grdGameLists.DataBind();
			this.pager1.TotalRecords = gameListByView.TotalRecords;
		}

		protected void grdGameLists_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			string text = this.grdGameLists.DataKeys[e.RowIndex].Value.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				try
				{
					bool flag = GameHelper.Delete(new int[]
					{
						int.Parse(text)
					});
					if (!flag)
					{
						throw new System.Exception("操作失败！");
					}
					this.ShowMsg("操作成功！", true);
					this.BindData();
				}
				catch (System.Exception ex)
				{
					this.ShowMsg(ex.Message, false);
				}
			}
		}

		protected void grdGameLists_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
		{
			string text = this.grdGameLists.DataKeys[e.RowIndex].Value.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				try
				{
					bool flag = GameHelper.UpdateStatus(int.Parse(text), GameStatus.结束);
					if (!flag)
					{
						throw new System.Exception("操作失败！");
					}
					this.ShowMsg("操作成功！", true);
					this.BindData();
				}
				catch (System.Exception ex)
				{
					this.ShowMsg(ex.Message, false);
				}
			}
		}

		protected void grdGameLists_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowIndex >= 0)
			{
				GameStatus gameStatus = (GameStatus)int.Parse(((System.Web.UI.WebControls.HiddenField)e.Row.FindControl("hfStatus")).Value);
				string value = ((System.Web.UI.WebControls.HiddenField)e.Row.FindControl("hfBeginTime")).Value;
				string value2 = ((System.Web.UI.WebControls.HiddenField)e.Row.FindControl("hfEndTime")).Value;
				System.Convert.ToDateTime(value);
				System.Convert.ToDateTime(value2);
				if (gameStatus == GameStatus.正常)
				{
					((System.Web.UI.WebControls.Button)e.Row.FindControl("lkDelete")).Visible = false;
					return;
				}
				((System.Web.UI.WebControls.Button)e.Row.FindControl("FinishBtn")).Visible = false;
			}
		}

		protected void btnSeach_Click(object sender, System.EventArgs e)
		{
			this.BindData();
		}

        protected string GetEditUrl(string gameId, string sstartTime)
        {
            System.DateTime now = System.DateTime.Now;
            System.DateTime.TryParse(sstartTime, out now);
            string text = string.Empty;
            string text2 = "编辑";
            if (now <= System.DateTime.Now)
            {
                text = "action=deital&";
                text2 = "详情";
            }
            GameType valueOrDefault = this.PGameType.GetValueOrDefault();
            switch (valueOrDefault)
            {
                case GameType.幸运大转盘:
                    return string.Format(string.Concat(new string[]
                    {
                        "<a href='EditGame.aspx?",
                        text,
                        "gameId={0}' class='btn btn-primary resetSize'>",
                        text2,
                        "</a>"
                    }), gameId);
                case GameType.疯狂砸金蛋:
                    return string.Format(string.Concat(new string[]
                    {
                        "<a href='EditGameEgg.aspx?",
                        text,
                        "gameId={0}' class='btn btn-primary resetSize'>",
                        text2,
                        "</a>"
                    }), gameId);
                case GameType.好运翻翻看:
                    return string.Format(string.Concat(new string[]
                    {
                        "<a href='EditGameXingYun.aspx?",
                        text,
                        "gameId={0}' class='btn btn-primary resetSize'>",
                        text2,
                        "</a>"
                    }), gameId);
                case GameType.大富翁:
                    return string.Format(string.Concat(new string[]
                    {
                        "<a href='EditGameDaFuWen.aspx?",
                        text,
                        "gameId={0}' class='btn btn-primary resetSize'>",
                        text2,
                        "</a>"
                    }), gameId);
                case GameType.刮刮乐:
                    return string.Format(string.Concat(new string[]
                    {
                        "<a href='EditGameGuaGuaLe.aspx?",
                        text,
                        "gameId={0}' class='btn btn-primary resetSize'>",
                        text2,
                        "</a>"
                    }), gameId);
            }
            return "";
        }

        protected string GetDetialUrl(string gameId)
        {
            GameType valueOrDefault = this.PGameType.GetValueOrDefault();
            switch (valueOrDefault)
            {
                case GameType.幸运大转盘:
                    return string.Format("EditGame.aspx?action=deital&gameId={0}", gameId);
                case GameType.疯狂砸金蛋:
                    return string.Format("EditGameEgg.aspx?action=deital&gameId={0}", gameId);
                case GameType.好运翻翻看:
                    return string.Format("EditGameXingYun.aspx?action=deital&gameId={0}", gameId);
                case GameType.大富翁:
                    return string.Format("EditGameDaFuWen.aspx?action=deital&gameId={0}", gameId);
                case GameType.刮刮乐:
                    return string.Format("EditGameGuaGuaLe.aspx?action=deital&gameId={0}", gameId);
            }
            return "";
        }

        protected string GetPrizeListsUrl(string gameId)
        {
            GameType valueOrDefault = this.PGameType.GetValueOrDefault();
            switch (valueOrDefault)
            {
                case GameType.幸运大转盘:
                    return string.Format("PrizeLists.aspx?gameId={0}", gameId);
                case GameType.疯狂砸金蛋:
                    return string.Format("PrizeListsEgg.aspx?gameId={0}", gameId);
                case GameType.好运翻翻看:
                    return string.Format("PrizeListsHaoYun.aspx?gameId={0}", gameId);
                case GameType.大富翁:
                    return string.Format("PrizeListsDaFuWen.aspx?gameId={0}", gameId);
                case GameType.刮刮乐:
                    return string.Format("PrizeListsGuaGuaLe.aspx?gameId={0}", gameId);
            }
            return "";
        }

		protected void btnDel_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdGameLists.Rows)
			{
				if (gridViewRow.RowIndex >= 0)
				{
					System.Web.UI.WebControls.CheckBox checkBox = gridViewRow.Cells[0].Controls[0] as System.Web.UI.WebControls.CheckBox;
					if (checkBox.Checked)
					{
						list.Add(int.Parse(this.grdGameLists.DataKeys[gridViewRow.RowIndex].Value.ToString()));
					}
				}
			}
			if (list.Count <= 0)
			{
				this.ShowMsg("请至少选择一条要删除的数据！", false);
				return;
			}
			try
			{
				bool flag = GameHelper.Delete(list.ToArray());
				if (!flag)
				{
					throw new System.Exception("操作失败！");
				}
				this.ShowMsg("操作成功！", true);
				this.BindData();
			}
			catch (System.Exception ex)
			{
				this.ShowMsg(ex.Message, false);
			}
		}

		protected string GetLimit(object limitEveryDay, object maximumDailyLimit)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			int num = (int)limitEveryDay;
			int num2 = (int)maximumDailyLimit;
			if (num == 0 && num2 == 0)
			{
				stringBuilder.Append("不限次");
			}
			else
			{
				if (num != 0)
				{
					stringBuilder.AppendFormat("每天参与{0}次", num);
				}
				stringBuilder.Append("<br/>");
				if (num2 != 0)
				{
					stringBuilder.AppendFormat("参与上限{0}次", num2);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
