using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Oneyuan
{
	public class AddOneyuanInfo : AdminPage
	{
		protected string EditId = "";

		protected OneTaoState OneTaoState = OneTaoState.NONE;

		protected string EditJsonDataStr = "EditJsonData=''";

		protected string viewAid = "";

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtEditInfo;

		protected OneTaoViewTab ViewTab1;

		protected System.Web.UI.HtmlControls.HtmlImage idImg;

		protected SetMemberRange SetMemberRange;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["action"];
			if (!string.IsNullOrEmpty(text))
			{
				this.AjaxAction(text);
			}
			this.EditId = base.Request.QueryString["aid"];
			this.viewAid = base.Request.QueryString["vaid"];
			if (string.IsNullOrEmpty(this.EditId))
			{
				this.EditId = this.viewAid;
			}
			if (!string.IsNullOrEmpty(this.EditId))
			{
				OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(this.EditId);
				if (oneyuanTaoInfoById != null)
				{
					this.SetMemberRange.Grade = oneyuanTaoInfoById.FitMember;
					this.SetMemberRange.DefualtGroup = oneyuanTaoInfoById.DefualtGroup;
					this.SetMemberRange.CustomGroup = oneyuanTaoInfoById.CustomGroup;
					this.OneTaoState = OneyuanTaoHelp.getOneTaoState(oneyuanTaoInfoById);
					if (this.OneTaoState == OneTaoState.已结束 && string.IsNullOrEmpty(this.viewAid))
					{
						base.Response.Redirect("OneyuanList.aspx");
						base.Response.End();
					}
					ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(oneyuanTaoInfoById.ProductId);
					if (productBaseInfo != null)
					{
						oneyuanTaoInfoById.MaxPrice = productBaseInfo.MarketPrice.Value;
						oneyuanTaoInfoById.storeKc = ProductHelper.GetProductSumStock(oneyuanTaoInfoById.ProductId);
					}
					this.EditJsonDataStr = "EditJsonData=" + JsonConvert.SerializeObject(oneyuanTaoInfoById, new JsonConverter[]
					{
						new IsoDateTimeConverter
						{
							DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
						}
					});
					if (!string.IsNullOrEmpty(this.viewAid))
					{
						this.txtEditInfo.InnerHtml = "查看一元夺宝<small>当前为查看模式，不可编辑活动内容</small>";
						return;
					}
					this.txtEditInfo.InnerHtml = "编辑一元夺宝<small>进行中的活动，只可以修改活动标题及活动详情，其它信息不能修改</small>";
				}
			}
		}

		private void AjaxAction(string action)
		{
			string s = "{\"state\":false,\"msg\":\"未定义操作\"}";
			System.Collections.Specialized.NameValueCollection form = base.Request.Form;
			if (action != null)
			{
				if (!(action == "edit"))
				{
					if (!(action == "save"))
					{
						if (!(action == "read"))
						{
							goto IL_417;
						}
					}
					else
					{
						try
						{
							OneyuanTaoInfo oneyuanTaoInfo = new OneyuanTaoInfo();
							oneyuanTaoInfo.IsOn = true;
							oneyuanTaoInfo.IsEnd = false;
							oneyuanTaoInfo.PrizeNumber = int.Parse(form["PrizeNumber"]);
							oneyuanTaoInfo.ActivityDec = form["ActivityDec"];
							oneyuanTaoInfo.Title = form["Title"];
							oneyuanTaoInfo.StartTime = System.DateTime.Parse(form["StartTime"]);
							oneyuanTaoInfo.EndTime = System.DateTime.Parse(form["EndTime"]);
							oneyuanTaoInfo.EachPrice = decimal.Parse(form["EachPrice"]);
							oneyuanTaoInfo.ReachNum = int.Parse(form["ReachNum"]);
							oneyuanTaoInfo.EachCanBuyNum = int.Parse(form["EachCanBuyNum"]);
							oneyuanTaoInfo.FitMember = form["FitMember"];
							oneyuanTaoInfo.DefualtGroup = form["DefualtGroup"];
							oneyuanTaoInfo.CustomGroup = form["CustomGroup"];
							oneyuanTaoInfo.FinishedNum = 0;
							oneyuanTaoInfo.SkuId = "N";
							oneyuanTaoInfo.ProductImg = form["ProductImg"];
							oneyuanTaoInfo.ProductId = int.Parse(form["ProductId"]);
							oneyuanTaoInfo.HeadImgage = form["HeadImgage"];
							oneyuanTaoInfo.ReachType = int.Parse(form["ReachType"]);
							oneyuanTaoInfo.ProductPrice = decimal.Parse(form["ProductPrice"]);
							oneyuanTaoInfo.ProductTitle = form["ProductTitle"];
							if (oneyuanTaoInfo.ActivityDec.Length > 100)
							{
								s = "{\"state\":false,\"msg\":\"活动描述信息太长！\"}";
							}
							else if (OneyuanTaoHelp.AddOneyuanTao(oneyuanTaoInfo))
							{
								s = "{\"state\":true,\"msg\":\"保存活动成功\"}";
							}
							else
							{
								s = "{\"state\":false,\"msg\":\"保存活动失败\"}";
							}
							goto IL_417;
						}
						catch (System.Exception ex)
						{
							s = "{\"state\":false,\"msg\":\"" + ex.Message.Replace("'", " ").Replace("\r\n", " ") + "\"}";
							goto IL_417;
						}
					}
					s = "{\"state\":false,\"msg\":\"读取数据\"}";
				}
				else
				{
					OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(form["ActivityId"]);
					if (oneyuanTaoInfoById != null)
					{
						this.OneTaoState = OneyuanTaoHelp.getOneTaoState(oneyuanTaoInfoById);
						if (this.OneTaoState == OneTaoState.已结束)
						{
							s = "{\"state\":false,\"msg\":\"当前活动已结束，不能再修改！\"}";
						}
						else
						{
							oneyuanTaoInfoById.ActivityDec = form["ActivityDec"];
							oneyuanTaoInfoById.Title = form["Title"];
							oneyuanTaoInfoById.FitMember = form["FitMember"];
							oneyuanTaoInfoById.DefualtGroup = form["DefualtGroup"];
							oneyuanTaoInfoById.CustomGroup = form["CustomGroup"];
							oneyuanTaoInfoById.HeadImgage = form["HeadImgage"];
							if (this.OneTaoState == OneTaoState.未开始)
							{
								oneyuanTaoInfoById.ProductId = int.Parse(form["ProductId"]);
								oneyuanTaoInfoById.StartTime = System.DateTime.Parse(form["StartTime"]);
								oneyuanTaoInfoById.EndTime = System.DateTime.Parse(form["EndTime"]);
								oneyuanTaoInfoById.EachPrice = decimal.Parse(form["EachPrice"]);
								oneyuanTaoInfoById.ReachNum = int.Parse(form["ReachNum"]);
								oneyuanTaoInfoById.EachCanBuyNum = int.Parse(form["EachCanBuyNum"]);
								oneyuanTaoInfoById.PrizeNumber = int.Parse(form["PrizeNumber"]);
								oneyuanTaoInfoById.ReachType = int.Parse(form["ReachType"]);
								oneyuanTaoInfoById.ProductPrice = decimal.Parse(form["ProductPrice"]);
								oneyuanTaoInfoById.ProductTitle = form["ProductTitle"];
								oneyuanTaoInfoById.FinishedNum = 0;
								oneyuanTaoInfoById.SkuId = "N";
								oneyuanTaoInfoById.ProductImg = form["ProductImg"];
							}
							if (oneyuanTaoInfoById.ActivityDec.Length > 100)
							{
								s = "{\"state\":false,\"msg\":\"活动描述信息太长！\"}";
							}
							else if (OneyuanTaoHelp.UpdateOneyuanTao(oneyuanTaoInfoById))
							{
								s = "{\"state\":true,\"msg\":\"活动修改成功！\"}";
							}
							else
							{
								s = "{\"state\":false,\"msg\":\"修改失败！\"}";
							}
						}
					}
					else
					{
						s = "{\"state\":false,\"msg\":\"活动信息不存在，可能已删除！\"}";
					}
				}
			}
			IL_417:
			base.Response.ClearContent();
			base.Response.ContentType = "application/json";
			base.Response.Write(s);
			base.Response.End();
		}

		protected AddOneyuanInfo() : base("m08", "yxp20")
		{
		}
	}
}
