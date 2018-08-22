using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hidistro.Core
{
	public static class DesSecurity
	{
		public static string DesEncrypt(string pToEncrypt, string sKey)
		{
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			byte[] bytes = Encoding.Default.GetBytes(pToEncrypt);
			dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(sKey);
			dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(sKey);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.FlushFinalBlock();
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = memoryStream.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				byte b = array[i];
				stringBuilder.AppendFormat("{0:X2}", b);
			}
			stringBuilder.ToString();
			return stringBuilder.ToString();
		}

		public static string DesDecrypt(string pToDecrypt, string sKey)
		{
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			byte[] array = new byte[pToDecrypt.Length / 2];
			for (int i = 0; i < pToDecrypt.Length / 2; i++)
			{
				int num = Convert.ToInt32(pToDecrypt.Substring(i * 2, 2), 16);
				array[i] = (byte)num;
			}
			dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(sKey);
			dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(sKey);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.FlushFinalBlock();
			StringBuilder stringBuilder = new StringBuilder();
			return Encoding.Default.GetString(memoryStream.ToArray());
		}
	}
}
