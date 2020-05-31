//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas

namespace TravelAgencies.Agencies
{
    interface IAdvertisingAgency
    {
        IOffer PrepareOffer(ITravelAgency travelAgency);
        IOffer PrepareTemporaryOffer(ITravelAgency travelAgency);
    }

    abstract class AbstractAgency : IAdvertisingAgency
    {
        private int temporaryDays;
        public AbstractAgency(int days)
        {
            temporaryDays = days;
        }
        public abstract IOffer PrepareOffer(ITravelAgency travelAgency);

        public IOffer PrepareTemporaryOffer(ITravelAgency travelAgency)
        {
            return new TemporaryOffer(PrepareOffer(travelAgency), temporaryDays);
        }
    }

    class GraphicsAgency : AbstractAgency
    {
        private int photosAmount;
        public GraphicsAgency(int days, int photosAmount) : base(days)
        {
            this.photosAmount = photosAmount;
        }

        public override IOffer PrepareOffer(ITravelAgency travelAgency)
        {
            IPhoto[] photos = new IPhoto[photosAmount];

            for(int i = 0; i < photosAmount; i++)
            {
                photos[i] = travelAgency.CreatePhoto();
            }

            return new GraphicsOffer(travelAgency.CreateTrip(), photos);
        }
    }

    class TextAgency : AbstractAgency
    {
        private int reviewsAmount;
        public TextAgency(int days, int reviewsAmount) : base(days)
        {
            this.reviewsAmount = reviewsAmount;
        }

        public override IOffer PrepareOffer(ITravelAgency travelAgency)
        {
            IReview[] reviews = new IReview[reviewsAmount];

            for (int i = 0; i < reviewsAmount; i++)
            {
                reviews[i] = travelAgency.CreateReview();
            }

            return new TextOffer(travelAgency.CreateTrip(), reviews);
        }
    }
}
