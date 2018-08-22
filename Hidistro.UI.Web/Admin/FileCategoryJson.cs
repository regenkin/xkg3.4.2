using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.UI.Web.Admin
{
	public class FileCategoryJson : AdminPage
	{
		protected FileCategoryJson() : base("m01", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
			base.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
			if (Globals.GetCurrentManagerUserId() == 0)
			{
				base.Response.Write(JsonMapper.ToJson(hashtable));
				base.Response.End();
				return;
			}
			System.Collections.Generic.List<System.Collections.Hashtable> list = new System.Collections.Generic.List<System.Collections.Hashtable>();
			hashtable["category_list"] = list;
			System.Data.DataTable photoCategories = GalleryHelper.GetPhotoCategories(0);
			foreach (System.Data.DataRow dataRow in photoCategories.Rows)
			{
				System.Collections.Hashtable hashtable2 = new System.Collections.Hashtable();
				hashtable2["cId"] = dataRow["CategoryId"];
				hashtable2["cName"] = dataRow["CategoryName"];
				list.Add(hashtable2);
			}
			base.Response.Write(JsonMapper.ToJson(hashtable));
			base.Response.End();
		}
	}
}
