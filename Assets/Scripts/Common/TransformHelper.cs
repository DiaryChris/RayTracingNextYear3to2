using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Common {
    public static class TransformHelper {
        /// <summary>
        /// 未知层级查找子物体
        /// </summary>
        /// <param name="currentTF"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        /// 

            //扩展方法
        public static Transform FindCHildByName(this Transform currentTF, string childName)

        {
            Transform childTF = currentTF.Find(childName);

            if (childTF != null)
            {
                return childTF;
            }
            for (int i = 0; i < currentTF.childCount; i++)
            {
                childTF = FindCHildByName(currentTF.GetChild(i), childName);//
                if (childTF != null)
                {
                    return childTF;
                }
            }
            //返回null回溯
            return null;

        }

        /// <summary>
        /// 获取周围敌人
        /// </summary>
        /// <param name="currentTF"></param>
        /// <param name="distance"></param>
        /// <param name="angle"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static Transform[] CalculateAroundObject(this Transform currentTF, float distance, float angle, params string[] tags)
        {
            List<Transform> list = new List<Transform>();
            
            //根据标签获取游戏对象---变换组件
            for (int i = 0; i < tags.Length; i++)
            {

                GameObject[] tempGOArray = GameObject.FindGameObjectsWithTag(tags[i]);
                Transform[] tempTFArray = new Transform[tempGOArray.Length];
                for (int j = 0; j < tempGOArray.Length; j++)

                {
                    tempTFArray[j] = tempGOArray[j].transform;
                }
                list.AddRange(tempTFArray);
               
              }
            List<Transform> result = list.FindAll(item =>
               Vector3.Distance(item.position, currentTF.position) < distance &&
               Vector3.Angle(currentTF.forward, item.position - currentTF.position) < angle / 2);


            return result.ToArray();
        }

        /// <summary>
        /// 注视方向旋转
        /// </summary>
        /// <param name="currentTF"></param>
        /// <param name="targetDir"></param>
        /// <param name="rotateSpeed"></param>
        public static void LookDirection(this Transform currentTF, Vector3 targetDir, float rotateSpeed)
        {
            if (targetDir == Vector3.zero) return;
            Quaternion dir = Quaternion.LookRotation(targetDir);
            currentTF.rotation = Quaternion.Lerp(currentTF.rotation, dir, rotateSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 注视位置旋转
        /// </summary>
        /// <param name="currentTF"></param>
        /// <param name="targetPos"></param>
        /// <param name="rotateSpeed"></param>
        public static void LookPosition(this Transform currentTF, Vector3 targetPos, float rotateSpeed)
        {
            Vector3 targetDir = targetPos - currentTF.position;
            LookDirection(currentTF, targetDir, rotateSpeed);
        }
    }
}