using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using LitJson;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.Admin
{
	public class UploadFileJson : AdminPage
	{
		private string savePath;

		private string saveUrl;

		protected UploadFileJson() : base("m01", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (ManagerHelper.GetCurrentManager() == null)
			{
				this.showError("您没有权限执行此操作！");
				return;
			}
			this.savePath = "~/Storage/master/gallery/";
			this.saveUrl = "/Storage/master/gallery/";
			int num = 0;
			if (base.Request.Form["fileCategory"] != null)
			{
				int.TryParse(base.Request.Form["fileCategory"], out num);
			}
			string text = string.Empty;
			if (base.Request.Form["imgTitle"] != null)
			{
				text = base.Request.Form["imgTitle"];
			}
			System.Web.HttpPostedFile httpPostedFile = base.Request.Files["imgFile"];
			if (httpPostedFile == null)
			{
				this.showError("请先选择文件！");
				return;
			}
			if (!ResourcesHelper.CheckPostedFile(httpPostedFile, "image"))
			{
				this.showError("不能上传空文件，且必须是有效的图片文件！");
				return;
			}
			string text2 = base.Server.MapPath(this.savePath);
			if (!System.IO.Directory.Exists(text2))
			{
				this.showError("上传目录不存在。");
				return;
			}
			text2 += string.Format("{0}/", System.DateTime.Now.ToString("yyyyMM"));
			this.saveUrl += string.Format("{0}/", System.DateTime.Now.ToString("yyyyMM"));
			if (!System.IO.Directory.Exists(text2))
			{
				System.IO.Directory.CreateDirectory(text2);
			}
			string fileName = httpPostedFile.FileName;
			if (text.Length == 0)
			{
				text = fileName;
			}
			string str = System.IO.Path.GetExtension(fileName).ToLower();
			string str2 = System.DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo) + str;
			string filename = text2 + str2;
			string text3 = this.saveUrl + str2;
			try
			{
				httpPostedFile.SaveAs(filename);
				Database database = DatabaseFactory.CreateDatabase();
				System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("insert into Hishop_PhotoGallery(CategoryId,PhotoName,PhotoPath,FileSize,UploadTime,LastUpdateTime)values(@cid,@name,@path,@size,@time,@time1)");
				database.AddInParameter(sqlStringCommand, "cid", System.Data.DbType.Int32, num);
				database.AddInParameter(sqlStringCommand, "name", System.Data.DbType.String, text);
				database.AddInParameter(sqlStringCommand, "path", System.Data.DbType.String, text3);
				database.AddInParameter(sqlStringCommand, "size", System.Data.DbType.Int32, httpPostedFile.ContentLength);
				database.AddInParameter(sqlStringCommand, "time", System.Data.DbType.DateTime, System.DateTime.Now);
				database.AddInParameter(sqlStringCommand, "time1", System.Data.DbType.DateTime, System.DateTime.Now);
				database.ExecuteNonQuery(sqlStringCommand);
			}
			catch
			{
				this.showError("保存文件出错！");
			}
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
			hashtable["error"] = 0;
			hashtable["url"] = Globals.ApplicationPath + text3;
			base.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
			base.Response.Write(JsonMapper.ToJson(hashtable));
			base.Response.End();
		}

		private void showError(string message)
		{
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
			hashtable["error"] = 1;
			hashtable["message"] = message;
			base.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
			base.Response.Write(JsonMapper.ToJson(hashtable));
			base.Response.End();
		}
	}
}
