using System;
using Solvers;

namespace Problems
{
    class NetworkProblem : Problem
    {
        public int DataToTransfer { get; }

        public NetworkProblem(string name, Func<int> computation, int dataToTransfer) : base(name, computation)
        {
            DataToTransfer = dataToTransfer;
        }
        public override void Accept(ISolver solver)
        {
            if (Solved) return;
            TryMarkAsSolved(solver.TrySolve(this));
        }
    }
}