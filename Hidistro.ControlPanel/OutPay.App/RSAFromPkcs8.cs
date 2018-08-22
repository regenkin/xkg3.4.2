using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hidistro.ControlPanel.OutPay.App
{
	public sealed class RSAFromPkcs8
	{
		public static string sign(string content, string privateKey, string input_charset)
		{
			Encoding encoding = Encoding.GetEncoding(input_charset);
			byte[] bytes = encoding.GetBytes(content);
			RSACryptoServiceProvider rSACryptoServiceProvider = RSAFromPkcs8.DecodePemPrivateKey(privateKey);
			SHA1 halg = new SHA1CryptoServiceProvider();
			byte[] inArray = rSACryptoServiceProvider.SignData(bytes, halg);
			return Convert.ToBase64String(inArray);
		}

		public static bool verify(string content, string signedString, string publicKey, string input_charset)
		{
			Encoding encoding = Encoding.GetEncoding(input_charset);
			byte[] bytes = encoding.GetBytes(content);
			byte[] signature = Convert.FromBase64String(signedString);
			RSAParameters parameters = RSAFromPkcs8.ConvertFromPublicKey(publicKey);
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.ImportParameters(parameters);
			SHA1 halg = new SHA1CryptoServiceProvider();
			return rSACryptoServiceProvider.VerifyData(bytes, halg, signature);
		}

		public static string decryptData(string resData, string privateKey, string input_charset)
		{
			byte[] array = Convert.FromBase64String(resData);
			List<byte> list = new List<byte>();
			for (int i = 0; i < array.Length / 128; i++)
			{
				byte[] array2 = new byte[128];
				for (int j = 0; j < 128; j++)
				{
					array2[j] = array[j + 128 * i];
				}
				list.AddRange(RSAFromPkcs8.decrypt(array2, privateKey, input_charset));
			}
			byte[] array3 = list.ToArray();
			char[] array4 = new char[Encoding.GetEncoding(input_charset).GetCharCount(array3, 0, array3.Length)];
			Encoding.GetEncoding(input_charset).GetChars(array3, 0, array3.Length, array4, 0);
			return new string(array4);
		}

		private static byte[] decrypt(byte[] data, string privateKey, string input_charset)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = RSAFromPkcs8.DecodePemPrivateKey(privateKey);
			SHA1 sHA = new SHA1CryptoServiceProvider();
			return rSACryptoServiceProvider.Decrypt(data, false);
		}

		private static RSACryptoServiceProvider DecodePemPrivateKey(string pemstr)
		{
			byte[] array = Convert.FromBase64String(pemstr);
			RSACryptoServiceProvider result;
			if (array != null)
			{
				RSACryptoServiceProvider rSACryptoServiceProvider = RSAFromPkcs8.DecodePrivateKeyInfo(array);
				result = rSACryptoServiceProvider;
			}
			else
			{
				result = null;
			}
			return result;
		}

		private static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
		{
			byte[] b = new byte[]
			{
				48,
				13,
				6,
				9,
				42,
				134,
				72,
				134,
				247,
				13,
				1,
				1,
				1,
				5,
				0
			};
			byte[] a = new byte[15];
			MemoryStream memoryStream = new MemoryStream(pkcs8);
			int num = (int)memoryStream.Length;
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			RSACryptoServiceProvider result;
			try
			{
				ushort num2 = binaryReader.ReadUInt16();
				if (num2 == 33072)
				{
					binaryReader.ReadByte();
				}
				else
				{
					if (num2 != 33328)
					{
						result = null;
						return result;
					}
					binaryReader.ReadInt16();
				}
				byte b2 = binaryReader.ReadByte();
				if (b2 != 2)
				{
					result = null;
				}
				else
				{
					num2 = binaryReader.ReadUInt16();
					if (num2 != 1)
					{
						result = null;
					}
					else
					{
						a = binaryReader.ReadBytes(15);
						if (!RSAFromPkcs8.CompareBytearrays(a, b))
						{
							result = null;
						}
						else
						{
							b2 = binaryReader.ReadByte();
							if (b2 != 4)
							{
								result = null;
							}
							else
							{
								b2 = binaryReader.ReadByte();
								if (b2 == 129)
								{
									binaryReader.ReadByte();
								}
								else if (b2 == 130)
								{
									binaryReader.ReadUInt16();
								}
								byte[] privkey = binaryReader.ReadBytes((int)((long)num - memoryStream.Position));
								RSACryptoServiceProvider rSACryptoServiceProvider = RSAFromPkcs8.DecodeRSAPrivateKey(privkey);
								result = rSACryptoServiceProvider;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				result = null;
			}
			finally
			{
				binaryReader.Close();
			}
			return result;
		}

		private static bool CompareBytearrays(byte[] a, byte[] b)
		{
			bool result;
			if (a.Length != b.Length)
			{
				result = false;
			}
			else
			{
				int num = 0;
				for (int i = 0; i < a.Length; i++)
				{
					byte b2 = a[i];
					if (b2 != b[num])
					{
						result = false;
						return result;
					}
					num++;
				}
				result = true;
			}
			return result;
		}

		private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
		{
			MemoryStream input = new MemoryStream(privkey);
			BinaryReader binaryReader = new BinaryReader(input);
			RSACryptoServiceProvider result;
			try
			{
				ushort num = binaryReader.ReadUInt16();
				if (num == 33072)
				{
					binaryReader.ReadByte();
				}
				else
				{
					if (num != 33328)
					{
						result = null;
						return result;
					}
					binaryReader.ReadInt16();
				}
				num = binaryReader.ReadUInt16();
				if (num != 258)
				{
					result = null;
				}
				else
				{
					byte b = binaryReader.ReadByte();
					if (b != 0)
					{
						result = null;
					}
					else
					{
						int integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] modulus = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] exponent = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] d = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] p = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] q = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] dP = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] dQ = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] inverseQ = binaryReader.ReadBytes(integerSize);
						RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
						rSACryptoServiceProvider.ImportParameters(new RSAParameters
						{
							Modulus = modulus,
							Exponent = exponent,
							D = d,
							P = p,
							Q = q,
							DP = dP,
							DQ = dQ,
							InverseQ = inverseQ
						});
						result = rSACryptoServiceProvider;
					}
				}
			}
			catch (Exception)
			{
				result = null;
			}
			finally
			{
				binaryReader.Close();
			}
			return result;
		}

		private static int GetIntegerSize(BinaryReader binr)
		{
			byte b = binr.ReadByte();
			int result;
			if (b != 2)
			{
				result = 0;
			}
			else
			{
				b = binr.ReadByte();
				int num;
				if (b == 129)
				{
					num = (int)binr.ReadByte();
				}
				else if (b == 130)
				{
					byte b2 = binr.ReadByte();
					byte b3 = binr.ReadByte();
					byte[] array = new byte[4];
					array[0] = b3;
					array[1] = b2;
					byte[] value = array;
					num = BitConverter.ToInt32(value, 0);
				}
				else
				{
					num = (int)b;
				}
				while (binr.ReadByte() == 0)
				{
					num--;
				}
				binr.BaseStream.Seek(-1L, SeekOrigin.Current);
				result = num;
			}
			return result;
		}

		private static RSAParameters ConvertFromPublicKey(string pemFileConent)
		{
			byte[] array = Convert.FromBase64String(pemFileConent);
			if (array.Length < 162)
			{
				throw new ArgumentException("pem file content is incorrect.");
			}
			byte[] array2 = new byte[128];
			byte[] array3 = new byte[3];
			Array.Copy(array, 29, array2, 0, 128);
			Array.Copy(array, 159, array3, 0, 3);
			return new RSAParameters
			{
				Modulus = array2,
				Exponent = array3
			};
		}

		private static RSAParameters ConvertFromPrivateKey(string pemFileConent)
		{
			byte[] array = Convert.FromBase64String(pemFileConent);
			if (array.Length < 609)
			{
				throw new ArgumentException("pem file content is incorrect.");
			}
			int num = 11;
			byte[] array2 = new byte[128];
			Array.Copy(array, num, array2, 0, 128);
			num += 128;
			num += 2;
			byte[] array3 = new byte[3];
			Array.Copy(array, num, array3, 0, 3);
			num += 3;
			num += 4;
			byte[] array4 = new byte[128];
			Array.Copy(array, num, array4, 0, 128);
			num += 128;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array5 = new byte[64];
			Array.Copy(array, num, array5, 0, 64);
			num += 64;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array6 = new byte[64];
			Array.Copy(array, num, array6, 0, 64);
			num += 64;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array7 = new byte[64];
			Array.Copy(array, num, array7, 0, 64);
			num += 64;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array8 = new byte[64];
			Array.Copy(array, num, array8, 0, 64);
			num += 64;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array9 = new byte[64];
			Array.Copy(array, num, array9, 0, 64);
			return new RSAParameters
			{
				Modulus = array2,
				Exponent = array3,
				D = array4,
				P = array5,
				Q = array6,
				DP = array7,
				DQ = array8,
				InverseQ = array9
			};
		}
	}
}
