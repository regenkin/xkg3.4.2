using Hidistro.ControlPanel.OutPay.App;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin.OutPay
{
	public class AliPaynotify_url : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Collections.Generic.SortedDictionary<string, string> requestPost = this.GetRequestPost();
			if (requestPost.Count > 0)
			{
				Notify notify = new Notify();
				bool flag = notify.Verify(requestPost, base.Request.Form["notify_id"], base.Request.Form["sign"]);
				if (flag)
				{
					string text = base.Request.Form["success_details"];
					try
					{
						if (!string.IsNullOrEmpty(text))
						{
							string[] array = text.Split(new char[]
							{
								'|'
							});
							string[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								string text2 = array2[i];
								string[] array3 = text2.Split(new char[]
								{
									'^'
								});
								if (array3.Length >= 8)
								{
									BalanceDrawRequestInfo balanceDrawRequestById = DistributorsBrower.GetBalanceDrawRequestById(array3[0]);
									if (balanceDrawRequestById != null && balanceDrawRequestById.IsCheck != "2")
									{
										VShopHelper.UpdateBalanceDrawRequest(int.Parse(array3[0]), "支付宝付款：流水号" + array3[6] + ",支付时间：" + array3[7]);
										VShopHelper.UpdateBalanceDistributors(balanceDrawRequestById.UserId, decimal.Parse(array3[3]));
									}
								}
							}
						}
						string text3 = base.Request.Form["fail_details"];
						if (!string.IsNullOrEmpty(text3))
						{
							string[] array4 = text3.Split(new char[]
							{
								'|'
							});
							string[] array5 = array4;
							for (int j = 0; j < array5.Length; j++)
							{
								string text4 = array5[j];
								string[] array6 = text4.Split(new char[]
								{
									'^'
								});
								if (array6.Length >= 8)
								{
									BalanceDrawRequestInfo balanceDrawRequestById2 = DistributorsBrower.GetBalanceDrawRequestById(array6[0]);
									if (balanceDrawRequestById2 != null && balanceDrawRequestById2.IsCheck != "2")
									{
										DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(new int[]
										{
											int.Parse(array6[0])
										}, 3, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + array6[5] + array6[6], array6[3]);
									}
								}
							}
						}
					}
					catch (System.Exception)
					{
						try
						{
							string path = Globals.MapPath("/Admin/OutPay/App/") + "payLog.txt";
							using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(path, true, System.Text.Encoding.UTF8))
							{
								streamWriter.Write(System.DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "支付成功，写入数据库失败->" + base.Request.Form.ToString() + "\r\n");
							}
						}
						catch (System.Exception)
						{
						}
					}
					base.Response.Write("success");
					return;
				}
				base.Response.Write("fail");
				try
				{
					string path2 = Globals.MapPath("/Admin/OutPay/App/") + "payLog.txt";
					using (System.IO.StreamWriter streamWriter2 = new System.IO.StreamWriter(path2, true, System.Text.Encoding.UTF8))
					{
						streamWriter2.Write(System.DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "支付失败->" + base.Request.Form.ToString() + "\r\n");
					}
					return;
				}
				catch (System.Exception)
				{
					return;
				}
			}
			base.Response.Write("无通知参数");
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
