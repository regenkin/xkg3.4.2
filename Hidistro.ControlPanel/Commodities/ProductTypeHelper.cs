using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.ControlPanel.Commodities
{
	public sealed class ProductTypeHelper
	{
		private ProductTypeHelper()
		{
		}

		public static DbQueryResult GetProductTypes(ProductTypeQuery query)
		{
			return new ProductTypeDao().GetProductTypes(query);
		}

		public static string GetBrandName(int type)
		{
			return new ProductTypeDao().GetBrandName(type);
		}

		public static IList<ProductTypeInfo> GetProductTypes()
		{
			return new ProductTypeDao().GetProductTypes();
		}

		public static ProductTypeInfo GetProductType(int typeId)
		{
			return new ProductTypeDao().GetProductType(typeId);
		}

		public static System.Data.DataTable GetBrandCategoriesByTypeId(int typeId)
		{
			return new ProductTypeDao().GetBrandCategoriesByTypeId(typeId);
		}

		public static int GetTypeId(string typeName)
		{
			int typeId = new ProductTypeDao().GetTypeId(typeName);
			int result;
			if (typeId > 0)
			{
				result = typeId;
			}
			else
			{
				ProductTypeInfo productTypeInfo = new ProductTypeInfo();
				productTypeInfo.TypeName = typeName;
				result = new ProductTypeDao().AddProductType(productTypeInfo);
			}
			return result;
		}

		public static int AddProductType(ProductTypeInfo productType)
		{
			int result;
			if (productType == null)
			{
				result = 0;
			}
			else
			{
				Globals.EntityCoding(productType, true);
				int num = new ProductTypeDao().AddProductType(productType);
				if (num > 0)
				{
					if (productType.Brands.Count > 0)
					{
						new ProductTypeDao().AddProductTypeBrands(num, productType.Brands);
					}
					EventLogs.WriteOperationLog(Privilege.AddProductType, string.Format(CultureInfo.InvariantCulture, "创建了一个新的商品类型:”{0}”", new object[]
					{
						productType.TypeName
					}));
				}
				result = num;
			}
			return result;
		}

		public static bool UpdateProductType(ProductTypeInfo productType)
		{
			bool result;
			if (productType == null)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(productType, true);
				bool flag = new ProductTypeDao().UpdateProductType(productType);
				if (flag)
				{
					if (new ProductTypeDao().DeleteProductTypeBrands(productType.TypeId))
					{
						new ProductTypeDao().AddProductTypeBrands(productType.TypeId, productType.Brands);
					}
					EventLogs.WriteOperationLog(Privilege.EditProductType, string.Format(CultureInfo.InvariantCulture, "修改了编号为”{0}”的商品类型", new object[]
					{
						productType.TypeId
					}));
				}
				result = flag;
			}
			return result;
		}

		public static bool DeleteProductType(int typeId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
			bool flag = new ProductTypeDao().DeleteProducType(typeId);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.DeleteProductType, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的商品类型", new object[]
				{
					typeId
				}));
			}
			return flag;
		}

		public static AttributeInfo GetAttribute(int attributeId)
		{
			return new AttributeDao().GetAttribute(attributeId);
		}

		public static bool AddAttribute(AttributeInfo attribute)
		{
			return new AttributeDao().AddAttribute(attribute);
		}

		public static int GetSpecificationId(int typeId, string specificationName)
		{
			int specificationId = new AttributeDao().GetSpecificationId(typeId, specificationName);
			int result;
			if (specificationId > 0)
			{
				result = specificationId;
			}
			else
			{
				AttributeInfo attributeInfo = new AttributeInfo();
				attributeInfo.TypeId = typeId;
				attributeInfo.UsageMode = AttributeUseageMode.Choose;
				attributeInfo.UseAttributeImage = false;
				attributeInfo.AttributeName = specificationName;
				result = new AttributeDao().AddAttributeName(attributeInfo);
			}
			return result;
		}

		public static bool AddAttributeName(AttributeInfo attribute)
		{
			return new AttributeDao().AddAttributeName(attribute) > 0;
		}

		public static bool UpdateAttribute(AttributeInfo attribute)
		{
			return new AttributeDao().UpdateAttribute(attribute);
		}

		public static bool UpdateAttributeName(AttributeInfo attribute)
		{
			return new AttributeDao().UpdateAttributeName(attribute);
		}

		public static bool DeleteAttribute(int attriubteId)
		{
			return new AttributeDao().DeleteAttribute(attriubteId);
		}

		public static void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence)
		{
			new AttributeDao().SwapAttributeSequence(attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
		}

		public static IList<AttributeInfo> GetAttributes(int typeId)
		{
			return new AttributeDao().GetAttributes(typeId);
		}

		public static IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode)
		{
			return new AttributeDao().GetAttributes(typeId, attributeUseageMode);
		}

		public static AttributeValueInfo GetAttributeValueInfo(int valueId)
		{
			return new AttributeValueDao().GetAttributeValueInfo(valueId);
		}

		public static int GetSpecificationValueId(int attributeId, string valueStr)
		{
			int specificationValueId = new AttributeValueDao().GetSpecificationValueId(attributeId, valueStr);
			int result;
			if (specificationValueId > 0)
			{
				result = specificationValueId;
			}
			else
			{
				AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
				attributeValueInfo.AttributeId = attributeId;
				attributeValueInfo.ValueStr = valueStr;
				result = new AttributeValueDao().AddAttributeValue(attributeValueInfo);
			}
			return result;
		}

		public static int AddAttributeValue(AttributeValueInfo attributeValue)
		{
			return new AttributeValueDao().AddAttributeValue(attributeValue);
		}

		public static bool ClearAttributeValue(int attributeId)
		{
			return new AttributeValueDao().ClearAttributeValue(attributeId);
		}

		public static bool DeleteAttributeValue(int attributeValueId)
		{
			return new AttributeValueDao().DeleteAttributeValue(attributeValueId);
		}

		public static bool UpdateAttributeValue(AttributeValueInfo attributeValue)
		{
			return new AttributeValueDao().UpdateAttributeValue(attributeValue);
		}

		public static void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
		{
			new AttributeValueDao().SwapAttributeValueSequence(attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
		}

		public static string UploadSKUImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
			{
				result = string.Empty;
			}
			else
			{
				string text = Globals.GetStoragePath() + "/sku/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
	}
}
