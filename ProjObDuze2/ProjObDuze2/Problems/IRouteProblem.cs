//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
//This file Can be modified

using BigTask2.Api;
using BigTask2.Data;
using BigTask2.ProblemSolvers;
using System.Collections.Generic;

namespace BigTask2.Interfaces
{
    interface IRouteProblem
	{
        IGraphDatabase Graph { get; set; }
        IEnumerable<Route> TrySolveWith(IGraphSolver solver);
	}
}
