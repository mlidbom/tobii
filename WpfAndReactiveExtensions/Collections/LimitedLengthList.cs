using System;
using System.Linq;

namespace WpfAndReactiveExtensions.Collections
{
    public class LimitedLengthList<T>
    {
        private readonly T[] _list;
        private int _currentPosition;

        public LimitedLengthList(int count)
        {
            _list = Enumerable.Repeat(default(T), count).ToArray();
        }

        public void Push(T item)
        {
            _list[_currentPosition] = item;
            _currentPosition = (_currentPosition + 1)%_list.Length;
        }

        public T this[int index] { get { return _list[(_currentPosition + index)%_list.Length]; } }
    }
}