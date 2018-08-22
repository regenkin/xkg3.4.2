using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.VShop;
using System;

namespace Hidistro.SaleSystem.Vshop
{
	public static class ShareMaterialBrowser
	{
		public static NineImgsesItem GetNineImgse(int id)
		{
			return new ShareMaterialDao().GetNineImgse(id);
		}

		public static int AddNineImgses(NineImgsesItem info)
		{
			return new ShareMaterialDao().AddNineImgses(info);
		}

		public static bool UpdateNineImgses(NineImgsesItem info)
		{
			return new ShareMaterialDao().UpdateNineImgses(info);
		}

		public static bool DeleteNineImgses(int id)
		{
			return new ShareMaterialDao().DeleteActivities(id);
		}

		public static DbQueryResult GetNineImgsesList(NineImgsesQuery query)
		{
			return new ShareMaterialDao().GetNineImgsesList(query);
		}
	}
}
