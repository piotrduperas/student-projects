//This file should not be modified

using System.Collections.Generic;

namespace BigTask2.Api
{
    class Filter
    {
        public int MinPopulation { get; set; }
        public bool RestaurantRequired { get; set; }
        public ISet<VehicleType> AllowedVehicles { get; set; }
    }
}
