using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallcube : MonoBehaviour
{

    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("IsFall", true);
        }
    }

   


}
