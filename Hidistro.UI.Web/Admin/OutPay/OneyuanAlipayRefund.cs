using Hidistro.ControlPanel.OutPay.App;
using Hidistro.Core;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin.OutPay
{
	public class OneyuanAlipayRefund : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Collections.Generic.SortedDictionary<string, string> requestPost = this.GetRequestPost();
			Globals.Debuglog(base.Request.Form.ToString(), "_Debuglog.txt");
			if (requestPost.Count <= 0)
			{
				base.Response.Write("无通知参数");
				return;
			}
			Notify notify = new Notify();
			bool flag = notify.Verify(requestPost, base.Request.Form["notify_id"], base.Request.Form["sign"]);
			if (flag)
			{
				string text = base.Request.Form["batch_no"];
				string arg_96_0 = base.Request.Form["success_num"];
				string text2 = base.Request.Form["result_details"];
				if (!OneyuanTaoHelp.IsExistAlipayRefundNUm(text) && !string.IsNullOrEmpty(text2))
				{
					try
					{
						string[] array = text2.Split(new char[]
						{
							'#'
						});
						System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string text3 = array2[i];
							string[] array3 = text3.Split(new char[]
							{
								'^'
							});
							OneyuanTaoParticipantInfo addParticipant = OneyuanTaoHelp.GetAddParticipant(0, "", array3[0].Trim());
							if (addParticipant != null)
							{
								if (text3.Contains("SUCCESS"))
								{
									list.Add(addParticipant.ActivityId);
									addParticipant.Remark = "退款成功";
									addParticipant.RefundNum = text;
									OneyuanTaoHelp.SetRefundinfo(addParticipant);
								}
								else
								{
									addParticipant.Remark = "退款失败:" + text3;
									OneyuanTaoHelp.SetRefundinfoErr(addParticipant);
								}
							}
						}
						list = list.Distinct<string>().ToList<string>();
						if (list.Count > 0)
						{
							OneyuanTaoHelp.SetIsAllRefund(list);
						}
					}
					catch (System.Exception ex)
					{
						Globals.Debuglog(base.Request.Form.ToString() + ":exception->" + ex.Message, "_Debuglog.txt");
					}
				}
				base.Response.Write("success");
				return;
			}
			base.Response.Write("fail");
			Globals.Debuglog(base.Request.Form.ToString(), "alipayrefun.txt");
		}

		public System.Collections.Generic.SortedDictionary<string, string> GetRequestPost()
		{
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			System.Collections.Specialized.NameValueCollection form = base.Request.Form;
			string[] allKeys = form.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				sortedDictionary.Add(allKeys[i], base.Request.Form[allKeys[i]]);
			}
			return sortedDictionary;
		}
	}
}
