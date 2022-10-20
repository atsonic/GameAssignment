using System;
using System.Collections.ObjectModel;

namespace Utils
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// 配列内の要素を複数キーでソートします
        /// </summary>
        public static void Sort<TSource, TResult>(
            this TSource[] array,
            Func<TSource, TResult> selector1,
            Func<TSource, TResult> selector2) where TResult : IComparable
        {
            Array.Sort(array, (x, y) =>
            {
                var result = selector1(x).CompareTo(selector1(y));
                return result != 0 ? result : selector2(x).CompareTo(selector2(y));
            });
        }
    }
}
