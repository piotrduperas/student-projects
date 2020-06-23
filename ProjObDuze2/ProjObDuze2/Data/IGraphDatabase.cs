//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
//This file contains fragments that You have to fulfill

using BigTask2.Api;
namespace BigTask2.Data
{
    public interface IGraphDatabase
    {
        IIterator<Route> GetRoutesFrom(City from);
        City GetByName(string cityName);
    }
}
