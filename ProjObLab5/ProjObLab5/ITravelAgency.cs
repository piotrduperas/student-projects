//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using TravelAgencies.DataAccess;

namespace TravelAgencies.Agencies
{
	public interface ITravelAgency
	{
		ITrip CreateTrip();
		IPhoto CreatePhoto();
		IReview CreateReview();
	}

	abstract class AbstractTravelAgency : ITravelAgency
	{
		protected IIterator<RoomData> rooms;
		protected IIterator<PhotMetadata> photos;
		protected IIterator<AttractionData> attractions;
		protected IIterator<ReviewData> reviews;
		protected Random rd;

		public AbstractTravelAgency(
			IDatabase<RoomData> rooms,
			IDatabase<PhotMetadata> photos,
			IDatabase<AttractionData> attractions,
			IDatabase<ReviewData> reviews,
			Random rd)
		{
			this.rooms = new InfiniteIterator<RoomData>(rooms.GetDecryptedIterator());
			this.photos = new InfiniteIterator<PhotMetadata>(photos.GetDecryptedIterator());
			this.attractions = new InfiniteIterator<AttractionData>(attractions.GetDecryptedIterator());
			this.reviews = new InfiniteIterator<ReviewData>(reviews.GetDecryptedIterator());
			this.rd = rd;
		}

		protected TripDay[] PrepareTrip(int days)
		{
			TripDay[] trip = new TripDay[days];
			for (int i = 0; i < days; i++)
			{
				Attraction[] attr = new Attraction[3];

				for (int j = 0; j < 3; j++)
				{
					attractions.Move();
					AttractionData a = attractions.Current();
					attr[j] = new Attraction(int.Parse(a.Price), int.Parse(a.Rating), a.Name);
				}

				rooms.Move();
				RoomData r = rooms.Current();
				Room room = new Room(int.Parse(r.Price), int.Parse(r.Rating), r.Name);

				trip[i] = new TripDay(room, attr);
			}
			return trip;
		}

		public abstract IPhoto CreatePhoto();
		public abstract IReview CreateReview();
		public abstract ITrip CreateTrip();
	}

	class LocationPhotoFilter : IteratorDecorator<PhotMetadata, PhotMetadata>
	{
		private double minLat, maxLat, minLon, maxLon;
		public LocationPhotoFilter(IIterator<PhotMetadata> it, 
			double minLat, double maxLat, 
			double minLon, double maxLon)
			: base(it)
		{
			this.minLat = minLat;
			this.maxLat = maxLat;
			this.minLon = minLon;
			this.maxLon = maxLon;
		}

		public override PhotMetadata Current()
		{
			return it.Current();
		}

		public override bool Move()
		{
			while (it.Move())
			{
				var c = it.Current();
				if (c.Longitude >= minLon && c.Longitude <= maxLon)
				{
					if (c.Latitude >= minLat && c.Latitude <= maxLat)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	class CountryTripFilter : IteratorDecorator<AttractionData, AttractionData>
	{
		private string country;

		public CountryTripFilter(IIterator<AttractionData> it, string country) : base(it)
		{
			this.country = country;
		}
		public override AttractionData Current()
		{
			return it.Current();
		}
		public override bool Move()
		{
			while (it.Move())
			{
				var c = it.Current();
				if (c.Country == country)
				{
					return true;
				}
			}
			return false;
		}
	}


}