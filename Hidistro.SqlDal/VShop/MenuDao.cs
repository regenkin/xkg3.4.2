using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.VShop
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu WHERE MenuId = @MenuId");
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menuId);
			MenuInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MenuInfo>(dataReader);
			}
			return result;
		}

		public MenuInfo GetFuwuMenu(int menuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu_Fuwu WHERE MenuId = @MenuId");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu WHERE ParentMenuId = 0 ORDER BY DisplaySequence ASC");
			IList<MenuInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MenuInfo>(dataReader);
			}
			return result;
		}

		public IList<MenuInfo> GetTopFuwuMenus()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu_Fuwu WHERE ParentMenuId = 0 ORDER BY DisplaySequence ASC");
			IList<MenuInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MenuInfo>(dataReader);
			}
			return result;
		}

		public bool UpdateMenu(MenuInfo menu)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_Menu SET ParentMenuId = @ParentMenuId, Name = @Name, Type = @Type, ReplyId = @ReplyId, DisplaySequence = @DisplaySequence, Bind = @Bind, [Content] = @Content WHERE MenuId = @MenuId");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, menu.ParentMenuId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.String, menu.Type);
			this.database.AddInParameter(sqlStringCommand, "ReplyId", System.Data.DbType.Int32, menu.ReplyId);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, menu.DisplaySequence);
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menu.MenuId);
			this.database.AddInParameter(sqlStringCommand, "Bind", System.Data.DbType.Int32, (int)menu.BindType);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, menu.Content);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool UpdateFuwuMenu(MenuInfo menu)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_Menu_Fuwu SET ParentMenuId = @ParentMenuId, Name = @Name, Type = @Type, ReplyId = @ReplyId, DisplaySequence = @DisplaySequence, Bind = @Bind, [Content] = @Content WHERE MenuId = @MenuId");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, menu.ParentMenuId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.String, menu.Type);
			this.database.AddInParameter(sqlStringCommand, "ReplyId", System.Data.DbType.Int32, menu.ReplyId);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, menu.DisplaySequence);
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menu.MenuId);
			this.database.AddInParameter(sqlStringCommand, "Bind", System.Data.DbType.Int32, (int)menu.BindType);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, menu.Content);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool SaveMenu(MenuInfo menu)
		{
			int allMenusCount = this.GetAllMenusCount();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO vshop_Menu (ParentMenuId, Name, Type, ReplyId, DisplaySequence, Bind, [Content]) VALUES(@ParentMenuId, @Name, @Type, @ReplyId, @DisplaySequence, @Bind, @Content)");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, menu.ParentMenuId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.String, menu.Type);
			this.database.AddInParameter(sqlStringCommand, "ReplyId", System.Data.DbType.Int32, menu.ReplyId);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, allMenusCount);
			this.database.AddInParameter(sqlStringCommand, "Bind", System.Data.DbType.Int32, (int)menu.BindType);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, menu.Content);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool SaveFuwuMenu(MenuInfo menu)
		{
			int allFuwuMenusCount = this.GetAllFuwuMenusCount();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO vshop_Menu_Fuwu (ParentMenuId, Name, Type, ReplyId, DisplaySequence, Bind, [Content]) VALUES(@ParentMenuId, @Name, @Type, @ReplyId, @DisplaySequence, @Bind, @Content)");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, menu.ParentMenuId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.String, menu.Type);
			this.database.AddInParameter(sqlStringCommand, "ReplyId", System.Data.DbType.Int32, menu.ReplyId);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, allFuwuMenusCount);
			this.database.AddInParameter(sqlStringCommand, "Bind", System.Data.DbType.Int32, (int)menu.BindType);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, menu.Content);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		private int GetAllMenusCount()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from vshop_Menu");
			return 1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		private int GetAllFuwuMenusCount()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from vshop_Menu_Fuwu");
			return 1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool DeleteMenu(int menuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE vshop_Menu WHERE MenuId = @MenuId or ParentMenuId= @MenuId");
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteFuwuMenu(int menuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE vshop_Menu_Fuwu WHERE MenuId = @MenuId or ParentMenuId= @MenuId");
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void SwapMenuSequence(int menuId, bool isUp)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Menu_SwapDisplaySequence");
			this.database.AddInParameter(storedProcCommand, "MenuId", System.Data.DbType.Int32, menuId);
			this.database.AddInParameter(storedProcCommand, "ZIndex", System.Data.DbType.Int32, isUp ? 0 : 1);
			this.database.ExecuteNonQuery(storedProcCommand);
		}

		public IList<MenuInfo> GetMenusByParentId(int parentId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu WHERE ParentMenuId = @ParentMenuId ORDER BY DisplaySequence ASC");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, parentId);
			IList<MenuInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MenuInfo>(dataReader);
			}
			return result;
		}

		public IList<MenuInfo> GetFuwuMenusByParentId(int parentId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu_Fuwu WHERE ParentMenuId = @ParentMenuId ORDER BY DisplaySequence ASC");
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
