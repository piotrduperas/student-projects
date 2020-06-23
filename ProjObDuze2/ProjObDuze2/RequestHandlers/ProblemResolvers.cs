//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using BigTask2.Interfaces;
using BigTask2.Problems;
using System.Collections.Generic;

namespace BigTask2.RequestHandlers
{
    class ResolverHandler : AbstractRequestHandler
    {
        private IProblemResolver resolver;
        public ResolverHandler(IProblemResolver resolver)
        {
            this.resolver = resolver;
        }
        protected override IRouteProblem HandleProblem(Request req, IRouteProblem problem = null)
        {
            return problem ?? resolver.Resolve(req);
        }
    }

    interface IProblemResolver
    {
        IRouteProblem Resolve(Request req);
    }

    class CostResolver : IProblemResolver
    {
        public IRouteProblem Resolve(Request req)
        {
            if (req.Problem == "Cost")
            {
                return new CostProblem(req.From, req.To);
            }
            return null;
        }
    }
    class TimeResolver : IProblemResolver
    {
        public IRouteProblem Resolve(Request req)
        {
            if (req.Problem == "Time")
            {
                return new TimeProblem(req.From, req.To);
            }
            return null;
        }
    }

    class CatchOtherProblem : AbstractRequestHandler
    {
        public override IEnumerable<Route> Handle(Request req, IRouteProblem problem)
        {
            if (problem == null) return null;
            return Next?.Handle(req, problem);
        }
    }
}
