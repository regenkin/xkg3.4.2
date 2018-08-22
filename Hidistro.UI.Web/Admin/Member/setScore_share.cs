using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	public class setScore_share : AdminPage
	{
		protected static bool _shareEnable = false;

		protected static string _urlType = "share";

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txt_ShareScore;

		protected System.Web.UI.WebControls.Button btn_shareSave;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btn_shareSave.Click += new System.EventHandler(this.btn_shareSave_Click);
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("type"))
			{
				setScore_share._urlType = base.Request["type"].ToString();
			}
			else
			{
				setScore_share._urlType = "share";
			}
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.txt_ShareScore.Text = masterSettings.share_Score.ToString();
				setScore_share._shareEnable = masterSettings.share_score_Enable;
			}
		}

		protected setScore_share() : base("m04", "hyp08")
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

		protected void btn_shareSave_Click(object sender, System.EventArgs e)
		{
			setScore_share._urlType = "share";
			int share_Score = 0;
			string text = this.txt_ShareScore.Text;
			if (!string.IsNullOrEmpty(text))
			{
				if (!this.bInt(text, ref share_Score))
				{
					this.ShowMsg("请输入正确的分享积分！", false);
				}
			}
			else
			{
				share_Score = 0;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.share_Score = share_Score;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("保存成功", true);
		}
	}
}
