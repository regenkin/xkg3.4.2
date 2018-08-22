using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Settings;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Settings
{
	public class ShippingModeDao
	{
		public string Error = "";

		private Database database;

		public ShippingModeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public IList<SpecifyRegionGroup> GetSpecifyRegionGroups(int templateId)
		{
			IList<SpecifyRegionGroup> result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT g.*,r.RegionIds FROM Hishop_FreightTemplate_SpecifyRegionGroups g  LEFT JOIN  vw_Hishop_FreightTemplate_SpecifyRegions r on (g.GroupId=r.GroupId) where  g.TemplateId =" + templateId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<SpecifyRegionGroup>(dataReader);
			}
			return result;
		}

		public FreightTemplate GetFreightTemplate(int templateId, bool includeDetail)
		{
			FreightTemplate freightTemplate = new FreightTemplate();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT * FROM Hishop_FreightTemplate_Templates Where TemplateId =@TemplateId");
			if (includeDetail)
			{
				System.Data.Common.DbCommand expr_24 = sqlStringCommand;
				expr_24.CommandText += " SELECT * FROM Hishop_FreightTemplate_FreeShipping g,vw_Hishop_FreightTemplate_FreeShippingRegions r Where g.FreeId=r.FreeId and TemplateId =@TemplateId";
				System.Data.Common.DbCommand expr_3B = sqlStringCommand;
				expr_3B.CommandText += " SELECT g.*,r.RegionIds FROM Hishop_FreightTemplate_SpecifyRegionGroups g  LEFT JOIN  vw_Hishop_FreightTemplate_SpecifyRegions r on (g.GroupId=r.GroupId) where  g.TemplateId =@TemplateId";
			}
			this.database.AddInParameter(sqlStringCommand, "TemplateId", System.Data.DbType.Int32, templateId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					if (dataReader["TemplateId"] != DBNull.Value)
					{
						freightTemplate.TemplateId = (int)dataReader["TemplateId"];
					}
					freightTemplate.Name = (string)dataReader["Name"];
					freightTemplate.FreeShip = (bool)dataReader["FreeShip"];
					freightTemplate.HasFree = (bool)dataReader["HasFree"];
					freightTemplate.MUnit = (int)((byte)dataReader["MUnit"]);
				}
				if (includeDetail)
				{
					dataReader.NextResult();
					freightTemplate.FreeShippings = new List<FreeShipping>();
					while (dataReader.Read())
					{
						FreeShipping freeShipping = new FreeShipping();
						freeShipping.TemplateId = (int)dataReader["TemplateId"];
						freeShipping.ModeId = (int)((byte)dataReader["ModeId"]);
						freeShipping.FreeId = (int)((decimal)dataReader["FreeId"]);
						freeShipping.ConditionNumber = (string)dataReader["ConditionNumber"];
						freeShipping.ConditionType = (int)((byte)dataReader["ConditionType"]);
						freeShipping.RegionIds = (string)dataReader["RegionIds"];
						freightTemplate.FreeShippings.Add(freeShipping);
					}
					dataReader.NextResult();
					freightTemplate.SpecifyRegionGroups = new List<SpecifyRegionGroup>();
					while (dataReader.Read())
					{
						SpecifyRegionGroup specifyRegionGroup = new SpecifyRegionGroup();
						specifyRegionGroup.TemplateId = (int)dataReader["TemplateId"];
						specifyRegionGroup.GroupId = (int)dataReader["GroupId"];
						specifyRegionGroup.FristNumber = (decimal)dataReader["FristNumber"];
						specifyRegionGroup.FristPrice = (decimal)dataReader["FristPrice"];
						specifyRegionGroup.AddNumber = (decimal)dataReader["AddNumber"];
						specifyRegionGroup.AddPrice = (decimal)dataReader["AddPrice"];
						specifyRegionGroup.ModeId = (int)((byte)dataReader["ModeId"]);
						specifyRegionGroup.IsDefault = (bool)dataReader["IsDefault"];
						string regionIds = "";
						if (DBNull.Value != dataReader["RegionIds"])
						{
							regionIds = (string)dataReader["RegionIds"];
						}
						specifyRegionGroup.RegionIds = regionIds;
						freightTemplate.SpecifyRegionGroups.Add(specifyRegionGroup);
					}
				}
			}
			return freightTemplate;
		}

		public bool CreateShippingTemplate(FreightTemplate freightTemplate)
		{
			this.Error = "";
			bool flag = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_FreightTemplate_Templates(Name,FreeShip,MUnit,HasFree)VALUES(@Name,@FreeShip,@MUnit,@HasFree)");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, freightTemplate.Name);
			this.database.AddInParameter(sqlStringCommand, "FreeShip", System.Data.DbType.Int16, freightTemplate.FreeShip);
			this.database.AddInParameter(sqlStringCommand, "MUnit", System.Data.DbType.Int16, freightTemplate.MUnit);
			this.database.AddInParameter(sqlStringCommand, "HasFree", System.Data.DbType.Int16, freightTemplate.HasFree);
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					this.database.ExecuteNonQuery(sqlStringCommand, dbTransaction);
					sqlStringCommand = this.database.GetSqlStringCommand("SELECT @@Identity");
					object obj = this.database.ExecuteScalar(sqlStringCommand, dbTransaction);
					int num = 0;
					if (obj != null && obj != DBNull.Value)
					{
						int.TryParse(obj.ToString(), out num);
						this.Error = num.ToString();
						flag = (num > 0);
					}
					if (flag && !freightTemplate.FreeShip)
					{
						System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(" ");
						this.database.AddInParameter(sqlStringCommand2, "TemplateId", System.Data.DbType.Int32, num);
						if (freightTemplate.FreeShippings != null && freightTemplate.FreeShippings.Count > 0)
						{
							StringBuilder stringBuilder = new StringBuilder();
							int num2 = 0;
							int num3 = 0;
							stringBuilder.Append("DECLARE @ERR INT; Set @ERR =0;");
							stringBuilder.Append(" DECLARE @FreeId Int;");
							foreach (FreeShipping current in freightTemplate.FreeShippings)
							{
								stringBuilder.Append(" INSERT INTO Hishop_FreightTemplate_FreeShipping(TemplateId,ModeId,ConditionNumber,ConditionType) VALUES( @TemplateId,").Append("@ModeId").Append(num2).Append(",@ConditionNumber").Append(num2).Append(",@ConditionType").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
								this.database.AddInParameter(sqlStringCommand2, "ModeId" + num2, System.Data.DbType.Int32, current.ModeId);
								this.database.AddInParameter(sqlStringCommand2, "ConditionNumber" + num2, System.Data.DbType.String, current.ConditionNumber);
								this.database.AddInParameter(sqlStringCommand2, "ConditionType" + num2, System.Data.DbType.Int32, current.ConditionType);
								stringBuilder.Append("Set @FreeId =@@identity;");
								if (current.FreeShippingRegions != null)
								{
									foreach (FreeShippingRegion current2 in current.FreeShippingRegions)
									{
										stringBuilder.Append(" INSERT INTO Hishop_FreightTemplate_FreeShippingRegions(FreeId,TemplateId,RegionId) VALUES(@FreeId,@TemplateId,").Append("@RegionId").Append(num3).Append("); SELECT @ERR=@ERR+@@ERROR;");
										this.database.AddInParameter(sqlStringCommand2, "RegionId" + num3, System.Data.DbType.Int32, current2.RegionId);
										num3++;
									}
								}
								num2++;
							}
							sqlStringCommand2.CommandText = stringBuilder.Append("SELECT @ERR;").ToString();
							int num4 = (int)this.database.ExecuteScalar(sqlStringCommand2, dbTransaction);
							if (num4 != 0)
							{
								this.Error = "指定包邮信息部份有错误，请查检！";
								dbTransaction.Rollback();
								flag = false;
							}
						}
						if (flag && freightTemplate.SpecifyRegionGroups != null && freightTemplate.SpecifyRegionGroups.Count > 0)
						{
							sqlStringCommand2 = this.database.GetSqlStringCommand(" ");
							this.database.AddInParameter(sqlStringCommand2, "TemplateId", System.Data.DbType.Int32, num);
							StringBuilder stringBuilder = new StringBuilder();
							int num2 = 0;
							int num3 = 0;
							stringBuilder.Append("DECLARE @ERR INT; Set @ERR =0;");
							stringBuilder.Append(" DECLARE @GroupId Int;");
							foreach (SpecifyRegionGroup current3 in freightTemplate.SpecifyRegionGroups)
							{
								stringBuilder.Append(" INSERT INTO Hishop_FreightTemplate_SpecifyRegionGroups(TemplateId,ModeId,FristNumber,FristPrice,AddNumber,AddPrice,IsDefault) VALUES(@TemplateId,").Append("@ModeId").Append(num2).Append(",@FristNumber").Append(num2).Append(",@FristPrice").Append(num2).Append(",@AddNumber").Append(num2).Append(",@AddPrice").Append(num2).Append(",@IsDefault").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
								this.database.AddInParameter(sqlStringCommand2, "ModeId" + num2, System.Data.DbType.Int16, current3.ModeId);
								this.database.AddInParameter(sqlStringCommand2, "FristNumber" + num2, System.Data.DbType.Decimal, current3.FristNumber);
								this.database.AddInParameter(sqlStringCommand2, "FristPrice" + num2, System.Data.DbType.Currency, current3.FristPrice);
								this.database.AddInParameter(sqlStringCommand2, "AddPrice" + num2, System.Data.DbType.Currency, current3.AddPrice);
								this.database.AddInParameter(sqlStringCommand2, "AddNumber" + num2, System.Data.DbType.Decimal, current3.AddNumber);
								this.database.AddInParameter(sqlStringCommand2, "IsDefault" + num2, System.Data.DbType.Int16, current3.IsDefault);
								stringBuilder.Append("Set @GroupId =@@identity;");
								if (current3.SpecifyRegions != null)
								{
									foreach (SpecifyRegion current4 in current3.SpecifyRegions)
									{
										stringBuilder.Append(" INSERT INTO Hishop_FreightTemplate_SpecifyRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId").Append(",@RegionId").Append(num3).Append("); SELECT @ERR=@ERR+@@ERROR;");
										this.database.AddInParameter(sqlStringCommand2, "RegionId" + num3, System.Data.DbType.Int32, current4.RegionId);
										num3++;
									}
								}
								num2++;
							}
							sqlStringCommand2.CommandText = stringBuilder.Append("SELECT @ERR;").ToString();
							int num4 = (int)this.database.ExecuteScalar(sqlStringCommand2, dbTransaction);
							if (num4 != 0)
							{
								this.Error = "运送方式部份信息有错误，请查检！";
								dbTransaction.Rollback();
								flag = false;
							}
						}
					}
					dbTransaction.Commit();
				}
				catch
				{
					if (dbTransaction.Connection != null)
					{
						dbTransaction.Rollback();
					}
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return flag;
		}

		public bool UpdateShippingTemplate(FreightTemplate freightTemplate)
		{
			bool result;
			if (freightTemplate.TemplateId == 0)
			{
				this.Error = "模板ID不存在！";
				result = false;
			}
			else
			{
				bool flag = false;
				StringBuilder stringBuilder = new StringBuilder("UPDATE Hishop_FreightTemplate_Templates SET Name=@Name,FreeShip=@FreeShip,MUnit=@MUnit,HasFree=@HasFree WHERE TemplateId=@TemplateId;");
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, freightTemplate.Name);
				this.database.AddInParameter(sqlStringCommand, "FreeShip", System.Data.DbType.Int16, freightTemplate.FreeShip);
				this.database.AddInParameter(sqlStringCommand, "MUnit", System.Data.DbType.Int16, freightTemplate.MUnit);
				this.database.AddInParameter(sqlStringCommand, "TemplateId", System.Data.DbType.Int32, freightTemplate.TemplateId);
				this.database.AddInParameter(sqlStringCommand, "HasFree", System.Data.DbType.Int16, freightTemplate.HasFree);
				using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
				{
					dbConnection.Open();
					System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						flag = (this.database.ExecuteNonQuery(sqlStringCommand, dbTransaction) > 0);
						System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(" ");
						StringBuilder stringBuilder2 = new StringBuilder();
						if (flag)
						{
							this.database.AddInParameter(sqlStringCommand2, "TemplateId", System.Data.DbType.Int32, freightTemplate.TemplateId);
							stringBuilder2.Append("delete from Hishop_FreightTemplate_SpecifyRegionGroups WHERE TemplateId=@TemplateId;");
							stringBuilder2.Append("delete from Hishop_FreightTemplate_FreeShipping WHERE TemplateId=@TemplateId;");
							stringBuilder2.Append("delete from Hishop_FreightTemplate_FreeShippingRegions WHERE TemplateId=@TemplateId;");
							stringBuilder2.Append("delete from Hishop_FreightTemplate_SpecifyRegions WHERE TemplateId=@TemplateId;");
							sqlStringCommand2.CommandText = stringBuilder2.ToString();
							this.database.ExecuteNonQuery(sqlStringCommand2, dbTransaction);
						}
						if (flag && !freightTemplate.FreeShip)
						{
							if (freightTemplate.FreeShippings != null && freightTemplate.FreeShippings.Count > 0)
							{
								sqlStringCommand2 = this.database.GetSqlStringCommand(" ");
								this.database.AddInParameter(sqlStringCommand2, "TemplateId", System.Data.DbType.Int32, freightTemplate.TemplateId);
								stringBuilder2.Clear();
								int num = 0;
								int num2 = 0;
								stringBuilder2.Append("DECLARE @ERR INT; Set @ERR =0;");
								stringBuilder2.Append(" DECLARE @FreeId Int;");
								foreach (FreeShipping current in freightTemplate.FreeShippings)
								{
									stringBuilder2.Append(" INSERT INTO Hishop_FreightTemplate_FreeShipping(TemplateId,ModeId,ConditionNumber,ConditionType) VALUES( @TemplateId,").Append("@ModeId").Append(num).Append(",@ConditionNumber").Append(num).Append(",@ConditionType").Append(num).Append("); SELECT @ERR=@ERR+@@ERROR;");
									this.database.AddInParameter(sqlStringCommand2, "ModeId" + num, System.Data.DbType.Int32, current.ModeId);
									this.database.AddInParameter(sqlStringCommand2, "ConditionNumber" + num, System.Data.DbType.String, current.ConditionNumber);
									this.database.AddInParameter(sqlStringCommand2, "ConditionType" + num, System.Data.DbType.Int32, current.ConditionType);
									stringBuilder2.Append("Set @FreeId =@@identity;");
									if (current.FreeShippingRegions != null)
									{
										foreach (FreeShippingRegion current2 in current.FreeShippingRegions)
										{
											stringBuilder2.Append(" INSERT INTO Hishop_FreightTemplate_FreeShippingRegions(FreeId,TemplateId,RegionId) VALUES(@FreeId,@TemplateId").Append(",@RegionId").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
											this.database.AddInParameter(sqlStringCommand2, "RegionId" + num2, System.Data.DbType.Int32, current2.RegionId);
											num2++;
										}
									}
									num++;
								}
								sqlStringCommand2.CommandText = stringBuilder2.Append("SELECT @ERR;").ToString();
								int num3 = (int)this.database.ExecuteScalar(sqlStringCommand2, dbTransaction);
								if (num3 != 0)
								{
									dbTransaction.Rollback();
									flag = false;
								}
							}
							if (flag && freightTemplate.SpecifyRegionGroups != null && freightTemplate.SpecifyRegionGroups.Count > 0)
							{
								sqlStringCommand2 = this.database.GetSqlStringCommand(" ");
								this.database.AddInParameter(sqlStringCommand2, "TemplateId", System.Data.DbType.Int32, freightTemplate.TemplateId);
								stringBuilder2.Clear();
								int num = 0;
								int num2 = 0;
								stringBuilder2.Append("DECLARE @ERR INT; Set @ERR =0;");
								stringBuilder2.Append(" DECLARE @GroupId Int;");
								foreach (SpecifyRegionGroup current3 in freightTemplate.SpecifyRegionGroups)
								{
									stringBuilder2.Append(" INSERT INTO Hishop_FreightTemplate_SpecifyRegionGroups(TemplateId,ModeId,FristNumber,FristPrice,AddNumber,AddPrice,IsDefault) VALUES(@TemplateId,").Append("@ModeId").Append(num).Append(",@FristNumber").Append(num).Append(",@FristPrice").Append(num).Append(",@AddNumber").Append(num).Append(",@AddPrice").Append(num).Append(",@IsDefault").Append(num).Append("); SELECT @ERR=@ERR+@@ERROR;");
									this.database.AddInParameter(sqlStringCommand2, "ModeId" + num, System.Data.DbType.Int16, current3.ModeId);
									this.database.AddInParameter(sqlStringCommand2, "FristNumber" + num, System.Data.DbType.Decimal, current3.FristNumber);
									this.database.AddInParameter(sqlStringCommand2, "FristPrice" + num, System.Data.DbType.Currency, current3.FristPrice);
									this.database.AddInParameter(sqlStringCommand2, "AddPrice" + num, System.Data.DbType.Currency, current3.AddPrice);
									this.database.AddInParameter(sqlStringCommand2, "AddNumber" + num, System.Data.DbType.Decimal, current3.AddNumber);
									this.database.AddInParameter(sqlStringCommand2, "IsDefault" + num, System.Data.DbType.Int16, current3.IsDefault);
									stringBuilder2.Append("Set @GroupId =@@identity;");
									if (current3.SpecifyRegions != null)
									{
										foreach (SpecifyRegion current4 in current3.SpecifyRegions)
										{
											stringBuilder2.Append(" INSERT INTO Hishop_FreightTemplate_SpecifyRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId").Append(",@RegionId").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
											this.database.AddInParameter(sqlStringCommand2, "RegionId" + num2, System.Data.DbType.Int32, current4.RegionId);
											num2++;
										}
									}
									num++;
								}
								sqlStringCommand2.CommandText = stringBuilder2.Append("SELECT @ERR;").ToString();
								int num3 = (int)this.database.ExecuteScalar(sqlStringCommand2, dbTransaction);
								if (num3 != 0)
								{
									dbTransaction.Rollback();
									flag = false;
								}
							}
						}
						if (flag)
						{
							dbTransaction.Commit();
						}
						else
						{
							dbTransaction.Rollback();
						}
					}
					catch
					{
						if (dbTransaction.Connection != null)
						{
							dbTransaction.Rollback();
						}
						flag = false;
					}
					finally
					{
						dbConnection.Close();
					}
				}
				result = flag;
			}
			return result;
		}

		public bool DeleteShippingTemplate(int templateId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_FreightTemplate_Templates Where TemplateId=@TemplateId");
			this.database.AddInParameter(sqlStringCommand, "TemplateId", System.Data.DbType.Int32, templateId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public string GetShippingTemplateLinkProduct(int[] templateIds)
		{
			string text = "";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ProductId as pid,FreightTemplateId as tid from Hishop_Products  Where FreightTemplateId in(" + string.Join<int>(",", templateIds) + ")");
			System.Data.DataTable dataTable;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						"^",
						dataRow["tid"],
						"|",
						dataRow["pid"]
					});
				}
			}
			if (text != "")
			{
				text = text.Remove(0, 1);
			}
			return text;
		}

		public int DeleteShippingTemplates(string templateIds)
		{
			string[] array = templateIds.Split(new char[]
			{
				','
			});
			int[] array2 = Array.ConvertAll<string, int>(array, (string s) => int.Parse(s));
			string shippingTemplateLinkProduct = this.GetShippingTemplateLinkProduct(array2);
			List<int> list = new List<int>();
			int[] array3 = array2;
			for (int i = 0; i < array3.Length; i++)
			{
				int item = array3[i];
				if (!("^" + shippingTemplateLinkProduct).Contains("^" + item.ToString() + "|"))
				{
					list.Add(item);
				}
			}
			int result;
			if (list.Count > 0)
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_FreightTemplate_Templates Where TemplateId in(" + string.Join<int>(",", list) + ")");
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public System.Data.DataTable GetAllFreightItems()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select t.Name,t.FreeShip,t.MUnit,t.HasFree,sp.*,s.RegionId from Hishop_FreightTemplate_SpecifyRegionGroups as sp left join Hishop_FreightTemplate_SpecifyRegions as s on sp.GroupId=s.GroupId  left join Hishop_FreightTemplate_Templates as t on t.TemplateId=sp.TemplateId ");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetFreeTemplateShipping(string RegionId, int TemplateId, int ModeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[]
			{
				"select fs.*,f.RegionId from  dbo.Hishop_FreightTemplate_FreeShippingRegions as f left join dbo.Hishop_FreightTemplate_FreeShipping as fs on f.FreeId=fs.FreeId where f.RegionId='",
				RegionId,
				"' and fs.TemplateId=",
				TemplateId,
				" and fs.ModeId=",
				ModeId
			}));
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public FreightTemplate GetTemplateMessage(int TemplateId)
		{
			FreightTemplate result;
			if (TemplateId <= 0)
			{
				result = null;
			}
			else
			{
				FreightTemplate freightTemplate = null;
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Hishop_FreightTemplate_Templates where TemplateId={0}", TemplateId));
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						freightTemplate = DataMapper.PopulateTemplateMessage(dataReader);
					}
				}
				result = freightTemplate;
			}
			return result;
		}

		public System.Data.DataTable GetSpecifyRegionGroupsModeId(string TemplateIds, string RegionId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new string[]
			{
				" select distinct sp.ModeId from Hishop_FreightTemplate_SpecifyRegionGroups as sp left join Hishop_FreightTemplate_SpecifyRegions as s on sp.GroupId=s.GroupId where sp.TemplateId in(",
				TemplateIds,
				") and (s.RegionId='",
				RegionId,
				"' or s.RegionId is null)"
			}));
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
	}
}
