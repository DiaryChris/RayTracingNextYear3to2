using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common;

public class GameManager : MonoSingleton<GameManager>
{

    public bool isVictory = false;



    public AudioClip BGM;
    public AudioClip VictoryBGM;
    public AudioSource BGMPlayer;


    /// <summary>
    /// （死亡时）重新加载
    /// </summary>
    public void RestartScene()
    {
        //当前场景加载
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   
    }


    /// <summary>
    /// 胜利时
    /// </summary>
    public void Victory()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        isVictory = true;
        SetBGM(VictoryBGM);
        if (!BGMPlayer.isPlaying)
        {
            PlayBGM();
        }
        Debug.Log("Victory");
    }


    #region audio


    public void SetBGM(AudioClip clip)
    {
        BGMPlayer.clip = clip;
    }

    //Audio Play
    public void PlayBGM()
    {
        //Debug.Log("BGM Play");
        BGMPlayer.Play();
    }

    public void PauseBGM()
    {
        BGMPlayer.Pause();
    }
    public void UnPauseBGM()
    {
        BGMPlayer.UnPause();
    }
    public void StopBGM()
    {
        BGMPlayer.Stop();
    }
    #endregion


    private void Start()
    {
        SetBGM(BGM);
        PlayBGM();
    }
}
