using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 委托：函数指针
/// 监听中心
/// 利：与外界没有联系，不容易出现连带错误
/// 弊：参数类型顺序，数量必须一致，广播事件码必须得知
/// </summary>

namespace EventSys
{
    public class EventCenter
    {
        //存放事件码               事件码     对应委托
        private static Dictionary<EventType, Delegate> m_EventTable = new Dictionary<EventType, Delegate>();

        /// <summary>
        /// 创建监听起始，将列表中没有的事件类型加入
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        private static void OnListenerAdding(EventType eventType, Delegate callBack)
        {
            //若不包含事件码，添加一个
            if (!m_EventTable.ContainsKey(eventType))
            {
                //暂时不添加回调，之后将按需添加
                m_EventTable.Add(eventType, null);
            }

            Delegate d = m_EventTable[eventType];
            //如果委托对应参数不一致，报错
            if (d != null && d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1}，要添加的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }

        /// <summary>
        /// 移除监听起始，使用完毕后移除
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        private static void OnListenerRemoving(EventType eventType, Delegate callBack)
        {
            //存在Key才可移除
            if (m_EventTable.ContainsKey(eventType))
            {
                Delegate d = m_EventTable[eventType];
               //若事件为空
                if (d == null)
                {
                    throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
                }
                //若类型不一致
                else if (d.GetType() != callBack.GetType())
                {
                    throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
                }
            }
            else
            {
                throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
            }
        }
        /// <summary>
        /// 移除监听后续，清理占用内存的空列表项
        /// </summary>
        /// <param name="eventType"></param>
        private static void OnListenerRemoved(EventType eventType)
        {
            if (m_EventTable[eventType] == null)
            {
                m_EventTable.Remove(eventType);
            }
        }



        #region 添加监听 0-5参数重载
        //no parameters
        /// <summary>
        /// 添加无参监听
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        public static void AddListener(EventType eventType, CallBack callBack)
        {
            OnListenerAdding(eventType, callBack);
            //将Delegate强转为CallBack放入列表
            m_EventTable[eventType] = (CallBack)m_EventTable[eventType] + callBack;
        }
        /// <summary>
        /// 添加一参监听
        /// 注：CallBack<T>参数数量
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        public static void AddListener<T>(EventType eventType, CallBack<T> callBack)
        {
            OnListenerAdding(eventType, callBack);
            //将Delegate强转为CallBack放入列表
            m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] + callBack;
        }
        /// <summary>
        /// 添加二参监听
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        public static void AddListener<T, X>(EventType eventType, CallBack<T, X> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X>)m_EventTable[eventType] + callBack;
        }
        /// <summary>
        /// 添加三参监听
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        public static void AddListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y>)m_EventTable[eventType] + callBack;
        }
        /// <summary>
        /// 添加四参监听
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        public static void AddListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z>)m_EventTable[eventType] + callBack;
        }
        /// <summary>
        /// 添加五参监听
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        public static void AddListener<T, X, Y, Z, W>(EventType eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z, W>)m_EventTable[eventType] + callBack;
        }
        #endregion

        #region 移除监听 0-5参数重载
        /// <summary>
        /// 移除无参监听
        /// 注：至少在OnDestroy方法中调用
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        public static void RemoveListener(EventType eventType, CallBack callBack)
        {

            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }


        //single parameters
        /// <summary>
        /// 移除一参监听
        /// 注：至少在OnDestroy方法中调用
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }
        //two parameters
        /// <summary>
        /// 移除二参监听
        /// 注：至少在OnDestroy方法中调用
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        public static void RemoveListener<T, X>(EventType eventType, CallBack<T, X> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }
        //three parameters
        /// <summary>
        /// 移除三参监听
        /// 注：至少在OnDestroy方法中调用
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        public static void RemoveListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }
        //four parameters
        /// <summary>
        /// 移除四参监听
        /// 注：至少在OnDestroy方法中调用
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        public static void RemoveListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }
        //five parameters
        /// <summary>
        /// 移除五参监听
        /// 注：至少在OnDestroy方法中调用
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="callBack">具体回调委托</param>
        public static void RemoveListener<T, X, Y, Z, W>(EventType eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z, W>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }
        #endregion


        #region 广播事件 0-5参数重载
        //no parameters
        public static void Broadcast(EventType eventType)
        {
            Delegate d;
            //TryGetValue：查找不确定是否存在的Key时使用
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                //获取后强转赋值
                CallBack callBack = d as CallBack;
                //非空则直接调用
                if (callBack != null)
                {
                    callBack();
                }
                else
                {
                    //强转失败,委托参数不对
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        //single parameters
        public static void Broadcast<T>(EventType eventType, T arg)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T> callBack = d as CallBack<T>;
                if (callBack != null)
                {
                    callBack(arg);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        //two parameters
        public static void Broadcast<T, X>(EventType eventType, T arg1, X arg2)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X> callBack = d as CallBack<T, X>;
                if (callBack != null)
                {
                    callBack(arg1, arg2);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        //three parameters
        public static void Broadcast<T, X, Y>(EventType eventType, T arg1, X arg2, Y arg3)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Y> callBack = d as CallBack<T, X, Y>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        //four parameters
        public static void Broadcast<T, X, Y, Z>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Y, Z> callBack = d as CallBack<T, X, Y, Z>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        //five parameters
        public static void Broadcast<T, X, Y, Z, W>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Y, Z, W> callBack = d as CallBack<T, X, Y, Z, W>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4, arg5);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        #endregion

    }
}
