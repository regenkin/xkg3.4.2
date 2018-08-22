using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Weixin.MP.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class MessageTemplets : AdminPage
	{
		public string WeixinAppId = "";

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);

		protected string CodeTicket = "";

		protected Script Script5;

		protected Script Script6;

		protected System.Web.UI.WebControls.Repeater rptAdminUserList;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected System.Web.UI.WebControls.Button btnSaveTemplatesList;

		protected System.Web.UI.WebControls.CheckBoxList cbPowerListDistributors;

		protected System.Web.UI.WebControls.CheckBoxList cbPowerListMember;

		protected System.Web.UI.WebControls.Button btnSaveUserMsgDetail;

		protected System.Web.UI.WebControls.Image imgQRCode;

		protected System.Web.UI.WebControls.Image imgHeadImage;

		protected System.Web.UI.WebControls.TextBox txtScanOpenID;

		protected System.Web.UI.WebControls.HiddenField hfWeiXinAccessToken;

		protected System.Web.UI.WebControls.HiddenField hfAppID;

		protected System.Web.UI.WebControls.TextBox txtAdminName;

		protected System.Web.UI.WebControls.TextBox txtAdminRole;

		protected System.Web.UI.WebControls.CheckBox cbMsg1;

		protected System.Web.UI.WebControls.CheckBox cbMsg2;

		protected System.Web.UI.WebControls.CheckBox cbMsg3;

		protected System.Web.UI.WebControls.CheckBox cbMsg5;

		protected System.Web.UI.WebControls.CheckBox cbMsg4;

		protected System.Web.UI.WebControls.CheckBox cbMsg6;

		protected System.Web.UI.WebControls.Button btnSaveRole;

		protected MessageTemplets() : base("m06", "wxp06")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				if (this.siteSettings.WeixinAppId.Length < 16)
				{
					this.ShowMsgAndReUrl("请先绑定微信公众号", false, "wxconfig.aspx");
					return;
				}
				this.BindData();
			}
		}

		private void BindData()
		{
			this.rptAdminUserList.DataSource = VShopHelper.GetAdminUserMsgList(0);
			this.rptAdminUserList.DataBind();
			this.rptList.DataSource = VShopHelper.GetMessageTemplates();
			this.rptList.DataBind();
			int num = 0;
			this.cbPowerListDistributors.Items.Clear();
			foreach (System.Data.DataRow dataRow in VShopHelper.GetAdminUserMsgDetail(true).Rows)
			{
				System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(dataRow["DetailName"].ToString(), dataRow["DetailType"].ToString());
				this.cbPowerListDistributors.Items.Add(item);
				this.cbPowerListDistributors.Items[num].Selected = (dataRow["IsSelected"].ToString() == "1");
				num++;
			}
			num = 0;
			this.cbPowerListMember.Items.Clear();
			foreach (System.Data.DataRow dataRow2 in VShopHelper.GetAdminUserMsgDetail(false).Rows)
			{
				System.Web.UI.WebControls.ListItem item2 = new System.Web.UI.WebControls.ListItem(dataRow2["DetailName"].ToString(), dataRow2["DetailType"].ToString());
				this.cbPowerListMember.Items.Add(item2);
				this.cbPowerListMember.Items[num].Selected = (dataRow2["IsSelected"].ToString() == "1");
				num++;
			}
			this.WeixinAppId = this.siteSettings.WeixinAppId;
			this.hfAppID.Value = this.siteSettings.WeixinAppId;
			string token_Message = TokenApi.GetToken_Message(this.siteSettings.WeixinAppId, this.siteSettings.WeixinAppSecret);
			this.hfWeiXinAccessToken.Value = token_Message;
			this.ShowQRImage();
		}

		protected void btnSaveRole_Click(object sender, System.EventArgs e)
		{
			MsgList myList = default(MsgList);
			myList.UserOpenId = this.txtScanOpenID.Text.Trim();
			myList.RoleName = this.txtAdminRole.Text.Trim();
			myList.RealName = this.txtAdminName.Text.Trim();
			myList.Msg1 = System.Convert.ToInt32(this.cbMsg1.Checked);
			myList.Msg2 = System.Convert.ToInt32(this.cbMsg2.Checked);
			myList.Msg3 = System.Convert.ToInt32(this.cbMsg3.Checked);
			myList.Msg4 = System.Convert.ToInt32(this.cbMsg4.Checked);
			myList.Msg5 = System.Convert.ToInt32(this.cbMsg5.Checked);
			myList.Msg6 = System.Convert.ToInt32(this.cbMsg6.Checked);
			myList.Type = 0;
			string msg = "";
			bool success = VShopHelper.SaveAdminUserMsgList(true, myList, myList.UserOpenId, out msg);
			this.BindData();
			this.ShowMsg(msg, success);
		}

		protected void rptAdminUserList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.HiddenField hiddenField = e.Item.FindControl("hdfAutoID") as System.Web.UI.WebControls.HiddenField;
				hiddenField.Value = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "AutoID").ToString();
				e.Item.FindControl("lbMsgList");
				System.Web.UI.WebControls.CheckBox checkBox = e.Item.FindControl("cbMsg1") as System.Web.UI.WebControls.CheckBox;
				System.Web.UI.WebControls.CheckBox checkBox2 = e.Item.FindControl("cbMsg2") as System.Web.UI.WebControls.CheckBox;
				System.Web.UI.WebControls.CheckBox checkBox3 = e.Item.FindControl("cbMsg3") as System.Web.UI.WebControls.CheckBox;
				System.Web.UI.WebControls.CheckBox checkBox4 = e.Item.FindControl("cbMsg4") as System.Web.UI.WebControls.CheckBox;
				System.Web.UI.WebControls.CheckBox checkBox5 = e.Item.FindControl("cbMsg5") as System.Web.UI.WebControls.CheckBox;
				System.Web.UI.WebControls.CheckBox checkBox6 = e.Item.FindControl("cbMsg6") as System.Web.UI.WebControls.CheckBox;
				checkBox.Checked = (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Msg1").ToString() == "1");
				checkBox2.Checked = (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Msg2").ToString() == "1");
				checkBox3.Checked = (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Msg3").ToString() == "1");
				checkBox4.Checked = (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Msg4").ToString() == "1");
				checkBox5.Checked = (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Msg5").ToString() == "1");
				checkBox6.Checked = (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Msg6").ToString() == "1");
			}
		}

		protected void rptList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.HiddenField hiddenField = e.Item.FindControl("hdfMessageType") as System.Web.UI.WebControls.HiddenField;
				hiddenField.Value = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "MessageType").ToString();
			}
		}

		protected void btnGetCreateTicket_Click(object sender, System.EventArgs e)
		{
			this.ShowQRImage();
		}

		private void ShowQRImage()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			string token_Message = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
			string text = BarCodeApi.CreateTicket(token_Message, "1", "QR_SCENE", "604800");
			this.CodeTicket = text;
			this.imgQRCode.ImageUrl = BarCodeApi.GetQRImageUrlByTicket(text);
		}

		protected void rptAdminUserList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			int num = 0;
			int.TryParse(e.CommandArgument.ToString(), out num);
			MsgList myList = default(MsgList);
			System.Web.UI.WebControls.TextBox textBox = e.Item.FindControl("txtUserOpenId") as System.Web.UI.WebControls.TextBox;
			myList.UserOpenId = textBox.Text.Trim();
			System.Web.UI.WebControls.TextBox textBox2 = e.Item.FindControl("txtRoleName") as System.Web.UI.WebControls.TextBox;
			myList.RoleName = textBox2.Text.Trim();
			System.Web.UI.WebControls.TextBox textBox3 = e.Item.FindControl("txtRealName") as System.Web.UI.WebControls.TextBox;
			myList.RealName = textBox3.Text.Trim();
			myList.Msg1 = System.Convert.ToInt32((e.Item.FindControl("cbMsg1") as System.Web.UI.WebControls.CheckBox).Checked);
			myList.Msg2 = System.Convert.ToInt32((e.Item.FindControl("cbMsg2") as System.Web.UI.WebControls.CheckBox).Checked);
			myList.Msg3 = System.Convert.ToInt32((e.Item.FindControl("cbMsg3") as System.Web.UI.WebControls.CheckBox).Checked);
			myList.Msg4 = System.Convert.ToInt32((e.Item.FindControl("cbMsg4") as System.Web.UI.WebControls.CheckBox).Checked);
			myList.Msg5 = System.Convert.ToInt32((e.Item.FindControl("cbMsg5") as System.Web.UI.WebControls.CheckBox).Checked);
			myList.Msg6 = System.Convert.ToInt32((e.Item.FindControl("cbMsg6") as System.Web.UI.WebControls.CheckBox).Checked);
			string msg = "";
			string commandName;
			if ((commandName = e.CommandName) != null)
			{
				if (!(commandName == "Save"))
				{
					if (!(commandName == "Delete"))
					{
						return;
					}
					bool flag = VShopHelper.DeleteAdminUserMsgList(myList, out msg);
					if (flag)
					{
						this.rptAdminUserList.Items[0].Visible = false;
					}
					this.ShowMsg(msg, flag);
				}
				else
				{
					if (myList.Msg1 + myList.Msg2 + myList.Msg3 + myList.Msg4 + myList.Msg5 + myList.Msg6 == 0)
					{
						this.ShowMsg("当前用户未选择任何消息提醒，无法保存。", false);
						return;
					}
					bool flag = VShopHelper.SaveAdminUserMsgList(false, myList, myList.UserOpenId, out msg);
					this.BindData();
					this.ShowMsg(msg, flag);
					return;
				}
			}
		}

		protected void btnSaveUserMsgDetail_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.List<MsgDetail> list = new System.Collections.Generic.List<MsgDetail>();
			for (int i = 0; i < this.cbPowerListDistributors.Items.Count; i++)
			{
				list.Add(new MsgDetail
				{
					DetailType = this.cbPowerListDistributors.Items[i].Value,
					IsSelectedByDistributor = this.cbPowerListDistributors.Items[i].Selected ? 1 : 0
				});
			}
			VShopHelper.UpdateWeiXinMsgDetail(true, list);
			list.Clear();
			for (int j = 0; j < this.cbPowerListMember.Items.Count; j++)
			{
				list.Add(new MsgDetail
				{
					DetailType = this.cbPowerListMember.Items[j].Value,
					IsSelectedByMember = this.cbPowerListMember.Items[j].Selected ? 1 : 0
				});
			}
			VShopHelper.UpdateWeiXinMsgDetail(false, list);
			this.ShowMsg("保存成功！", true);
		}

		protected void btnSaveTemplatesList_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.List<MessageTemplate> list = new System.Collections.Generic.List<MessageTemplate>();
			for (int i = 0; i < this.rptList.Items.Count; i++)
			{
				list.Add(new MessageTemplate
				{
					MessageType = ((System.Web.UI.WebControls.HiddenField)this.rptList.Items[i].FindControl("hdfMessageType")).Value,
					SendWeixin = true,
					WeixinTemplateId = ((System.Web.UI.WebControls.TextBox)this.rptList.Items[i].FindControl("txtTemplateId")).Text.Trim()
				});
			}
			VShopHelper.UpdateSettings(list);
			this.ShowMsg("保存设置成功", true);
		}
	}
}
