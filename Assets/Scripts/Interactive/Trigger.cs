using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{

    public Transform trans;
    
    private Color prevColor;

    private void Start()
    {
        if(trans == null)
        {
            trans = transform.parent;
        }
    }

    public virtual void HandleTriggerEnterEvent(Collider other)
    {
        prevColor = trans.GetComponent<MeshRenderer>().material.GetColor("_BaseColor");
        trans.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.yellow);
    }
    public virtual void HandleTriggerStayEvent(Collider other)
    {
   
    }

    public virtual void HandleTriggerExitEvent(Collider other)
    {
        trans.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", prevColor);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        HandleTriggerEnterEvent(other);

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        HandleTriggerExitEvent(other);
    }


    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        HandleTriggerStayEvent(other);
    }
}
