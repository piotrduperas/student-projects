//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
//Oyster is a website with reviews of various holiday destinations.
namespace TravelAgencies.DataAccess
{
	class ReviewData
	{
		public string Review { get; set; }
		public string UserName { get; set; }
		public ReviewData(BSTNode n)
		{
			if (n == null) return;
			(Review, UserName) = (n.Review, n.UserName);
		}
	}

	class BSTNodeWrapper : IBST<BSTNode>
	{
		public BSTNodeWrapper(BSTNode node)
		{
			this.Data = node;
		}
		public IBST<BSTNode> Left
		{
			get
			{
				if (Data.Left == null) {
					return null;
				}
				return new BSTNodeWrapper(Data.Left);
			}
		}

		public IBST<BSTNode> Right
		{
			get
			{
				if (Data.Right == null)
				{
					return null;
				}
				return new BSTNodeWrapper(Data.Right);
			}
		}

		public BSTNode Data { get; }
	}

	class BSTNode
	{
		public string Review { get; set; }
		public string UserName { get; set; }
		public BSTNode Left { get; set; }
		public BSTNode Right { get; set; }
	}

	class OysterDatabase : IDatabase<ReviewData>
	{
		public BSTNode Reviews { get; set; }

		public IIterator<ReviewData> GetDecryptedIterator()
		{
			return GetIterator();
		}

		public IIterator<ReviewData> GetIterator()
		{
			return new OysterDatabaseIterator(this);
		}
	}

	internal class OysterDatabaseIterator : IteratorDecorator<BSTNode, ReviewData>
	{
		public OysterDatabaseIterator(OysterDatabase od) 
			: base(new BSTInOrderIterator<BSTNode>(new BSTNodeWrapper(od.Reviews)))
		{
		}

		public override ReviewData Current()
		{
			return new ReviewData(it.Current());
		}
	}
}
