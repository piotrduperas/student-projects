//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using BigTask2.Problems;
using System.Collections.Generic;

namespace BigTask2.ProblemSolvers
{
    interface IGraphSolver
    {
        IEnumerable<Route> TrySolve(CostProblem problem);
        IEnumerable<Route> TrySolve(TimeProblem problem);
    }
}
