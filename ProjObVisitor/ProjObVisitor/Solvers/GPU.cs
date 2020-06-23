using System;
using Problems;

namespace Solvers
{
    class GPU : ISolver
    {
        static private int MaxTemperature { get; } = 100;
        static private int CPUProblemTemperatureMultiplier { get; } = 3;

        private readonly string model;
        private int temperature;
        private int coolingFactor;

        public GPU(string model, int temperature, int coolingFactor)
        {
            this.model = model;
            this.temperature = temperature;
            this.coolingFactor = coolingFactor;
        }
        private bool DidThermalThrottle()
        {
            if (temperature > MaxTemperature)
            {
                Console.WriteLine($"GPU {model} thermal throttled");
                CoolDown();
                return true;
            }

            return false;
        }

        private void CoolDown()
        {
            temperature -= coolingFactor;
        }

        public virtual int? TrySolve(CPUProblem p)
        {
            if (temperature <= MaxTemperature)
            {
                temperature += p.RequiredThreads * CPUProblemTemperatureMultiplier;
                DidThermalThrottle();
                Console.WriteLine("Problem {0} - solved with GPU {1}", p.Name, model);
                return p.Computation();
            }
            Console.WriteLine("Problem {0} - failed to solve with GPU {1}", p.Name, model);
            return null;
        }

        public virtual int? TrySolve(GPUProblem p)
        {
            if(temperature <= MaxTemperature)
            {
                temperature += p.GpuTemperatureIncrease;
                DidThermalThrottle();
                Console.WriteLine("Problem {0} - solved with GPU {1}", p.Name, model);
                return p.Computation();
            }
            Console.WriteLine("Problem {0} - failed to solve with GPU {1}", p.Name, model);
            return null;
        }

        public virtual int? TrySolve(NetworkProblem p)
        {
            Console.WriteLine("Problem {0} - failed to solve with GPU {1}", p.Name, model);
            return null;
        }
    }
}