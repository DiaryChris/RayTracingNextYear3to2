using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Trigger
{
    public GameObject InterCube;

    public bool canpush;

    private void Update()
    {
        if (canpush)
        {
            InterCube.GetComponent<Rigidbody>().mass = 1;
        }
        else
        { InterCube.GetComponent<Rigidbody>().mass = 100; }
    }


    public override void HandleTriggerStayEvent(Collider other)
    {
        base.HandleTriggerStayEvent(other);

        if (other.gameObject.GetComponent<PlayerMovement>().pState == PlayerState.Pushing)
        {
            canpush = true;
        }
        else
        {
            canpush = false;
        }
        
        // Add new interactive function with box here
    }


    public override void HandleTriggerExitEvent(Collider other)
    {
        base.HandleTriggerExitEvent(other);

        canpush = false;

        //InterCube.GetComponent<Rigidbody>().isKinematic = true;
        // Add new interactive function with box here
    }
}
