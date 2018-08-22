using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	[PrivilegeCheck(Privilege.EditMemberGrade)]
	public class AddMemberGrade : AdminPage
	{
		protected static bool _bAdd = true;

		protected static int _gradeid = 0;

		protected string htmlOperName = "添加";

		protected Script Script4;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txtRankName;

		protected System.Web.UI.WebControls.TextBox txtValue;

		protected System.Web.UI.HtmlControls.HtmlInputCheckBox cbIsDefault;

		protected System.Web.UI.WebControls.TextBox txt_tradeVol;

		protected System.Web.UI.WebControls.TextBox txt_tradeTimes;

		protected System.Web.UI.WebControls.TextBox txtRankDesc;

		protected System.Web.UI.WebControls.Button btnSubmitMemberRanks;

		protected AddMemberGrade() : base("m04", "hyp03")
		{
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSubmitMemberRanks.Click += new System.EventHandler(this.btnSubmitMemberRanks_Click);
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string[] allKeys = base.Request.Params.AllKeys;
			int num = 0;
			if (allKeys.Contains("id"))
			{
				if (this.bInt(base.Request["id"], ref num))
				{
					AddMemberGrade._gradeid = num;
					AddMemberGrade._bAdd = false;
				}
				else
				{
					AddMemberGrade._bAdd = true;
				}
			}
			else
			{
				AddMemberGrade._bAdd = true;
			}
			if (!base.IsPostBack && !AddMemberGrade._bAdd)
			{
				this.htmlOperName = "编辑";
				MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(num);
				if (memberGrade == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				Globals.EntityCoding(memberGrade, false);
				this.txtRankName.Text = memberGrade.Name;
				this.txtRankDesc.Text = memberGrade.Description;
				this.txtValue.Text = memberGrade.Discount.ToString();
				this.txt_tradeVol.Text = memberGrade.TranVol.ToString();
				this.cbIsDefault.Checked = memberGrade.IsDefault;
				this.txt_tradeTimes.Text = memberGrade.TranTimes.ToString();
				if (memberGrade.IsDefault)
				{
					this.cbIsDefault.Disabled = true;
				}
			}
		}

		private bool bContain(string[] arr, string val)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				string text = arr[i];
				if (text.ToLower() == val.ToLower())
				{
					return true;
				}
			}
			return false;
		}

		private bool bInt(string val, ref int i)
		{
			i = 0;
			return !val.Contains(".") && !val.Contains("-") && int.TryParse(val, out i);
		}

		private bool bDouble(string val, ref double i)
		{
			i = 0.0;
			return !val.Contains("-") && double.TryParse(val, out i);
		}

		private void btnSubmitMemberRanks_Click(object sender, System.EventArgs e)
		{
			string text = this.txtRankName.Text;
			string text2 = this.txt_tradeVol.Text;
			string text3 = this.txt_tradeTimes.Text;
			string text4 = this.txtValue.Text;
			string arg_3B_0 = this.txtRankDesc.Text;
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("会员等级名称不能为空！", false);
				return;
			}
			if (text.Length > 20)
			{
				this.ShowMsg("会员等级名称最长20个字符", false);
				return;
			}
			double value = 0.0;
			if (!string.IsNullOrEmpty(text2))
			{
				double num = 0.0;
				if (!this.bDouble(text2, ref num))
				{
					this.ShowMsg("请输入正确的交易额！", false);
					return;
				}
				value = num;
			}
			int value2 = 0;
			if (!string.IsNullOrEmpty(text3))
			{
				int num2 = 0;
				if (!this.bInt(text3, ref num2))
				{
					this.ShowMsg("请输入正确的交易次数！", false);
					return;
				}
				value2 = num2;
			}
			int num3 = 0;
			if (string.IsNullOrEmpty(text4))
			{
				this.ShowMsg("会员折扣不能为空！", false);
				return;
			}
			if (!this.bInt(text4, ref num3))
			{
				this.ShowMsg("会员折扣必须是不大于100的正整数！", false);
				return;
			}
			if (num3 > 100)
			{
				this.ShowMsg("会员折扣必须是不大于100的正整数！", false);
				return;
			}
			MemberGradeInfo memberGradeInfo;
			if (AddMemberGrade._bAdd)
			{
				memberGradeInfo = new MemberGradeInfo();
			}
			else
			{
				memberGradeInfo = MemberHelper.GetMemberGrade(AddMemberGrade._gradeid);
			}
			memberGradeInfo.Name = text;
			memberGradeInfo.Description = this.txtRankDesc.Text.Trim();
			memberGradeInfo.IsDefault = this.cbIsDefault.Checked;
			memberGradeInfo.TranVol = new double?(value);
			memberGradeInfo.TranTimes = new int?(value2);
			memberGradeInfo.Discount = num3;
			if (AddMemberGrade._bAdd && MemberHelper.IsExist(text))
			{
				this.ShowMsg("该等级名称已存在，请修改等级名称", false);
				return;
			}
			if (MemberHelper.HasSameMemberGrade(memberGradeInfo))
			{
				this.ShowMsg("已经存在相同交易额或交易次数的等级，每个会员等级的交易额或交易次数不能相同", false);
				return;
			}
			try
			{
				if (AddMemberGrade._bAdd)
				{
					if (MemberHelper.CreateMemberGrade(memberGradeInfo))
					{
						this.ShowMsgAndReUrl("新增会员等级成功", true, "/Admin/Member/MemberGrades.aspx");
					}
					else
					{
						this.ShowMsg("新增会员等级失败", false);
					}
				}
				else if (MemberHelper.UpdateMemberGrade(memberGradeInfo))
				{
					this.ShowMsgAndReUrl("修改会员等级成功", true, "/Admin/Member/MemberGrades.aspx");
				}
				else
				{
					this.ShowMsg("修改会员等级失败", false);
				}
			}
			catch (System.Exception ex)
			{
				this.ShowMsg("操作失败,原因是：" + ex.Message, false);
			}
		}

		private bool ValidationMemberGrade(MemberGradeInfo memberGrade)
		{
			ValidationResults validationResults = Validation.Validate<MemberGradeInfo>(memberGrade, new string[]
			{
				"ValMemberGrade"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
