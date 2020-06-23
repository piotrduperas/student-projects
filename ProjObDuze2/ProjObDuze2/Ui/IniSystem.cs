//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using System;
using System.Collections.Generic;

namespace BigTask2.Ui
{
    class IniSystem : ISystem
    {
        public IForm Form { get; } = new IniForm();

        public IDisplay Display { get; } = new IniDisplay();
    }

    class IniSystemFactory : ISystemFactory
    {
        public ISystem CreateSystem()
        {
            return new IniSystem();
        }
    }

    class IniForm : DictionaryForm
    {
        public override void Insert(string command)
        {
            string[] split = command.Split('=');
            if (split.Length != 2) throw new ArgumentException();

            string key = split[0].Trim();

            if (entries.ContainsKey(key))
            {
                entries.Clear();
            }

            entries.Add(key, split[1].Trim());
        }
    }

    class IniDisplay : IDisplay
    {
        public void Print(IEnumerable<Route> routes)
        {
            if (routes == null)
            {
                Console.WriteLine("=");
                return;
            }

            bool isFirst = true;
            double totalTime = 0;
            double totalCost = 0;

            foreach (Route route in routes)
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

            Console.WriteLine($"totalTime={totalTime:0.##}");
            Console.WriteLine($"totalCost={totalCost:0.##}\n");
        }

        private void PrintCity(City city)
        {
            Console.WriteLine($"=City=");
            Console.WriteLine($"Name={city.Name}");
            Console.WriteLine($"Population={city.Population}");
            Console.WriteLine($"HasRestaurant={city.HasRestaurant}\n");
        }

        private void PrintRoute(Route route)
        {
            Console.WriteLine($"=Route=");
            Console.WriteLine($"Vehicle={route.VehicleType}");
            Console.WriteLine($"Cost={route.Cost:0.##}");
            Console.WriteLine($"TravelTime={route.TravelTime:0.##}\n");
        }
    }
}
