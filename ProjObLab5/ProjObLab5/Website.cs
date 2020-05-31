//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using System.Collections.Generic;

namespace TravelAgencies.Agencies
{
    interface IWebsite
    {
        void Present();
        void AddOffer(IOffer offer);
    }

    class OfferWebsite : IWebsite
    {
        private List<IOffer> offers = new List<IOffer>();

        public void AddOffer(IOffer offer)
        {
            offers.Add(offer);
        }

        public void Present()
        {
            foreach (IOffer offer in offers)
            {
                offer.Display();
                Console.WriteLine("");
            }
        }
    }
}
