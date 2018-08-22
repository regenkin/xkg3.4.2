using ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class EditGameEgg : AdminPage
	{
		protected GameType gameType = GameType.疯狂砸金蛋;

		private int _gameId = -1;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Label lbGameNum;

		protected System.Web.UI.WebControls.Label lbPrizeGade0;

		protected System.Web.UI.WebControls.Label lbPrizeGade1;

		protected System.Web.UI.WebControls.Label lbPrizeGade2;

		protected System.Web.UI.WebControls.Label lbPrizeGade3;

		protected System.Web.UI.WebControls.Label lbGameDescription;

		protected System.Web.UI.WebControls.Label lbBeginTime;

		protected System.Web.UI.WebControls.Label lbEedTime;

		protected UCGameInfo UCGameInfo;

		protected UCGamePrizeInfo UCGamePrizeInfo;

		protected System.Web.UI.WebControls.Button btnSubmit1;

		protected EditGameEgg() : base("m08", "yxp08")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				this._gameId = int.Parse(base.Request.QueryString["gameId"]);
			}
			catch (System.Exception)
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!this.Page.IsPostBack)
			{
				this.BindDate();
			}
		}

		private void BindDate()
		{
			GameInfo modelByGameId = GameHelper.GetModelByGameId(this._gameId);
			if (modelByGameId != null)
			{
				this.UCGameInfo.GameInfo = modelByGameId;
				System.Collections.Generic.IList<GamePrizeInfo> gamePrizeListsByGameId = GameHelper.GetGamePrizeListsByGameId(this._gameId);
				this.UCGamePrizeInfo.PrizeLists = gamePrizeListsByGameId;
				this.UCGamePrizeInfo.NotPrzeDescription = modelByGameId.NotPrzeDescription;
				try
				{
					this.lbPrizeGade0.Text = "一等奖：" + gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.一等奖).PrizeType.ToString();
					this.lbPrizeGade1.Text = "二等奖：" + gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.二等奖).PrizeType.ToString();
					this.lbPrizeGade2.Text = "三等奖：" + gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.三等奖).PrizeType.ToString();
					this.lbPrizeGade3.Text = "四等奖：" + gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.四等奖).PrizeType.ToString();
					this.lbBeginTime.Text = modelByGameId.BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
					this.lbEedTime.Text = modelByGameId.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
					this.lbGameDescription.Text = Globals.HtmlDecode(modelByGameId.Description);
				}
				catch (System.Exception)
				{
				}
			}
		}

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			this.SaveDate();
		}

		private void SaveDate()
		{
			try
			{
				GameInfo gameInfo = this.UCGameInfo.GameInfo;
				gameInfo.PrizeRate = this.UCGamePrizeInfo.PrizeRate;
				gameInfo.NotPrzeDescription = this.UCGamePrizeInfo.NotPrzeDescription;
				System.Collections.Generic.IList<GamePrizeInfo> prizeLists = this.UCGamePrizeInfo.PrizeLists;
				string[] array = new string[]
				{
					"一等奖",
					"二等奖",
					"三等奖",
					"四等奖"
				};
				int num = 0;
				bool flag = GameHelper.Update(gameInfo);
				if (!flag)
				{
					throw new System.Exception("更新失败！");
				}
				for (int i = 0; i < prizeLists.Count<GamePrizeInfo>(); i++)
				{
					GamePrizeInfo gamePrizeInfo = prizeLists[i];
					gamePrizeInfo.GameId = gameInfo.GameId;
					if (gamePrizeInfo.PrizeId > 0)
					{
						if (gamePrizeInfo.PrizeType == PrizeType.赠送积分)
						{
							gamePrizeInfo.PrizeImage = "/utility/pics/jifen60.png";
						}
						if (gamePrizeInfo.PrizeType == PrizeType.赠送优惠券)
						{
							gamePrizeInfo.PrizeImage = "/utility/pics/yhq60.png";
						}
						if (!GameHelper.UpdatePrize(gamePrizeInfo))
						{
							throw new System.Exception("更新奖品信息时失败！");
						}
						num += gamePrizeInfo.PrizeCount;
					}
					else
					{
						if (string.IsNullOrEmpty(gamePrizeInfo.PrizeName))
						{
							gamePrizeInfo.PrizeName = array[i];
						}
						if (gamePrizeInfo.PrizeType == PrizeType.赠送积分)
						{
							gamePrizeInfo.PrizeImage = "/utility/pics/jifen60.png";
						}
						if (gamePrizeInfo.PrizeType == PrizeType.赠送优惠券)
						{
							gamePrizeInfo.PrizeImage = "/utility/pics/yhq60.png";
						}
						if (!GameHelper.CreatePrize(gamePrizeInfo))
						{
							throw new System.Exception("添加奖品信息时失败！");
						}
						num += gamePrizeInfo.PrizeCount;
					}
				}
				bool flag2 = GameHelper.DeleteWinningPools(gameInfo.GameId);
				if (num > 0 && flag2)
				{
					GameHelper.CreateWinningPools(gameInfo.PrizeRate, num, gameInfo.GameId);
				}
				this.Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "ShowSuccess", "<script>$(function () { ShowStep2(); })</script>");
			}
			catch (System.Exception ex)
			{
				this.ShowMsg(ex.Message, false);
			}
		}
	}
}
