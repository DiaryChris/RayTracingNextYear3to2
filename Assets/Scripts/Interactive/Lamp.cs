using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour,IChargeable,INoPower
{

    public GameObject light;

    

    public void Charging(Transform Ts)
    {
        Debug.Log("亮灯");
        light.SetActive(true);
    }

    public void NoPower(Transform Ts)
    {
        Debug.Log("关灯");
        light.SetActive(false);
    }
}
