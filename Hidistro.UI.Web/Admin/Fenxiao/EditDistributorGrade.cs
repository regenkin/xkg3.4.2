using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class EditDistributorGrade : AdminPage
	{
		protected string ReUrl = "distributorgradelist.aspx";

		private int GradeId;

		protected string htmlOperatorName = "编辑";

		protected System.Web.UI.HtmlControls.HtmlGenericControl EditTitle;

		protected UpImg uploader1;

		protected System.Web.UI.WebControls.TextBox txtName;

		protected System.Web.UI.WebControls.TextBox txtCommissionsLimit;

		protected System.Web.UI.WebControls.TextBox txtFirstCommissionRise;

		protected System.Web.UI.WebControls.TextBox txtSecondCommissionRise;

		protected System.Web.UI.WebControls.TextBox txtThirdCommissionRise;

		protected System.Web.UI.WebControls.TextBox txtDescription;

		protected System.Web.UI.HtmlControls.HtmlGenericControl GIsDefault;

		protected System.Web.UI.WebControls.RadioButtonList rbtnlIsDefault;

		protected System.Web.UI.WebControls.Button btnEditUser;

		protected EditDistributorGrade() : base("m05", "fxp04")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (this.Page.Request.QueryString["ID"] != null)
			{
				if (!int.TryParse(this.Page.Request.QueryString["ID"], out this.GradeId))
				{
					base.GotoResourceNotFound();
					return;
				}
			}
			else
			{
				this.htmlOperatorName = "新增";
			}
			this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
			if (!this.Page.IsPostBack)
			{
				this.LoadDistributorGradeInfo();
			}
		}

		protected void btnEditUser_Click(object sender, System.EventArgs e)
		{
			decimal num = 0.0m;
			decimal firstCommissionRise = 0.0m;
			decimal secondCommissionRise = 0.0m;
			decimal thirdCommissionRise = 0.0m;
			DistributorGradeInfo distributorGradeInfo = new DistributorGradeInfo();
			if (this.GradeId > 0)
			{
				distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(this.GradeId);
			}
			distributorGradeInfo.Name = this.txtName.Text.Trim();
			decimal.TryParse(this.txtCommissionsLimit.Text.Trim(), out num);
			decimal.TryParse(this.txtFirstCommissionRise.Text.Trim(), out firstCommissionRise);
			decimal.TryParse(this.txtSecondCommissionRise.Text.Trim(), out secondCommissionRise);
			decimal.TryParse(this.txtThirdCommissionRise.Text.Trim(), out thirdCommissionRise);
			distributorGradeInfo.CommissionsLimit = num;
			distributorGradeInfo.FirstCommissionRise = firstCommissionRise;
			distributorGradeInfo.SecondCommissionRise = secondCommissionRise;
			distributorGradeInfo.ThirdCommissionRise = thirdCommissionRise;
			distributorGradeInfo.IsDefault = (this.rbtnlIsDefault.SelectedIndex == 0);
			distributorGradeInfo.Description = this.txtDescription.Text.Trim();
			distributorGradeInfo.Ico = this.uploader1.UploadedImageUrl;
			if (DistributorGradeBrower.IsExistsMinAmount(this.GradeId, num))
			{
				this.ShowMsg("已存在相同佣金的分销商等级", false);
				return;
			}
			if (this.GradeId > 0)
			{
				if (DistributorGradeBrower.UpdateDistributor(distributorGradeInfo))
				{
					if (base.Request.QueryString["reurl"] != null)
					{
						this.ReUrl = base.Request.QueryString["reurl"].ToString();
					}
					this.ShowMsgAndReUrl("成功修改了分销商等级", true, this.ReUrl);
					return;
				}
				this.ShowMsg("分销商等级修改失败", false);
				return;
			}
			else
			{
				if (DistributorGradeBrower.CreateDistributorGrade(distributorGradeInfo))
				{
					this.ShowMsgAndReUrl("成功新增了分销商等级", true, this.ReUrl);
					return;
				}
				this.ShowMsg("分销商等级新增失败", false);
				return;
			}
		}

		private bool ValidationMember(MemberInfo member)
		{
			ValidationResults validationResults = Validation.Validate<MemberInfo>(member, new string[]
			{
				"ValMember"
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

		private void LoadDistributorGradeInfo()
		{
			if (this.GradeId > 0)
			{
				DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(this.GradeId);
				if (distributorGradeInfo == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.txtName.Text = distributorGradeInfo.Name;
				this.txtCommissionsLimit.Text = distributorGradeInfo.CommissionsLimit.ToString("F2");
				this.txtFirstCommissionRise.Text = distributorGradeInfo.FirstCommissionRise.ToString();
				this.txtSecondCommissionRise.Text = distributorGradeInfo.SecondCommissionRise.ToString();
				this.txtThirdCommissionRise.Text = distributorGradeInfo.ThirdCommissionRise.ToString();
				this.rbtnlIsDefault.SelectedIndex = (distributorGradeInfo.IsDefault ? 0 : 1);
				if (distributorGradeInfo.IsDefault)
				{
					this.GIsDefault.Style.Add("display", "none");
				}
				if (distributorGradeInfo.IsDefault)
				{
					this.rbtnlIsDefault.Enabled = false;
				}
				this.txtDescription.Text = distributorGradeInfo.Description;
				string ico = distributorGradeInfo.Ico;
				if (ico != "/utility/pics/grade.png")
				{
					this.uploader1.UploadedImageUrl = ico;
				}
			}
		}
	}
}
