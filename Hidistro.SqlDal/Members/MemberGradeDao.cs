using Hidistro.Entities;
using Hidistro.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Members
{
	public class MemberGradeDao
	{
		private Database database;

		public MemberGradeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public IList<MemberGradeInfo> GetMemberGrades(string GradeIds = "")
		{
			string text = "SELECT * FROM aspnet_MemberGrades";
			if (!string.IsNullOrEmpty(GradeIds))
			{
				text = text + " where GradeId in(" + GradeIds + ");";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			IList<MemberGradeInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MemberGradeInfo>(dataReader);
			}
			return result;
		}

		public MemberGradeInfo GetMemberGrade(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			MemberGradeInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MemberGradeInfo>(dataReader);
			}
			return result;
		}

		public int GetDefaultMemberGrade()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT GradeId FROM aspnet_MemberGrades WHERE IsDefault = 1");
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(GradeId) as Count FROM aspnet_MemberGrades WHERE Points=@Points AND GradeId<>@GradeId;");
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, memberGrade.Points);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, memberGrade.GradeId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public bool HasSameMemberGrade(MemberGradeInfo memberGrade)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(GradeId) as Count FROM aspnet_MemberGrades WHERE ((TranVol=@TranVol and TranVol is not null ) or (TranTimes=@TranTimes and TranTimes is not null)) AND GradeId<>@GradeId;");
			this.database.AddInParameter(sqlStringCommand, "TranVol", System.Data.DbType.Double, memberGrade.TranVol);
			this.database.AddInParameter(sqlStringCommand, "TranTimes", System.Data.DbType.Int32, memberGrade.TranTimes);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, memberGrade.GradeId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public bool CreateMemberGrade(MemberGradeInfo memberGrade)
		{
			string text = string.Empty;
			if (memberGrade.IsDefault)
			{
				text += "UPDATE aspnet_MemberGrades SET IsDefault = 0";
			}
			text += " INSERT INTO aspnet_MemberGrades ([Name], Description, Points, IsDefault, Discount,TranVol,TranTimes) VALUES (@Name, @Description, @Points, @IsDefault, @Discount,@TranVol,@TranTimes)";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, memberGrade.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, memberGrade.Description);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, memberGrade.Points);
			this.database.AddInParameter(sqlStringCommand, "IsDefault", System.Data.DbType.Boolean, memberGrade.IsDefault);
			this.database.AddInParameter(sqlStringCommand, "Discount", System.Data.DbType.Int32, memberGrade.Discount);
			this.database.AddInParameter(sqlStringCommand, "TranVol", System.Data.DbType.Double, memberGrade.TranVol);
			this.database.AddInParameter(sqlStringCommand, "TranTimes", System.Data.DbType.Int32, memberGrade.TranTimes);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteMemberGrade(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_MemberGrades WHERE GradeId = @GradeId AND IsDefault = 0 AND NOT EXISTS(SELECT * FROM aspnet_Members WHERE GradeId = @GradeId)");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateMemberGrade(MemberGradeInfo memberGrade)
		{
			string text = "";
			if (memberGrade.IsDefault)
			{
				text += "UPDATE aspnet_MemberGrades SET IsDefault = 0;";
			}
			text += "UPDATE aspnet_MemberGrades SET [Name] = @Name,[IsDefault]=@IsDefault, Description = @Description, Points = @Points, Discount = @Discount ,TranVol=@TranVol ,TranTimes=@TranTimes WHERE GradeId = @GradeId;";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, memberGrade.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, memberGrade.Description);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, memberGrade.Points);
			this.database.AddInParameter(sqlStringCommand, "Discount", System.Data.DbType.Int32, memberGrade.Discount);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, memberGrade.GradeId);
			this.database.AddInParameter(sqlStringCommand, "TranVol", System.Data.DbType.Double, memberGrade.TranVol);
			this.database.AddInParameter(sqlStringCommand, "TranTimes", System.Data.DbType.Int32, memberGrade.TranTimes);
			this.database.AddInParameter(sqlStringCommand, "IsDefault", System.Data.DbType.Boolean, memberGrade.IsDefault);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void SetDefalutMemberGrade(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_MemberGrades SET IsDefault = 0;UPDATE aspnet_MemberGrades SET IsDefault = 1 WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int SelectUserCountGrades(int gid)
		{
			string query = "SELECT COUNT(*) FROM dbo.aspnet_Members WHERE GradeId=" + gid;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool IsExist(string name)
		{
			string query = "SELECT COUNT(*) FROM dbo.aspnet_MemberGrades WHERE Name='" + name + "'";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) > 0;
		}

		public int SetUserGroup(int day)
		{
			string query = "UPDATE dbo.Hishop_UserGroupSet SET ActiveDay=" + day;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int SelectUserGroupSet()
		{
			string query = "SELECT ActiveDay FROM  dbo.Hishop_UserGroupSet ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}
	}
}
