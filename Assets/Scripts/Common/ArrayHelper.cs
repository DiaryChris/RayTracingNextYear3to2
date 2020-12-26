using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    //静态类：工具类，占内存的，这样的类越少越好
    /*C#扩展方法：在已有的类代码的情况下，
     * 为其增加新的功能
     * 具体的需求：不修改微软的数组类，为他增加新的方法
     * 
     * 三要素：
     * 1.扩展方法所在的类（静态）
     * 2.在第一个参数上，使用this关键字修饰符被扩展类型
     * 3.在另一个命名空间下
     * 
     * 作用：让调用者，使用的时候更加方便
     * （好像在使用被扩展类型的自身的方法）
     * 语法：被扩展类型.扩展方法()；
     */
    /// <summary>
    /// 数组助手类：提供常用对数组操作的功能
    /// </summary>
    public static class ArrayHelper
    {
        //先明确被包装的方法原型
        //在定义相应的委托


        /// <summary>
        /// 查找所有满足条件的元素对象 【遍历数组的长度，获取元素
        /// </summary>
        /// <typeparam name="T"> 对象数组的元素类型</typeparam>
        /// <param name="nums">对象数组</param>
        /// <param name="Condition">查找条件</param>
        /// <returns></returns>
        public static T[] FindAll<T>(this T[] array, Func<T, bool> Condition)
        {
            //建立一个集合让数组的长度与集合的长度保持一致
            List<T> list = new List<T>(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                //判定一下，满足条件可以存到集合中的；
                if (Condition(array[i]))
                {
                    //给集合添加元素
                    list.Add(array[i]);
                }
            }
            //把集合变成数组返回数组
            return list.ToArray();
        }
        /// <summary>
        /// 查找数组对象中单个的元素
        /// </summary>
        /// 参数1数组，参数2 查找条件
        public static T Find<T>( this T[] array, Func<T, bool> Condition)
        {
            for (int i = 0; i < array.Length; i++)
            {
                //满足条件，返回相应的数组元素
                if (Condition(array[i]))
                {
                    return array[i];
                }
            }
            //泛型的默认值
            return default(T);
        }
        /// <summary>
        /// 获取满足条件的数组中的最大值
        /// </summary>
        /// <typeparam name="T">对象元素的类型</typeparam>
        /// <param name="array">随想的数组</param>
        /// <param name="Condition">获取条件</param>
        /// <returns></returns>
        public static T GetMax<T, TKey>(this T[] array, Func<T, TKey>
            Condition) where TKey : IComparable
        {
            //存最大值的临时变量
            T max = array[0];
            //遍历数组获取满足条件的组大致
            for (int i = 0; i < array.Length; i++)
            {
                //判断条件(CompareTo比较的方法[max 数组中元素的比较])
                if (Condition(max).CompareTo(Condition(array[i])) < 0)
                {
                    max = array[i];
                }
            }
            return max;
        }
        /// <summary>
        /// 获取数组的最小值
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <typeparam name="TKey">委托方法的返回值类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="Condition">比较的条件</param>
        /// <returns></returns>
        public static T GetMin<T, TKey>
            (this T[] array, Func<T, TKey> Condition) where TKey : IComparable
        {
            T min = array[0];
            for (int i = 0; i < array.Length; i++)
            {
                if (Condition(min).CompareTo(Condition(array[i])) > 0)
                {
                    min = array[i];
                }
            }
            return min;
        }
        /// <summary>
        /// 数组的升序排列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="array"></param>
        /// <param name="Condition"></param> （冒泡排序）
        public static void OrderBy<T, TKey>(this T[] array,
            Func<T, TKey> Condition) where TKey : IComparable
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    //判定条件
                    if (Condition(array[j]).CompareTo(Condition(array[j + 1])) > 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }
        /// <summary>
        /// 降序排列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="array"></param>
        /// <param name="Condition"></param>
        public static void OrderByDescending<T, TKey>
            (this T[] array, Func<T, TKey> Condition) where TKey : IComparable
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    if (Condition(array[j]).CompareTo(Condition(array[j + 1])) < 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }


        //筛选数组对象  Gameobject ==>Transform
        //有两个数组长度一致，一个是别人传进来，另一个与其长度一致
        //用来存储改变后的元素
        /// <summary>
        /// 筛选对象
        /// </summary>
        /// <typeparam name="T">筛选数组的类型</typeparam>
        /// <typeparam name="TKey">筛选后的类型</typeparam>
        /// <param name="array">对象数组</param>
        /// <param name="Condition">筛选策略</param>
        public static TKey[] Select<T, TKey>(this T[] array,
            Func<T, TKey> Condition)
        {
            TKey[] keys = new TKey[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                keys[i] = Condition(array[i]);
            }
            return keys;
        }





        // 简单介绍 了解
        /* 接口的关键字 Interface
         * 接口是一种规范，
         * 接口内的方法没有方法体，继承接口的子类必须实现接口中的方法
         * 接口内的内容不用写访问修饰符 默认为Public
         */


    }
}
