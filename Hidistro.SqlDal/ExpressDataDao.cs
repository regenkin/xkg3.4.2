using Hidistro.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal
{
	public class ExpressDataDao
	{
		private Database database;

		public ExpressDataDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool AddExpressData(ExpressDataInfo model)
		{
			string query;
			if (!string.IsNullOrEmpty(this.GetExpressDataList(model.CompanyCode, model.ExpressNumber)))
			{
				query = "update Hishop_OrderExpressData set DataContent=@DataContent where CompanyCode=@CompanyCode and ExpressNumber=@ExpressNumber";
			}
			else
			{
				query = "insert into Hishop_OrderExpressData(CompanyCode,ExpressNumber,DataContent) values(@CompanyCode,@ExpressNumber,@DataContent)";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "CompanyCode", System.Data.DbType.String, model.CompanyCode);
			this.database.AddInParameter(sqlStringCommand, "ExpressNumber", System.Data.DbType.String, model.ExpressNumber);
			this.database.AddInParameter(sqlStringCommand, "DataContent", System.Data.DbType.String, model.DataContent);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public string GetExpressDataList(string computer, string expressNo)
		{
			string query = "select top 1 DataContent from Hishop_OrderExpressData where CompanyCode=@CompanyCode and ExpressNumber=@ExpressNumber ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "CompanyCode", System.Data.DbType.String, computer);
			this.database.AddInParameter(sqlStringCommand, "ExpressNumber", System.Data.DbType.String, expressNo);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj != null)
			{
				result = obj.ToString();
			}
			else
			{
				result = "";
			}
			return result;
		}
	}
}
