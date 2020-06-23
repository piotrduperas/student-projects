//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;

namespace BigTask2.Data
{
    class MergedDatabase : IGraphDatabase
    {
        private IGraphDatabase[] databases;
        public MergedDatabase(params IGraphDatabase[] databases)
        {
            this.databases = databases;
        }

        public IIterator<Route> GetRoutesFrom(City from)
        {
            IIterator<Route>[] iterators = new IIterator<Route>[databases.Length];
            for(int i = 0; i < databases.Length; i++)
            {
                iterators[i] = databases[i].GetRoutesFrom(from);
            }
            return new MergedIterator<Route>(iterators);
        }

        public City GetByName(string cityName)
        {
            foreach (IGraphDatabase db in databases)
            {
                City found = db.GetByName(cityName);
                if (found != null) return found;
            }
            return null;
        }
    }
}
