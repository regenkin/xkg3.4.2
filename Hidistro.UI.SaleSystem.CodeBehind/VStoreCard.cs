using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VStoreCard : VshopTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlImage imglogo;

		private System.Web.UI.HtmlControls.HtmlInputHidden ShareInfo;

		private System.Web.UI.HtmlControls.HtmlControl editPanel;

		private int userId = 0;

		protected override void OnInit(System.EventArgs e)
		{
			string a = System.Web.HttpContext.Current.Request["action"];
			if (a == "ReCreadt")
			{
				System.Web.HttpContext.Current.Response.ContentType = "application/json";
				string text = System.Web.HttpContext.Current.Request["imageUrl"];
				string s = "";
				if (string.IsNullOrEmpty(text))
				{
					s = "{\"success\":\"false\",\"message\":\"图片地址为空\"}";
				}
				try
				{
					MemberInfo currentMember = MemberProcessor.GetCurrentMember();
					DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMember.UserId);
					string setJson = System.IO.File.ReadAllText(System.Web.HttpRuntime.AppDomainAppPath.ToString() + "Storage/Utility/StoreCardSet.js");
					StoreCardCreater storeCardCreater = new StoreCardCreater(setJson, text, text, Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + "/Follow.aspx?ReferralId=" + this.userId.ToString(), currentMember.UserName, userIdDistributors.StoreName, currentMember.UserId);
					string text2 = "";
					if (storeCardCreater.ReadJson() && storeCardCreater.CreadCard(out text2))
					{
						s = "{\"success\":\"true\",\"message\":\"生成成功\"}";
						DistributorsBrower.UpdateStoreCard(this.userId, text2);
					}
					else
					{
						s = "{\"success\":\"false\",\"message\":\"" + text2 + "\"}";
					}
				}
				catch (System.Exception ex)
				{
					s = "{\"success\":\"false\",\"message\":\"" + ex.Message + "\"}";
				}
				System.Web.HttpContext.Current.Response.Write(s);
				System.Web.HttpContext.Current.Response.End();
			}
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VStoreCard.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["ReferralId"], out this.userId))
			{
				base.GotoResourceNotFound("");
			}
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(this.userId);
			if (userIdDistributors == null)
			{
				base.GotoResourceNotFound("");
			}
			this.imglogo = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("QrcodeImg");
			int currentMemberUserId = Globals.GetCurrentMemberUserId();
			this.editPanel = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("editPanel");
			this.editPanel.Visible = false;
			if (currentMemberUserId == this.userId)
			{
				this.imglogo.Attributes.Add("Admin", "true");
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				System.DateTime cardCreatTime = userIdDistributors.CardCreatTime;
				string text = System.IO.File.ReadAllText(System.Web.HttpRuntime.AppDomainAppPath.ToString() + "Storage/Utility/StoreCardSet.js");
				JObject jObject = JsonConvert.DeserializeObject(text) as JObject;
				System.DateTime t = default(System.DateTime);
				if (jObject != null && jObject["writeDate"] != null)
				{
					t = System.DateTime.Parse(jObject["writeDate"].ToString());
				}
				if (string.IsNullOrEmpty(userIdDistributors.StoreCard) || cardCreatTime < t)
				{
					StoreCardCreater storeCardCreater = new StoreCardCreater(text, currentMember.UserHead, userIdDistributors.Logo, Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + "/Follow.aspx?ReferralId=" + this.userId.ToString(), currentMember.UserName, userIdDistributors.StoreName, this.userId);
					string imgUrl = "";
					if (storeCardCreater.ReadJson() && storeCardCreater.CreadCard(out imgUrl))
					{
						DistributorsBrower.UpdateStoreCard(this.userId, imgUrl);
					}
				}
			}
			if (string.IsNullOrEmpty(userIdDistributors.StoreCard))
			{
				userIdDistributors.StoreCard = "/Storage/master/DistributorCards/StoreCard" + this.userId.ToString() + ".jpg";
			}
			this.ShareInfo = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("ShareInfo");
			this.imglogo.Src = userIdDistributors.StoreCard;
			PageTitle.AddSiteNameTitle("掌柜名片");
		}
	}
}
