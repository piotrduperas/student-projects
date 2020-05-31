//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgencies.DataAccess;

namespace TravelAgencies.Init
{
    static class OysterGenerator
    {
        public static OysterDatabase GenerateOysterDataBase(Random R, int dbSize, double rightChance)
        {
            var result = new OysterDatabase();
            result.Reviews = CreateNode(R);
            for (int i = 1; i < dbSize; i++)
            {
                var node = CreateNode(R);
                var prev = result.Reviews;
                var current = result.Reviews;
                bool goLeft = false;
                while(current != null)
                {
                    prev = current;
                    goLeft = R.NextDouble() > rightChance;
                    if (goLeft) current = prev.Left;
                    else current = prev.Right;
                }
                if (goLeft) prev.Left = node;
                else prev.Right = node;
            }

            return result;
        }

        static BSTNode CreateNode(Random R)
        {
            return new BSTNode()
            {
                UserName = $"{DBGeneratorUtils.AnyFromArray(R, usernamePrefix)}{DBGeneratorUtils.AnyFromArray(R, usernameBase)}{DBGeneratorUtils.AnyFromArray(R, usernameSuffix)}",
                Review = DBGeneratorUtils.AnyFromArray(R, reviews)
            };
        }

        static string[] usernamePrefix =
        {
            "Super",
            "SAD",
            "Jeep",
            "PragueFan ",
            "Awesome",
            "rich",
            "New",
            "The",
            "Big",
        };
        static string[] usernameBase =
        {
            "sQuIrReL",
            "sailor",
            "John",
            "tourist",
            "Katy",
            "BUCKET",
            "Beerdrinker",
            "Ben"
        };
        static string[] usernameSuffix =
        {
            " the cat-lover",
            "_DJ",
            "257",
            " not found",
            " of thunder",
            "1",
            "PL"
                
        };

        static string[] reviews =
        {
            "The main challenge is probably to get there as the trafic is rather complicated, and the parking, impossible. But it is worth it",
            "Once you can fight your way through the bus loads of tourist who cannot read the keep off the grass signs , or think it does not apply to them",
            "I belive there is no photo that shows the beauty of the complex.",
            "No word, no photo will never express the real beauty.",
            "Very pretty, totally worth going",
            "From the train station is very easy to find the tower. You won’t miss it.",
            "Before climbing we had a great time posing in photos from afar",
            "Amazing historic landmark to be seen, once in a lifetime, especially during a day with dry weather.",
            "Once you get to the top the views are amazing - we were there at sunset and it was lovely.",
            "Nice historical site with decent restaurants food and souvenirs vendors.",
            "Coming there via train is better based on my first visit.",
            "It is a hassle and time consuming to get a replacement for another rental.",
            "NOT A GREAT PLACE TO VISIT",
            "HIGHLY OVER RATED",
            "Though we had visited in elate evening, the actual effect was not appreciable.",
            "Can definitely omit it",
            "Not worth longer than an hours visit, if you're passing that way!",
            "Good to see smaller than you think",
            "Great site, just make a plan and do some research before your visit.",
            "I came here after making last minute plans and didn't do any research",
            "I wasn't aware that you can buy tickets to climb to the top.",
            "Interesting to see",
            "Beautiful structure.",
            "Must see bucket list item; but don't dedicate hours to this stop",
            "Together with the rest of the duomo and square it's quite stunning.",
            "GREAT EXPERIENCE",
            "Our Guide SAM was very good, we enjoyed the tour very much! ",
            "Very good guide , explained everything about the history of the place,we highly recommend this tour.",
            "Our guide was very friendly and made the tour really interesting with informative bits of lighthearted information. ",
            "Would recommend to anyone",
        };
    }
}
