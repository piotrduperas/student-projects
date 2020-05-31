//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;

namespace TravelAgencies.DataAccess
{
	class PhotMetadata
	{
		public string Name { get; set; }
		public string Camera { get; set; }
		public double[] CameraSettings { get; set; }
		public DateTime Date { get; set; }
		public string WidthPx { get; set; }//Encrypted
		public string HeightPx { get; set; }//Encrypted
		public double Longitude { get; set; }
		public double Latitude { get; set; }

	}

	class PhotMetadataDecoder : CodecWithEncryptor<PhotMetadata, string>
	{
		public PhotMetadataDecoder(IEncryptor<string> cipher) : base(cipher)
		{
		}

		public override PhotMetadata Encrypt(PhotMetadata t)
		{
			t.WidthPx = cipher.Encrypt(t.WidthPx);
			t.HeightPx = cipher.Encrypt(t.HeightPx);
			return t;
		}

		public override PhotMetadata Decrypt(PhotMetadata t)
		{
			string width = cipher.Decrypt(t.WidthPx);
			string height = cipher.Decrypt(t.HeightPx);
			if (cipher.Encrypt(width) != t.WidthPx) return null;
			if (cipher.Encrypt(height) != t.HeightPx) return null;
			t.WidthPx = width;
			t.HeightPx = height;
			return t;
		}
	}

	class ShutterStockDatabase : IDatabase<PhotMetadata>
	{
		public PhotMetadata[][][] Photos;

		public IIterator<PhotMetadata> GetDecryptedIterator()
		{
			return new DecryptingIterator<PhotMetadata>(GetIterator(), GetCipher());
		}

		private IEncryptor<PhotMetadata> GetCipher()
		{
			ChainEncryptor<string> codecChain = new ChainEncryptor<string>();

			codecChain.Join(new CezarStringCodec(4))
				.Join(new FrameStringCodec(1)) //CezarCodec(n=4) -> FrameCodec(n=1) -> PushCodec(n=-3) -> ReverseCodec
				.Join(new PushStringCodec(-3))
				.Join(new ReverseStringCodec());

			return new PhotMetadataDecoder(codecChain);
		}

		public IIterator<PhotMetadata> GetIterator()
		{
			return new ShutterStockDatabaseIterator(this);
		}
	}

	class ShutterStockDatabaseIterator : IIterator<PhotMetadata>
	{
		private ShutterStockDatabase ssd;
		private int[] i;

		public ShutterStockDatabaseIterator(ShutterStockDatabase ssd)
		{
			this.ssd = ssd;
			Reset();
		}

		public PhotMetadata Current()
		{
			PhotMetadata p = ssd.Photos[i[0]][i[1]][i[2]];
			PhotMetadata n = new PhotMetadata();
			n.Name = p.Name;
			n.Camera = p.Camera;
			n.CameraSettings = (double[]) p.CameraSettings.Clone(); // Rzutowanie, ale raczej bez znaczenia w kontekscie zadania
			n.Date = p.Date;
			n.WidthPx = p.WidthPx;
			n.HeightPx = p.HeightPx;
			n.Longitude = p.Longitude;
			n.Latitude = p.Latitude;
			return n;
		}

		public bool Move()
		{
			while (i[0] < ssd.Photos.Length)
			{
				var t0 = ssd.Photos[i[0]];

				while (i[1] < t0?.Length)
				{
					var t1 = t0[i[1]];

					while (++i[2] < t1?.Length)
					{
						if (t1[i[2]] != null) return true;
					}

					i[1]++;
					i[2] = -1;
				}
				
				i[0]++;
				i[1] = 0;
			} 

			return false;
		}

		public void Reset()
		{
			i = new int[]{ 0, 0, -1};
		}
	}
}
