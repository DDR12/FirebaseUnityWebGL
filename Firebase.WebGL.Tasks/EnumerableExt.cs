using System;
using System.Collections.Generic;


/// <summary>
/// General extensions to LINQ.
/// </summary>
public static class EnumerableExt
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="fn"></param>
    public static void Each<T>(this IEnumerable<T> source, Action<T, int> fn)
    {
        int index = 0;

        foreach (T item in source)
        {
            fn.Invoke(item, index);
            index++;
        }
    }
}