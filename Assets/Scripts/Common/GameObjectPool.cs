using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
核心思想：空间 换 时间 
               内存     CPU
优点：减少创建/销毁过程。
缺点：占用内存空间过多。
适用性：频繁 创建与销毁物体。
*/
namespace Common
{
       public interface IResetable
    {
        void OnReset();

    }
    /// <summary>
    /// 游戏对象池
    /// </summary>
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        //key 类别（子弹、水果、敌人……） value:某一类别的所有对象
        private Dictionary<string, List<GameObject>> cache;

        private new void Awake()
        {
            base.Awake();
            cache = new Dictionary<string, List<GameObject>>();
        }

        //1. 通过对象池创建对象的功能
        public GameObject CreateObject(string key, GameObject prefab, Vector3 position, Quaternion rotate)
        {
            //******************方法******************************
            // --- 在池中查找可以使用的对象(被禁用的对象)
            GameObject go = FindUsableObject(key);
            //   if(没找到)    创建对象、存入池中
            if (go == null)
                go = AddObject(key, prefab);
            //   使用对象（设置位置、设置旋转、激活） 
            UseObject(position, rotate, go);
            
            return go;
        }

        private  void UseObject(Vector3 position, Quaternion rotate, GameObject go)
        {
            go.transform.position = position;
            go.transform.rotation = rotate;
            go.SetActive(true);
            //一个物体可能有多个脚本需要Reset
            foreach (var item in go.GetComponents<IResetable>())
            {
                item.OnReset();
            } 
        }

        private GameObject AddObject(string key, GameObject prefab)
        {
            GameObject go = Instantiate(prefab);
            if (!cache.ContainsKey(key))
            {
                cache.Add(key, new List<GameObject>());
            }
            cache[key].Add(go);
            return go;
        }

        private GameObject FindUsableObject(string key)
        {
         
            if (cache.ContainsKey(key))
            {
                return  cache[key].Find(g => g.activeInHierarchy == false);
            }
            return null;
       
        }

        //2. 回收
        public void CollectObject(GameObject go, float delay = 0)
        {
            if (delay == 0)
                go.SetActive(false);
            else
                StartCoroutine(DelayCollectObject(go, delay));
        }


        //   延迟回收
        private IEnumerator DelayCollectObject(GameObject go, float delay)
        {
            // --- 禁用  
            yield return new WaitForSeconds(delay);
            go.SetActive(false);
        }

        //3. 清空
        //  销毁存储的游戏对象 Destroy( 游戏对象  )     
        //  移除字典中的记录    Remove(键)
        public void Clear(string key)
        {
            for (int i = 0; i < cache[key].Count; i++)
            {
                Destroy(cache[key][i]);
            }
            cache.Remove(key);
        }

        public void ClearAll()
        {
            //将所有键存入List（foreach只读）
            List<string> keyList = new List<string>(cache.Keys);
            //遍历List移除字典
            foreach (var key in keyList)
            {
                Clear(key);
            }
            //foreach-->List-->cache
        }
    }
}