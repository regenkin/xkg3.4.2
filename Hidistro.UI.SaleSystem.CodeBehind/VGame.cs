using ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class VGame : VMemberTemplatedWebControl
	{
		private string htmlTitle = string.Empty;

		protected override void OnInit(System.EventArgs e)
		{
			GameType gameType = GameType.幸运大转盘;
			try
			{
				gameType = (GameType)System.Enum.Parse(typeof(GameType), this.Page.Request.QueryString["type"]);
			}
			catch (System.Exception)
			{
				base.GotoResourceNotFound("");
			}
			this.htmlTitle = gameType.ToString();
			if (this.SkinName == null)
			{
				switch (gameType)
				{
				case GameType.幸运大转盘:
					this.SkinName = "skin-vGameZhuangPan.html";
					break;
				case GameType.疯狂砸金蛋:
					this.SkinName = "skin-vGameEgg.html";
					break;
				case GameType.好运翻翻看:
					this.SkinName = "skin-vGameHaoYun.html";
					break;
				case GameType.大富翁:
					this.SkinName = "skin-vGameDaFuWen.html";
					break;
				case GameType.刮刮乐:
					this.SkinName = "skin-vGameGuaGuaLe.html";
					break;
				default:
					base.GotoResourceNotFound("");
					break;
				}
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle(this.htmlTitle);
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)this.FindControl("litJs");
			string keyWord = Globals.RequestQueryStr("gamid");
			GameInfo modelByGameId = GameHelper.GetModelByGameId(keyWord);
			string s = string.Empty;
			if (modelByGameId != null)
			{
				s = modelByGameId.NotPrzeDescription;
				string gameTitle = modelByGameId.GameTitle;
				string description = modelByGameId.Description;
				System.Uri url = this.Context.Request.Url;
				string text = url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
				string text2 = string.Empty;
				GameType gameType = GameType.幸运大转盘;
				try
				{
					gameType = (GameType)System.Enum.Parse(typeof(GameType), Globals.RequestQueryNum("type").ToString());
				}
				catch
				{
				}
				string text3 = string.Empty;
				switch (gameType)
				{
				case GameType.幸运大转盘:
				{
					text2 = "/Utility/pics/game_dzp.png";
					System.Collections.Generic.IList<GamePrizeInfo> gamePrizeListsByGameId = GameHelper.GetGamePrizeListsByGameId(modelByGameId.GameId);
					foreach (GamePrizeInfo current in gamePrizeListsByGameId)
					{
						object obj = text3;
						text3 = string.Concat(new object[]
						{
							obj,
							"<img src='",
							current.PrizeImage,
							"' id='price",
							current.PrizeId,
							"' style='display:none;' />"
						});
					}
					break;
				}
				case GameType.疯狂砸金蛋:
					text2 = "/Utility/pics/game_zjd.png";
					break;
				case GameType.好运翻翻看:
					text2 = "/Utility/pics/game_ffk.png";
					break;
				case GameType.大富翁:
					text2 = "/Utility/pics/game_dfw.png";
					break;
				case GameType.刮刮乐:
					text2 = "/Utility/pics/game_ggk.png";
					break;
				default:
					text2 = "/Utility/pics/game_dzp.png";
					break;
				}
				literal.Text = string.Concat(new string[]
				{
					text3,
					"<script>thanksTips=\"",
					this.Context.Server.HtmlEncode(s),
					"\";wxinshare_title=\"",
					this.Context.Server.HtmlEncode(gameTitle),
					"\";wxinshare_desc=\"",
					this.Context.Server.HtmlEncode(description.Replace("\n", " ").Replace("\r", "")),
					"\";wxinshare_link=location.href;wxinshare_imgurl=\"",
					text,
					text2,
					"\"</script>"
				});
			}
			else
			{
				this.Context.Response.Redirect("/default.aspx");
				this.Context.Response.End();
			}
		}
	}
}
