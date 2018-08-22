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
	public class ShipperDao
	{
		private Database database;

		public ShipperDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool SwapShipper(int ShipperId, string ShipperTag)
		{
			string value;
			if (ShipperTag == "退货")
			{
				value = "发货";
			}
			else
			{
				value = "退货";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("UPDATE Hishop_Shippers SET ShipperTag=@ShipperTag where  ShipperId != @ShipperId;").Append("UPDATE Hishop_Shippers SET ShipperTag=@NewTag where  ShipperId = @ShipperId;");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ShipperId", System.Data.DbType.Int32, ShipperId);
			this.database.AddInParameter(sqlStringCommand, "ShipperTag", System.Data.DbType.String, ShipperTag);
			this.database.AddInParameter(sqlStringCommand, "NewTag", System.Data.DbType.String, value);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool AddShipper(ShippersInfo shipper)
		{
			string empty = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			if (shipper.ShipperTag == "1")
			{
				stringBuilder.AppendLine("update Hishop_Shippers set ShipperTag='0' where ShipperTag='1' ;");
				stringBuilder.AppendLine("update Hishop_Shippers set ShipperTag='2' where ShipperTag='3' ;");
			}
			else if (shipper.ShipperTag == "2")
			{
				stringBuilder.AppendLine("update Hishop_Shippers set ShipperTag='0' where ShipperTag='2' ;");
				stringBuilder.AppendLine("update Hishop_Shippers set ShipperTag='1' where ShipperTag='3' ;");
			}
			else if (shipper.ShipperTag == "3")
			{
				stringBuilder.AppendLine("update Hishop_Shippers set ShipperTag='0' where ShipperTag='1' ;");
				stringBuilder.AppendLine("update Hishop_Shippers set ShipperTag='0' where ShipperTag='2' ;");
				stringBuilder.AppendLine("update Hishop_Shippers set ShipperTag='0' where ShipperTag='3' ;");
			}
			stringBuilder.AppendLine("IF EXISTS(select top 1 * from Hishop_Shippers where ShipperId=@ShipperId)").AppendLine("Begin").Append("UPDATE Hishop_Shippers SET ShipperTag=@ShipperTag, ShipperName=@ShipperName,").Append("RegionId=@RegionId, Address=@Address, CellPhone=@CellPhone,TelPhone=@TelPhone ").AppendLine("where ShipperId=@ShipperId;").AppendLine("End").AppendLine("ELSE").AppendLine("Begin").Append("INSERT INTO Hishop_Shippers (IsDefault, ShipperTag, ShipperName, RegionId, Address, CellPhone, TelPhone, Zipcode, Remark)").AppendLine(" VALUES (@IsDefault, @ShipperTag, @ShipperName, @RegionId, @Address, @CellPhone, @TelPhone, @Zipcode, @Remark);").AppendLine("End");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "IsDefault", System.Data.DbType.Boolean, shipper.IsDefault);
			this.database.AddInParameter(sqlStringCommand, "ShipperTag", System.Data.DbType.String, shipper.ShipperTag);
			this.database.AddInParameter(sqlStringCommand, "ShipperName", System.Data.DbType.String, shipper.ShipperName);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, shipper.RegionId);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, shipper.Address);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, shipper.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, shipper.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, shipper.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, shipper.Remark);
			this.database.AddInParameter(sqlStringCommand, "ShipperId", System.Data.DbType.Int16, shipper.ShipperId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateShipper(ShippersInfo shipper)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Shippers SET ShipperTag = @ShipperTag, ShipperName = @ShipperName, RegionId = @RegionId, Address = @Address, CellPhone = @CellPhone, TelPhone = @TelPhone, Zipcode = @Zipcode, Remark =@Remark WHERE ShipperId = @ShipperId");
			this.database.AddInParameter(sqlStringCommand, "ShipperTag", System.Data.DbType.String, shipper.ShipperTag);
			this.database.AddInParameter(sqlStringCommand, "ShipperName", System.Data.DbType.String, shipper.ShipperName);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, shipper.RegionId);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, shipper.Address);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, shipper.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, shipper.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, shipper.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, shipper.Remark);
			this.database.AddInParameter(sqlStringCommand, "ShipperId", System.Data.DbType.Int32, shipper.ShipperId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteShipper(int shipperId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Shippers WHERE ShipperId = @ShipperId");
			this.database.AddInParameter(sqlStringCommand, "ShipperId", System.Data.DbType.Int32, shipperId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public ShippersInfo GetShipper(int shipperId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Shippers WHERE ShipperId = @ShipperId");
			this.database.AddInParameter(sqlStringCommand, "ShipperId", System.Data.DbType.Int32, shipperId);
			ShippersInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ShippersInfo>(dataReader);
			}
			return result;
		}

		public IList<ShippersInfo> GetShippers(bool includeDistributor)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Shippers");
			if (!includeDistributor)
			{
				System.Data.Common.DbCommand expr_1A = sqlStringCommand;
				expr_1A.CommandText += " WHERE DistributorUserId = 0";
			}
			IList<ShippersInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ShippersInfo>(dataReader);
			}
			return result;
		}

		public void SetDefalutShipper(int shipperId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Shippers SET IsDefault = 0;UPDATE Hishop_Shippers SET IsDefault = 1 WHERE ShipperId = @ShipperId");
			this.database.AddInParameter(sqlStringCommand, "ShipperId", System.Data.DbType.Int32, shipperId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
