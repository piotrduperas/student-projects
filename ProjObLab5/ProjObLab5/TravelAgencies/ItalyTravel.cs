//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using System.Collections.Generic;
using System.Text;
using TravelAgencies.DataAccess;

namespace TravelAgencies.Agencies
{
	class ItalyTravel : AbstractTravelAgency
	{
		public ItalyTravel(
			IDatabase<RoomData> rooms,
			IDatabase<PhotMetadata> photos,
			IDatabase<AttractionData> attractions,
			IDatabase<ReviewData> reviews,
			Random rd)
			: base(rooms, photos, attractions, reviews, rd)
		{
			this.photos = new LocationPhotoFilter(this.photos, 37.7, 44.0, 8.8, 15.2);
			this.attractions = new CountryTripFilter(this.attractions, "Italy");
		}

		public override IPhoto CreatePhoto()
		{
			photos.Move();
			return new ItalyPhoto(Photo.FromPhotMetadata(photos.Current()));
		}

		public override IReview CreateReview()
		{
			reviews.Move();
			ReviewData rv = reviews.Current();
			return new ItalyReview(new Review(rv.UserName, rv.Review));
		}

		public override ITrip CreateTrip()
		{
			int days = rd.Next(1, 5);
			return new ItalyTrip(new Trip(PrepareTrip(days)));
		}
	}



	class ItalyPhoto : PhotoDecorator
	{
		public ItalyPhoto(IPhoto p) : base(p) { }
		public override string Name => "Dello_" + photo.Name;
	}

	class ItalyTrip : TripDecorator
	{
		public ItalyTrip(ITrip t) : base(t) { }
		public override string Country => "Italy";
	}

	class ItalyReview : ReviewDecorator
	{
		public ItalyReview(IReview r) : base(r) { }
		public override string UserName => "Della_" + review.UserName;
	}
}
