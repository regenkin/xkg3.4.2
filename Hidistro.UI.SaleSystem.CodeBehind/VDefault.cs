using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VDefault : VshopTemplatedWebControl
	{
		protected int itemcount = 0;

		private VshopTemplatedRepeater rptProducts;

		private HiImage imglogo;

		private System.Web.UI.HtmlControls.HtmlImage img;

		private System.Web.UI.WebControls.Literal litstorename;

		private System.Web.UI.WebControls.Literal litdescription;

		private System.Web.UI.WebControls.Literal litImgae;

		private System.Web.UI.WebControls.Literal litItemParams;

		private System.Web.UI.WebControls.Literal litattention;

		private Pager pager;

		private DataTable dtpromotion = null;

		private VshopTemplatedRepeater rptCategories;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VDefault.html";
			}
			base.OnInit(e);
		}

		private void rptCategories_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				if ((e.Item.ItemIndex + 1) % 4 == 1)
				{
					System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litStart");
					literal.Visible = true;
				}
				else if ((e.Item.ItemIndex + 1) % 4 == 0 || e.Item.ItemIndex + 1 == this.itemcount)
				{
					System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litEnd");
					literal2.Visible = true;
				}
				System.Web.UI.WebControls.Literal literal3 = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litpromotion");
				if (!string.IsNullOrEmpty(literal3.Text))
				{
					literal3.Text = "<img src='" + literal3.Text + "'/>";
				}
				else
				{
					literal3.Text = "<img src='/Storage/master/default.png'/>";
				}
			}
		}

		private void rptProducts_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.Controls[0].FindControl("litpromotion");
				string text = "";
				if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "MainCategoryPath") != null)
				{
					text = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "MainCategoryPath").ToString();
				}
				DataView defaultView = this.dtpromotion.DefaultView;
				if (!string.IsNullOrEmpty(text))
				{
					defaultView.RowFilter = " ActivitiesType=0 ";
					if (defaultView.Count > 0)
					{
						literal.Text = string.Concat(new string[]
						{
							"<span class=\"sale-favourable\"><i>满",
							decimal.Parse(defaultView[0]["MeetMoney"].ToString()).ToString("0"),
							"</i><i>减",
							decimal.Parse(defaultView[0]["ReductionMoney"].ToString()).ToString("0"),
							"</i></span>"
						});
					}
					else
					{
						defaultView.RowFilter = " ActivitiesType= " + text.Split(new char[]
						{
							'|'
						})[0].ToString();
						if (defaultView.Count > 0)
						{
							literal.Text = string.Concat(new string[]
							{
								"<span class=\"sale-favourable\"><i>满",
								decimal.Parse(defaultView[0]["MeetMoney"].ToString()).ToString("0"),
								"</i><i>减",
								decimal.Parse(defaultView[0]["ReductionMoney"].ToString()).ToString("0"),
								"</i></span>"
							});
						}
					}
				}
			}
		}

		protected override void AttachChildControls()
		{
			this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
			this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
			this.rptProducts.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptProducts_ItemDataBound);
			this.rptCategories.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptCategories_ItemDataBound);
			this.img = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("imgDefaultBg");
			this.pager = (Pager)this.FindControl("pager");
			this.litstorename = (System.Web.UI.WebControls.Literal)this.FindControl("litstorename");
			this.litdescription = (System.Web.UI.WebControls.Literal)this.FindControl("litdescription");
			this.litattention = (System.Web.UI.WebControls.Literal)this.FindControl("litattention");
			this.imglogo = (HiImage)this.FindControl("imglogo");
			this.litImgae = (System.Web.UI.WebControls.Literal)this.FindControl("litImgae");
			this.litItemParams = (System.Web.UI.WebControls.Literal)this.FindControl("litItemParams");
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralId"]))
			{
				System.Web.HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					this.Page.Response.Redirect("Default.aspx?ReferralId=" + httpCookie.Value);
				}
			}
			if (this.rptCategories.Visible)
			{
				DataTable brandCategories = CategoryBrowser.GetBrandCategories();
				this.itemcount = brandCategories.Rows.Count;
				if (brandCategories.Rows.Count > 0)
				{
					this.rptCategories.DataSource = brandCategories;
					this.rptCategories.DataBind();
				}
			}
			this.Page.Session["stylestatus"] = "3";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			PageTitle.AddSiteNameTitle(masterSettings.SiteName);
			this.litstorename.Text = masterSettings.SiteName;
			this.litdescription.Text = masterSettings.ShopIntroduction;
			if (!string.IsNullOrEmpty(masterSettings.DistributorLogoPic))
			{
				this.imglogo.ImageUrl = masterSettings.DistributorLogoPic.Split(new char[]
				{
					'|'
				})[0];
			}
			if (this.referralId <= 0)
			{
				System.Web.HttpCookie httpCookie2 = System.Web.HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
				if (httpCookie2 != null && !string.IsNullOrEmpty(httpCookie2.Value))
				{
					this.referralId = int.Parse(httpCookie2.Value);
					this.Page.Response.Redirect("Default.aspx?ReferralId=" + this.referralId.ToString(), true);
				}
			}
			else
			{
				System.Web.HttpCookie httpCookie2 = System.Web.HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
				if (httpCookie2 != null && !string.IsNullOrEmpty(httpCookie2.Value) && this.referralId.ToString() != httpCookie2.Value)
				{
					this.Page.Response.Redirect("Default.aspx?ReferralId=" + this.referralId.ToString(), true);
				}
			}
			System.Collections.Generic.IList<BannerInfo> list = new System.Collections.Generic.List<BannerInfo>();
			list = VshopBrowser.GetAllBanners();
			foreach (BannerInfo current in list)
			{
				TplCfgInfo tplCfgInfo = new NavigateInfo();
				tplCfgInfo.LocationType = current.LocationType;
				tplCfgInfo.Url = current.Url;
				string text = "javascript:";
				if (!string.IsNullOrEmpty(current.Url))
				{
					text = tplCfgInfo.LoctionUrl;
				}
				System.Web.UI.WebControls.Literal expr_3E2 = this.litImgae;
				string text2 = expr_3E2.Text;
				expr_3E2.Text = string.Concat(new string[]
				{
					text2,
					"<a  id=\"ahref\" href='",
					text,
					"'><img src=\"",
					current.ImageUrl,
					"\" title=\"",
					current.ShortDesc,
					"\" alt=\"",
					current.ShortDesc,
					"\" /></a>"
				});
			}
			if (list.Count == 0)
			{
				this.litImgae.Text = "<a id=\"ahref\"  href='javascript:'><img src=\"/Utility/pics/default.jpg\" title=\"\"  /></a>";
			}
			DistributorsInfo distributorsInfo = new DistributorsInfo();
			distributorsInfo = DistributorsBrower.GetUserIdDistributors(this.referralId);
			if (distributorsInfo != null && distributorsInfo.UserId > 0)
			{
				PageTitle.AddSiteNameTitle(distributorsInfo.StoreName);
				this.litdescription.Text = distributorsInfo.StoreDescription;
				this.litstorename.Text = distributorsInfo.StoreName;
				if (!string.IsNullOrEmpty(distributorsInfo.Logo))
				{
					this.imglogo.ImageUrl = distributorsInfo.Logo;
				}
				else if (!string.IsNullOrEmpty(masterSettings.DistributorLogoPic))
				{
					this.imglogo.ImageUrl = masterSettings.DistributorLogoPic.Split(new char[]
					{
						'|'
					})[0];
				}
				if (!string.IsNullOrEmpty(distributorsInfo.BackImage))
				{
					this.litImgae.Text = "";
					string[] array = distributorsInfo.BackImage.Split(new char[]
					{
						'|'
					});
					for (int i = 0; i < array.Length; i++)
					{
						string text3 = array[i];
						if (!string.IsNullOrEmpty(text3))
						{
							System.Web.UI.WebControls.Literal expr_5D7 = this.litImgae;
							expr_5D7.Text = expr_5D7.Text + "<a ><img src=\"" + text3 + "\" title=\"\"  /></a>";
						}
					}
				}
			}
			this.dtpromotion = ProductBrowser.GetAllFull();
			if (this.rptProducts != null)
			{
				ProductQuery productQuery = new ProductQuery();
				productQuery.PageSize = this.pager.PageSize;
				productQuery.PageIndex = this.pager.PageIndex;
				productQuery.SortBy = "DisplaySequence";
				productQuery.SortOrder = SortAction.Desc;
				DbQueryResult homeProduct = ProductBrowser.GetHomeProduct(MemberProcessor.GetCurrentMember(), productQuery);
				this.rptProducts.DataSource = homeProduct.Data;
				this.rptProducts.DataBind();
				this.pager.TotalRecords = homeProduct.TotalRecords;
				if (this.pager.TotalRecords <= this.pager.PageSize)
				{
					this.pager.Visible = false;
				}
			}
			if (this.img != null)
			{
				this.img.Src = new VTemplateHelper().GetDefaultBg();
			}
			if (!string.IsNullOrEmpty(masterSettings.GuidePageSet))
			{
				this.litattention.Text = masterSettings.GuidePageSet;
			}
			string userAgent = this.Page.Request.UserAgent;
			if (userAgent.ToLower().Contains("alipay") && !string.IsNullOrEmpty(masterSettings.AliPayFuwuGuidePageSet))
			{
				if (!string.IsNullOrEmpty(masterSettings.GuidePageSet))
				{
					this.litattention.Text = masterSettings.AliPayFuwuGuidePageSet;
				}
			}
			string text4 = "";
			if (!string.IsNullOrEmpty(masterSettings.ShopHomePic))
			{
				text4 = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + masterSettings.ShopHomePic;
			}
			string text5 = "";
			string text6 = (distributorsInfo == null) ? masterSettings.SiteName : distributorsInfo.StoreName;
			if (!string.IsNullOrEmpty(masterSettings.DistributorBackgroundPic))
			{
				text5 = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + masterSettings.DistributorBackgroundPic.Split(new char[]
				{
					'|'
				})[0];
			}
			this.litItemParams.Text = string.Concat(new string[]
			{
				text4,
				"|",
				masterSettings.ShopHomeName,
				"|",
				masterSettings.ShopHomeDescription,
				"$"
			});
			this.litItemParams.Text = string.Concat(new object[]
			{
				this.litItemParams.Text,
				text5,
				"|好店推荐之",
				text6,
				"商城|一个购物赚钱的好去处|",
				System.Web.HttpContext.Current.Request.Url
			});
		}
	}
}
