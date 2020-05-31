//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
namespace TravelAgencies.DataAccess
{

	public class RoomData
	{
		public string Name { get; set; }
		public string Rating { get; set; }
		public string Price { get; set; }

		internal RoomData(string Name, string Rating, string Price)
		{
			(this.Name, this.Rating, this.Price) = (Name, Rating, Price);
		}
	}

	class RoomDataDecoder : CodecWithEncryptor<RoomData, string>
	{
		public RoomDataDecoder(IEncryptor<string> cipher) : base(cipher)
		{
		}

		public override RoomData Encrypt(RoomData t)
		{
			t.Rating = cipher.Encrypt(t.Rating);
			t.Price = cipher.Encrypt(t.Price);
			return t;
		}

		public override RoomData Decrypt(RoomData t)
		{
			string rating = cipher.Decrypt(t.Rating);
			string price = cipher.Decrypt(t.Price);
			if (cipher.Encrypt(rating) != t.Rating) return null;
			if (cipher.Encrypt(price) != t.Price) return null;
			t.Rating = rating;
			t.Price = price;
			return t;
		}
	}


	class ListNodeWrapper : IListNode<ListNode>
	{
		public ListNode Data { get; }
		public ListNodeWrapper(ListNode node)
		{
			this.Data = node;
		}
		public IListNode<ListNode> Next
		{
			get
			{
				return Data.Next == null ? null : new ListNodeWrapper(Data.Next);
			}
		}

		public static IIterator<IIterator<ListNode>> FromArray(ListNode[] arr)
		{
			var lits = new ListIterator<ListNode>[arr.Length];
			
			for(int i = 0; i < arr.Length; i++)
			{
				lits[i] = new ListIterator<ListNode>(new ListNodeWrapper(arr[i]));
			}
			return new ArrayIterator<IIterator<ListNode>>(lits);
		}
	}

	class BookingDatabaseIterator : IteratorDecorator<ListNode, RoomData>
	{
		public BookingDatabaseIterator(BookingDatabase bd) 
			: base(new CompositeIterator<ListNode>(ListNodeWrapper.FromArray(bd.Rooms)))
		{
		}

		public override RoomData Current()
		{
			ListNode ln = it.Current();
			return new RoomData(ln.Name, ln.Rating, ln.Price);
		}
	}

	class ListNode
	{
		public ListNode Next { get; set; }
		public string Name { get; set; }
		public string Rating { get; set; }//Encrypted
		public string Price { get; set; }//Encrypted

	}

	class BookingDatabase : IDatabase<RoomData>
	{
		public ListNode[] Rooms { get; set; }

		public IIterator<RoomData> GetDecryptedIterator()
		{
			return new DecryptingIterator<RoomData>(GetIterator(), GetCipher());
		}

		public IIterator<RoomData> GetIterator()
		{
			return new BookingDatabaseIterator(this);
		}

		private IEncryptor<RoomData> GetCipher()
		{
			ChainEncryptor<string> codecChain = new ChainEncryptor<string>();

			codecChain.Join(new FrameStringCodec(2)) //FrameCodec(n=2) -> ReverseCodec -> CezarCodec(n=-1) -> SwapCodec
				.Join(new ReverseStringCodec())
				.Join(new CezarStringCodec(-1))
				.Join(new SwapStringCodec());

			return new RoomDataDecoder(codecChain);	
		}
	}
}
