using System;
using Problems;

namespace Solvers
{
    class CPU : ISolver
    {
        private readonly string model;
        private readonly int threads;

        public CPU(string model, int threads)
        {
            this.model = model;
            this.threads = threads;
        }

        public virtual int? TrySolve(CPUProblem p)
        {
            if(threads >= p.RequiredThreads)
            {
                Console.WriteLine("Problem {0} - solved with CPU {1}", p.Name, model);
                return p.Computation();
            }
            Console.WriteLine("Problem {0} - failed to solve with CPU {1}", p.Name, model);
            return null;
        }

        public virtual int? TrySolve(GPUProblem p)
        {
            Console.WriteLine("Problem {0} - failed to solve with CPU {1}", p.Name, model);
            return null;
        }

        public virtual int? TrySolve(NetworkProblem p)
        {
            Console.WriteLine("Problem {0} - failed to solve with CPU {1}", p.Name, model);
            return null;
        }
    }
}