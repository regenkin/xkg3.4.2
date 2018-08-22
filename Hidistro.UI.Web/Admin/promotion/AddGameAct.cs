using ControlPanel.Promotions;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class AddGameAct : AdminPage
	{
		protected eGameType _gameType;

		protected static int _gameId = 0;

		protected static int _step = 1;

		protected string _grade = "";

		protected string _json = "";

		protected System.Collections.Generic.IList<GameActPrizeInfo> _lst;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.TextBox txt_decrip;

		protected System.Web.UI.WebControls.Button saveBtn;

		protected System.Web.UI.WebControls.TextBox txt_grades;

		protected System.Web.UI.WebControls.TextBox txt_uPoint;

		protected System.Web.UI.WebControls.TextBox txt_gPoint;

		protected System.Web.UI.WebControls.CheckBox onlyChk;

		protected System.Web.UI.WebControls.RadioButton rd1;

		protected System.Web.UI.WebControls.RadioButton rd2;

		protected System.Web.UI.WebControls.RadioButton rd3;

		protected System.Web.UI.WebControls.RadioButton rd4;

		protected System.Web.UI.WebControls.Button back_Step2;

		protected System.Web.UI.WebControls.Button save_Step2;

		protected System.Web.UI.WebControls.TextBox txt_json;

		protected System.Web.UI.WebControls.Button back_Step3;

		protected System.Web.UI.WebControls.Button save_Step3;

		protected System.Web.UI.WebControls.Button Button1;

		protected System.Web.UI.WebControls.Button Button2;

		protected System.Web.UI.HtmlControls.HtmlInputHidden htxtRoleId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
			this.save_Step2.Click += new System.EventHandler(this.save_Step2_Click);
			this.back_Step2.Click += new System.EventHandler(this.back_Step2_Click);
			this.save_Step3.Click += new System.EventHandler(this.save_Step3_Click);
			this.back_Step3.Click += new System.EventHandler(this.back_Step3_Click);
			string[] allKeys = base.Request.Params.AllKeys;
			this.txt_decrip.MaxLength = 600;
			if (allKeys.Contains("id") && base.Request["id"].ToString().bInt(ref AddGameAct._gameId))
			{
				GameActInfo gameActInfo = GameActHelper.Get(AddGameAct._gameId);
				if (gameActInfo == null)
				{
					this.ShowMsg("没有这个游戏~", false);
					return;
				}
				AddGameAct._step = gameActInfo.CreateStep;
				this._gameType = gameActInfo.GameType;
				this.txt_name.Text = gameActInfo.GameName;
				this.calendarStartDate.SelectedDate = new System.DateTime?(gameActInfo.BeginDate);
				this.calendarEndDate.SelectedDate = new System.DateTime?(gameActInfo.EndDate);
				this.txt_decrip.Text = gameActInfo.Decription;
				this.txt_gPoint.Text = gameActInfo.GivePoint.ToString();
				this.txt_uPoint.Text = gameActInfo.usePoint.ToString();
				this._grade = gameActInfo.MemberGrades;
				this.onlyChk.Checked = gameActInfo.bOnlyNotWinner;
				if (gameActInfo.attendTimes == 0)
				{
					this.rd1.Checked = true;
				}
				else if (gameActInfo.attendTimes == 1)
				{
					this.rd2.Checked = true;
				}
				else if (gameActInfo.attendTimes == 2)
				{
					this.rd3.Checked = true;
				}
				else if (gameActInfo.attendTimes == 3)
				{
					this.rd4.Checked = true;
				}
			}
			if (allKeys.Contains("step"))
			{
				int step = 0;
				if (base.Request["step"].ToString().bInt(ref step))
				{
					AddGameAct._step = step;
				}
			}
			if (AddGameAct._step > 4)
			{
				AddGameAct._step = 4;
			}
			if (AddGameAct._step == 3)
			{
				System.Collections.Generic.IList<GameActPrizeInfo> prizesModel = GameActHelper.GetPrizesModel(AddGameAct._gameId);
				if (prizesModel != null && prizesModel.Count > 0)
				{
					foreach (GameActPrizeInfo current in prizesModel)
					{
						current.PointRate = System.Math.Round(current.PointRate);
						current.CouponRate = System.Math.Round(current.CouponRate);
						current.ProductRate = System.Math.Round(current.ProductRate);
					}
					this._json = JsonConvert.SerializeObject(prizesModel);
				}
			}
		}

		private void SaveGameStep2()
		{
			string text = this.txt_grades.Text.Trim();
			string val = this.txt_uPoint.Text.Trim();
			string text2 = this.txt_gPoint.Text.Trim();
			int usePoint = 0;
			int givePoint = 0;
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请选择适用会员等级！", false);
				return;
			}
			if (!val.bInt(ref usePoint))
			{
				this.ShowMsg("请输入正确的消耗积分！", false);
				return;
			}
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "0";
			}
			if (!text2.bInt(ref givePoint))
			{
				this.ShowMsg("请输入正确的赠送积分！", false);
				return;
			}
			GameActInfo gameActInfo = GameActHelper.Get(AddGameAct._gameId);
			gameActInfo.MemberGrades = text;
			gameActInfo.usePoint = usePoint;
			gameActInfo.GivePoint = givePoint;
			gameActInfo.bOnlyNotWinner = this.onlyChk.Checked;
			if (this.rd1.Checked)
			{
				gameActInfo.attendTimes = 0;
			}
			else if (this.rd2.Checked)
			{
				gameActInfo.attendTimes = 1;
			}
			else if (this.rd3.Checked)
			{
				gameActInfo.attendTimes = 2;
			}
			else if (this.rd4.Checked)
			{
				gameActInfo.attendTimes = 3;
			}
			gameActInfo.CreateStep = 3;
			string text3 = "";
			GameActHelper.Update(gameActInfo, ref text3);
			base.Response.Redirect("AddGameAct.aspx?id=" + AddGameAct._gameId + "&step=3");
		}

		private int AddGame()
		{
			string text = this.txt_name.Text.Trim();
			string text2 = this.txt_decrip.Text.Trim();
			System.DateTime date = this.calendarStartDate.SelectedDate.Value.Date;
			System.DateTime dateTime = this.calendarEndDate.SelectedDate.Value.Date.AddDays(1.0).AddSeconds(-1.0);
			if (string.IsNullOrEmpty(text) || text.Length > 30)
			{
				this.ShowMsg("请输入正确的游戏名，不超过30个字符！", false);
				return 0;
			}
			if (text2.Length > 600)
			{
				this.ShowMsg("游戏说明不能超过600个字符！", false);
				return 0;
			}
			if (dateTime < date)
			{
				this.ShowMsg("结束时间不能早于开始时间!", false);
				return 0;
			}
			GameActInfo gameActInfo = new GameActInfo();
			if (AddGameAct._gameId != 0)
			{
				gameActInfo = GameActHelper.Get(AddGameAct._gameId);
			}
			gameActInfo.GameName = text;
			gameActInfo.Decription = text2;
			gameActInfo.BeginDate = date;
			gameActInfo.EndDate = dateTime;
			gameActInfo.CreateStep = 2;
			string text3 = "";
			int result;
			if (AddGameAct._gameId != 0)
			{
				GameActHelper.Update(gameActInfo, ref text3);
				result = gameActInfo.GameId;
			}
			else
			{
				result = GameActHelper.Create(gameActInfo, ref text3);
			}
			return result;
		}

		protected AddGameAct() : base("m08", "yxp07")
		{
		}

		protected void saveBtn_Click(object sender, System.EventArgs e)
		{
			AddGameAct._gameId = this.AddGame();
			base.Response.Redirect("AddGameAct.aspx?id=" + AddGameAct._gameId + "&step=2");
		}

		protected void back_Step2_Click(object sender, System.EventArgs e)
		{
			base.Response.Redirect("AddGameAct.aspx?id=" + AddGameAct._gameId + "&step=1");
		}

		protected void save_Step2_Click(object sender, System.EventArgs e)
		{
			this.SaveGameStep2();
		}

		protected void back_Step3_Click(object sender, System.EventArgs e)
		{
			base.Response.Redirect("AddGameAct.aspx?id=" + AddGameAct._gameId + "&step=2");
		}

		protected void save_Step3_Click(object sender, System.EventArgs e)
		{
			try
			{
				string text = this.txt_json.Text.Trim();
				if (text.Length <= 0)
				{
					this.ShowMsg("请设定奖品信息！", false);
				}
				else
				{
					System.Collections.Generic.List<GameActPrizeInfo> list = new System.Collections.Generic.List<GameActPrizeInfo>();
					JArray jArray = (JArray)JsonConvert.DeserializeObject(text);
					decimal d = 0m;
					if (jArray.Count > 0)
					{
						for (int i = 0; i < jArray.Count; i++)
						{
							GameActPrizeInfo gameActPrizeInfo = new GameActPrizeInfo();
							int num = int.Parse(jArray[i]["prizeId"].ToString());
							if (num != 0)
							{
								gameActPrizeInfo = GameActHelper.GetPrize(AddGameAct._gameId, num);
							}
							else
							{
								gameActPrizeInfo.Id = 0;
							}
							gameActPrizeInfo.PrizeName = jArray[i]["prizeName"].ToString();
							gameActPrizeInfo.PrizeType = (ePrizeType)int.Parse(jArray[i]["prizeType"].ToString());
							gameActPrizeInfo.GrivePoint = int.Parse(jArray[i]["point"].ToString());
							gameActPrizeInfo.PointNumber = int.Parse(jArray[i]["pointNumber"].ToString());
							gameActPrizeInfo.PointRate = int.Parse(jArray[i]["pointRate"].ToString());
							gameActPrizeInfo.GiveCouponId = int.Parse(jArray[i]["coupon"].ToString());
							gameActPrizeInfo.CouponNumber = int.Parse(jArray[i]["couponNumber"].ToString());
							gameActPrizeInfo.CouponRate = int.Parse(jArray[i]["couponRate"].ToString());
							gameActPrizeInfo.GiveProductId = int.Parse(jArray[i]["product"].ToString());
							gameActPrizeInfo.ProductNumber = int.Parse(jArray[i]["productNumber"].ToString());
							gameActPrizeInfo.ProductRate = int.Parse(jArray[i]["productRate"].ToString());
							gameActPrizeInfo.sort = i + 1;
							gameActPrizeInfo.GameId = AddGameAct._gameId;
							d += gameActPrizeInfo.PointRate + gameActPrizeInfo.CouponRate + gameActPrizeInfo.ProductRate;
							list.Add(gameActPrizeInfo);
						}
					}
					if (d > 100m)
					{
						this.ShowMsg("中奖率总和不能大于100！", false);
					}
					else
					{
						foreach (GameActPrizeInfo current in list)
						{
							if (current.Id != 0)
							{
								GameActHelper.UpdatePrize(current);
							}
							else
							{
								GameActHelper.InsertPrize(current);
							}
						}
					}
				}
			}
			catch (System.Exception)
			{
				this.ShowMsg("保存奖品失败！", false);
			}
		}
	}
}
