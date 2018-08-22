using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	public class setScore_sign : AdminPage
	{
		protected static bool _signEnable = false;

		protected static bool _continuityEnable = false;

		protected static string _urlType = "sign";

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.HiddenField hdContinuityEnable;

		protected System.Web.UI.WebControls.TextBox txtEverDayScore;

		protected System.Web.UI.WebControls.TextBox txtStraightDay;

		protected System.Web.UI.WebControls.TextBox txt_sign_RewardScore;

		protected System.Web.UI.WebControls.Button btn_signSave;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btn_signSave.Click += new System.EventHandler(this.btn_signSave_Click);
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("type"))
			{
				setScore_sign._urlType = base.Request["type"].ToString();
			}
			else
			{
				setScore_sign._urlType = "sign";
			}
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.txtEverDayScore.Text = masterSettings.SignPoint.ToString();
				this.txtStraightDay.Text = masterSettings.SignWhere.ToString();
				this.txt_sign_RewardScore.Text = masterSettings.SignWherePoint.ToString();
				setScore_sign._signEnable = masterSettings.sign_score_Enable;
				setScore_sign._continuityEnable = masterSettings.open_signContinuity;
			}
		}

		protected setScore_sign() : base("m04", "hyp07")
		{
		}

		private bool bInt(string val, ref int i)
		{
			i = 0;
			return !val.Contains("-") && int.TryParse(val, out i);
		}

		private bool bDouble(string val, ref double i)
		{
			i = 0.0;
			return !val.Contains("-") && double.TryParse(val, out i);
		}

		protected void btn_signSave_Click(object sender, System.EventArgs e)
		{
			int signPoint = 0;
			int signWhere = 0;
			int signWherePoint = 0;
			string text = this.txtEverDayScore.Text;
			string text2 = this.txtStraightDay.Text;
			string text3 = this.txt_sign_RewardScore.Text;
			setScore_sign._urlType = "sign";
			if (!string.IsNullOrEmpty(text))
			{
				if (!this.bInt(text, ref signPoint))
				{
					this.ShowMsg("请输入正确的每日签到积分！", false);
				}
			}
			else
			{
				signPoint = 0;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				if (!this.bInt(text2, ref signWhere))
				{
					this.ShowMsg("请输入正确的连续签到天数！", false);
				}
			}
			else
			{
				signWhere = 0;
			}
			if (!string.IsNullOrEmpty(text3))
			{
				if (!this.bInt(text3, ref signWherePoint))
				{
					this.ShowMsg("请输入正确的连续签到奖励积分！", false);
				}
			}
			else
			{
				signWherePoint = 0;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.SignPoint = signPoint;
			masterSettings.SignWhere = signWhere;
			masterSettings.SignWherePoint = signWherePoint;
			masterSettings.open_signContinuity = (this.hdContinuityEnable.Value == "1");
			SettingsManager.Save(masterSettings);
			setScore_sign._continuityEnable = (this.hdContinuityEnable.Value == "1");
			this.ShowMsg("保存成功", true);
		}
	}
}
