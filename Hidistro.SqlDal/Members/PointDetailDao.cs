using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Members
{
	public class PointDetailDao
	{
		private Database database;

		public PointDetailDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public decimal GetIntegral(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(IntegralChange) FROM vshop_IntegralDetail WHERE Userid = @Userid and IntegralChange>0");
			this.database.AddInParameter(sqlStringCommand, "Userid", System.Data.DbType.Int32, userId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			decimal result;
			if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
			{
				result = (decimal)obj;
			}
			else
			{
				result = 0m;
			}
			return result;
		}
	}
}
