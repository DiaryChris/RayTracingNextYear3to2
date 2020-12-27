using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{

    public Transform trans;
    
    private Color prevColor;
    private float prevOutlineWidth;

    private void Start()
    {
        if(trans == null)
        {
            trans = transform.parent;
        }
    }

    public virtual void HandleTriggerEnterEvent(Collider other)
    {
        //prevColor = trans.GetComponent<MeshRenderer>().material.GetColor("_BaseColor");
        //trans.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color32(225, 180, 64, 255));

        prevOutlineWidth = trans.GetComponent<MeshRenderer>().material.GetFloat("_OutlineWidth");
        trans.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 1.0f);
        trans.GetComponent<MeshRenderer>().material.SetInt("_UseRim", 1);
    }
    public virtual void HandleTriggerStayEvent(Collider other)
    {
   
    }

    public virtual void HandleTriggerExitEvent(Collider other)
    {
        //trans.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", prevColor);
        trans.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", prevOutlineWidth);
        trans.GetComponent<MeshRenderer>().material.SetInt("_UseRim", 0);

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
