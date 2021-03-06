﻿//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Algorithms;
using BigTask2.Api;
using BigTask2.Problems;
using System.Collections.Generic;

namespace BigTask2.ProblemSolvers
{
    class DijkstraSolver : IGraphSolver
    {
        public IEnumerable<Route> TrySolve(CostProblem problem)
        {
            return new DijkstraCost().Solve(
                problem.Graph,
                problem.Graph.GetByName(problem.From),
                problem.Graph.GetByName(problem.To)
            );
        }

        public IEnumerable<Route> TrySolve(TimeProblem problem)
        {
            return new DijkstraTime().Solve(
                problem.Graph, 
                problem.Graph.GetByName(problem.From), 
                problem.Graph.GetByName(problem.To)
            );
        }
    }
}
