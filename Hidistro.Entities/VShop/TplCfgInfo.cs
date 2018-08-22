using System;
using System.Web;

namespace Hidistro.Entities.VShop
{
	public class TplCfgInfo
	{
		public int Id
		{
			get
			{
				return this.BannerId;
			}
			set
			{
				this.Id = this.BannerId;
			}
		}

		public int BannerId
		{
			get;
			set;
		}

		public string ImageUrl
		{
			get;
			set;
		}

		public string ShortDesc
		{
			get;
			set;
		}

		public int DisplaySequence
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public bool IsDisable
		{
			get;
			set;
		}

		public LocationType LocationType
		{
			get;
			set;
		}

		public virtual string LoctionUrl
		{
			get
			{
				int port = HttpContext.Current.Request.Url.Port;
				string arg = HttpContext.Current.Request.Url.Host + ((port == 80) ? "" : (":" + port.ToString()));
				string result = string.Empty;
				switch (this.LocationType)
				{
				case LocationType.Vote:
					result = string.Format("http://{0}/Vshop/Vote.aspx?VoteId={1}", arg, this.Url);
					break;
				case LocationType.Activity:
				{
					string[] array = this.Url.Split(new char[]
					{
						','
					});
					LotteryActivityType lotteryActivityType = (LotteryActivityType)System.Enum.Parse(typeof(LotteryActivityType), array[0]);
					LotteryActivityType lotteryActivityType2 = lotteryActivityType;
					if (lotteryActivityType2 == LotteryActivityType.SignUp)
					{
						result = string.Format("http://{0}/Vshop/Activity.aspx?id={1}", arg, array[1]);
					}
					break;
				}
				case LocationType.Home:
					result = string.Format("http://{0}Default.aspx", arg);
					break;
				case LocationType.Category:
					result = string.Format("http://{0}ProductList.aspx", arg);
					break;
				case LocationType.ShoppingCart:
					result = string.Format("http://{0}/Vshop/ShoppingCart.aspx", arg);
					break;
				case LocationType.OrderCenter:
					result = string.Format("http://{0}/Vshop/MemberCenter.aspx", arg);
					break;
				case LocationType.Link:
					result = "http://" + this.Url;
					if (this.Url.IndexOf("http") > -1)
					{
						result = this.Url;
					}
					break;
				case LocationType.Phone:
					result = "tel://" + this.Url;
					if (this.Url.IndexOf("tel") > -1)
					{
						result = this.Url;
					}
					break;
				case LocationType.Address:
					result = this.Url;
					break;
				case LocationType.Brand:
					result = "/vshop/BrandList.aspx";
					break;
				}
				return result;
			}
		}
	}
}
