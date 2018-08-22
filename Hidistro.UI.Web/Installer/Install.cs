using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Installer
{
	public class Install : System.Web.UI.Page
	{
		private string action;

		private string dbServer;

		private string dbName;

		private string dbUsername;

		private string dbPassword;

		private string username;

		private string email;

		private string password;

		private string password2;

		private bool isAddDemo;

		private bool testSuccessed;

		private System.Collections.Generic.IList<string> errorMsgs;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.Label lblErrMessage;

		protected System.Web.UI.WebControls.TextBox txtDbServer;

		protected System.Web.UI.WebControls.TextBox txtDbName;

		protected System.Web.UI.WebControls.TextBox txtDbUsername;

		protected System.Web.UI.WebControls.TextBox txtDbPassword;

		protected System.Web.UI.WebControls.HiddenField hdfSiteName;

		protected System.Web.UI.WebControls.TextBox txtUsername;

		protected System.Web.UI.WebControls.TextBox txtEmail;

		protected System.Web.UI.WebControls.TextBox txtPassword;

		protected System.Web.UI.WebControls.TextBox txtPassword2;

		protected System.Web.UI.WebControls.CheckBox chkIsAddDemo;

		protected System.Web.UI.WebControls.Label litSetpErrorMessage;

		protected System.Web.UI.WebControls.Button btnInstall;

		private void LoadParameters()
		{
			bool flag = !string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true";
			if (flag)
			{
				this.action = base.Request["action"];
				this.dbServer = base.Request["DBServer"];
				this.dbName = base.Request["DBName"];
				this.dbUsername = base.Request["DBUsername"];
				this.dbPassword = base.Request["DBPassword"];
				this.username = base.Request["Username"];
				this.email = base.Request["Email"];
				this.password = base.Request["Password"];
				this.password2 = base.Request["Password2"];
				this.isAddDemo = (!string.IsNullOrEmpty(base.Request["IsAddDemo"]) && base.Request["IsAddDemo"] == "true");
				this.testSuccessed = (!string.IsNullOrEmpty(base.Request["TestSuccessed"]) && base.Request["TestSuccessed"] == "true");
				return;
			}
			this.dbServer = this.txtDbServer.Text;
			this.dbName = this.txtDbName.Text;
			this.dbUsername = this.txtDbUsername.Text;
			this.dbPassword = this.txtDbPassword.Text;
			this.username = this.txtUsername.Text;
			this.email = this.txtEmail.Text;
			this.password = this.txtPassword.Text;
			this.password2 = this.txtPassword2.Text;
			this.isAddDemo = this.chkIsAddDemo.Checked;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
			bool flag = !string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true";
			if (flag)
			{
				string str = "无效的操作类型：" + this.action;
				bool flag2 = false;
				if (this.action == "Test")
				{
					flag2 = this.ExecuteTest();
				}
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				if (flag2)
				{
					base.Response.Write("{\"Status\":\"OK\"}");
				}
				else
				{
					string text = "";
					if (this.errorMsgs != null && this.errorMsgs.Count > 0)
					{
						foreach (string current in this.errorMsgs)
						{
							text = text + "{\"Text\":\"" + current + "\"},";
						}
						text = text.Substring(0, text.Length - 1);
						this.errorMsgs.Clear();
					}
					else
					{
						text = "{\"Text\":\"" + str + "\"}";
					}
					base.Response.Write(string.Format("{{\"Status\":\"Fail\",\"ErrorMsgs\":[{0}]}}", text));
				}
				base.Response.End();
				return;
			}
			if (!this.Page.IsPostBack && !(base.Request.UrlReferrer == null))
			{
				base.Request.UrlReferrer.OriginalString.IndexOf("Activation.aspx");
			}
		}

		private void btnInstall_Click(object sender, System.EventArgs e)
		{
			string empty = string.Empty;
			if (!this.ValidateUser(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			if (!this.testSuccessed && !this.ExecuteTest())
			{
				this.ShowMsg("数据库链接信息有误", false);
				return;
			}
			if (!this.CreateDataSchema(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			if (!this.CreateAdministrator(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			if (!this.AddInitData(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			if (this.isAddDemo && !this.AddDemoData(out empty))
			{
				return;
			}
			if (!this.SaveSiteSettings(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			if (!this.SaveConfig(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			this.Context.Response.Redirect("Succeed.aspx", true);
		}

		private void ShowMsg(string errorMsg, bool seccess)
		{
			this.lblErrMessage.Text = errorMsg;
		}

		private bool CreateDataSchema(out string errorMsg)
		{
			string text = base.Request.MapPath("SqlScripts/Schema.sql");
			if (!System.IO.File.Exists(text))
			{
				errorMsg = "没有找到数据库架构文件-Schema.sql";
				return false;
			}
			return this.ExecuteScriptFile(text, out errorMsg);
		}

		private bool CreateAdministrator(out string errorMsg)
		{
			System.Data.Common.DbConnection dbConnection = new System.Data.SqlClient.SqlConnection(this.GetConnectionString());
			dbConnection.Open();
			System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand();
			dbCommand.Connection = dbConnection;
			dbCommand.CommandType = System.Data.CommandType.Text;
			dbCommand.CommandText = "INSERT INTO aspnet_Roles(RoleName,IsDefault) VALUES('超级管理员',1); SELECT @@IDENTITY";
			int num = System.Convert.ToInt32(dbCommand.ExecuteScalar());
			dbCommand.CommandText = "INSERT INTO aspnet_Managers(RoleId, UserName, Password, Email, CreateDate) VALUES (@RoleId, @UserName, @Password, @Email, getdate())";
			dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RoleId", num));
			dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Username", this.username));
			dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Password", HiCryptographer.Md5Encrypt(this.password)));
			dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Email", this.email));
			dbCommand.ExecuteNonQuery();
			dbConnection.Close();
			errorMsg = null;
			return true;
		}

		private bool AddInitData(out string errorMsg)
		{
			string text = base.Request.MapPath("SqlScripts/SiteInitData.zh-CN.Sql");
			if (!System.IO.File.Exists(text))
			{
				errorMsg = "没有找到初始化数据文件-SiteInitData.Sql";
				return false;
			}
			return this.ExecuteScriptFile(text, out errorMsg);
		}

		private bool AddDemoData(out string errorMsg)
		{
			string text = base.Request.MapPath("SqlScripts/SiteDemo.zh-CN.sql");
			if (!System.IO.File.Exists(text))
			{
				errorMsg = "没有找到演示数据文件-SiteDemo.Sql";
				return false;
			}
			return this.ExecuteScriptFile(text, out errorMsg);
		}

		private bool SaveSiteSettings(out string errorMsg)
		{
			errorMsg = null;
			bool result;
			try
			{
				string filename = base.Request.MapPath(Globals.ApplicationPath + "/config/SiteSettings.config");
				XmlDocument xmlDocument = new XmlDocument();
				SiteSettings siteSettings = new SiteSettings(base.Request.Url.Host);
				xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + System.Environment.NewLine + "<Settings></Settings>");
				siteSettings.VTheme = "t1";
				siteSettings.SiteName = this.hdfSiteName.Value.Trim();
				siteSettings.CheckCode = Install.CreateKey(20);
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
				result = true;
			}
			catch (System.Exception ex)
			{
				errorMsg = ex.Message;
				result = false;
			}
			return result;
		}

		private bool SaveConfig(out string errorMsg)
		{
			bool result;
			try
			{
				Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
				using (System.Security.Cryptography.RijndaelManaged cryptographer = this.GetCryptographer())
				{
					configuration.AppSettings.Settings["IV"].Value = System.Convert.ToBase64String(cryptographer.IV);
					configuration.AppSettings.Settings["Key"].Value = System.Convert.ToBase64String(cryptographer.Key);
				}
				System.Web.Configuration.MachineKeySection machineKeySection = (System.Web.Configuration.MachineKeySection)configuration.GetSection("system.web/machineKey");
				machineKeySection.ValidationKey = Install.CreateKey(20);
				machineKeySection.DecryptionKey = Install.CreateKey(24);
				machineKeySection.Validation = System.Web.Configuration.MachineKeyValidation.SHA1;
				machineKeySection.Decryption = "3DES";
				configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = this.GetConnectionString();
				configuration.ConnectionStrings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
				configuration.Save();
				errorMsg = null;
				result = true;
			}
			catch (System.Exception ex)
			{
				errorMsg = ex.Message;
				result = false;
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

		private bool ExecuteTest()
		{
			this.errorMsgs = new System.Collections.Generic.List<string>();
			System.Data.Common.DbTransaction dbTransaction = null;
			System.Data.Common.DbConnection dbConnection = null;
			string item;
			try
			{
				if (this.ValidateConnectionStrings(out item))
				{
					System.Data.Common.DbConnection dbConnection2;
					dbConnection = (dbConnection2 = new System.Data.SqlClient.SqlConnection(this.GetConnectionString()));
					try
					{
						dbConnection.Open();
						System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand();
						dbTransaction = dbConnection.BeginTransaction();
						dbCommand.Connection = dbConnection;
						dbCommand.Transaction = dbTransaction;
						dbCommand.CommandText = "CREATE TABLE installTest(Test bit NULL)";
						dbCommand.ExecuteNonQuery();
						dbCommand.CommandText = "DROP TABLE installTest";
						dbCommand.ExecuteNonQuery();
						dbTransaction.Commit();
						dbConnection.Close();
						goto IL_94;
					}
					finally
					{
						if (dbConnection2 != null)
						{
							((System.IDisposable)dbConnection2).Dispose();
						}
					}
				}
				this.errorMsgs.Add(item);
				IL_94:;
			}
			catch (System.Exception ex)
			{
				this.errorMsgs.Add(ex.Message);
				if (dbTransaction != null)
				{
					try
					{
						dbTransaction.Rollback();
					}
					catch (System.Exception ex2)
					{
						this.errorMsgs.Add(ex2.Message);
					}
				}
				if (dbConnection != null && dbConnection.State != System.Data.ConnectionState.Closed)
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}
			}
			string folderPath = base.Request.MapPath(Globals.ApplicationPath + "/config/test.txt");
			if (!Install.TestFolder(folderPath, out item))
			{
				this.errorMsgs.Add(item);
			}
			try
			{
				Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
				if (configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString == "none")
				{
					configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = "required";
				}
				else
				{
					configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = "none";
				}
				configuration.Save();
			}
			catch (System.Exception ex3)
			{
				this.errorMsgs.Add(ex3.Message);
			}
			folderPath = base.Request.MapPath(Globals.ApplicationPath + "/storage/test.txt");
			if (!Install.TestFolder(folderPath, out item))
			{
				this.errorMsgs.Add(item);
			}
			return this.errorMsgs.Count == 0;
		}

		private bool ExecuteScriptFile(string pathToScriptFile, out string errorMsg)
		{
			System.IO.StreamReader streamReader = null;
			System.Data.SqlClient.SqlConnection sqlConnection = null;
			bool result;
			try
			{
				string applicationPath = Globals.ApplicationPath;
				System.IO.StreamReader streamReader2;
				streamReader = (streamReader2 = new System.IO.StreamReader(pathToScriptFile));
				try
				{
					System.Data.SqlClient.SqlConnection sqlConnection2;
					sqlConnection = (sqlConnection2 = new System.Data.SqlClient.SqlConnection(this.GetConnectionString()));
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
							string text = Install.NextSqlFromStream(streamReader);
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
				errorMsg = null;
				result = true;
			}
			catch (System.Data.SqlClient.SqlException ex)
			{
				errorMsg = ex.Message;
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
				result = false;
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

		private bool ValidateConnectionStrings(out string msg)
		{
			msg = null;
			if (string.IsNullOrEmpty(this.dbServer) || string.IsNullOrEmpty(this.dbName) || string.IsNullOrEmpty(this.dbUsername))
			{
				msg = "数据库连接信息不完整";
				return false;
			}
			return true;
		}

		private bool ValidateUser(out string msg)
		{
			msg = null;
			if (string.IsNullOrEmpty(this.username) || string.IsNullOrEmpty(this.email) || string.IsNullOrEmpty(this.password) || string.IsNullOrEmpty(this.password2))
			{
				msg = "管理员账号信息不完整";
				return false;
			}
			HiConfiguration config = HiConfiguration.GetConfig();
			if (this.username.Length > config.UsernameMaxLength || this.username.Length < config.UsernameMinLength)
			{
				msg = string.Format("管理员用户名的长度只能在{0}和{1}个字符之间", config.UsernameMinLength, config.UsernameMaxLength);
				return false;
			}
			if (string.Compare(this.username, "anonymous", true) == 0)
			{
				msg = "不能使用anonymous作为管理员用户名";
				return false;
			}
			if (!System.Text.RegularExpressions.Regex.IsMatch(this.username, config.UsernameRegex))
			{
				msg = "管理员用户名的格式不符合要求，用户名一般由字母、数字、下划线和汉字组成，且必须以汉字或字母开头";
				return false;
			}
			if (this.email.Length > 256)
			{
				msg = "电子邮件的长度必须小于256个字符";
				return false;
			}
			if (!System.Text.RegularExpressions.Regex.IsMatch(this.email, config.EmailRegex))
			{
				msg = "电子邮件的格式错误";
				return false;
			}
			if (this.password != this.password2)
			{
				msg = "管理员登录密码两次输入不一致";
				return false;
			}
			if (this.password.Length < System.Web.Security.Membership.Provider.MinRequiredPasswordLength || this.password.Length > config.PasswordMaxLength)
			{
				msg = string.Format("管理员登录密码的长度只能在{0}和{1}个字符之间", System.Web.Security.Membership.Provider.MinRequiredPasswordLength, config.PasswordMaxLength);
				return false;
			}
			return true;
		}

		private string GetConnectionString()
		{
			return string.Format("server={0};uid={1};pwd={2};Trusted_Connection=no;database={3}", new object[]
			{
				this.dbServer,
				this.dbUsername,
				this.dbPassword,
				this.dbName
			});
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

		private static bool TestFolder(string folderPath, out string errorMsg)
		{
			bool result;
			try
			{
				System.IO.File.WriteAllText(folderPath, "Hi");
				System.IO.File.AppendAllText(folderPath, ",This is a test file.");
				System.IO.File.Delete(folderPath);
				errorMsg = null;
				result = true;
			}
			catch (System.Exception ex)
			{
				errorMsg = ex.Message;
				result = false;
			}
			return result;
		}
	}
}
