using Hidistro.Entities;
using Hidistro.Entities.Settings;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Settings
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM VShop_NavMenu WHERE MenuId = @MenuId");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM VShop_NavMenu WHERE ParentMenuId = 0");
			IList<MenuInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MenuInfo>(dataReader);
			}
			return result;
		}

		public bool UpdateMenu(MenuInfo menu)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE VShop_NavMenu SET ParentMenuId = @ParentMenuId, Name = @Name, Type = @Type,DisplaySequence = @DisplaySequence,  [Content] = @Content,ShopMenuPic=@ShopMenuPic WHERE MenuId = @MenuId");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, menu.ParentMenuId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.String, menu.Type);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, menu.DisplaySequence);
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menu.MenuId);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, menu.Content);
			this.database.AddInParameter(sqlStringCommand, "ShopMenuPic", System.Data.DbType.String, menu.ShopMenuPic);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public int SaveMenu(MenuInfo menu)
		{
			int allMenusCount = this.GetAllMenusCount();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO VShop_NavMenu (ParentMenuId, Name, Type,  DisplaySequence,  [Content],ShopMenuPic) VALUES(@ParentMenuId, @Name, @Type, @DisplaySequence, @Content,@ShopMenuPic);select @@IDENTITY ;");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, menu.ParentMenuId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.String, menu.Type);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, allMenusCount);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, menu.Content);
			this.database.AddInParameter(sqlStringCommand, "ShopMenuPic", System.Data.DbType.String, menu.ShopMenuPic);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool UpdateMenuName(MenuInfo menu)
		{
			int allMenusCount = this.GetAllMenusCount();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update VShop_NavMenu  set Name=@Name  where MenuId=@MenuId");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menu.MenuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		private int GetAllMenusCount()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from VShop_NavMenu");
			return 1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool DeleteMenu(int menuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE VShop_NavMenu WHERE (MenuId = @MenuId or ParentMenuId=@ParentMenuId)");
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menuId);
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, menuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<MenuInfo> GetMenusByParentId(int parentId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM VShop_NavMenu WHERE ParentMenuId = @ParentMenuId");
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
