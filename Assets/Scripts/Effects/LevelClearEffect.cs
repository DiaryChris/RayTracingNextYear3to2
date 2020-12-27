using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClearEffect : MonoBehaviour
{
    public float emitSpeed = 100f;

    private SphereCollider trigger;
    private bool isEmitting;

    private void Start()
    {
        trigger = GetComponent<SphereCollider>();
        trigger.radius = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q Effect");
            isEmitting = true;
        }
        if (isEmitting)
        {
            EmitEffect(Time.deltaTime);
        }
    }

    private void EmitEffect(float dt)
    {
        trigger.radius += emitSpeed * dt;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("new trigger enter, name:" + other.gameObject.name);

        MeshRenderer targetMesh = other.GetComponent<MeshRenderer>();
        if (targetMesh)
        {
            targetMesh.material.SetColor("_BaseColor", Color.black);
        }
    }

}
