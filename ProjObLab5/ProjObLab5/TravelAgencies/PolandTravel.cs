//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using TravelAgencies.DataAccess;

namespace TravelAgencies.Agencies
{
	class PolandTravel : AbstractTravelAgency
	{
		public PolandTravel(
			IDatabase<RoomData> rooms,
			IDatabase<PhotMetadata> photos,
			IDatabase<AttractionData> attractions,
			IDatabase<ReviewData> reviews,
			Random rd)
			: base(rooms, photos, attractions, reviews, rd)
		{
			this.photos = new LocationPhotoFilter(this.photos, 49.8, 54.2, 14.4, 23.5);
			this.attractions = new CountryTripFilter(this.attractions, "Poland");
		}

		public override IPhoto CreatePhoto()
		{
			photos.Move();
			return new PolandPhoto(Photo.FromPhotMetadata(photos.Current()));
		}

		public override IReview CreateReview()
		{
			reviews.Move();
			ReviewData rv = reviews.Current();
			return new PolandReview(new Review(rv.UserName, rv.Review));
		}

		public override ITrip CreateTrip()
		{
			int days = rd.Next(1, 5);
			return new PolandTrip(new Trip(PrepareTrip(days)));
		}
	}



	class PolandPhoto : PhotoDecorator
	{
		public PolandPhoto(IPhoto p) : base(p) { }
		public override string Name => photo.Name.Replace('c', 'ć').Replace('s', 'ś');
	}

	class PolandTrip : TripDecorator
	{
		public PolandTrip(ITrip t) : base(t) { }
		public override string Country => "Poland";
	}

	class PolandReview : ReviewDecorator
	{
		public PolandReview(IReview r) : base(r) { }
		public override string UserName => review.UserName.Replace('a', 'ą').Replace('e', 'ę');
		public override string Content => review.Content.Replace('a', 'ą').Replace('e', 'ę');

	}
}
