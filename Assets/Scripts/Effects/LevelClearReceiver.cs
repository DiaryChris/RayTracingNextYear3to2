using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelClearReceiver : MonoBehaviour
{
    public bool changeMaterial;
    public Material darkMaterial;
    public Material lightMaterial;

    public bool enableObject;
    public GameObject targetObject;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnLevelClear()
    {
        // Change material to light one
        if (changeMaterial)
        {
            meshRenderer.material = lightMaterial;
        }

        if (enableObject && targetObject!= null)
        {
            targetObject.SetActive(true);
            // play animation here
        }
    }




}
