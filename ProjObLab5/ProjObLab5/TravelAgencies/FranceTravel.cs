//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TravelAgencies.DataAccess;

namespace TravelAgencies.Agencies
{
	class FranceTravel : AbstractTravelAgency
	{
		public FranceTravel(
			IDatabase<RoomData> rooms,
			IDatabase<PhotMetadata> photos,
			IDatabase<AttractionData> attractions,
			IDatabase<ReviewData> reviews,
			Random rd)
			: base(rooms, photos, attractions, reviews, rd)
		{
			this.photos = new LocationPhotoFilter(this.photos, 43.6, 50.0, 0.0, 5.4);
			this.attractions = new CountryTripFilter(this.attractions, "France");
		}

		public override IPhoto CreatePhoto()
		{
			photos.Move();
			return new FrancePhoto(Photo.FromPhotMetadata(photos.Current()));
		}

		public override IReview CreateReview()
		{
			reviews.Move();
			ReviewData rv = reviews.Current();
			return new FranceReview(new Review(rv.UserName, rv.Review));
		}

		public override ITrip CreateTrip()
		{
			int days = rd.Next(1, 5);
			return new FranceTrip(new Trip(PrepareTrip(days)));
		}
	}



	class FrancePhoto : PhotoDecorator
	{
		public FrancePhoto(IPhoto p) : base(p) { }
	}

	class FranceTrip : TripDecorator
	{
		public FranceTrip(ITrip t) : base(t) { }
		public override string Country => "France";
	}

	class FranceReview : ReviewDecorator
	{
		public FranceReview(IReview r) : base(r) { }
		public override string Content => Regex.Replace(" " + review.Content + " ", @" [a-zA-Z]{1,3} ", " la ").Trim();

	}
}
