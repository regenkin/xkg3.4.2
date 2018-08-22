using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Settings;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Sales
{
	public class ExpressTemplateDao
	{
		private Database database;

		public ExpressTemplateDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool AddExpressTemplate(string expressName, string xmlFile)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ExpressTemplates(ExpressName, XmlFile, IsUse) VALUES(@ExpressName, @XmlFile, 1)");
			this.database.AddInParameter(sqlStringCommand, "ExpressName", System.Data.DbType.String, expressName);
			this.database.AddInParameter(sqlStringCommand, "XmlFile", System.Data.DbType.String, xmlFile);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateExpressTemplate(int expressId, string expressName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ExpressTemplates SET ExpressName = @ExpressName WHERE ExpressId = @ExpressId");
			this.database.AddInParameter(sqlStringCommand, "ExpressName", System.Data.DbType.String, expressName);
			this.database.AddInParameter(sqlStringCommand, "ExpressId", System.Data.DbType.Int32, expressId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetExpressIsUse(int expressId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ExpressTemplates SET IsUse = ~IsUse WHERE ExpressId = @ExpressId");
			this.database.AddInParameter(sqlStringCommand, "ExpressId", System.Data.DbType.Int32, expressId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteExpressTemplate(int expressId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ExpressTemplates WHERE ExpressId = @ExpressId");
			this.database.AddInParameter(sqlStringCommand, "ExpressId", System.Data.DbType.Int32, expressId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int DeleteExpressTemplates(string expressIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ExpressTemplates WHERE ExpressId in(" + expressIds + ")");
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public System.Data.DataTable GetExpressTemplates(bool? isUser)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ExpressTemplates");
			if (isUser.HasValue)
			{
				System.Data.Common.DbCommand expr_24 = sqlStringCommand;
				expr_24.CommandText += string.Format(" WHERE IsUse = '{0}'", isUser);
			}
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public bool SetExpressDefault(int expressId)
		{
			string text = "UPDATE Hishop_ExpressTemplates SET IsUse = 1 WHERE ExpressId = @ExpressId;";
			text += "UPDATE Hishop_ExpressTemplates SET IsDefault = 0 WHERE IsDefault = 1 and ExpressId!=@ExpressId;";
			text += "UPDATE Hishop_ExpressTemplates SET IsDefault = ~IsDefault WHERE ExpressId = @ExpressId;";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "ExpressId", System.Data.DbType.Int32, expressId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool IsExistExpress(string ExpressName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(ExpressId) as c FROM Hishop_ExpressTemplates WHERE ExpressName=@ExpressName");
			this.database.AddInParameter(sqlStringCommand, "ExpressName", System.Data.DbType.String, ExpressName);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public IList<FreightTemplate> GetFreightTemplates()
		{
			IList<FreightTemplate> result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_FreightTemplate_Templates order by TemplateId desc");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<FreightTemplate>(dataReader);
			}
			return result;
		}
	}
}
