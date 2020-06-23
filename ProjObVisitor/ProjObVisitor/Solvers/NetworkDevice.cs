using System;
using Problems;

namespace Solvers
{
    abstract class NetworkDevice : ISolver
    {
        protected string DeviceType { get; set; } = "NetworkDevice";

        protected readonly string model;
        private int dataLimit;

        protected NetworkDevice(string model, int dataLimit)
        {
            this.model = model;
            this.dataLimit = dataLimit;
        }

        public virtual int? TrySolve(CPUProblem p)
        {
            Console.WriteLine("Problem {0} - failed to solve with {1} {2}", p.Name, DeviceType, model);
            return null;
        }

        public virtual int? TrySolve(GPUProblem p)
        {
            Console.WriteLine("Problem {0} - failed to solve with {1} {2}", p.Name, DeviceType, model);
            return null;
        }

        public virtual int? TrySolve(NetworkProblem p)
        {
            if(p.DataToTransfer <= dataLimit)
            {
                dataLimit -= p.DataToTransfer;
                Console.WriteLine("Problem {0} - solved with {1} {2}", p.Name, DeviceType, model);
                return p.Computation();
            }
            Console.WriteLine("Problem {0} - failed to solve with {1} {2}", p.Name, DeviceType, model);
            return null;
        }
    }
}