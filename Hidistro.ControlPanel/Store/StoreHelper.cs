using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SqlDal;
using Hidistro.SqlDal.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web;
using System.Xml;

namespace Hidistro.ControlPanel.Store
{
	public static class StoreHelper
	{
		public static IList<VoteInfo> GetVoteList()
		{
			return new VoteDao().GetVoteList();
		}

		public static int CreateVote(VoteInfo vote)
		{
			int num = 0;
			VoteDao voteDao = new VoteDao();
			long num2 = voteDao.CreateVote(vote);
			if (num2 > 0L)
			{
				num = 1;
				if (vote.VoteItems != null)
				{
					foreach (VoteItemInfo current in vote.VoteItems)
					{
						current.VoteId = num2;
						current.ItemCount = 0;
						num += voteDao.CreateVoteItem(current, null);
					}
				}
			}
			return num;
		}

		public static bool UpdateVote(VoteInfo vote)
		{
			VoteDao voteDao = new VoteDao();
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!voteDao.UpdateVote(vote, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else if (!voteDao.DeleteVoteItem(vote.VoteId, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						int num = 0;
						if (vote.VoteItems != null)
						{
							foreach (VoteItemInfo current in vote.VoteItems)
							{
								current.VoteId = vote.VoteId;
								current.ItemCount = 0;
								num += voteDao.CreateVoteItem(current, dbTransaction);
							}
							if (num < vote.VoteItems.Count)
							{
								dbTransaction.Rollback();
								result = false;
								return result;
							}
						}
						dbTransaction.Commit();
						result = true;
					}
				}
				catch
				{
					dbTransaction.Rollback();
					result = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}

		public static int DeleteVote(long voteId)
		{
			return new VoteDao().DeleteVote(voteId);
		}

		public static VoteInfo GetVoteById(long voteId)
		{
			return new VoteDao().GetVoteById(voteId);
		}

		public static IList<VoteItemInfo> GetVoteItems(long voteId)
		{
			return new VoteDao().GetVoteItems(voteId);
		}

		public static int GetVoteCounts(long voteId)
		{
			return new VoteDao().GetVoteCounts(voteId);
		}

		public static string BackupData()
		{
			return new BackupRestoreDao().BackupData(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/data/Backup/"));
		}

		public static bool InserBackInfo(string fileName, string version, long fileSize)
		{
			string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
			bool result;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
				XmlElement xmlElement = xmlDocument.CreateElement("backupfile");
				xmlElement.SetAttribute("BackupName", fileName);
				xmlElement.SetAttribute("Version", version.ToString());
				xmlElement.SetAttribute("FileSize", fileSize.ToString());
				xmlElement.SetAttribute("BackupTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				xmlNode.AppendChild(xmlElement);
				xmlDocument.Save(filename);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static System.Data.DataTable GetBackupFiles()
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			dataTable.Columns.Add("BackupName", typeof(string));
			dataTable.Columns.Add("Version", typeof(string));
			dataTable.Columns.Add("FileSize", typeof(string));
			dataTable.Columns.Add("BackupTime", typeof(string));
			string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
			foreach (XmlNode xmlNode in childNodes)
			{
				XmlElement xmlElement = (XmlElement)xmlNode;
				System.Data.DataRow dataRow = dataTable.NewRow();
				dataRow["BackupName"] = xmlElement.GetAttribute("BackupName");
				dataRow["Version"] = xmlElement.GetAttribute("Version");
				dataRow["FileSize"] = xmlElement.GetAttribute("FileSize");
				dataRow["BackupTime"] = xmlElement.GetAttribute("BackupTime");
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}

		public static bool DeleteBackupFile(string backupName)
		{
			string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
			bool result;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
				foreach (XmlNode xmlNode in childNodes)
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					if (xmlElement.GetAttribute("BackupName") == backupName)
					{
						xmlDocument.SelectSingleNode("root").RemoveChild(xmlNode);
					}
				}
				xmlDocument.Save(filename);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static bool RestoreData(string bakFullName)
		{
			BackupRestoreDao backupRestoreDao = new BackupRestoreDao();
			bool result = backupRestoreDao.RestoreData(bakFullName);
			backupRestoreDao.Restor();
			return result;
		}

		public static string UploadLinkImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
			{
				result = string.Empty;
			}
			else
			{
				string text = Globals.GetStoragePath() + "/link/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}

		public static string UploadVoteImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
			{
				result = string.Empty;
			}
			else
			{
				string text = Globals.GetStoragePath() + "/vote/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}

		public static string UploadLogo(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
			{
				result = string.Empty;
			}
			else
			{
				string text = Globals.GetStoragePath() + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}

		public static void DeleteImage(string imageUrl)
		{
			if (!string.IsNullOrEmpty(imageUrl))
			{
				try
				{
					string path = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + imageUrl);
					if (File.Exists(path))
					{
						File.Delete(path);
					}
				}
				catch
				{
				}
			}
		}
	}
}
