using System.Collections.Generic;

namespace WpfAndReactiveExtensions.Linq
{
    public static class Sequences
    {
        public static IEnumerable<T> RemoveConsecutiveDuplicates<T>(this IEnumerable<T> me)
        {
            using (var enumerator = me.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    var lastValue = enumerator.Current;
                    yield return lastValue;
                    while (enumerator.MoveNext())
                    {
                        if (!lastValue.Equals(enumerator.Current))
                        {
                            yield return lastValue = enumerator.Current;
                        }
                    }
                }
            }
        }
    }
}