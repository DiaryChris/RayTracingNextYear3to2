using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 单例模板
/// </summary>
/// <typeparam name="T">子类类型</typeparam>

//约束：T必须是子类
namespace Common
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {

        // Use this for initialization
        //public static T Instance { get; private set; }
        public static T instance;
        public static T Instance
        {
            get
            {
                //在Awake时也可寻找
                if (instance == null)
                {
                    //在场景中查找指定类型引用
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {   //场景没有就自己创建一个       typeof:返回类型信息
                        new GameObject("Singleton of " + typeof(T)).AddComponent<T>();
                        //立即执行Awake
                    }
                }

                return instance;
            }
        }

        protected void Awake()
        {
            if (instance == null)
            {
                instance = this as T;


            }


        }
    }
}
