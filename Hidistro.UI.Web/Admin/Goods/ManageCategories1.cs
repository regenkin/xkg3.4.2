using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class ManageCategories1 : AdminPage
	{
		protected System.Web.UI.WebControls.LinkButton btnOrder;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected System.Web.UI.HtmlControls.HtmlInputText txtthird;

		protected System.Web.UI.HtmlControls.HtmlInputText txtsecond;

		protected System.Web.UI.HtmlControls.HtmlInputText txtfirst;

		protected System.Web.UI.HtmlControls.HtmlInputText txtcategoryId;

		protected System.Web.UI.WebControls.Button btnSetCommissions;

		protected ManageCategories1() : base("m02", "spp06")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSetCommissions.Click += new System.EventHandler(this.btnSetCommissions_Click);
			this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
			if (!this.Page.IsPostBack)
			{
				string text = Globals.RequestFormStr("picurl");
				if (!string.IsNullOrEmpty(text))
				{
					byte[] imageContent = this.GetImageContent(text);
					this.WriteResponse(imageContent);
				}
				this.BindData();
			}
		}

		private byte[] GetImageContent(string picurl)
		{
			System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(picurl);
			httpWebRequest.AllowAutoRedirect = true;
			httpWebRequest.Proxy = new System.Net.WebProxy
			{
				BypassProxyOnLocal = true,
				UseDefaultCredentials = true
			};
			System.Net.WebResponse response = httpWebRequest.GetResponse();
			byte[] result;
			using (System.IO.Stream responseStream = response.GetResponseStream())
			{
				using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
				{
					byte[] array = new byte[1024];
					int count;
					while ((count = responseStream.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		private void WriteResponse(byte[] content)
		{
			string s = System.DateTime.Now.ToString("HHmmss") + ".png";
			base.Response.Clear();
			base.Response.ClearHeaders();
			base.Response.Buffer = false;
			base.Response.ContentType = "application/octet-stream";
			base.Response.AppendHeader("Content-Disposition", "attachment;filename=" + base.Server.UrlEncode(s));
			base.Response.AppendHeader("Content-Length", content.Length.ToString());
			base.Response.BinaryWrite(content);
			base.Response.Flush();
			base.Response.End();
		}

		private void btnSetCommissions_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtcategoryId.Value) || System.Convert.ToInt32(this.txtcategoryId.Value) <= 0)
			{
				this.ShowMsg("请选择要编辑的佣金分类", false);
				return;
			}
			CategoryInfo categorys = this.GetCategorys();
			if (categorys != null)
			{
				if (categorys.ParentCategoryId == 0)
				{
					if (CatalogHelper.UpdateCategory(categorys) == CategoryActionStatus.Success)
					{
						HiCache.Remove("DataCache-Categories");
						HiCache.Remove("DataCache-CategoryList");
						this.ShowMsg("保存成功", true);
						this.BindData();
						return;
					}
					this.ShowMsg("保存失败", false);
					return;
				}
				else
				{
					this.ShowMsg("只允许修改顶级类目佣金！", false);
				}
			}
		}

		public string FormatEditeCommission(object ParentCategoryId, object CategoryId, object FirstCommission, object SecondCommission, object ThirdCommission)
		{
			string result = string.Empty;
			if (ParentCategoryId.ToString() == "0")
			{
				result = string.Concat(new object[]
				{
					" onclick=\"EditeCommission(",
					CategoryId,
					",'",
					FirstCommission,
					"','",
					SecondCommission,
					"','",
					ThirdCommission,
					"')\""
				});
			}
			else
			{
				result = " onclick=\"return false\" style=\"display:none\" ";
			}
			return result;
		}

		private CategoryInfo GetCategorys()
		{
			CategoryInfo category = CatalogHelper.GetCategory(System.Convert.ToInt32(this.txtcategoryId.Value));
			if (category == null)
			{
				this.ShowMsg("无法获取当前分类", false);
				return null;
			}
			bool flag = false;
			try
			{
				if (this.txtfirst.Value.Trim() == "")
				{
					category.FirstCommission = "0";
				}
				else if (System.Convert.ToDecimal(this.txtfirst.Value.Trim()) < 0m || System.Convert.ToDecimal(this.txtfirst.Value.Trim()) > 100m)
				{
					this.ShowMsg("输入的佣金格式不正确！", false);
					flag = true;
				}
				else
				{
					category.FirstCommission = System.Convert.ToDecimal(this.txtfirst.Value.Trim()).ToString("F2");
				}
				if (this.txtsecond.Value.Trim() == "")
				{
					category.SecondCommission = "0";
				}
				else if (System.Convert.ToDecimal(this.txtsecond.Value.Trim()) < 0m || System.Convert.ToDecimal(this.txtsecond.Value.Trim()) > 100m)
				{
					this.ShowMsg("输入的佣金格式不正确！", false);
					flag = true;
				}
				else
				{
					category.SecondCommission = System.Convert.ToDecimal(this.txtsecond.Value.Trim()).ToString("F2");
				}
				if (this.txtthird.Value.Trim() == "")
				{
					category.ThirdCommission = "0";
				}
				else if (System.Convert.ToDecimal(this.txtthird.Value.Trim()) < 0m || System.Convert.ToDecimal(this.txtthird.Value.Trim()) > 100m)
				{
					this.ShowMsg("输入的佣金格式不正确！", false);
					flag = true;
				}
				else
				{
					category.ThirdCommission = System.Convert.ToDecimal(this.txtthird.Value.Trim()).ToString("F2");
				}
				if (flag)
				{
					CategoryInfo result = null;
					return result;
				}
			}
			catch (System.Exception)
			{
				this.ShowMsg("输入的佣金格式不正确！", false);
				flag = true;
				if (flag)
				{
					CategoryInfo result = null;
					return result;
				}
			}
			return category;
		}

		private void btnOrder_Click(object sender, System.EventArgs e)
		{
			for (int i = 0; i < this.rptList.Items.Count; i++)
			{
				int num = 0;
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)this.rptList.Items[i].FindControl("txtSequence");
				if (int.TryParse(textBox.Text.Trim(), out num))
				{
					int categoryId = Globals.ToNum(((System.Web.UI.WebControls.HiddenField)this.rptList.Items[i].FindControl("hdfCategoryID")).Value);
					CategoryInfo category = CatalogHelper.GetCategory(categoryId);
					if (category.DisplaySequence != num)
					{
						CatalogHelper.SwapCategorySequence(categoryId, num);
					}
				}
			}
			HiCache.Remove("DataCache-Categories");
			HiCache.Remove("DataCache-CategoryList");
			this.BindData();
			this.ShowMsg("排序保存成功", true);
		}

		protected void rptList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				int num = (int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Depth");
				string text = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Name").ToString();
				if (num == 1)
				{
					text = "<b>" + text + "</b>";
				}
				else
				{
					System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl = e.Item.FindControl("spShowImage") as System.Web.UI.HtmlControls.HtmlGenericControl;
					htmlGenericControl.Visible = false;
				}
				for (int i = 1; i < num; i++)
				{
					text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + text;
				}
				System.Web.UI.WebControls.Literal literal = e.Item.FindControl("lblCategoryName") as System.Web.UI.WebControls.Literal;
				literal.Text = text;
				System.Web.UI.WebControls.HiddenField hiddenField = e.Item.FindControl("hdfCategoryID") as System.Web.UI.WebControls.HiddenField;
				hiddenField.Value = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "CategoryID").ToString();
			}
		}

		private void BindData()
		{
			this.rptList.DataSource = CatalogHelper.GetSequenceCategories();
			this.rptList.DataBind();
		}

		protected void rptList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "DeleteCagetory")
			{
				int categoryId = Globals.ToNum(e.CommandArgument.ToString());
				if (!CatalogHelper.IsExitProduct(categoryId.ToString()))
				{
					if (CatalogHelper.DeleteCategory(categoryId))
					{
						HiCache.Remove("DataCache-Categories");
						HiCache.Remove("DataCache-CategoryList");
						this.ShowMsg("成功删除了指定的分类", true);
					}
					else
					{
						this.ShowMsg("分类删除失败，未知错误", false);
					}
				}
				else
				{
					this.ShowMsg("分类下有商品，请先删除商品再到商品回收站彻底删除。", false);
				}
			}
			this.BindData();
		}
	}
}
