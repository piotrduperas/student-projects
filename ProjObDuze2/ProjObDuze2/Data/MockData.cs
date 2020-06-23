//This file should not be modified

using BigTask2.Api;
using System.Collections.Generic;

namespace BigTask2.Data
{
	static class MockData
	{
		private static readonly City Wyzima = new City { Name = "Wyzima", HasRestaurant = true, Population = 325_000 };
		private static readonly City Novigrad = new City { Name = "Novigrad", HasRestaurant = false, Population = 800_000 };
		private static readonly City Gotham = new City { Name = "Gotham", HasRestaurant = true, Population = 4_500_000 };
		private static readonly City Zootopia = new City { Name = "Zootopia", HasRestaurant = false, Population = 700_000 };
		private static readonly City Stormwind = new City { Name = "Stormwind", HasRestaurant = true, Population = 200_000 };
		private static readonly City Orgrimar = new City { Name = "Orgrimar", HasRestaurant = false, Population = 300_000 };
		private static readonly City Dalaran = new City { Name = "Dalaran", HasRestaurant = true, Population = 260_000 };
		private static readonly City Osgiliath = new City { Name = "Osgiliath", HasRestaurant = false, Population = 10_000 };
		private static readonly City Grimford = new City { Name = "Grimford", HasRestaurant = true, Population = 50_000 };
		private static readonly City Kingslanding = new City { Name = "Kingslanding", HasRestaurant = true, Population = 1_500_000 };
		private static readonly City Winterfell = new City { Name = "Winterfell", HasRestaurant = false, Population = 360_000 };
		private static readonly City Neverwinter = new City { Name = "Neverwinter", HasRestaurant = true, Population = 20_000 };
        private static readonly City Braavos = new City { Name = "Braavos", HasRestaurant = false, Population = 220_000 };
        private static readonly City Myr = new City { Name = "Myr", HasRestaurant = true, Population = 480_000 };
        private static readonly City Volantis = new City { Name = "Volantis", HasRestaurant = true, Population = 105_000 };
        private static readonly City Megalopolis = new City { Name = "Megalopolis", HasRestaurant = false, Population = 3_000_000 };

        private static readonly IEnumerable<Route> carRoutes = new List<Route>
		{
            ByCar(Wyzima, Zootopia, 585.79, 82.84),
            ByCar(Wyzima, Orgrimar, 789.61, 460.86),
            ByCar(Wyzima, Dalaran, 992.25, 102.47),
            ByCar(Wyzima, Kingslanding, 820.44, 199.37),
            ByCar(Wyzima, Winterfell, 699.21, 174.05),
            ByCar(Wyzima, Neverwinter, 801.61, 400.03),
            ByCar(Novigrad, Zootopia, 690.54, 457.55),
            ByCar(Novigrad, Orgrimar, 200.98, 33.65),
            ByCar(Novigrad, Kingslanding, 407.47, 326.46),
            ByCar(Novigrad, Myr, 284.93, 226.16),
            ByCar(Gotham, Wyzima, 795.41, 305.4),
            ByCar(Gotham, Novigrad, 790.43, 274.8),
            ByCar(Gotham, Osgiliath, 974.46, 264.54),
            ByCar(Gotham, Winterfell, 494.66, 368.25),
            ByCar(Zootopia, Novigrad, 558.38, 284.49),
            ByCar(Zootopia, Gotham, 66.79, 65.25),
            ByCar(Zootopia, Stormwind, 170.12, 409.99),
            ByCar(Zootopia, Orgrimar, 952.67, 416.76),
            ByCar(Zootopia, Grimford, 606.39, 440.57),
            ByCar(Zootopia, Winterfell, 741.08, 77.78),
            ByCar(Zootopia, Braavos, 68.41, 176.21),
            ByCar(Stormwind, Wyzima, 484.07, 306.59),
            ByCar(Stormwind, Gotham, 711.4, 421.34),
            ByCar(Stormwind, Zootopia, 194.53, 381.36),
            ByCar(Stormwind, Dalaran, 159.09, 234.61),
            ByCar(Stormwind, Grimford, 30.24, 299.97),
            ByCar(Stormwind, Winterfell, 942.98, 273.06),
            ByCar(Stormwind, Myr, 305.28, 220.03),
            ByCar(Stormwind, Volantis, 636.55, 72.03),
            ByCar(Orgrimar, Novigrad, 813.11, 42.92),
            ByCar(Orgrimar, Zootopia, 841.26, 137.6),
            ByCar(Orgrimar, Osgiliath, 139.54, 429.42),
            ByCar(Orgrimar, Kingslanding, 639.49, 56.84),
            ByCar(Orgrimar, Winterfell, 52.25, 395.4),
            ByCar(Orgrimar, Neverwinter, 534.97, 464.47),
            ByCar(Orgrimar, Myr, 856.43, 475.13),
            ByCar(Dalaran, Zootopia, 148.13, 488.89),
            ByCar(Dalaran, Winterfell, 790.73, 81.16),
            ByCar(Osgiliath, Zootopia, 426.28, 396.11),
            ByCar(Osgiliath, Stormwind, 169.9, 358.88),
            ByCar(Osgiliath, Kingslanding, 196.85, 39.14),
            ByCar(Osgiliath, Myr, 914.34, 29.43),
            ByCar(Osgiliath, Volantis, 788.1, 164.15),
            ByCar(Grimford, Novigrad, 73.77, 424.12),
            ByCar(Grimford, Zootopia, 580.3, 13.85),
            ByCar(Grimford, Osgiliath, 550.21, 57.53),
            ByCar(Grimford, Winterfell, 278.34, 281.08),
            ByCar(Grimford, Neverwinter, 689.52, 3.51),
            ByCar(Grimford, Myr, 371.58, 383.67),
            ByCar(Grimford, Volantis, 756.09, 28.29),
            ByCar(Kingslanding, Wyzima, 761.84, 400.45),
            ByCar(Kingslanding, Gotham, 173.73, 248.37),
            ByCar(Kingslanding, Stormwind, 73.48, 94.54),
            ByCar(Kingslanding, Grimford, 222.07, 301.48),
            ByCar(Kingslanding, Winterfell, 223.51, 93.03),
            ByCar(Kingslanding, Neverwinter, 959.81, 261.47),
            ByCar(Kingslanding, Myr, 924.4, 160.7),
            ByCar(Kingslanding, Volantis, 709.56, 245.29),
            ByCar(Winterfell, Wyzima, 777.19, 292.24),
            ByCar(Winterfell, Novigrad, 698.64, 51.75),
            ByCar(Winterfell, Stormwind, 689.39, 199.15),
            ByCar(Winterfell, Orgrimar, 398.99, 490.15),
            ByCar(Winterfell, Dalaran, 614.61, 217.01),
            ByCar(Winterfell, Volantis, 982.41, 336.77),
            ByCar(Neverwinter, Novigrad, 146.15, 114.03),
            ByCar(Neverwinter, Zootopia, 201.35, 411.44),
            ByCar(Neverwinter, Stormwind, 338.83, 186.69),
            ByCar(Neverwinter, Orgrimar, 824.29, 438.95),
            ByCar(Neverwinter, Dalaran, 540.99, 42.89),
            ByCar(Neverwinter, Kingslanding, 108.54, 102.4),
            ByCar(Neverwinter, Myr, 43.72, 1.13),
            ByCar(Neverwinter, Volantis, 351.72, 154.96),
            ByCar(Braavos, Novigrad, 525.1, 461.56),
            ByCar(Braavos, Zootopia, 199.6, 92.53),
            ByCar(Braavos, Kingslanding, 919.87, 50.22),
            ByCar(Braavos, Neverwinter, 875.54, 457.54),
            ByCar(Braavos, Myr, 320.6, 344.38),
            ByCar(Myr, Gotham, 322.6, 416.43),
            ByCar(Myr, Stormwind, 886.55, 321.71),
            ByCar(Myr, Dalaran, 375.63, 258.7),
            ByCar(Myr, Osgiliath, 73.67, 94.8),
            ByCar(Myr, Kingslanding, 310.37, 397.11),
            ByCar(Myr, Braavos, 452.91, 357.97),
            ByCar(Myr, Volantis, 378.57, 194.07),
            ByCar(Myr, Megalopolis, 330.74, 125.82),
            ByCar(Volantis, Gotham, 100.4, 92.85),
            ByCar(Volantis, Zootopia, 412.9, 255.8),
            ByCar(Volantis, Neverwinter, 385.1, 194.06),
            ByCar(Volantis, Megalopolis, 934.85, 117.2),
            ByCar(Megalopolis, Wyzima, 700.32, 354.19),
            ByCar(Megalopolis, Zootopia, 621.35, 22.91),
            ByCar(Megalopolis, Winterfell, 297.45, 355.39),
            ByCar(Megalopolis, Volantis, 761.2, 357.18)
        };

		private static readonly IEnumerable<Route> trainRoutes = new List<Route>
		{
            ByTrain(Wyzima, Myr, 696.94, 61.61),
            ByTrain(Wyzima, Megalopolis, 119.43, 457.46),
            ByTrain(Novigrad, Grimford, 342.58, 16.06),
            ByTrain(Gotham, Wyzima, 953.01, 230.72),
            ByTrain(Gotham, Orgrimar, 50.49, 250.96),
            ByTrain(Gotham, Winterfell, 594.63, 441.48),
            ByTrain(Zootopia, Stormwind, 115.38, 14.08),
            ByTrain(Stormwind, Wyzima, 411.26, 173.28),
            ByTrain(Stormwind, Winterfell, 698.96, 402.59),
            ByTrain(Stormwind, Megalopolis, 922.94, 257.43),
            ByTrain(Orgrimar, Dalaran, 760.78, 359.35),
            ByTrain(Orgrimar, Myr, 222.84, 454.24),
            ByTrain(Orgrimar, Megalopolis, 876.8, 160.51),
            ByTrain(Dalaran, Wyzima, 923.54, 93.41),
            ByTrain(Dalaran, Stormwind, 361.1, 117.25),
            ByTrain(Dalaran, Grimford, 435.18, 438.33),
            ByTrain(Dalaran, Winterfell, 484.13, 385.96),
            ByTrain(Dalaran, Volantis, 362.06, 353.39),
            ByTrain(Dalaran, Megalopolis, 671.72, 482.04),
            ByTrain(Osgiliath, Wyzima, 152.48, 371.59),
            ByTrain(Osgiliath, Gotham, 353.47, 37.65),
            ByTrain(Osgiliath, Orgrimar, 898.06, 108.3),
            ByTrain(Osgiliath, Kingslanding, 547.8, 312.61),
            ByTrain(Grimford, Gotham, 842.93, 305.7),
            ByTrain(Grimford, Dalaran, 11.86, 467.38),
            ByTrain(Kingslanding, Zootopia, 595.85, 332.61),
            ByTrain(Kingslanding, Osgiliath, 539.45, 24.37),
            ByTrain(Kingslanding, Grimford, 704.93, 342.51),
            ByTrain(Kingslanding, Winterfell, 497.27, 338.11),
            ByTrain(Kingslanding, Neverwinter, 63.32, 167.99),
            ByTrain(Winterfell, Gotham, 987.89, 455.72),
            ByTrain(Neverwinter, Wyzima, 610.01, 462.38),
            ByTrain(Neverwinter, Novigrad, 680.08, 108.54),
            ByTrain(Neverwinter, Braavos, 72.13, 67.74),
            ByTrain(Neverwinter, Myr, 347.47, 283.14),
            ByTrain(Neverwinter, Megalopolis, 920.54, 79.99),
            ByTrain(Braavos, Dalaran, 426.79, 264.85),
            ByTrain(Braavos, Grimford, 390.35, 52.86),
            ByTrain(Braavos, Winterfell, 85.52, 462.74),
            ByTrain(Myr, Grimford, 149.28, 178.04),
            ByTrain(Myr, Kingslanding, 172.76, 335.51),
            ByTrain(Myr, Neverwinter, 312.19, 145.5),
            ByTrain(Volantis, Wyzima, 449.19, 93.33),
            ByTrain(Volantis, Stormwind, 506.93, 190.92),
            ByTrain(Volantis, Osgiliath, 470.97, 252.43),
            ByTrain(Megalopolis, Osgiliath, 323.43, 85.92),
            ByTrain(Megalopolis, Myr, 341.75, 190.27),
            ByTrain(Megalopolis, Volantis, 877.73, 414.02)
        };

		public static (IGraphDatabase car, IGraphDatabase trains) InitDatabases()
		{
			return (new AdjacencyListDatabase(carRoutes), new MatrixDatabase(trainRoutes));
		}

		private static Route ByCar(City from, City to, double cost, double travelTime)
		{
			return new Route { From = from, To = to, Cost = cost, TravelTime = travelTime, VehicleType = VehicleType.Car };
		}

		private static Route ByTrain(City from, City to, double cost, double travelTime)
		{
			return new Route { From = from, To = to, Cost = cost, TravelTime = travelTime, VehicleType = VehicleType.Train };
		}
	}
}
