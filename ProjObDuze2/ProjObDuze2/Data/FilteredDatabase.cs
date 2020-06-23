//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using System.Collections.Generic;

namespace BigTask2.Data
{
    class FilteredDatabase : IGraphDatabase
    {
        private IGraphDatabase database;
        private ICityFilterCondition condition;

        public FilteredDatabase(IGraphDatabase database, ICityFilterCondition condition)
        {
            this.database = database;
            this.condition = condition;
        }

        public City GetByName(string cityName)
        {
            City found = database.GetByName(cityName);

            if (condition.IsTrueFor(found)) return found;
            return null;
        }

        public IIterator<Route> GetRoutesFrom(City from)
        {
            if (condition.IsTrueFor(from))
                return new FilteredRouteIterator(database.GetRoutesFrom(from), condition);
            else
                return new ListIterator<Route>(new List<Route>());
        }
    }

    class FilteredRouteIterator : IIterator<Route>
    {
        private IIterator<Route> iterator;
        private ICityFilterCondition condition;

        public FilteredRouteIterator(IIterator<Route> iterator, ICityFilterCondition condition)
        {
            this.iterator = iterator;
            this.condition = condition;
        }

        public Route Current()
        {
            return iterator.Current();
        }

        public bool MoveNext()
        {
            do
            {
                if (!iterator.MoveNext()) return false;
            } while (!condition.IsTrueFor(iterator.Current().To));
            return true;
        }
    }

    interface ICityFilterCondition
    {
        bool IsTrueFor(City city);
    }
}
