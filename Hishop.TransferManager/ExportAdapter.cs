using System;

namespace Hishop.TransferManager
{
	public abstract class ExportAdapter
	{
		public abstract Target Source
		{
			get;
		}

		public abstract Target ExportTo
		{
			get;
		}

		public abstract void DoExport();
	}
}
