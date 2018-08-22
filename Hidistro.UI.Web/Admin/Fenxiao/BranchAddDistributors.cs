using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class BranchAddDistributors : AdminPage
	{
		private static System.Collections.Generic.IList<string> exportdistirbutors;

		protected Hidistro.UI.Common.Controls.Style Style1;

		protected System.Web.UI.HtmlControls.HtmlGenericControl EditTitle;

		protected System.Web.UI.HtmlControls.HtmlInputRadioButton radionumber;

		protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioaccount;

		protected System.Web.UI.HtmlControls.HtmlInputText txtslsdistributors;

		protected System.Web.UI.HtmlControls.HtmlInputText txtnumber;

		protected System.Web.UI.HtmlControls.HtmlTextArea txtdistributornames;

		protected System.Web.UI.WebControls.Button batchCreate;

		protected System.Web.UI.WebControls.Button btnExport;

		protected BranchAddDistributors() : base("m05", "fxp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request["action"]) && base.Request["action"] == "SearchKey")
			{
				string str = string.Empty;
				if (!string.IsNullOrEmpty(base.Request["keyword"]))
				{
					str = MemberHelper.GetAllDistributorsName(base.Request["keyword"]);
				}
				base.Response.ContentType = "application/json";
				base.Response.Write("{\"data\":[" + str + "]}");
				base.Response.End();
			}
			if (!base.IsPostBack)
			{
				BranchAddDistributors.exportdistirbutors = null;
			}
			this.batchCreate.Click += new System.EventHandler(this.batchCreate_Click);
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
		}

		private void btnExport_Click(object sender, System.EventArgs e)
		{
			this.Page.Response.Clear();
			this.Page.Response.Buffer = true;
			this.Page.Response.Charset = "GB2312";
			this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=tempdistributors.txt");
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			base.Response.ContentType = "text/plain";
			this.EnableViewState = false;
			System.Globalization.CultureInfo formatProvider = new System.Globalization.CultureInfo("ZH-CN", true);
			new System.IO.StringWriter(formatProvider);
			this.Page.Response.Write(string.Join("\r\n", BranchAddDistributors.exportdistirbutors));
			this.Page.Response.End();
		}

		private System.Collections.Generic.IList<string> CreateDistributros(int len)
		{
			System.Collections.Generic.IList<string> list = new System.Collections.Generic.List<string>();
			System.Random random = new System.Random(System.Environment.TickCount);
			for (int i = 0; i < len; i++)
			{
				list.Add(random.Next(11111111, 99999999).ToString());
			}
			return list;
		}

		private void batchCreate_Click(object sender, System.EventArgs e)
		{
			try
			{
				string value = this.txtslsdistributors.Value;
				int num = MemberHelper.IsExiteDistributorNames(value);
				if (string.IsNullOrEmpty(value) || num <= 0)
				{
					this.ShowMsg("输入的推荐分销商不存在！", false);
				}
				else if (this.radionumber.Checked)
				{
					if (string.IsNullOrEmpty(this.txtnumber.Value.Trim()))
					{
						this.ShowMsg("请输入要生成的账号数量", false);
					}
					else
					{
						int num2 = 0;
						int.TryParse(this.txtnumber.Value, out num2);
						if (num2 <= 0 || num2 > 999)
						{
							this.ShowMsg("数值必须在1~999之间的正整数", false);
						}
						else if (this.CheckDistributorIsCanAuthorization(num2))
						{
							System.Collections.Generic.IList<string> distributornames = this.CreateDistributros(num2);
							BranchAddDistributors.exportdistirbutors = MemberHelper.BatchCreateMembers(distributornames, num, "1");
							this.ShowMsg("批量制作成功", true);
							if (BranchAddDistributors.exportdistirbutors != null && BranchAddDistributors.exportdistirbutors.Count > 0)
							{
								this.btnExport.Visible = true;
								this.btnExport.Text = "导出分销商";
							}
						}
					}
				}
				else
				{
					string value2 = this.txtdistributornames.Value;
					System.Collections.Generic.IList<string> list = new System.Collections.Generic.List<string>();
					if (string.IsNullOrEmpty(value2))
					{
						this.ShowMsg("请输入要制作的账号", false);
					}
					else
					{
						bool flag = false;
						string[] array = value2.Split(new string[]
						{
							"\r\n"
						}, System.StringSplitOptions.None);
						if (this.CheckDistributorIsCanAuthorization(array.Count<string>()))
						{
							string[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								string text = array2[i];
								if (string.IsNullOrEmpty(text) || text.Length < 6 || text.Length > 50)
								{
									flag = true;
									break;
								}
								list.Add(text);
							}
							if (flag)
							{
								this.ShowMsg("每个账号长度在2~10个字符", false);
							}
							else
							{
								BranchAddDistributors.exportdistirbutors = MemberHelper.BatchCreateMembers(list, num, "2");
								if (BranchAddDistributors.exportdistirbutors != null && BranchAddDistributors.exportdistirbutors.Count > 0)
								{
									this.btnExport.Visible = true;
									this.btnExport.Text = "导出失败分销商";
									this.ShowMsg("部份分销商生成失败，请查看导出文档！", true);
								}
								else
								{
									this.btnExport.Visible = false;
									this.ShowMsg("生成成功！", true);
								}
							}
						}
					}
				}
			}
			catch (System.Exception)
			{
				throw;
			}
		}

		private bool CheckDistributorIsCanAuthorization(int number)
		{
			int num = 0;
			if (!SystemAuthorizationHelper.CheckDistributorIsCanAuthorization(number, out num))
			{
				this.ShowMsg(string.Format("对不起，你最多只能再生成{0}个分销商！请确认后重试 ", num), false);
				return false;
			}
			return true;
		}
	}
}
