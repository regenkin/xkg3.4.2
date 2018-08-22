using ControlPanel.Promotions;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class AddVote : AdminPage
	{
		protected int id;

		protected VoteInfo vote = new VoteInfo();

		protected string items = string.Empty;

		protected Script Script4;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.HiddenField hidpic;

		protected System.Web.UI.WebControls.HiddenField hidpicdel;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected SetMemberRange SetMemberRange;

		protected ucUeditor fkContent;

		protected AddVote() : base("m08", "yxp06")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("id") && base.Request["id"].ToString().bInt(ref this.id))
			{
				this.vote = VoteHelper.GetVote((long)this.id);
				if (this.vote == null)
				{
					this.ShowMsg("没有这个投票调查！", false);
				}
				this.hidpic.Value = this.vote.ImageUrl;
				this.fkContent.Text = this.vote.Description;
				this.calendarStartDate.SelectedDate = new System.DateTime?(this.vote.StartDate.Date);
				this.calendarEndDate.SelectedDate = new System.DateTime?(this.vote.EndDate.Date);
				System.Web.UI.WebControls.HiddenField hiddenField = this.SetMemberRange.FindControl("txt_Grades") as System.Web.UI.WebControls.HiddenField;
				System.Web.UI.WebControls.HiddenField hiddenField2 = this.SetMemberRange.FindControl("txt_DefualtGroup") as System.Web.UI.WebControls.HiddenField;
				System.Web.UI.WebControls.HiddenField hiddenField3 = this.SetMemberRange.FindControl("txt_CustomGroup") as System.Web.UI.WebControls.HiddenField;
				this.SetMemberRange.Grade = this.vote.MemberGrades;
				this.SetMemberRange.DefualtGroup = this.vote.DefualtGroup;
				this.SetMemberRange.CustomGroup = this.vote.CustomGroup;
				hiddenField.Value = this.vote.MemberGrades;
				hiddenField2.Value = this.vote.DefualtGroup;
				hiddenField3.Value = this.vote.CustomGroup;
				if (this.vote.VoteItems != null && this.vote.VoteItems.Count > 0)
				{
					foreach (VoteItemInfo current in this.vote.VoteItems)
					{
						this.items = this.items + current.VoteItemName + ",";
					}
				}
				this.items = this.items.TrimEnd(new char[]
				{
					','
				}).Replace(',', '\n');
			}
		}
	}
}
