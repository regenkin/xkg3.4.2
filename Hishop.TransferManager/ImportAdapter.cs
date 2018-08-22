using System;

namespace Hishop.TransferManager
{
	public abstract class ImportAdapter
	{
		public abstract Target Source
		{
			get;
		}

		public abstract Target ImportTo
		{
			get;
		}

		public abstract string PrepareDataFiles(params object[] initParams);

		public abstract object[] CreateMapping(params object[] initParams);

		public abstract object[] ParseIndexes(params object[] importParams);

		public abstract object[] ParseProductData(params object[] importParams);
	}
}
