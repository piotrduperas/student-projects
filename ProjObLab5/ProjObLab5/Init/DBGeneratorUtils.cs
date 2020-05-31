//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgencies.Init
{
    static class DBGeneratorUtils
    {
        public static T AnyFromArray<T>(Random R, T[] array) => array[R.Next(array.Length)];
    }
}
