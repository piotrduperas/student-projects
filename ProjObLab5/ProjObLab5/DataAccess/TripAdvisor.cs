//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using System.Collections.Generic;

namespace TravelAgencies.DataAccess
{
	public class AttractionData
	{
		public string Name { get; set; }
		public string Price { get; set; }
		public string Rating { get; set; }
		public string Country { get; set; }
		public AttractionData(string Name, string Price, string Rating, string Country)
		{
			this.Name = Name;
			this.Price = Price;
			this.Rating = Rating;
			this.Country = Country;
		}
	}

	class TripAdvisorDecoder : CodecWithEncryptor<AttractionData, string>
	{
		public TripAdvisorDecoder(IEncryptor<string> cipher) : base(cipher)
		{
		}

		public override AttractionData Encrypt(AttractionData t)
		{
			t.Rating = cipher.Encrypt(t.Rating);
			t.Price = cipher.Encrypt(t.Price);
			return t;
		}

		public override AttractionData Decrypt(AttractionData t)
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


	class TripAdvisorDatabase : IDatabase<AttractionData>
	{
		public Guid[] Ids;
		public Dictionary<Guid, string>[] Names { get; set; }
		public Dictionary<Guid, string> Prices { get; set; }//Encrypted
		public Dictionary<Guid, string> Ratings { get; set; }//Encrypted
		public Dictionary<Guid, string> Countries { get; set; }

		public IIterator<AttractionData> GetDecryptedIterator()
		{
			return new DecryptingIterator<AttractionData>(GetIterator(), GetCipher());
		}

		private IEncryptor<AttractionData> GetCipher()
		{
			ChainEncryptor<string> codecChain = new ChainEncryptor<string>();

			codecChain.Join(new PushStringCodec(3))
				.Join(new FrameStringCodec(2))      //PushCodec(n=3) -> FrameCodec(n=2) -> SwapCodec -> PushCodec(n=3)
				.Join(new SwapStringCodec())
				.Join(new PushStringCodec(3));

			return new TripAdvisorDecoder(codecChain);
		}

		public IIterator<AttractionData> GetIterator()
		{
			return new TripAdvisorDatabaseIterator(this);
		}
	}

	class TripAdvisorDatabaseIterator : IIterator<AttractionData>
	{
		private TripAdvisorDatabase tad;
		private ArrayIterator<Guid> GuidIt;
		private int nameIndex;

		public TripAdvisorDatabaseIterator(TripAdvisorDatabase tad)
		{
			this.tad = tad;
			Reset();
		}

		public AttractionData Current()
		{
			Guid id = GuidIt.Current();
			return new AttractionData(tad.Names[nameIndex][id], tad.Prices[id], tad.Ratings[id], tad.Countries[id]);
		}

		public bool Move()
		{
			while (GuidIt.Move())
			{
				Guid id = GuidIt.Current();
				if (!tad.Ratings.ContainsKey(id)) continue;
				if (!tad.Prices.ContainsKey(id)) continue;
				if (!tad.Countries.ContainsKey(id)) continue;

				for(int i = 0; i < tad.Names.Length; i++)
				{
					if (tad.Names[i].ContainsKey(id))
					{
						nameIndex = i;
						return true;
					}
				}
			}
			return false;
		}

		public void Reset()
		{
			GuidIt = new ArrayIterator<Guid>(tad.Ids);
		}
	}
}

