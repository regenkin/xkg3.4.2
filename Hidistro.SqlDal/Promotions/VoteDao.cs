using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class VoteDao
	{
		private Database database;

		public VoteDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public DbQueryResult Query(VoteSearch query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1 ");
			if (query.status != VoteStatus.All)
			{
				if (query.status == VoteStatus.In)
				{
					stringBuilder.AppendFormat("and [StartDate] <= '{0}' and  [EndDate] >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
				else if (query.status == VoteStatus.End)
				{
					stringBuilder.AppendFormat("and [EndDate] < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
				else if (query.status == VoteStatus.unBegin)
				{
					stringBuilder.AppendFormat("and [StartDate] > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
			}
			if (!string.IsNullOrEmpty(query.Name))
			{
				stringBuilder.AppendFormat("and VoteName like '%{0}%'  ", query.Name);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Votes", "VoteId", stringBuilder.ToString(), "*");
		}

		public IList<VoteInfo> GetVoteList()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Votes");
			IList<VoteInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<VoteInfo>(dataReader);
			}
			return result;
		}

		public long CreateVote(VoteInfo vote)
		{
			long result;
			try
			{
				string query = "INSERT INTO [Hishop_Votes]([VoteName],[IsBackup] ,[MaxCheck],[ImageUrl],[StartDate],[EndDate],[Description],[MemberGrades],[DefualtGroup],[CustomGroup],[IsMultiCheck])VALUES (@VoteName,@IsBackup,@MaxCheck,@ImageUrl,@StartDate,@EndDate,@Description,@MemberGrades,@DefualtGroup,@CustomGroup,@IsMultiCheck); SELECT CAST(scope_identity() AS int);";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				this.database.AddInParameter(sqlStringCommand, "VoteName", System.Data.DbType.String, vote.VoteName);
				this.database.AddInParameter(sqlStringCommand, "IsBackup", System.Data.DbType.Boolean, false);
				this.database.AddInParameter(sqlStringCommand, "MaxCheck", System.Data.DbType.Int32, vote.MaxCheck);
				this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, vote.ImageUrl);
				this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, vote.StartDate);
				this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, vote.EndDate);
				this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, vote.Description);
				this.database.AddInParameter(sqlStringCommand, "MemberGrades", System.Data.DbType.String, vote.MemberGrades);
				this.database.AddInParameter(sqlStringCommand, "DefualtGroup", System.Data.DbType.String, vote.DefualtGroup);
				this.database.AddInParameter(sqlStringCommand, "CustomGroup", System.Data.DbType.String, vote.CustomGroup);
				this.database.AddInParameter(sqlStringCommand, "IsMultiCheck", System.Data.DbType.Boolean, vote.IsMultiCheck);
				int num = (int)this.database.ExecuteScalar(sqlStringCommand);
				vote.VoteId = (long)num;
				if (vote.VoteItems != null && vote.VoteItems.Count > 0)
				{
					foreach (VoteItemInfo current in vote.VoteItems)
					{
						string query2 = "INSERT INTO [Hishop_VoteItems]([VoteId],[VoteItemName],[ItemCount]) VALUES (@VoteId,@VoteItemName,@ItemCount)";
						sqlStringCommand = this.database.GetSqlStringCommand(query2);
						this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, num);
						this.database.AddInParameter(sqlStringCommand, "VoteItemName", System.Data.DbType.String, current.VoteItemName);
						this.database.AddInParameter(sqlStringCommand, "ItemCount", System.Data.DbType.Int32, 0);
						this.database.ExecuteNonQuery(sqlStringCommand);
					}
				}
				result = (long)num;
			}
			catch (Exception evar_5_23D)
			{
				result = 0L;
			}
			return result;
		}

		public bool CreateVoteItem(VoteItemInfo item)
		{
			bool result;
			try
			{
				string query = string.Format("INSERT INTO [Hishop_VoteItems] ([VoteId],[VoteItemName],[ItemCount]) VALUES ({0},'{1}',{2})", item.VoteId, item.VoteItemName, item.ItemCount);
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			catch (Exception var_2_48)
			{
				result = false;
			}
			return result;
		}

		public bool UpdateVoteItem(VoteItemInfo item)
		{
			bool result;
			try
			{
				string query = "UPDATE [Hishop_VoteItems] SET [VoteItemName] = @VoteItemName where VoteItemId=@VoteItemId";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				this.database.AddInParameter(sqlStringCommand, "VoteItemName", System.Data.DbType.String, item.VoteItemName);
				this.database.AddInParameter(sqlStringCommand, "VoteItemId", System.Data.DbType.Int64, item.VoteItemId);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			catch (Exception var_2_60)
			{
				result = false;
			}
			return result;
		}

		public bool DeleteItem(long itemId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from Hishop_VoteItems where VoteItemId=@ItemId");
			this.database.AddInParameter(sqlStringCommand, "ItemId", System.Data.DbType.Int64, itemId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateVote(VoteInfo vote)
		{
			if (vote.StartDate > vote.EndDate)
			{
				DateTime startDate = vote.StartDate;
				vote.StartDate = vote.EndDate;
				vote.EndDate = startDate;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_Votes] SET [VoteName] = @VoteName,[MaxCheck] = @MaxCheck,[ImageUrl] = @ImageUrl ,[StartDate] = @StartDate,[EndDate] = @EndDate,[Description] = @Description,[MemberGrades] = @MemberGrades,[DefualtGroup] = @DefualtGroup,[CustomGroup] = @CustomGroup,[IsMultiCheck] = @IsMultiCheck WHERE VoteId = @VoteId ;");
			this.database.AddInParameter(sqlStringCommand, "VoteName", System.Data.DbType.String, vote.VoteName);
			this.database.AddInParameter(sqlStringCommand, "MaxCheck", System.Data.DbType.Int32, vote.MaxCheck);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, vote.ImageUrl);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, vote.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, vote.EndDate);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, vote.Description);
			this.database.AddInParameter(sqlStringCommand, "MemberGrades", System.Data.DbType.String, vote.MemberGrades);
			this.database.AddInParameter(sqlStringCommand, "DefualtGroup", System.Data.DbType.String, vote.DefualtGroup);
			this.database.AddInParameter(sqlStringCommand, "CustomGroup", System.Data.DbType.String, vote.CustomGroup);
			this.database.AddInParameter(sqlStringCommand, "IsMultiCheck", System.Data.DbType.Boolean, vote.IsMultiCheck);
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, vote.VoteId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateVote(VoteInfo vote, System.Data.Common.DbTransaction trans)
		{
			if (vote.StartDate > vote.EndDate)
			{
				DateTime startDate = vote.StartDate;
				vote.StartDate = vote.EndDate;
				vote.EndDate = startDate;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_Votes] SET [VoteName] = @VoteName,[MaxCheck] = @MaxCheck,[ImageUrl] = @ImageUrl ,[StartDate] = @StartDate,[EndDate] = @EndDate,[Description] = @Description,[MemberGrades] = @MemberGrades,[DefualtGroup] = @DefualtGroup,[CustomGroup] = @CustomGroup,[IsMultiCheck] = @IsMultiCheck WHERE VoteId = @VoteId ;");
			this.database.AddInParameter(sqlStringCommand, "VoteName", System.Data.DbType.String, vote.VoteName);
			this.database.AddInParameter(sqlStringCommand, "MaxCheck", System.Data.DbType.Int32, vote.MaxCheck);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, vote.ImageUrl);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, vote.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, vote.EndDate);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, vote.Description);
			this.database.AddInParameter(sqlStringCommand, "MemberGrades", System.Data.DbType.String, vote.MemberGrades);
			this.database.AddInParameter(sqlStringCommand, "DefualtGroup", System.Data.DbType.String, vote.DefualtGroup);
			this.database.AddInParameter(sqlStringCommand, "CustomGroup", System.Data.DbType.String, vote.CustomGroup);
			this.database.AddInParameter(sqlStringCommand, "IsMultiCheck", System.Data.DbType.Boolean, vote.IsMultiCheck);
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, vote.VoteId);
			return this.database.ExecuteNonQuery(sqlStringCommand, trans) > 0;
		}

		public bool UpdateVoteAll(VoteInfo vote)
		{
			bool result;
			try
			{
				VoteInfo voteById = this.GetVoteById(vote.VoteId);
				if (voteById != null)
				{
					if (vote.VoteItems.Count > 0)
					{
						using (IEnumerator<VoteItemInfo> enumerator = vote.VoteItems.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								VoteItemInfo item = enumerator.Current;
								if (voteById.VoteItems.Count == 0)
								{
									this.CreateVoteItem(item);
								}
								else
								{
									List<VoteItemInfo> list = (from x in voteById.VoteItems
									where x.VoteItemName.Equals(item.VoteItemName)
									select x).ToList<VoteItemInfo>();
									if (list.Count == 0)
									{
										this.CreateVoteItem(item);
									}
								}
							}
						}
						List<string> nowId = (from q in vote.VoteItems
						select q.VoteItemName).ToList<string>();
						IEnumerable<VoteItemInfo> enumerable = from x in voteById.VoteItems
						where !nowId.Contains(x.VoteItemName)
						select x;
						if (enumerable.Count<VoteItemInfo>() > 0)
						{
							foreach (VoteItemInfo current in enumerable)
							{
								this.DeleteItem(current.VoteItemId);
							}
						}
						result = this.UpdateVote(vote);
					}
					else
					{
						result = false;
					}
				}
				else
				{
					result = false;
				}
			}
			catch (Exception var_7_1D2)
			{
				result = false;
			}
			return result;
		}

		public int DeleteVote(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Votes WHERE VoteId = @VoteId; DELETE FROM Hishop_VoteItems WHERE VoteId = @VoteId;");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public VoteInfo PopulateVote(System.Data.IDataRecord reader)
		{
			VoteInfo voteInfo = new VoteInfo();
			Type type = voteInfo.GetType();
			for (int i = 0; i < reader.FieldCount; i++)
			{
				string name = reader.GetName(i);
				PropertyInfo property = type.GetProperty(name);
				if (property != null)
				{
					property.SetValue(voteInfo, reader[i], null);
				}
			}
			return voteInfo;
		}

		public VoteInfo GetVoteById(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Votes WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			VoteInfo voteInfo = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					voteInfo = this.PopulateVote(dataReader);
					voteInfo.VoteCounts = this.GetVoteCounts(voteId);
					voteInfo.VoteAttends = this.GetVoteAttends(voteId);
				}
			}
			if (voteInfo != null)
			{
				string query = "select * from Hishop_VoteItems where VoteId=@VoteId";
				sqlStringCommand = this.database.GetSqlStringCommand(query);
				this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					IList<VoteItemInfo> list = ReaderConvert.ReaderToList<VoteItemInfo>(dataReader);
					int voteCounts = voteInfo.VoteCounts;
					if (voteCounts > 0)
					{
						foreach (VoteItemInfo current in list)
						{
							current.Percentage = current.ItemCount * 100 / voteCounts;
						}
					}
					voteInfo.VoteItems = list;
				}
			}
			return voteInfo;
		}

		public IList<VoteItemInfo> GetVoteItems(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			IList<VoteItemInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<VoteItemInfo>(dataReader);
			}
			return result;
		}

		public int CreateVoteItem(VoteItemInfo voteItem, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_VoteItems(VoteId, VoteItemName, ItemCount) Values(@VoteId, @VoteItemName, @ItemCount)");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteItem.VoteId);
			this.database.AddInParameter(sqlStringCommand, "VoteItemName", System.Data.DbType.String, voteItem.VoteItemName);
			this.database.AddInParameter(sqlStringCommand, "ItemCount", System.Data.DbType.Int32, voteItem.ItemCount);
			int result;
			if (dbTran == null)
			{
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			else
			{
				result = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
			}
			return result;
		}

		public bool DeleteVoteItem(long voteId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0;
		}

		public int GetVoteCounts(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}

		public int GetVoteAttends(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(*) as Num FROM Hishop_VoteRecord WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}

		public System.Data.DataTable LoadVote(int voteId, out string voteName, out int checkNum, out int voteNum)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT VoteName, MaxCheck, (SELECT SUM(ItemCount) FROM Hishop_VoteItems WHERE VoteId = @VoteId) AS VoteNum FROM Hishop_Votes WHERE VoteId = @VoteId; SELECT * FROM Hishop_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			voteName = string.Empty;
			checkNum = 1;
			voteNum = 0;
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					voteName = (string)dataReader["VoteName"];
					checkNum = (int)dataReader["MaxCheck"];
					voteNum = (int)dataReader["VoteNum"];
				}
				dataReader.NextResult();
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public bool Vote(int voteId, string itemIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("IF EXISTS (SELECT 1 FROM Hishop_Votes WHERE VoteId=@VoteId AND (GETDATE() < StartDate OR GETDATE() > EndDate) ) return;INSERT INTO Hishop_VoteRecord (UserId, VoteId) VALUES (@UserId, @VoteId);" + string.Format(" UPDATE Hishop_VoteItems SET ItemCount = ItemCount + 1 WHERE VoteId = @VoteId AND VoteItemId IN ({0})", itemIds));
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int64, Globals.GetCurrentMemberUserId());
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool IsVote(int voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_VoteRecord WHERE VoteId = @VoteId AND UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int64, Globals.GetCurrentMemberUserId());
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
	}
}
