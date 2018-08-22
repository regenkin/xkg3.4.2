using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class AddNineImages : AdminPage
	{
		protected int nid;

		protected System.Web.UI.WebControls.Literal EditType;

		protected AddNineImages() : base("m01", "dpp10")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["task"];
			if (!string.IsNullOrEmpty(text))
			{
				string s = "未定义操作";
				string shareDesc = base.Request.Form["ShareDesc"];
				string image = base.Request.Form["image1"];
				string image2 = base.Request.Form["image2"];
				string image3 = base.Request.Form["image3"];
				string image4 = base.Request.Form["image4"];
				string image5 = base.Request.Form["image5"];
				string image6 = base.Request.Form["image6"];
				string image7 = base.Request.Form["image7"];
				string image8 = base.Request.Form["image8"];
				string image9 = base.Request.Form["image9"];
				string s2 = base.Request.Form["ID"];
				NineImgsesItem nineImgsesItem = new NineImgsesItem();
				nineImgsesItem.CreatTime = System.DateTime.Now;
				nineImgsesItem.ShareDesc = shareDesc;
				nineImgsesItem.image1 = image;
				nineImgsesItem.image2 = image2;
				nineImgsesItem.image3 = image3;
				nineImgsesItem.image4 = image4;
				nineImgsesItem.image5 = image5;
				nineImgsesItem.image6 = image6;
				nineImgsesItem.image7 = image7;
				nineImgsesItem.image8 = image8;
				nineImgsesItem.image9 = image9;
				int num = 0;
				int.TryParse(s2, out num);
				string a;
				if ((a = text) != null)
				{
					if (!(a == "del"))
					{
						if (!(a == "read"))
						{
							if (!(a == "edit"))
							{
								if (a == "add")
								{
									if (ShareMaterialBrowser.AddNineImgses(nineImgsesItem) > 0)
									{
										s = "success";
									}
									else
									{
										s = "保存失败！";
									}
								}
							}
							else if (num == 0)
							{
								s = "ID参数不正确";
							}
							else
							{
								nineImgsesItem.id = num;
								if (ShareMaterialBrowser.UpdateNineImgses(nineImgsesItem))
								{
									s = "success";
								}
								else
								{
									s = "修改失败！";
								}
							}
						}
						else if (num == 0)
						{
							s = "falid：参数不正确";
						}
						else
						{
							NineImgsesItem nineImgse = ShareMaterialBrowser.GetNineImgse(num);
							if (nineImgse != null)
							{
								s = JsonConvert.SerializeObject(nineImgse, new JsonConverter[]
								{
									new IsoDateTimeConverter
									{
										DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
									}
								});
							}
							else
							{
								s = "falid：素材已删除";
							}
						}
					}
					else if (num == 0)
					{
						s = "falid：参数不正确";
					}
					else if (ShareMaterialBrowser.DeleteNineImgses(num))
					{
						s = "success";
					}
					else
					{
						s = "falid：删除失败";
					}
				}
				base.Response.Write(s);
				base.Response.End();
			}
			string s3 = base.Request.QueryString["ID"];
			this.nid = 0;
			if (int.TryParse(s3, out this.nid))
			{
				this.EditType.Text = "编辑";
				return;
			}
			this.EditType.Text = "新增";
		}
	}
}
