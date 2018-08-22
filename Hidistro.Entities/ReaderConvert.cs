using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Hidistro.Entities
{
	public class ReaderConvert
	{
		public static T ReaderToModel<T>(IDataReader objReader) where T : new()
		{
			T result;
			if (objReader != null && objReader.Read())
			{
				System.Type typeFromHandle = typeof(T);
				int fieldCount = objReader.FieldCount;
				T t = (default(T) == null) ? System.Activator.CreateInstance<T>() : default(T);
				for (int i = 0; i < fieldCount; i++)
				{
					if (!ReaderConvert.IsNullOrDBNull(objReader[i]))
					{
						System.Reflection.PropertyInfo property = typeFromHandle.GetProperty(objReader.GetName(i).Replace("_", ""), System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);
						if (property != null)
						{
							System.Type type = property.PropertyType;
							if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(System.Nullable<>)))
							{
								NullableConverter nullableConverter = new NullableConverter(type);
								type = nullableConverter.UnderlyingType;
							}
							if (type.IsEnum)
							{
								object value = System.Enum.ToObject(type, objReader[i]);
								property.SetValue(t, value, null);
							}
							else
							{
								property.SetValue(t, ReaderConvert.CheckType(objReader[i], type), null);
							}
						}
					}
				}
				result = t;
			}
			else
			{
				result = default(T);
			}
			return result;
		}

		public static T DataRowToModel<T>(DataRow objReader) where T : new()
		{
			T result;
			if (objReader != null)
			{
				System.Type typeFromHandle = typeof(T);
				int count = objReader.Table.Columns.Count;
				T t = (default(T) == null) ? System.Activator.CreateInstance<T>() : default(T);
				for (int i = 0; i < count; i++)
				{
					if (!ReaderConvert.IsNullOrDBNull(objReader[i]))
					{
						try
						{
							System.Reflection.PropertyInfo property = typeFromHandle.GetProperty(objReader.Table.Columns[i].ColumnName.Replace("_", ""), System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);
							if (property != null)
							{
								System.Type type = property.PropertyType;
								if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(System.Nullable<>)))
								{
									NullableConverter nullableConverter = new NullableConverter(type);
									type = nullableConverter.UnderlyingType;
								}
								if (type.IsEnum)
								{
									object value = System.Enum.ToObject(type, objReader[i]);
									property.SetValue(t, value, null);
								}
								else
								{
									property.SetValue(t, ReaderConvert.CheckType(objReader[i], type), null);
								}
							}
						}
						catch
						{
						}
					}
				}
				result = t;
			}
			else
			{
				result = default(T);
			}
			return result;
		}

		public static System.Collections.Generic.IList<T> ReaderToList<T>(IDataReader objReader) where T : new()
		{
			System.Collections.Generic.IList<T> result;
			if (objReader != null)
			{
				System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();
				System.Type typeFromHandle = typeof(T);
				while (objReader.Read())
				{
					T t = (default(T) == null) ? System.Activator.CreateInstance<T>() : default(T);
					for (int i = 0; i < objReader.FieldCount; i++)
					{
						if (!ReaderConvert.IsNullOrDBNull(objReader[i]))
						{
							System.Reflection.PropertyInfo property = typeFromHandle.GetProperty(objReader.GetName(i).Replace("_", ""), System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);
							if (property != null)
							{
								System.Type type = property.PropertyType;
								if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(System.Nullable<>)))
								{
									NullableConverter nullableConverter = new NullableConverter(type);
									type = nullableConverter.UnderlyingType;
								}
								if (property.PropertyType.IsEnum)
								{
									object value = System.Enum.ToObject(type, objReader[i]);
									property.SetValue(t, value, null);
								}
								else
								{
									property.SetValue(t, ReaderConvert.CheckType(objReader[i], type), null);
								}
							}
						}
					}
					list.Add(t);
				}
				result = list;
			}
			else
			{
				result = null;
			}
			return result;
		}

		private static object CheckType(object value, System.Type conversionType)
		{
			object result;
			if (value == null)
			{
				result = null;
			}
			else
			{
				result = System.Convert.ChangeType(value, conversionType);
			}
			return result;
		}

		private static bool IsNullOrDBNull(object obj)
		{
			return obj == null || obj is System.DBNull;
		}
	}
}
