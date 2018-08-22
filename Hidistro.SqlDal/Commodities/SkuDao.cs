using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.SqlDal.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
	public class SkuDao
	{
		private Database database;

		public SkuDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public SKUItem GetSkuItem(string skuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_SKUs WHERE SkuId=@SkuId;");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			SKUItem result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateSKU(dataReader);
				}
			}
			return result;
		}

		public SKUItem GetProductAndSku(MemberInfo currentMember, int productId, string options)
		{
			SKUItem result;
			if (string.IsNullOrEmpty(options))
			{
				result = null;
			}
			else
			{
				string[] array = options.Split(new char[]
				{
					','
				});
				if (array == null || array.Length <= 0)
				{
					result = null;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (currentMember != null)
					{
						int discount = new MemberGradeDao().GetMemberGrade(currentMember.GradeId).Discount;
						stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice,");
						stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", currentMember.GradeId);
						stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", currentMember.GradeId, discount);
						stringBuilder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
					}
					else
					{
						stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice, SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId");
					}
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text = array2[i];
						string[] array3 = text.Split(new char[]
						{
							':'
						});
						stringBuilder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUItems WHERE AttributeId = {0} AND ValueId = {1}) ", array3[0], array3[1]);
					}
					SKUItem sKUItem = null;
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
					this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
					using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
					{
						if (dataReader.Read())
						{
							sKUItem = DataMapper.PopulateSKU(dataReader);
						}
					}
					result = sKUItem;
				}
			}
			return result;
		}

		public System.Data.DataTable GetSkus(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetExpandAttributes(int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa JOIN Hishop_Attributes a ON pa.AttributeId = a.AttributeId");
			stringBuilder.Append(" JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			System.Data.DataTable dataTable;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			System.Data.DataTable dataTable2 = new System.Data.DataTable();
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				dataTable2 = dataTable.Clone();
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					bool flag = false;
					if (dataTable2.Rows.Count > 0)
					{
						foreach (System.Data.DataRow dataRow2 in dataTable2.Rows)
						{
							if ((int)dataRow2["AttributeId"] == (int)dataRow["AttributeId"])
							{
								flag = true;
								System.Data.DataRow dataRow3;
								(dataRow3 = dataRow2)["ValueStr"] = dataRow3["ValueStr"] + ", " + dataRow["ValueStr"];
							}
						}
					}
					if (!flag)
					{
						System.Data.DataRow dataRow4 = dataTable2.NewRow();
						dataRow4["AttributeId"] = dataRow["AttributeId"];
						dataRow4["AttributeName"] = dataRow["AttributeName"];
						dataRow4["ValueStr"] = dataRow["ValueStr"];
						dataTable2.Rows.Add(dataRow4);
					}
				}
			}
			return dataTable2;
		}
	}
}
