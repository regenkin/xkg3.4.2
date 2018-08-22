using Ajax;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class ShopIndex : AdminPage
	{
		public class ManageThemeInfo
		{
			public string Name
			{
				get;
				set;
			}

			public string ThemeName
			{
				get;
				set;
			}

			public string ThemeImgUrl
			{
				get;
				set;
			}
		}

		public const string tempFileDic = "/admin/shop/ShopEdit.aspx";

		public const string tempImgDic = "/Templates/vshop/";

		public string tempLatePath = "";

		public string templateCuName = "";

		public string showUrl = "";

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Repeater Repeater1;

		public ShopIndex() : base("m01", "dpp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Utility.RegisterTypeForAjax(typeof(ShopIndex));
			if (!base.IsPostBack)
			{
				int port = base.Request.Url.Port;
				string text = (port == 80) ? "" : (":" + port.ToString());
				this.showUrl = string.Concat(new string[]
				{
					"http://",
					base.Request.Url.Host,
					text,
					Globals.ApplicationPath,
					"/default.aspx"
				});
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				this.tempLatePath = masterSettings.VTheme;
				this.DataBind();
			}
		}

		public override void DataBind()
		{
			this.Repeater1.DataSource = this.LoadThemes();
			this.Repeater1.DataBind();
		}

		protected System.Collections.Generic.IList<ShopIndex.ManageThemeInfo> LoadThemes()
		{
			XmlDocument xmlDocument = new XmlDocument();
			System.Collections.Generic.IList<ShopIndex.ManageThemeInfo> list = new System.Collections.Generic.List<ShopIndex.ManageThemeInfo>();
			string[] array = System.IO.Directory.Exists(base.Server.MapPath("/Templates/vshop/")) ? System.IO.Directory.GetDirectories(base.Server.MapPath("/Templates/vshop/")) : null;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string path = array2[i];
				System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(path);
				string text = directoryInfo.Name.ToLower(System.Globalization.CultureInfo.InvariantCulture);
				if (text.Length > 0 && !text.StartsWith("_"))
				{
					System.IO.FileInfo[] files = directoryInfo.GetFiles("template.xml");
					System.IO.FileInfo[] array3 = files;
					for (int j = 0; j < array3.Length; j++)
					{
						System.IO.FileInfo fileInfo = array3[j];
						ShopIndex.ManageThemeInfo manageThemeInfo = new ShopIndex.ManageThemeInfo();
						System.IO.FileStream fileStream = fileInfo.OpenRead();
						xmlDocument.Load(fileStream);
						fileStream.Close();
						manageThemeInfo.Name = xmlDocument.SelectSingleNode("root/Name").InnerText;
						manageThemeInfo.ThemeName = text;
						if (text == this.tempLatePath)
						{
							this.templateCuName = xmlDocument.SelectSingleNode("root/Name").InnerText;
						}
						list.Add(manageThemeInfo);
					}
				}
			}
			return list;
		}

		public string GetTempUrl(string tempLateLogicName)
		{
			if (!string.IsNullOrEmpty(tempLateLogicName))
			{
				return "/admin/shop/ShopEdit.aspx?tempName=" + tempLateLogicName;
			}
			return "/admin/shop/ShopEdit.aspx?tempName=ti";
		}

		public string GetTempLateLogicName(string fileName)
		{
			if (!string.IsNullOrEmpty(fileName))
			{
				return fileName.Substring(0, base.Eval("Name").ToString().LastIndexOf("."));
			}
			return "ti";
		}

		public string GetImgName(string fileName)
		{
			return "/Templates/vshop/" + fileName + "/default.png";
		}

		[AjaxMethod]
		public bool EnableTemp(string TempName)
		{
			if (string.IsNullOrEmpty(TempName))
			{
				return false;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			masterSettings.VTheme = TempName;
			SettingsManager.Save(masterSettings);
			HiCache.Remove("TemplateFileCache");
			return true;
		}
	}
}
