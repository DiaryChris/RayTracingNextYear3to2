using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common;

public class GameManager : MonoSingleton<GameManager>
{

    /// <summary>
    /// （死亡时）重新加载
    /// </summary>
    public static void RestartScene()
    {
        //当前场景加载
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   
    }


    /// <summary>
    /// 胜利时
    /// </summary>
    public static void Victory()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        Debug.Log("Victory");
    }
}
