using Hidistro.ControlPanel.Members;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	public class CustomDistributorAddMembers : AdminPage
	{
		protected string localUrl = string.Empty;

		protected int currentGroupId;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Literal GroupName;

		protected TrimTextBox txtStroeName;

		protected TrimTextBox txtTradeMoney1;

		protected TrimTextBox txtTradeMoney2;

		protected TrimTextBox txtTradeNum1;

		protected TrimTextBox txtTradeNum2;

		protected System.Web.UI.WebControls.Button btnSelect;

		protected System.Web.UI.HtmlControls.HtmlGenericControl resultDiv;

		protected System.Web.UI.WebControls.Literal litMembersNum;

		protected System.Web.UI.WebControls.Button btnJoin;

		protected System.Web.UI.WebControls.HiddenField hdGrades;

		protected System.Web.UI.WebControls.HiddenField hdDefualtGroup;

		protected System.Web.UI.WebControls.HiddenField hdCustomGroup;

		protected System.Web.UI.WebControls.HiddenField hdRegisterDate;

		protected System.Web.UI.WebControls.HiddenField hdTradeDate;

		protected CustomDistributorAddMembers() : base("m04", "hyp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.localUrl = base.Request.Url.ToString();
			if (!int.TryParse(this.Page.Request.QueryString["GroupId"], out this.currentGroupId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
			if (!base.IsPostBack)
			{
				CustomGroupingInfo groupInfoById = CustomGroupingHelper.GetGroupInfoById(this.currentGroupId);
				if (groupInfoById != null)
				{
					this.GroupName.Text = groupInfoById.GroupName;
					this.resultDiv.Visible = false;
					return;
				}
				this.ShowMsgAndReUrl("参数错误！", false, "CustomDistributorList.aspx");
			}
		}

		protected void btnJoin_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<int> memberList = CustomGroupingHelper.GetMemberList(this.GetMemberQuery());
			if (memberList != null)
			{
				CustomGroupingHelper.AddCustomGroupingUser(memberList, this.currentGroupId);
				this.ShowMsgAndReUrl("添加成功！", true, "/Admin/member/CustomDistributorDetail.aspx?GroupId=" + this.currentGroupId);
				return;
			}
			this.ShowMsg("未找到符合条件的会员，请重新选择条件！", false);
		}

		protected void btnSelect_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<int> memberList = CustomGroupingHelper.GetMemberList(this.GetMemberQuery());
			if (memberList != null)
			{
				this.resultDiv.Visible = true;
				this.litMembersNum.Text = memberList.Count.ToString();
			}
		}

		private MemberQuery GetMemberQuery()
		{
			MemberQuery memberQuery = new MemberQuery();
			if (!string.IsNullOrEmpty(this.txtStroeName.Text))
			{
				memberQuery.StoreName = this.txtStroeName.Text.Trim();
			}
			if (!string.IsNullOrEmpty(this.txtTradeMoney1.Text))
			{
				decimal value = 0m;
				if (decimal.TryParse(this.txtTradeMoney1.Text, out value))
				{
					memberQuery.TradeMoneyStart = new decimal?(value);
				}
			}
			if (!string.IsNullOrEmpty(this.txtTradeMoney2.Text))
			{
				decimal value2 = 0m;
				if (decimal.TryParse(this.txtTradeMoney2.Text, out value2))
				{
					memberQuery.TradeMoneyEnd = new decimal?(value2);
				}
			}
			if (!string.IsNullOrEmpty(this.txtTradeNum1.Text))
			{
				int value3 = 0;
				if (int.TryParse(this.txtTradeNum1.Text, out value3))
				{
					memberQuery.TradeNumStart = new int?(value3);
				}
			}
			if (!string.IsNullOrEmpty(this.txtTradeNum2.Text))
			{
				int value4 = 0;
				if (int.TryParse(this.txtTradeNum2.Text, out value4))
				{
					memberQuery.TradeNumEnd = new int?(value4);
				}
			}
			memberQuery.Stutas = new UserStatus?(UserStatus.Normal);
			if (!string.IsNullOrEmpty(this.hdGrades.Value) && !this.hdGrades.Value.Equals("-1"))
			{
				memberQuery.GradeIds = this.hdGrades.Value;
			}
			if (!string.IsNullOrEmpty(this.hdDefualtGroup.Value) && !this.hdDefualtGroup.Value.Equals("-1"))
			{
				memberQuery.ClientType = this.hdDefualtGroup.Value;
			}
			if (!string.IsNullOrEmpty(this.hdCustomGroup.Value) && !this.hdCustomGroup.Value.Equals("-1"))
			{
				memberQuery.GroupIds = this.hdCustomGroup.Value;
			}
			string value5;
			if (!string.IsNullOrEmpty(this.hdRegisterDate.Value) && !this.hdRegisterDate.Value.Equals("all") && (value5 = this.hdRegisterDate.Value) != null)
			{
				if (!(value5 == "week"))
				{
					if (!(value5 == "month"))
					{
						if (!(value5 == "threeMonth"))
						{
							if (value5 == "moreMonth")
							{
								memberQuery.RegisterEndTime = new System.DateTime?(System.DateTime.Now.AddMonths(-3));
							}
						}
						else
						{
							memberQuery.RegisterStartTime = new System.DateTime?(System.DateTime.Now.AddMonths(-3));
							memberQuery.RegisterEndTime = new System.DateTime?(System.DateTime.Now);
						}
					}
					else
					{
						memberQuery.RegisterStartTime = new System.DateTime?(System.DateTime.Now.AddMonths(-1));
						memberQuery.RegisterEndTime = new System.DateTime?(System.DateTime.Now);
					}
				}
				else
				{
					memberQuery.RegisterStartTime = new System.DateTime?(System.DateTime.Now.AddDays(-7.0));
					memberQuery.RegisterEndTime = new System.DateTime?(System.DateTime.Now);
				}
			}
			string value6;
			if (!string.IsNullOrEmpty(this.hdTradeDate.Value) && !this.hdTradeDate.Value.Equals("all") && (value6 = this.hdTradeDate.Value) != null)
			{
				if (!(value6 == "week"))
				{
					if (!(value6 == "month"))
					{
						if (!(value6 == "threeMonth"))
						{
							if (value6 == "moreMonth")
							{
								memberQuery.EndTime = new System.DateTime?(System.DateTime.Now.AddMonths(-3));
							}
						}
						else
						{
							memberQuery.StartTime = new System.DateTime?(System.DateTime.Now.AddMonths(-3));
							memberQuery.EndTime = new System.DateTime?(System.DateTime.Now);
						}
					}
					else
					{
						memberQuery.StartTime = new System.DateTime?(System.DateTime.Now.AddMonths(-1));
						memberQuery.EndTime = new System.DateTime?(System.DateTime.Now);
					}
				}
				else
				{
					memberQuery.StartTime = new System.DateTime?(System.DateTime.Now.AddDays(-7.0));
					memberQuery.EndTime = new System.DateTime?(System.DateTime.Now);
				}
			}
			return memberQuery;
		}

		protected string GetMemberGrande()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Collections.Generic.IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
			if (memberGrades != null && memberGrades.Count > 0)
			{
				foreach (MemberGradeInfo current in memberGrades)
				{
					stringBuilder.Append(" <label class=\"middle mr20\">");
					stringBuilder.AppendFormat("<input type=\"checkbox\" class=\"memberGradeCheck\" value=\"{0}\">{1}", current.GradeId, current.Name);
					stringBuilder.Append("  </label>");
				}
			}
			return stringBuilder.ToString();
		}

		protected string GetMemberCustomGroup()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Collections.Generic.IList<CustomGroupingInfo> customGroupingList = CustomGroupingHelper.GetCustomGroupingList();
			if (customGroupingList != null && customGroupingList.Count > 0)
			{
				foreach (CustomGroupingInfo current in customGroupingList)
				{
					if (current.Id != this.currentGroupId)
					{
						stringBuilder.Append(" <label class=\"middle mr20\">");
						stringBuilder.AppendFormat("<input type=\"checkbox\" class=\"CustomGroup\" value=\"{0}\">{1}", current.Id, current.GroupName);
						stringBuilder.Append("  </label>");
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
