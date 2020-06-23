//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using BigTask2.Data;
using BigTask2.Interfaces;
using System.Collections.Generic;

namespace BigTask2.RequestHandlers
{
    class DatabaseHandler : AbstractRequestHandler
    {
        private IGraphDatabase database;
        private VehicleType type;

        public DatabaseHandler(IGraphDatabase db, VehicleType type)
        {
            database = db;
            this.type = type;
        }
        protected override IRouteProblem HandleProblem(Request req, IRouteProblem problem)
        {
            if (req.Filter.AllowedVehicles.Contains(type))
            {
                if(problem.Graph == null)
                {
                    problem.Graph = database;
                }
                else
                {
                    problem.Graph = new MergedDatabase(problem.Graph, database);
                }
            }
            return problem;
        }
    }

    class NoDatabaseHandler : AbstractRequestHandler
    {
        public override IEnumerable<Route> Handle(Request req, IRouteProblem problem = null)
        {
            if (problem.Graph == null) return null;
            return Next?.Handle(req, problem);
        }
    }
}
