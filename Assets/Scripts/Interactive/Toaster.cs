using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toaster : MonoBehaviour,IChargeable,INoPower
{
    public Animator toaster;


 


    public void Charging(Transform Ts)
    {
        Debug.Log("涂面包");
        toaster.SetBool("IsCharge", true);
        Ts.gameObject.SetActive(true);
    }

    public void NoPower(Transform Ts)
    {
        Debug.Log("面包没电");
        toaster.SetBool("IsCharge", false);
        Ts.gameObject.SetActive(false);
    }


}
