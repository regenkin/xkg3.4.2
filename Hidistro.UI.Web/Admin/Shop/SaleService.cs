using ControlPanel.Settings;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.MeiQia.Api.Api;
using Hishop.MeiQia.Api.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class SaleService : AdminPage
	{
		protected bool enable;

		protected System.Web.UI.WebControls.TextBox txt_phone;

		protected System.Web.UI.WebControls.TextBox txt_pwd;

		protected System.Web.UI.WebControls.Button OpenAccount;

		protected System.Web.UI.WebControls.Button ChangePwd;

		protected System.Web.UI.WebControls.Button btnBindUser;

		protected System.Web.UI.WebControls.Button btnClear;

		protected System.Web.UI.WebControls.Repeater grdCustomers;

		protected System.Web.UI.HtmlControls.HtmlInputHidden htxtRoleId;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.TextBox txt_cphone;

		protected System.Web.UI.WebControls.TextBox txt_cpwd;

		protected System.Web.UI.WebControls.TextBox txt_id;

		protected SaleService() : base("m01", "dpp05")
		{
		}

		[PrivilegeCheck(Privilege.Summary)]
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.enable = masterSettings.EnableSaleService;
				CustomerServiceSettings masterSettings2 = CustomerServiceManager.GetMasterSettings(false);
				this.txt_phone.Text = masterSettings2.unit;
				this.txt_pwd.Attributes["Value"] = masterSettings2.password;
				if (!string.IsNullOrEmpty(masterSettings2.unit))
				{
					this.txt_phone.Enabled = false;
					this.OpenAccount.Visible = false;
					this.ChangePwd.Visible = true;
					this.btnBindUser.Visible = false;
					this.btnClear.Visible = true;
					if (string.IsNullOrEmpty(masterSettings2.unitid))
					{
						string tokenValue = TokenApi.GetTokenValue(masterSettings2.AppId, masterSettings2.AppSecret);
						if (!string.IsNullOrEmpty(tokenValue))
						{
							string unitId = EnterpriseApi.GetUnitId(tokenValue, masterSettings2.unit);
							masterSettings2.unitid = unitId;
							CustomerServiceManager.Save(masterSettings2);
						}
					}
				}
				else
				{
					this.txt_phone.Enabled = true;
					this.OpenAccount.Visible = true;
					this.ChangePwd.Visible = false;
					this.btnBindUser.Visible = true;
					this.btnClear.Visible = false;
				}
				this.BindCustomers(masterSettings2.unit);
			}
		}

		private void BindCustomers(string unit)
		{
			System.Data.DataTable customers = CustomerServiceHelper.GetCustomers(unit);
			this.grdCustomers.DataSource = customers;
			this.grdCustomers.DataBind();
		}

		protected void OpenAccount_Click(object sender, System.EventArgs e)
		{
			CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
			if (string.IsNullOrEmpty(this.txt_phone.Text))
			{
				this.ShowMsg("请输入手机号码！", false);
			}
			if (string.IsNullOrEmpty(this.txt_pwd.Text))
			{
				this.ShowMsg("请输入密码！", false);
			}
			string tokenValue = TokenApi.GetTokenValue(masterSettings.AppId, masterSettings.AppSecret);
			if (!string.IsNullOrEmpty(tokenValue))
			{
				SiteSettings masterSettings2 = SettingsManager.GetMasterSettings(false);
				string value = string.Empty;
				if (!string.IsNullOrEmpty(masterSettings2.DistributorLogoPic))
				{
					value = Globals.DomainName + masterSettings2.DistributorLogoPic;
				}
				string value2 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(this.txt_pwd.Text, "MD5").ToLower();
				string text = EnterpriseApi.CreateEnterprise(tokenValue, new System.Collections.Generic.Dictionary<string, string>
				{
					{
						"unit",
						this.txt_phone.Text
					},
					{
						"password",
						value2
					},
					{
						"unitname",
						masterSettings2.SiteName
					},
					{
						"activated",
						"1"
					},
					{
						"logo",
						value
					},
					{
						"url",
						""
					},
					{
						"tel",
						masterSettings2.ShopTel
					},
					{
						"contact",
						""
					},
					{
						"location",
						""
					}
				});
				if (!string.IsNullOrWhiteSpace(text))
				{
					string jsonValue = Hishop.MeiQia.Api.Util.Common.GetJsonValue(text, "errcode");
					string jsonValue2 = Hishop.MeiQia.Api.Util.Common.GetJsonValue(text, "errmsg");
					if (jsonValue == "0")
					{
						string unitId = EnterpriseApi.GetUnitId(tokenValue, this.txt_phone.Text);
						if (!string.IsNullOrEmpty(unitId))
						{
							masterSettings.unitid = unitId;
							masterSettings.unit = this.txt_phone.Text;
							masterSettings.password = this.txt_pwd.Text;
							CustomerServiceManager.Save(masterSettings);
							this.ShowMsgAndReUrl("开通主账号成功！", true, "saleservice.aspx");
						}
						else
						{
							this.ShowMsg("获取主账号Id失败！", false);
						}
					}
					else
					{
						string text2 = jsonValue2;
						if (text2.Contains("has registered"))
						{
							this.ShowMsg("您输入的账号名称已被注册，请更换一个再注册！", false);
						}
						else
						{
							this.ShowMsg("开通主账号失败！(" + jsonValue2 + ")", false);
						}
					}
				}
				else
				{
					this.ShowMsg("开通主账号失败！", false);
				}
				this.enable = masterSettings2.EnableSaleService;
				return;
			}
			this.ShowMsg("获取access_token失败！", false);
		}

		protected void ChangePwd_Click(object sender, System.EventArgs e)
		{
			CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
			if (string.IsNullOrEmpty(this.txt_phone.Text))
			{
				this.ShowMsg("请输入手机号码！", false);
			}
			if (string.IsNullOrEmpty(this.txt_pwd.Text))
			{
				this.ShowMsg("请输入密码！", false);
			}
			string tokenValue = TokenApi.GetTokenValue(masterSettings.AppId, masterSettings.AppSecret);
			if (!string.IsNullOrEmpty(tokenValue))
			{
				SiteSettings masterSettings2 = SettingsManager.GetMasterSettings(false);
				string value = string.Empty;
				if (!string.IsNullOrEmpty(masterSettings2.DistributorLogoPic))
				{
					value = Globals.DomainName + masterSettings2.DistributorLogoPic;
				}
				string value2 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(this.txt_pwd.Text, "MD5").ToLower();
				string text = EnterpriseApi.UpdateEnterprise(tokenValue, new System.Collections.Generic.Dictionary<string, string>
				{
					{
						"unit",
						this.txt_phone.Text
					},
					{
						"password",
						value2
					},
					{
						"unitname",
						masterSettings2.SiteName
					},
					{
						"activated",
						"1"
					},
					{
						"logo",
						value
					},
					{
						"url",
						""
					},
					{
						"tel",
						masterSettings2.ShopTel
					},
					{
						"contact",
						""
					},
					{
						"location",
						""
					}
				});
				if (!string.IsNullOrWhiteSpace(text))
				{
					string jsonValue = Hishop.MeiQia.Api.Util.Common.GetJsonValue(text, "errcode");
					string jsonValue2 = Hishop.MeiQia.Api.Util.Common.GetJsonValue(text, "errmsg");
					if (jsonValue == "0")
					{
						masterSettings.password = this.txt_pwd.Text;
						CustomerServiceManager.Save(masterSettings);
						this.ShowMsg("修改密码成功！", true);
					}
					else
					{
						this.ShowMsg("修改密码失败！(" + jsonValue2 + ")", false);
					}
				}
				else
				{
					this.ShowMsg("修改密码失败！", false);
				}
				this.enable = masterSettings2.EnableSaleService;
				return;
			}
			this.ShowMsg("获取access_token失败！", false);
		}

		protected void btnBindUser_Click(object sender, System.EventArgs e)
		{
			string text = this.txt_phone.Text.Trim();
			string text2 = this.txt_pwd.Text;
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请输入邮箱！", false);
			}
			if (string.IsNullOrEmpty(text2))
			{
				this.ShowMsg("请输入密码！", false);
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			string value = string.Empty;
			if (!string.IsNullOrEmpty(masterSettings.DistributorLogoPic))
			{
				value = Globals.DomainName + masterSettings.DistributorLogoPic;
			}
			CustomerServiceSettings masterSettings2 = CustomerServiceManager.GetMasterSettings(false);
			string tokenValue = TokenApi.GetTokenValue(masterSettings2.AppId, masterSettings2.AppSecret);
			string value2 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(text2, "MD5").ToLower();
			if (!string.IsNullOrEmpty(tokenValue))
			{
				string text3 = EnterpriseApi.CreateEnterprise(tokenValue, new System.Collections.Generic.Dictionary<string, string>
				{
					{
						"unit",
						text
					},
					{
						"password",
						value2
					},
					{
						"unitname",
						masterSettings.SiteName
					},
					{
						"activated",
						"1"
					},
					{
						"logo",
						value
					},
					{
						"url",
						""
					},
					{
						"tel",
						masterSettings.ShopTel
					},
					{
						"contact",
						""
					},
					{
						"location",
						""
					}
				});
				if (!string.IsNullOrWhiteSpace(text3))
				{
					string jsonValue = Hishop.MeiQia.Api.Util.Common.GetJsonValue(text3, "errcode");
                    Hishop.MeiQia.Api.Util.Common.GetJsonValue(text3, "errmsg");
					if (jsonValue == "10020")
					{
						string unitId = EnterpriseApi.GetUnitId(tokenValue, text);
						if (!string.IsNullOrEmpty(unitId))
						{
							masterSettings2.unitid = unitId;
							masterSettings2.unit = this.txt_phone.Text;
							masterSettings2.password = this.txt_pwd.Text;
							CustomerServiceManager.Save(masterSettings2);
							this.ShowMsgAndReUrl("绑定成功。", true, "SaleService.aspx");
							return;
						}
					}
					else
					{
						this.ShowMsgAndReUrl("账号不存在！", true, "SaleService.aspx");
					}
				}
			}
		}

		protected void btnClear_Click(object sender, System.EventArgs e)
		{
			try
			{
				CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
				if (masterSettings != null)
				{
					masterSettings.unitid = "";
					masterSettings.unit = "";
					masterSettings.password = "";
					CustomerServiceManager.Save(masterSettings);
					this.ShowMsgAndReUrl("已回到初始状态，请重新配置在线客服！", true, "SaleService.aspx");
				}
			}
			catch (System.Exception)
			{
			}
		}
	}
}
