using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Members
{
	public class DistributorGradeDao
	{
		private Database database;

		public DistributorGradeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool SetAddCommission(int gradeid, decimal addcommission)
		{
			string query = "UPDATE aspnet_DistributorGrade set AddCommission=@AddCommission where GradeId=" + gradeid;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "AddCommission", System.Data.DbType.Decimal, addcommission);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ClearAddCommission()
		{
			string query = "UPDATE aspnet_DistributorGrade set AddCommission=0";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool CreateDistributorGrade(DistributorGradeInfo distributorgrade)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (distributorgrade.IsDefault)
			{
				stringBuilder.AppendLine("UPDATE aspnet_DistributorGrade set IsDefault=0 where IsDefault=1;");
			}
			stringBuilder.AppendLine("INSERT INTO aspnet_DistributorGrade").AppendLine("(Name,Description,CommissionsLimit,FirstCommissionRise,SecondCommissionRise,ThirdCommissionRise,IsDefault,Ico)").AppendLine("VALUES(@Name,@Description,@CommissionsLimit,@FirstCommissionRise,@SecondCommissionRise,@ThirdCommissionRise,@IsDefault,@Ico);select @@identity");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, distributorgrade.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, distributorgrade.Description);
			this.database.AddInParameter(sqlStringCommand, "CommissionsLimit", System.Data.DbType.Decimal, distributorgrade.CommissionsLimit);
			this.database.AddInParameter(sqlStringCommand, "FirstCommissionRise", System.Data.DbType.Decimal, distributorgrade.FirstCommissionRise);
			this.database.AddInParameter(sqlStringCommand, "SecondCommissionRise", System.Data.DbType.Decimal, distributorgrade.SecondCommissionRise);
			this.database.AddInParameter(sqlStringCommand, "ThirdCommissionRise", System.Data.DbType.Decimal, distributorgrade.ThirdCommissionRise);
			this.database.AddInParameter(sqlStringCommand, "IsDefault", System.Data.DbType.Boolean, distributorgrade.IsDefault);
			this.database.AddInParameter(sqlStringCommand, "Ico", System.Data.DbType.String, distributorgrade.Ico);
			int num = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
			bool flag = num > 0;
			if (flag && distributorgrade.IsDefault)
			{
				this.SetGradeDefault(num);
			}
			return flag;
		}

		public IList<DistributorGradeInfo> GetDistributorGradeInfos()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_DistributorGrade");
			IList<DistributorGradeInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<DistributorGradeInfo>(dataReader);
			}
			return result;
		}

		public bool IsExistsMinAmount(int gradeid, decimal minorderamount)
		{
			bool result = false;
			string text = "select top 1 GradeId from aspnet_DistributorGrade where CommissionsLimit=" + minorderamount;
			if (gradeid > 0)
			{
				text = text + " and GradeId<>" + gradeid;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				result = true;
			}
			dataReader.Close();
			return result;
		}

		public Dictionary<int, int> GetGradeCount(string ReferralStatus)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DistributorGradeId g, COUNT(UserId) as c FROM aspnet_Distributors where  ReferralStatus=@ReferralStatus group by DistributorGradeId");
			this.database.AddInParameter(sqlStringCommand, "ReferralStatus", System.Data.DbType.String, ReferralStatus);
			System.Data.DataTable dataTable;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				dictionary.Add((int)dataTable.Rows[i]["g"], (int)dataTable.Rows[i]["c"]);
			}
			return dictionary;
		}

		public bool UpdateDistributor(DistributorGradeInfo distributorgrade)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (distributorgrade.IsDefault)
			{
				stringBuilder.AppendLine("UPDATE aspnet_DistributorGrade set IsDefault=0 where IsDefault=1;");
			}
			stringBuilder.AppendLine("UPDATE aspnet_DistributorGrade SET Name=@Name,Description=@Description,CommissionsLimit=@CommissionsLimit,");
			stringBuilder.AppendLine("FirstCommissionRise=@FirstCommissionRise,SecondCommissionRise=@SecondCommissionRise,ThirdCommissionRise=@ThirdCommissionRise,Ico=@Ico,IsDefault=@IsDefault WHERE GradeId=@GradeId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, distributorgrade.GradeId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, distributorgrade.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, distributorgrade.Description);
			this.database.AddInParameter(sqlStringCommand, "CommissionsLimit", System.Data.DbType.Decimal, distributorgrade.CommissionsLimit);
			this.database.AddInParameter(sqlStringCommand, "FirstCommissionRise", System.Data.DbType.Decimal, distributorgrade.FirstCommissionRise);
			this.database.AddInParameter(sqlStringCommand, "SecondCommissionRise", System.Data.DbType.Decimal, distributorgrade.SecondCommissionRise);
			this.database.AddInParameter(sqlStringCommand, "ThirdCommissionRise", System.Data.DbType.Decimal, distributorgrade.ThirdCommissionRise);
			this.database.AddInParameter(sqlStringCommand, "IsDefault", System.Data.DbType.Boolean, distributorgrade.IsDefault);
			this.database.AddInParameter(sqlStringCommand, "Ico", System.Data.DbType.String, distributorgrade.Ico);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DistributorGradeInfo GetDistributorGradeInfo(int gradeid)
		{
			DistributorGradeInfo result;
			if (gradeid <= 0)
			{
				result = null;
			}
			else
			{
				DistributorGradeInfo distributorGradeInfo = null;
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_DistributorGrade where gradeid={0}", gradeid));
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						distributorGradeInfo = DataMapper.PopulateDistributorGradeInfo(dataReader);
					}
				}
				result = distributorGradeInfo;
			}
			return result;
		}

		public DistributorGradeInfo GetIsDefaultDistributorGradeInfo()
		{
			DistributorGradeInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_DistributorGrade where IsDefault=1 order by CommissionsLimit asc", new object[0]));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateDistributorGradeInfo(dataReader);
				}
			}
			return result;
		}

		public System.Data.DataTable GetAllDistributorGrade()
		{
			string query = "select * from aspnet_DistributorGrade order by CommissionsLimit asc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			return dataSet.Tables[0];
		}

		public DbQueryResult GetDistributorGradeRequest(DistributorGradeQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(query.Name))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" Name LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_DistributorGrade ", "GradeID", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public DbQueryResult GetDistributorGrade(DistributorGradeQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.GradeId > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" GradeId = {0}", query.GradeId);
			}
			if (!string.IsNullOrEmpty(query.Name))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" Name LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
			}
			if (!string.IsNullOrEmpty(query.Description))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" Description LIKE '%{0}%'", DataHelper.CleanSearchString(query.Description));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_DistributorGrade", "GradeID", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public bool SetGradeDefault(int gradeid)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_DistributorGrade set IsDefault=0 where GradeId<>@GradeId;UPDATE aspnet_DistributorGrade set IsDefault=1 where GradeId=@GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool HasDistributor(int greadeid)
		{
			string query = "select * from aspnet_Distributors where DistributorGradeId=@DistributorGradeId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "DistributorGradeId", System.Data.DbType.Int32, greadeid);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			return dataSet.Tables[0].Rows.Count > 0;
		}

		public string DelOneGrade(int gradeid)
		{
			string result = string.Empty;
			if (this.HasDistributor(gradeid))
			{
				result = "-1";
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete  aspnet_DistributorGrade  where GradeId=@GradeId and IsDefault=0");
				this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeid);
				result = ((this.database.ExecuteNonQuery(sqlStringCommand) > 0) ? "1" : "0");
			}
			return result;
		}
	}
}
