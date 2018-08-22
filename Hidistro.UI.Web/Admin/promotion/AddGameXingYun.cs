using ControlPanel.Promotions;
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
	public class AddGameXingYun : AdminPage
	{
		protected GameType gameType = GameType.好运翻翻看;

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

		protected AddGameXingYun() : base("m08", "yxp09")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.UCGameInfo.GameType = this.gameType;
		}

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			this.SaveDate();
		}

		private void SaveDate()
		{
			int num = -1;
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
				int num2 = 0;
				bool flag = GameHelper.Create(gameInfo, out num);
				if (!flag)
				{
					throw new System.Exception("添加失败！");
				}
				for (int i = 0; i < prizeLists.Count<GamePrizeInfo>(); i++)
				{
					GamePrizeInfo gamePrizeInfo = prizeLists[i];
					gamePrizeInfo.GameId = num;
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
					num2 += gamePrizeInfo.PrizeCount;
				}
				if (num2 > 0)
				{
					GameHelper.CreateWinningPools(gameInfo.PrizeRate, num2, num);
				}
				this.Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "ShowSuccess", "<script>$(function () { ShowStep2(); })</script>");
			}
			catch (System.Exception ex)
			{
				if (num > 0)
				{
					GameHelper.Delete(new int[]
					{
						num
					});
				}
				this.ShowMsg(ex.Message, false);
			}
		}

		private void BindViewInfo(GameInfo gameInfo, System.Collections.Generic.IList<GamePrizeInfo> prizeLists)
		{
			this.lbGameDescription.Text = gameInfo.Description;
			this.lbBeginTime.Text = gameInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
			this.lbEedTime.Text = gameInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
			this.lbPrizeGade0.Text = string.Format("{0}：{1}", PrizeGrade.一等奖, prizeLists.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.一等奖).PrizeType.ToString());
			this.lbPrizeGade1.Text = string.Format("{0}：{1}", PrizeGrade.二等奖, prizeLists.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.二等奖).PrizeType.ToString());
			this.lbPrizeGade2.Text = string.Format("{0}：{1}", PrizeGrade.三等奖, prizeLists.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.三等奖).PrizeType.ToString());
			this.lbPrizeGade3.Text = string.Format("{0}：{1}", PrizeGrade.四等奖, prizeLists.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.四等奖).PrizeType.ToString());
		}
	}
}
