using Hidistro.ControlPanel.Settings;
using Hidistro.Entities.Settings;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class AddShippingTemplate : AdminPage
	{
		private class taskMsg
		{
			public string state
			{
				get;
				set;
			}

			public string msg
			{
				get;
				set;
			}
		}

		private AddShippingTemplate.taskMsg TaskMsg = new AddShippingTemplate.taskMsg();

		protected AddShippingTemplate() : base("m09", "szp06")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string value = base.Request.Form["task"];
			if (!string.IsNullOrEmpty(value))
			{
				this.TaskMsg.state = "faild";
				this.TaskMsg.msg = "未知错误";
				FreightTemplate freightTemplate = this.ReadPostData(new System.Collections.Specialized.NameValueCollection
				{
					base.Request.Form
				});
				if (this.TaskMsg.state == "success")
				{
					if (SettingsHelper.CreateShippingTemplate(freightTemplate))
					{
						this.TaskMsg.msg = "添加成功！";
					}
					else
					{
						this.TaskMsg.state = "faild";
						this.TaskMsg.msg = "添加数据失败:" + SettingsHelper.Error;
					}
				}
				base.Response.Write(JsonConvert.SerializeObject(this.TaskMsg));
				base.Response.End();
			}
		}

		private FreightTemplate ReadPostData(System.Collections.Specialized.NameValueCollection Form)
		{
			string text = "";
			FreightTemplate freightTemplate = new FreightTemplate();
			try
			{
				this.TaskMsg.msg = "";
				freightTemplate.Name = Form["Name"];
				freightTemplate.TemplateId = int.Parse(Form["TemplateId"]);
				if (!string.IsNullOrEmpty(freightTemplate.Name))
				{
					freightTemplate.MUnit = int.Parse(Form["MUnit"]);
					if (int.Parse(Form["FreeShip"]) == 0)
					{
						freightTemplate.FreeShip = false;
					}
					else
					{
						freightTemplate.FreeShip = true;
					}
					if (!freightTemplate.FreeShip)
					{
						if (int.Parse(Form["HasFree"]) == 0)
						{
							freightTemplate.HasFree = false;
						}
						else
						{
							freightTemplate.HasFree = true;
							int num = 0;
							freightTemplate.FreeShippings = new System.Collections.Generic.List<FreeShipping>();
							while (!string.IsNullOrEmpty(Form["freeShippings[" + num + "][ModelId]"]) && this.TaskMsg.msg == "")
							{
								FreeShipping freeShipping = new FreeShipping();
								freeShipping.ModeId = int.Parse(Form["freeShippings[" + num + "][ModelId]"]);
								freeShipping.ConditionType = int.Parse(Form["freeShippings[" + num + "][ConditionType]"]);
								freeShipping.ConditionNumber = Form["freeShippings[" + num + "][ConditionNumber]"];
								string text2 = Form["freeShippings[" + num + "][FreeRegions]"];
								if (!string.IsNullOrEmpty(text2))
								{
									string[] array = text2.Split(new char[]
									{
										','
									});
									if (array.Length > 0)
									{
										freeShipping.FreeShippingRegions = new System.Collections.Generic.List<FreeShippingRegion>();
										string[] array2 = array;
										for (int i = 0; i < array2.Length; i++)
										{
											string s = array2[i];
											int num2 = 0;
											if (int.TryParse(s, out num2) && num2 != 0)
											{
												FreeShippingRegion freeShippingRegion = new FreeShippingRegion();
												freeShippingRegion.RegionId = num2;
												freeShipping.FreeShippingRegions.Add(freeShippingRegion);
											}
										}
									}
								}
								ValidationResults validationResults = Validation.Validate<FreeShipping>(freeShipping, new string[]
								{
									"ValFree"
								});
								text = "";
								if (!validationResults.IsValid)
								{
									foreach (ValidationResult current in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
									{
										text += current.Message;
									}
									this.TaskMsg.msg = text;
								}
								freightTemplate.FreeShippings.Add(freeShipping);
								num++;
							}
						}
						int num3 = 0;
						if (!string.IsNullOrEmpty(Form["shipperSelect[" + num3 + "][ModelId]"]) && this.TaskMsg.msg == "")
						{
							freightTemplate.SpecifyRegionGroups = new System.Collections.Generic.List<SpecifyRegionGroup>();
							while (!string.IsNullOrEmpty(Form["shipperSelect[" + num3 + "][ModelId]"]))
							{
								if (!(this.TaskMsg.msg == ""))
								{
									break;
								}
								SpecifyRegionGroup specifyRegionGroup = new SpecifyRegionGroup();
								specifyRegionGroup.ModeId = int.Parse(Form["shipperSelect[" + num3 + "][ModelId]"]);
								specifyRegionGroup.FristPrice = decimal.Parse(Form["shipperSelect[" + num3 + "][FristPrice]"]);
								specifyRegionGroup.AddNumber = decimal.Parse(Form["shipperSelect[" + num3 + "][AddNumber]"]);
								specifyRegionGroup.FristNumber = decimal.Parse(Form["shipperSelect[" + num3 + "][FristNumber]"]);
								specifyRegionGroup.AddPrice = decimal.Parse(Form["shipperSelect[" + num3 + "][AddPrice]"]);
								specifyRegionGroup.IsDefault = false;
								if (int.Parse(Form["shipperSelect[" + num3 + "][IsDefault]"]) == 1)
								{
									specifyRegionGroup.IsDefault = true;
								}
								string text3 = Form["shipperSelect[" + num3 + "][SpecifyRegions]"];
								if (!string.IsNullOrEmpty(text3))
								{
									string[] array3 = text3.Split(new char[]
									{
										','
									});
									if (array3.Length > 0)
									{
										specifyRegionGroup.SpecifyRegions = new System.Collections.Generic.List<SpecifyRegion>();
										string[] array4 = array3;
										for (int j = 0; j < array4.Length; j++)
										{
											string s2 = array4[j];
											int num4 = 0;
											if (int.TryParse(s2, out num4) && num4 != 0)
											{
												SpecifyRegion specifyRegion = new SpecifyRegion();
												specifyRegion.RegionId = num4;
												specifyRegionGroup.SpecifyRegions.Add(specifyRegion);
											}
										}
									}
								}
								ValidationResults validationResults = Validation.Validate<SpecifyRegionGroup>(specifyRegionGroup, new string[]
								{
									"ValRegionGroup"
								});
								text = "";
								if (!validationResults.IsValid)
								{
									foreach (ValidationResult current2 in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
									{
										text += current2.Message;
									}
									this.TaskMsg.msg = text;
								}
								freightTemplate.SpecifyRegionGroups.Add(specifyRegionGroup);
								num3++;
							}
						}
						else
						{
							this.TaskMsg.msg = "没有运送方式选择";
						}
					}
					else
					{
						freightTemplate.HasFree = false;
					}
				}
				else
				{
					this.TaskMsg.msg = "模板名称不能为空";
				}
				if (this.TaskMsg.msg == "")
				{
					this.TaskMsg.state = "success";
				}
				else
				{
					this.TaskMsg.state = "faild";
				}
			}
			catch (System.Exception ex)
			{
				this.TaskMsg.msg = "参数异常：" + ex.Message.ToString();
			}
			return freightTemplate;
		}
	}
}
