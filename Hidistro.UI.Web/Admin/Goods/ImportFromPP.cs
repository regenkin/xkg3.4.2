using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.TransferManager;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.ProductBatchUpload)]
	public class ImportFromPP : AdminPage
	{
		private string _dataPath;

		protected System.Web.UI.WebControls.DropDownList dropImportVersions;

		protected System.Web.UI.WebControls.FileUpload fileUploader;

		protected System.Web.UI.WebControls.Button btnUpload;

		protected System.Web.UI.WebControls.DropDownList dropFiles;

		protected ProductCategoriesDropDownList dropCategories;

		protected BrandCategoriesDropDownList dropBrandList;

		protected System.Web.UI.WebControls.RadioButton radOnSales;

		protected System.Web.UI.WebControls.RadioButton radUnSales;

		protected System.Web.UI.WebControls.RadioButton radInStock;

		protected System.Web.UI.WebControls.Button btnImport;

		protected ImportFromPP() : base("m02", "spp04")
		{
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this._dataPath = this.Page.Request.MapPath("~/App_Data/data/paipai");
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropBrandList.DataBind();
				this.BindImporters();
				this.BindFiles();
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
		}

		private void BindImporters()
		{
			this.dropImportVersions.Items.Clear();
			this.dropImportVersions.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
			System.Collections.Generic.Dictionary<string, string> importAdapters = TransferHelper.GetImportAdapters(new YfxTarget("1.2"), "拍拍助理");
			foreach (string current in importAdapters.Keys)
			{
				this.dropImportVersions.Items.Add(new System.Web.UI.WebControls.ListItem(importAdapters[current].Replace("4.0", "2013"), current));
			}
		}

		private void BindFiles()
		{
			this.dropFiles.Items.Clear();
			this.dropFiles.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
			System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(this._dataPath);
			System.IO.FileInfo[] files = directoryInfo.GetFiles("*.zip", System.IO.SearchOption.TopDirectoryOnly);
			System.IO.FileInfo[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				System.IO.FileInfo fileInfo = array[i];
				string name = fileInfo.Name;
				this.dropFiles.Items.Add(new System.Web.UI.WebControls.ListItem(name, name));
			}
		}

		private void btnUpload_Click(object sender, System.EventArgs e)
		{
			if (this.dropImportVersions.SelectedValue.Length == 0)
			{
				this.ShowMsg("请先选择一个导入插件", false);
				return;
			}
			if (!this.fileUploader.HasFile)
			{
				this.ShowMsg("请先选择一个数据包文件", false);
				return;
			}
			if (this.fileUploader.PostedFile.ContentLength == 0 || (this.fileUploader.PostedFile.ContentType != "application/x-zip-compressed" && this.fileUploader.PostedFile.ContentType != "application/zip" && this.fileUploader.PostedFile.ContentType != "application/octet-stream"))
			{
				this.ShowMsg("请上传正确的数据包文件", false);
				return;
			}
			string fileName = System.IO.Path.GetFileName(this.fileUploader.PostedFile.FileName);
			this.fileUploader.PostedFile.SaveAs(System.IO.Path.Combine(this._dataPath, fileName));
			this.BindFiles();
			this.dropFiles.SelectedValue = fileName;
		}

		private void btnImport_Click(object sender, System.EventArgs e)
		{
			if (!this.CheckItems())
			{
				return;
			}
			string text = this.dropFiles.SelectedValue;
			string text2 = System.IO.Path.Combine(this._dataPath, System.IO.Path.GetFileNameWithoutExtension(text));
			ImportAdapter importer = TransferHelper.GetImporter(this.dropImportVersions.SelectedValue, new object[0]);
			int value = this.dropCategories.SelectedValue.Value;
			int? selectedValue = this.dropBrandList.SelectedValue;
			ProductSaleStatus saleStatus = ProductSaleStatus.Delete;
			if (this.radInStock.Checked)
			{
				saleStatus = ProductSaleStatus.OnStock;
			}
			if (this.radUnSales.Checked)
			{
				saleStatus = ProductSaleStatus.UnSale;
			}
			if (this.radOnSales.Checked)
			{
				saleStatus = ProductSaleStatus.OnSale;
			}
			text = System.IO.Path.Combine(this._dataPath, text);
			if (!System.IO.File.Exists(text))
			{
				this.ShowMsg("选择的数据包文件有问题！", false);
				return;
			}
			this.PrepareDataFiles(text2, text);
			try
			{
				object[] array = importer.ParseProductData(new object[]
				{
					text2
				});
				ProductHelper.ImportProducts((System.Data.DataTable)array[0], value, 0, selectedValue, saleStatus, false);
				System.IO.File.Delete(text);
				System.IO.Directory.Delete(text2, true);
				this.BindFiles();
				this.ShowMsg("此次商品批量导入操作已成功！", true);
			}
			catch (System.Exception)
			{
				this.ShowMsg("选择的数据包文件有问题！", false);
			}
		}

		private void PrepareDataFiles(string dir, string filename)
		{
			using (ZipFile zipFile = ZipFile.Read(System.IO.Path.Combine(new string[]
			{
				filename
			})))
			{
				foreach (ZipEntry current in zipFile)
				{
					current.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
				}
			}
		}

		private bool CheckItems()
		{
			string text = "";
			if (this.dropImportVersions.SelectedValue.Length == 0)
			{
				text += Formatter.FormatErrorMessage("请选择一个导入插件！");
			}
			if (this.dropFiles.SelectedValue.Length == 0)
			{
				text += Formatter.FormatErrorMessage("请选择要导入的数据包文件！");
			}
			if (!this.dropCategories.SelectedValue.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择要导入的商品分类！");
			}
			if (!string.IsNullOrEmpty(text) || text.Length > 0)
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
	}
}
