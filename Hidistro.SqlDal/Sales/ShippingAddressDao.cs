using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Sales
{
	public class ShippingAddressDao
	{
		private Database database;

		public ShippingAddressDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public IList<ShippingAddressInfo> GetShippingAddresses(int userId)
		{
			IList<ShippingAddressInfo> result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE  UserID = @UserID");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ShippingAddressInfo>(dataReader);
			}
			return result;
		}

		public int AddShippingAddress(ShippingAddressInfo shippingAddress)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_UserShippingAddresses(RegionId,UserId,ShipTo,Address,Zipcode,TelPhone,CellPhone,IsDefault) VALUES(@RegionId,@UserId,@ShipTo,@Address,@Zipcode,@TelPhone,@CellPhone,@IsDefault); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, shippingAddress.RegionId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, shippingAddress.UserId);
			this.database.AddInParameter(sqlStringCommand, "ShipTo", System.Data.DbType.String, shippingAddress.ShipTo);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, shippingAddress.Address);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, shippingAddress.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, shippingAddress.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, shippingAddress.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "IsDefault", System.Data.DbType.Boolean, shippingAddress.IsDefault);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool DelShippingAddress(int shippingid, int userid)
		{
			StringBuilder stringBuilder = new StringBuilder("delete from Hishop_UserShippingAddresses");
			stringBuilder.Append(" where shippingId=@shippingId and UserId=@UserId ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "shippingId", System.Data.DbType.Int32, shippingid);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateShippingAddress(ShippingAddressInfo shippingAddress)
		{
			string value = shippingAddress.Address.Replace("\n", " ").Replace("\r", "");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update Hishop_UserShippingAddresses");
			stringBuilder.Append(" set ShipTo=@ShipTo,");
			stringBuilder.Append("Address=@Address,");
			stringBuilder.Append("Zipcode=@Zipcode,");
			stringBuilder.Append("TelPhone=@TelPhone,");
			stringBuilder.Append("CellPhone=@CellPhone,");
			stringBuilder.Append(" RegionId=@RegionId");
			stringBuilder.Append(" where shippingId=@shippingId");
			stringBuilder.Append(" and UserId=@UserId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, shippingAddress.RegionId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, shippingAddress.UserId);
			this.database.AddInParameter(sqlStringCommand, "ShipTo", System.Data.DbType.String, shippingAddress.ShipTo);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, value);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, shippingAddress.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, shippingAddress.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, shippingAddress.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "shippingId", System.Data.DbType.Int32, shippingAddress.ShippingId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetDefaultShippingAddress(int shippingId, int UserId)
		{
			StringBuilder stringBuilder = new StringBuilder("UPDATE  Hishop_UserShippingAddresses SET IsDefault = 0 where UserId=@UserId;");
			stringBuilder.Append("UPDATE  Hishop_UserShippingAddresses SET IsDefault = 1 WHERE ShippingId = @ShippingId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "shippingId", System.Data.DbType.Int32, shippingId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public ShippingAddressInfo GetShippingAddress(int shippingId, int userid)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE ShippingId = @ShippingId and UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "ShippingId", System.Data.DbType.Int32, shippingId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userid);
			ShippingAddressInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateShippingAddress(dataReader);
				}
			}
			return result;
		}
	}
}
