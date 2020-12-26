using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSys;
using System;
using UnityEngine.UI;

/// <summary>
/// 挂载给接受监听消息的物体
/// 发送信息: EventCenter.Broadcast(EventSys.EventType, Func);
/// </summary>
public class ShowText : MonoBehaviour
{
    /// <summary>
    /// 开始时注册监听
    /// 注：AddListener<需要和具体委托方法>
    /// </summary>
    private void Awake()
    {
        EventCenter.AddListener<string>(EventSys.EventType.ShowScore, Show);
  
    }
    /// <summary>
    /// 结束后移除监听
    /// 注：必须移除否则报错
    /// </summary>
    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventSys.EventType.ShowScore, Show);
    }

    /// <summary>
    /// 具体实现方法
    /// </summary>
    /// <param name="str"></param>
    private void Show(string str)
    {
        this.gameObject.GetComponent<Text>().text = str;
    }
}
