//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using BigTask2.Data;
using BigTask2.Interfaces;

namespace BigTask2.RequestHandlers
{
    class UseFilter : AbstractRequestHandler
    {
        private ICityFilter filter;
        public UseFilter(ICityFilter filter)
        {
            this.filter = filter;
        }
        protected override IRouteProblem HandleProblem(Request req, IRouteProblem problem)
        {
            if (req.Filter == null) return problem;

            problem.Graph = new FilteredDatabase(problem.Graph, filter.GetFilterCondition(req.Filter));
            return problem;
        }
    }

    interface ICityFilter
    {
        ICityFilterCondition GetFilterCondition(Filter filter);
    }

    class PopulationFilter : ICityFilter
    {
        public ICityFilterCondition GetFilterCondition(Filter filter)
        {
            return new PopulationCondition(filter.MinPopulation);
        }

        class PopulationCondition : ICityFilterCondition
        {
            private int population;

            public PopulationCondition(int population)
            {
                this.population = population;
            }

            public bool IsTrueFor(City city)
            {
                return city.Population >= population;
            }
        }
    }

    class RestaurantFilter : ICityFilter
    {
        public ICityFilterCondition GetFilterCondition(Filter filter)
        {
            return new RestaurantCondition(filter.RestaurantRequired);
        }

        class RestaurantCondition : ICityFilterCondition
        {
            private bool required;

            public RestaurantCondition(bool required)
            {
                this.required = required;
            }
            public bool IsTrueFor(City city)
            {
                return !required || city.HasRestaurant;
            }
        }
    }
}
