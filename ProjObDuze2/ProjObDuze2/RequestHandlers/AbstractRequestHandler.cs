//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using BigTask2.Interfaces;
using System.Collections.Generic;

namespace BigTask2.RequestHandlers
{
    abstract class AbstractRequestHandler : IRequestHandler
    {
        public IRequestHandler Next { get; private set; }

        protected virtual IRouteProblem HandleProblem(Request req, IRouteProblem problem)
        {
            return problem;
        }

        public virtual IEnumerable<Route> Handle(Request req, IRouteProblem problem)
        {
            return Next?.Handle(req, HandleProblem(req, problem));
        }

        public IRequestHandler Then(IRequestHandler handler)
        {
            return Next = handler;
        }
    }
}
