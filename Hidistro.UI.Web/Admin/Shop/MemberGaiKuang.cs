using Hidistro.ControlPanel.VShop;
using Hidistro.Entities.StatisticsReport;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class MemberGaiKuang : AdminPage
	{
		public string ActiveUserQty = "0";

		public string SleepUserQty = "0";

		public string SuccessTradeUserQty = "0";

		public string SuccessTradeUserQty_Yesterday = "0";

		public string PotentialUserQty = "0";

		public string PotentialUserQty_Yesterday = "0";

		public string CollectUserQty = "0";

		public string CartUserQty = "0";

		public string MemberQty = "0";

		public string MemberGradeList = "";

		public string QtyList_Grade = "";

		public string RegionNameList = "";

		public string RegionQtyList = "";

		protected MemberGaiKuang() : base("m04", "hyp01")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				string text = "";
				ShopStatisticHelper.StatisticsOrdersByRecDate(System.DateTime.Today, UpdateAction.AllUpdate, 0, out text);
				this.LoadData();
			}
		}

		private void LoadData()
		{
			System.Data.DataRow dataRow = ShopStatisticHelper.MemberGlobal_GetCountInfo();
			if (dataRow != null)
			{
				this.ActiveUserQty = base.GetFieldValue(dataRow, "ActiveUserQty");
				this.SleepUserQty = base.GetFieldValue(dataRow, "SleepUserQty");
				this.SuccessTradeUserQty = base.GetFieldValue(dataRow, "SuccessTradeUserQty");
				this.SuccessTradeUserQty_Yesterday = base.GetFieldValue(dataRow, "SuccessTradeUserQty_Yesterday");
				this.PotentialUserQty = base.GetFieldValue(dataRow, "PotentialUserQty");
				this.PotentialUserQty_Yesterday = base.GetFieldValue(dataRow, "PotentialUserQty_Yesterday");
				this.CollectUserQty = base.GetFieldValue(dataRow, "CollectUserQty");
				this.CartUserQty = base.GetFieldValue(dataRow, "CartUserQty");
				this.MemberQty = base.GetFieldValue(dataRow, "MemberQty");
			}
			System.Data.DataTable dataTable = ShopStatisticHelper.MemberGlobal_GetStatisticList(1);
			System.Data.DataTable dataTable2 = ShopStatisticHelper.MemberGlobal_GetStatisticList(2);
			this.MemberGradeList = "";
			int num = 0;
			int num2 = dataTable.Rows.Count;
			foreach (System.Data.DataRow drOne in dataTable.Rows)
			{
				this.MemberGradeList = this.MemberGradeList + "'" + base.GetFieldValue(drOne, "Name") + "'";
				this.QtyList_Grade = this.QtyList_Grade + "{" + string.Format("value:{0}, name:'{1}'", base.GetFieldValue(drOne, "Total"), base.GetFieldValue(drOne, "Name")) + "}";
				if (num < num2 - 1)
				{
					this.MemberGradeList += ",";
					this.QtyList_Grade += ",";
				}
				this.QtyList_Grade += "\n";
				num++;
			}
			System.Data.DataView defaultView = dataTable2.DefaultView;
			defaultView.Sort = "Total Desc";
			dataTable2 = defaultView.ToTable();
			this.RegionNameList = "";
			this.RegionQtyList = "";
			num = 0;
			num2 = dataTable2.Rows.Count;
			if (num2 > 9)
			{
				num2 = 9;
			}
			foreach (System.Data.DataRow drOne2 in dataTable2.Rows)
			{
				if (num >= 9)
				{
					break;
				}
				this.RegionNameList = this.RegionNameList + "'" + base.GetFieldValue(drOne2, "RegionName") + "'";
				this.RegionQtyList += base.GetFieldValue(drOne2, "Total");
				if (num < num2 - 1)
				{
					this.RegionNameList += ",";
					this.RegionQtyList += ",";
				}
				num++;
			}
		}
	}
}
