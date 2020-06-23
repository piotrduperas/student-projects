//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using System;
using System.Collections.Generic;

namespace BigTask2.Ui
{
    class XmlSystem : ISystem
    {
        public IForm Form { get; } = new XmlForm();

        public IDisplay Display { get; } = new XmlDisplay();
    }

    class XmlSystemFactory : ISystemFactory
    {
        public ISystem CreateSystem()
        {
            return new XmlSystem();
        }
    }

    class XmlForm : DictionaryForm
    {
        public override void Insert(string command)
        {
            string[] split = command.Split('<', '>', '/');
            if (split.Length != 6) throw new ArgumentException();

            string key = split[1].Trim();
            if(key != split[4].Trim()) throw new ArgumentException();

            if (entries.ContainsKey(key))
            {
                entries.Clear();
            }

            entries.Add(key, split[2].Trim());
        }
    }

    class XmlDisplay : IDisplay
    {
        public void Print(IEnumerable<Route> routes)
        {
            if (routes == null)
            {
                Console.WriteLine("<>");
                return;
            }

            bool isFirst = true;
            double totalTime = 0;
            double totalCost = 0;

            foreach(Route route in routes)
            {
                if (isFirst)
                {
                    PrintCity(route.From);
                    isFirst = false;
                }
                PrintRoute(route);
                PrintCity(route.To);

                totalCost += route.Cost;
                totalTime += route.TravelTime;
            }

            Console.WriteLine($"<totalTime>{totalTime:0.##}</totalTime>");
            Console.WriteLine($"<totalCost>{totalCost:0.##}</totalCost>\n");
        }

        private void PrintCity(City city)
        {
            Console.WriteLine($"<City/>");
            Console.WriteLine($"<Name>{city.Name}</Name>");
            Console.WriteLine($"<Population>{city.Population}</Population>");
            Console.WriteLine($"<HasRestaurant>{city.HasRestaurant}</HasRestaurant>\n");
        }

        private void PrintRoute(Route route)
        {
            Console.WriteLine($"<Route/>");
            Console.WriteLine($"<Vehicle>{route.VehicleType}</Vehicle>");
            Console.WriteLine($"<Cost>{route.Cost:0.##}</Cost>");
            Console.WriteLine($"<TravelTime>{route.TravelTime:0.##}</TravelTime>\n");
        }
    }
}
