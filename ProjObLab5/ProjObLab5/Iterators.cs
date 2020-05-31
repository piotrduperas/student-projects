//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;

namespace TravelAgencies.DataAccess
{
    public interface IIterable<T>
    {
        IIterator<T> GetIterator();
    }

    public interface IIterator<T>
    {
        public T Current();
        public bool Move();
        public void Reset();
    }

    public abstract class IteratorDecorator<U, T> : IIterator<T>
    {
        protected IIterator<U> it;
        public IteratorDecorator(IIterator<U> it)
        {
            this.it = it;
        }
        public abstract T Current();
        public virtual bool Move()
        {
            return it.Move();
        }
        public virtual void Reset()
        {
            it.Reset();
        }
    }


    public class ArrayIterator<T> : IIterator<T>
    {
        private int index;
        private T[] array;

        public ArrayIterator(T[] array)
        {
            this.array = array;
            Reset();
        }

        public T Current()
        {
            return array[index];
        }

        public bool Move()
        {
            if (++index >= array.Length) return false;
            return true;
        }

        public void Reset()
        {
            index = -1;
        }
    }

    public interface IBST<T>
    {
        IBST<T> Left { get; }
        IBST<T> Right { get; }
        T Data { get; }
    }

    public class BSTInOrderIterator<T> : IIterator<T>
    {
        IBST<T> root;
        int state;
        BSTInOrderIterator<T> it;

        public BSTInOrderIterator(IBST<T> bst)
        {
            root = bst;
            Reset();
        }

        public T Current()
        {
            if(state == 2)
            {
                return root.Data;
            }
            return it.Current();
        }

        public bool Move()
        {
            if (state == 0)
            {
                if(root.Left != null)
                {
                    state = 1;
                    it = new BSTInOrderIterator<T>(root.Left);
                }
                else
                {
                    state = 2;
                    return true;
                }
            }
            if(state == 1)
            {
                if (!it.Move())
                {
                    state = 2;
                    return true;
                }
            }
            if(state == 2)
            {
                if (root.Right != null)
                {
                    state = 3;
                    it = new BSTInOrderIterator<T>(root.Right);
                }
                else
                {
                    return false;
                }
            }
            if(state == 3)
            {
                if (!it.Move())
                {
                    state = 4;
                }
            }
            if(state == 4)
            {
                return false;
            }

            return true;
        }

        public void Reset()
        {
            it = null;
            state = 0;
        }
    }

    public interface IListNode<T>
    {
        IListNode<T> Next { get; }
        T Data { get; }
    }

    public class ListIterator<T> : IIterator<T>
    {
        private IListNode<T> firstNode;
        private IListNode<T> currentNode;

        public ListIterator(IListNode<T> first)
        {
            firstNode = first;
            Reset();
        }

        public T Current()
        {
            return currentNode.Data;
        }

        public bool Move()
        {
            if (currentNode != null)
            {
                if (currentNode.Next != null)
                {
                    currentNode = currentNode.Next;
                    return true;
                }
                return false;
            }

            currentNode = firstNode;

            return currentNode != null;
        }

        public void Reset()
        {
            currentNode = null;
        }
    }

    public class CompositeIterator<T> : IIterator<T>
    {
        private IIterator<IIterator<T>> it;
        private bool canMoveChild;
        public CompositeIterator(IIterator<IIterator<T>> it)
        {
            this.it = it;
            Reset();
        }

        public T Current()
        {
            return it.Current().Current();
        }

        public bool Move()
        {
            while (it.Move())
            {
                canMoveChild = it.Current().Move();
                if (!canMoveChild)
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        public void Reset()
        {
            canMoveChild = false;
            it.Reset();
            while (it.Move())
            {
                it.Current().Reset();
            }
            it.Reset();
        }
    }

    public class DecryptingIterator<T> : IteratorDecorator<T, T>
    {
        protected IEncryptor<T> enc;
        public DecryptingIterator(IIterator<T> it, IEncryptor<T> enc) : base(it)
        {
            this.enc = enc;
        }

        public override T Current()
        {
            return enc.Decrypt(it.Current());
        }

        public override bool Move()
        {
            while (it.Move())
            {
                if (enc.Decrypt(it.Current()) != null) return true;
            }
            return false;
        }
    }

    public class InfiniteIterator<T> : IteratorDecorator<T, T>
    {
        public InfiniteIterator(IIterator<T> it) : base(it)
        {
            it.Reset();
            if (!it.Move()) throw new ArgumentException("Passed iterator is empty");
            it.Reset();
        }

        public override T Current()
        {
            return it.Current();
        }

        public override bool Move()
        {
            if (!it.Move())
            {
                it.Reset();
                it.Move();
            }
            return true;
        }
    }
}
