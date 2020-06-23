//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using BigTask2.Interfaces;
using System.Collections.Generic;

namespace BigTask2.RequestHandlers
{
    class RequestValidator : AbstractRequestHandler
    {
        public override IEnumerable<Route> Handle(Request req, IRouteProblem problem)
        {
            if (req == null) return null;

            string[] values = new string[] { req.From, req.To, req.Solver, req.Problem };
            foreach(string s in values)
            {
                if (s == null || s == "") return null;
            }

            return Next?.Handle(req, HandleProblem(req, problem));
        }
    }
}
