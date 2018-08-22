using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using LitJson;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text.RegularExpressions;

namespace Hidistro.UI.Web.Admin
{
	public class FileManagerJson : AdminPage
	{
		public class NameSorter : System.Collections.IComparer
		{
			private bool ascend;

			public NameSorter(bool isAscend)
			{
				this.ascend = isAscend;
			}

			public int Compare(object x, object y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					if (!this.ascend)
					{
						return 1;
					}
					return -1;
				}
				else if (y == null)
				{
					if (!this.ascend)
					{
						return -1;
					}
					return 1;
				}
				else
				{
					System.IO.FileInfo fileInfo = new System.IO.FileInfo(x.ToString());
					System.IO.FileInfo fileInfo2 = new System.IO.FileInfo(y.ToString());
					if (!this.ascend)
					{
						return fileInfo2.FullName.CompareTo(fileInfo.FullName);
					}
					return fileInfo.FullName.CompareTo(fileInfo2.FullName);
				}
			}
		}

		public class SizeSorter : System.Collections.IComparer
		{
			private bool ascend;

			public SizeSorter(bool isAscend)
			{
				this.ascend = isAscend;
			}

			public int Compare(object x, object y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					if (!this.ascend)
					{
						return 1;
					}
					return -1;
				}
				else if (y == null)
				{
					if (!this.ascend)
					{
						return -1;
					}
					return 1;
				}
				else
				{
					System.IO.FileInfo fileInfo = new System.IO.FileInfo(x.ToString());
					System.IO.FileInfo fileInfo2 = new System.IO.FileInfo(y.ToString());
					if (!this.ascend)
					{
						return fileInfo2.Length.CompareTo(fileInfo.Length);
					}
					return fileInfo.Length.CompareTo(fileInfo2.Length);
				}
			}
		}

		public class DateTimeSorter : System.Collections.IComparer
		{
			private bool ascend;

			private int type;

			public DateTimeSorter(int sortType, bool isAscend)
			{
				this.ascend = isAscend;
				this.type = sortType;
			}

			public int Compare(object x, object y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					if (!this.ascend)
					{
						return 1;
					}
					return -1;
				}
				else if (y == null)
				{
					if (!this.ascend)
					{
						return -1;
					}
					return 1;
				}
				else
				{
					System.IO.FileInfo fileInfo = new System.IO.FileInfo(x.ToString());
					System.IO.FileInfo fileInfo2 = new System.IO.FileInfo(y.ToString());
					if (this.type == 0)
					{
						if (!this.ascend)
						{
							return fileInfo2.CreationTime.CompareTo(fileInfo.CreationTime);
						}
						return fileInfo.CreationTime.CompareTo(fileInfo2.CreationTime);
					}
					else
					{
						if (!this.ascend)
						{
							return fileInfo2.LastWriteTime.CompareTo(fileInfo.LastWriteTime);
						}
						return fileInfo.LastWriteTime.CompareTo(fileInfo2.LastWriteTime);
					}
				}
			}
		}

		private string dir = "image";

		protected FileManagerJson() : base("m01", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
			if (Globals.GetCurrentManagerUserId() == 0)
			{
				base.Response.Write("没有权限！");
				base.Response.End();
				return;
			}
			this.dir = base.Request["dir"];
			if (string.IsNullOrEmpty(this.dir))
			{
				this.dir = "image";
			}
			string a = "false";
			if (base.Request.QueryString["isAdvPositions"] != null)
			{
				a = base.Request.QueryString["isAdvPositions"].ToString().ToLower().Trim();
			}
			string dirPath = "~/Storage/master/gallery/";
			string url = "/Storage/master/gallery/";
			string a2;
			if ((a2 = this.dir) != null)
			{
				if (!(a2 == "image"))
				{
					if (!(a2 == "file"))
					{
						if (!(a2 == "flash"))
						{
							if (a2 == "media")
							{
								dirPath = "~/Storage/master/media/";
								url = "/Storage/master/media/";
							}
						}
						else
						{
							dirPath = "~/Storage/master/flash/";
							url = "/Storage/master/flash/";
						}
					}
					else
					{
						dirPath = "~/Storage/master/accessory/";
						url = "/Storage/master/accessory/";
					}
				}
				else
				{
					dirPath = "~/Storage/master/gallery/";
					url = "/Storage/master/gallery/";
				}
			}
			string text = base.Request.QueryString["order"];
			text = (string.IsNullOrEmpty(text) ? "uploadtime" : text.ToLower());
			string text2 = base.Request.QueryString["path"];
			if (text2 == null || text2 == "")
			{
				text2 = "-1";
			}
			if (a == "false")
			{
				this.FillTableForDb(text2, text, hashtable);
			}
			else
			{
				this.FillTableForPath(dirPath, url, text, hashtable);
			}
			string text3 = base.Request.Url.ToString();
			text3 = text3.Substring(0, text3.IndexOf("/", 7));
			text3 += base.Request.ApplicationPath;
			if (text3.EndsWith("/"))
			{
				text3 = text3.Substring(0, text3.Length - 1);
			}
			hashtable["domain"] = text3;
			base.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
			base.Response.Write(JsonMapper.ToJson(hashtable));
			base.Response.End();
		}

		public int GetCategoryFile(int iCategryId, out decimal fileSize)
		{
			int result = 0;
			fileSize = 0m;
			Database database = DatabaseFactory.CreateDatabase();
			string query = string.Format("select Count(PhotoId) as FileCount,isnull(Sum(FileSize),0) as AllFileSize from Hishop_PhotoGallery", new object[0]);
			if (iCategryId > 0)
			{
				query = string.Format("select Count(PhotoId) as FileCount,isnull(Sum(FileSize),0) as AllFileSize from Hishop_PhotoGallery where CategoryId={0} ", iCategryId.ToString());
			}
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
			using (System.Data.IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = (int)dataReader["FileCount"];
					fileSize = (int)dataReader["AllFileSize"];
				}
			}
			return result;
		}

		public void FillTableForDb(string cid, string order, System.Collections.Hashtable table)
		{
			int num = 0;
			int.TryParse(cid, out num);
			Database database = DatabaseFactory.CreateDatabase();
			table["moveup_dir_path"] = "";
			table["current_dir_path"] = "";
			table["current_url"] = "";
			new System.Collections.Generic.List<System.Collections.Hashtable>();
			System.Collections.Generic.List<System.Collections.Hashtable> list = new System.Collections.Generic.List<System.Collections.Hashtable>();
			table["file_list"] = list;
			if (num > 0)
			{
				System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
				hashtable["is_dir"] = true;
				hashtable["has_file"] = true;
				hashtable["filesize"] = 0;
				hashtable["is_photo"] = false;
				hashtable["filetype"] = "";
				hashtable["filename"] = "上级目录";
				hashtable["path"] = "-1";
				hashtable["datetime"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				list.Add(hashtable);
			}
			if (num <= 0)
			{
				string query = "select CategoryId,CategoryName from Hishop_PhotoCategories order by DisplaySequence";
				System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
				using (System.Data.IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
				{
					while (dataReader.Read())
					{
						decimal num2 = 0m;
						int categoryFile = this.GetCategoryFile((int)dataReader["CategoryId"], out num2);
						System.Collections.Hashtable hashtable2 = new System.Collections.Hashtable();
						hashtable2["is_dir"] = true;
						hashtable2["has_file"] = (categoryFile > 0);
						hashtable2["filesize"] = num2;
						hashtable2["is_photo"] = false;
						hashtable2["filetype"] = "";
						hashtable2["filename"] = dataReader["CategoryName"];
						hashtable2["cid"] = dataReader["CategoryId"];
						hashtable2["path"] = dataReader["CategoryName"];
						hashtable2["datetime"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						list.Add(hashtable2);
					}
				}
			}
			string query2 = string.Format("select * from Hishop_PhotoGallery where CategoryId=0 order by {0}", order);
			if (num > 0)
			{
				query2 = string.Format("select * from Hishop_PhotoGallery where CategoryId={0} order by {1}", num, order);
			}
			System.Data.Common.DbCommand sqlStringCommand2 = database.GetSqlStringCommand(query2);
			using (System.Data.IDataReader dataReader2 = database.ExecuteReader(sqlStringCommand2))
			{
				while (dataReader2.Read())
				{
					System.Collections.Hashtable hashtable3 = new System.Collections.Hashtable();
					hashtable3["cid"] = dataReader2["CategoryId"];
					hashtable3["name"] = dataReader2["PhotoName"];
					hashtable3["path"] = dataReader2["PhotoPath"];
					hashtable3["filename"] = dataReader2["PhotoName"];
					hashtable3["filesize"] = dataReader2["FileSize"];
					hashtable3["addedtime"] = dataReader2["UploadTime"];
					hashtable3["updatetime"] = dataReader2["LastUpdateTime"];
					hashtable3["datetime"] = ((dataReader2["LastUpdateTime"] == System.DBNull.Value) ? "" : ((System.DateTime)dataReader2["LastUpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss"));
					string text = dataReader2["PhotoPath"].ToString().Trim();
					hashtable3["filetype"] = text.Substring(text.LastIndexOf('.'));
					list.Add(hashtable3);
				}
			}
			table["total_count"] = list.Count;
			table["current_cateogry"] = num;
		}

		public void FillTableForPath(string dirPath, string url, string order, System.Collections.Hashtable table)
		{
			string text = base.Request.QueryString["path"];
			text = (string.IsNullOrEmpty(text) ? "" : text);
			if (System.Text.RegularExpressions.Regex.IsMatch(text, "\\.\\."))
			{
				base.Response.Write("Access is not allowed.");
				base.Response.End();
			}
			if (text != "" && !text.EndsWith("/"))
			{
				text += "/";
			}
			string text2;
			string value;
			string text3;
			string value2;
			if (text == "")
			{
				text2 = dirPath;
				value = url;
				text3 = "";
				value2 = "";
			}
			else
			{
				text2 = dirPath + text;
				value = url + text;
				text3 = text;
				value2 = System.Text.RegularExpressions.Regex.Replace(text3, "(.*?)[^\\/]+\\/$", "$1");
			}
			text2 = base.Server.MapPath(text2);
			if (!System.IO.Directory.Exists(text2))
			{
				base.Response.Write("此目录不存在！" + text2);
				base.Response.End();
			}
			string[] directories = System.IO.Directory.GetDirectories(text2);
			string[] files = System.IO.Directory.GetFiles(text2);
			switch (order)
			{
			case "uploadtime":
				System.Array.Sort(files, new FileManagerJson.DateTimeSorter(0, true));
				goto IL_268;
			case "uploadtime desc":
				System.Array.Sort(files, new FileManagerJson.DateTimeSorter(0, false));
				goto IL_268;
			case "lastupdatetime":
				System.Array.Sort(files, new FileManagerJson.DateTimeSorter(1, true));
				goto IL_268;
			case "lastupdatetime desc":
				System.Array.Sort(files, new FileManagerJson.DateTimeSorter(1, false));
				goto IL_268;
			case "photoname":
				System.Array.Sort(files, new FileManagerJson.NameSorter(true));
				goto IL_268;
			case "photoname desc":
				System.Array.Sort(files, new FileManagerJson.NameSorter(false));
				goto IL_268;
			case "filesize":
				System.Array.Sort(files, new FileManagerJson.SizeSorter(true));
				goto IL_268;
			case "filesize desc":
				System.Array.Sort(files, new FileManagerJson.SizeSorter(false));
				goto IL_268;
			}
			System.Array.Sort(files, new FileManagerJson.NameSorter(true));
			IL_268:
			table["moveup_dir_path"] = value2;
			table["current_dir_path"] = text3;
			table["current_url"] = value;
			table["total_count"] = directories.Length + files.Length;
			System.Collections.Generic.List<System.Collections.Hashtable> list = new System.Collections.Generic.List<System.Collections.Hashtable>();
			table["file_list"] = list;
			if (text != "")
			{
				System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
				hashtable["is_dir"] = true;
				hashtable["has_file"] = true;
				hashtable["filesize"] = 0;
				hashtable["is_photo"] = false;
				hashtable["filetype"] = "";
				hashtable["filename"] = "";
				hashtable["path"] = "";
				hashtable["datetime"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				hashtable["filename"] = "上级目录";
				list.Add(hashtable);
			}
			for (int i = 0; i < directories.Length; i++)
			{
				System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(directories[i]);
				System.Collections.Hashtable hashtable2 = new System.Collections.Hashtable();
				hashtable2["is_dir"] = true;
				hashtable2["has_file"] = (directoryInfo.GetFileSystemInfos().Length > 0);
				hashtable2["filesize"] = 0;
				hashtable2["is_photo"] = false;
				hashtable2["filetype"] = "";
				hashtable2["filename"] = directoryInfo.Name;
				hashtable2["path"] = directoryInfo.Name;
				hashtable2["datetime"] = directoryInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
				list.Add(hashtable2);
			}
			for (int j = 0; j < files.Length; j++)
			{
				System.IO.FileInfo fileInfo = new System.IO.FileInfo(files[j]);
				System.Collections.Hashtable hashtable3 = new System.Collections.Hashtable();
				hashtable3["cid"] = "-1";
				hashtable3["name"] = fileInfo.Name;
				hashtable3["path"] = url + text + fileInfo.Name;
				hashtable3["filename"] = fileInfo.Name;
				hashtable3["filesize"] = fileInfo.Length;
				hashtable3["addedtime"] = fileInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
				hashtable3["updatetime"] = fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
				hashtable3["datetime"] = fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
				hashtable3["filetype"] = fileInfo.Extension.Substring(1);
				list.Add(hashtable3);
			}
		}
	}
}
