using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SqlDal.Promotions;
using System;
using System.Linq;

namespace ControlPanel.Promotions
{
	public class VoteHelper
	{
		private static VoteDao _vote = new VoteDao();

		public static long Create(VoteInfo vote)
		{
			return VoteHelper._vote.CreateVote(vote);
		}

		public static bool Update(VoteInfo vote, bool isUpdateItems = true)
		{
			bool result;
			if (isUpdateItems)
			{
				result = VoteHelper._vote.UpdateVoteAll(vote);
			}
			else
			{
				result = VoteHelper._vote.UpdateVote(vote);
			}
			return result;
		}

		public static VoteInfo GetVote(long Id)
		{
			return VoteHelper._vote.GetVoteById(Id);
		}

		public static bool Delete(long Id)
		{
			return VoteHelper._vote.DeleteVote(Id) > 0;
		}

		public static DbQueryResult Query(VoteSearch query)
		{
			return VoteHelper._vote.Query(query);
		}

		public static int GetVoteCounts(long voteId)
		{
			return VoteHelper._vote.GetVoteCounts(voteId);
		}

		public static int GetVoteAttends(long voteId)
		{
			return VoteHelper._vote.GetVoteAttends(voteId);
		}

		public static bool Vote(int voteId, string itemIds)
		{
			if (!VoteHelper.IsVote(voteId))
			{
				VoteInfo vote = VoteHelper.GetVote((long)voteId);
				if (vote.IsMultiCheck)
				{
					if (vote.MaxCheck > 0 && vote.MaxCheck < itemIds.Split(new char[]
					{
						','
					}).Count<string>())
					{
						throw new Exception(string.Format("对不起！您最多能选{0}项...", vote.MaxCheck));
					}
				}
				return new VoteDao().Vote(voteId, itemIds);
			}
			throw new Exception("已投过票！");
		}

		public static bool IsVote(int voteId)
		{
			return new VoteDao().IsVote(voteId);
		}
	}
}
