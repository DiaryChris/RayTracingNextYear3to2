using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*

	*/

namespace Common
{
    ///<summary>
    ///资源管理器
    ///</summary>
    public class ResourceManager : MonoBehaviour
    {
        private static Dictionary<string, string> configMap;
        //静态构造函数执行时机：类被加载时
        static ResourceManager()
        {
            configMap = new Dictionary<string, string>();
            string configContent = GetConfigFile("ResMap.txt");
            BuildConfigMap(configContent);
        }
        //解析
        private static void BuildConfigMap(string content)
        {
            //提供仅向前的读取方法
            StringReader reader = new StringReader(content);

            string line;
            //每次先读取第一行，然后判断
            while ((line = reader.ReadLine()) != null)
            {
                string[] keyValue = line.Split('=');
                configMap.Add(keyValue[0], keyValue[1]);

            }

        }

        private static string GetConfigFile(string fileName)
        {
            //StreamingAssets
            //pc    StreamingAssets
            //Android  !/assets/
            //ios       /Raw/

            string path = Application.streamingAssetsPath + "/" + fileName;
            //File只能在PC上读取
            if (Application.platform != RuntimePlatform.Android)
            {
                path = "file://" + path;
            }
            //using:使用完自动释放=www.Dispose();
            using (WWW www = new WWW(path))
            {
                while (true)
                {      //有可能一次读不完
                    if (www.isDone)
                    {
                        return www.text;
                    }
                }

            }

        }
        private static string GetValue(string resourceName)
        {
            return configMap.ContainsKey(resourceName) ? configMap[resourceName] : null;

        }

        public static T Load<T>(string resourceName) where T : Object
        {
            //配置文件
            //根据资源名称查找路径
            string path = configMap[resourceName];
            //通过Resources加载资源
            return Resources.Load<T>(path);
        }



    }
}
