using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
	public class AttributeValueDao
	{
		private Database database;

		public AttributeValueDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public AttributeValueInfo GetAttributeValueInfo(int valueId)
		{
			AttributeValueInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE ValueId=@ValueId");
			this.database.AddInParameter(sqlStringCommand, "ValueId", System.Data.DbType.Int32, valueId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<AttributeValueInfo>(dataReader);
			}
			return result;
		}

		public int GetSpecificationValueId(int attributeId, string ValueStr)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ValueId FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId AND ValueStr = @ValueStr");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attributeId);
			this.database.AddInParameter(sqlStringCommand, "ValueStr", System.Data.DbType.String, ValueStr);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result = 0;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			return result;
		}

		public int AddAttributeValue(AttributeValueInfo attributeValue)
		{
			int result = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl);SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attributeValue.AttributeId);
			this.database.AddInParameter(sqlStringCommand, "ValueStr", System.Data.DbType.String, attributeValue.ValueStr);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, attributeValue.ImageUrl);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				result = Convert.ToInt32(obj.ToString());
			}
			return result;
		}

		public bool UpdateAttributeValue(AttributeValueInfo attributeValue)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_AttributeValues SET  ValueStr=@ValueStr, ImageUrl=@ImageUrl WHERE ValueId=@valueId");
			this.database.AddInParameter(sqlStringCommand, "ValueStr", System.Data.DbType.String, attributeValue.ValueStr);
			this.database.AddInParameter(sqlStringCommand, "ValueId", System.Data.DbType.Int32, attributeValue.ValueId);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, attributeValue.ImageUrl);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteAttributeValue(int attributeValueId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE ValueId = @ValueId AND not exists (SELECT * FROM Hishop_SKUItems WHERE ValueId = @ValueId) DELETE FROM Hishop_ProductAttributes WHERE ValueId = @ValueId");
			this.database.AddInParameter(sqlStringCommand, "ValueId", System.Data.DbType.Int32, attributeValueId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ClearAttributeValue(int attributeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId)");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attributeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Hishop_AttributeValues", "ValueId", "DisplaySequence", attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
		}
	}
}
