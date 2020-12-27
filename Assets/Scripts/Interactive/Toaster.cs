using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toaster : MonoBehaviour,IChargeable,INoPower
{
    public Animator toaster;


    

    public void Charging()
    {
        Debug.Log("涂面包");
        toaster.SetBool("IsCharge", true);
    }

    public void NoPower()
    {
        Debug.Log("面包没电");
        toaster.SetBool("IsCharge", false);
    }
}
