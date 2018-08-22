using Hidistro.Entities;
using Hidistro.Entities.Weibo;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Weibo
{
	public class MenuDao
	{
		private Database database;

		public MenuDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public MenuInfo GetMenu(int menuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Menu WHERE MenuId = @MenuId");
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menuId);
			MenuInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MenuInfo>(dataReader);
			}
			return result;
		}

		public IList<MenuInfo> GetTopMenus()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Menu WHERE ParentMenuId = 0");
			IList<MenuInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MenuInfo>(dataReader);
			}
			return result;
		}

		public bool UpdateMenu(MenuInfo menu)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Weibo_Menu SET ParentMenuId = @ParentMenuId, Name = @Name, Type = @Type,DisplaySequence = @DisplaySequence,  [Content] = @Content WHERE MenuId = @MenuId");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, menu.ParentMenuId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.String, "view");
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, menu.DisplaySequence);
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menu.MenuId);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, menu.Content);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool SaveMenu(MenuInfo menu)
		{
			int allMenusCount = this.GetAllMenusCount();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Weibo_Menu (ParentMenuId, Name, Type,  DisplaySequence,  [Content]) VALUES(@ParentMenuId, @Name, @Type, @DisplaySequence, @Content)");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, menu.ParentMenuId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.String, "view");
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, allMenusCount);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, menu.Content);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		private int GetAllMenusCount()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from Weibo_Menu");
			return 1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool DeleteMenu(int menuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE Weibo_Menu WHERE MenuId = @MenuId");
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<MenuInfo> GetMenusByParentId(int parentId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Menu WHERE ParentMenuId = @ParentMenuId ");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, parentId);
			IList<MenuInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MenuInfo>(dataReader);
			}
			return result;
		}
	}
}
