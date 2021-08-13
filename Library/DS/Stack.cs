using System;
using System.Collections.Generic;

namespace Library
{
    public class Stack<T> : IEnumerable<T> where T : IComparable<T>
    {
        private ArrayList<T> _collection { get; set; }
        public int Count { get { return _collection.Count; } }

        public Stack() { 
            _collection = new ArrayList<T>();
        }


        public Stack(int initialCapacity)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            _collection = new ArrayList<T>(initialCapacity);
        }

        public bool IsEmpty
        {
            get
            {
                return _collection.IsEmpty;
            }
        }
        public T Top
        {
            get
            {
                try
                {
                    return _collection[_collection.Count - 1];
                }
                catch (Exception)
                {
                    throw new Exception("Stack is empty.");
                }
            }
        }
        public void Push(T dataItem)
        {
            _collection.Add(dataItem);
        }
        public T Pop()
        {
            if (Count > 0)
            {
                var top = Top;
                _collection.RemoveAt(_collection.Count - 1);
                return top;
            }

            throw new Exception("Stack is empty.");
        }

        public T[] ToArray()
        {
            return _collection.ToArray();
        }

        public string ToHumanReadable()
        {
            return _collection.ToHumanReadable();
        }


        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _collection.Count - 1; i >= 0; --i)
                yield return _collection[i];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }

}
