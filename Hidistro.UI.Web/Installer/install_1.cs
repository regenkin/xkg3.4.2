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
    public class Install_1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string errorMsg;
            SaveConfig(out errorMsg);
            Response.Write(errorMsg);
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
                machineKeySection.ValidationKey = CreateKey(20);
                machineKeySection.DecryptionKey = CreateKey(24);
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

        private string GetConnectionString()
        {
            return string.Format("server={0};uid={1};pwd={2};Trusted_Connection=no;database={3}", new object[]
            {
                "120.76.242.38",
                "pufang",
                "pufang520#",
                "销客多"
            });
        }

        private System.Security.Cryptography.RijndaelManaged GetCryptographer()
        {
            System.Security.Cryptography.RijndaelManaged rijndaelManaged = new System.Security.Cryptography.RijndaelManaged();
            rijndaelManaged.KeySize = 128;
            rijndaelManaged.GenerateIV();
            rijndaelManaged.GenerateKey();
            return rijndaelManaged;
        }

        private string CreateKey(int len)
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
    }
}