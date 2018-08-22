using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	public class setScore_shopping : AdminPage
	{
		protected static bool _shoppingEnable = false;

		protected static string _urlType = "shopping";

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txt_ShoppingScore;

		protected System.Web.UI.WebControls.CheckBox chk;

		protected System.Web.UI.WebControls.TextBox txt_OrderValue;

		protected System.Web.UI.WebControls.TextBox txt_shopping_RewardScore;

		protected System.Web.UI.WebControls.Button btn_shoppingSave;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btn_shoppingSave.Click += new System.EventHandler(this.btn_shoppingSave_Click);
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("type"))
			{
				setScore_shopping._urlType = base.Request["type"].ToString();
			}
			else
			{
				setScore_shopping._urlType = "shopping";
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			setScore_shopping._shoppingEnable = masterSettings.shopping_score_Enable;
			if (!base.IsPostBack)
			{
				this.txt_ShoppingScore.Text = masterSettings.PointsRate.ToString("f0");
				this.chk.Checked = masterSettings.shopping_reward_Enable;
				this.txt_OrderValue.Text = masterSettings.shopping_reward_OrderValue.ToString("F2");
				this.txt_shopping_RewardScore.Text = masterSettings.shopping_reward_Score.ToString();
			}
		}

		protected setScore_shopping() : base("m04", "hyp06")
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

		protected void btn_shoppingSave_Click(object sender, System.EventArgs e)
		{
			setScore_shopping._urlType = "shopping";
			int value = 0;
			double shopping_reward_OrderValue = 0.0;
			int shopping_reward_Score = 0;
			bool @checked = this.chk.Checked;
			string text = this.txt_ShoppingScore.Text;
			string text2 = this.txt_OrderValue.Text;
			string text3 = this.txt_shopping_RewardScore.Text;
			if (!string.IsNullOrEmpty(text))
			{
				if (!this.bInt(text, ref value))
				{
					this.ShowMsg("请输入正确的购物积分！", false);
				}
			}
			else
			{
				value = 0;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				if (!this.bDouble(text2, ref shopping_reward_OrderValue))
				{
					this.ShowMsg("请输入正确的单笔订单价值！", false);
				}
			}
			else
			{
				shopping_reward_OrderValue = 0.0;
			}
			if (!string.IsNullOrEmpty(text3))
			{
				if (!this.bInt(text3, ref shopping_reward_Score))
				{
					this.ShowMsg("请输入正确的购物奖励积分！", false);
				}
			}
			else
			{
				shopping_reward_Score = 0;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.PointsRate = value;
			masterSettings.shopping_reward_Enable = @checked;
			masterSettings.shopping_reward_OrderValue = shopping_reward_OrderValue;
			masterSettings.shopping_reward_Score = shopping_reward_Score;
			SettingsManager.Save(masterSettings);
			this.ShowMsgAndReUrl("保存成功", true, "/Admin/member/setScore_shopping.aspx");
		}
	}
}
