//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using BigTask2.Interfaces;
using System.Collections.Generic;

namespace BigTask2.RequestHandlers
{
    interface IRequestHandler
    {
        IRequestHandler Next { get; }
        IRequestHandler Then(IRequestHandler handler);
        IEnumerable<Route> Handle(Request req, IRouteProblem problem = null);
    }
}
