using Problems;

namespace Solvers
{
    interface ISolver
    {
        public int? TrySolve(CPUProblem p);
        public int? TrySolve(GPUProblem p);
        public int? TrySolve(NetworkProblem p);
    }
}