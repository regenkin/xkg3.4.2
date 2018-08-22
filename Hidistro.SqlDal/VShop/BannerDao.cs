using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class BannerDao
	{
		private Database database;

		public BannerDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public IList<BannerInfo> GetAllBanners()
		{
			IList<BannerInfo> result = new List<BannerInfo>();
			StringBuilder stringBuilder = new StringBuilder("select * from  Hishop_Banner where type=1 ORDER BY DisplaySequence ASC");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<BannerInfo>(dataReader);
			}
			return result;
		}

		public IList<NavigateInfo> GetAllNavigate()
		{
			StringBuilder stringBuilder = new StringBuilder("select * from  Hishop_Banner where type=2 ORDER BY DisplaySequence ASC");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			IList<NavigateInfo> result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<NavigateInfo>(dataReader);
			}
			return result;
		}

		public int GetCountBanner()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(BannerId) from Hishop_Banner where type=1");
			int result;
			if (this.database.ExecuteScalar(sqlStringCommand) != DBNull.Value)
			{
				result = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
			}
			else
			{
				result = 0;
			}
			return result;
		}

		private int GetMaxBannerSequence()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select max(DisplaySequence) from Hishop_Banner");
			int result;
			if (this.database.ExecuteScalar(sqlStringCommand) != DBNull.Value)
			{
				result = 1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
			}
			else
			{
				result = 1;
			}
			return result;
		}

		public bool SaveTplCfg(TplCfgInfo info)
		{
			int maxBannerSequence = this.GetMaxBannerSequence();
			StringBuilder stringBuilder = new StringBuilder("insert into  Hishop_Banner (ShortDesc,ImageUrl,DisplaySequence,LocationType,Url,Type,IsDisable)");
			stringBuilder.Append("values (@ShortDesc,@ImageUrl,@DisplaySequence,@LocationType,@Url,@Type,@IsDisable)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ShortDesc", System.Data.DbType.String, info.ShortDesc);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, info.ImageUrl);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.String, maxBannerSequence);
			this.database.AddInParameter(sqlStringCommand, "LocationType", System.Data.DbType.Int32, (int)info.LocationType);
			this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, info.Url);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, info.Type);
			this.database.AddInParameter(sqlStringCommand, "IsDisable", System.Data.DbType.Boolean, info.IsDisable);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateTplCfg(TplCfgInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update Hishop_Banner set ");
			stringBuilder.Append("ShortDesc=@ShortDesc,");
			stringBuilder.Append("ImageUrl=@ImageUrl,");
			stringBuilder.Append("DisplaySequence=@DisplaySequence,");
			stringBuilder.Append("LocationType=@LocationType,");
			stringBuilder.Append("Url=@Url,");
			stringBuilder.Append("Type=@Type,");
			stringBuilder.Append("IsDisable=@IsDisable");
			stringBuilder.Append(" where BannerId=@BannerId ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "BannerId", System.Data.DbType.Int32, info.Id);
			this.database.AddInParameter(sqlStringCommand, "ShortDesc", System.Data.DbType.String, info.ShortDesc);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, info.ImageUrl);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.String, info.DisplaySequence);
			this.database.AddInParameter(sqlStringCommand, "LocationType", System.Data.DbType.Int32, (int)info.LocationType);
			this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, info.Url);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, info.Type);
			this.database.AddInParameter(sqlStringCommand, "IsDisable", System.Data.DbType.Boolean, info.IsDisable);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public TplCfgInfo GetTplCfgById(int id)
		{
			StringBuilder stringBuilder = new StringBuilder(" select * from Hishop_Banner where BannerId=@BannerId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "BannerId", System.Data.DbType.Int32, id);
			TplCfgInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<TplCfgInfo>(dataReader);
			}
			return result;
		}

		public bool DelTplCfg(int id)
		{
			StringBuilder stringBuilder = new StringBuilder(" delete from Hishop_Banner where BannerId=@BannerId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "BannerId", System.Data.DbType.Int32, id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
