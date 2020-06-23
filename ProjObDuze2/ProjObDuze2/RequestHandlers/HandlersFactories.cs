//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using BigTask2.Data;
using BigTask2.ProblemSolvers;

namespace BigTask2.RequestHandlers
{
    /*
     * Użycie w chainie ".Then(Use.Database(...))" moim zdaniem
     * 1. wyglada lepiej i czytelniej niż ".Then(new Database())"
     * 2. a do tego unikamy użycia bezpośrednio new
     * 3. pozwala użyć moim zdaniem fajnej konwencji, że
     *    - obiekty z Use. przepuszczają request dalej lub zwracają poprawny wynik
     *    - obiekty z Catch. zatrzymują chain jeśli wystąpił błąd, lub 
     *      przepuszczają poprawne requesty dalej
     */
    
    static class Use
    {
        public static IRequestHandler RequestValidator()
        {
            return new RequestValidator();
        }

        public static IRequestHandler Database(IGraphDatabase db, VehicleType type)
        {
            return new DatabaseHandler(db, type);
        }

        public static IRequestHandler Resolver(IProblemResolver resolver)
        {
            return new ResolverHandler(resolver);
        }

        public static IRequestHandler Filter(ICityFilter filter)
        {
            return new UseFilter(filter);
        }

        public static IRequestHandler Solver(IGraphSolver graphSolver, string solver)
        {
            return new UseSolver(graphSolver, solver);
        }
    }

    static class Catch
    {
        public static IRequestHandler NullDatabase()
        {
            return new NoDatabaseHandler();
        }

        public static IRequestHandler UnresolvedProblem()
        {
            return new CatchOtherProblem();
        }
    }
}
