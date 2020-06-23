using System;
using Problems;

namespace Solvers
{
    class Ethernet : NetworkDevice
    {
        public Ethernet(string model, int dataLimit) : base(model, dataLimit)
        {
            DeviceType = "Ethernet";
        }
    }
}