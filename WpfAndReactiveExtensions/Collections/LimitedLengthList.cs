using System;
using System.Linq;

namespace WpfAndReactiveExtensions.Collections
{
    public class LimitedLengthList<T>
    {
        private readonly T[] _array;
        private int _currentPosition;

        public LimitedLengthList(int count)
        {
            _array = new T[count];
        }

        public void Push(T item)
        {
            _array[_currentPosition] = item;
            _currentPosition = (_currentPosition + 1)%_array.Length;
        }

        public T this[int index] { get { return _array[(_currentPosition + index)%_array.Length]; } }
    }
}