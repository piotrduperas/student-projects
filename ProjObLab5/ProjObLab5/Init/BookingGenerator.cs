//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgencies.DataAccess;

namespace TravelAgencies.Init
{
	static class BookingGenerator
	{
		public static BookingDatabase GenerateBookingDataBase(Random R, int dbSize, int listMaxLength)
		{
			var result = new BookingDatabase();
			var list = new List<ListNode>();
			int toGenerateLeft = dbSize;
			while (toGenerateLeft > 0)
			{
				var listNode = GenerateRoom(R);
				ListNode iter = listNode;
				for (int i = Math.Min(R.Next(1, listMaxLength), toGenerateLeft - 1); i > 0; i--)
				{
					iter.Next = GenerateRoom(R);
					toGenerateLeft--;
					iter = iter.Next;
				}
				toGenerateLeft--;
				list.Add(listNode);
			}
			result.Rooms = list.ToArray();
			return result;
		}

		private static ListNode GenerateRoom(Random R)
		{
			return new ListNode()
			{
				Name = DBGeneratorUtils.AnyFromArray(R, bookingNames),
				Price = valueMap[R.Next(2, 20) * 10 - (R.NextDouble() < 0.7 ? 1 : 0)],
				Rating = valueMap[R.Next(1, 6)]
			};
		}

		static string[] bookingNames =
		{
			"Ag Spagna Apartment",
			"Luxury A Casa Simpatia",
			"Residenza Rondanini Flat 2",
			"La piccola terrazza in Vaticano",
			"Trastevere Premium Apartment",
			"Giardino delle Orchidee",
			"Ca Simpatia",
			"The Palatine Garden Apartment",
			"Maisonette Campo de Fiori",
			"Hotel Infinito",
			"Hotel Mirage",
			"Volturno Guest House",
			"The Palatine Garden Apartment",
			"Ostiense with Elegance",
			"St. Peter Apartment",
			"Hotel la Perle Montparnasse",
			"Hôtel Vacances Bleues Provinces Opera",
			"Hôtel des Andelys",
			"Timhotel Paris Place D’Italie",
			"Hotel de la Paix Tour Eiffel",
			"Studio 52",
			"Apartamenty Roko",
			"Mermaid Station Apartments",
			"Hotel Arche Geo",
			"Sangate Hotel Airport",
			"Motel One Warsaw-Chopin",
			"Arche Hotel Poloneza"
		};

		private static Dictionary<int, string> valueMap = new Dictionary<int, string>()
		{
			{1,"10100"},{2,"10110"},{3,"10120"},{4,"10130"},{5,"10140"},{6,"10150"},
			{19,"100801"},{20,"101901"},{29,"101801"},{30,"102901"},{39,"102801"},
			{40,"103901"},{49,"103801"},{50,"104901"},{59,"104801"},{60,"105901"},
			{69,"105801"},{70,"106901"},{79,"106801"},{80,"107901"},{89,"107801"},
			{90,"108901"},{99,"108801"},{100,"1099100"},{109,"1098100"},{110,"1009100"},
			{119,"1008100"},{120,"1019100"},{129,"1018100"},{130,"1029100"},{139,"1028100"},
			{140,"1039100"},{149,"1038100"},{150,"1049100"},{159,"1048100"},{160,"1059100"},
			{169,"1058100"},{170,"1069100"},{179,"1068100"},{180,"1079100"},{189,"1078100"},
			{190,"1089100"},{199,"1088100"},{200,"1099110"}
		};
	}
}
