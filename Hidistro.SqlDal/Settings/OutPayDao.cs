using Microsoft.Practices.EnterpriseLibrary.Data;
using System;

namespace Hidistro.SqlDal.Settings
{
	public class OutPayDao
	{
		private Database database;

		public OutPayDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
	}
}
