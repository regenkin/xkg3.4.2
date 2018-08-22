using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
	public class ProductBatchDao
	{
		private Database database;

		public ProductBatchDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public System.Data.DataTable GetProductBaseInfo(string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT ProductId, ProductName, ProductCode, MarketPrice, ThumbnailUrl40, SaleCounts, ShowSaleCounts FROM Hishop_Products WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds)));
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public ProductInfo GetProductBaseInfo(int productId)
		{
			ProductInfo result = null;
			IList<ProductInfo> productBaseInfo = this.GetProductBaseInfo(new int[]
			{
				productId
			});
			if (productBaseInfo.Count > 0)
			{
				result = productBaseInfo[0];
			}
			return result;
		}

		public IList<ProductInfo> GetProductBaseInfo(IEnumerable<int> productIds)
		{
			string productBaseInfoSelectSQL = this.getProductBaseInfoSelectSQL(productIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(productBaseInfoSelectSQL);
			IList<ProductInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ProductInfo>(dataReader);
			}
			return result;
		}

		private string getProductBaseInfoSelectSQL(IEnumerable<int> productIds)
		{
			return string.Format("SELECT ProductId, ProductName, ProductCode, MarketPrice, ThumbnailUrl40, SaleCounts, ShowSaleCounts,SaleStatus FROM Hishop_Products WHERE ProductId IN ({0})", string.Join<int>(",", productIds));
		}

		public bool UpdateProductNames(string productIds, string prefix, string suffix)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ProductName = '{0}'+ProductName+'{1}' WHERE ProductId IN ({2})", DataHelper.CleanSearchString(prefix), DataHelper.CleanSearchString(suffix), productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ReplaceProductNames(string productIds, string oldWord, string newWord)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ProductName = REPLACE(ProductName, '{0}', '{1}') WHERE ProductId IN ({2})", DataHelper.CleanSearchString(oldWord), DataHelper.CleanSearchString(newWord), productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateProductBaseInfo(System.Data.DataTable dt)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
			foreach (System.Data.DataRow dataRow in dt.Rows)
			{
				num++;
				string text = num.ToString();
				stringBuilder.AppendFormat(" UPDATE Hishop_Products SET ProductName = @ProductName{0}, ProductCode = @ProductCode{0}, MarketPrice = @MarketPrice{0}", text);
				stringBuilder.AppendFormat(" WHERE ProductId = {0}", dataRow["ProductId"]);
				this.database.AddInParameter(sqlStringCommand, "ProductName" + text, System.Data.DbType.String, dataRow["ProductName"]);
				this.database.AddInParameter(sqlStringCommand, "ProductCode" + text, System.Data.DbType.String, dataRow["ProductCode"]);
				this.database.AddInParameter(sqlStringCommand, "MarketPrice" + text, System.Data.DbType.String, dataRow["MarketPrice"]);
			}
			sqlStringCommand.CommandText = stringBuilder.ToString();
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ShowSaleCounts = {0} WHERE ProductId IN ({1})", showSaleCounts, productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateVisitCounts(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("\r\n                    if exists(select top 1 * from vshop_Statistics_ProductsVisits \r\n\t                    where ProductID= {0} and CONVERT(varchar(10), RecDate, 120) = CONVERT(varchar(10), GETDATE(), 120) \r\n\t                    )\r\n                    begin\t\r\n\t                    UPDATE vshop_Statistics_ProductsVisits SET TotalVisits = isnull(TotalVisits,0) + 1 WHERE ProductId = {0} and CONVERT(varchar(10), RecDate, 120) = CONVERT(varchar(10), GETDATE(), 120)\r\n                    end\r\n                    else\r\n                    begin\r\n\t                    insert into vshop_Statistics_ProductsVisits(RecDate,ProductID,TotalVisits)\r\n\t\t                    values ( CONVERT(varchar(10), GETDATE(), 120), {0},  1 )\r\n                    end            \r\n                ", productId));
			try
			{
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
			catch
			{
			}
			System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET VistiCounts = VistiCounts + 1 WHERE ProductId = {0}", productId));
			return this.database.ExecuteNonQuery(sqlStringCommand2) > 0;
		}

		public bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ShowSaleCounts = SaleCounts {0} {1} WHERE ProductId IN ({2})", operation, showSaleCounts, productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateShowSaleCounts(System.Data.DataTable dt)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (System.Data.DataRow dataRow in dt.Rows)
			{
				stringBuilder.AppendFormat(" UPDATE Hishop_Products SET ShowSaleCounts = {0} WHERE ProductId = {1}", dataRow["ShowSaleCounts"], dataRow["ProductId"]);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public System.Data.DataTable GetSkuStocks(string productIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT p.ProductId,ProductName, SkuId, SKU, Stock, ThumbnailUrl40 FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
			stringBuilder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
			stringBuilder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable dataTable = null;
			System.Data.DataTable dataTable2 = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			dataTable.Columns.Add("SKUContent");
			if (dataTable != null && dataTable.Rows.Count > 0 && dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					string text = string.Empty;
					foreach (System.Data.DataRow dataRow2 in dataTable2.Rows)
					{
						if ((string)dataRow["SkuId"] == (string)dataRow2["SkuId"])
						{
							object obj = text;
							text = string.Concat(new object[]
							{
								obj,
								dataRow2["AttributeName"],
								"：",
								dataRow2["ValueStr"],
								"; "
							});
						}
					}
					dataRow["SKUContent"] = text;
				}
			}
			return dataTable;
		}

		public bool UpdateSkuStock(string productIds, int stock)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_SKUs SET Stock = {0} WHERE ProductId IN ({1})", stock, DataHelper.CleanSearchString(productIds)));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool AddSkuStock(string productIds, int addStock)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_SKUs SET Stock = CASE WHEN Stock + ({0}) < 0 THEN 0 ELSE Stock + ({0}) END WHERE ProductId IN ({1})", addStock, DataHelper.CleanSearchString(productIds)));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateSkuStock(Dictionary<string, int> skuStocks)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in skuStocks.Keys)
			{
				stringBuilder.AppendFormat(" UPDATE Hishop_SKUs SET Stock = {0} WHERE SkuId = '{1}'", skuStocks[current], DataHelper.CleanSearchString(current));
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public System.Data.DataTable GetSkuMemberPrices(string productIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT SkuId, ProductName, SKU, CostPrice, MarketPrice, SalePrice FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
			stringBuilder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
			stringBuilder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
			stringBuilder.AppendLine(" SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] AS MemberGradeName,Discount FROM aspnet_MemberGrades");
			stringBuilder.AppendLine(" SELECT SkuId, (SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] FROM aspnet_MemberGrades WHERE GradeId = sm.GradeId) AS MemberGradeName,MemberSalePrice");
			stringBuilder.AppendFormat(" FROM Hishop_SKUMemberPrice sm WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable dataTable = null;
			System.Data.DataTable dataTable2 = null;
			System.Data.DataTable dataTable3 = null;
			System.Data.DataTable dataTable4 = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					dataTable.Columns.Add("SKUContent");
					dataReader.NextResult();
					dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
					dataReader.NextResult();
					dataTable4 = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (dataTable4 != null && dataTable4.Rows.Count > 0)
					{
						foreach (System.Data.DataRow dataRow in dataTable4.Rows)
						{
							dataTable.Columns.Add((string)dataRow["MemberGradeName"]);
						}
					}
					dataReader.NextResult();
					dataTable3 = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
			}
			if (dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow2 in dataTable.Rows)
				{
					string text = string.Empty;
					foreach (System.Data.DataRow dataRow3 in dataTable2.Rows)
					{
						if ((string)dataRow2["SkuId"] == (string)dataRow3["SkuId"])
						{
							object obj = text;
							text = string.Concat(new object[]
							{
								obj,
								dataRow3["AttributeName"],
								"：",
								dataRow3["ValueStr"],
								"; "
							});
						}
					}
					dataRow2["SKUContent"] = text;
				}
			}
			if (dataTable3 != null && dataTable3.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow2 in dataTable.Rows)
				{
					foreach (System.Data.DataRow dataRow4 in dataTable3.Rows)
					{
						if ((string)dataRow2["SkuId"] == (string)dataRow4["SkuId"])
						{
							dataRow2[(string)dataRow4["MemberGradeName"]] = dataRow4["MemberSalePrice"];
						}
					}
				}
			}
			if (dataTable4 != null && dataTable4.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow2 in dataTable.Rows)
				{
					decimal d = decimal.Parse(dataRow2["SalePrice"].ToString());
					foreach (System.Data.DataRow dataRow5 in dataTable4.Rows)
					{
						decimal d2 = decimal.Parse(dataRow5["Discount"].ToString());
						string arg = (d * (d2 / 100m)).ToString("F2");
						dataRow2[(string)dataRow5["MemberGradeName"]] = dataRow2[(string)dataRow5["MemberGradeName"]] + "|" + arg;
					}
				}
			}
			return dataTable;
		}

		public bool CheckPrice(string productIds, int baseGradeId, decimal checkPrice, bool isMember)
		{
			StringBuilder stringBuilder = new StringBuilder(" ");
			if (baseGradeId == -2)
			{
				stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs WHERE ProductId IN ({0}) AND CostPrice - {1} < 0", productIds, checkPrice);
			}
			else if (baseGradeId == -3)
			{
				stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs WHERE ProductId IN ({0}) AND SalePrice - {1} < 0", productIds, checkPrice);
			}
			else if (isMember)
			{
				stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE MemberSalePrice - {0} < 0 AND GradeId = {1}", checkPrice, baseGradeId);
				stringBuilder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0})) ", productIds);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (gradeId == -2)
			{
				stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
			}
			else if (gradeId == -3)
			{
				stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
			}
			else
			{
				stringBuilder.AppendFormat("DELETE FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
				stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, {1} AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({2})", gradeId, price, DataHelper.CleanSearchString(productIds));
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateSkuMemberPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
		{
			StringBuilder stringBuilder = new StringBuilder(" ");
			if (gradeId == -2)
			{
				if (baseGradeId == -2)
				{
					stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
				}
				else if (baseGradeId == -3)
				{
					stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = SalePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
				}
			}
			else if (gradeId == -3)
			{
				if (baseGradeId == -2)
				{
					stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
				}
				else if (baseGradeId == -3)
				{
					stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = SalePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
				}
			}
			else
			{
				stringBuilder.AppendFormat("DELETE FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
				if (baseGradeId == -2)
				{
					stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, CostPrice {1} ({2}) AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({3})", new object[]
					{
						gradeId,
						operation,
						price,
						DataHelper.CleanSearchString(productIds)
					});
				}
				else if (baseGradeId == -3)
				{
					stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, SalePrice {1} ({2}) AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({3})", new object[]
					{
						gradeId,
						operation,
						price,
						DataHelper.CleanSearchString(productIds)
					});
				}
				else
				{
					stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, MemberSalePrice {1} ({2}) AS MemberSalePrice", gradeId, operation, price);
					stringBuilder.AppendFormat(" FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", baseGradeId, DataHelper.CleanSearchString(productIds));
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool GetSKUMemberPrice(string productIds, int gradeId)
		{
			string query = string.Format("SELECT * FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable dataTable = new System.Data.DataTable();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dataTable.Rows.Count > 0;
		}

		public bool UpdateSkuMemberPrices(System.Data.DataSet ds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			System.Data.DataTable dataTable = ds.Tables["skuPriceTable"];
			System.Data.DataTable dataTable2 = ds.Tables["skuMemberPriceTable"];
			string text = string.Empty;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						"'",
						dataRow["skuId"],
						"',"
					});
					stringBuilder.AppendFormat(" UPDATE Hishop_SKUs SET CostPrice = {0}, SalePrice = {1} WHERE SkuId = '{2}'", dataRow["costPrice"], dataRow["salePrice"], dataRow["skuId"]);
				}
			}
			if (text.Length > 1)
			{
				stringBuilder.AppendFormat(" DELETE FROM Hishop_SKUMemberPrice WHERE SkuId IN ({0}) ", text.Remove(text.Length - 1));
			}
			if (dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable2.Rows)
				{
					stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId, GradeId, MemberSalePrice) VALUES ('{0}', {1}, {2})", dataRow["skuId"], dataRow["gradeId"], dataRow["memberPrice"]);
				}
			}
			bool result;
			if (stringBuilder.Length <= 0)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
	}
}
