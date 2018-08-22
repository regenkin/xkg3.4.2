using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.VShop
{
	public class ScanScenceDao
	{
		private Database database;

		private static object CreatLockObj = new object();

		public ScanScenceDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public ScanInfos GetScanInfosById(int id)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Vshop_ScanSceneInfos WHERE Id={0}", id));
			ScanInfos result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ScanInfos>(dataReader);
			}
			return result;
		}

		public ScanInfos GetScanInfosByScenceId(string Sceneid)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Vshop_ScanSceneInfos WHERE Sceneid='{0}'", Sceneid));
			ScanInfos result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ScanInfos>(dataReader);
			}
			return result;
		}

		public int GetMaxScenceId(int type, string Platform)
		{
			int num = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT max(Sceneid*1) FROM Vshop_ScanSceneInfos WHERE type=@type and  Platform=@Platform");
			this.database.AddInParameter(sqlStringCommand, "type", System.Data.DbType.Int32, type);
			this.database.AddInParameter(sqlStringCommand, "Platform", System.Data.DbType.String, Platform);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null && obj != DBNull.Value)
			{
				num = Convert.ToInt32(obj);
			}
			if (num == 0)
			{
				num = 1;
			}
			return num;
		}

		public ScanInfos GetScanInfosByUserId(int Userid, int type = 0, string Platform = "WX")
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Vshop_ScanSceneInfos WHERE Platform=@Platform and BindUserId=@BindUserId and type=@type");
			this.database.AddInParameter(sqlStringCommand, "BindUserId", System.Data.DbType.Int32, Userid);
			this.database.AddInParameter(sqlStringCommand, "type", System.Data.DbType.Int16, type);
			this.database.AddInParameter(sqlStringCommand, "Platform", System.Data.DbType.String, Platform);
			ScanInfos result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ScanInfos>(dataReader);
			}
			return result;
		}

		public bool getCreatScanId(int userId, string Platform = "WX", int type = 0)
		{
			bool result = false;
			lock (ScanScenceDao.CreatLockObj)
			{
				int num = this.GetMaxScenceId(type, Platform);
				num++;
				result = this.AddScanInfos(new ScanInfos
				{
					BindUserId = userId,
					Sceneid = num.ToString(),
					CreateTime = DateTime.Now,
					DescInfo = "分销商关注公众号",
					Platform = Platform,
					type = type,
					LastActiveTime = DateTime.Now,
					CodeUrl = ""
				});
			}
			return result;
		}

		public ScanInfos GetScanInfosByTicket(string Ticket)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Vshop_ScanSceneInfos WHERE CodeUrl=@CodeUrl");
			this.database.AddInParameter(sqlStringCommand, "CodeUrl", System.Data.DbType.String, Ticket);
			ScanInfos result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ScanInfos>(dataReader);
			}
			return result;
		}

		public bool AddScanInfos(ScanInfos info)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into Vshop_ScanSceneInfos(Platform,Sceneid,BindUserId,DescInfo,type,CreateTime, CodeUrl,LastActiveTime)\r\n            VALUES(@Platform,@Sceneid,@BindUserId,@DescInfo,@type,@CreateTime, @CodeUrl,@LastActiveTime)");
			this.database.AddInParameter(sqlStringCommand, "BindUserId", System.Data.DbType.Int32, info.BindUserId);
			this.database.AddInParameter(sqlStringCommand, "Sceneid", System.Data.DbType.String, info.Sceneid);
			this.database.AddInParameter(sqlStringCommand, "Platform", System.Data.DbType.String, info.Platform);
			this.database.AddInParameter(sqlStringCommand, "type", System.Data.DbType.Int16, info.type);
			this.database.AddInParameter(sqlStringCommand, "DescInfo", System.Data.DbType.String, info.DescInfo);
			this.database.AddInParameter(sqlStringCommand, "CodeUrl", System.Data.DbType.String, info.CodeUrl);
			this.database.AddInParameter(sqlStringCommand, "CreateTime", System.Data.DbType.Date, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "LastActiveTime", System.Data.DbType.Date, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool updateScanInfos(ScanInfos info)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Vshop_ScanSceneInfos set Platform=@Platform,Sceneid=@Sceneid,BindUserId=@BindUserId,\r\n             DescInfo=@DescInfo,type=@type,CreateTime=@CreateTime,LastActiveTime=@LastActiveTime, CodeUrl=@CodeUrl where Id=@Id");
			this.database.AddInParameter(sqlStringCommand, "BindUserId", System.Data.DbType.Int32, info.BindUserId);
			this.database.AddInParameter(sqlStringCommand, "Sceneid", System.Data.DbType.String, info.Sceneid);
			this.database.AddInParameter(sqlStringCommand, "Platform", System.Data.DbType.String, info.Platform);
			this.database.AddInParameter(sqlStringCommand, "type", System.Data.DbType.Int16, info.type);
			this.database.AddInParameter(sqlStringCommand, "DescInfo", System.Data.DbType.String, info.DescInfo);
			this.database.AddInParameter(sqlStringCommand, "CodeUrl", System.Data.DbType.String, info.CodeUrl);
			this.database.AddInParameter(sqlStringCommand, "CreateTime", System.Data.DbType.Date, info.CreateTime);
			this.database.AddInParameter(sqlStringCommand, "LastActiveTime", System.Data.DbType.Date, info.LastActiveTime);
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, info.id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ClearScanBind(string Platform)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from Vshop_ScanSceneInfos where Platform=@Platform");
			this.database.AddInParameter(sqlStringCommand, "Platform", System.Data.DbType.String, Platform);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool updateScanInfosCodeUrl(ScanInfos info)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Vshop_ScanSceneInfos set CreateTime=@CreateTime,LastActiveTime=@LastActiveTime,CodeUrl=@CodeUrl where Id=@Id");
			this.database.AddInParameter(sqlStringCommand, "CodeUrl", System.Data.DbType.String, info.CodeUrl);
			this.database.AddInParameter(sqlStringCommand, "CreateTime", System.Data.DbType.Date, info.CreateTime);
			this.database.AddInParameter(sqlStringCommand, "LastActiveTime", System.Data.DbType.Date, info.LastActiveTime);
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, info.id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool updateScanInfosLastActiveTime(ScanInfos info)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Vshop_ScanSceneInfos set LastActiveTime=@LastActiveTime where Sceneid=@Sceneid");
			this.database.AddInParameter(sqlStringCommand, "Sceneid", System.Data.DbType.String, info.Sceneid);
			this.database.AddInParameter(sqlStringCommand, "LastActiveTime", System.Data.DbType.Date, info.LastActiveTime);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
