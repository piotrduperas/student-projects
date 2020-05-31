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
	static class ShutterStockGenerator
	{
		public static ShutterStockDatabase GenerateShutterStockDataBase(Random R, int dbSize, int arrayMaxLength, double emptyChance)
		{
			var result = new ShutterStockDatabase();
			result.Photos = new PhotMetadata[R.Next(arrayMaxLength / 2, arrayMaxLength)][][];
			int toGenerateLeft = dbSize;
			for (int i = 0; i < result.Photos.Length; i++)
			{
				if (R.NextDouble() < emptyChance) continue;
				result.Photos[i] = new PhotMetadata[R.Next(arrayMaxLength)][];
				for (int j = result.Photos[i].Length; j-- > 0;)
				{
					if (R.NextDouble() < emptyChance) continue;
					result.Photos[i][j] = new PhotMetadata[Math.Min(R.Next(arrayMaxLength), toGenerateLeft)];
					for (int k = result.Photos[i][j].Length; k-- > 0;)
					{
						if (R.NextDouble() < emptyChance) continue;
						result.Photos[i][j][k] = GeneratePhotoData(R);
						if (--toGenerateLeft == 0)
							return result;
					}
				}
			}
			return result;
		}

		private static PhotMetadata GeneratePhotoData(Random R)
		{
			var result = new PhotMetadata()
			{
				Camera = DBGeneratorUtils.AnyFromArray(R, cameras),
				Name = DBGeneratorUtils.AnyFromArray(R, namePrefix) + DBGeneratorUtils.AnyFromArray(R, nameBase) + ".jpg",
				CameraSettings = GetCameraSettings(R),
				Date = new DateTime(2004, 1, 1).AddDays(R.Next(365 * 16)),
			};
			(result.Longitude, result.Latitude) = GetRandomLatLong(R);
            int pixelIdx = R.Next(pixelWidths.Length);
			result.WidthPx = pixelMap[pixelWidths[pixelIdx]];
			result.HeightPx = pixelMap[pixelHeights[pixelIdx]];
			return result;
		}
		private static (double, double) GetRandomLatLong(Random R)
		{
			var c = R.Next(0, 3);
			if (c == 0)
				return (R.NextDouble() * 9.1 + 14.4, R.NextDouble() * 4.4 + 49.8);
			if (c == 1)
				return (R.NextDouble() * 6.4 + 8.8, R.NextDouble() * 6.3 + 37.7);
			return (R.NextDouble() * 5.4, R.NextDouble() * 6.4 + 43.6);
		}

		private static double[] GetCameraSettings(Random R)
		{
			return new[] { R.Next(100, 3200), R.Next(10, 80) / 10.0, R.Next(1, 8) * 12, R.Next(1, 12) * 5 };
		}

		private static string[] cameras =
		{
			"Nikon",
			"Canon",
			"Panasonic Lumix",
			"Fujifilm",
			"Olympus",
			"Sony",
			"Instax"
		};

		private static int[] pixelWidths =
		{
			800,
			1024,
			1920,
			600,
			3840
		};

		private static int[] pixelHeights =
		{
			600,
			1280,
			1080,
			600,
			2160
		};

		private static string[] namePrefix =
		{
		   "big_",
		   "pic_",
			"hdd_",
			"all_",
			"attr_",
			"small_",
			"wide_",
			"final_",
			"test_",
			"last_",
			"final_final_",
			"dark_",
			"other side_",
			"front_",
			"back_"
		};

		private static string[] nameBase =
		{
			"view",
			"panoramic",
			"city",
			"beach",
			"tower",
			"plaza",
			"main",
			"hotel",
			"street",
			"obrazeczek",
			"car",
			"balcony",
			"window"
		};

		private static Dictionary<int, string> pixelMap = new Dictionary<int, string>()
		{
			{600,"40114"},
			{800,"42114"},
			{1024,"451186"},
			{1080,"451142"},
			{1280,"651142"},
			{1920,"351146"},
			{2160,"561140"},
			{3840,"271148"},
		};
	}
}
