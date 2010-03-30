using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WpfAndReactiveExtensions.Collections
{
    public class LimitedLengthList<T> : IEnumerable<T>
    {
        private readonly T[] _list;
        private int currentPosition;

        public LimitedLengthList(IEnumerable<T> initialItems)
        {
            _list = initialItems.ToArray();
        }

        public LimitedLengthList(int count)
        {
            _list = Enumerable.Repeat(default(T), count).ToArray();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int index = 0; index < _list.Length; index++)
            {
                yield return _list[RealIndex(index)];
            }
        }

        private int RealIndex(int index)
        {
            int realIndex = index + currentPosition;
            return realIndex < _list.Length ? realIndex : realIndex - _list.Length;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Push(T item)
        {            
            _list[currentPosition] = item;
            ShiftPosition();
        }

        private void ShiftPosition()
        {
            currentPosition++;
            if(currentPosition >= _list.Length)
            {
                currentPosition = 0;
            }
        }
    }
}