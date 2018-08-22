using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class DistributorFrozenList : AdminPage
	{
		private string StoreName = "";

		private string Grade = "0";

		private string Status = "1";

		private string RealName = "";

		private string CellPhone = "";

		private string MicroSignal = "";

		protected System.Web.UI.WebControls.Literal ListActive;

		protected System.Web.UI.WebControls.Literal Listfrozen;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txtStoreName;

		protected System.Web.UI.WebControls.TextBox txtRealName;

		protected System.Web.UI.WebControls.TextBox txtMicroSignal;

		protected System.Web.UI.WebControls.TextBox txtCellPhone;

		protected DistributorGradeDropDownList DrGrade;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected System.Web.UI.WebControls.Button UnFrozenCheck;

		protected System.Web.UI.WebControls.Button CancleCheck;

		protected System.Web.UI.WebControls.HyperLink btnDownTaobao;

		protected System.Web.UI.WebControls.Repeater reDistributor;

		protected Pager pager;

		protected DistributorGradeDropDownList GradeCheckList;

		protected System.Web.UI.WebControls.Button GradeCheck;

		protected System.Web.UI.WebControls.TextBox txtPassword;

		protected System.Web.UI.WebControls.TextBox txtConformPassword;

		protected System.Web.UI.WebControls.Button PassCheck;

		protected System.Web.UI.WebControls.HiddenField EditUserID;

		protected System.Web.UI.WebControls.TextBox EdittxtRealname;

		protected System.Web.UI.WebControls.TextBox EdittxtCellPhone;

		protected System.Web.UI.WebControls.TextBox EdittxtQQNum;

		protected System.Web.UI.WebControls.TextBox EdittxtPassword;

		protected System.Web.UI.WebControls.Button EditSave;

		protected DistributorFrozenList() : base("m05", "fxp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.reDistributor.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.reDistributor_ItemCommand);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.UnFrozenCheck.Click += new System.EventHandler(this.UnFrozenCheck_Click);
			this.CancleCheck.Click += new System.EventHandler(this.CancleCheck_Click);
			this.PassCheck.Click += new System.EventHandler(this.PassCheck_Click);
			this.GradeCheck.Click += new System.EventHandler(this.GradeCheck_Click);
			this.EditSave.Click += new System.EventHandler(this.EditSave_Click);
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindData();
				this.GradeCheckList.DataBind();
				this.DrGrade.DataBind();
				this.DrGrade.SelectedValue = new int?(int.Parse(this.Grade));
			}
		}

		private void EditSave_Click(object sender, System.EventArgs e)
		{
			string text = this.EditUserID.Value.Trim();
			if (text.Length <= 0)
			{
				this.ShowMsg("用户ID为空，参数异常！", false);
				return;
			}
			string text2 = this.EdittxtPassword.Text.Trim();
			if (text2.Length > 0 && (text2.Length > 20 || text2.Length < 6))
			{
				this.ShowMsg("用户密码长度在6-20位之间！", false);
				return;
			}
			if (DistributorsBrower.EditDisbutosInfos(text, this.EdittxtQQNum.Text, this.EdittxtCellPhone.Text, this.EdittxtRealname.Text, HiCryptographer.Md5Encrypt(text2)))
			{
				this.ReBind(true);
				return;
			}
			this.ShowMsg("成功用户信息失败", false);
		}

		private void PassCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要修改密码的分销商", false);
				return;
			}
			if (this.txtPassword.Text.Trim().Length < 6 || this.txtPassword.Text.Trim().Length > 20)
			{
				this.ShowMsg("密码长度在6-20位之间！", false);
				return;
			}
			if (this.txtPassword.Text != this.txtConformPassword.Text)
			{
				this.ShowMsg("两次输入密码不一致！", false);
				return;
			}
			int num = MemberProcessor.SetMultiplePwd(text, HiCryptographer.Md5Encrypt(this.txtPassword.Text.Trim()));
			this.ShowMsg(string.Format("成功修改了{0}个分销商的密码", num), true);
		}

		private void GradeCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			string grade = this.GradeCheckList.SelectedValue.ToString();
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要修改等级的分销商", false);
				return;
			}
			int num = DistributorsBrower.EditCommisionsGrade(text, grade);
			this.ShowMsg(string.Format("成功修改了{0}个分销商的等级", num), true);
			this.ReBind(true);
		}

		private void CancleCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要取消资质的分销商", false);
				return;
			}
			int num = DistributorsBrower.FrozenCommisionChecks(text, "9");
			this.ShowMsg(string.Format("成功取消了{0}个分销商的资质", num), true);
			this.ReBind(true);
		}

		private void UnFrozenCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要解冻的分销商", false);
				return;
			}
			int num = DistributorsBrower.FrozenCommisionChecks(text, "0");
			this.ShowMsg(string.Format("成功解冻了{0}分销商", num), true);
			this.ReBind(true);
		}

		private void reDistributor_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Frozen")
			{
				if (!DistributorsBrower.FrozenCommision(int.Parse(e.CommandArgument.ToString()), "1"))
				{
					this.ShowMsg("冻结失败", false);
					return;
				}
				this.ShowMsg("冻结成功", true);
				this.ReBind(true);
			}
			if (e.CommandName == "Thaw")
			{
				if (!DistributorsBrower.FrozenCommision(int.Parse(e.CommandArgument.ToString()), "0"))
				{
					this.ShowMsg("解冻失败", false);
					return;
				}
				this.ShowMsg("解冻成功", true);
				this.ReBind(true);
			}
			if (e.CommandName == "Forbidden")
			{
				if (DistributorsBrower.FrozenCommision(int.Parse(e.CommandArgument.ToString()), "9"))
				{
					this.ShowMsg("取消资质成功！", true);
					this.ReBind(true);
					return;
				}
				this.ShowMsg("取消资质失败", false);
			}
		}

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
				{
					this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Grade"]))
				{
					this.Grade = base.Server.UrlDecode(this.Page.Request.QueryString["Grade"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Status"]))
				{
					this.Status = base.Server.UrlDecode(this.Page.Request.QueryString["Status"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RealName"]))
				{
					this.RealName = base.Server.UrlDecode(this.Page.Request.QueryString["RealName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CellPhone"]))
				{
					this.CellPhone = base.Server.UrlDecode(this.Page.Request.QueryString["CellPhone"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["MicroSignal"]))
				{
					this.MicroSignal = base.Server.UrlDecode(this.Page.Request.QueryString["MicroSignal"]);
				}
				this.txtStoreName.Text = this.StoreName;
				this.DrGrade.SelectedValue = new int?(int.Parse(this.Grade));
				this.txtCellPhone.Text = this.CellPhone;
				this.txtMicroSignal.Text = this.MicroSignal;
				this.txtRealName.Text = this.RealName;
				return;
			}
			this.StoreName = this.txtStoreName.Text;
			this.Grade = this.DrGrade.SelectedValue.ToString();
			this.CellPhone = this.txtCellPhone.Text;
			this.RealName = this.txtRealName.Text;
			this.MicroSignal = this.txtMicroSignal.Text;
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("Grade", this.DrGrade.Text);
			nameValueCollection.Add("StoreName", this.txtStoreName.Text);
			nameValueCollection.Add("CellPhone", this.txtCellPhone.Text);
			nameValueCollection.Add("RealName", this.txtRealName.Text);
			nameValueCollection.Add("MicroSignal", this.txtMicroSignal.Text);
			nameValueCollection.Add("Status", this.Status);
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}

		private void BindData()
		{
			DistributorsQuery distributorsQuery = new DistributorsQuery();
			distributorsQuery.GradeId = int.Parse(this.Grade);
			distributorsQuery.StoreName = this.StoreName;
			distributorsQuery.CellPhone = this.CellPhone;
			distributorsQuery.RealName = this.RealName;
			distributorsQuery.MicroSignal = this.MicroSignal;
			distributorsQuery.ReferralStatus = int.Parse(this.Status);
			distributorsQuery.PageIndex = this.pager.PageIndex;
			distributorsQuery.PageSize = this.pager.PageSize;
			distributorsQuery.SortOrder = SortAction.Desc;
			distributorsQuery.SortBy = "userid";
			Globals.EntityCoding(distributorsQuery, true);
			DbQueryResult distributors = VShopHelper.GetDistributors(distributorsQuery, null, null);
			this.reDistributor.DataSource = distributors.Data;
			this.reDistributor.DataBind();
			this.pager.TotalRecords = distributors.TotalRecords;
			System.Data.DataTable distributorsNum = VShopHelper.GetDistributorsNum();
			this.ListActive.Text = "分销商列表(" + distributorsNum.Rows[0]["active"].ToString() + ")";
			this.Listfrozen.Text = "已冻结(" + distributorsNum.Rows[0]["frozen"].ToString() + ")";
		}
	}
}
