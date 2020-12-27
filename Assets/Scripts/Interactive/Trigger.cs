using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{

    public Transform trans;

    public Color highlightColor = Color.yellow;
    public float highlightOutlineWidth = 5.0f;
    public Color highlightOutlineColor = new Color(1, 1, 1);

    private Color prevColor;
    private float prevOutlineWidth;
    private Color prevOutlineColor;

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
        trans.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", highlightColor);

        prevOutlineWidth = trans.GetComponent<MeshRenderer>().material.GetFloat("_OutlineWidth");
        trans.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", highlightOutlineWidth);
        prevOutlineColor = trans.GetComponent<MeshRenderer>().material.GetColor("_OutlineColorVertex");
        trans.GetComponent<MeshRenderer>().material.SetColor("_OutlineColorVertex", highlightOutlineColor);

        trans.GetComponent<MeshRenderer>().material.SetInt("_UseRim", 1);
    }
    public virtual void HandleTriggerStayEvent(Collider other)
    {
   
    }

    public virtual void HandleTriggerExitEvent(Collider other)
    {
        trans.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", prevColor);
        trans.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", prevOutlineWidth);
        trans.GetComponent<MeshRenderer>().material.SetColor("_OutlineColorVertex", prevOutlineColor);
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
