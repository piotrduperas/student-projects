//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using BigTask2.Interfaces;
using BigTask2.ProblemSolvers;
using System.Collections.Generic;

namespace BigTask2.RequestHandlers
{
    class UseSolver : AbstractRequestHandler
    {
        private IGraphSolver graphSolver;
        private string solver;

        public UseSolver(IGraphSolver graphSolver, string solver)
        {
            this.graphSolver = graphSolver;
            this.solver = solver;
        }

        public override IEnumerable<Route> Handle(Request req, IRouteProblem problem)
        {
            if (req.Solver == solver)
            {
                IEnumerable<Route> result = problem.TrySolveWith(graphSolver);
                if (result != null) return result;
            }
            return Next?.Handle(req, problem);
        }
    }
}
