using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Settings;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Settings
{
	public class CustomerServiceDao
	{
		public string Error = "";

		private Database database;

		public CustomerServiceDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public int CreateCustomer(CustomerServiceInfo info, ref string msg)
		{
			msg = "未知错误";
			int result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT userver  FROM MeiQia_Userver WHERE userver=@Name");
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, info.userver);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
					msg = "登录手机号重复";
					result = 0;
				}
				else
				{
					sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [MeiQia_Userver]([unit],[userver],[password],[nickname],[realname],[level],[tel]) VALUES (@unit,@userver,@password,@nickname,@realname,@level,@tel); SELECT CAST(scope_identity() AS int);");
					this.database.AddInParameter(sqlStringCommand, "unit", System.Data.DbType.String, info.unit);
					this.database.AddInParameter(sqlStringCommand, "userver", System.Data.DbType.String, info.userver);
					this.database.AddInParameter(sqlStringCommand, "password", System.Data.DbType.String, info.password);
					this.database.AddInParameter(sqlStringCommand, "nickname", System.Data.DbType.String, info.nickname);
					this.database.AddInParameter(sqlStringCommand, "realname", System.Data.DbType.String, info.realname);
					this.database.AddInParameter(sqlStringCommand, "level", System.Data.DbType.String, info.level);
					this.database.AddInParameter(sqlStringCommand, "tel", System.Data.DbType.String, info.tel);
					int num = (int)this.database.ExecuteScalar(sqlStringCommand);
					msg = "";
					result = num;
				}
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				result = 0;
			}
			return result;
		}

		public bool UpdateCustomer(CustomerServiceInfo info, ref string msg)
		{
			msg = "未知错误";
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT userver  FROM MeiQia_Userver WHERE userver=@Name and id <> @ID");
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, info.userver);
				this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, info.id);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
					msg = "登录手机号重复";
					result = false;
				}
				else
				{
					sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [MeiQia_Userver] SET [unit] = @unit,[userver] = @userver,[password] = @password,[nickname] = @nickname,[realname] = @realname,[level] = @level,[tel] = @tel WHERE id=@id");
					this.database.AddInParameter(sqlStringCommand, "unit", System.Data.DbType.String, info.unit);
					this.database.AddInParameter(sqlStringCommand, "userver", System.Data.DbType.String, info.userver);
					this.database.AddInParameter(sqlStringCommand, "password", System.Data.DbType.String, info.password);
					this.database.AddInParameter(sqlStringCommand, "nickname", System.Data.DbType.String, info.nickname);
					this.database.AddInParameter(sqlStringCommand, "realname", System.Data.DbType.String, info.realname);
					this.database.AddInParameter(sqlStringCommand, "level", System.Data.DbType.String, info.level);
					this.database.AddInParameter(sqlStringCommand, "tel", System.Data.DbType.String, info.tel);
					this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, info.id);
					int num = this.database.ExecuteNonQuery(sqlStringCommand);
					msg = "";
					result = (num > 0);
				}
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				result = false;
			}
			return result;
		}

		public bool DeletCustomer(int id)
		{
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Delete  FROM MeiQia_Userver WHERE id = @ID");
				this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, id);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			catch (Exception var_1_3F)
			{
				result = false;
			}
			return result;
		}

		public System.Data.DataTable GetCustomers(string unit)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("select * from  MeiQia_Userver where unit=@unit", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "unit", System.Data.DbType.String, unit);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				result = dataTable;
			}
			return result;
		}

		public CustomerServiceInfo GetCustomer(int id)
		{
			CustomerServiceInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM MeiQia_Userver WHERE id = @ID");
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<CustomerServiceInfo>(dataReader);
			}
			return result;
		}
	}
}
