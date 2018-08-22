using Hidistro.ControlPanel.Store;
using Hidistro.Entities.FenXiao;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class CustomDistributorStatistics : AdminPage
	{
		private int id;

		protected System.Web.UI.WebControls.Repeater repCustomDistributorStatisticList;

		protected System.Web.UI.WebControls.Button lkbDelectSelect;

		protected UpImg uploader1;

		protected System.Web.UI.WebControls.TextBox txtStoreName;

		protected System.Web.UI.WebControls.TextBox txtOrderNum;

		protected System.Web.UI.WebControls.TextBox txtCommTotal;

		protected System.Web.UI.WebControls.HiddenField hiddid;

		protected System.Web.UI.WebControls.Button btnSaveComm;

		protected CustomDistributorStatistics() : base("m05", "fxp11")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSaveComm.Click += new System.EventHandler(this.btnSaveComm_Click);
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void btnSaveComm_Click(object sender, System.EventArgs e)
		{
			string value = this.hiddid.Value;
			if (string.IsNullOrEmpty(this.uploader1.UploadedImageUrl.ToString()))
			{
				this.ShowMsg("请选择图片上传！", false);
				return;
			}
			string uploadedImageUrl = this.uploader1.UploadedImageUrl;
			string storeName = this.txtStoreName.Text.Trim();
			string text = this.txtOrderNum.Text.Trim();
			string text2 = this.txtCommTotal.Text.Trim();
			CustomDistributorStatistic customDistributorStatistic = new CustomDistributorStatistic();
			customDistributorStatistic.OrderNums = (string.IsNullOrEmpty(text) ? 0 : int.Parse(text));
			customDistributorStatistic.Logo = uploadedImageUrl;
			customDistributorStatistic.StoreName = storeName;
			customDistributorStatistic.CommTotalSum = (string.IsNullOrEmpty(text2) ? 0f : float.Parse(text2));
			if (!string.IsNullOrEmpty(value))
			{
				customDistributorStatistic.id = int.Parse(value);
				System.Data.DataTable customDistributorStatistic2 = VShopHelper.GetCustomDistributorStatistic(customDistributorStatistic.StoreName);
				if (customDistributorStatistic2.Rows.Count > 0 && customDistributorStatistic.id != int.Parse(customDistributorStatistic2.Rows[0]["id"].ToString()))
				{
					this.ShowMsg("店铺名称已经存在，请重新添加店铺名称!", false);
					return;
				}
				VShopHelper.UpdateCustomDistributorStatistic(customDistributorStatistic);
				this.ShowMsgAndReUrl("修改成功", true, "CustomDistributorStatistics.aspx");
				return;
			}
			else
			{
				System.Data.DataTable dataTable = (System.Data.DataTable)VShopHelper.GetCustomDistributorStatisticList().Data;
				if (dataTable != null && dataTable.Rows.Count >= 10)
				{
					this.ShowMsg("自定义排行榜最多添加10条记录!", false);
					return;
				}
				System.Data.DataTable customDistributorStatistic3 = VShopHelper.GetCustomDistributorStatistic(customDistributorStatistic.StoreName);
				if (customDistributorStatistic3.Rows.Count > 0)
				{
					this.ShowMsg("店铺名称已经存在，请重新添加店铺名称!", false);
					return;
				}
				VShopHelper.InsertCustomDistributorStatistic(customDistributorStatistic);
				this.ShowMsgAndReUrl("添加成功", true, "CustomDistributorStatistics.aspx");
				return;
			}
		}

		private void BindData()
		{
			this.repCustomDistributorStatisticList.DataSource = VShopHelper.GetCustomDistributorStatisticList().Data;
			this.repCustomDistributorStatisticList.DataBind();
		}

		protected void lkbDelectSelect_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要删除的分销商排名", false);
				return;
			}
			VShopHelper.DeleteCustomDistributorStatistic(text);
			this.ShowMsg("删除成功", true);
			this.BindData();
		}

		protected void repCustomDistributorStatisticList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Edit")
			{
				return;
			}
			if (e.CommandName == "Update")
			{
				return;
			}
			if (e.CommandName == "Delete")
			{
				string value = e.CommandArgument.ToString();
				if (!string.IsNullOrEmpty(value))
				{
					VShopHelper.DeleteCustomDistributorStatistic(value);
					this.ShowMsg("删除成功", true);
					this.BindData();
				}
			}
		}
	}
}
