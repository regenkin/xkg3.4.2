using Ajax;
using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.StatisticsReport;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class DistributorList : AdminPage
	{
		private string StoreName = "";

		private string StoreName1 = "";

		private string Grade = "0";

		private string Status = "0";

		private string RealName = "";

		private string CellPhone = "";

		private string MicroSignal = "";

		protected string localUrl = string.Empty;

		private StatisticNotifier myNotifier = new StatisticNotifier();

		private UpdateStatistics myEvent = new UpdateStatistics();

		protected System.Web.UI.WebControls.Literal ListActive;

		protected System.Web.UI.WebControls.Literal Listfrozen;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txtStoreName;

		protected System.Web.UI.WebControls.TextBox txtRealName;

		protected System.Web.UI.WebControls.TextBox txtStoreName1;

		protected System.Web.UI.WebControls.TextBox txtMicroSignal;

		protected System.Web.UI.WebControls.TextBox txtCellPhone;

		protected DistributorGradeDropDownList DrGrade;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected System.Web.UI.WebControls.Button FrozenCheck;

		protected System.Web.UI.WebControls.Button CancleCheck;

		protected System.Web.UI.WebControls.HyperLink btnDownTaobao;

		protected System.Web.UI.WebControls.Repeater reDistributor;

		protected Pager pager;

		protected DistributorGradeDropDownList GradeCheckList;

		protected System.Web.UI.WebControls.Button GradeCheck;

		protected System.Web.UI.WebControls.TextBox txtPassword;

		protected System.Web.UI.WebControls.TextBox txtConformPassword;

		protected System.Web.UI.WebControls.Button PassCheck;

		protected System.Web.UI.WebControls.Label lboldCommission;

		protected System.Web.UI.HtmlControls.HtmlInputText txtCommission;

		protected System.Web.UI.HtmlControls.HtmlInputText txtSetCommissionBark;

		protected System.Web.UI.HtmlControls.HtmlInputText txtUserId;

		protected System.Web.UI.HtmlControls.HtmlInputText txtoldCommission;

		protected System.Web.UI.WebControls.Button UpdateCommission;

		protected System.Web.UI.WebControls.HiddenField EditUserID;

		protected System.Web.UI.WebControls.TextBox EdittxtRealname;

		protected System.Web.UI.WebControls.TextBox EdittxtCellPhone;

		protected System.Web.UI.WebControls.TextBox EdittxtQQNum;

		protected System.Web.UI.WebControls.TextBox EdittxtPassword;

		protected System.Web.UI.WebControls.Button EditSave;

		protected DistributorList() : base("m05", "fxp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Utility.RegisterTypeForAjax(typeof(DistributorList));
			this.localUrl = base.Request.Url.ToString();
			string text = base.Request.QueryString["task"];
			if (!string.IsNullOrEmpty(text))
			{
				string s = "{error:1,msg:'未定义操作！'}";
				if (text == "readinfo")
				{
					int num;
					if (int.TryParse(this.Page.Request.QueryString["UserId"], out num))
					{
						DistributorsQuery distributorsQuery = new DistributorsQuery();
						distributorsQuery.UserId = int.Parse(this.Page.Request.QueryString["UserId"]);
						distributorsQuery.ReferralStatus = -1;
						distributorsQuery.PageIndex = 1;
						distributorsQuery.PageSize = 1;
						distributorsQuery.SortOrder = SortAction.Desc;
						distributorsQuery.SortBy = "userid";
						Globals.EntityCoding(distributorsQuery, true);
						DbQueryResult distributors = VShopHelper.GetDistributors(distributorsQuery, null, null);
						if (distributors.Data != null)
						{
							System.Data.DataTable value = new System.Data.DataTable();
							value = (System.Data.DataTable)distributors.Data;
							s = "{error:0,Data:" + JsonConvert.SerializeObject(value) + "}";
						}
						else
						{
							s = "{error:1,msg:'分销商信息不存在'}";
						}
					}
					else
					{
						s = "{error:1,msg:'userid错误'}";
					}
				}
				base.Response.Write(s);
				base.Response.End();
			}
			this.reDistributor.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.reDistributor_ItemCommand);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.FrozenCheck.Click += new System.EventHandler(this.FrozenCheck_Click);
			this.CancleCheck.Click += new System.EventHandler(this.CancleCheck_Click);
			this.PassCheck.Click += new System.EventHandler(this.PassCheck_Click);
			this.GradeCheck.Click += new System.EventHandler(this.GradeCheck_Click);
			this.EditSave.Click += new System.EventHandler(this.EditSave_Click);
			this.UpdateCommission.Click += new System.EventHandler(this.UpdateCommission_Click);
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
			this.EditPasswordSendWeiXinMessage(text, this.txtPassword.Text.Trim());
			this.ShowMsg(string.Format("成功修改了{0}个分销商的密码", num), true);
		}

		private void EditPasswordSendWeiXinMessage(string userIds, string password)
		{
			try
			{
				System.Collections.Generic.List<MemberInfo> memberInfoList = MemberProcessor.GetMemberInfoList(userIds);
				if (memberInfoList != null && memberInfoList.Count > 0)
				{
					foreach (MemberInfo current in memberInfoList)
					{
						if (current != null)
						{
							Messenger.SendWeiXinMsg_PasswordReset(current, password);
						}
					}
				}
			}
			catch (System.Exception)
			{
			}
		}

		private void GradeCheck_Click(object sender, System.EventArgs e)
		{
			string strIds = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				strIds = base.Request["CheckBoxGroup"];
			}
			string EditGrade = this.GradeCheckList.SelectedValue.ToString();
			if (strIds.Length <= 0)
			{
				this.ShowMsg("请先选择要修改等级的分销商", false);
				return;
			}
			int num = DistributorsBrower.EditCommisionsGrade(strIds, EditGrade);
			if (num > 0)
			{
				new System.Threading.Thread(()=>
				{
					DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(int.Parse(EditGrade));
					if (distributorGradeInfo != null)
					{
						string[] array = strIds.Split(new char[]
						{
							','
						});
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string s = array2[i];
							int userId = 0;
							if (int.TryParse(s, out userId))
							{
								try
								{
									MemberInfo member = MemberProcessor.GetMember(userId, true);
									if (member != null)
									{
										Messenger.SendWeiXinMsg_DistributorGradeChange(member, distributorGradeInfo.Name);
									}
								}
								catch (System.Exception ex)
								{
									Globals.Debuglog("升级变动提醒发送错误：" + ex.Message + "-- " + strIds, "_Debuglog.txt");
								}
							}
						}
					}
				}).Start();
			}
			this.ShowMsg(string.Format("成功修改了{0}个分销商的等级", num), true);
			this.BindData();
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
			try
			{
				this.myNotifier.updateAction = UpdateAction.MemberUpdate;
				this.myNotifier.actionDesc = "批量取消分销商资质";
				this.myNotifier.RecDateUpdate = System.DateTime.Today;
				this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
				this.myNotifier.UpdateDB();
			}
			catch (System.Exception)
			{
			}
			this.ShowMsg(string.Format("成功取消了{0}个分销商的资质", num), true);
			this.ReBind(true);
		}

		private void FrozenCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要冻结的分销商", false);
				return;
			}
			int num = DistributorsBrower.FrozenCommisionChecks(text, "1");
			this.ShowMsg(string.Format("成功冻结了{0}个分销商", num), true);
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
					try
					{
						this.myNotifier.updateAction = UpdateAction.MemberUpdate;
						this.myNotifier.actionDesc = "取消分销商资质";
						this.myNotifier.RecDateUpdate = System.DateTime.Today;
						this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
						this.myNotifier.UpdateDB();
					}
					catch (System.Exception)
					{
					}
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
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName1"]))
				{
					this.StoreName1 = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName1"]);
				}
				this.txtStoreName1.Text = this.StoreName1;
				this.txtStoreName.Text = this.StoreName;
				this.DrGrade.SelectedValue = new int?(int.Parse(this.Grade));
				this.txtCellPhone.Text = this.CellPhone;
				this.txtMicroSignal.Text = this.MicroSignal;
				this.txtRealName.Text = this.RealName;
				return;
			}
			this.StoreName1 = this.txtStoreName1.Text;
			this.StoreName = this.txtStoreName.Text;
			this.Grade = this.DrGrade.SelectedValue.ToString();
			this.CellPhone = this.txtCellPhone.Text;
			this.RealName = this.txtRealName.Text;
			this.MicroSignal = this.txtMicroSignal.Text;
			if (string.IsNullOrEmpty(this.Grade))
			{
				this.Grade = "0";
			}
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("Grade", this.DrGrade.Text);
			nameValueCollection.Add("StoreName", this.txtStoreName.Text);
			nameValueCollection.Add("StoreName1", this.txtStoreName1.Text);
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
			distributorsQuery.StoreName1 = this.StoreName1;
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

		private void UpdateCommission_Click(object sender, System.EventArgs e)
		{
			int userId = System.Convert.ToInt32(this.txtUserId.Value.Trim());
			ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
			decimal num = 0m;
			try
			{
				num = System.Convert.ToDecimal(this.txtCommission.Value.Trim());
			}
			catch
			{
				this.ShowMsg("佣金值应为数字", false);
				return;
			}
			decimal d = System.Convert.ToDecimal(this.txtoldCommission.Value.Trim());
			if (d + num < 0m)
			{
				this.ShowMsg("减去佣金不能大于当前佣金", false);
			}
			else
			{
				string text = this.txtSetCommissionBark.Value.Trim();
				if (string.IsNullOrEmpty(text))
				{
					this.ShowMsg("备注不能为空", false);
					return;
				}
				if (text.Length > 200)
				{
					this.ShowMsg("备注内容过长", false);
					return;
				}
				text = currentManager.UserName + ":手动调整佣金：" + text;
				if (VShopHelper.UpdateCommission(userId, num, text))
				{
					this.BindData();
					this.ShowMsg("成功调整佣金", true);
					this.txtCommission.Value = "";
					this.txtSetCommissionBark.Value = "";
					return;
				}
				this.ShowMsg("调整佣金失败", false);
				this.txtCommission.Value = "";
				this.txtSetCommissionBark.Value = "";
				return;
			}
		}
	}
}
