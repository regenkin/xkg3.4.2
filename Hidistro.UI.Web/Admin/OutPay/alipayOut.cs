using Hidistro.ControlPanel.OutPay.App;
using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin.OutPay
{
	public class alipayOut : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			string alipay_Pid = masterSettings.Alipay_Pid;
			string alipay_Key = masterSettings.Alipay_Key;
			string alipay_mid = masterSettings.Alipay_mid;
			string alipay_mName = masterSettings.Alipay_mName;
			string text = "utf-8";
            Hidistro.ControlPanel.OutPay.App.Core.setConfig(alipay_Pid, "MD5", alipay_Key, text);
			string value = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + "/admin/OutPay/AliPaynotify_url.aspx";
			string value2 = System.DateTime.Now.ToString("yyyyMMdd");
			string value3 = System.DateTime.Now.ToString("yyyyMMddHHmmssff");
			decimal d = 0m;
			int num = 0;
			string text2 = base.Request.Form["Paydata"];
			if (string.IsNullOrEmpty(text2))
			{
				base.Response.Write("<span style='line-height:40px;color:red;padding:10px'>付款申请参数不能为空！</span>");
				base.Response.End();
			}
			string s = "";
			string[] array = text2.Split(new char[]
			{
				'|'
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text3 = array2[i];
				string[] array3 = text3.Split(new char[]
				{
					'^'
				});
				if (array3.Length == 5)
				{
					decimal d2 = 0m;
					if (decimal.TryParse(array3[3], out d2))
					{
						d += d2;
						num++;
					}
					else
					{
						if (array3[1].Length < 4)
						{
							s = string.Concat(new string[]
							{
								"<span style='line-height:40px;color:red;padding:10px'>第",
								(num + 1).ToString(),
								"帐户名不正确,请检查：",
								text3,
								"</span>"
							});
							break;
						}
						s = string.Concat(new string[]
						{
							"<span style='line-height:40px;color:red;padding:10px'>第",
							(num + 1).ToString(),
							"项付款参数有误,请检查：",
							text3,
							"</span>"
						});
						break;
					}
				}
			}
			if (num != array.Length)
			{
				base.Response.Write(s);
				base.Response.End();
			}
			string s2 = Hidistro.ControlPanel.OutPay.App.Core.BuildRequest(new System.Collections.Generic.SortedDictionary<string, string>
			{
				{
					"partner",
					alipay_Pid
				},
				{
					"_input_charset",
					text
				},
				{
					"service",
					"batch_trans_notify"
				},
				{
					"notify_url",
					value
				},
				{
					"email",
					alipay_mid
				},
				{
					"account_name",
					alipay_mName
				},
				{
					"pay_date",
					value2
				},
				{
					"batch_no",
					value3
				},
				{
					"batch_fee",
					d.ToString()
				},
				{
					"batch_num",
					num.ToString()
				},
				{
					"detail_data",
					text2
				}
			}, "get", "确认");
			base.Response.Write(s2);
		}
	}
}
