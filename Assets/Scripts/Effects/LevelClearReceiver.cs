using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelClearReceiver : MonoBehaviour
{
    public bool changeMaterial;
    public Material darkMaterial;
    public Material lightMaterial;

    public bool playAnimation;

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

        if (playAnimation)
        {
            gameObject.SetActive(true);
            // play animation here
        }
    }




}
