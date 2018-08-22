using ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class PrizeDeliveryDetail : AdminPage
	{
		private string Did = "";

		protected ListImage txtImage;

		protected System.Web.UI.WebControls.Literal txtProductName;

		protected System.Web.UI.WebControls.Literal txtGameTitle;

		protected System.Web.UI.WebControls.Literal txtGameType;

		protected System.Web.UI.WebControls.Literal txtPlayTime;

		protected System.Web.UI.WebControls.Literal txtDeliever;

		protected System.Web.UI.WebControls.Literal txtDTel;

		protected System.Web.UI.WebControls.Literal txtPrizeGrade;

		protected System.Web.UI.WebControls.Literal txtRegionName;

		protected System.Web.UI.WebControls.Literal txtAddress;

		protected System.Web.UI.WebControls.Literal txtReceiver;

		protected System.Web.UI.WebControls.Literal txtTel;

		protected System.Web.UI.WebControls.Literal txtExpressName;

		protected System.Web.UI.WebControls.Literal txtCourierNumber;

		protected System.Web.UI.WebControls.Literal txtStatus;

		protected PrizeDeliveryDetail() : base("m08", "yxp16")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.Did = base.Request.QueryString["LogId"];
			string text = Globals.RequestQueryStr("pid");
			if (string.IsNullOrEmpty(this.Did))
			{
				return;
			}
			PrizesDeliveQuery prizesDeliveQuery = new PrizesDeliveQuery();
			prizesDeliveQuery.Status = -1;
			prizesDeliveQuery.PageIndex = 1;
			prizesDeliveQuery.PageSize = 2;
			prizesDeliveQuery.PrizeType = -1;
			DbQueryResult dbQueryResult;
			if (!string.IsNullOrEmpty(text) && text != "0")
			{
				prizesDeliveQuery.Pid = text;
				dbQueryResult = GameHelper.GetAllPrizesDeliveryList(prizesDeliveQuery, "", "*");
			}
			else
			{
				prizesDeliveQuery.LogId = this.Did;
				prizesDeliveQuery.SortBy = "LogId";
				dbQueryResult = GameHelper.GetPrizesDeliveryList(prizesDeliveQuery, "", "*");
			}
			System.Data.DataTable dataTable = (System.Data.DataTable)dbQueryResult.Data;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				System.Data.DataRow dataRow = dataTable.Rows[0];
				this.txtStatus.Text = GameHelper.GetPrizesDeliveStatus(this.Dbnull2str(dataRow["status"]), this.Dbnull2str(dataRow["IsLogistics"]), this.Dbnull2str(dataRow["PrizeType"]), dataRow["gametype"].ToString());
				this.txtCourierNumber.Text = this.Dbnull2str(dataRow["CourierNumber"]);
				this.txtExpressName.Text = this.Dbnull2str(dataRow["ExpressName"]);
				this.txtTel.Text = this.Dbnull2str(dataRow["Tel"]);
				this.txtDTel.Text = this.Dbnull2str(dataRow["Tel"]);
				this.txtReceiver.Text = this.Dbnull2str(dataRow["Receiver"]);
				this.txtDeliever.Text = this.Dbnull2str(dataRow["Receiver"]);
				this.txtAddress.Text = this.Dbnull2str(dataRow["Address"]);
				this.txtRegionName.Text = this.Dbnull2str(dataRow["ReggionPath"]);
				this.txtImage.ImageUrl = ((dataRow["ThumbnailUrl100"] == System.DBNull.Value) ? "/utility/pics/none.gif" : dataRow["ThumbnailUrl100"].ToString());
				if (this.txtRegionName.Text.Trim() != "")
				{
					string[] array = this.txtRegionName.Text.Trim().Split(new char[]
					{
						','
					});
					this.txtRegionName.Text = RegionHelper.GetFullRegion(int.Parse(array[array.Length - 1]), ",");
				}
				if (!string.IsNullOrEmpty(text) && text != "0")
				{
					if (this.txtTel.Text == "")
					{
						this.txtTel.Text = this.Dbnull2str(dataRow["Tel"]);
						this.txtDTel.Text = this.Dbnull2str(dataRow["Tel"]);
					}
					this.txtPlayTime.Text = ((System.DateTime)dataRow["WinTime"]).ToString("yyyy-MM-dd HH:mm:ss");
					this.txtGameTitle.Text = dataRow["Title"].ToString();
				}
				else
				{
					if (this.txtImage.ImageUrl == "/utility/pics/none.gif" && dataRow["PrizeId"] != System.DBNull.Value && Globals.ToNum(dataRow["PrizeId"]) > 0)
					{
						int num = Globals.ToNum(dataRow["PrizeId"]);
						if (num > 0)
						{
							GamePrizeInfo gamePrizeInfoById = GameHelper.GetGamePrizeInfoById(num);
							if (gamePrizeInfoById != null)
							{
								this.txtImage.ImageUrl = gamePrizeInfoById.PrizeImage;
							}
						}
					}
					if (this.txtDeliever.Text == "")
					{
						this.txtReceiver.Text = this.Dbnull2str(dataRow["RealName"]);
						this.txtDeliever.Text = this.Dbnull2str(dataRow["RealName"]);
					}
					if (this.txtTel.Text == "")
					{
						this.txtTel.Text = this.Dbnull2str(dataRow["CellPhone"]);
						this.txtDTel.Text = this.Dbnull2str(dataRow["CellPhone"]);
					}
					this.txtGameTitle.Text = dataRow["GameTitle"].ToString();
					this.txtPlayTime.Text = ((System.DateTime)dataRow["PlayTime"]).ToString("yyyy-MM-dd HH:mm:ss");
				}
				this.txtPrizeGrade.Text = GameHelper.GetPrizeGradeName(this.Dbnull2str(dataRow["PrizeGrade"]));
				this.txtGameType.Text = GameHelper.GetGameTypeName(dataRow["GameType"].ToString());
				this.txtProductName.Text = dataRow["ProductName"].ToString();
			}
		}

		private string Dbnull2str(object data)
		{
			return (data == System.DBNull.Value) ? "" : data.ToString();
		}
	}
}
