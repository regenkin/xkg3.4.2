using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
	public class TaobaoProductDao
	{
		private Database database;

		public TaobaoProductDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public System.Data.DataSet GetTaobaoProductDetails(int productId)
		{
			System.Data.DataSet dataSet = new System.Data.DataSet();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId, HasSKU, ProductName, ProductCode, MarketPrice, (SELECT [Name] FROM Hishop_Categories WHERE CategoryId = p.CategoryId) AS CategoryName, (SELECT [Name] FROM Hishop_ProductLines WHERE LineId = p.LineId) AS ProductLineName, (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId = p.BrandId) AS BrandName, (SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice, (SELECT MIN(CostPrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS CostPrice, (SELECT SUM(Stock) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Stock FROM Hishop_Products p WHERE ProductId = @ProductId SELECT AttributeName, ValueStr FROM Hishop_ProductAttributes pa join Hishop_Attributes a ON pa.AttributeId = a.AttributeId JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC SELECT Weight AS '重量', Stock AS '库存', CostPrice AS '成本价', SalePrice AS '一口价', SkuId AS '商家编码' FROM Hishop_SKUs s WHERE ProductId = @ProductId; SELECT SkuId AS '商家编码',AttributeName,UseAttributeImage,ValueStr,ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC SELECT * FROM Taobao_Products WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			System.Data.DataTable table;
			System.Data.DataTable table2;
			System.Data.DataTable dataTable;
			System.Data.DataTable dataTable2;
			System.Data.DataTable table3;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				table = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				table2 = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				table3 = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				if (dataTable2 != null && dataTable2.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable2.Rows)
					{
						System.Data.DataColumn dataColumn = new System.Data.DataColumn();
						dataColumn.ColumnName = (string)dataRow["AttributeName"];
						if (!dataTable.Columns.Contains(dataColumn.ColumnName))
						{
							dataTable.Columns.Add(dataColumn);
						}
					}
					foreach (System.Data.DataRow dataRow2 in dataTable.Rows)
					{
						foreach (System.Data.DataRow dataRow in dataTable2.Rows)
						{
							if (string.Compare((string)dataRow2["商家编码"], (string)dataRow["商家编码"]) == 0)
							{
								dataRow2[(string)dataRow["AttributeName"]] = dataRow["ValueStr"];
							}
						}
					}
				}
			}
			dataSet.Tables.Add(table);
			dataSet.Tables.Add(table2);
			dataSet.Tables.Add(dataTable);
			dataSet.Tables.Add(table3);
			return dataSet;
		}

		public bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Taobao_Products WHERE ProductId = @ProductId;INSERT INTO Taobao_Products(Cid, StuffStatus, ProductId, ProTitle,Num, LocationState, LocationCity, FreightPayer, PostFee, ExpressFee, EMSFee, HasInvoice, HasWarranty, HasDiscount, ValidThru, ListTime, PropertyAlias,InputPids,InputStr, SkuProperties, SkuQuantities, SkuPrices, SkuOuterIds) VALUES(@Cid, @StuffStatus, @ProductId, @ProTitle,@Num, @LocationState, @LocationCity, @FreightPayer, @PostFee, @ExpressFee, @EMSFee, @HasInvoice, @HasWarranty, @HasDiscount, @ValidThru, @ListTime,@PropertyAlias,@InputPids, @InputStr, @SkuProperties, @SkuQuantities, @SkuPrices, @SkuOuterIds);");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, taobaoProduct.ProductId);
			this.database.AddInParameter(sqlStringCommand, "Cid", System.Data.DbType.Int64, taobaoProduct.Cid);
			this.database.AddInParameter(sqlStringCommand, "StuffStatus", System.Data.DbType.String, taobaoProduct.StuffStatus);
			this.database.AddInParameter(sqlStringCommand, "ProTitle", System.Data.DbType.String, taobaoProduct.ProTitle);
			this.database.AddInParameter(sqlStringCommand, "Num", System.Data.DbType.Int64, taobaoProduct.Num);
			this.database.AddInParameter(sqlStringCommand, "LocationState", System.Data.DbType.String, taobaoProduct.LocationState);
			this.database.AddInParameter(sqlStringCommand, "LocationCity", System.Data.DbType.String, taobaoProduct.LocationCity);
			this.database.AddInParameter(sqlStringCommand, "FreightPayer", System.Data.DbType.String, taobaoProduct.FreightPayer);
			this.database.AddInParameter(sqlStringCommand, "PostFee", System.Data.DbType.Currency, taobaoProduct.PostFee);
			this.database.AddInParameter(sqlStringCommand, "ExpressFee", System.Data.DbType.Currency, taobaoProduct.ExpressFee);
			this.database.AddInParameter(sqlStringCommand, "EMSFee", System.Data.DbType.Currency, taobaoProduct.EMSFee);
			this.database.AddInParameter(sqlStringCommand, "HasInvoice", System.Data.DbType.Boolean, taobaoProduct.HasInvoice);
			this.database.AddInParameter(sqlStringCommand, "HasWarranty", System.Data.DbType.Boolean, taobaoProduct.HasWarranty);
			this.database.AddInParameter(sqlStringCommand, "HasDiscount", System.Data.DbType.Boolean, taobaoProduct.HasDiscount);
			this.database.AddInParameter(sqlStringCommand, "ValidThru", System.Data.DbType.Int64, taobaoProduct.ValidThru);
			this.database.AddInParameter(sqlStringCommand, "ListTime", System.Data.DbType.DateTime, taobaoProduct.ListTime);
			this.database.AddInParameter(sqlStringCommand, "PropertyAlias", System.Data.DbType.String, taobaoProduct.PropertyAlias);
			this.database.AddInParameter(sqlStringCommand, "InputPids", System.Data.DbType.String, taobaoProduct.InputPids);
			this.database.AddInParameter(sqlStringCommand, "InputStr", System.Data.DbType.String, taobaoProduct.InputStr);
			this.database.AddInParameter(sqlStringCommand, "SkuProperties", System.Data.DbType.String, taobaoProduct.SkuProperties);
			this.database.AddInParameter(sqlStringCommand, "SkuQuantities", System.Data.DbType.String, taobaoProduct.SkuQuantities);
			this.database.AddInParameter(sqlStringCommand, "SkuPrices", System.Data.DbType.String, taobaoProduct.SkuPrices);
			this.database.AddInParameter(sqlStringCommand, "SkuOuterIds", System.Data.DbType.String, taobaoProduct.SkuOuterIds);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool IsExitTaobaoProduct(long taobaoProductId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT COUNT(*) FROM Hishop_Products WHERE TaobaoProductId = {0}", taobaoProductId));
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
	}
}
