using Hidistro.Core.Enums;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class DistributorGradeLitral : Literal
	{
		public object GradeId
		{
			get
			{
				return this.ViewState["GradeId"];
			}
			set
			{
				this.ViewState["GradeId"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			switch ((DistributorGrade)this.GradeId)
			{
			case DistributorGrade.OneDistributor:
				base.Text = "一级";
				break;
			case DistributorGrade.TowDistributor:
				base.Text = "二级";
				break;
			case DistributorGrade.ThreeDistributor:
				base.Text = "三级";
				break;
			default:
				base.Text = "暂无";
				break;
			}
			base.Render(writer);
		}
	}
}
