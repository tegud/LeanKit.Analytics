using System;
using System.Collections.Generic;

namespace LeanKit.Utilities.Collections
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> FlattenHierarchy<T>(this T node, Func<T, IEnumerable<T>> getChildEnumerator)
        {
            yield return node;
            if (getChildEnumerator(node) != null)
            {
                foreach (var child in getChildEnumerator(node))
                {
                    foreach (var childOrDescendant in child.FlattenHierarchy(getChildEnumerator))
                    {
                        yield return childOrDescendant;
                    }
                }
            }
        }

        public static IEnumerable<T1> SelectWithNext<T, T1>(this IEnumerable<T> enumerable, Func<T, T, T1> selectAction)
        {
            using (var iterator = enumerable.GetEnumerator())
            {
                iterator.MoveNext();

                var current = iterator.Current;

                if(!iterator.MoveNext())
                {
                    yield return selectAction(current, default(T));
                    yield break;
                }

                do
                {
                    var next = iterator.Current;

                    yield return selectAction(current, next);

                    current = next;
                } while (iterator.MoveNext());

                yield return selectAction(current, default(T));
            }
        }
    }
}