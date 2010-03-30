using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WpfAndReactiveExtensions.Collections
{
    public class LimitedLengthList<T> : IEnumerable<T>
    {
        private readonly LinkedList<T> _list;

        public LimitedLengthList(IEnumerable<T> initialItems)
        {
            _list = new LinkedList<T>(initialItems);
        }

        public LimitedLengthList(int count)
        {
            _list = new LinkedList<T>(Enumerable.Repeat(default(T), count));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Push(T item)
        {
            _list.RemoveFirst();
            _list.AddLast(item);
            return;
        }
    }
}