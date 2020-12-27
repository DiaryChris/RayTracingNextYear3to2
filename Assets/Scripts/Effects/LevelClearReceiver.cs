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

    public bool playAnimation;
    public Animation sourceAnimation;


    private MeshRenderer meshRenderer;
    private Animator animator;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
    }

    public void OnLevelClear()
    {
        // Change material to light one
        if (changeMaterial && meshRenderer !=null)
        {
            meshRenderer.material = lightMaterial;
        }

        if (enableObject && targetObject!= null)
        {
            targetObject.SetActive(true);
            // play animation here
        }

        if(playAnimation && animator != null)
        {
            animator.Play(sourceAnimation.name);
        }

    }




}
