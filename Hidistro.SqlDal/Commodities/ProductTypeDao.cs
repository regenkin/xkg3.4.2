using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
	public class ProductTypeDao
	{
		private Database database;

		public ProductTypeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public DbQueryResult GetProductTypes(ProductTypeQuery query)
		{
			return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_ProductTypes", "TypeId", string.IsNullOrEmpty(query.TypeName) ? string.Empty : string.Format("TypeName LIKE '%{0}%'", DataHelper.CleanSearchString(query.TypeName)), "*");
		}

		public IList<ProductTypeInfo> GetProductTypes()
		{
			IList<ProductTypeInfo> result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypes");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ProductTypeInfo>(dataReader);
			}
			return result;
		}

		public ProductTypeInfo GetProductType(int typeId)
		{
			ProductTypeInfo productTypeInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypes WHERE TypeId = @TypeId;SELECT * FROM Hishop_ProductTypeBrands WHERE ProductTypeId = @TypeId");
			this.database.AddInParameter(sqlStringCommand, "TypeId", System.Data.DbType.Int32, typeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				productTypeInfo = ReaderConvert.ReaderToModel<ProductTypeInfo>(dataReader);
				dataReader.NextResult();
				while (dataReader.Read())
				{
					productTypeInfo.Brands.Add((int)dataReader["BrandId"]);
				}
			}
			return productTypeInfo;
		}

		public System.Data.DataTable GetBrandCategoriesByTypeId(int typeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT B.BrandId,B.BrandName FROM Hishop_BrandCategories B INNER JOIN Hishop_ProductTypeBrands PB ON B.BrandId=PB.BrandId WHERE PB.ProductTypeId=@ProductTypeId ORDER BY B.DisplaySequence DESC");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", System.Data.DbType.Int32, typeId);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public string GetBrandName(int typeId)
		{
			string text = "";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select BrandName from vw_Hishop_BrandTypeAndBrandCategories where ProductTypeId=@ProductTypeId");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", System.Data.DbType.Int32, typeId);
			System.Data.DataTable dataTable;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			if (dataTable.Rows.Count > 0)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					text = text + dataTable.Rows[i]["BrandName"] + ",";
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}

		public int GetTypeId(string typeName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TypeId FROM Hishop_ProductTypes where TypeName = @TypeName");
			this.database.AddInParameter(sqlStringCommand, "TypeName", System.Data.DbType.String, typeName);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public int AddProductType(ProductTypeInfo productType)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypes(TypeName, Remark) VALUES (@TypeName, @Remark); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "TypeName", System.Data.DbType.String, productType.TypeName);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, productType.Remark);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public void AddProductTypeBrands(int typeId, IList<int> brands)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", System.Data.DbType.Int32, typeId);
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32);
			foreach (int current in brands)
			{
				this.database.SetParameterValue(sqlStringCommand, "BrandId", current);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}

		public bool UpdateProductType(ProductTypeInfo productType)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ProductTypes SET TypeName = @TypeName, Remark = @Remark WHERE TypeId = @TypeId");
			this.database.AddInParameter(sqlStringCommand, "TypeName", System.Data.DbType.String, productType.TypeName);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, productType.Remark);
			this.database.AddInParameter(sqlStringCommand, "TypeId", System.Data.DbType.Int32, productType.TypeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteProductTypeBrands(int typeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypeBrands WHERE ProductTypeId=@ProductTypeId");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", System.Data.DbType.Int32, typeId);
			bool result;
			try
			{
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public bool DeleteProducType(int typeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypes WHERE TypeId = @TypeId AND not exists (SELECT productId FROM Hishop_Products WHERE TypeId = @TypeId)");
			this.database.AddInParameter(sqlStringCommand, "TypeId", System.Data.DbType.Int32, typeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
