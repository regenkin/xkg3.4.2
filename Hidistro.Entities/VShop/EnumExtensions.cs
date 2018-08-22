using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.Entities.VShop
{
	public static class EnumExtensions
	{
		private static System.Collections.Generic.Dictionary<string, string> _cacheEnumShowTextdDic = new System.Collections.Generic.Dictionary<string, string>();

		public static string ToShowText(this System.Enum en)
		{
			return en.ToShowText(false, ",");
		}

		public static string ToShowText(this System.Enum en, bool exceptionIfFail, string flagsSeparator)
		{
			string enumFullName = EnumExtensions.GetEnumFullName(en);
			string showText;
			string result;
			if (!EnumExtensions._cacheEnumShowTextdDic.TryGetValue(enumFullName, out showText))
			{
				System.Type type = en.GetType();
				object[] customAttributes = type.GetCustomAttributes(typeof(System.FlagsAttribute), false);
				if (customAttributes != null && customAttributes.Length > 0)
				{
					long num = System.Convert.ToInt64(en);
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					string[] names = System.Enum.GetNames(type);
					string text = "";
					string[] array = names;
					for (int i = 0; i < array.Length; i++)
					{
						string text2 = array[i];
						long num2 = System.Convert.ToInt64(System.Enum.Parse(type, text2));
						if (num2 == 0L)
						{
							object[] customAttributes2 = type.GetField(text2).GetCustomAttributes(typeof(EnumShowTextAttribute), false);
							if (customAttributes2.Length > 0)
							{
								text = ((EnumShowTextAttribute)customAttributes2[0]).ShowText;
							}
						}
						else if ((num2 & num) == num2)
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.Append(flagsSeparator);
							}
							object[] customAttributes2 = type.GetField(text2).GetCustomAttributes(typeof(EnumShowTextAttribute), false);
							if (customAttributes2.Length > 0)
							{
								stringBuilder.Append(((EnumShowTextAttribute)customAttributes2[0]).ShowText);
							}
							else
							{
								if (exceptionIfFail)
								{
									throw new System.InvalidOperationException(string.Format("此枚举{0}未定义EnumShowTextAttribute", enumFullName));
								}
								stringBuilder.Append(text2);
							}
						}
					}
					if (stringBuilder.Length > 0)
					{
						result = stringBuilder.ToString();
						return result;
					}
					result = text;
					return result;
				}
				else
				{
					System.Reflection.FieldInfo field = type.GetField(en.ToString());
					if (field == null)
					{
						throw new System.InvalidOperationException(string.Format("非完整枚举{0}", enumFullName));
					}
					object[] customAttributes2 = field.GetCustomAttributes(typeof(EnumShowTextAttribute), false);
					if (customAttributes2.Length > 0)
					{
						showText = ((EnumShowTextAttribute)customAttributes2[0]).ShowText;
						lock (EnumExtensions._cacheEnumShowTextdDic)
						{
							EnumExtensions._cacheEnumShowTextdDic[enumFullName] = showText;
						}
					}
					else
					{
						if (exceptionIfFail)
						{
							throw new System.InvalidOperationException(string.Format("此枚举{0}未定义EnumShowTextAttribute", enumFullName));
						}
						result = en.ToString();
						return result;
					}
				}
			}
			result = showText;
			return result;
		}

		private static string GetEnumFullName(System.Enum en)
		{
			return en.GetType().FullName + "." + en.ToString();
		}

		public static void BindEnum<T>(this ListControl listControl, string Unbindkey = "") where T : struct
		{
			System.Type typeFromHandle = typeof(T);
			if (!typeFromHandle.IsEnum)
			{
				throw new System.InvalidOperationException("类型必须是枚举:" + typeFromHandle.FullName);
			}
			System.Array values = System.Enum.GetValues(typeFromHandle);
			if (!listControl.AppendDataBoundItems)
			{
				listControl.Items.Clear();
			}
			foreach (System.Enum @enum in values)
			{
				if (@enum.ToString() != Unbindkey)
				{
					listControl.Items.Add(new ListItem(@enum.ToShowText(), @enum.ToString()));
				}
			}
		}
	}
}
