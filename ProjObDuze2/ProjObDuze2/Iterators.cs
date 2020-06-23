//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System.Collections.Generic;

namespace BigTask2
{
    public interface IIterator<T>
    {
        T Current();
        bool MoveNext();
    }

    public class ListIterator<T> : IIterator<T>
    {
        private List<T> list;
        private int index;

        public ListIterator(List<T> list)
        {
            this.list = list;
            index = -1;
        }
        public T Current()
        {
            return list[index];
        }

        public bool MoveNext()
        {
            return ++index < list.Count;
        }
    }

    public class MergedIterator<T> : IIterator<T>
    {
        private IIterator<T>[] iterators;
        private int current = 0;

        public MergedIterator(IIterator<T>[] iterators)
        {
            this.iterators = iterators;
        }

        public T Current()
        {
            return iterators[current].Current();
        }

        public bool MoveNext()
        {
            while(current < iterators.Length)
            {
                if (iterators[current].MoveNext()) return true;
                current++;
            }
            return false;
        }
    }

    public class NullFilterIterator<T> : IIterator<T>
    {
        private IIterator<T> iterator;

        public NullFilterIterator(IIterator<T> iterator)
        {
            this.iterator = iterator;
        }

        public T Current()
        {
            return iterator.Current();
        }

        public bool MoveNext()
        {
            do
            {
                if (!iterator.MoveNext()) return false;
            } while (iterator.Current() == null);
            return true;
        }
    }
}
