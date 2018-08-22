using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace Hidistro.UI.Web.IsvInstall
{
	public class Isv : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Clear();
			context.Response.ClearHeaders();
			context.Response.ClearContent();
			context.Response.Expires = -1;
			string text = Globals.RequestQueryStr("action");
			string a;
			if ((a = text) != null)
			{
				if (a == "setup")
				{
					this.SetUP();
					return;
				}
				if (a == "addadmin")
				{
					this.WriteReturnCode(200, "ok");
					return;
				}
			}
			this.WriteReturnCode(200, "默认ok");
		}

		private void SetUP()
		{
			if (ConfigurationManager.AppSettings["Installer"] == null)
			{
				this.WriteReturnCode(700, "已完成安装");
				return;
			}
			string connectionstr = string.Format("server={0};uid={1};pwd={2};Trusted_Connection=no;database={3}", new object[]
			{
				Globals.RequestQueryStr("dbhost"),
				Globals.RequestQueryStr("dbuser"),
				Globals.RequestQueryStr("dbpassword"),
				Globals.RequestQueryStr("dbname")
			});
			string text = string.Empty;
			text = this.CreateDataSchema(connectionstr);
			if (!string.IsNullOrEmpty(text))
			{
				this.WriteReturnCode(100, text);
			}
			text = this.CreateAdministrator(connectionstr);
			if (!string.IsNullOrEmpty(text))
			{
				this.WriteReturnCode(300, text);
			}
			text = this.AddInitData(connectionstr);
			if (!string.IsNullOrEmpty(text))
			{
				this.WriteReturnCode(400, text);
			}
			text = this.SaveSiteSettings();
			if (!string.IsNullOrEmpty(text))
			{
				this.WriteReturnCode(500, text);
			}
			text = this.SaveConfig(connectionstr);
			if (!string.IsNullOrEmpty(text))
			{
				this.WriteReturnCode(600, text);
				return;
			}
			this.WriteReturnCode(200, "安装成功");
		}

		private void WriteReturnCode(int status, string msg)
		{
			System.Xml.Linq.XDocument xDocument = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "utf-8", "no"), new object[]
			{
				new System.Xml.Linq.XElement("rsp", new object[]
				{
					new System.Xml.Linq.XElement("code", status.ToString()),
					new System.Xml.Linq.XElement("msg", msg)
				})
			});
			System.Web.HttpContext.Current.Response.ContentType = "text/xml";
			System.Web.HttpContext.Current.Response.Write(xDocument.ToString());
			System.Web.HttpContext.Current.Response.End();
		}

		private string CreateDataSchema(string connectionstr)
		{
			string text = System.Web.HttpContext.Current.Request.MapPath("~/Installer/SqlScripts/Schema.sql");
			if (!System.IO.File.Exists(text))
			{
				return "没有找到数据库架构文件-Schema.sql";
			}
			return this.ExecuteScriptFile(text, connectionstr);
		}

		private string AddInitData(string connectionstr)
		{
			string text = System.Web.HttpContext.Current.Request.MapPath("~/Installer/SqlScripts/SiteInitData.zh-CN.Sql");
			if (!System.IO.File.Exists(text))
			{
				return "没有找到初始化数据文件-SiteInitData.Sql";
			}
			return this.ExecuteScriptFile(text, connectionstr);
		}

		private string CreateAdministrator(string connectionstr)
		{
			System.Data.Common.DbConnection dbConnection = new System.Data.SqlClient.SqlConnection(connectionstr);
			dbConnection.Open();
			System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand();
			dbCommand.Connection = dbConnection;
			dbCommand.CommandType = System.Data.CommandType.Text;
			dbCommand.CommandText = "INSERT INTO aspnet_Roles(RoleName,IsDefault) VALUES('超级管理员',1); SELECT @@IDENTITY";
			int num = System.Convert.ToInt32(dbCommand.ExecuteScalar());
			dbCommand.CommandText = "INSERT INTO aspnet_Managers(RoleId, UserName, Password, Email, CreateDate) VALUES (@RoleId, @UserName, @Password, @Email, getdate())";
			dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RoleId", num));
			dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Username", "admin"));
			dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Password", HiCryptographer.Md5Encrypt("123456")));
			dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Email", "test@hishop.com"));
			dbCommand.ExecuteNonQuery();
			dbConnection.Close();
			return "";
		}

		private string ExecuteScriptFile(string pathToScriptFile, string connectionstr)
		{
			System.IO.StreamReader streamReader = null;
			System.Data.SqlClient.SqlConnection sqlConnection = null;
			string result;
			try
			{
				string applicationPath = Globals.ApplicationPath;
				System.IO.StreamReader streamReader2;
				streamReader = (streamReader2 = new System.IO.StreamReader(pathToScriptFile));
				try
				{
					System.Data.SqlClient.SqlConnection sqlConnection2;
					sqlConnection = (sqlConnection2 = new System.Data.SqlClient.SqlConnection(connectionstr));
					try
					{
						System.Data.Common.DbCommand dbCommand = new System.Data.SqlClient.SqlCommand
						{
							Connection = sqlConnection,
							CommandType = System.Data.CommandType.Text,
							CommandTimeout = 60
						};
						sqlConnection.Open();
						while (!streamReader.EndOfStream)
						{
							string text = Isv.NextSqlFromStream(streamReader);
							if (!string.IsNullOrEmpty(text))
							{
								dbCommand.CommandText = text.Replace("$VirsualPath$", applicationPath);
								dbCommand.ExecuteNonQuery();
							}
						}
						sqlConnection.Close();
					}
					finally
					{
						if (sqlConnection2 != null)
						{
							((System.IDisposable)sqlConnection2).Dispose();
						}
					}
					streamReader.Close();
				}
				finally
				{
					if (streamReader2 != null)
					{
						((System.IDisposable)streamReader2).Dispose();
					}
				}
				result = "";
			}
			catch (System.Data.SqlClient.SqlException ex)
			{
				if (sqlConnection != null && sqlConnection.State != System.Data.ConnectionState.Closed)
				{
					sqlConnection.Close();
					sqlConnection.Dispose();
				}
				if (streamReader != null)
				{
					streamReader.Close();
					streamReader.Dispose();
				}
				result = ex.Message;
			}
			return result;
		}

		private static string NextSqlFromStream(System.IO.StreamReader reader)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string text = reader.ReadLine().Trim();
			while (!reader.EndOfStream && string.Compare(text, "GO", true, System.Globalization.CultureInfo.InvariantCulture) != 0)
			{
				stringBuilder.Append(text + System.Environment.NewLine);
				text = reader.ReadLine();
			}
			if (string.Compare(text, "GO", true, System.Globalization.CultureInfo.InvariantCulture) != 0)
			{
				stringBuilder.Append(text + System.Environment.NewLine);
			}
			return stringBuilder.ToString();
		}

		private string SaveSiteSettings()
		{
			string result;
			try
			{
				string filename = System.Web.HttpContext.Current.Request.MapPath("~/config/SiteSettings.config");
				XmlDocument xmlDocument = new XmlDocument();
				SiteSettings siteSettings = new SiteSettings(System.Web.HttpContext.Current.Request.Url.Host);
				xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + System.Environment.NewLine + "<Settings></Settings>");
				siteSettings.VTheme = "t1";
				siteSettings.SiteName = "微信分销大师";
				siteSettings.CheckCode = Isv.CreateKey(20);
				siteSettings.DistributorLogoPic = "/utility/pics/headLogo.jpg";
				siteSettings.DistributorsMenu = true;
				siteSettings.EnableShopMenu = true;
				siteSettings.ShopDefault = true;
				siteSettings.MemberDefault = true;
				siteSettings.GoodsType = true;
				siteSettings.GoodsCheck = true;
				siteSettings.ActivityMenu = true;
				siteSettings.ActivityMenu = true;
				siteSettings.BrandMenu = true;
				siteSettings.GoodsListMenu = true;
				siteSettings.OrderShowDays = 7;
				siteSettings.CloseOrderDays = 3;
				siteSettings.FinishOrderDays = 7;
				siteSettings.MaxReturnedDays = 15;
				siteSettings.TaxRate = 0m;
				siteSettings.PointsRate = 1m;
				siteSettings.IsValidationService = true;
				siteSettings.SMSSender = "";
				siteSettings.SMSSettings = "";
				siteSettings.ShopMenuStyle = "1";
				siteSettings.EnablePodRequest = false;
				siteSettings.EnableCommission = true;
				siteSettings.EnableAlipayRequest = false;
				siteSettings.EnableWeiXinRequest = false;
				siteSettings.EnableOffLineRequest = true;
				siteSettings.EnableWapShengPay = false;
				siteSettings.OffLinePayContent = "<p>请填写在线支付帮助内容</p>";
				siteSettings.DistributorDescription = "<p><img src=\"/utility/pics/fxs.png\" title=\"fenxiao.png\" alt=\"fenxiao.png\"/></p>";
				siteSettings.DistributorBackgroundPic = "/Storage/data/DistributorBackgroundPic/default.jpg|";
				siteSettings.SaleService = "<p>请填写售后服务内容</p>";
				siteSettings.MentionNowMoney = "1";
				siteSettings.PointsRate = 1m;
				siteSettings.PointsRate = 1m;
				siteSettings.Disabled = false;
				siteSettings.ShareAct_Enable = true;
				siteSettings.SignPoint = 10;
				siteSettings.SignWherePoint = 10;
				siteSettings.SignWhere = 10;
				siteSettings.ActiveDay = 1;
				siteSettings.sign_EverDayScore = 50;
				siteSettings.sign_StraightDay = 2;
				siteSettings.sign_RewardScore = 20;
				siteSettings.sign_score_Enable = true;
				siteSettings.shopping_reward_Enable = true;
				siteSettings.shopping_score_Enable = true;
				siteSettings.shopping_Score = 100;
				siteSettings.shopping_reward_Score = 1;
				siteSettings.shopping_reward_OrderValue = 100.0;
				siteSettings.share_score_Enable = true;
				siteSettings.share_Score = 100;
				siteSettings.PointToCashRate = 100;
				siteSettings.PonitToCash_Enable = true;
				siteSettings.PonitToCash_MaxAmount = 1000m;
				siteSettings.DrawPayType = "1|0|2|3";
				siteSettings.BatchAliPay = true;
				siteSettings.BatchWeixinPay = true;
				siteSettings.BatchWeixinPayCheckRealName = 0;
				siteSettings.EnableSaleService = false;
				siteSettings.ServiceMeiQia = "";
				siteSettings.App_Secret = "836e49139e90c64f21251a6dec9c2cca";
				siteSettings.WriteToXml(xmlDocument);
				xmlDocument.Save(filename);
				result = "";
			}
			catch (System.Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}

		private static string CreateKey(int len)
		{
			byte[] array = new byte[len];
			new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(array);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(string.Format("{0:X2}", array[i]));
			}
			return stringBuilder.ToString();
		}

		private string SaveConfig(string connectionstr)
		{
			string result;
			try
			{
				Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
				using (System.Security.Cryptography.RijndaelManaged cryptographer = this.GetCryptographer())
				{
					configuration.AppSettings.Settings["IV"].Value = System.Convert.ToBase64String(cryptographer.IV);
					configuration.AppSettings.Settings["Key"].Value = System.Convert.ToBase64String(cryptographer.Key);
				}
				System.Web.Configuration.MachineKeySection machineKeySection = (System.Web.Configuration.MachineKeySection)configuration.GetSection("system.web/machineKey");
				machineKeySection.ValidationKey = Isv.CreateKey(20);
				machineKeySection.DecryptionKey = Isv.CreateKey(24);
				machineKeySection.Validation = System.Web.Configuration.MachineKeyValidation.SHA1;
				machineKeySection.Decryption = "3DES";
				configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = connectionstr;
				configuration.ConnectionStrings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
				configuration.Save();
				configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
				configuration.AppSettings.Settings.Remove("Installer");
				configuration.Save();
				result = "";
			}
			catch (System.Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}

		private System.Security.Cryptography.RijndaelManaged GetCryptographer()
		{
			System.Security.Cryptography.RijndaelManaged rijndaelManaged = new System.Security.Cryptography.RijndaelManaged();
			rijndaelManaged.KeySize = 128;
			rijndaelManaged.GenerateIV();
			rijndaelManaged.GenerateKey();
			return rijndaelManaged;
		}
	}
}
