﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Wombat.Extensions.DataTypeExtensions
{
    public static partial class DataTypeExtensions
    {
        /// <summary>
        /// 复制序列中的数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="iEnumberable">原数据</param>
        /// <param name="startIndex">原数据开始复制的起始位置</param>
        /// <param name="length">需要复制的数据长度</param>
        /// <returns></returns>
        public static IEnumerable<T> Copy<T>(this IEnumerable<T> iEnumberable, int startIndex, int length)
        {
            var sourceArray = iEnumberable.ToArray();
            T[] newArray = new T[length];
            Array.Copy(sourceArray, startIndex, newArray, 0, length);

            return newArray;
        }

        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumberable, Action<T> func)
        {
            foreach (var item in iEnumberable)
            {
                func(item);
            }
        }

        /// <summary>
        /// 给 IEnumerable 拓展 ForEachAsync 方法
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumerable">数据源</param>
        /// <param name="func">异步方法</param>
        /// <returns>Task</returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> iEnumerable, Func<T, Task> func)
        {
            foreach (var item in iEnumerable)
            {
                await func(item);
            }
        }


        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumberable, Action<T, int> func)
        {
            var array = iEnumberable.ToArray();
            for (int i = 0; i < array.Count(); i++)
            {
                func(array[i], i);
            }
        }




        /// <summary>
        /// 给 IEnumerable 拓展 ForEachAsync 方法（顺序执行）
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumerable">数据源</param>
        /// <param name="func">异步方法</param>
        /// <returns>Task</returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> iEnumerable, Func<T, int, Task> func)
        {
            var array = iEnumerable.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                await func(array[i], i);
            }
        }





        /// <summary>
        /// IEnumerable转换为List'T'
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static List<T> CastToList<T>(this IEnumerable source)
        {
            return new List<T>(source.Cast<T>());
        }

        /// <summary>
        /// 将IEnumerable'T'转为对应的DataTable
        /// </summary>
        /// <typeparam name="T">数据模型</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> iEnumberable)
        {
            return iEnumberable.ToJson().ToDataTable();
        }


        /// <summary>
        /// 去重
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }


        public static IEnumerable<T> Slice<T>(this IEnumerable<T> source, int startIndex, int size)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            var enumerable = source as T[] ?? source.ToArray();
            int num = enumerable.Count();
            if (startIndex < 0 || num < startIndex)
                throw new ArgumentOutOfRangeException("startIndex");
            if (size < 0 || startIndex + size > num)
                throw new ArgumentOutOfRangeException("size");

            return enumerable.Skip(startIndex).Take(size);
        }



        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> model)
        {
            return new ObservableCollection<T>(model);
        }
    }
}
