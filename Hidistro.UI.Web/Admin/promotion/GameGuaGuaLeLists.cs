using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class GameGuaGuaLeLists : AdminPage
	{
		protected string isFinished = "0";

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected UCGameLists UCGameLists1;

		protected GameType PGameType
		{
			get
			{
				return GameType.刮刮乐;
			}
		}

		protected GameGuaGuaLeLists() : base("m08", "yxp11")
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
