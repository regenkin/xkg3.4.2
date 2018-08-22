using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class ShareMaterialDao
	{
		private Database database;

		public ShareMaterialDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public int AddNineImgses(NineImgsesItem info)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO Vshop_ShareMaterial_NineImages(").Append("ShareDesc,CreatTime,image1,image2,image3,image4,image5,image6,image7,image8,image9)").Append(" VALUES (").Append("@ShareDesc,@CreatTime,@image1,@image2,@image3,@image4,@image5,@image6,@image7,@image8,@image9)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ShareDesc", System.Data.DbType.String, info.ShareDesc);
			this.database.AddInParameter(sqlStringCommand, "CreatTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "image1", System.Data.DbType.String, info.image1);
			this.database.AddInParameter(sqlStringCommand, "image2", System.Data.DbType.String, info.image2);
			this.database.AddInParameter(sqlStringCommand, "image3", System.Data.DbType.String, info.image3);
			this.database.AddInParameter(sqlStringCommand, "image4", System.Data.DbType.String, info.image4);
			this.database.AddInParameter(sqlStringCommand, "image5", System.Data.DbType.String, info.image5);
			this.database.AddInParameter(sqlStringCommand, "image6", System.Data.DbType.String, info.image6);
			this.database.AddInParameter(sqlStringCommand, "image7", System.Data.DbType.String, info.image7);
			this.database.AddInParameter(sqlStringCommand, "image8", System.Data.DbType.String, info.image8);
			this.database.AddInParameter(sqlStringCommand, "image9", System.Data.DbType.String, info.image9);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool UpdateNineImgses(NineImgsesItem info)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Vshop_ShareMaterial_NineImages SET ").Append("ShareDesc=@ShareDesc,").Append("CreatTime=@CreatTime,").Append("image1=@image1,").Append("image2=@image2,").Append("image3=@image3,").Append("image4=@image4,").Append("image5=@image5,").Append("image6=@image6,").Append("image7=@image7,").Append("image8=@image8,").Append("image9=@image9 ").Append(" WHERE id=@id");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ShareDesc", System.Data.DbType.String, info.ShareDesc);
			this.database.AddInParameter(sqlStringCommand, "CreatTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "image1", System.Data.DbType.String, info.image1);
			this.database.AddInParameter(sqlStringCommand, "image2", System.Data.DbType.String, info.image2);
			this.database.AddInParameter(sqlStringCommand, "image3", System.Data.DbType.String, info.image3);
			this.database.AddInParameter(sqlStringCommand, "image4", System.Data.DbType.String, info.image4);
			this.database.AddInParameter(sqlStringCommand, "image5", System.Data.DbType.String, info.image5);
			this.database.AddInParameter(sqlStringCommand, "image6", System.Data.DbType.String, info.image6);
			this.database.AddInParameter(sqlStringCommand, "image7", System.Data.DbType.String, info.image7);
			this.database.AddInParameter(sqlStringCommand, "image8", System.Data.DbType.String, info.image8);
			this.database.AddInParameter(sqlStringCommand, "image9", System.Data.DbType.String, info.image9);
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, info.id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteActivities(int Id)
		{
			string query = "DELETE FROM Vshop_ShareMaterial_NineImages WHERE id=@id";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, Id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DbQueryResult GetNineImgsesList(NineImgsesQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(query.key))
			{
				stringBuilder.AppendFormat(" ShareDesc LIKE '%{0}%'", DataHelper.CleanSearchString(query.key));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Vshop_ShareMaterial_NineImages ", "id", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public NineImgsesItem GetNineImgse(int id)
		{
			NineImgsesItem result = null;
			if (id > 0)
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Vshop_ShareMaterial_NineImages WHERE id={0}", id));
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					result = ReaderConvert.ReaderToModel<NineImgsesItem>(dataReader);
				}
			}
			return result;
		}
	}
}
