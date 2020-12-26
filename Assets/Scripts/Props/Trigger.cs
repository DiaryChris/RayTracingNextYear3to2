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

    public virtual void HandleTriggerEnterEvent()
    {
        prevColor = trans.GetComponent<MeshRenderer>().material.GetColor("_BaseColor");
        trans.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.yellow);
    }


    public virtual void HandleTriggerExitEvent()
    {
        trans.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", prevColor);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        HandleTriggerEnterEvent();

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        HandleTriggerExitEvent();
    }
}
