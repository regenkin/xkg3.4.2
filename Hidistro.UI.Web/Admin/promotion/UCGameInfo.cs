using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class UCGameInfo : System.Web.UI.UserControl
	{
		protected bool isAllCheck;

		protected bool memberCheck;

		private GameInfo _gameInfo;

		private System.Collections.Generic.IList<MemberGradeItem> _memberGrades = new System.Collections.Generic.List<MemberGradeItem>();

		protected System.Web.UI.WebControls.TextBox txtGameTitle;

		protected System.Web.UI.WebControls.HiddenField hfGameId;

		protected ucDateTimePicker dateBeginTime;

		protected ucDateTimePicker dateEndTime;

		protected System.Web.UI.WebControls.TextBox txtDescription;

		protected SetMemberRange memberRange;

		protected System.Web.UI.WebControls.TextBox txtNeedPoint;

		protected System.Web.UI.WebControls.TextBox txtGivePoint;

		protected System.Web.UI.WebControls.CheckBox cbOnlyGiveNotPrizeMember;

		protected System.Web.UI.WebControls.TextBox txtLimitEveryDay;

		protected System.Web.UI.WebControls.TextBox txtMaximumDailyLimit;

		protected System.Web.UI.WebControls.HiddenField hfKeyWord;

		protected System.Web.UI.WebControls.TextBox txtGameUrl;

		public GameInfo GameInfo
		{
			get
			{
				this.GetDate();
				return this._gameInfo;
			}
			set
			{
				this._gameInfo = value;
			}
		}

		public GameType GameType
		{
			get;
			set;
		}

		protected System.Collections.Generic.IList<MemberGradeItem> MemberGrades
		{
			get
			{
				return this._memberGrades;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindDate();
			}
		}

		private void BindDate()
		{
			if (this._gameInfo != null)
			{
				this.memberCheck = (this._gameInfo.MemberCheck == 1);
				this.hfGameId.Value = this._gameInfo.GameId.ToString();
				this.txtGameTitle.Text = this._gameInfo.GameTitle;
				this.dateBeginTime.SelectedDate = new System.DateTime?(this._gameInfo.BeginTime);
				this.dateEndTime.SelectedDate = new System.DateTime?(this._gameInfo.EndTime);
				this.txtDescription.Text = this._gameInfo.Description.Replace("<br/>", "\n");
				this.txtNeedPoint.Text = this._gameInfo.NeedPoint.ToString();
				this.txtGivePoint.Text = this._gameInfo.GivePoint.ToString();
				this.cbOnlyGiveNotPrizeMember.Checked = this._gameInfo.OnlyGiveNotPrizeMember;
				this.txtLimitEveryDay.Text = this._gameInfo.LimitEveryDay.ToString();
				this.txtMaximumDailyLimit.Text = this._gameInfo.MaximumDailyLimit.ToString();
				System.Web.UI.WebControls.HiddenField hiddenField = this.memberRange.FindControl("txt_Grades") as System.Web.UI.WebControls.HiddenField;
				System.Web.UI.WebControls.HiddenField hiddenField2 = this.memberRange.FindControl("txt_DefualtGroup") as System.Web.UI.WebControls.HiddenField;
				System.Web.UI.WebControls.HiddenField hiddenField3 = this.memberRange.FindControl("txt_CustomGroup") as System.Web.UI.WebControls.HiddenField;
				this.memberRange.Grade = this._gameInfo.ApplyMembers;
				this.memberRange.CustomGroup = this._gameInfo.CustomGroup;
				this.memberRange.DefualtGroup = this._gameInfo.DefualtGroup;
				hiddenField.Value = this.memberRange.Grade;
				hiddenField2.Value = this.memberRange.DefualtGroup;
				hiddenField3.Value = this.memberRange.CustomGroup;
				switch (this._gameInfo.PlayType)
				{
				case PlayType.一天一次:
				case PlayType.一次:
				case PlayType.一天两次:
				case PlayType.两次:
                        System.Collections.Generic.IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
                        foreach (MemberGradeInfo current in memberGrades)
                        {
                            MemberGradeItem memberGradeItem = new MemberGradeItem();
                            if (!this.isAllCheck)
                            {
                                memberGradeItem.IsCheck = this.IsCheck(current.GradeId.ToString());
                            }
                            else
                            {
                                memberGradeItem.IsCheck = this.isAllCheck;
                            }
                            memberGradeItem.Name = current.Name;
                            memberGradeItem.GradeId = current.GradeId.ToString();
                            this._memberGrades.Add(memberGradeItem);
                        }
                        break;
                }
                if (string.Equals(this._gameInfo.ApplyMembers, "0") && string.Equals(this._gameInfo.DefualtGroup, "0") && string.Equals(this._gameInfo.CustomGroup, "0"))
                {
                    this.isAllCheck = true;
                }
                this.txtGameUrl.Text = this._gameInfo.GameUrl;
            }
			string text = System.Guid.NewGuid().ToString().Replace("-", "");
			this.hfKeyWord.Value = text;
			string text2 = this.CreateGameUrl(text);
			this.txtGameUrl.Text = text2;
			
		}

		private bool IsCheck(string gradeId)
		{
			bool result = false;
			if (this._gameInfo != null)
			{
				string[] array = this._gameInfo.ApplyMembers.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Count<string>(); i++)
				{
					if (string.Equals(array[i], gradeId))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		private void GetDate()
		{
			if (this._gameInfo == null)
			{
				this._gameInfo = new GameInfo();
			}
			try
			{
				this._gameInfo.GameId = int.Parse(this.hfGameId.Value);
			}
			catch (System.Exception)
			{
				this._gameInfo.GameId = 0;
				this._gameInfo.GameType = this.GameType;
			}
			this._gameInfo.GameTitle = this.txtGameTitle.Text;
			try
			{
				this._gameInfo.BeginTime = this.dateBeginTime.SelectedDate.Value;
			}
			catch (System.InvalidOperationException)
			{
				throw new System.Exception("活动时间期的开始日期不能为空！");
			}
			try
			{
				this._gameInfo.EndTime = this.dateEndTime.SelectedDate.Value;
			}
			catch (System.InvalidOperationException)
			{
				throw new System.Exception("活动时间的结束日期不能为空！");
			}
			try
			{
				this._gameInfo.NeedPoint = int.Parse(this.txtNeedPoint.Text);
			}
			catch (System.FormatException)
			{
				throw new System.Exception("活动消耗积分格式不对！");
			}
			try
			{
				this._gameInfo.GivePoint = int.Parse(this.txtGivePoint.Text);
			}
			catch (System.FormatException)
			{
				throw new System.Exception("活动参与送积分格式不对！");
			}
			try
			{
				this._gameInfo.LimitEveryDay = int.Parse(this.txtLimitEveryDay.Text);
			}
			catch (System.FormatException)
			{
				throw new System.Exception("每人最多限次格式不对！");
			}
			try
			{
				this._gameInfo.MaximumDailyLimit = int.Parse(this.txtMaximumDailyLimit.Text);
			}
			catch (System.FormatException)
			{
				throw new System.Exception("每人每天限次不对！");
			}
			this._gameInfo.Description = this.txtDescription.Text.Replace("\n", "<br/>");
			this._gameInfo.OnlyGiveNotPrizeMember = this.cbOnlyGiveNotPrizeMember.Checked;
			this._gameInfo.ApplyMembers = base.Request["allmember"];
			System.Web.UI.WebControls.HiddenField hiddenField = this.memberRange.FindControl("txt_Grades") as System.Web.UI.WebControls.HiddenField;
			System.Web.UI.WebControls.HiddenField hiddenField2 = this.memberRange.FindControl("txt_DefualtGroup") as System.Web.UI.WebControls.HiddenField;
			System.Web.UI.WebControls.HiddenField hiddenField3 = this.memberRange.FindControl("txt_CustomGroup") as System.Web.UI.WebControls.HiddenField;
			if (string.IsNullOrEmpty(this._gameInfo.ApplyMembers))
			{
				this._gameInfo.ApplyMembers = hiddenField.Value;
			}
			this._gameInfo.CustomGroup = hiddenField3.Value;
			this._gameInfo.DefualtGroup = hiddenField2.Value;
			this._gameInfo.MemberCheck = ((base.Request["MemberCheck"] == "on") ? 1 : 0);
			this._gameInfo.GameUrl = this.txtGameUrl.Text.Trim();
			this._gameInfo.KeyWork = this.hfKeyWord.Value;
		}

		private string CreateGameUrl(string keyWord)
		{
			System.Uri url = System.Web.HttpContext.Current.Request.Url;
			string text = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(System.Globalization.CultureInfo.InvariantCulture));
			string text2 = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[]
			{
				url.Scheme,
				Globals.DomainName,
				text
			});
			return string.Format("{0}{1}/Game.aspx?gamid={2}&type={3}", new object[]
			{
				text2,
				Globals.ApplicationPath,
				keyWord,
				(int)this.GameType
			});
		}
	}
}
