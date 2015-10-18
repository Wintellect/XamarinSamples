using System;
using System.Collections.Generic;
using System.Linq;

namespace PropertyDependencyDemo.Mvvm
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Performs the specified action on each element of the collection.
        /// </summary>
        /// <param name="list">A collection of elements.</param>
        /// <param name="action">The <see cref="T:System.Action`1"/> delegate to perform on each element of the collection.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (list == null)
                throw new ArgumentNullException("list");
            foreach (var item in list.ToList())
                action(item);
        }

        /// <summary>
        /// Performs a full outer join of two collections that share a common key
        /// </summary>
        /// <typeparam name="TA">Element Type of first collection</typeparam>
        /// <typeparam name="TB">Element Type of second collection</typeparam>
        /// <typeparam name="TK">Common Key Type</typeparam>
        /// <typeparam name="TR">Resulting collection Type</typeparam>
        /// <param name="a">First collection</param>
        /// <param name="b">Second collection</param>
        /// <param name="selectKeyA">Key selector for first collection</param>
        /// <param name="selectKeyB">Key selector for second collection</param>
        /// <param name="projection">Results selector</param>
        /// <param name="defaultA">Default value used when the first collection does not have a joining key</param>
        /// <param name="defaultB">Default value used when the second collection does not have a joining key</param>
        /// <param name="cmp">Custom Key comparer</param>
        /// <returns></returns>
        public static IList<TR> FullOuterJoin<TA, TB, TK, TR>(
            this IEnumerable<TA> a,
            IEnumerable<TB> b,
            Func<TA, TK> selectKeyA,
            Func<TB, TK> selectKeyB,
            Func<TA, TB, TK, TR> projection,
            TA defaultA = default(TA),
            TB defaultB = default(TB),
            IEqualityComparer<TK> cmp = null)
        {
            cmp = cmp ?? EqualityComparer<TK>.Default;
            var alookup = a.ToLookup(selectKeyA, cmp);
            var blookup = b.ToLookup(selectKeyB, cmp);

            var keys = new HashSet<TK>(alookup.Select(p => p.Key), cmp);
            keys.UnionWith(blookup.Select(p => p.Key));

            var join = from key in keys
                from xa in alookup[key].DefaultIfEmpty(defaultA)
                from xb in blookup[key].DefaultIfEmpty(defaultB)
                select projection(xa, xb, key);

            return join.ToList();
        }
    }
}

