using System;

namespace Hishop.Weixin.MP.Domain
{
	public class Music : IThumbMedia
	{
		public string Title
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string MusicUrl
		{
			get;
			set;
		}

		public string HQMusicUrl
		{
			get;
			set;
		}

		public int ThumbMediaId
		{
			get;
			set;
		}
	}
}
