using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Trigger
{
    // Start is called before the first frame update
    public Animator vicAnim;

    public override void HandleTriggerStayEvent(Collider other)
    {
        base.HandleTriggerStayEvent(other);

        vicAnim.SetBool("isVictory", true);

        GameManager.instance.Victory();

    }

}
