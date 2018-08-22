using System;
using System.Reflection;
using System.Resources;

namespace Hishop.Components.Validation
{
	public static class ResourceLoader
	{
		public static string LoadString(string baseName, string resourceName)
		{
			return ResourceLoader.LoadString(baseName, resourceName, Assembly.GetCallingAssembly());
		}

		public static string LoadString(string baseName, string resourceName, Assembly asm)
		{
			if (string.IsNullOrEmpty(baseName))
			{
				throw new ArgumentNullException("baseName");
			}
			if (string.IsNullOrEmpty(resourceName))
			{
				throw new ArgumentNullException("resourceName");
			}
			string text = null;
			if (asm != null)
			{
				text = ResourceLoader.SearchForResource(asm, baseName, resourceName);
			}
			if (text == null)
			{
				text = ResourceLoader.LoadAssemblyString(Assembly.GetExecutingAssembly(), baseName, resourceName);
			}
			if (text == null)
			{
				return string.Empty;
			}
			return text;
		}

		private static string SearchForResource(Assembly asm, string baseName, string resourceName)
		{
			string[] manifestResourceNames = asm.GetManifestResourceNames();
			string[] array = manifestResourceNames;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				string baseName2 = (string)text.Clone();
				if (text.EndsWith(".resources"))
				{
					baseName2 = text.Replace(".resources", string.Empty);
				}
				string text2 = ResourceLoader.LoadAssemblyString(asm, baseName2, resourceName);
				if (!string.IsNullOrEmpty(text2))
				{
					return text2;
				}
			}
			return null;
		}

		private static string LoadAssemblyString(Assembly asm, string baseName, string resourceName)
		{
			try
			{
				ResourceManager resourceManager = new ResourceManager(baseName, asm);
				return resourceManager.GetString(resourceName);
			}
			catch (MissingManifestResourceException)
			{
			}
			return null;
		}
	}
}
