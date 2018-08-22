using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.StatisticsReport;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Sales
{
	public class MemberRegion : AdminPage
	{
		public string QtyList1 = "";

		public int MaxQty;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected MemberRegion() : base("m10", "tjp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.UpdateMemberTopRegion();
				this.BindData();
			}
		}

		private void UpdateMemberTopRegion()
		{
			System.Data.DataTable top50NotTopRegionIdBind = MemberHelper.GetTop50NotTopRegionIdBind();
			if (top50NotTopRegionIdBind.Rows.Count > 0)
			{
				for (int i = 0; i < top50NotTopRegionIdBind.Rows.Count; i++)
				{
					MemberInfo member = MemberHelper.GetMember(Globals.ToNum(top50NotTopRegionIdBind.Rows[i]["UserID"]));
					if (member != null)
					{
						member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
						MemberHelper.Update(member);
					}
				}
			}
		}

		private void BindData()
		{
			OrderStatisticsQuery orderStatisticsQuery = new OrderStatisticsQuery();
			orderStatisticsQuery.PageIndex = 1;
			orderStatisticsQuery.PageSize = 50;
			orderStatisticsQuery.SortOrder = SortAction.Desc;
			orderStatisticsQuery.SortBy = "TotalRec";
			Globals.EntityCoding(orderStatisticsQuery, true);
			DbQueryResult dbQueryResult = ShopStatisticHelper.Member_GetRegionReport(orderStatisticsQuery);
			int num = 0;
			System.Data.DataTable dataTable = ((System.Data.DataTable)dbQueryResult.Data).Clone();
			foreach (System.Data.DataRow dataRow in ((System.Data.DataTable)dbQueryResult.Data).Rows)
			{
				num++;
				System.Data.DataRow dataRow2 = dataTable.NewRow();
				dataRow2["RegionName"] = dataRow["RegionName"];
				dataRow2["TotalRec"] = dataRow["TotalRec"];
				dataTable.Rows.Add(dataRow2);
				if (num == 1)
				{
					this.MaxQty = int.Parse(dataRow2["TotalRec"].ToString());
				}
				if (num >= 10)
				{
					break;
				}
			}
			this.rptList.DataSource = dataTable;
			this.rptList.DataBind();
			this.QtyList1 = "";
			num = 0;
			int count = ((System.Data.DataTable)dbQueryResult.Data).Rows.Count;
			foreach (System.Data.DataRow drOne in ((System.Data.DataTable)dbQueryResult.Data).Rows)
			{
				this.QtyList1 = string.Concat(new string[]
				{
					this.QtyList1,
					"{name: '",
					base.GetFieldValue(drOne, "RegionName"),
					"',value: ",
					base.GetFieldIntValue(drOne, "TotalRec").ToString(),
					"}"
				});
				if (num < count - 1)
				{
					this.QtyList1 += ",\n";
				}
				num++;
			}
		}
	}
}
