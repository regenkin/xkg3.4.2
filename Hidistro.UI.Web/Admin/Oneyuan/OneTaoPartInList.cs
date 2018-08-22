using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Oneyuan
{
	public class OneTaoPartInList : AdminPage
	{
		private string NickName = "";

		private bool ShowType;

		private string VaidStr = "";

		private System.Collections.Generic.Dictionary<string, string> LuckDic = new System.Collections.Generic.Dictionary<string, string>();

		private System.Collections.Generic.Dictionary<string, string> LuckAllDic = new System.Collections.Generic.Dictionary<string, string>();

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtEditInfo;

		protected OneTaoViewTab ViewTab1;

		protected System.Web.UI.WebControls.TextBox txtUserName;

		protected System.Web.UI.WebControls.CheckBox ShowIsPrize;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected System.Web.UI.WebControls.Repeater Datalist;

		protected Pager pager;

		protected OneTaoPartInList() : base("m08", "yxp20")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
		}

		protected void rptypelist_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Literal literal = e.Item.FindControl("PrizeNum") as System.Web.UI.WebControls.Literal;
				System.Web.UI.WebControls.Literal literal2 = e.Item.FindControl("AllPrizeNum") as System.Web.UI.WebControls.Literal;
				System.Data.DataRowView dataRowView = (System.Data.DataRowView)e.Item.DataItem;
				if ((bool)dataRowView["IsWin"])
				{
					if (this.LuckDic.ContainsKey(dataRowView["Pid"].ToString()))
					{
						literal.Text = this.LuckDic[dataRowView["Pid"].ToString()];
					}
					else
					{
						literal.Text = "";
					}
				}
				else
				{
					literal.Text = "";
				}
				if (this.LuckAllDic.ContainsKey(dataRowView["Pid"].ToString()))
				{
					literal2.Text = this.LuckAllDic[dataRowView["Pid"].ToString()];
					return;
				}
				literal2.Text = "无号码";
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
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["NickName"]))
				{
					this.NickName = base.Server.UrlDecode(this.Page.Request.QueryString["NickName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShowType"]))
				{
					bool.TryParse(this.Page.Request.QueryString["ShowType"], out this.ShowType);
				}
				this.VaidStr = this.Page.Request.QueryString["vaid"];
				this.ShowIsPrize.Checked = this.ShowType;
				this.txtUserName.Text = this.NickName;
				return;
			}
			this.ShowType = this.ShowIsPrize.Checked;
			this.NickName = this.txtUserName.Text;
			this.VaidStr = this.Page.Request.QueryString["vaid"];
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("vaid", this.VaidStr);
			nameValueCollection.Add("NickName", this.NickName);
			nameValueCollection.Add("ShowType", this.ShowType.ToString());
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}

		private void initLuckDic(System.Collections.Generic.IList<LuckInfo> luckWinlist, System.Collections.Generic.IList<LuckInfo> luckLoselist)
		{
			foreach (LuckInfo current in luckWinlist)
			{
				if (!this.LuckDic.ContainsKey(current.Pid))
				{
					this.LuckDic.Add(current.Pid, current.PrizeNum);
				}
				else
				{
					System.Collections.Generic.Dictionary<string, string> luckDic;
					string pid;
					(luckDic = this.LuckDic)[pid = current.Pid] = luckDic[pid] + "<br/>" + current.PrizeNum;
				}
			}
			this.LuckAllDic = new System.Collections.Generic.Dictionary<string, string>(this.LuckDic);
			foreach (LuckInfo current2 in luckLoselist)
			{
				if (!this.LuckAllDic.ContainsKey(current2.Pid))
				{
					this.LuckAllDic.Add(current2.Pid, current2.PrizeNum);
				}
				else
				{
					System.Collections.Generic.Dictionary<string, string> luckAllDic;
					string pid2;
					(luckAllDic = this.LuckAllDic)[pid2 = current2.Pid] = luckAllDic[pid2] + "<br/>" + current2.PrizeNum;
				}
			}
		}

		private void BindData()
		{
			OneyuanTaoPartInQuery oneyuanTaoPartInQuery = new OneyuanTaoPartInQuery();
			oneyuanTaoPartInQuery.PageIndex = this.pager.PageIndex;
			oneyuanTaoPartInQuery.PageSize = this.pager.PageSize;
			oneyuanTaoPartInQuery.ActivityId = this.VaidStr;
			oneyuanTaoPartInQuery.IsPay = 1;
			oneyuanTaoPartInQuery.SortBy = "Pid";
			oneyuanTaoPartInQuery.UserName = this.NickName;
			if (this.ShowType)
			{
				oneyuanTaoPartInQuery.state = 3;
			}
			DbQueryResult oneyuanPartInDataTable = OneyuanTaoHelp.GetOneyuanPartInDataTable(oneyuanTaoPartInQuery);
			System.Collections.Generic.IList<LuckInfo> luckInfoList = OneyuanTaoHelp.getLuckInfoList(true, this.VaidStr);
			System.Collections.Generic.IList<LuckInfo> luckInfoList2 = OneyuanTaoHelp.getLuckInfoList(false, this.VaidStr);
			this.initLuckDic(luckInfoList, luckInfoList2);
			this.Datalist.DataSource = oneyuanPartInDataTable.Data;
			this.Datalist.DataBind();
			this.pager.TotalRecords = oneyuanPartInDataTable.TotalRecords;
		}

		protected void ShowIsPrize_CheckedChanged1(object sender, System.EventArgs e)
		{
			this.ReBind(false);
		}
	}
}
