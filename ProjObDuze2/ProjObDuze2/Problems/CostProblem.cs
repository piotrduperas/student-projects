//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
//This file Can be modified

using BigTask2.Api;
using BigTask2.Data;
using BigTask2.Interfaces;
using BigTask2.ProblemSolvers;
using System.Collections.Generic;

namespace BigTask2.Problems
{
	class CostProblem : IRouteProblem
	{
		public string From, To;
		public CostProblem(string from, string to)
		{
			From = from;
			To = to;
		}

        public IGraphDatabase Graph { get; set; }

		public IEnumerable<Route> TrySolveWith(IGraphSolver solver)
		{
			return solver.TrySolve(this);
		}
	}
}
