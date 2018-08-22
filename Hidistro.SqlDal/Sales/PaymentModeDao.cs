using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Sales
{
	public class PaymentModeDao
	{
		private Database database;

		public PaymentModeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public PaymentModeActionStatus CreateUpdateDeletePaymentMode(PaymentModeInfo paymentMode, DataProviderAction action)
		{
			PaymentModeActionStatus result;
			if (null == paymentMode)
			{
				result = PaymentModeActionStatus.UnknowError;
			}
			else
			{
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_PaymentType_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", System.Data.DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
				if (action == DataProviderAction.Create)
				{
					this.database.AddOutParameter(storedProcCommand, "ModeId", System.Data.DbType.Int32, 4);
				}
				else
				{
					this.database.AddInParameter(storedProcCommand, "ModeId", System.Data.DbType.Int32, paymentMode.ModeId);
				}
				if (action != DataProviderAction.Delete)
				{
					this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, paymentMode.Name);
					this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, paymentMode.Description);
					this.database.AddInParameter(storedProcCommand, "Gateway", System.Data.DbType.String, paymentMode.Gateway);
					this.database.AddInParameter(storedProcCommand, "IsUseInpour", System.Data.DbType.Boolean, paymentMode.IsUseInpour);
					this.database.AddInParameter(storedProcCommand, "IsUseInDistributor", System.Data.DbType.Boolean, paymentMode.IsUseInDistributor);
					this.database.AddInParameter(storedProcCommand, "Charge", System.Data.DbType.Currency, paymentMode.Charge);
					this.database.AddInParameter(storedProcCommand, "IsPercent", System.Data.DbType.Boolean, paymentMode.IsPercent);
					this.database.AddInParameter(storedProcCommand, "Settings", System.Data.DbType.String, paymentMode.Settings);
				}
				this.database.ExecuteNonQuery(storedProcCommand);
				PaymentModeActionStatus paymentModeActionStatus = (PaymentModeActionStatus)((int)this.database.GetParameterValue(storedProcCommand, "Status"));
				result = paymentModeActionStatus;
			}
			return result;
		}

		public void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Hishop_PaymentTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
		}

		public PaymentModeInfo GetPaymentMode(int modeId)
		{
			PaymentModeInfo result = new PaymentModeInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes WHERE ModeId = @ModeId");
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePayment(dataReader);
				}
			}
			return result;
		}

		public PaymentModeInfo GetPaymentMode(string gateway)
		{
			PaymentModeInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_PaymentTypes WHERE Gateway = @Gateway");
			this.database.AddInParameter(sqlStringCommand, "Gateway", System.Data.DbType.String, gateway);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePayment(dataReader);
				}
			}
			return result;
		}

		public IList<PaymentModeInfo> GetPaymentModes()
		{
			IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes Order by DisplaySequence desc");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulatePayment(dataReader));
				}
			}
			return list;
		}
	}
}
