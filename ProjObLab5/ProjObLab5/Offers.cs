//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using System.Text;

namespace TravelAgencies.Agencies
{
    interface IOffer
    {
        ITrip Trip { get; }
        void Display();
    }

    abstract class AbstractOffer : IOffer
    {
        public ITrip Trip { get; }
        public AbstractOffer(ITrip trip)
        {
            Trip = trip;
        }

        public abstract void Display();

        protected string TripDescription()
        {
            StringBuilder sb = new StringBuilder();

            var (rating, price) = GetRatingAndPrice();
            sb.Append($"\nRating: {rating}\nPrice: {price}\n\n");

            for (int i = 0; i < Trip.Days.Length; i++)
            {
                sb.Append($"Day {i + 1} in {Trip.Country}!\n");
                sb.Append($"Accomodation: {Trip.Days[i].Room.Name}\n");
                sb.Append($"Attractions:\n");

                for (int j = 0; j < Trip.Days[i].Attractions.Length; j++)
                {
                    sb.Append($"\t{Trip.Days[i].Attractions[j].Name}\n");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        protected (double rating, double price) GetRatingAndPrice()
        {
            double ratingSum = 0;
            int ratingCount = 0;
            double price = 0;

            for (int i = 0; i < Trip.Days.Length; i++)
            {
                TripDay td = Trip.Days[i];
                price += td.Room.Price;
                ratingSum += td.Room.Rating;
                ratingCount++;

                for (int j = 0; j < Trip.Days[i].Attractions.Length; j++)
                {
                    price += td.Attractions[j].Price;
                    ratingSum += td.Attractions[j].Rating;
                    ratingCount++;
                }
            }
            return (ratingSum / ratingCount, price);
        }
    }

    class GraphicsOffer : AbstractOffer
    {
        private IPhoto[] Photos { get; }

        public GraphicsOffer(ITrip trip, IPhoto[] photos) : base(trip)
        {
            Photos = photos;
        }
        protected string PhotosDescription()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Photos.Length; i++)
            {
                sb.Append($"{Photos[i].Name} ({Photos[i].WidthPx}x{Photos[i].HeightPx})\n");
            }
            return sb.ToString();
        }
        public override void Display()
        {
            Console.WriteLine(TripDescription() + "\n" + PhotosDescription() + "\n");
        }
    }

    class TextOffer : AbstractOffer
    {
        private IReview[] Reviews { get; }

        public TextOffer(ITrip trip, IReview[] reviews) : base(trip)
        {
            Reviews = reviews;
        }
        protected string ReviewsDescription()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Reviews.Length; i++)
            {
                sb.Append($"Review by {Reviews[i].UserName}:\n\t {Reviews[i].Content}\n");
            }
            return sb.ToString();
        }
        public override void Display()
        {
            Console.WriteLine(TripDescription() + "\n" + ReviewsDescription() + "\n");
        }
    }

    class TemporaryOffer : IOffer
    {
        IOffer offer;
        int days;

        public TemporaryOffer(IOffer offer, int days)
        {
            this.offer = offer;
            this.days = days;
        }

        public ITrip Trip => offer.Trip;

        public void Display()
        {
            if (days-- > 0) offer.Display();
            else Console.WriteLine("The offer has expired\n");
        }
    }
}
