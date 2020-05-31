//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using TravelAgencies.DataAccess;
using TravelAgencies.Agencies;

namespace TravelAgencies
{

	class Program
	{
		static void Main(string[] args) { new Program().Run(); }

		private const int WebsitePermanentOfferCount = 2;
		private const int WebsiteTemporaryOfferCount = 3;
		private Random rd = new Random(257);

		//----------
		//YOUR CODE - additional fileds/properties/methods
		//----------

		public void Run()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			(
				BookingDatabase accomodationData,
				TripAdvisorDatabase tripsData,
				ShutterStockDatabase photosData,
				OysterDatabase reviewData
			) = Init.Init.Run();


			//----------
			//YOUR CODE - set up everything
			//----------

			IAdvertisingAgency[] adAgencies = new IAdvertisingAgency[]{
				new GraphicsAgency(2, 4),
				new GraphicsAgency(3, 3),
				new GraphicsAgency(4, 2),
				new TextAgency(6, 3),
				new TextAgency(2, 2),
				new TextAgency(1, 5),
			};

			ITravelAgency[] travelAgencies = new ITravelAgency[]{
				new FranceTravel(accomodationData, photosData, tripsData, reviewData, rd),
				new PolandTravel(accomodationData, photosData, tripsData, reviewData, rd),
				new ItalyTravel(accomodationData, photosData, tripsData, reviewData, rd),
			};


			while (true)
            {
				Console.Clear();

				//----------
				//YOUR CODE - run
				//----------

				IWebsite offerWebsite = new OfferWebsite();

				IAdvertisingAgency adAgency;
				
				ITravelAgency travelAgency;
				

				for(int i = 0; i < WebsitePermanentOfferCount; i++)
				{
					adAgency = adAgencies[rd.Next(0, adAgencies.Length)];
					travelAgency = travelAgencies[rd.Next(0, travelAgencies.Length)];
					offerWebsite.AddOffer(adAgency.PrepareOffer(travelAgency));
				}

				for (int i = 0; i < WebsiteTemporaryOfferCount; i++)
				{
					adAgency = adAgencies[rd.Next(0, adAgencies.Length)];
					travelAgency = travelAgencies[rd.Next(0, travelAgencies.Length)];
					offerWebsite.AddOffer(adAgency.PrepareTemporaryOffer(travelAgency));
				}


				//uncomment
				Console.WriteLine("\n\n=======================FIRST PRESENT======================");
				offerWebsite.Present();
				Console.WriteLine("\n\n=======================SECOND PRESENT======================");
				offerWebsite.Present();
				Console.WriteLine("\n\n=======================THIRD PRESENT======================");
				offerWebsite.Present();


				if (HandleInput()) break;
			}
		}
		bool HandleInput()
		{
			var key = Console.ReadKey(true);
			return key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.Q;
		}
    }
}
