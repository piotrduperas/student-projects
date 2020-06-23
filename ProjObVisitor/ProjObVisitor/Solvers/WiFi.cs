using System;
using Problems;

namespace Solvers
{
    class WiFi : NetworkDevice
    {
        private readonly double packetLossChance;
        private static readonly Random rng = new Random(1597);

        public WiFi(string model, int dataLimit, double packetLossChance) : base(model, dataLimit)
        {
            DeviceType = "WiFi";
            this.packetLossChance = packetLossChance;
        }
        public override int? TrySolve(NetworkProblem p)
        {
            if (rng.NextDouble() < packetLossChance)
            {
                Console.WriteLine("Problem {0} - failed to solve with {1} {2}", p.Name, DeviceType, model);
                return null;
            }
            int? res = base.TrySolve(p);
            return res;
        }
    }
}