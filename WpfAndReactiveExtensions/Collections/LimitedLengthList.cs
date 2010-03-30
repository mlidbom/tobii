using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WpfAndReactiveExtensions.Collections
{
    public class LimitedLengthList<T> : IEnumerable<T>
    {
        private readonly T[] _list;
        private int _currentPosition;

        public LimitedLengthList(int count)
        {
            _list = Enumerable.Repeat(default(T), count).ToArray();
        }


        private int RealIndex(int index)
        {
            if (index >= _list.Length)
            {
                throw new IndexOutOfRangeException();
            }
            int realIndex = index + _currentPosition;
            return realIndex < _list.Length ? realIndex : realIndex - _list.Length;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int index = 0; index < _list.Length; index++)
            {
                yield return this[index];
            }
        }

        public void Push(T item)
        {
            _list[_currentPosition] = item;
            ShiftPosition();
        }

        public T this[int index] { get { return _list[RealIndex(index)]; } }

        private void ShiftPosition()
        {
            ++_currentPosition;
            if (_currentPosition >= _list.Length)
            {
                _currentPosition = 0;
            }
        }
    }
}