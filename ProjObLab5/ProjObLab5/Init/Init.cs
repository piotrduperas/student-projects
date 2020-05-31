//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using TravelAgencies.DataAccess;

namespace TravelAgencies.Init
{
	class Init
	{
		static Random R = new Random(100000001);
		public static (BookingDatabase, TripAdvisorDatabase, ShutterStockDatabase, OysterDatabase) Run()
		{
			var accomodationData = BookingGenerator.GenerateBookingDataBase(R, 25, 6);
			var tripsData = TripAdvisorGenerator.GenerateTripAdvisorDataBase(R, 30);
			var photosData = ShutterStockGenerator.GenerateShutterStockDataBase(R, 200, 12, 0.1);
			var reviewData = OysterGenerator.GenerateOysterDataBase(R, 50, R.NextDouble() / 5 + 0.4);

			return (accomodationData, tripsData, photosData, reviewData);
		}
	}
}
