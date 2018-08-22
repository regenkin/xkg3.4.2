using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class GameEggLists : AdminPage
	{
		protected string isFinished = "0";

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected UCGameLists UCGameLists1;

		protected GameType PGameType
		{
			get
			{
				return GameType.疯狂砸金蛋;
			}
		}

		protected GameEggLists() : base("m08", "yxp08")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.UCGameLists1.PGameType = new GameType?(this.PGameType);
			this.isFinished = base.Request.QueryString["isFinished"];
			if (string.IsNullOrEmpty(this.isFinished))
			{
				this.isFinished = "0";
			}
		}
	}
}
