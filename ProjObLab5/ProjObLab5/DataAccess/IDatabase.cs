//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
namespace TravelAgencies.DataAccess
{
    public interface IDatabase<T> : IIterable<T>, IDecryptable<T>
    {

    }
}
