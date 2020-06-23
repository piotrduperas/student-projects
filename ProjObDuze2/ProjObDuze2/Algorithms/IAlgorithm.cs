//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using BigTask2.Api;
using BigTask2.Data;
using System.Collections.Generic;

namespace BigTask2.Algorithms
{
	public interface IAlgorithm
	{
		public IEnumerable<Route> Solve(IGraphDatabase graph, City from, City to);
	}
}
