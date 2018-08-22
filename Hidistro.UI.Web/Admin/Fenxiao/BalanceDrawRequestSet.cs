using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class BalanceDrawRequestSet : AdminPage
	{
		protected Hidistro.UI.Common.Controls.Style Style1;

		protected Script Script4;

		protected System.Web.UI.WebControls.CheckBoxList DrawPayType;

		protected System.Web.UI.HtmlControls.HtmlGenericControl alipaypanel;

		protected System.Web.UI.HtmlControls.HtmlInputCheckBox alipayCheck;

		protected System.Web.UI.HtmlControls.HtmlGenericControl weipaypanel;

		protected System.Web.UI.HtmlControls.HtmlInputCheckBox weixinPayCheck;

		protected System.Web.UI.WebControls.RadioButtonList CheckRealName;

		protected System.Web.UI.WebControls.TextBox txtApplySet;

		protected System.Web.UI.WebControls.Button btnSave;

		protected BalanceDrawRequestSet() : base("m05", "fxp06")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.alipaypanel.Style.Add("display", "none");
				this.weipaypanel.Style.Add("display", "none");
				this.txtApplySet.Text = masterSettings.MentionNowMoney;
				string drawPayType = masterSettings.DrawPayType;
				if (drawPayType != "")
				{
					string[] array = drawPayType.Split(new char[]
					{
						'|'
					});
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string a = array2[i];
						if (a != "")
						{
							foreach (System.Web.UI.WebControls.ListItem listItem in this.DrawPayType.Items)
							{
								if (a == listItem.Value)
								{
									listItem.Selected = true;
								}
							}
							if (a == "0")
							{
								this.weipaypanel.Style["display"] = "";
							}
							else if (a == "1")
							{
								this.alipaypanel.Style["display"] = "";
							}
						}
					}
				}
				int batchWeixinPayCheckRealName = masterSettings.BatchWeixinPayCheckRealName;
				this.CheckRealName.SelectedValue = batchWeixinPayCheckRealName.ToString();
				if (masterSettings.BatchWeixinPay)
				{
					this.weixinPayCheck.Checked = true;
				}
				if (masterSettings.BatchAliPay)
				{
					this.alipayCheck.Checked = true;
				}
			}
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			string text = "";
			foreach (System.Web.UI.WebControls.ListItem listItem in this.DrawPayType.Items)
			{
				if (listItem.Selected)
				{
					text = text + "|" + listItem.Value;
					if (listItem.Value == "0")
					{
						this.weipaypanel.Style["display"] = "";
					}
					else if (listItem.Value == "1")
					{
						this.alipaypanel.Style["display"] = "";
					}
				}
			}
			if (text == "")
			{
				this.ShowMsg("至少选择一种提现支付方式额!", false);
				return;
			}
			int num = 0;
			if (!int.TryParse(this.txtApplySet.Text.Trim(), out num))
			{
				this.ShowMsg("最少提现金额，请填写正整数！!", false);
				return;
			}
			text = text.Remove(0, 1);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.DrawPayType = text;
			masterSettings.MentionNowMoney = this.txtApplySet.Text.Trim();
			masterSettings.BatchWeixinPay = this.weixinPayCheck.Checked;
			masterSettings.BatchAliPay = this.alipayCheck.Checked;
			if (!this.DrawPayType.Items[0].Selected)
			{
				masterSettings.BatchAliPay = false;
			}
			if (!this.DrawPayType.Items[1].Selected)
			{
				masterSettings.BatchWeixinPay = false;
			}
			masterSettings.BatchWeixinPayCheckRealName = int.Parse(this.CheckRealName.SelectedValue);
			SettingsManager.Save(masterSettings);
			this.ShowMsg("修改成功", true);
		}
	}
}
