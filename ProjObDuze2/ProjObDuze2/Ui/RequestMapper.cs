//This file should not be modified

using BigTask2.Api;
using System.Collections.Generic;

namespace BigTask2.Ui
{
    static class RequestMapper
    {
        public static Request Map(IForm form)
        {
            return new Request
            {
                Solver = form.GetTextValue("solver"),
                From = form.GetTextValue("from"),
                To = form.GetTextValue("to"),
                Problem = form.GetTextValue("problem"),
                Filter = new Filter
                {
                    MinPopulation = form.GetNumericValue("minPopulation"),
                    RestaurantRequired = form.GetBoolValue("restaurantRequired"),
                    AllowedVehicles = GetVehicles(form)
                },
            };
        }

        private static ISet<VehicleType> GetVehicles(IForm form)
        {
            ISet<VehicleType> vehicles = new HashSet<VehicleType>();

            if (form.GetBoolValue("train")) vehicles.Add(VehicleType.Train);
            if (form.GetBoolValue("car")) vehicles.Add(VehicleType.Car);

            return vehicles;
        }
    }
}
