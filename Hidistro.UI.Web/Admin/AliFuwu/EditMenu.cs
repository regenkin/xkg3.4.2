using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.VShop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.AliFuwu
{
	public class EditMenu : AdminPage
	{
		protected int iNameByteWidth = 8;

		protected int iframeHeight = 240;

		private int oneLineHeight = 44;

		private int id = Globals.RequestQueryNum("MenuID");

		private int parentid = Globals.RequestQueryNum("PID");

		protected System.Web.UI.WebControls.HiddenField hdfIframeHeight;

		protected System.Web.UI.WebControls.TextBox txtMenuName;

		protected System.Web.UI.HtmlControls.HtmlGenericControl liParent;

		protected System.Web.UI.WebControls.Literal lblParent;

		protected System.Web.UI.HtmlControls.HtmlGenericControl liBind;

		protected System.Web.UI.WebControls.DropDownList ddlType;

		protected System.Web.UI.HtmlControls.HtmlGenericControl liValue;

		protected System.Web.UI.WebControls.DropDownList ddlValue;

		protected System.Web.UI.HtmlControls.HtmlGenericControl liUrl;

		protected System.Web.UI.WebControls.TextBox txtUrl;

		protected System.Web.UI.WebControls.Button btnAddMenu;

		protected EditMenu() : base("m11", "fwp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddMenu.Click += new System.EventHandler(this.btnAddMenu_Click);
			if (!this.Page.IsPostBack)
			{
				this.liValue.Visible = false;
				this.liUrl.Visible = false;
				MenuInfo menuInfo = new MenuInfo();
				if (this.id > 0)
				{
					menuInfo = VShopHelper.GetFuwuMenu(this.id);
					if (menuInfo != null)
					{
						this.txtMenuName.Text = menuInfo.Name;
						if (menuInfo.ParentMenuId == 0)
						{
							System.Collections.Generic.IList<MenuInfo> fuwuMenusByParentId = VShopHelper.GetFuwuMenusByParentId(this.id);
							if (fuwuMenusByParentId.Count > 0)
							{
								this.liBind.Visible = false;
								this.iframeHeight -= this.oneLineHeight;
							}
							this.liParent.Visible = false;
							this.iframeHeight -= this.oneLineHeight;
						}
						else
						{
							this.iNameByteWidth = 20;
							this.lblParent.Text = VShopHelper.GetFuwuMenu(menuInfo.ParentMenuId).Name;
						}
						this.ddlType.SelectedValue = System.Convert.ToString((int)menuInfo.BindType);
						BindType bindType = menuInfo.BindType;
						switch (bindType)
						{
						case BindType.Key:
						case BindType.Topic:
							this.liUrl.Visible = false;
							this.liValue.Visible = true;
							break;
						default:
							if (bindType == BindType.Url)
							{
								this.liUrl.Visible = true;
								this.liValue.Visible = false;
							}
							else
							{
								this.liUrl.Visible = false;
								this.liValue.Visible = false;
							}
							break;
						}
						BindType bindType2 = menuInfo.BindType;
						if (bindType2 != BindType.Key)
						{
							if (bindType2 == BindType.Url)
							{
								this.txtUrl.Text = menuInfo.Content;
							}
						}
						else
						{
							this.ddlValue.DataSource = from a in ReplyHelper.GetAllReply()
							where !string.IsNullOrWhiteSpace(a.Keys)
							select a;
							this.ddlValue.DataTextField = "Keys";
							this.ddlValue.DataValueField = "Id";
							this.ddlValue.DataBind();
							this.ddlValue.SelectedValue = menuInfo.ReplyId.ToString();
						}
					}
					else
					{
						this.ShowMsgAndReUrl("参数不正确", false, "ManageMenu.aspx", "parent");
					}
				}
				else if (this.parentid > 0)
				{
					MenuInfo fuwuMenu = VShopHelper.GetFuwuMenu(this.parentid);
					if (menuInfo != null)
					{
						this.iNameByteWidth = 20;
						this.lblParent.Text = fuwuMenu.Name;
					}
					else
					{
						this.ShowMsgAndReUrl("参数不正确", false, "ManageMenu.aspx", "parent");
					}
				}
				else
				{
					this.iframeHeight -= this.oneLineHeight;
					this.liParent.Visible = false;
				}
				this.hdfIframeHeight.Value = this.iframeHeight.ToString();
			}
		}

		private void btnAddMenu_Click(object sender, System.EventArgs e)
		{
			if (this.ddlType.SelectedValue == "1" && this.ddlValue.Items.Count <= 0)
			{
				this.ShowMsgToTarget("关键字不能为空", false, "parent");
				return;
			}
			MenuInfo menuInfo = new MenuInfo();
			if (this.id > 0)
			{
				menuInfo = VShopHelper.GetFuwuMenu(this.id);
			}
			else
			{
				menuInfo.ParentMenuId = this.parentid;
				if (!VShopHelper.CanAddFuwuMenu(menuInfo.ParentMenuId))
				{
					this.ShowMsgToTarget("一级菜单不能超过三个，对应二级菜单不能超过五个", false, "parent");
					return;
				}
			}
			int num = 16;
			string msg = "菜单标题不超过16个字节！";
			if (menuInfo.ParentMenuId > 0)
			{
				num = 14;
				msg = "二级菜单不超过14个字节！";
			}
			string text = this.txtMenuName.Text;
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsgToTarget("请填写菜单名称！", false, "parent");
				return;
			}
			if (this.GetStrLen(text) > num)
			{
				this.ShowMsgToTarget(msg, false, "parent");
				return;
			}
			menuInfo.Name = this.txtMenuName.Text;
			menuInfo.Type = "click";
			if (menuInfo.ParentMenuId == 0)
			{
				menuInfo.Type = "view";
			}
			else if (string.IsNullOrEmpty(this.ddlType.SelectedValue) || this.ddlType.SelectedValue == "0")
			{
				this.ShowMsgToTarget("二级菜单必须绑定一个对象", false, "parent");
				return;
			}
			menuInfo.Bind = System.Convert.ToInt32(this.ddlType.SelectedValue);
			BindType bindType = menuInfo.BindType;
			switch (bindType)
			{
			case BindType.Key:
				menuInfo.ReplyId = System.Convert.ToInt32(this.ddlValue.SelectedValue);
				break;
			case BindType.Topic:
				menuInfo.Content = this.ddlValue.SelectedValue;
				break;
			default:
				if (bindType == BindType.Url)
				{
					menuInfo.Content = this.txtUrl.Text.Trim();
				}
				break;
			}
			if (this.id > 0)
			{
				if (VShopHelper.UpdateFuwuMenu(menuInfo))
				{
					this.DoFunction("菜单修改成功！");
					return;
				}
				this.ShowMsgToTarget("菜单添加失败", false, "parent");
				return;
			}
			else
			{
				if (VShopHelper.SaveFuwuMenu(menuInfo))
				{
					this.DoFunction("菜单添加成功！");
					return;
				}
				this.ShowMsgToTarget("菜单添加失败", false, "parent");
				return;
			}
		}

		private int GetStrLen(string strData)
		{
			System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("GB2312");
			return encoding.GetByteCount(strData);
		}

		private void DoFunction(string msg)
		{
			string str = "parent.$('#myModal').modal('hide');parent.loadmenu();parent.ShowMsg('" + msg + "',true)";
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
			}
		}

		protected void ddlType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindType bindType = (BindType)System.Convert.ToInt32(this.ddlType.SelectedValue);
			BindType bindType2 = bindType;
			switch (bindType2)
			{
			case BindType.Key:
			case BindType.Topic:
				this.liUrl.Visible = false;
				this.liValue.Visible = true;
				break;
			default:
				if (bindType2 == BindType.Url)
				{
					this.liUrl.Visible = true;
					this.liValue.Visible = false;
				}
				else
				{
					this.liUrl.Visible = false;
					this.liValue.Visible = false;
				}
				break;
			}
			BindType bindType3 = bindType;
			if (bindType3 != BindType.Key)
			{
				return;
			}
			this.ddlValue.DataSource = from a in ReplyHelper.GetAllReply()
			where !string.IsNullOrWhiteSpace(a.Keys)
			select a;
			this.ddlValue.DataTextField = "Keys";
			this.ddlValue.DataValueField = "Id";
			this.ddlValue.DataBind();
		}
	}
}
