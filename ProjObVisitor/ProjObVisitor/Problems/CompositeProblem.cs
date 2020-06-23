using System;
using System.Collections.Generic;
using System.Linq;
using ResultsCombiners;
using Solvers;

namespace Problems
{
    class CompositeProblem : Problem
    {
        private readonly IEnumerable<Problem> problems;
        private readonly IResultsCombiner resultsCombiner;

        public CompositeProblem(string name, IEnumerable<Problem> problems,
            IResultsCombiner resultsCombiner) : base(name, () => 0)
        {
            this.problems = problems;
            this.resultsCombiner = resultsCombiner;
        }

        public override void Accept(ISolver solver)
        {
            if (Solved) return;
            bool anyUnsolved = false;
            List<int> results = new List<int>();
            foreach(Problem p in problems)
            {
                p.Accept(solver);
                if (p.Solved)
                {
                    results.Add(p.Result.Value);
                }
                else
                {
                    anyUnsolved = true;
                }
            }

            if (!anyUnsolved)
            {
                TryMarkAsSolved(resultsCombiner.CombineResults(results));
            }
        }
    }
}