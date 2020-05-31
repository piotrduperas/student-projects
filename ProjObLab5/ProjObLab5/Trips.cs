//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
namespace TravelAgencies.Agencies
{
	public interface ITrip
	{
		string Country { get; }
		TripDay[] Days { get; }
	}

	public class Room
	{
		public int Price { get; }
		public int Rating { get; }
		public string Name { get; }
		public Room(int price, int rating, string name)
		{
			this.Price = price;
			this.Rating = rating;
			this.Name = name;
		}
	}

	public class Attraction
	{
		public int Price { get; }
		public int Rating { get; }
		public string Name { get; }
		public Attraction(int price, int rating, string name)
		{
			this.Price = price;
			this.Rating = rating;
			this.Name = name;
		}
	}
	public class TripDay
	{
		public Room Room { get; }
		public Attraction[] Attractions { get; }
		public TripDay(Room room, Attraction[] attractions)
		{
			this.Room = room;
			this.Attractions = attractions;
		}
	}
	class Trip : ITrip
	{
		public string Country { get; }
		public TripDay[] Days { get; }
		public Trip(TripDay[] days) 
		{
			this.Days = days;
		}
	}

	abstract class TripDecorator : ITrip
	{
		protected ITrip trip;

		public TripDecorator(ITrip trip)
		{
			this.trip = trip;
		}
		public virtual string Country => trip.Country;
		public virtual TripDay[] Days => trip.Days;
	}
}
