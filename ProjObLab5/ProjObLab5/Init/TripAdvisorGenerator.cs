using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgencies.DataAccess;
//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
namespace TravelAgencies.Init
{
	static class TripAdvisorGenerator
	{
		public static TripAdvisorDatabase GenerateTripAdvisorDataBase(Random R, int dbSize)
		{
			var result = new TripAdvisorDatabase
			{
				Ids = new Guid[dbSize],
				Countries = new Dictionary<Guid, string>(),
				Names = new Dictionary<Guid, string>[R.Next(3, 6)],
				Prices = new Dictionary<Guid, string>(),
				Ratings = new Dictionary<Guid, string>()
			};
			for (int i = 0; i < result.Names.Length; i++)
				result.Names[i] = new Dictionary<Guid, string>();
			for (int i = 0; i < dbSize; i++)
			{
				Guid guid = Guid.NewGuid();
				result.Ids[i] = guid;
				result.Names[R.Next(result.Names.Length)][guid] = DBGeneratorUtils.AnyFromArray(R, tripAdvisorNames);
				result.Prices[guid] = valuesMap[(R.Next(1, 80) * 5)];
				result.Ratings[guid] = valuesMap[R.Next(1, 6)];
				result.Countries[guid] = tripAdvisorCountries[R.Next(0, tripAdvisorCountries.Length * 2 - 1) / 2];
			}
			return result;
		}

		static string[] tripAdvisorNames =
		{
			"Casco Historico",
			"Mirador del Valle",
			"Catedral Primada",
			"Custodia de Arfe",
			"Monastery of San Juan",
			"Alcantara Bridge",
			"Puerta de Bisagra",
			"Jewish Quarter",
			"Puerta de Alfonso VI",
			"El Pozo De Los Deseos",
			"Puerta del Sol",
			"Alcazar",
			"Castillo de San Servando",
			"Basilica of St. Anthony",
			"Monastero Di San Daniele",
			"Parco Urbano Termale",
			"Orto Botanico di Padova",
			"Centro Storico",
			"Prato della Valle",
			"Palazzo della Ragione",
			"Castello dei Da Peraga",
			"Centro Commerciale Le Brentelle",
			"Gavrinis Island",
			"Grand Site Naturel de Ploumanac'h",
			"Musee Memoires",
			"Sentier des douaniers",
			"Cote de Granit Rose",
			"Les Aiguilles de Port Coton",
			"Pointe de Pen-Hir",
		};

		static string[] tripAdvisorCountries =
		{
			"France",
			"Italy",
			"Poland",
			"Croatia"
		};

		static Dictionary<int, string> valuesMap = new Dictionary<int, string>()
		{
			{1,"21121"},{2,"22121"},{3,"23121"},{4,"24121"},{5,"25121"},{6,"26121"},
			{10,"012211"},{15,"512211"},{20,"012212"},{25,"512212"},{30,"012213"},
			{35,"512213"},{40,"012214"},{45,"512214"},{50,"012215"},{55,"512215"},
			{60,"012216"},{65,"512216"},{70,"012217"},{75,"512217"},{80,"012218"},
			{85,"512218"},{90,"012219"},{95,"512219"},{100,"2012101"},{105,"2512101"},
			{110,"2012111"},{115,"2512111"},{120,"2012121"},{125,"2512121"},{130,"2012131"},
			{135,"2512131"},{140,"2012141"},{145,"2512141"},{150,"2012151"},{155,"2512151"},
			{160,"2012161"},{165,"2512161"},{170,"2012171"},{175,"2512171"},{180,"2012181"},
			{185,"2512181"},{190,"2012191"},{195,"2512191"},{200,"2012102"},{205,"2512102"},
			{210,"2012112"},{215,"2512112"},{220,"2012122"},{225,"2512122"},{230,"2012132"},
			{235,"2512132"},{240,"2012142"},{245,"2512142"},{250,"2012152"},{255,"2512152"},
			{260,"2012162"},{265,"2512162"},{270,"2012172"},{275,"2512172"},{280,"2012182"},
			{285,"2512182"},{290,"2012192"},{295,"2512192"},{300,"2012103"},{305,"2512103"},
			{310,"2012113"},{315,"2512113"},{320,"2012123"},{325,"2512123"},{330,"2012133"},
			{335,"2512133"},{340,"2012143"},{345,"2512143"},{350,"2012153"},{355,"2512153"},
			{360,"2012163"},{365,"2512163"},{370,"2012173"},{375,"2512173"},{380,"2012183"},
			{385,"2512183"},{390,"2012193"},{395,"2512193"},{400,"2012104"},
		};

	}
}
