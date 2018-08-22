using ASPNET.WebControls;
using ControlPanel.Promotions;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class UCPrizeLists : System.Web.UI.UserControl
	{
		protected string isFinished = "1";

		protected int gameId = -1;

		private int pageSize = 10;

		private int pageIndex = 1;

		protected System.Web.UI.WebControls.Label lbGameName;

		protected PageSize hrefPageSize;

		protected Grid grdPrizeLists;

		protected Pager pager1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.isFinished = base.Request.QueryString["isFinished"];
			if (string.IsNullOrEmpty(this.isFinished))
			{
				this.isFinished = "1";
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
			try
			{
				this.gameId = int.Parse(base.Request.QueryString["gameId"]);
			}
			catch (System.Exception)
			{
			}
			this.pager1.DefaultPageSize = this.pageSize;
			if (!base.IsPostBack)
			{
				this.BindGameInfo();
				this.BindData();
			}
		}

		private void BindData()
		{
			PrizesDeliveQuery prizesDeliveQuery = new PrizesDeliveQuery
			{
				SortBy = "LogId",
				GameId = new int?(this.gameId),
				PageIndex = this.pageIndex,
				PageSize = this.pageSize
			};
			string text = this.isFinished;
			if (!string.IsNullOrEmpty(text))
			{
				prizesDeliveQuery.IsUsed = new int?(int.Parse(text));
			}
			DbQueryResult prizeLogLists = GameHelper.GetPrizeLogLists(prizesDeliveQuery);
			System.Data.DataTable dataSource = (System.Data.DataTable)prizeLogLists.Data;
			this.grdPrizeLists.DataSource = dataSource;
			this.grdPrizeLists.DataBind();
			this.pager1.TotalRecords = prizeLogLists.TotalRecords;
		}

		private void BindGameInfo()
		{
			GameInfo modelByGameId = GameHelper.GetModelByGameId(this.gameId);
			if (modelByGameId != null)
			{
				this.lbGameName.Text = modelByGameId.GameTitle;
			}
		}
	}
}
