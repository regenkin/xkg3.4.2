using ControlPanel.Promotions;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class PromotionGamePrizesHandler : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			try
			{
				string s = "{\"type\":\"fail\",\"message\":\"删除失败\"}";
				string a = context.Request.QueryString["action"];
				if (a == "DeletePrize")
				{
					s = this.DeletePrize(context);
				}
				if (a == "AddPrize")
				{
					s = this.AddPrizeHtml(context);
				}
				context.Response.Write(s);
			}
			catch (System.Exception ex)
			{
				context.Response.Write("{\"type\":\"error\",\"message\":\"" + ex.Message + "\"}");
			}
		}

		private string AddPrizeHtml(System.Web.HttpContext context)
		{
			string result = "";
			int index = 0;
			PrizeGrade prizeGrade = PrizeGrade.一等奖;
			int gameId = Globals.RequestQueryNum("gameId");
			if (int.TryParse(context.Request.QueryString["index"], out index))
			{
				switch (index)
				{
				case 0:
					prizeGrade = PrizeGrade.一等奖;
					break;
				case 1:
					prizeGrade = PrizeGrade.二等奖;
					break;
				case 2:
					prizeGrade = PrizeGrade.三等奖;
					break;
				case 3:
					prizeGrade = PrizeGrade.四等奖;
					break;
				}
				result = "{\"type\":\"ok\",\"message\":\"" + Globals.String2Json(this.GetPrizeInfoHtml(prizeGrade, null, index, gameId)) + "\"}";
			}
			return result;
		}

		private System.Collections.Generic.List<System.Web.UI.WebControls.ListItem> BindDdlCouponId()
		{
			System.Collections.Generic.List<System.Web.UI.WebControls.ListItem> list = new System.Collections.Generic.List<System.Web.UI.WebControls.ListItem>();
			System.Data.DataTable unFinishedCoupon = CouponHelper.GetUnFinishedCoupon(System.DateTime.Now, new CouponType?(CouponType.活动赠送));
			if (unFinishedCoupon != null)
			{
				foreach (System.Data.DataRow dataRow in unFinishedCoupon.Rows)
				{
					list.Add(new System.Web.UI.WebControls.ListItem
					{
						Text = dataRow["CouponName"].ToString(),
						Value = dataRow["CouponId"].ToString()
					});
				}
			}
			return list;
		}

		protected string GetPrizeInfoHtml(PrizeGrade prizeGrade, GamePrizeInfo model, int index, int gameId)
		{
			System.Collections.Generic.List<System.Web.UI.WebControls.ListItem> list = this.BindDdlCouponId();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("<div class='tabContent' id='tempContentId" + index + "'>");
			stringBuilder.Append("<div class='form-horizontal clearfix'>");
			stringBuilder.Append("<div class='form-group setmargin'>");
			stringBuilder.Append("<label class='col-xs-3 pad resetSize control-label'><em>*</em>&nbsp;&nbsp;奖品类别：</label>");
			stringBuilder.Append("<div class='form-inline col-xs-9'>");
			stringBuilder.Append("<div class='resetradio selectradio pt3' >");
			stringBuilder.Append("<label class=\"mr20\">");
			if (model != null)
			{
				stringBuilder.AppendFormat(" <input type=\"radio\" id=\"rd{0}_0\" name=\"prizeType_{0}\" {1} value=\"0\" />赠送积分</label>", (int)prizeGrade, (model.PrizeType == PrizeType.赠送积分) ? "checked=\"checked\"" : "");
				stringBuilder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_1\" name=\"prizeType_{0}\" {1} value=\"1\" />赠送优惠券</label>", (int)prizeGrade, (model.PrizeType == PrizeType.赠送优惠券) ? "checked=\"checked\"" : "");
				stringBuilder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_2\" name=\"prizeType_{0}\" {1} value=\"2\" />赠送商品</label>", (int)prizeGrade, (model.PrizeType == PrizeType.赠送商品) ? "checked=\"checked\"" : "");
				stringBuilder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_3\" name=\"prizeType_{0}\" {1} value=\"3\" />其他奖品</label>", (int)prizeGrade, (model.PrizeType == PrizeType.其他奖品) ? "checked=\"checked\"" : "");
				stringBuilder.AppendFormat("<input type=\"hidden\" id=\"prizeTypeValue{0}\" value=\"{1}\" />", (int)prizeGrade, (int)model.PrizeType);
				stringBuilder.AppendFormat("<input type=\"hidden\" name=\"prizeInfoId{0}\" value=\"{1}\" />", (int)prizeGrade, model.PrizeId);
				stringBuilder.AppendFormat("<input type=\"hidden\" name=\"prizeGameId{0}\" value=\"{1}\" />", (int)prizeGrade, model.GameId);
			}
			else
			{
				stringBuilder.AppendFormat(" <input type=\"radio\" id=\"rd{0}_0\" name=\"prizeType_{0}\" checked=\"checked\" value=\"0\" />赠送积分</label>", (int)prizeGrade);
				stringBuilder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_1\" name=\"prizeType_{0}\" value=\"1\" />赠送优惠券</label>", (int)prizeGrade);
				stringBuilder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_2\" name=\"prizeType_{0}\" value=\"2\" />赠送商品</label>", (int)prizeGrade);
				stringBuilder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_3\" name=\"prizeType_{0}\" value=\"3\" />其他奖品</label>", (int)prizeGrade);
				stringBuilder.AppendFormat("<input type=\"hidden\" id=\"prizeTypeValue{0}\" value=\"{1}\" />", (int)prizeGrade, index);
				stringBuilder.AppendFormat("<input type=\"hidden\" name=\"prizeInfoId{0}\" value=\"{1}\" />", (int)prizeGrade, 0);
				stringBuilder.AppendFormat("<input type=\"hidden\" name=\"prizeGameId{0}\" value=\"{1}\" />", (int)prizeGrade, gameId);
			}
			stringBuilder.Append(" </div></div></div>");
			stringBuilder.Append("<div class=\"form-group setmargin\" style=\"display:normal\">");
			stringBuilder.Append(" <label class=\"col-xs-3 pad resetSize control-label\" for=\"Prize\"><em>*</em>&nbsp;&nbsp;奖品名称：</label> <div class=\"form-inline col-xs-9\">");
			if (model != null)
			{
				stringBuilder.AppendFormat("<input type=\"text\" name=\"txtPrize{0}\" id=\"txtPrize{0}\" class=\"form-control resetSize\" value=\"{1}\"/>", (int)prizeGrade, model.Prize);
			}
			else
			{
				stringBuilder.AppendFormat("<input type=\"text\" name=\"txtPrize{0}\" id=\"txtPrize{0}\" class=\"form-control resetSize\" value=\"\"/>", (int)prizeGrade);
			}
			stringBuilder.Append("</div></div>");
			if (model != null && model.PrizeType == PrizeType.赠送积分)
			{
				stringBuilder.Append(" <div class=\"form-group setmargin give giveint\"  style=\"display:normal\">");
			}
			else
			{
				stringBuilder.Append(" <div class=\"form-group setmargin give giveint\">");
			}
			stringBuilder.Append(" <label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp");
			stringBuilder.Append("赠送积分：</label> <div class=\"form-inline col-xs-9\">");
			if (model != null)
			{
				stringBuilder.AppendFormat(" <input type=\"text\" name=\"txtGivePoint{0}\" id=\"txtGivePoint{0}\" class=\"form-control resetSize\" value=\"{1}\" />", (int)prizeGrade, model.GivePoint);
			}
			else
			{
				stringBuilder.AppendFormat(" <input type=\"text\" name=\"txtGivePoint{0}\" id=\"txtGivePoint{0}\" class=\"form-control resetSize\" value=\"0\" />", (int)prizeGrade);
			}
			if (model != null && model.PrizeType == PrizeType.赠送优惠券)
			{
				stringBuilder.Append(" </div> </div><div class=\"form-group setmargin give givecop\" style=\"display:normal\">");
			}
			else
			{
				stringBuilder.Append(" </div> </div><div class=\"form-group setmargin give givecop\">");
			}
			stringBuilder.Append(" <label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp;赠送优惠券：</label> <div class=\"form-inline col-xs-9\">");
			stringBuilder.AppendFormat(" <select name=\"seletCouponId{0}\" id=\"seletCouponId{0}\" class=\"form-control resetSize\">", (int)prizeGrade);
			if (model != null)
			{
				using (System.Collections.Generic.List<System.Web.UI.WebControls.ListItem>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						System.Web.UI.WebControls.ListItem current = enumerator.Current;
						if (string.Equals(model.GiveCouponId, current.Value))
						{
							stringBuilder.AppendFormat(" <option value=\"{0}\" selected=\"selected\">{1}</option>", current.Value, current.Text);
						}
						else
						{
							stringBuilder.AppendFormat(" <option value=\"{0}\">{1}</option>", current.Value, current.Text);
						}
					}
					goto IL_3C9;
				}
			}
			foreach (System.Web.UI.WebControls.ListItem current2 in list)
			{
				stringBuilder.AppendFormat(" <option value=\"{0}\">{1}</option>", current2.Value, current2.Text);
			}
			IL_3C9:
			stringBuilder.Append(" </select> </div>  </div> ");
			if (model != null && model.PrizeType == PrizeType.赠送商品)
			{
				stringBuilder.Append("<div class=\"form-group setmargin give giveshop\" style=\"display:normal\">");
			}
			else
			{
				stringBuilder.Append("<div class=\"form-group setmargin give giveshop\">");
			}
			stringBuilder.Append("<label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp;赠送商品：</label>");
			stringBuilder.Append("<div class=\"form-inline col-xs-9\"><div class=\"pt3\">");
			if (model != null)
			{
				stringBuilder.AppendFormat("<img id=\"imgProduct{0}\" style=\"width:30px; height:30px;\" name=\"imgProduct{0}\"  src=\"{1}\"onclick=\"SelectShopBookId({0});\" />", (int)prizeGrade, string.IsNullOrEmpty(model.GriveShopBookPicUrl) ? "../images/u100.png" : model.GriveShopBookPicUrl);
				stringBuilder.AppendFormat("<input type=\"hidden\" name=\"txtShopbookId{0}\" id=\"txtShopbookId{0}\"  value=\"{1}\" />", (int)prizeGrade, model.GiveShopBookId);
				stringBuilder.AppendFormat("<input type=\"hidden\" id=\"txtProductPic{0}\" name=\"txtProductPic{0}\"  value=\"{1}\" />", (int)prizeGrade, string.IsNullOrEmpty(model.GriveShopBookPicUrl) ? "../images/u100.png" : model.GriveShopBookPicUrl);
			}
			else
			{
				stringBuilder.AppendFormat("<img id=\"imgProduct{0}\" style=\"width:30px; height:30px;\" name=\"imgProduct{0}\" src=\"../images/u100.png\" onclick=\"SelectShopBookId({0});\" />", (int)prizeGrade);
				stringBuilder.AppendFormat("<input type=\"hidden\" name=\"txtShopbookId{0}\" id=\"txtShopbookId{0}\" />", (int)prizeGrade);
				stringBuilder.AppendFormat("<input type=\"hidden\" id=\"txtProductPic{0}\" name=\"txtProductPic{0}\"  />", (int)prizeGrade);
			}
			stringBuilder.Append("</div> </div></div>");
			if (model != null && model.PrizeType == PrizeType.其他奖品)
			{
				stringBuilder.Append("<div class=\"form-group setmargin give other\" style=\"display:normal\">");
			}
			else
			{
				stringBuilder.Append("<div class=\"form-group setmargin give other\">");
			}
			stringBuilder.Append("<label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp;是否配送：</label>");
			stringBuilder.Append("<div class=\"form-inline col-xs-9\"><div class=\"pt3 resetradio mb5 pt3 allradio\">");
			if (model != null)
			{
				stringBuilder.AppendFormat("<label class=\"mr20\"> <input type=\"checkbox\" id=\"ckbNeed_{0}\" name=\"ckbNeed_{0}\"  {1}>是，需要配送</label>", (int)prizeGrade, (model.IsLogistics == 1) ? "checked" : "");
			}
			else
			{
				stringBuilder.AppendFormat("<label class=\"mr20\"> <input type=\"checkbox\" id=\"ckbNeed_{0}\" name=\"ckbNeed_{0}\" >是，需要配送</label>", (int)prizeGrade);
			}
			stringBuilder.Append("</div> </div></div>");
			stringBuilder.Append("<div class=\"form-group setmargin\">");
			stringBuilder.Append(" <label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp;奖品数量：</label> <div class=\"form-inline col-xs-9\">");
			if (model != null)
			{
				stringBuilder.AppendFormat("<input type=\"text\" name=\"txtPrizeCount{0}\" id=\"txtPrizeCount{0}\" class=\"form-control resetSize\" value=\"{1}\"/>", (int)prizeGrade, model.PrizeCount);
			}
			else
			{
				stringBuilder.AppendFormat("<input type=\"text\" name=\"txtPrizeCount{0}\" id=\"txtPrizeCount{0}\" class=\"form-control resetSize\" value=\"1\"/>", (int)prizeGrade);
			}
			stringBuilder.Append("  <small>奖品数量为0时不设此奖项</small> </div> </div>");
			if (model != null && model.PrizeType == PrizeType.其他奖品)
			{
				stringBuilder.Append("<div class=\"form-group setmargin give other\" style=\"display:normal\">");
			}
			else
			{
				stringBuilder.Append("<div class=\"form-group setmargin give other\">");
			}
			stringBuilder.Append("<label class=\"col-xs-3 pad resetSize control-label\" for=\"PrizeImage\"><em></em>&nbsp;&nbsp;奖品图片：</label>");
			stringBuilder.Append("<div class=\"form-inline col-xs-9\"><div class=\"pt3\" style=\"vertical-align:bottom;\">");
			if (model != null)
			{
				stringBuilder.AppendFormat("<img id=\"PrizeImage{0}\" style=\"width:60px; height:60px;\" name=\"PrizeImage{0}\"  src=\"{1}\"onclick=\"SelectPrizeImage({0});\" />  <div style=\"margin-left:70px\">仅支持jpg、 png、gif，尺寸60*60px,不超过1M  </div>", (int)prizeGrade, string.IsNullOrEmpty(model.PrizeImage) ? "../images/u100.png" : model.PrizeImage);
				stringBuilder.AppendFormat("<input type=\"hidden\" id=\"hiddPrizeImage{0}\" name=\"hiddPrizeImage{0}\"  value=\"{1}\" />", (int)prizeGrade, string.IsNullOrEmpty(model.PrizeImage) ? "../images/u100.png" : model.PrizeImage);
			}
			else
			{
				stringBuilder.AppendFormat("<img id=\"PrizeImage{0}\" style=\"width:60px; height:60px;\" name=\"PrizeImage{0}\" src=\"../images/u100.png\" onclick=\"SelectPrizeImage({0});\" />  <div style=\"margin-left:70px\">仅支持jpg、 png、gif，尺寸60*60px,不超过1M</div> ", (int)prizeGrade);
				stringBuilder.AppendFormat("<input type=\"hidden\" id=\"hiddPrizeImage{0}\" name=\"hiddPrizeImage{0}\"  />", (int)prizeGrade);
			}
			stringBuilder.Append("</div> </div></div>");
			stringBuilder.Append("</div></div>");
			return stringBuilder.ToString();
		}

		public string DeletePrize(System.Web.HttpContext context)
		{
			int gameId = 0;
			int prizeId = 0;
			string result = "{\"type\":\"fail\",\"message\":\"删除失败，请确认活动未开始！\"}";
			if (int.TryParse(context.Request.QueryString["gameId"], out gameId) && int.TryParse(context.Request.QueryString["prizeId"], out prizeId))
			{
				GamePrizeInfo gamePrizeInfo = new GamePrizeInfo();
				gamePrizeInfo.GameId = gameId;
				gamePrizeInfo.PrizeId = prizeId;
				bool flag = GameHelper.DeletePromotionGamePrize(gamePrizeInfo);
				if (flag)
				{
					bool flag2 = GameHelper.DeleteWinningPools(gamePrizeInfo.GameId);
					GameInfo gameInfoById = GameHelper.GetGameInfoById(gamePrizeInfo.GameId);
					if (gameInfoById != null)
					{
						int num = 0;
						System.Collections.Generic.IList<GamePrizeInfo> gamePrizeListsByGameId = GameHelper.GetGamePrizeListsByGameId(gamePrizeInfo.GameId);
						foreach (GamePrizeInfo current in gamePrizeListsByGameId)
						{
							num += current.PrizeCount;
						}
						if (num > 0 && flag2)
						{
							GameHelper.CreateWinningPools(gameInfoById.PrizeRate, num, gameInfoById.GameId);
						}
						result = "{\"type\":\"ok\",\"message\":\"删除成功\"}";
					}
				}
			}
			return result;
		}
	}
}
